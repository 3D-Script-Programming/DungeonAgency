using System;
public enum Type
{
    FIRE,
    WATER,
    WIND
}
public class Character
{
    private int level;
    private int exp;
    private int str, bal, vtp;
    private Type type;

    public Character(int str, int bal, int vtp, Type type)
    {
        level = 1;
        exp = 0;

        this.str = str;
        this.bal = bal;
        this.vtp = vtp;
        this.type = type;
    }

    public Character(int level, int str, int bal, int vtp, Type type)
        :this(str, bal, vtp, type)
    {
        LevelUp(level);
    }

    public void LevelUp()
    {
        if (GetReqExp() > exp)
            return;

        level++;
        str = (int)(Math.Log10(100 + 100000 / level) + Math.Log(100 + 10000 / str, 10 + str));
        bal = (int)(Math.Log10(100 + 100000 / level) + Math.Log(100 + 10000 / bal, 10 + str));
        vtp = (int)(Math.Log10(100 + 100000 / level) + Math.Log(100 + 10000 / vtp, 10 + str));

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

    public Type GetType()
    {
        return type;
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
        return (int)(Math.Pow(level, 1.5)) * 250;
    }

    public int GetMaxHP()
    {
        return vtp * 400;
    }

    public int GetDamage()
    {
        Random rand = new Random();
        int maxDamage = str * 15;
        int minDamage = str * 15 * bal / 2;

        return rand.Next(minDamage, maxDamage);
    }
}
