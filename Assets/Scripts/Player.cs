using System.Collections.Generic;

public class Player
{
    int gold;
    int evilPoint;

    List<Character> monsters = new List<Character>();
    List<DungeonRoom> dungeon = new List<DungeonRoom>
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