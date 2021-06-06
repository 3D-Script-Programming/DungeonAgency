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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static Character CreateCharacter(int rank)
    {
        System.Random random = new System.Random();
        int statusSum = (int)Math.Pow(1.2, rank) * 50;
        int defaultStatus = statusSum * 2 / 5;

        statusSum -= defaultStatus;

        int str = random.Next(statusSum);
        int bal = random.Next(statusSum -= str);
        int vtp = statusSum - bal;

        int potential = 10 - (int)Math.Log(random.Next(2048), 2);

        Nature nature = (Nature)random.Next(3);

        return new Character(str, bal, vtp, potential, nature);
    }
    public static List<Character> CreateCharacterList(int count)
    {
        System.Random random = new System.Random();
        List<Character> characters = new List<Character>();

        for (int i = 0; i < count; i++)
            characters.Add(CreateCharacter(random.Next()));

        return characters;

    }

    public void SaveData() { }
    public void LoadData() { }
}
