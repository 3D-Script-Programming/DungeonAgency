using System;
using UnityEngine;
using System.Collections.Generic;

public class Player
{
    int gold = 0;
    int evilPoint = 0; // 임시 악명 설정
    int[] items = {0, 0};

    private List<Character> monsters = new List<Character>();
    private List<DungeonRoom> dungeon = new List<DungeonRoom>
    {
        new DungeonRoom(),
        new DungeonRoom(),
        new DungeonRoom()
    };

    public DungeonRoom GetRoom(int index)
    {
        if (dungeon.Count > index)
            return dungeon[index];

        return null;
    }

    public int GetRoomCount()
    {
        return dungeon.Count;
    }

    public Character GetMonster(int index)
    {
        if (monsters.Count > index)
            return monsters[index];

        return null;
    }

    public List<Character> GetMonsterList()
    {
        if (monsters.Count > 0)
            return monsters;

        return null;
    }

    public void AddItem(Item item, int count)
    {
        if (item == Item.CROWN)
            items[0] += count;
        if (item == Item.TREASURE)
            items[1] += count;
    }

    public int GetItem(Item item)
    {
        if (item == Item.CROWN)
            return items[0];
        else if(item == Item.TREASURE)
            return items[1];
        return 0;
    }

    public void UseCrown() {
        items[0]--;
    }

    public void UnuseCrown() {
        items[0]++;
    }

    public void UseTreasure() {
        items[1]--;
    }

    public void UnuseTreasure() {
        items[1]++;
    }

    public void AddMonster(Character monster)
    {
        monsters.Add(monster);
    }

    public void AddRangeMonster(List<Character> monsters)
    {
        this.monsters.AddRange(monsters);
    }

    public void RemoveMonster(int index)
    {
        if (monsters.Count > index)
            monsters.RemoveAt(index);
    }

    public void AddRoom()
    {
        dungeon.Add(new DungeonRoom());
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetEvilPoint()
    {
        return evilPoint;
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    public void AddEvilPoint(int evilPoint)
    {
        this.evilPoint += evilPoint;
        if (evilPoint < 0)
        {
            this.evilPoint = 0;
        }
    }

    public int GetHeroRank()
    {
        int count = 0;
        for (int i = 0; i < dungeon.Count; i++)
        {
            if(dungeon[i].Item == Item.TREASURE)
            {
                count += 500;
            }
        }

        double rank = (evilPoint + count) / Math.Pow(10, Math.Log(evilPoint + count) + 2);
        return (int)rank;
    }

}