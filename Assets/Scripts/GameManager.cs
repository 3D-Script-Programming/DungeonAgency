using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;
    private AudioSource audioSource;

    public static GameManager instance;

    public Player Player { get => player; set => player = value; }
    public AudioClip buttonSound;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        player = new Player();
        player.AddGold(5000);
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

    public static void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public static void MoveBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public static void MoveManageScene()
    {
        SceneManager.LoadScene("ManageScene");
    }

    public static void MoveMarketScene()
    {
        SceneManager.LoadScene("MarketScene");
    }

    public static void MoveSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
    }

    public void OffPause()
    {
        Time.timeScale = 1f;
    }

    public void ButtonSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(buttonSound);
    }

    public void SetMusic(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void SaveData() { }
    public void LoadData() { }
}
