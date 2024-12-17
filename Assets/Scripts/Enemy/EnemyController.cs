using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerControllerSelf, playerControllerPlayer;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private HPBarController enemyHPBar;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private ItemData itemData;
    [SerializeField] private GameObject player1ShootBtn;
    [SerializeField] private ShowTurnController showTurn;
    [SerializeField]
    private List<GameObject> enemyItemsButtons;
    private bool isShooting = false, isDoubleAttack = false;
    private int missedChance, doubleAttackNum, powerThrowNum;

    private void Start()
    {
        SetItem();
    }

    private void Update()
    {
        if (gamePlayManager.isSinglePlayer && !gamePlayManager.isPlayer1Turn && !isShooting)
        {
            player1ShootBtn.SetActive(false);
            isShooting = true;
            CheckUseItems();
            StartCoroutine(DelayedShoot());
        }
    }

    public void SetEnemy()
    {
        string difficulty = gamePlayManager.difficulty;
        string Easy = "easy";
        var easyEnemy = enemyData.enemyData.Find(p => p.Name == Easy);
        string Normal = "normal";
        var normalEnemy = enemyData.enemyData.Find(p => p.Name == Normal);
        string Hard = "hard";
        var hardEnemy = enemyData.enemyData.Find(p => p.Name == Hard);

        if (difficulty == "easy")
        {
            playerControllerSelf.hp = easyEnemy.HP;
            enemyHPBar.maxHP = easyEnemy.HP;
            missedChance = easyEnemy.MissedChance;
        }
        else if (difficulty == "normal")
        {
            playerControllerSelf.hp = normalEnemy.HP;
            enemyHPBar.maxHP = normalEnemy.HP;
            missedChance = normalEnemy.MissedChance;
        }
        else if (difficulty == "hard")
        {
            playerControllerSelf.hp = hardEnemy.HP;
            enemyHPBar.maxHP = hardEnemy.HP;
            missedChance = hardEnemy.MissedChance;
        }

        foreach (GameObject enemyItemsButton in enemyItemsButtons)
        {
            enemyItemsButton.SetActive(false);
        }
    }

    public void SetItem()
    {
        string PowerThrow = "PowerThrow";
        var powerThrow = itemData.itemData.Find(p => p.Name == PowerThrow);
        powerThrowNum = powerThrow.Inventory;
        string DoubleAttack = "DoubleAttack";
        var doubleAttack = itemData.itemData.Find(p => p.Name == DoubleAttack);
        doubleAttackNum = doubleAttack.Inventory;
    }


    private IEnumerator DelayedShoot()
    {
        if (playerControllerPlayer.isDoubleAttacking)
        {
            yield return new WaitForSeconds(7f);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }

        if (!gamePlayManager.endGame)
        {
            StartCoroutine(showTurn.ShowPigTurn());
            float chargeForce = CalculateChargeForce();
            playerControllerSelf.chargeForce = chargeForce;
            playerControllerSelf.ShootProjectile();

            isShooting = false;
            yield return new WaitForSeconds(2f);
            player1ShootBtn.SetActive(true);
            if (!isDoubleAttack)
            {
                StartCoroutine(showTurn.ShowAuntieTurn());
            }
        }
    }

    private float CalculateChargeForce()
    {
        float chargeForce;

        if (playerControllerSelf.isWindRight)
        {
            if (playerControllerSelf.windForce <= 1f) chargeForce = 7.5f;
            else if (playerControllerSelf.windForce <= 1.1f) chargeForce = 7f;
            else if (playerControllerSelf.windForce <= 1.2f) chargeForce = 6f;
            else if (playerControllerSelf.windForce <= 1.3f) chargeForce = 5.5f;
            else if (playerControllerSelf.windForce <= 1.5f) chargeForce = 5f;
            else if (playerControllerSelf.windForce <= 1.7f) chargeForce = 4.5f;
            else if (playerControllerSelf.windForce <= 1.9f) chargeForce = 4;
            else chargeForce = 3.5f;
        }
        else
        {
            if (playerControllerSelf.windForce <= 1f) chargeForce = 7.5f;
            else if (playerControllerSelf.windForce <= 1.01f) chargeForce = 8f;
            else if (playerControllerSelf.windForce <= 1.02f) chargeForce = 9f;
            else if (playerControllerSelf.windForce <= 1.05f) chargeForce = 10f;
            else if (playerControllerSelf.windForce <= 1.1f) chargeForce = 11f;
            else chargeForce = 11f;
        }

        if (CheckMissChance())
        {
            chargeForce += 2f;
        }

        return chargeForce;
    }

    private bool CheckMissChance()
    {
        int randomValue = Random.Range(0, 101);
        return randomValue < missedChance;
    }

    public void CheckUseItems()
    {
        string difficulty = gamePlayManager.difficulty;
        bool isWindRight = playerControllerSelf.isWindRight;
        float windForce = playerControllerSelf.windForce;

        if (isWindRight)
        {
            if (difficulty == "normal" && windForce < 1.3f)
            {
                if (doubleAttackNum > 0)
                {
                    isDoubleAttack = true;
                    StartCoroutine(EnemyUseDoubleAttack());
                    doubleAttackNum--;
                }
            }
            else if (difficulty == "hard")
            {
                if (windForce < 1.3f)
                {
                    if (doubleAttackNum > 0)
                    {
                        isDoubleAttack = true;
                        StartCoroutine(EnemyUseDoubleAttack());
                        doubleAttackNum--;
                    }
                }
                else if (windForce > 1.7f)
                {
                    if (powerThrowNum > 0)
                    {
                        playerControllerSelf.UsePowerThrow();
                        powerThrowNum--;
                    }
                }
            }
        }
    }

    private IEnumerator EnemyUseDoubleAttack()
    {
        playerControllerSelf.UseDoubleAttack();
        if (playerControllerPlayer.isDoubleAttacking)
        {
            yield return new WaitForSeconds(9.01f);
        }
        else
        {
            yield return new WaitForSeconds(7.01f);
        }
        player1ShootBtn.SetActive(false);
        yield return new WaitForSeconds(4f);
        isDoubleAttack = false;
        StartCoroutine(showTurn.ShowAuntieTurn());
        player1ShootBtn.SetActive(true);
    }
}

