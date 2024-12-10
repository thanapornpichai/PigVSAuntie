using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance { get; private set; }

    public bool IsSinglePlayer { get; private set; }
    public string Difficulty { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetGameSettings(bool isSinglePlayer, string difficulty)
    {
        IsSinglePlayer = isSinglePlayer;
        Difficulty = difficulty;
    }
}
