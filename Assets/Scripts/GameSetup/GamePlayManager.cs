using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    public bool isSinglePlayer;
    public string difficulty;

    public bool isPlayer1Turn = true;
    public float turnTimeWarning;
    private float turnTime;
    private float countdownTimer, countTimer;
    public float winforceMin, winforceMax;
    private bool turnEnded = false, endGame = false;
    private const string ReplayKey = "IsReplay";

    [SerializeField] private TMP_Text playerTurnTxt,playerWinTxt,playerTimeTxt;
    [SerializeField] private GameObject player2ShootBtn;
    [SerializeField] private GameObject resultDialog;
    [SerializeField] private PlayerController player1PlayerController;
    [SerializeField] private PlayerController player2PlayerController;
    [SerializeField] private ItemController player1ItemController;
    [SerializeField] private ItemController player2ItemController;
    [SerializeField] private WindBarController windBarController;
    [SerializeField] private TimeBarController player1TimeBar;
    [SerializeField] private TimeBarController player2TimeBar;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private AudioController audioController;
    [SerializeField] private GameData gameData;

    private float windForce;
    private bool isWindRight;

    void Start()
    {
        isSinglePlayer = GameSetting.Instance.IsSinglePlayer;
        difficulty = GameSetting.Instance.Difficulty;
        SetGameData();

        StartTurn();

        if (isSinglePlayer)
        {
            player2ShootBtn.SetActive(false);
            enemyController.SetEnemy();
        }
    }

    void Update()
    {
        if (!endGame)
        {
            countTimer += Time.deltaTime;

            if (!turnEnded)
            {
                countdownTimer -= Time.deltaTime;

                if (countdownTimer <= turnTimeWarning)
                {
                    if (isPlayer1Turn)
                    {
                        player1TimeBar.UpdateTimeBar(countdownTimer);
                    }
                    else
                    {
                        player2TimeBar.UpdateTimeBar(countdownTimer);
                    }
                }

                if (countdownTimer <= 0f)
                {
                    EndTurn();
                }
            }
        }
    }

    private void SetGameData()
    {
        string TimeToThink = "TimeToThink";
        var timeToThink = gameData.gameData.Find(p => p.Name == TimeToThink);
        turnTime = timeToThink.Value;
        string TimeToWarning = "TimeToWarning";
        var timeToWarning = gameData.gameData.Find(p => p.Name == TimeToWarning);
        turnTimeWarning = timeToWarning.Value;
        string WindForceMin = "WindForceMin";
        var windForceMin = gameData.gameData.Find(p => p.Name == WindForceMin);
        winforceMin = windForceMin.Value;
        string WindForceMax = "WindForceMax";
        var windForceMax = gameData.gameData.Find(p => p.Name == WindForceMax);
        winforceMax = windForceMax.Value;
    }


    private void StartTurn()
    {
        turnEnded = false;
        countdownTimer = turnTime;

        windForce = Random.Range(winforceMin, winforceMax);
        isWindRight = Random.value > 0.5f;

        player1PlayerController.SetWindForce(windForce);
        player1PlayerController.isWindRight = isWindRight;
        player2PlayerController.SetWindForce(windForce);
        player2PlayerController.isWindRight = isWindRight;

        windBarController.UpdateWindBar(windForce, isWindRight);

        player1PlayerController.isTurn = isPlayer1Turn;
        player2PlayerController.isTurn = !isPlayer1Turn;
        if (isPlayer1Turn)
        {
            playerTurnTxt.text = "Player 1";
            player1ItemController.ResetItemButtons();
            player1ItemController.DisableOpponentItemButtons();
        }
        else
        {
            playerTurnTxt.text = "Player 2";
            player2ItemController.ResetItemButtons();
            player2ItemController.DisableOpponentItemButtons();
        }
    }

    public void PlayerShot()
    {
        if (!turnEnded)
        {
            EndTurn();
        }
    }

    public void PlayerHeal()
    {
        if (!turnEnded)
        {
            EndTurn();
        }
    }

    private void EndTurn()
    {
        turnEnded = true;

        player1TimeBar.ResetTime();
        player2TimeBar.ResetTime();

        if (isPlayer1Turn)
        {
            StartCoroutine(CheckDamagePlayer1());
        }
        else
        {
            StartCoroutine(CheckDamagePlayer2());
        }

        isPlayer1Turn = !isPlayer1Turn;

        StartTurn();
    }

    private IEnumerator CheckDamagePlayer1()
    {
        yield return new WaitForSeconds(2f);
        if (player2PlayerController.getDamage == false)
        {
                StartCoroutine(player2PlayerController.MissedHitAnimation());
        }
    }

    private IEnumerator CheckDamagePlayer2()
    {
        yield return new WaitForSeconds(2f);
        if (player1PlayerController.getDamage == false)
        {
            StartCoroutine(player1PlayerController.MissedHitAnimation());
        }
    }
    public void ShowGameResult(string winName)
    {
        endGame = true;
        audioController.PlayWin();
        if(player1PlayerController.hp <= 0)
        {
            StartCoroutine(player1PlayerController.LoseAnimation());
            StartCoroutine(player2PlayerController.WinAnimation());
        }
        else if(player2PlayerController.hp <= 0)
        {
            StartCoroutine(player1PlayerController.WinAnimation());
            StartCoroutine(player2PlayerController.LoseAnimation());
        }
        resultDialog.SetActive(true);
        PlayerPrefs.SetInt(ReplayKey, 1);
        playerWinTxt.text = winName + " Win!";
        int minutes = Mathf.FloorToInt(countTimer / 60);
        int seconds = Mathf.FloorToInt(countTimer % 60);
        string timeDisplay = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        playerTimeTxt.text = timeDisplay + " m";
    }
}
