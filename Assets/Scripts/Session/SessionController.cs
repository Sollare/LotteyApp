using System;
using LotteyServerApp.Models;
using UnityEngine;
using System.Collections;

public class SessionController : MonoBehaviour
{
    public delegate void SessionDelegate(User user);

    public event SessionDelegate SessionStarted;
    public event SessionDelegate SessionEnded;

    protected virtual void OnSessionStarted(User user)
    {
        SessionDelegate handler = SessionStarted;
        if (handler != null) handler(user);
    }
    protected virtual void OnSessionEnded(User user)
    {
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
        string authorizationString = string.Format("{0}/BetaApi/Login.aspx?name={1}&password={2}", WWWOperations.instance.ServerUrl, username, password);
        WWWOperations.instance.FetchJsonObject<ResponseMessage>(authorizationString, AuthorizationResult);
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
            Debug.LogError("Получен неизвестный объект");
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

        SessionStarted(_currentUser);
    }

    #endregion
}
