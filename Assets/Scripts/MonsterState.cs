using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterState : MonoBehaviour
{
    private BattleManager battleManager;
    private Character character;
    private Vector3 startPosition;
    private bool actionStarted = false;
    private float animateSpeed = 15f;
    private bool isDead = false;
    private int spawnNumber; // 생성된 위치 넘버: 012 전열 345 후열

    public enum CharacterState
    {
        TURNCHECK,
        CHOOSEACTION,
        WAITING,
        ACTION
    }
    public CharacterState currentState;
    public GameObject targetObject;
    public Animator animator;
    public Slider healthSlider;


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
                if (battleManager.myTurn == true)
                {
                    if (spawnNumber == battleManager.monsterNumber[0])
                    {
                        ChooseAction();
                        currentState = CharacterState.WAITING;
                    }
                }
                break;
            case (CharacterState.WAITING):
                break;
            case (CharacterState.ACTION):
                StartCoroutine(TimeForAction());
                currentState = CharacterState.TURNCHECK;
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

        HeroState target = targetObject.GetComponent<HeroState>();

        animator.SetTrigger("Forward");
        while (MoveTowardEnemy()) { yield return null; }

        // 공격 이펙트 실행과 데미지 계산
        int damage = character.GetDamage();
        if(damage == character.GetMaxDamage())
        {
            animator.SetTrigger("Critical");
        }
        else
        {
            animator.SetTrigger("Attack");

        }

        GameObject effect;
        if (character.GetNature() == Nature.FIRE) effect = battleManager.fireEffect;
        else if (character.GetNature() == Nature.WATER) effect = battleManager.waterEffect;
        else effect = battleManager.windEffect;

        yield return new WaitForSeconds(0.4f);
        GameObject gameObject = Instantiate(effect, targetObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

        target.GetCharacter().GetHit(damage);
        target.healthSlider.value = target.GetCharacter().GetHP();

        if (target.GetCharacter().GetHP() == 0)
        {
            target.animator.SetTrigger("Die");
            target.SetIsDead(true);
            battleManager.heroesInBattle.Remove(targetObject);
            battleManager.heroNumber.Remove(target.GetSpawnNumber());
            battleManager.heroCps.Remove(target.GetCharacter().GetCP());
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

        actionStarted = false;

        // performList를 Wait로 reset
        battleManager.battleStates = BattleManager.PerformAction.WAIT;
    }

    private bool MoveTowardEnemy()
    {
        Vector3 enemyPosition =
            new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z - 2.5f);
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
        healthSlider.maxValue = character.GetMaxHP();
        healthSlider.value = character.GetHP();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetIsDead(bool value)
    {
        isDead = value;
    }

    public int GetSpawnNumber()
    {
        return spawnNumber;
    }

    public void SetSpawnNumber(int value)
    {
        spawnNumber = value;
    }
}
