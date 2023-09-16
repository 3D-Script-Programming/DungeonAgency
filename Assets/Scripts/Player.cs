using System;
using UnityEngine;
using System.Collections.Generic;

public class Player
{
    // 플레이어의 골드 속성
    public int Gold { get; private set; } = 0;

    // 플레이어의 악명 속성
    public int Infamy { get; private set; } = 1;

    // 아이템 수량 배열 속성 (index 0: 보스왕관 개수, index 1: 보물상자 개수)
    public int[] Items { get; private set; } = { 0, 0 };

    // 몬스터 리스트 속성
    public List<Character> Monsters { get; private set; } = new List<Character>();

    // 던전 방 리스트 속성
    public List<DungeonRoom> Dungeon { get; private set; } = new List<DungeonRoom>
    {
        new DungeonRoom(),
        new DungeonRoom(),
        new DungeonRoom()
    };

    // 특정 인덱스의 던전 방 반환
    public DungeonRoom GetRoom(int index)
    {
        return (index >= 0 && index < Dungeon.Count) ? Dungeon[index] : null;
    }

    // 던전 방의 개수 반환
    public int GetRoomCount()
    {
        return Dungeon.Count;
    }

    // 특정 인덱스의 몬스터 반환
    public Character GetMonster(int index)
    {
        return (index >= 0 && index < Monsters.Count) ? Monsters[index] : null;
    }

    // 몬스터 리스트 반환
    public List<Character> GetMonsterList()
    {
        return Monsters.Count > 0 ? Monsters : null;
    }

    // 아이템 추가 메서드
    public void AddItem(Item item, int count)
    {
        if (item == Item.CROWN)
            Items[0] += count;
        if (item == Item.TREASURE)
            Items[1] += count;
    }

    // 아이템 수량 반환 메서드
    public int GetItem(Item item)
    {
        return item == Item.CROWN ? Items[0] : (item == Item.TREASURE ? Items[1] : 0);
    }

    // 크라운 사용 메서드
    public void UseCrown()
    {
        Items[0]--;
    }

    // 크라운 사용 취소 메서드
    public void UnuseCrown()
    {
        Items[0]++;
    }

    // 보물 사용 메서드
    public void UseTreasure()
    {
        Items[1]--;
    }

    // 보물 사용 취소 메서드
    public void UnuseTreasure()
    {
        Items[1]++;
    }

    // 몬스터 추가 메서드
    public void AddMonster(Character monster)
    {
        Monsters.Add(monster);
    }

    // 몬스터 리스트 추가 메서드
    public void AddRangeMonster(List<Character> monsters)
    {
        Monsters.AddRange(monsters);
    }

    // 특정 인덱스의 몬스터 제거 메서드
    public void RemoveMonster(int index)
    {
        if (index >= 0 && index < Monsters.Count)
            Monsters.RemoveAt(index);
    }

    // 던전에 방 추가 메서드
    public void AddRoom()
    {
        Dungeon.Add(new DungeonRoom());
    }

    // 용사(Enemy) 랭크 계산 메서드
    public int GetHeroRank()
    {
        int count = 0;
        foreach (var room in Dungeon)
        {
            if (room.Item == Item.TREASURE)
            {
                count += 500;
            }
        }

        double rank = (Infamy + count) / Math.Pow(10, Math.Log(Infamy + count) + 2);
        return (int)rank;
    }

    // 골드 추가 메서드
    public void AddGold(int gold)
    {
        Gold += gold;
    }

    // 악명 포인트 추가 메서드
    public void AddInfamy(int infamy)
    {
        Infamy += infamy;
        if (Infamy <= 0)
        {
            Infamy = 1;
        }
    }
}