using System;
using System.Collections.Generic;

public static class CharacterFactory
{
    // 랜덤 숫자 생성기
    private static readonly Random random = new Random();

    // 몬스터 생성 메서드
    public static Character CreateMonster(int rank)
    {
        return CreateCharacter(true, rank);
    }

    // 영웅 생성 메서드
    public static Character CreateHero(int rank)
    {
        return CreateCharacter(false, rank);
    }

    // 랜덤 몬스터 생성 메서드
    public static Character CreateMonster()
    {
        return CreateCharacter(true, random.Next(21));
    }

    // 랜덤 영웅 생성 메서드
    public static Character CreateHero()
    {
        return CreateCharacter(false, random.Next(21));
    }

    // 몬스터 리스트 생성 메서드
    public static List<Character> CreateMonsterList(params int[] ranks)
    {
        return CreateCharacterList(true, ranks);
    }

    // 영웅 리스트 생성 메서드
    public static List<Character> CreateHeroList(params int[] ranks)
    {
        return CreateCharacterList(false, ranks);
    }

    // 몬스터 리스트 생성 메서드 (개수 지정)
    public static List<Character> CreateMonsterList(int count)
    {
        return CreateCharacterList(true, count);
    }

    // 영웅 리스트 생성 메서드 (개수 지정)
    public static List<Character> CreateHeroList(int count)
    {
        return CreateCharacterList(false, count);
    }

    // 캐릭터 생성 메서드
    private static Character CreateCharacter(bool isMonster, int rank)
    {
        // 능력치 계산
        int statusSum = (int)Math.Pow(1.2, rank) * 50;
        int defaultStatus = statusSum * 2 / 5;
        statusSum -= defaultStatus;
        int str = random.Next(statusSum) + 1;
        int bal = random.Next(statusSum - str) + 1;
        int vtp = statusSum - bal;

        // 잠재력 계산
        int potential = 10 - (int)Math.Log(random.Next(2048) + 1, 2);

        // 자연 속성 설정
        Nature nature = (Nature)Enum.ToObject(typeof(Nature), random.Next(Enum.GetValues(typeof(Nature)).Length));

        // 프리팹 경로 설정
        string prefabPath = GetPrefabPath(isMonster, random.Next(Enum.GetValues(typeof(Monster)).Length));

        // 캐릭터 생성 및 반환
        return new Character(prefabPath, str, bal, vtp, potential, nature);
    }

    // 캐릭터 리스트 생성 메서드 (여러 랭크에 대한 리스트 생성)
    private static List<Character> CreateCharacterList(bool isMonster, params int[] ranks)
    {
        List<Character> characters = new List<Character>();
        foreach (int rank in ranks)
        {
            characters.Add(CreateCharacter(isMonster, rank));
        }
        return characters;
    }

    // 캐릭터 리스트 생성 메서드 (개수 지정)
    private static List<Character> CreateCharacterList(bool isMonster, int count)
    {
        List<Character> characters = new List<Character>();
        for (int i = 0; i < count; i++)
        {
            characters.Add(CreateCharacter(isMonster, random.Next(21)));
        }
        return characters;
    }

    // 프리팹 경로 생성 메서드
    private static string GetPrefabPath(bool isMonster, int prefabCode)
    {
        if (isMonster)
            return "Monster/" + ((Monster)Enum.ToObject(typeof(Monster), prefabCode)).ToString();
        else
            return "Hero/" + ((Hero)Enum.ToObject(typeof(Hero), prefabCode)).ToString();
    }
}