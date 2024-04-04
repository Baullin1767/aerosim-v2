using System;
using System.Collections;
using System.Collections.Generic;
using TimeCheck;
using Unity.VisualScripting;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private static Bootstrapper _instance;
    public static Bootstrapper Instance => _instance;

    [SerializeField]
    private GameObject mainImage;
    
    [SerializeField]
    private TimeConfig timeConfig;

    [SerializeField]
    private GameObject blockedWindow;
    
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        mainImage.gameObject.SetActive(true);
        StartRoutine(LoadAll());
    }

    private IEnumerator LoadAll()
    {
        yield return timeConfig.FindEndTime();
        if (timeConfig.IsEndTime) blockedWindow.gameObject.SetActive(true);
        mainImage.gameObject.SetActive(false);
    }

    public void StartRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
