using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource BGSound, winSound;

    public void PlayWin()
    {
        BGSound.Stop();
        winSound.Play();
    }
}
