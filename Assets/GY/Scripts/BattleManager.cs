using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }
    public PerformAction battleStates;

    public bool myTurn;
    public int monsterCount = 0;
    public int heroesCount = 0;

    // 한 턴마다 공격에 대한 정보를 담는 리스트
    public List<HandleTurn> performList = new List<HandleTurn>();
    // TODO: GameManager에서 배열로 넘겨줌. 배열 -> 리스트로 형변환
    public List<GameObject> monsterInBattle = new List<GameObject>();
    public List<GameObject> heroesInBattle = new List<GameObject>();

    public int monsterPriority = 0;
    public int heroPriority = 0;

    private void Start()
    {
        battleStates = PerformAction.WAIT;
        monsterInBattle.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
        heroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        myTurn = true;
    }

    private void Update()
    {
        switch (battleStates)
        {
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
                    MonsterState monsterState = performer.GetComponent<MonsterState>();
                    monsterState.attackTarget = performList[0].attackerTarget;
                    monsterState.currentState = MonsterState.CharacterState.ACTION;
                    monsterPriority++;
                    monsterPriority %= monsterInBattle.Count;
                }
                else if(performer.tag == "Hero")
                {
                    HeroState heroState = performer.GetComponent<HeroState>();
                    heroState.attackTarget = performList[0].attackerTarget;
                    heroState.currentState = HeroState.CharacterState.ACTION;
                    heroPriority++;
                    heroPriority %= heroesInBattle.Count;
                }
                myTurn = !myTurn;
                battleStates = PerformAction.PERFORMACTION;
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
}
