using System;
using UnityEngine;
using System.Collections.Generic;
public static class CharacterFactory
{
    private static System.Random random = new System.Random();
    public static Character CreateMonster(int rank)
    {
        return Create(true, rank);
    }

    public static Character CreateHero(int rank)
    {
        return Create(false, rank);
    }

    public static Character CreateMonster()
    {
        return Create(true, random.Next(21));
    }

    public static Character CreateHero()
    {
        return Create(false, random.Next(21));
    }

    public static List<Character> CreateMonsterList(params int[] ranks)
    {
        return CreateList(true, ranks);
    }

    public static List<Character> CreateHeroList(params int[] ranks)
    {
        return CreateList(false, ranks);
    }

    public static List<Character> CreateMonsterList(int count)
    {
        return CreateList(true, count);
    }

    public static List<Character> CreateHeroList(int count)
    {
        return CreateList(false, count);
    }

    private static Character Create(bool isMonster, int rank)
    {
        int statusSum = (int)Math.Pow(1.2, rank) * 50;
        int defaultStatus = statusSum * 2 / 5;

        statusSum -= defaultStatus;

        int str = random.Next(statusSum);
        int bal = random.Next(statusSum -= str);
        int vtp = statusSum - bal;

        int potential = 10 - (int)Math.Log(random.Next(2048), 2);

        Nature nature = (Nature)Enum.ToObject(typeof(Nature), random.Next(Enum.GetValues(typeof(Nature)).Length));

        string prefabPath;
        if (isMonster)
        {
            prefabPath = GetPrefabPath(isMonster, random.Next(Enum.GetValues(typeof(Monster)).Length));
        }
        else
        {
            prefabPath = GetPrefabPath(isMonster, random.Next(Enum.GetValues(typeof(Hero)).Length));
        }
        return new Character(prefabPath, str, bal, vtp, potential, nature);
    }

    private static List<Character> CreateList(bool isMonster, params int[] ranks)
    {
        List<Character> characters = new List<Character>();
        foreach (int r in ranks)
        {
            characters.Add(Create(isMonster, r));
        }

        return characters;
    }

    private static List<Character> CreateList(bool isMonster, int count)
    {
        List<Character> characters = new List<Character>();
        for(int i = 0; i < count; i++)
        {
            characters.Add(Create(isMonster, random.Next(21)));
        }

        return characters;
    }

    private static string GetPrefabPath(bool isMonster, int prefabCode)
    {
        if (isMonster)
           return "Monster/"+((Monster)Enum.ToObject(typeof(Monster), prefabCode)).ToString();
        else
            return "Hero/" + ((Hero)Enum.ToObject(typeof(Hero), prefabCode)).ToString();
    }
}
