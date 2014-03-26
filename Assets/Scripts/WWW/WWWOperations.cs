﻿using Newtonsoft.Json;
using UnityEngine;
using System.Collections;

public class WWWOperations : MonoBehaviour
{
    public delegate void OnDataFecthed(string data, string error);
    public delegate void OnObjectFecthed<T>(T fetchedObject, string error);
    public delegate void OnObjectFecthedChainedCallback<T>(T fetchedObject, string error, OnObjectFecthed<T> callback);

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
        StartCoroutine(EnumeratorFetchJsonObjectChained<T>(url, callback));
    }

    static IEnumerator EnumeratorFetchJsonObjectChained<T>(string url, OnObjectFecthed<T> callback)
    {
        Debug.Log("Fetching using url: " + url);

        var www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            Debug.LogWarning("Fetch error: " + www.error);
            callback(default(T), www.error);
            //callback(default(T), www.error);
        }
        else
        {
            //Debug.LogWarning("Fetched: " + www.text);

            var deserializedObject = JsonConvert.DeserializeObject<T>(www.text);
            
            if (object.Equals(deserializedObject, default(T)))
            {
                Debug.Log("Fetched object of type <" + typeof(T) + "> is null");
                callback(deserializedObject, "Не удалось десериализовать объект");
            }
            else
            {
                callback(deserializedObject, null);
            }

            //Debug.LogWarning("Deserialization result: " + deserializedObject);

        }
    }

    public void FetchJsonObject<T>(string url, OnObjectFecthed<T> originalCallback, OnObjectFecthedChainedCallback<T> chainedCallback)
    {
        StartCoroutine(EnumeratorFetchJsonObject<T>(url, originalCallback, chainedCallback));
    }

    static IEnumerator EnumeratorFetchJsonObject<T>(string url, OnObjectFecthed<T> originalCallback, OnObjectFecthedChainedCallback<T> chainedCallback)
    {
        Debug.Log("Fetching using url: " + url);

        var www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            Debug.LogWarning("Fetch error: " + www.error);
            chainedCallback(default(T), www.error, originalCallback);
            //callback(default(T), www.error);
        }
        else
        {
            //Debug.LogWarning("Fetched: " + www.text);

            var deserializedObject = JsonConvert.DeserializeObject<T>(www.text);

            if (object.Equals(deserializedObject, default(T)))
            {
                Debug.Log("Fetched object of type <" + typeof(T) + "> is null");
                chainedCallback(deserializedObject, "Не удалось десериализовать объект", originalCallback);
            }
            else
            {
                //callback(deserializedObject, null);
                chainedCallback(deserializedObject, null, originalCallback);
            }

            //Debug.LogWarning("Deserialization result: " + deserializedObject);

        }
    }

    
}