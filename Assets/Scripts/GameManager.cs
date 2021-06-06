using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;

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

    // Start is called before the first frame update
    void Start()
    {
        player.AddRangeMonster(CharacterFactory.CreateMonsterList(6));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveData() { }
    public void LoadData() { }
}
