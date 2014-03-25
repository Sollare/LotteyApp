using Newtonsoft.Json;
using UnityEngine;
using System.Collections;

public class WWWOperations : MonoBehaviour
{
    public delegate void OnDataFecthed(string data, string error);
    public delegate void OnObjectFecthed<T>(T fetchedObject, string error);

    private static WWWOperations _instance;

    public static WWWOperations instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("WWW").GetComponent<WWWOperations>();

            return _instance;
        }
    }

    public string ServerUrl = @"http://sstucloud.no-ip.info/LotteyServerApp";

    void Start()
    {
        SessionController.instance.SignIn("root", "123456");
        //SessionController.instance.SignUp("Test", "test@yandex.ru", "123");
    }

    public void FetchData(string url, OnDataFecthed callback)
    {
        StartCoroutine(EnumeratorFetchData(url, callback));
    }

    static IEnumerator EnumeratorFetchData(string url, OnDataFecthed callback)
    {
        var www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            Debug.LogWarning("Fetch error: " + www.error);
            callback(null, www.error);
        }
        else
        {
            Debug.LogWarning("Fetched: " + www.text);
            callback(www.text, null);
        }
    }

    public void FetchJsonObject<T>(string url, OnObjectFecthed<T> callback)
    {
        StartCoroutine(EnumeratorFetchJsonObject<T>(url, callback));
    }

    static IEnumerator EnumeratorFetchJsonObject<T>(string url, OnObjectFecthed<T> callback)
    {
        Debug.Log("Fetching using url: " + url);

        var www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            Debug.LogWarning("Fetch error: " + www.error);
            callback(default(T), www.error);
        }
        else
        {
            Debug.LogWarning("Fetched: " + www.text);

            var deserializedObject = JsonConvert.DeserializeObject<T>(www.text);
            
            if (deserializedObject.Equals(default(T)))
            {
                Debug.LogError("Не удалось десериализовать объект");
                callback(deserializedObject, "Не удалось десериализовать объект");
            }
            else
            {
                callback(deserializedObject, null);
            }

            //Debug.LogWarning("Deserialization result: " + deserializedObject);

        }
    }
}