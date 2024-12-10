using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loginPanel, menuPanel, howToPlayDialog, modeDialog, levelDialog;

    private bool isSinglePlayer;
    private string difficulty;
    private const string ReplayKey = "IsReplay";

    private void Start()
    {
        if (PlayerPrefs.GetInt(ReplayKey) == 1)
        {
            OpenMenu();
            PlayerPrefs.SetInt(ReplayKey, 0);
        }
    }

    public void OpenMenu()
    {
        loginPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        menuPanel.SetActive(false);
        howToPlayDialog.SetActive(true);
    }

    public void OpenMode()
    {
        howToPlayDialog.SetActive(false);
        modeDialog.SetActive(true);
    }
    public void OpenLevel()
    {
        modeDialog.SetActive(false);
        levelDialog.SetActive(true);
    }

    public void SelectSinglePlayer()
    {
        isSinglePlayer = true;
    }

    public void SelectTwoPlayer()
    {
        isSinglePlayer = false;
    }

    public void SelectDifficulty(string selectedDifficulty)
    {
        difficulty = selectedDifficulty;
    }

    public void StartGame()
    {
        GameSetting.Instance.SetGameSettings(isSinglePlayer, difficulty);
        SceneManager.LoadScene("GameScene");
    }
}
