using System;
using UnityEngine;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class Character
{
    // 캐릭터의 프리팹과 속성들을 프로퍼티로 정의4
    public string Name { get; private set; }
    public GameObject Prefab { get; private set; }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int Strength { get; private set; }
    public int Balance { get; private set; }
    public int Vitality { get; private set; }
    public Nature Nature { get; private set; }
    public int HP { get; private set; }
    public int Potential { get; private set; }

    // 캐릭터 고유 번호 
    public Guid UniqueID { get; private set; }

    // 현재 배치된 룸 넘버 : -1은 배치되어 있지 않음.
    public int CurrentRoomNumber { get; private set; }

    // 보스 상태 및 일반 상태에서의 능력치 배율을 상수로 정의
    private const float BossMultiplier = 4.0f;
    private const float NormalMultiplier = 1.0f;

    // 데미지 계산식에 적용할 배율 상수 정의
    private const int MaxDamageMultiplier = 15;
    private const int MinDamageDenominator = 20;

    // 생성자: 캐릭터 초기화
    public Character(string name, string prefabPath, int strength, int balance, int vitality, int potential, Nature nature)
    {
        Name = name;
        CurrentRoomNumber = -1;
        Level = 1;
        Exp = 0;
        Strength = strength;
        Balance = balance;
        Vitality = vitality;
        Nature = nature;
        Potential = potential;
        HP = GetMaxHP();
        Prefab = Resources.Load<GameObject>(prefabPath);

        UniqueID = Guid.NewGuid(); // 새로운 고유한 ID 생성
    }

    // 생성자 오버로드: 특정 레벨로 캐릭터 초기화
    public Character(string name, string prefabPath, int level, int strength, int balance, int vitality, int potential, Nature nature)
        : this(name, prefabPath, strength, balance, vitality, potential, nature)
    {
        LevelUp(level);
    }

    // 캐릭터 이름을 추출하는 유틸리티 메서드
    private string GetCharacterName(string prefabPath)
    {
        string[] pathComponents = prefabPath.Split('/');
        string nameWithoutDigits = Regex.Replace(pathComponents[1], @"\d", "");
        return nameWithoutDigits;
    }

    // 캐릭터 레벨 업
    public void LevelUp()
    {
        if (GetRequiredExp() > Exp)
            return;

        Level++;
        int statusSum = (int)(Math.Log10(100 + Level) * 15 + Potential * 1.2);
        int statusIncrement = statusSum / 3;
        Strength += statusIncrement;
        Vitality += statusIncrement;
        Balance += statusIncrement;
        HP = GetMaxHP();
        Exp = 0;
    }

    // 특정 레벨로 캐릭터 레벨 업
    public void LevelUp(int level)
    {
        while (Level < level)
        {
            Exp = GetRequiredExp();
            LevelUp();
        }
    }

    // 캐릭터의 전투력(CP) 계산
    public int GetCP()
    {
        return Strength * 8 + Balance * 3 + Vitality * 4;
    }

    // 필요한 경험치 계산
    public int GetRequiredExp()
    {
        int levelWeight = (int)Math.Pow(Level, 1.3);
        return levelWeight * (200 + (Potential * 80) * (Level + 5) / 10);
    }

    // 최대 체력 계산
    public int GetMaxHP()
    {
        return Vitality * 50;
    }

    // 캐릭터가 공격을 받을 때 체력 감소 처리
    public void GetHit(int damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
        }
    }

    // 랜덤한 데미지 값 반환
    public int GetDamage()
    {
        int maxDamage = Strength * MaxDamageMultiplier;
        int minDamage = maxDamage / (1 + (int)(Math.Log(Balance, 2) * MinDamageDenominator));

        return Random.Range(minDamage, maxDamage);
    }

    // 최대 데미지 값 반환
    public int GetMaxDamage()
    {
        return Strength * MaxDamageMultiplier;
    }

    // 캐릭터의 가격 계산
    public int GetPrice()
    {
        return (int)Math.Pow(GetCP(), 1.2);
    }

    // 보스 상태로 설정 (능력치 배율 적용)
    public void SetBoss()
    {
        MultiplyStats(BossMultiplier);
    }

    // 일반 상태로 설정 (능력치 배율 복원)
    public void FinishBoss()
    {
        MultiplyStats(NormalMultiplier);
    }

    // 능력치를 주어진 배율만큼 조정
    private void MultiplyStats(float multiplier)
    {
        Strength = Mathf.RoundToInt(Strength * multiplier);
        Balance = Mathf.RoundToInt(Balance * multiplier);
        Vitality = Mathf.RoundToInt(Vitality * multiplier);
    }

    // 경험치 추가 및 레벨 업 처리
    public void AddExp(int addExp)
    {
        int requiredExp = GetRequiredExp();
        Exp += addExp;
        while (requiredExp < Exp)
        {
            int remainingExp = Exp - requiredExp;
            LevelUp();
            requiredExp = GetRequiredExp();
            Exp += remainingExp;
        }
    }

    // 체력을 최대값으로 재설정
    public void SetResetHp()
    {
        HP = GetMaxHP();
    }

    // 몬스터를 특정 던전 방에 배치하는 메서드
    public void PlaceInRoom(int roomNumber)
    {
        CurrentRoomNumber = roomNumber;
    }

    // 몬스터를 던전 방에서 제거하는 메서드
    public void RemoveFromRoom()
    {
        CurrentRoomNumber = -1;
    }
}