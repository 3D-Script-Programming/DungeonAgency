using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    private BattleManager battleManager;
    public Character chracter; // 참조로 가져옴
    public Animator animator;

    public enum CharacterState
    {
        TURNCHECK,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public CharacterState currentState;
    private Vector3 startPosition;
    private bool actionStarted = false;
    public GameObject attackTarget;
    private float animateSpeed = 10f;
    HandleTurn myAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myAttack = new HandleTurn();
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        startPosition = transform.position;
        currentState = CharacterState.TURNCHECK;
    }

    private void Update()
    {
        switch (currentState)
        {
            case (CharacterState.TURNCHECK):
                if (battleManager.myTurn == true)
                {
                    if (this.gameObject == battleManager.monsterInBattle[battleManager.monsterPriority])
                        currentState = CharacterState.CHOOSEACTION;
                }
                break;
            case (CharacterState.CHOOSEACTION):
                ChooseAction(battleManager.heroesInBattle);
                currentState = CharacterState.WAITING;
                break;
            case (CharacterState.WAITING):
                break;
            case (CharacterState.ACTION):
                StartCoroutine(TimeForAction());
                    currentState = CharacterState.TURNCHECK;
                break;
            case (CharacterState.DEAD):
                // TODO: die
                animator.SetTrigger("Die");
                break;
        }
    }

    void ChooseAction(List<GameObject> targetList)
    {
        myAttack.attackerGameObject = this.gameObject;
        // TODO: cp가 가장 높은 타겟을 선택해야함 => 게임 메니저에서 용사와 몬스터 cp 가장 높은 타겟 가져오기
        myAttack.attackerTarget = targetList[Random.Range(0, targetList.Count)];
        battleManager.CollectActions(myAttack);
    }


    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        animator.SetTrigger("Forward");
        while (MoveTowardEnemy()) { yield return null; }

        animator.SetTrigger("Critical");
        yield return new WaitForSeconds(0.4f);
        attackTarget.GetComponent<HeroState>().animator.SetTrigger("GetHit");

        // 잠깐 기다림
        yield return new WaitForSeconds(1.2f);

        //제자리로 돌아옴
        animator.SetTrigger("Backward");
        while (MoveTowardBack()) { yield return null; }

        // 공격 이펙트 실행과 데미지 계산

        // 제자리로 돌아와서 idle 애니메이션 실행
        animator.SetTrigger("Idle");

        // BattleManager에 performList에서 하나를 제거
        battleManager.performList.RemoveAt(0);
        // performList를 Wait로 reset
        battleManager.battleStates = BattleManager.PerformAction.WAIT;

        actionStarted = false;
    }

    private bool MoveTowardEnemy()
    {
        Vector3 enemyPosition =
            new Vector3(attackTarget.transform.position.x, attackTarget.transform.position.y, attackTarget.transform.position.z - 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, animateSpeed * Time.deltaTime));
    }
    private bool MoveTowardBack()
    {
        return startPosition != (transform.position = Vector3.MoveTowards(transform.position, startPosition, animateSpeed * Time.deltaTime));
    }
}
