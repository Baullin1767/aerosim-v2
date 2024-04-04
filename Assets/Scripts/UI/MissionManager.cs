using Services;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MissionManager
{
    public static void Intro()
    {
        SceneManager.LoadScene("Intro");
    }

    public static void Start()
    {
        SceneManager.LoadScene("Main");
    }

    public static void Mission()
    {
        SceneManager.LoadScene("Mission");
    }

    public static void Success()
    {
        var gameUi = UnityEngine.Object.FindObjectOfType<GameUI>();
        var uiManager = UnityEngine.Object.FindObjectOfType<UIManager>();
        if (gameUi.PreStart)
        {
            MissionService.Instance.CreateFailMenu(uiManager.transform);
            return;
        }
        MissionService.Instance.CreateSuccessMenu(uiManager.transform);
        //uiManager.OpenSuccessMenu();
        //SceneManager.LoadScene("Success");
    }

    public static void Fail()
    {
        Debug.Log("Fail");
        var uiManager = UnityEngine.Object.FindObjectOfType<UIManager>();
        MissionService.Instance.CreateFailMenu(uiManager.transform);
        //uiManager.OpenFailMenu();
        //SceneManager.LoadScene("Fail");
    }
}