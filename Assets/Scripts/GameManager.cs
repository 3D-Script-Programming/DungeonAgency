using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player player;

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

    public Player Player { get => player; set => player = value; }

    private static GameManager m_instance;

    private void Awake()
    {
        player = new Player();

        // 임시 몬스터 9마리 생성
        int[] rank = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        player.AddRangeMonster(CharacterFactory.CreateMonsterList(rank));

        // 임시 각 룸에 몬스터 배치
        // 1번 룸
        player.GetRoom(0).PlaceMonster(0, player.GetMonster(0));
        player.GetRoom(0).PlaceMonster(2, player.GetMonster(1));
        player.GetRoom(0).PlaceMonster(5, player.GetMonster(2));

        // 2번 룸
        player.GetRoom(1).PlaceMonster(0, player.GetMonster(3));
        player.GetRoom(1).PlaceMonster(1, player.GetMonster(4));
        player.GetRoom(1).PlaceMonster(2, player.GetMonster(5));


        // 3번 룸 보스방 보스는 몬스터 리스트 0번으로 배치할 것!
        player.GetRoom(2).Item = Item.CROWN;
        player.GetRoom(2).PlaceMonster(0, player.GetMonster(8));
    }
    void Start()
    {
    }

    void Update()
    {

    }

    public void SaveData() { }
    public void LoadData() { }
}
