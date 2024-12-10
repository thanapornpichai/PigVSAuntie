using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpGame : MonoBehaviour
{
    private const string ReplayKey = "IsReplay";
    private const string LoginKey = "IsLogin";
    private SceneController sceneControl;

    private void Awake()
    {
        PlayerPrefs.SetInt(LoginKey, 0);
        PlayerPrefs.SetInt(ReplayKey, 0);
        sceneControl = GetComponent<SceneController>();
    }

    private void Start()
    {
        sceneControl.NextScene("MenuScene");
    }
}
