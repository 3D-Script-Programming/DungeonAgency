using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class Character
{
    private int level;
    private int exp;
    private int str, bal, vtp;
    private Nature nature;
    private int hp;
    private int rank, potential;
    private string name;

    private GameObject prefab;

    public GameObject Prefab { get => prefab; set => prefab = value; }

    public Character(string prefabPath, int str, int bal, int vtp, int potential, Nature nature)
    {
        level = 1;
        exp = 0;

        this.str = str;
        this.bal = bal;
        this.vtp = vtp;
        this.nature = nature;
        this.potential = potential;
        hp = GetMaxHP();
        prefab = Resources.Load<GameObject>(prefabPath);
        name = prefabPath.Split('/')[1];
        name = Regex.Replace(name, @"\d", "");
    }

    public Character(string prefabPath, int level, int str, int bal, int vtp, int potential, Nature nature)
        : this(prefabPath, str, bal, vtp, potential, nature)
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
        hp = GetMaxHP();
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

    public string GetName()
    {
        return name;
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
        return vtp * 50;
    }

    public int GetHP()
    {
        return hp;
    }

    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
    }

    public int GetDamage()
    {
        int maxDamage = str * 15;
        int minDamage = maxDamage / (1 + (int)(Math.Log(bal, 2) * 20));

        return CharacterFactory.random.Next(minDamage, maxDamage);
    }

    public int GetMaxDamage()
    {
        return str * 15;
    }

    public int GetSTR()
    {
        return str;
    }

    public int GetBAL()
    {
        return bal;
    }

    public int GetVTP()
    {
        return vtp;
    }
    public int GetPrice()
    {
        return (int)Math.Pow(GetCP(), 1.2);
    }

    public void SetBoss()
    {
        str = str * 4;
        bal = bal * 4;
        vtp = vtp * 4;
    }

    public void FinishBoss()
    {
        str = str / 4;
        bal = bal / 4;
        vtp = vtp / 4;
    }

    public void AddExp(int addExp)
    {
        int reqExp = GetReqExp();
        exp += addExp;
        while (reqExp < exp)
        {
            int remainExp = exp - reqExp;
            LevelUp();
            reqExp = GetReqExp();
            exp += remainExp;
        }
    }

    public void SetResetHp()
    {
        hp = GetMaxHP();
    }
}
