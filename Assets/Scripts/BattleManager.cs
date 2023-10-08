using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 플레이어 객체
    private Player player;

    // 전투 UI 매니저
    private BattleUIManager UIManager;

    // 현재 방 인덱스
    public int currentRoom;

    // 오디오 재생 여부
    private bool playingAudio;

    // 전투 종료 여부
    public bool isEnd;

    // 현재 전투 상태
    public BattleState battleStates;

    // 용사 리스트
    public List<Character> heroes;

    // 현재 턴 (0: 몬스터 턴, 1: 용사 턴, -1: 전투 종료)
    public int Turn;

    // 수행 목록
    public List<GameObject> performList;

    // 전투 중인 몬스터 리스트
    public List<GameObject> monsterInBattle;

    // 전투 중인 용사 리스트
    public List<GameObject> heroesInBattle;

    // 몬스터 번호 리스트
    public List<int> monsterNumber;

    // 용사 번호 리스트
    public List<int> heroNumber;

    // 몬스터 전투력 리스트
    public List<int> monsterCps;

    // 용사 전투력 리스트
    public List<int> heroCps;

    // 공격 이펙트 게임 오브젝트들
    public GameObject fireEffect;
    public GameObject waterEffect;
    public GameObject windEffect;

    // 몬스터의 공격 이펙트 오브젝트 풀들
    public ObjectPool fireEffectPool;
    public ObjectPool waterEffectPool;
    public ObjectPool windEffectPool;

    // 몬스터, 보스, 용사 스포너 게임 오브젝트들
    public MonsterSpawner monsterSpawner;
    public BossSpawner bossSpawner;
    public HeroSpawner heroSpawner;

    // 몬스터 및 용사 재로딩 락
    public bool reloadMonsterLock;
    public bool reloadHeroLock;

    // 몬스터 전투력의 합 및 용사 전투력의 합
    public double sumMonsterCp;
    public double sumHeroCp;

    // 평균 몬스터 전투력 및 평균 용사 전투력
    public double avgHeroCp;
    public double avgMonsterCp;

    // 배경음, 승리음, 패배음 오디오 클립들
    public AudioClip backgroundSound;
    public AudioClip winSound;
    public AudioClip failSound;

    private void Awake()
    {
        // 초기화
        player = GameManager.s_Instance.player;

        heroes = new List<Character>();
        performList = new List<GameObject>();
        monsterInBattle = new List<GameObject>();
        heroesInBattle = new List<GameObject>();
        monsterNumber = new List<int>();
        heroNumber = new List<int>();
        monsterCps = new List<int>();
        heroCps = new List<int>();

        // 각각의 공격 이펙트 풀 초기화
        fireEffectPool = new ObjectPool(fireEffect, 1);
        waterEffectPool = new ObjectPool(waterEffect, 1);
        windEffectPool = new ObjectPool(windEffect, 1);
    }

    private void Start()
    {
        // 초기화
        InitializeBattle();
    }

    // 전투 초기화
    private void InitializeBattle()
    {
        // 전투 초기화 관련 변수 설정
        playingAudio = false;
        reloadMonsterLock = true;
        reloadHeroLock = true;
        isEnd = false;
        battleStates = BattleState.READY;
        Turn = 0;
        currentRoom = 0;
        UIManager = GetComponent<BattleUIManager>();
        GameManager.s_Instance.SetMusic(backgroundSound);

        // 몬스터 스포너에 현재 방 몬스터 설정
        monsterSpawner.Monsters = player.GetRoom(currentRoom).Monsters;

        // 용사 생성 및 스포너에 설정
        heroes = Enumerable.Range(0, 6).Select(_ => CharacterFactory.CreateHero(player.GetHeroRank())).ToList();
        heroSpawner.Heros = heroes;

        // 보스인 경우 보스 스포너에 설정, 아니면 몬스터 스포너 활성화
        if (player.GetRoom(currentRoom).Items == Item.CROWN)
        {
            bossSpawner.Boss = player.GetRoom(currentRoom).Monsters[0];
            bossSpawner.gameObject.SetActive(true);
        }
        else
            monsterSpawner.gameObject.SetActive(true);

        // 용사 스포너 활성화
        heroSpawner.gameObject.SetActive(true);

        CalculateCpSums();
        CalculateAvgCp();
    }

    private void Update()
    {
        // 몬스터와 용사 재로딩 락이 풀렸을 때
        if (!reloadMonsterLock && !reloadHeroLock && !isEnd)
        {
            // 전투 결과 확인
            CheckBattleOutcome();
        }
    }

    // 전투 결과 확인
    private void CheckBattleOutcome()
    {
        if (monsterInBattle.Count == 0)
        {
            currentRoom++;
            // 더이상 룸이 없다면, 그리고 룸에 몬스터가 없는 경우
            if (currentRoom > player.GetRoomCount() || player.GetRoom(currentRoom).NoMonstersInRoom())
                HandleBattleLoss();
            else
                ReloadInit(); // 다음 룸 몬스터 재로드
        }
        else if (heroesInBattle.Count == 0)
            HandleBattleWin();
        else
            ProcessBattleState();
    }

    // 전투 상태에 따라 처리
    private void ProcessBattleState()
    {
        switch (battleStates)
        {
            case BattleState.READY:
                break;
            case BattleState.WAIT:
                ProcessWaitState();
                break;
            case BattleState.TAKEACTION:
                ProcessTakeActionState();
                break;
            case BattleState.PERFORMACTION:
                break;
        }
    }

    // 대기 상태 처리
    private void ProcessWaitState()
    {
        // 수행 목록이 있으면 행동 상태로 전환
        if (performList.Count > 0)
        {
            battleStates = BattleState.TAKEACTION;
        }
    }

    // 행동 상태 처리
    private void ProcessTakeActionState()
    {
        // 수행 목록에서 첫 번째 행동자 가져오기
        GameObject performer = performList[0];
        if (performer.tag == "Monster")
        {
            // 몬스터 턴 처리
            ProcessMonsterTurn(performer);
        }
        else if (performer.tag == "Hero")
        {
            // 용사 턴 처리
            ProcessHeroTurn(performer);
        }
    }

    // 몬스터 턴 처리
    private void ProcessMonsterTurn(GameObject performer)
    {
        MonsterController monsterController = performer.GetComponent<MonsterController>();
        if (!monsterController.IsDead)
        {
            SetMonsterTarget(monsterController);
            monsterController.currentState = CharacterState.ACTION;
            battleStates = BattleState.PERFORMACTION;
            Turn = 1;
            MovePriority(monsterNumber);
        }
        else
        {
            // 몬스터가 이미 죽은 경우 수행 목록에서 제거하고 대기 상태로 전환
            performList.RemoveAt(0);
            battleStates = BattleState.WAIT;
        }
    }

    // 용사 턴 처리
    private void ProcessHeroTurn(GameObject performer)
    {
        HeroController heroController = performer.GetComponent<HeroController>();
        if (!heroController.IsDead)
        {
            SetHeroTarget(heroController);
            heroController.currentState = CharacterState.ACTION;
            battleStates = BattleState.PERFORMACTION;
            Turn = 0;
            MovePriority(heroNumber);
        }
        else
        {
            // 용사가 이미 죽은 경우 수행 목록에서 제거하고 대기 상태로 전환
            performList.RemoveAt(0);
            battleStates = BattleState.WAIT;
        }
    }

    // 몬스터의 타겟 설정
    private void SetMonsterTarget(MonsterController monsterController)
    {
        for (int i = 0; i < heroesInBattle.Count; i++)
        {
            HeroController target = heroesInBattle[i].GetComponent<HeroController>();
            int heroCpMax = heroCps.Max();
            if (target.GetCharacter().GetCP() == heroCpMax)
            {
                monsterController.targetObject = heroesInBattle[i];
                break;
            }
        }
    }

    // 용사의 타겟 설정
    private void SetHeroTarget(HeroController heroController)
    {
        for (int i = 0; i < monsterInBattle.Count; i++)
        {
            MonsterController target = monsterInBattle[i].GetComponent<MonsterController>();
            if (target.GetCharacter().GetCP() == monsterCps.Max())
            {
                heroController.targetObject = monsterInBattle[i];
                break;
            }
        }
    }

    // 우선순위 설정
    private void MovePriority(List<int> number)
    {
        int currentNumber = number[0];
        number.RemoveAt(0);
        number.Add(currentNumber);
    }

    // 승리 처리
    private void HandleBattleWin()
    {
        isEnd = true;
        if (!playingAudio)
        {
            GameManager.s_Instance.SetMusic(winSound);
            playingAudio = true;
        }
        avgHeroCp = sumHeroCp / 6;
        avgMonsterCp = sumMonsterCp / player.GetMonsterList().Count;

        // 승리 보상 적용
        AddPlayerRewardsOnWin((int)avgHeroCp, (int)avgMonsterCp);

        // 게임 종료 처리
        InitializeGame();
    }

    // 승리 보상 적용
    private void AddPlayerRewardsOnWin(int avgHeroCp, int avgMonsterCp)
    {
        int addExp = 1500 * (avgHeroCp / avgMonsterCp);
        int addGold = 1500 * (avgHeroCp / avgMonsterCp);
        int addInfamy = 300 * (avgHeroCp / avgMonsterCp);

        foreach (var monster in player.GetMonsterList())
        {
            monster.AddExp(addExp);
        }

        // UI 갱신 및 보스 몬스터 처리
        UIManager.SetWinText(addGold, addInfamy);
        UIManager.winUI.SetActive(true);

        player.AddGold(addGold);
        player.AddInfamy(addInfamy);
    }

    // 패배 처리
    private void HandleBattleLoss()
    {
        isEnd = true;
        if (!playingAudio)
        {
            GameManager.s_Instance.SetMusic(failSound);
            playingAudio = true;
        }
        avgHeroCp = sumHeroCp / 6;
        avgMonsterCp = sumMonsterCp / player.GetMonsterList().Count;

        // 패배 보상 적용
        AddPlayerRewardsOnLoss((int)avgHeroCp, (int)avgMonsterCp);

        // 게임 종료 처리
        InitializeGame();
    }

    // 패배 보상 적용
    private void AddPlayerRewardsOnLoss(int avgHeroCp, int avgMonsterCp)
    {
        int addExp = 1000 * (avgHeroCp / avgMonsterCp);
        int addGold = 800 * (avgHeroCp / avgMonsterCp);
        int addInfamy = -50 * (avgHeroCp / avgMonsterCp);

        foreach (var monster in player.GetMonsterList())
        {
            monster.AddExp(addExp);
        }

        // UI 갱신 및 보스 몬스터 처리
        UIManager.SetFailText(addGold, addInfamy);
        UIManager.failUI.SetActive(true);

        player.AddGold(addGold);
        player.AddInfamy(addInfamy);
    }

    // 게임 종료 처리
    private void InitializeGame()
    {
        // 각 방의 보스 몬스터 종료 및 아이템 제거
        for (int i = 0; i < player.GetRoomCount(); i++)
        {
            if (player.GetRoom(i).Items == Item.CROWN)
                player.GetRoom(i).Monsters[0].FinishBoss();
            player.GetRoom(i).RemoveItem();
        }
        ClearLists();
    }

    // 전투 초기화 및 재로딩
    private void ReloadInit()
    {
        DestroyCombatants();
        ClearLists();

        reloadMonsterLock = true;
        reloadHeroLock = true;
        Turn = 0;

        if (player.GetRoom(currentRoom).Items == Item.CROWN)
        {
            bossSpawner.Boss = player.GetRoom(currentRoom).Monsters[0];
            bossSpawner.gameObject.SetActive(true);
        }
        else
        {
            monsterSpawner.gameObject.SetActive(false);
            monsterSpawner.Monsters = player.GetRoom(currentRoom).Monsters;
            monsterSpawner.gameObject.SetActive(true);
        }

        heroSpawner.gameObject.SetActive(false);
        heroSpawner.Heros = heroes;
        heroSpawner.gameObject.SetActive(true);

        CalculateCpSums();
        CalculateAvgCp();
    }

    // 몬스터 전투력 및 용사 전투력의 합 계산
    private void CalculateCpSums()
    {
        sumMonsterCp = monsterCps.Sum();
        sumHeroCp = heroCps.Sum();
    }

    // 평균 몬스터 전투력 및 평균 용사 전투력 계산
    private void CalculateAvgCp()
    {
        avgHeroCp = sumHeroCp / heroes.Count;
        avgMonsterCp = sumMonsterCp / monsterInBattle.Count;
    }

    // 전투 중인 몬스터와 용사 제거
    private void DestroyCombatants()
    {
        GameObject[] removeMonster = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] removeHero = GameObject.FindGameObjectsWithTag("Hero");

        foreach (var monster in removeMonster)
        {
            Destroy(monster);
        }

        foreach (var hero in removeHero)
        {
            Destroy(hero);
        }
    }

    // 리스트 초기화
    private void ClearLists()
    {
        performList.Clear();
        monsterInBattle.Clear();
        heroesInBattle.Clear();
        monsterNumber.Clear();
        heroNumber.Clear();
        monsterCps.Clear();
        heroCps.Clear();
    }
}
