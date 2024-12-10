using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    public int maxHP;

    public void SetMaxHP(int maxHp)
    {
        maxHP = maxHp;
        UpdateHPBar(maxHP);
    }

    public void UpdateHPBar(int currentHP)
    {
        hpFillImage.fillAmount = (float)currentHP / maxHP;
    }
}
