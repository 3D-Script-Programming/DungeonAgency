using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 현재 방 인덱스
    private int currentRoom;

    // 플레이어 객체
    private Player player;

    // 전투 UI 매니저
    private BattleUIManager UIManager;

    // 오디오 재생 여부
    private bool playingAudio = false;

    // 전투 종료 여부
    private bool isEnd;

    // 현재 전투 상태
    public BattleState battleStates;

    // 용사 리스트
    public List<Character> heroes = new List<Character>();

    // 현재 턴 (0: 몬스터 턴, 1: 용사 턴, -1: 전투 종료)
    public int Turn;

    // 수행 목록
    public List<GameObject> performList = new List<GameObject>();

    // 전투 중인 몬스터 리스트
    public List<GameObject> monsterInBattle = new List<GameObject>();

    // 전투 중인 용사 리스트
    public List<GameObject> heroesInBattle = new List<GameObject>();

    // 몬스터 번호 리스트
    public List<int> monsterNumber = new List<int>();

    // 용사 번호 리스트
    public List<int> heroNumber = new List<int>();

    // 몬스터 전투력 리스트
    public List<int> monsterCps = new List<int>();

    // 용사 전투력 리스트
    public List<int> heroCps = new List<int>();

    // 공격 이펙트 게임 오브젝트들
    public GameObject fireEffect;
    public GameObject waterEffect;
    public GameObject windEffect;

    // 몬스터, 보스, 용사 스포너 게임 오브젝트들
    public MonsterSpawner monsterSpawner;
    public BossSpawner bossSpawner;
    public HeroSpawner heroSpawner;

    // 몬스터 및 용사 재로딩 락
    public bool reloadMonsterLock = true;
    public bool reloadHeroLock = true;

    // 몬스터 전투력의 합 및 용사 전투력의 합
    public double sumMonsterCp = 0;
    public double sumHeroCp = 0;

    // 평균 몬스터 전투력 및 평균 용사 전투력
    public double avgHeroCp;
    public double avgMonsterCp;

    // 배경음, 승리음, 패배음 오디오 클립들
    public AudioClip backgroundSound;
    public AudioClip winSound;
    public AudioClip failSound;

    // 초기화
    private void Awake()
    {
        // 플레이어와 현재 방 설정
        player = GameManager.s_Instance.player;
        currentRoom = 0;

        // 몬스터 스포너에 현재 방 몬스터 설정
        monsterSpawner.Monsters = player.GetRoom(currentRoom).Monsters;

        // 용사 생성 및 스포너에 설정
        for (int i = 0; i < 6; i++)
        {
            heroes.Add(CharacterFactory.CreateHero(player.GetHeroRank()));
        }
        heroSpawner.Heros = heroes;

        // 보스인 경우 보스 스포너에 설정, 아니면 몬스터 스포너 활성화
        if (player.GetRoom(currentRoom).Items == Item.CROWN)
        {
            bossSpawner.Boss = player.GetRoom(currentRoom).Monsters[0];
            bossSpawner.gameObject.SetActive(true);
        }
        else
        {
            monsterSpawner.gameObject.SetActive(true);
        }

        // 용사 스포너 활성화
        heroSpawner.gameObject.SetActive(true);
    }

    // 시작
    private void Start()
    {
        // 초기화
        isEnd = false;
        battleStates = BattleState.READY;
        Turn = 0;
        UIManager = gameObject.GetComponent<BattleUIManager>();
        GameManager.s_Instance.SetMusic(backgroundSound);
    }

    // 업데이트
    private void Update()
    {
        // 몬스터와 용사 재로딩 락이 풀렸을 때
        if (!reloadMonsterLock && !reloadHeroLock)
        {
            // 전투 종료 체크
            if (monsterInBattle.Count == 0)
            {
                // 다음 방으로 이동 또는 전투 종료
                currentRoom++;
                if (currentRoom < player.GetRoomCount())
                {
                    ReloadInit();
                }
                else
                {
                    Turn = -1;
                    for (int i = 0; i < heroesInBattle.Count; i++)
                    {
                        heroesInBattle[i].GetComponent<HeroController>().animatorController.Victory();
                    }
                    GameOver();
                }
            }
            else if (heroesInBattle.Count == 0)
            {
                // 전투 승리 처리
                Turn = -1;
                for (int i = 0; i < monsterInBattle.Count; i++)
                {
                    monsterInBattle[i].GetComponent<MonsterController>().animatorController.Victory();
                }
                GameWin();
            }
            else
            {
                // 전투 상태에 따라 처리
                switch (battleStates)
                {
                    case (BattleState.READY):
                        break;
                    case (BattleState.WAIT):
                        // 수행 목록이 있으면 행동을 취하도록 상태 변경
                        if (performList.Count > 0)
                        {
                            battleStates = BattleState.TAKEACTION;
                        }
                        break;
                    case (BattleState.TAKEACTION):
                        // 수행 목록의 첫 번째 객체 가져오기
                        GameObject performer = performList[0];
                        if (performer.tag == "Monster")
                        {
                            // 몬스터의 턴 처리
                            MonsterController MonsterController = performer.GetComponent<MonsterController>();
                            if (!MonsterController.IsDead)
                            {
                                // 전투 중인 용사 중 가장 높은 전투력을 가진 용사를 찾아서 몬스터에게 타겟으로 설정
                                for (int i = 0; i < heroesInBattle.Count; i++)
                                {
                                    HeroController target = heroesInBattle[i].GetComponent<HeroController>();
                                    int heroCpMax = heroCps.Max();
                                    if (target.GetCharacter().GetCP() == heroCpMax)
                                    {
                                        MonsterController.targetObject = heroesInBattle[i];
                                        break;
                                    }
                                }
                                MonsterController.currentState = CharacterState.ACTION;
                                battleStates = BattleState.PERFORMACTION;
                                Turn = 1;
                                MovePriority(monsterNumber);
                            }
                            else
                            {
                                // 몬스터가 이미 죽었으면 수행 목록에서 제거하고 대기 상태로 변경
                                performList.RemoveAt(0);
                                battleStates = BattleState.WAIT;
                            }
                        }
                        else if (performer.tag == "Hero")
                        {
                            // 용사의 턴 처리
                            HeroController HeroController = performer.GetComponent<HeroController>();
                            if (!HeroController.IsDead)
                            {
                                // 전투 중인 몬스터 중 가장 높은 전투력을 가진 몬스터를 찾아서 타겟으로 설정
                                for (int i = 0; i < monsterInBattle.Count; i++)
                                {
                                    MonsterController target = monsterInBattle[i].GetComponent<MonsterController>();
                                    if (target.GetCharacter().GetCP() == monsterCps.Max())
                                    {
                                        HeroController.targetObject = monsterInBattle[i];
                                        break;
                                    }
                                }
                                HeroController.currentState = CharacterState.ACTION;
                                battleStates = BattleState.PERFORMACTION;
                                Turn = 0;
                                MovePriority(heroNumber);
                            }
                            else
                            {
                                // 용사가 이미 죽었으면 수행 목록에서 제거하고 대기 상태로 변경
                                performList.RemoveAt(0);
                                battleStates = BattleState.WAIT;
                            }
                        }
                        break;
                    case (BattleState.PERFORMACTION):
                        // 아무 동작하지 않음 (대기 상태)
                        break;
                }
            }
        }
    }

    // 플레이어가 수행할 동작을 수집하는 함수
    public void CollectActions(GameObject input)
    {
        performList.Add(input);
    }

    // 우선순위를 변경하는 함수
    private void MovePriority(List<int> number)
    {
        // 맨 뒤 순서로 이동
        int currentNumber = number[0];
        number.RemoveAt(0);
        number.Add(currentNumber);
    }

    // 게임 승리 처리 함수
    private void GameWin()
    {
        if (isEnd) return;
        isEnd = true;
        if (!playingAudio)
        {
            GameManager.s_Instance.SetMusic(winSound);
            playingAudio = true;
        }
        avgHeroCp = sumHeroCp / 6;
        avgMonsterCp = sumMonsterCp / player.GetMonsterList().Count;

        // 전투 종료 시 플레이어에게 경험치, 골드, 악명을 추가
        int addExp = (int)(1500 * (avgHeroCp / avgMonsterCp));
        int addGold = (int)(1500 * (avgHeroCp / avgMonsterCp));
        int addInfamy = (int)(300 * (avgHeroCp / avgMonsterCp));

        // 각 몬스터에게 경험치 추가
        for (int i = 0; i < player.GetMonsterList().Count; i++)
        {
            player.GetMonster(i).AddExp(addExp);
        }

        // 플레이어에게 골드 및 악명 추가
        player.AddGold(addGold);
        player.AddInfamy(addInfamy);

        // UI에 승리 정보 설정 및 활성화
        UIManager.SetWinText(addGold, addInfamy);
        UIManager.winUI.SetActive(true);

        // 각 방의 보스 몬스터 종료 및 아이템 제거
        for (int i = 0; i < player.GetRoomCount(); i++)
        {
            if (player.GetRoom(i).Items == Item.CROWN)
                player.GetRoom(i).Monsters[0].FinishBoss();
            player.GetRoom(i).PlaceItem(Item.NONE);
        }
    }

    // 게임 패배 처리 함수
    private void GameOver()
    {
        if (isEnd) return;
        isEnd = true;
        if (!playingAudio)
        {
            GameManager.s_Instance.SetMusic(failSound);
            playingAudio = true;
        }
        avgHeroCp = sumHeroCp / 6;
        avgMonsterCp = sumMonsterCp / player.GetMonsterList().Count;

        // 전투 종료 시 플레이어에게 경험치, 골드, 악명을 추가 (패배 시 악명 감소)
        int addExp = (int)(1000 * (avgHeroCp / avgMonsterCp));
        int addGold = (int)(800 * (avgHeroCp / avgMonsterCp));
        int addInfamy = (int)(-50 * (avgHeroCp / avgMonsterCp));

        // 각 몬스터에게 경험치 추가
        for (int i = 0; i < player.GetMonsterList().Count; i++)
        {
            player.GetMonster(i).AddExp(addExp);
        }

        // 플레이어에게 골드 및 악명 추가
        player.AddGold(addGold);
        player.AddInfamy(addInfamy);

        // UI에 패배 정보 설정 및 활성화
        UIManager.SetFailText(addGold, addInfamy);
        UIManager.failUI.SetActive(true);

        // 각 방의 보스 몬스터 종료 및 아이템 제거
        for (int i = 0; i < player.GetRoomCount(); i++)
        {
            if (player.GetRoom(i).Items == Item.CROWN)
                player.GetRoom(i).Monsters[0].FinishBoss();
            player.GetRoom(i).PlaceItem(Item.NONE);
        }
    }

    // 전투 초기화 함수
    private void ReloadInit()
    {
        reloadMonsterLock = true;
        reloadHeroLock = true;

        Turn = 0;

        // 수행 목록, 전투 중인 용사 및 몬스터 초기화
        int performListCount = performList.Count;
        for (int i = 0; i < performListCount; i++)
        {
            performList.RemoveAt(0);
        }

        int heroInBattleCount = heroesInBattle.Count;
        for (int i = 0; i < heroInBattleCount; i++)
        {
            heroesInBattle.RemoveAt(0);
        }

        int heroNumberCount = heroNumber.Count;
        for (int i = 0; i < heroNumberCount; i++)
        {
            heroNumber.RemoveAt(0);
        }

        int heroCpsCount = heroCps.Count;
        for (int i = 0; i < heroCpsCount; i++)
        {
            heroCps.RemoveAt(0);
        }

        // 전투 중인 몬스터 및 용사 삭제
        GameObject[] removeMonster = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] removeHero = GameObject.FindGameObjectsWithTag("Hero");
        for (int i = 0; i < removeMonster.Length; i++)
        {
            Destroy(removeMonster[i]);
        }
        for (int i = 0; i < removeHero.Length; i++)
        {
            Destroy(removeHero[i]);
        }

        // 다음 방이 보스인 경우 보스 스포너에 설정, 아니면 몬스터 스포너 활성화
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

        // 용사 스포너에 용사 설정 및 활성화
        heroSpawner.gameObject.SetActive(false);
        heroSpawner.Heros = heroes;
        heroSpawner.gameObject.SetActive(true);
    }

    // 전투 시작을 알리는 함수
    public void GetReady()
    {
        battleStates = BattleState.WAIT;
    }
}

