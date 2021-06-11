using System;
using UnityEngine;
using System.Collections.Generic;

public class Player
{
    int gold;
    int evilPoint = 10; // 임시 악명 설정

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
        if(monsters.Count > 0)
            return monsters;

        return null;
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
        if(monsters.Count > index)
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

}