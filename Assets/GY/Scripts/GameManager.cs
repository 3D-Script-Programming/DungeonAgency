using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int gold;
    int evilPoint;

    List<Character> monsters = new List<Character>();
    List<DungeonRoom> dungeon = new List<DungeonRoom>();//처음에 3칸

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData() { }
    public void LoadData() { }

    public int GetGold()
    {
        return gold;
    }

    public int GetEvilPoint()
    {
        return evilPoint;
    }
}
