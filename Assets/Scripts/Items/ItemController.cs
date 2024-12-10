using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public PlayerController player;
    public List<Button> itemButtons;
    public List<Button> opponentItemButtons;
    public GamePlayManager gamePlayManager;

    public void UseThrowPower()
    {
        if (player.isTurn)
        {
            player.UsePowerThrow();
            UsedItem(0);
            DisableItemButtons();
        }
    }

    public void UseDoubleAttack()
    {
        if (player.isTurn)
        {
            player.UseDoubleAttack();
            UsedItem(1);
            DisableItemButtons();
        }
    }

    public void UseHeal()
    {
        if (player.isTurn)
        {
            player.UseHeal();
            UsedItem(2);
            DisableItemButtons();
        }
    }

    private void UsedItem(int index)
    {
        itemButtons[index].gameObject.SetActive(false);
    }

    private void DisableItemButtons()
    {
        foreach (Button itemButton in itemButtons)
        {
            itemButton.interactable = false;
        }
    }

    public void DisableOpponentItemButtons()
    {
        foreach (Button opponentItemButton in opponentItemButtons)
        {
            opponentItemButton.interactable = false;
        }
    }

    public void ResetItemButtons()
    {
        foreach (Button itemButton in itemButtons)
        {
            itemButton.interactable = true;
        }
    }
}
