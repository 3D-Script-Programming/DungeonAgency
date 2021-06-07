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
        player.AddRangeMonster(CharacterFactory.CreateMonsterList(6));
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
