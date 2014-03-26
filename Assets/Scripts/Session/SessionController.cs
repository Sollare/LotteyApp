using System;
using LotteyServerApp.Models;
using UnityEngine;
using System.Collections;

public class SessionController : MonoBehaviour
{
    public delegate void SessionDelegate(User user);

    public event SessionDelegate SessionStarted;
    public event SessionDelegate SessionEnded;

    public static bool isAuthorized;
    public static bool isAttemptingAuthorization;

    protected virtual void OnSessionStarted(User user)
    {
        Debug.Log(">>> СЕССИЯ НАЧАТА <<<");
        isAuthorized = true;

        SessionDelegate handler = SessionStarted;
        if (handler != null) handler(user);
    }
    protected virtual void OnSessionEnded(User user)
    {
        isAuthorized = false;

        SessionDelegate handler = SessionEnded;
        if (handler != null) handler(user);
    }

    private static SessionController _instance;
    public static SessionController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("WWW").GetComponent<SessionController>();

            return _instance;
        }
    }

    private User _currentUser;
    public User currentUser
    {
        get
        {
            return _currentUser;
        }
    }

    public void SignIn(string username, string password)
    {
        StopCoroutine("AuthorizationAttempts");

        isAttemptingAuthorization = true;
        StartCoroutine(AuthorizationAttempts(username, password));
    }

    public void SignOut()
    {
        if (_currentUser != null)
        {
            _currentUser = null;
            SessionEnded(null);
        }
    }

    public void SignUp(string username, string email, string password)
    {
        string authorizationString = string.Format("{0}/BetaApi/Register.aspx?name={1}&email={2}&password={3}", WWWOperations.instance.ServerUrl, username, email, password);
        WWWOperations.instance.FetchJsonObject<ResponseMessage>(authorizationString, SignUpResult);
    }
    public void UpdateUserData(int userId)
    {
        //Debug.Log(">> Запрос информации о пользователе");
        string userInfoString = string.Format("{0}/UserAPI?id={1}", WWWOperations.instance.ServerUrl, userId);
       
        WWWOperations.instance.FetchJsonObject<User>(userInfoString, UserInfoUpdateCallback);
    }


    #region CallbackHandlers

    private void AuthorizationResult(ResponseMessage fetchedObject, string error)
    {
        if (fetchedObject == null)
        {
            Debug.LogError("Получен неизвестный объект: " + error);
            return;
        }

        if (fetchedObject.Code == 0)
        {
            Debug.Log("Ошибка авторизации: " + fetchedObject.Message);
        }
        else
        {
            int userId = int.Parse(fetchedObject.Message);
            Debug.Log("Успешно авторизованы");

            UpdateUserData(userId);
        }
    }


    IEnumerator AuthorizationAttempts(string username, string password)
    {
        int counter = 0;
        while (counter++ < 3 && !isAuthorized)
        {
            string authorizationString = string.Format("{0}/BetaApi/Login.aspx?name={1}&password={2}", WWWOperations.instance.ServerUrl, username, password);
            WWWOperations.instance.FetchJsonObject<ResponseMessage>(authorizationString, AuthorizationResult);

            yield return new WaitForSeconds(4f);
        }

        isAttemptingAuthorization = false;
    }

    private void SignUpResult(ResponseMessage fetchedObject, string error)
    {
        if (fetchedObject == null)
        {
            Debug.LogError("Получен неизвестный объект");
            return;
        }

        if (fetchedObject.Code == 0)
        {
            Debug.Log("Ошибка регистрации: " + fetchedObject.Message);
        }
        else
        {
            Debug.Log(fetchedObject.Message);
        }
    }
    
    private void UserInfoUpdateCallback(User fetchedObject, string error)
    {
        if (fetchedObject == null)
        {
            Debug.LogError("Получен неизвестный объект");
            return;
        }

        _currentUser = fetchedObject;
        Debug.Log(fetchedObject.ToString());

        OnSessionStarted(_currentUser);
    }

    #endregion
}
