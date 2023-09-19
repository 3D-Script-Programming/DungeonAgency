using System;
using System.Collections.Generic;

public class DungeonRoom
{
    private List<Character> monsters = new List<Character>(6); // 전열(0, 1, 2) 및 후열(3, 4, 5)
    private Item item = Item.NONE;

    // Monsters를 반환하는 프로퍼티입니다.
    public List<Character> Monsters => monsters;

    // 아이템 프로퍼티
    public Item Item { get => item; set => item = value; }

    // 인덱스를 통해 몬스터를 추가합니다.
    public void PlaceMonster(int index, Character monster)
    {
        if (index >= 0 && index < monsters.Count)
        {
            monsters[index] = monster; // 유효한 인덱스 내에서 몬스터를 추가합니다.
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
        if (index >= 0 && index < 6)
        {
            monsters[index] = null; // 유효한 인덱스 내에서 몬스터를 제거합니다.
        }
    }

    // 특정 몬스터를 몬스터 목록에서 제거합니다.
    public void RemoveMonster(Character monster)
    {
        monsters.Remove(monster);
    }

    // 아이템을 놓습니다.
    public void PlaceItem(Item item)
    {
        this.item = item;
    }

    // 아이템을 제거합니다.
    public void RemoveItem()
    {
        item = Item.NONE;
    }

    // 현재 아이템을 가져옵니다.
    public Item GetItem()
    {
        return item;
    }
}