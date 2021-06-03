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

    // 한 턴마다 공격에 대한 정보를 담는 리스트
    public List<HandleTurn> performList = new List<HandleTurn>();
    public List<GameObject> monsterInBattle = new List<GameObject>();
    public List<GameObject> heroesInBattle = new List<GameObject>();

    private void Start()
    {
        battleStates = PerformAction.WAIT;
        monsterInBattle.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
        heroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
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
                ChracterState chracterState = performer.GetComponent<ChracterState>();
                chracterState.attackTarget = performList[0].attackerTarget;
                chracterState.currentState = ChracterState.TurnState.ACTION;
                battleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):

                break;
        }
    }
    
    public void CollectActions(HandleTurn input)
    {
        performList.Add(input);
    }
}
