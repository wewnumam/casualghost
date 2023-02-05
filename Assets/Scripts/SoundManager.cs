using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    

    [SerializeField] private AudioSource audioSourceComponent;
    [SerializeField] private AudioClip hijaiyahAlifSFX;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayHijaiyahAlifSFX()
    {
        audioSourceComponent.clip = hijaiyahAlifSFX;
        audioSourceComponent.Play();
    }
}
