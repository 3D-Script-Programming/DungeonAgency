using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }

    private static SoundManager m_instance;


    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void SetMusic(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }
}
