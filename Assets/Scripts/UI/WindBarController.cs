using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindBarController : MonoBehaviour
{
    [SerializeField] private Image windBarRight;
    [SerializeField] private Image windBarLeft;
    [SerializeField] private GameObject windArrowRight;
    [SerializeField] private GameObject windArrowLeft;
    [SerializeField] private GamePlayManager gamePlayManager;

    public void UpdateWindBar(float windForce, bool isWindRight)
    {
        windForce = Mathf.Clamp(windForce, gamePlayManager.winforceMin, gamePlayManager.winforceMax);

        if (isWindRight)
        {
            windBarRight.fillAmount = (windForce - 1f) / 1f;
            windBarRight.gameObject.SetActive(true);
            windBarLeft.gameObject.SetActive(false);
            windArrowRight.gameObject.SetActive(true);
            windArrowLeft.gameObject.SetActive(false);
        }
        else
        {
            windBarLeft.fillAmount = (windForce - 1f) / 1f;
            windBarLeft.gameObject.SetActive(true);
            windBarRight.gameObject.SetActive(false);
            windArrowLeft.gameObject.SetActive(true);
            windArrowRight.gameObject.SetActive(false);
        }
    }
}
