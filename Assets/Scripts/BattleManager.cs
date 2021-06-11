using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    private int currentRoom;
    private Player player;
    public Character[] heroes = new Character[6];

    public enum PerformAction
    {
        READY,
        WAIT,
        TAKEACTION,
        PERFORMACTION 
    }
    public PerformAction battleStates;
    public int Turn; // -1: 전투 종료, 0: 몬스터 턴, 1: 용사 턴
    public List<GameObject> performList = new List<GameObject>();
    public List<GameObject> monsterInBattle = new List<GameObject>();
    public List<GameObject> heroesInBattle = new List<GameObject>();
    public List<int> monsterNumber = new List<int>();
    public List<int> heroNumber = new List<int>();
    public List<int> monsterCps = new List<int>();
    public List<int> heroCps = new List<int>();
    public GameObject fireEffect;
    public GameObject waterEffect;
    public GameObject windEffect;
    public GameObject gameWinUI;
    public GameObject gameOverUI;
    public GameObject monsterSpawner;
    public GameObject heroSpawner;
    public bool reloadMonsterLock = false;
    public bool reloadHeroLock = false;

    private void Awake()
    {
        player = GameManager.instance.Player;
        currentRoom = 0;

        monsterSpawner.GetComponent<MonsterSpawner>().SetMonster(player.GetRoom(currentRoom).Monsters);

        int evilPoint = player.GetEvilPoint();
        double rank = evilPoint / Math.Pow(10, Math.Log(evilPoint) + 2);
        for (int i = 0; i < 6; i++)
        {
            heroes[i] = CharacterFactory.CreateHero((int)rank);
        }
        heroSpawner.GetComponent<HeroSpawner>().SetHeroes(heroes);

        monsterSpawner.SetActive(true);
        heroSpawner.SetActive(true);

    }

    private void Start()
    {
        battleStates = PerformAction.READY;
        Turn = 0;
    }

    private void Update()
    {
        if (!reloadMonsterLock && !reloadHeroLock)
        {
            // 몬스터가 다 죽은 경우
            if (monsterInBattle.Count == 0)
            {
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
                Turn = -1;
                for (int i = 0; i < monsterInBattle.Count; i++)
                {
                    monsterInBattle[i].GetComponent<MonsterController>().animatorController.Victory();
                }
                GameWin();
            }
            else
            {
                switch (battleStates)
                {
                    case (PerformAction.READY):
                        break;
                    case (PerformAction.WAIT):
                        if (performList.Count > 0)
                        {
                            battleStates = PerformAction.TAKEACTION;
                        }
                        break;
                    case (PerformAction.TAKEACTION):
                        GameObject performer = performList[0];
                        if (performer.tag == "Monster")
                        {
                            MonsterController MonsterController = performer.GetComponent<MonsterController>();
                            if (!MonsterController.GetIsDead())
                            {
                                for (int i = 0; i < heroesInBattle.Count; i++)
                                {
                                    if (heroesInBattle[i].GetComponent<HeroController>().GetCharacter().GetCP() == heroCps.Max())
                                    {
                                        MonsterController.targetObject = heroesInBattle[i];
                                        break;
                                    }
                                }
                                MonsterController.currentState = CharacterState.ACTION;
                                battleStates = PerformAction.PERFORMACTION;
                                Turn = 1;
                                MovePriority(monsterNumber);
                            }
                            else
                            {
                                performList.RemoveAt(0);
                                battleStates = PerformAction.WAIT;
                            }
                        }
                        else if (performer.tag == "Hero")
                        {
                            HeroController HeroController = performer.GetComponent<HeroController>();
                            if (!HeroController.GetIsDead())
                            {
                                for (int i = 0; i < monsterInBattle.Count; i++)
                                {
                                    if (monsterInBattle[i].GetComponent<MonsterController>().GetCharacter().GetCP() == monsterCps.Max())
                                    {
                                        HeroController.targetObject = monsterInBattle[i];
                                        break;
                                    }
                                }
                                HeroController.currentState = CharacterState.ACTION;
                                battleStates = PerformAction.PERFORMACTION;
                                Turn = 0;
                                MovePriority(heroNumber);
                            }
                            else
                            {
                                performList.RemoveAt(0);
                                battleStates = PerformAction.WAIT;
                            }
                        }
                        break;
                    case (PerformAction.PERFORMACTION):
                        // idle
                        break;
                }
            }
        }
    }
    
    public void CollectActions(GameObject input)
    {
        performList.Add(input);
    }

    private void MovePriority(List<int> number)
    {
        // 맨 뒤 순서로 이동
        int currentNumber = number[0];
        number.RemoveAt(0);
        number.Add(currentNumber);
    }

    private void GameWin()
    {
        gameWinUI.SetActive(true);
        // TODO: 경험치, 악명, 골드 획득

    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        // TODO: 경험치, 악명, 골드 획득

    }

    private void ReloadInit()
    {
        reloadMonsterLock = true;
        reloadHeroLock = true;

        Turn = 0;

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

        monsterSpawner.SetActive(false);
        monsterSpawner.GetComponent<MonsterSpawner>().SetMonster(player.GetRoom(currentRoom).Monsters);
        monsterSpawner.SetActive(true);
        heroSpawner.SetActive(false);
        heroSpawner.GetComponent<HeroSpawner>().SetHeroes(heroes);
        heroSpawner.SetActive(true);
    }

    public void GetReady() {
        battleStates = PerformAction.WAIT;
    }
}
