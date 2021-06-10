using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum PerformAction
    {
        READY,
        WAIT,
        TAKEACTION,
        PERFORMACTION 
    }
    public PerformAction battleStates;

    public bool myTurn;

    public List<HandleTurn> performList = new List<HandleTurn>();
    public List<GameObject> monsterInBattle = new List<GameObject>();
    public List<GameObject> heroesInBattle = new List<GameObject>();
    public List<int> monsterNumber = new List<int>();
    public List<int> heroNumber = new List<int>();
    public List<int> monsterCps = new List<int>();
    public List<int> heroCps = new List<int>();
    public GameObject fireEffect;
    public GameObject waterEffect;
    public GameObject windEffect;

    private void Start()
    {
        battleStates = PerformAction.READY;
        myTurn = true;
    }

    private void Update()
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
                GameObject performer = performList[0].attackerGameObject;
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
                        myTurn = !myTurn;
                        movePriority(monsterNumber);
                    }
                    else
                    {
                        performList.RemoveAt(0);
                        battleStates = PerformAction.WAIT;
                    }
                }
                else if(performer.tag == "Hero")
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
                        myTurn = !myTurn;
                        movePriority(heroNumber);
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
    
    public void CollectActions(HandleTurn input)
    {
        performList.Add(input);
    }

    private void movePriority(List<int> number)
    {
        // 맨 뒤 순서로 이동
        int currentNumber = number[0];
        number.RemoveAt(0);
        number.Add(currentNumber);
    }

    public void GetReady() {
        battleStates = PerformAction.WAIT;
    }
}
