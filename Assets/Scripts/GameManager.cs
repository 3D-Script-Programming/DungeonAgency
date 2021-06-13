using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;
    private AudioSource audioSource;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;

    public Player Player { get => player; set => player = value; }
    public AudioClip buttonSound;

    private void Awake()
    {
        player = new Player();

        // 임시 몬스터 9마리 생성
        int[] rank = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        player.AddRangeMonster(CharacterFactory.CreateMonsterList(rank));

        audioSource = gameObject.GetComponent<AudioSource>();
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

    public static void MoveShopScene()
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
