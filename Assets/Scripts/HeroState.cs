using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour
{
    private BattleManager battleManager;
    public Character chracter;
    public int priority;

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
    private float animateSpeed = 15f;
    HandleTurn myAttack;

    private void Start()
    {
        // TODO: 캐릭터 스텟은 게임 메니저가 관리함 이후에 지워야 해
        chracter = new Character(10, 10, 10, Type.FIRE);
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
                if (battleManager.myTurn == false)
                {
                    if (priority == battleManager.heroesCount)
                    {
                        currentState = CharacterState.CHOOSEACTION;
                    }
                }
                break;
            case (CharacterState.CHOOSEACTION):
                ChooseAction(battleManager.monsterInBattle);
                currentState = CharacterState.WAITING;
                break;
            case (CharacterState.WAITING):
                // TODO: idle
                break;
            case (CharacterState.ACTION):
                StartCoroutine(TimeForAction());
                if (chracter.GetMaxHP() < 0)
                {
                    currentState = CharacterState.DEAD;
                }
                else
                {
                    currentState = CharacterState.TURNCHECK;
                }
                break;
            case (CharacterState.DEAD):
                // TODO: die
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
        Vector3 enemyPosition =
                new Vector3(attackTarget.transform.position.x, attackTarget.transform.position.y, attackTarget.transform.position.z + 2);

        while (MoveToward(enemyPosition)) { yield return null; }

        // TODO: attack 애니메이션 실행

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
        // TODO: move 애니메이션 실행
    }
}
