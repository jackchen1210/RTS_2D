using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagerHelper
{
    public static event UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> OnSceneLoaded
    {
        add
        {
            SceneManager.sceneLoaded += value;
        }
        remove
        {
            SceneManager.sceneLoaded -= value;
        }
    }

    public enum Scene
    {
        MainScene,
        GameScene
    }

    public static void SetSceneTo(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
        TimeResume();
    }

    internal static void TimePause()
    {
        Time.timeScale = 0;
    }
    internal static void TimeResume()
    {
        Time.timeScale = 1;
    }
}
