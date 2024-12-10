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
    [SerializeField]
    private List<GameObject> enemyItemsButtons;
    private bool isShooting = false;
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
            yield return new WaitForSeconds(4f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        
        float chargeForce = CalculateChargeForce();
        playerControllerSelf.chargeForce = chargeForce;
        playerControllerSelf.ShootProjectile();

        isShooting = false;
        yield return new WaitForSeconds(2f);
        player1ShootBtn.SetActive(true);
    }

    private float CalculateChargeForce()
    {
        float chargeForce;

        if (playerControllerSelf.isWindRight)
        {
            if (playerControllerSelf.windForce >= 1f && playerControllerSelf.windForce < 1.5f)
            {
                chargeForce = 7f;
            }
            else if (playerControllerSelf.windForce >= 1.5f && playerControllerSelf.windForce < 1.7f)
            {
                chargeForce = 5f;
            }
            else if (playerControllerSelf.windForce >= 1.7f && playerControllerSelf.windForce < 1.9f)
            {
                chargeForce = 4.5f;
            }
            else if (playerControllerSelf.windForce >= 1.9f && playerControllerSelf.windForce <= 2f)
            {
                chargeForce = 4f;
            }
            else
            {
                chargeForce = 0f;
            }
        }
        else
        {
            if (playerControllerSelf.windForce >= 1f && playerControllerSelf.windForce < 1.2f)
            {
                chargeForce = 9f;
            }
            else if (playerControllerSelf.windForce >= 1.2f && playerControllerSelf.windForce < 1.5f)
            {
                chargeForce = 11f;
            }
            else if (playerControllerSelf.windForce >= 1.5f && playerControllerSelf.windForce < 1.7f)
            {
                chargeForce = 13f;
            }
            else if (playerControllerSelf.windForce >= 1.7f && playerControllerSelf.windForce <= 2f)
            {
                chargeForce = 15f;
            }
            else
            {
                chargeForce = 0f;
            }
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
        yield return new WaitForSeconds(4.1f);
        player1ShootBtn.SetActive(false);
        yield return new WaitForSeconds(2f);
        player1ShootBtn.SetActive(true);
    }
}

