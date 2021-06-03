using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterState : MonoBehaviour
{
    private BattleManager battleManager;
    public Character chracter;
    public enum TurnState
    {
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public TurnState currentState;
    private Vector3 startPosition;
    private bool actionStarted = false;
    public GameObject attackTarget;
    private float animateSpeed = 10f;

    private void Start()
    {
        chracter = new Character(10, 10, 10, Type.FIRE);
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        startPosition = transform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):

                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):

                break;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.attackerGameObject = this.gameObject;
        if(this.gameObject.tag == "Monster")
        {
            // 공격 대상 설정 용사 -> 몬스터 
            myAttack.attackerTarget = battleManager.heroesInBattle[Random.Range(0, battleManager.heroesInBattle.Count)];
        }
        else if(this.gameObject.tag == "Hero")
        {
            // 공격 대상 설정 용사 -> 몬스터 
            myAttack.attackerTarget = battleManager.monsterInBattle[Random.Range(0, battleManager.monsterInBattle.Count)];
        }
        battleManager.CollectActions(myAttack);
    }
    
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;
        Vector3 enemyPosition;
        if (attackTarget.tag == "Monster")
        {
            enemyPosition =
                new Vector3(attackTarget.transform.position.x, attackTarget.transform.position.y, attackTarget.transform.position.z + 2);
        }
        else
        {
            enemyPosition =
                new Vector3(attackTarget.transform.position.x, attackTarget.transform.position.y, attackTarget.transform.position.z - 2);
        }

        // 공격 대상에게 가까이 가서 attack 애니메이션 실행
        while (MoveToward(enemyPosition)) { yield return null; }

        // 잠깐 기다림
        yield return new WaitForSeconds(0.5f);

        //제자리로 돌아옴
        while (MoveToward(startPosition)) { yield return null; }

        // 공격 이펙트 실행과 데미지 계산

        // 제자리로 돌아와서 idle 애니메이션 실행

        // BattleManager에 performList에서 하나를 제거
        battleManager.performList.RemoveAt(0);
        // performList를 Wait로 reset
        battleManager.battleStates = BattleManager.PerformAction.WAIT;

        actionStarted = false;
    }

    private bool MoveToward(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animateSpeed * Time.deltaTime));
    }
}
