using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarController : MonoBehaviour
{
    [SerializeField] private GameObject playerTimeBar;
    [SerializeField] private Image timeBar;
    [SerializeField] private GameData gameData;
    private float maxTime;

    private void Start()
    {
        SetGameData();
    }
    private void SetGameData()
    {
        string TimeToWarning = "TimeToWarning";
        var timeToWarning = gameData.gameData.Find(p => p.Name == TimeToWarning);
        maxTime = timeToWarning.Value;
    }

    public void UpdateTimeBar(float currentTime)
    {
        if (timeBar != null)
        {
            playerTimeBar.SetActive(true);
            timeBar.fillAmount = currentTime / maxTime;
        }
    }

    public void ResetTime()
    {
        playerTimeBar.SetActive(false);
    }
}

