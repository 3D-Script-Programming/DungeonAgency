using System;
using System.Collections;
using UnityEngine;

public class Character:MonoBehaviour
{
    private int level;
    private int exp;
    private int str, bal, vtp;
    private Nature nature;
    private int hp;

    private int rank, potential;
    private string name;

    private GameObject prefab;

    public Character(string prefabPath, int str, int bal, int vtp, int potential, Nature nature)
    {
        level = 1;
        exp = 0;

        this.str = str;
        this.bal = bal;
        this.vtp = vtp;
        this.nature = nature;
        this.potential = potential;

        prefab = Resources.Load<GameObject>(prefabPath);
    }

    public Character(string prefabPath, int level, int str, int bal, int vtp, int potential, Nature nature)
        :this(prefabPath, str, bal, vtp, potential, nature)
    {
        LevelUp(level);
    }

    public void LevelUp()
    {
        if (GetReqExp() > exp)
            return;

        level++; 
        int statusSum = (int)(Math.Log10(100 + level) * 15 + potential * 1.2);
        str += statusSum / 3;
        vtp += statusSum / 3;
        bal += statusSum / 3;

        exp = 0;
    }

    public void LevelUp(int level)
    {
        while (this.level < level)
        {
            exp = GetReqExp();
            LevelUp();
        }
    }

    public int GetCP()
    {
        return str * 8 + bal * 3 + vtp * 4;
    }

    public Nature GetNature()
    {
        return nature;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetReqExp()
    {
        int levelWeight = (int)Math.Pow(level, 1.3);
        return levelWeight * (200 + (potential * 80) * (rank + 5) / 10);
    }

    public int GetMaxHP()
    {
        return vtp * 400;
    }

    public int GetHP()
    {
        return hp;
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
    }

    public int GetDamage()
    {
        System.Random rand = new System.Random();
        int maxDamage = str * 15;
        int minDamage = str * 15 * bal / 2;

        return rand.Next(minDamage, maxDamage);
    }
}
