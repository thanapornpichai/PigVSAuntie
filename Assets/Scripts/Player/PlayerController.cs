using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bonePrefab;
    public GameObject powerPrefab;
    [SerializeField] private Transform shootPoint;
    private float minForce;
    public float maxForce;
    public float windForce;
    public bool isWindRight;
    private float savedWindForce;
    private bool savedIsWindRight;
    public int normalDamage, smallDamage;
    public int hp;
    private int hpMax, hpHeal;
    public bool isPlayer1;
    public bool isTurn;

    public float chargeForce;
    private float savedchargeForce;
    public bool isCharging = false;
    private bool isDoubleAttackActive = false;
    public bool isDoubleAttacking = false;
    public bool getDamage = false;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private HPBarController hpBarController;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ItemData itemData;

    public GameObject currentProjectilePrefab;
    public GameObject opponentShootButton;

    private SkeletonAnimation skeletonAnimation;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", true);

        currentProjectilePrefab = bonePrefab;

        SetPlayerData();
        SetItemData();

        if (hpBarController != null)
        {
            hpBarController.SetMaxHP(hp);
        }

    }

    void Update()
    {
        if (!isTurn) return;

        if (isCharging)
        {
            ChargePower();
        }
    }

    private void SetPlayerData()
    {
        string playerName = "Player";
        var player = playerData.playerData.Find(p => p.Name == playerName);
        hp = player.HP;
        hpMax = player.HP;
        minForce = player.MinPower;
        maxForce = player.MaxPower;
        normalDamage = player.NormalAttack;
        smallDamage = player.SmallAttack;
    }

    private void SetItemData()
    {
        string Heal = "Heal";
        var heal = itemData.itemData.Find(p => p.Name == Heal);
        hpHeal = heal.HP;
    }

    public void ChargePower()
    {
        if (isTurn)
        {
            isCharging = true;
            chargeForce += Time.deltaTime * 10f;
            chargeForce = Mathf.Clamp(chargeForce, minForce, maxForce);
        }
    }

    public void EndCharge()
    {
        if (isTurn)
        {
            ShootProjectile();
            isCharging = false;
            chargeForce = minForce;
            if (!gamePlayManager.isSinglePlayer && !isDoubleAttacking)
            {
                StartCoroutine(SetButtonNextTurn());
            }
        }
    }

    private IEnumerator SetButtonNextTurn()
    {
        opponentShootButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        opponentShootButton.SetActive(true);
    }

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(currentProjectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 baseDirection = shootPoint.right + shootPoint.up * 1.5f;
            Vector2 shootDirection = isPlayer1 ? new Vector2(-baseDirection.x, baseDirection.y).normalized : baseDirection.normalized;

            Vector2 totalForce = shootDirection * chargeForce;

            if (isWindRight)
            {
                if (isPlayer1)
                {
                    totalForce /= (windForce / 1.5f);
                }
                else
                {
                    totalForce *= (windForce * 1.5f);
                }
            }
            else
            {
                if (isPlayer1)
                {
                    totalForce *= (windForce * 1.5f);
                }
                else
                {
                    totalForce /= (windForce / 1.5f);
                }
            }
            savedchargeForce = chargeForce;
            savedWindForce = windForce;
            savedIsWindRight = isWindRight;

            rb.AddForce(totalForce, ForceMode2D.Impulse);
            gamePlayManager.PlayerShot();
        }

        if (isDoubleAttackActive)
        {
            if (opponentShootButton != null && !gamePlayManager.isSinglePlayer)
            {
                opponentShootButton.SetActive(false);
            }
            StartCoroutine(DoubleAttackCoroutine());
            isDoubleAttackActive = false;
        }
        else
        {
            EndTurn();
        }
    }



    public void UsePowerThrow()
    {
        currentProjectilePrefab = powerPrefab;
    }

    public void UseDoubleAttack()
    {
        isDoubleAttackActive = true;
    }

    private IEnumerator DoubleAttackCoroutine()
    {
        isDoubleAttacking = true;
        yield return new WaitUntil(() => !isTurn);

        yield return new WaitForSeconds(2f);

        GameObject projectile = Instantiate(currentProjectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 baseDirection = shootPoint.right + shootPoint.up * 1.5f;
            Vector2 shootDirection = isPlayer1 ? new Vector2(-baseDirection.x, baseDirection.y).normalized : baseDirection.normalized;

            Vector2 totalForce = shootDirection * savedchargeForce;

            if (savedIsWindRight)
            {
                if (isPlayer1)
                {
                    totalForce /= (savedWindForce / 1.5f);
                }
                else
                {
                    totalForce *= (savedWindForce * 1.5f);
                }
            }
            else
            {
                if (isPlayer1)
                {
                    totalForce *= (savedWindForce * 1.5f);
                }
                else
                {
                    totalForce /= (savedWindForce / 1.5f);
                }
            }

            rb.AddForce(totalForce, ForceMode2D.Impulse);
        }

        chargeForce = 0f;

        yield return new WaitForSeconds(2f);

        if (opponentShootButton != null && !gamePlayManager.isSinglePlayer)
        {
            opponentShootButton.SetActive(true);
        }

        isDoubleAttacking = false;

        EndTurn();

        if (gamePlayManager.isSinglePlayer && gamePlayManager.isPlayer1Turn)
        {
            isTurn = true;
        }
    }


    public void UseHeal()
    {
        hp += hpHeal;
        hp = Mathf.Clamp(hp, 0, hpMax);

        if (hpBarController != null)
        {
            hpBarController.UpdateHPBar(hp);
        }

        gamePlayManager.PlayerHeal();

        EndTurn();
    }

    public void SetWindForce(float newWindForce)
    {
        windForce = newWindForce;
    }

    public void EndTurn()
    {
        isTurn = false;
        currentProjectilePrefab = bonePrefab;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hpBarController != null)
        {
            hpBarController.UpdateHPBar(hp);
        }

        if (hp <= 0)
        {
            if (gamePlayManager.isPlayer1Turn)
            {
                gamePlayManager.ShowGameResult("Pig");
            }
            else
            {
                gamePlayManager.ShowGameResult("Auntie");
            }
        }
    }

    public IEnumerator HitBodyAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 2", true);
        yield return new WaitForSeconds(3f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 2", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", true);
        getDamage = false;
    }

    public IEnumerator HitHeadAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Moody Friendly", true);
        yield return new WaitForSeconds(3f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Moody Friendly", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", true);
        getDamage = false;
    }

    public IEnumerator MissedHitAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Happy Friendly", true);
        yield return new WaitForSeconds(3f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Happy Friendly", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", true);
    }

    public IEnumerator WinAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Cheer Friendly", true);
        yield return new WaitForSeconds(3f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Cheer Friendly", true);
    }

    public IEnumerator LoseAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Moody UnFriendly", true);
        yield return new WaitForSeconds(3f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle Friendly 1", false);
        skeletonAnimation.AnimationState.SetAnimation(0, "Moody UnFriendly", true);
    }

}
