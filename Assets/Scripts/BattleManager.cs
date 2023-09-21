using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private int currentRoom;
    private Player player;
    private UIBattleManager UI;
    private bool playingAudio = false;

    private bool isEnd;

    public enum PerformAction
    {
        READY,
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }
    public PerformAction battleStates;
    public Character[] heroes = new Character[6];
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
    public GameObject monsterSpawner;
    public GameObject bossSpawner;
    public GameObject heroSpawner;
    public bool reloadMonsterLock = true;
    public bool reloadHeroLock = true;
    public double sumMonsterCp = 0;
    public double sumHeroCp = 0;
    public double avgHeroCp;
    public double avgMonsterCp;
    public AudioClip backgroundSound;
    public AudioClip winSound;
    public AudioClip failSound;

    private void Awake()
    {
        player = GameManager.s_Instance.player;
        currentRoom = 0;

        monsterSpawner.GetComponent<MonsterSpawner>().SetMonster(player.GetRoom(currentRoom).Monsters);

        for (int i = 0; i < 6; i++)
        {
            heroes[i] = CharacterFactory.CreateHero(player.GetHeroRank());
        }
        heroSpawner.GetComponent<HeroSpawner>().SetHeroes(heroes);

        if (player.GetRoom(currentRoom).Items == Item.CROWN)
        {
            bossSpawner.GetComponent<BossSpawner>().SetBoss(player.GetRoom(currentRoom).Monsters[0]);
            bossSpawner.SetActive(true);
        }
        else
        {
            monsterSpawner.SetActive(true);
        }
        heroSpawner.SetActive(true);

    }

    private void Start()
    {
        isEnd = false;
        battleStates = PerformAction.READY;
        Turn = 0;
        UI = gameObject.GetComponent<UIBattleManager>();
        GameManager.s_Instance.SetMusic(backgroundSound);
    }

    private void Update()
    {
        if (!reloadMonsterLock && !reloadHeroLock)
        {
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
                                    HeroController target = heroesInBattle[i].GetComponent<HeroController>();
                                    int heroCpMax = heroCps.Max();
                                    if (target.GetCharacter().GetCP() == heroCpMax)
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
                                    MonsterController target = monsterInBattle[i].GetComponent<MonsterController>();
                                    if (target.GetCharacter().GetCP() == monsterCps.Max())
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
        if (isEnd) return;
        isEnd = true;
        if (!playingAudio)
        {
            GameManager.s_Instance.SetMusic(winSound);
            playingAudio = true;
        }
        avgHeroCp = sumHeroCp / 6;
        avgMonsterCp = sumMonsterCp / player.GetMonsterList().Count;

        int addExp = (int)(1500 * (avgHeroCp / avgMonsterCp));
        int addGold = (int)(1500 * (avgHeroCp / avgMonsterCp));
        int addInfamy = (int)(300 * (avgHeroCp / avgMonsterCp));

        for (int i = 0; i < player.GetMonsterList().Count; i++)
        {
            player.GetMonster(i).AddExp(addExp);
        }
        player.AddGold(addGold);
        player.AddInfamy(addInfamy);
        UI.SetWinText(addGold, addInfamy);
        UI.winUI.SetActive(true);
        for (int i = 0; i < player.GetRoomCount(); i++)
        {
            if (player.GetRoom(i).Items == Item.CROWN)
                player.GetRoom(i).Monsters[0].FinishBoss();
            player.GetRoom(i).PlaceItem(Item.NONE);
        }
    }

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

        int addExp = (int)(1000 * (avgHeroCp / avgMonsterCp));
        int addGold = (int)(800 * (avgHeroCp / avgMonsterCp));
        int addInfamy = (int)(-50 * (avgHeroCp / avgMonsterCp));

        for (int i = 0; i < player.GetMonsterList().Count; i++)
        {
            player.GetMonster(i).AddExp(addExp);
        }
        player.AddGold(addGold);
        player.AddInfamy(addInfamy);
        UI.SetFailText(addGold, addInfamy);
        UI.failUI.SetActive(true);
        for (int i = 0; i < player.GetRoomCount(); i++)
        {
            if (player.GetRoom(i).Items == Item.CROWN)
                player.GetRoom(i).Monsters[0].FinishBoss();
            player.GetRoom(i).PlaceItem(Item.NONE);
        }
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

        if (player.GetRoom(currentRoom).Items == Item.CROWN)
        {
            bossSpawner.GetComponent<BossSpawner>().SetBoss(player.GetRoom(currentRoom).Monsters[0]);
            bossSpawner.SetActive(true);
        }
        else
        {
            monsterSpawner.SetActive(false);
            monsterSpawner.GetComponent<MonsterSpawner>().SetMonster(player.GetRoom(currentRoom).Monsters);
            monsterSpawner.SetActive(true);
        }

        heroSpawner.SetActive(false);
        heroSpawner.GetComponent<HeroSpawner>().SetHeroes(heroes);
        heroSpawner.SetActive(true);
    }

    public void GetReady()
    {
        battleStates = PerformAction.WAIT;
    }
}
