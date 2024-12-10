using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHead : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private string bonesTag, powerTag;
    [SerializeField] private ItemData itemData;
    private int powerThrowDamage;
    private void Start()
    {
        SetItem();
    }
    public void SetItem()
    {
        string PowerThrow = "PowerThrow";
        var powerThrow = itemData.itemData.Find(p => p.Name == PowerThrow);
        powerThrowDamage = powerThrow.Damage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(bonesTag))
        {
            StartCoroutine(playerController.HitHeadAnimation());
            playerController.TakeDamage(playerController.normalDamage);
            playerController.getDamage = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(powerTag))
        {
            StartCoroutine(playerController.HitHeadAnimation());
            playerController.TakeDamage(powerThrowDamage);
            playerController.currentProjectilePrefab = playerController.bonePrefab;
            playerController.getDamage = true;
            Destroy(other.gameObject);
        }
    }
}
