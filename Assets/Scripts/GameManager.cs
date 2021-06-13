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

        // 임시 각 룸에 몬스터 배치
        // 1번 룸
        player.GetRoom(0).PlaceMonster(0, player.GetMonster(0));

        // 2번 룸
        player.GetRoom(1).PlaceMonster(0, player.GetMonster(3));
        player.GetRoom(1).PlaceMonster(1, player.GetMonster(4));
        player.GetRoom(1).PlaceMonster(2, player.GetMonster(5));


        // 3번 룸 보스방 보스는 몬스터 리스트 0번으로 배치할 것!
        player.GetRoom(2).Item = Item.CROWN;
        player.GetRoom(2).PlaceMonster(0, player.GetMonster(8));

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
