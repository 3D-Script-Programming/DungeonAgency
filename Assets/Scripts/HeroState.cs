using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour
{
    private BattleManager battleManager;
    private Character character;
    private Vector3 startPosition;
    private bool actionStarted = false;
    private float animateSpeed = 10f;
    private bool isDead = false;

    public enum CharacterState
    {
        TURNCHECK,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public CharacterState currentState;
    public GameObject targetObject;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        startPosition = transform.position;
        currentState = CharacterState.TURNCHECK;
    }

    private void Update()
    {
        switch (currentState)
        {
            case (CharacterState.TURNCHECK):
                if (isDead)
                {
                    battleManager.heroPriority++;
                    battleManager.heroPriority %= battleManager.heroesInBattle.Count;
                    currentState = CharacterState.DEAD;
                }
                else
                {
                    if (battleManager.myTurn == false)
                    {
                        if (gameObject == battleManager.heroesInBattle[battleManager.heroPriority])
                            currentState = CharacterState.CHOOSEACTION;
                    }
                }
                break;
            case (CharacterState.CHOOSEACTION):
                ChooseAction();
                currentState = CharacterState.WAITING;
                break;
            case (CharacterState.WAITING):
                break;
            case (CharacterState.ACTION):
                StartCoroutine(TimeForAction());
                currentState = CharacterState.TURNCHECK;
                break;
            case (CharacterState.DEAD):
                break;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.attackerGameObject = gameObject;
        battleManager.CollectActions(myAttack);
    }


    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        MonsterState target = targetObject.GetComponent<MonsterState>();

        animator.SetTrigger("Forward");
        while (MoveTowardEnemy()) { yield return null; }

        // TODO: attack 애니메이션 실행
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);

        target.GetCharacter().GetHit(character.GetDamage());

        if (target.GetCharacter().GetHP() == 0)
        {
            target.animator.SetTrigger("Die");
            target.SetIsDead(true);
        }
        else
        {
            target.animator.SetTrigger("GetHit");
        }

        // 잠깐 기다림
        yield return new WaitForSeconds(1.2f);

        //제자리로 돌아옴
        animator.SetTrigger("Backward");
        while (MoveTowardBack()) { yield return null; }

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
                    new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z + 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, animateSpeed * Time.deltaTime));
    }
    private bool MoveTowardBack()
    {
        return startPosition != (transform.position = Vector3.MoveTowards(transform.position, startPosition, animateSpeed * Time.deltaTime));
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetIsDead(bool value)
    {
        isDead = value;
    }

}
