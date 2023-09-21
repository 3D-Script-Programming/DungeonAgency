using System;
using System.Collections.Generic;

public static class CharacterFactory
{
    // 랜덤 숫자 생성기
    private static readonly Random random = new Random();

    // 몬스터 생성 메서드
    public static Character CreateMonster(int rank = -1)
    {
        // rank 매개변수가 -1이면 랜덤한 rank로 생성
        return CreateCharacter(true, rank);
    }

    // 용사(Enemy) 생성 메서드
    public static Character CreateHero(int rank = -1)
    {
        // rank 매개변수가 -1이면 랜덤한 rank로 생성
        return CreateCharacter(false, rank);
    }

    // 몬스터 리스트 생성 메서드 (여러 랭크에 대한 리스트 생성)
    public static List<Character> CreateMonsterList(params int[] ranks)
    {
        return CreateCharacterList(true, ranks);
    }

    // 용사(Enemy) 리스트 생성 메서드 (여러 랭크에 대한 리스트 생성)
    public static List<Character> CreateHeroList(params int[] ranks)
    {
        return CreateCharacterList(false, ranks);
    }

    // 몬스터 리스트 생성 메서드 (개수 지정)
    public static List<Character> CreateMonsterList(int count)
    {
        return CreateCharacterList(true, count);
    }

    // 용사(Enemy) 리스트 생성 메서드 (개수 지정)
    public static List<Character> CreateHeroList(int count)
    {
        return CreateCharacterList(false, count);
    }

    // 캐릭터 생성 메서드
    private static Character CreateCharacter(bool isMonster, int rank)
    {
        // rank가 -1인 경우, 랜덤한 rank를 생성
        if (rank == -1)
        {
            rank = random.Next(21);
        }

        // 능력치 계산
        int statusSum = (int)Math.Pow(1.2, rank) * 50;
        int defaultStatus = statusSum * 2 / 5;
        statusSum -= defaultStatus;
        int str = random.Next(statusSum) + 1;
        int bal = random.Next(statusSum - str) + 1;
        int vtp = statusSum - bal;

        // 잠재력 계산
        int potential = 10 - (int)Math.Log(random.Next(2048) + 1, 2);

        // 속성 설정
        Nature nature = (Nature)Enum.ToObject(typeof(Nature), random.Next(Enum.GetValues(typeof(Nature)).Length));

        // 프리팹 경로 설정
        string prefabPath = GetPrefabPath(isMonster, random.Next(isMonster ? Enum.GetValues(typeof(Monster)).Length : Enum.GetValues(typeof(Hero)).Length));

        // '/' 문자를 기준으로 분할하여 마지막 부분을 추출하여 name 변수에 할당
        string[] pathParts = prefabPath.Split('/');
        string name = pathParts[pathParts.Length - 1];

        // 캐릭터 생성 및 반환
        return new Character(name, prefabPath, str, bal, vtp, potential, nature);
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
        // 몬스터 또는 용사(Enemy) 폴더와 이름을 결합하여 프리팹 경로 생성
        string type = isMonster ? "Monster" : "Hero";
        string[] names = isMonster ? Enum.GetNames(typeof(Monster)) : Enum.GetNames(typeof(Hero));
        return $"{type}/{names[prefabCode]}";
    }
}