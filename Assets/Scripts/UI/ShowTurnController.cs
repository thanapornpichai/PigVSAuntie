using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTurnController : MonoBehaviour
{
    [SerializeField]
    private GameObject pigTurn, auntieTurn;

    public IEnumerator ShowPigTurn()
    {
        pigTurn.SetActive(true);
        yield return new WaitForSeconds(1f);
        pigTurn.SetActive(false);
    }

    public IEnumerator ShowAuntieTurn()
    {
        auntieTurn.SetActive(true);
        yield return new WaitForSeconds(1f);
        auntieTurn.SetActive(false);
    }

}
