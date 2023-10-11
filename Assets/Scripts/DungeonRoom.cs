
using System;
using System.Collections.Generic;

public class DungeonRoom
{
    // Monsters를 반환하는 프로퍼티입니다.
    // 전열(0, 1, 2) 및 후열(3, 4, 5)
    public List<Character> Monsters { get; private set; }

    // 아이템 프로퍼티
    public Item Items { get; private set; }

    // 던전 룸 번호 
    public int RoomNumber { get; private set; }

    // 생성자 : 변수 초기화
    public DungeonRoom(int roomNumber)
    {
        RoomNumber = roomNumber;
        Monsters = new List<Character>(6);
        for (int i = 0; i < 6; i++)
        {
            Monsters.Add(null); // 초기에 null로 채워진 요소를 추가
        }
        Items = Item.NONE;
    }

    // 인덱스를 통해 몬스터를 추가합니다.
    public void PlaceMonster(int index, Character monster)
    {
        if (index >= 0 && index <= Monsters.Count)
        {
            Monsters[index] = monster; // 유효한 인덱스 내에서 몬스터를 추가합니다.
            Monsters[index].PlaceInRoom(RoomNumber); // 추가한 몬스터를 방에 배치합니다.
        }
        else
        {
            // 인덱스가 유효하지 않은 경우에 대한 처리
            Console.WriteLine("유효하지 않은 인덱스입니다.");
        }
    }

    // 인덱스를 통해 몬스터를 제거합니다.
    public void RemoveMonster(int index)
    {
        if (index >= 0 && index < Monsters.Count)
        {
            Monsters[index].RemoveFromRoom(); // 해당 몬스터를 방에서 제거합니다.
            Monsters[index] = null; // 해당 인덱스에 몬스터를 null을 넣어줍니다.
        }
        else
        {
            Console.WriteLine("유효하지 않은 인덱스입니다.");
        }
    }

    // 특정 몬스터를 몬스터 목록에서 제거합니다.
    public void RemoveMonster(Character monster)
    {
        if (Monsters.Contains(monster))
        {
            monster.RemoveFromRoom(); // 특정 몬스터를 방에서 제거합니다.
            int index = Monsters.IndexOf(monster); // 특정 몬스터의 인덱스를 찾습니다.

            // 해당 인덱스에 null을 설정하여 몬스터를 몬스터 목록에서 제거합니다.
            Monsters[index] = null;
        }
    }

    // 아이템을 놓습니다.
    public void PlaceItem(Item item)
    {
        this.Items = item;
    }

    // 아이템을 제거합니다.
    public void RemoveItem()
    {
        Items = Item.NONE;
    }

    // 룸에 몬스터가 있는지 확인하는 메서드
    public bool NoMonstersInRoom()
    {
        // Monsters 리스트가 null이거나 비어 있는 경우에는 몬스터가 없다고 판단합니다.
        if (Monsters == null || Monsters.Count == 0)
        {
            return true;
        }

        // Monsters 리스트의 모든 요소가 null인 경우에만 몬스터가 없다고 판단합니다.
        foreach (var monster in Monsters)
        {
            if (monster != null)
            {
                return false; // 하나라도 몬스터가 있다면 false 반환
            }
        }

        return true; // 모든 몬스터가 null인 경우 true 반환
    }

    // 인덱스를 통해 몬스터의 위치를 반환합니다.
    public int GetMonsterPosition(Character monster)
    {
        return Monsters.IndexOf(monster);
    }
}