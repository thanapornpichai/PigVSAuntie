using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Image chargeBarImage;
    [SerializeField]
    private GameObject chargeBarObject;

    void Update()
    {
        if (playerController != null && chargeBarImage != null && chargeBarObject != null)
        {
            chargeBarObject.SetActive(playerController.isTurn && playerController.isCharging &&Input.GetMouseButton(0));

            float fillAmount = playerController.chargeForce / playerController.maxForce;
            chargeBarImage.fillAmount = Mathf.Clamp01(fillAmount);
        }
    }
}
