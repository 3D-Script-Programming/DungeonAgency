using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    private BattleManager battleManager;
    private Character character;
    private Vector3 startPosition;
    private bool actionStarted = false;
    private float animateSpeed = 15f;
    private bool isDead = false;
    private int spawnNumber; // 생성된 위치 넘버: 012 전열 345 후열

    public CharacterState currentState;
    public GameObject targetObject;
    public Animator animator;
    public Slider healthSlider;


    private void Start()
    {
        animator = GetComponent<Animator>();
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        currentState = CharacterState.READY;
    }

    private void Update()
    {
        switch (currentState)
        {
            case (CharacterState.READY):
                StartCoroutine(TimeForReady());
                currentState = CharacterState.TURNCHECK;
                break;
            case (CharacterState.TURNCHECK):
                if (battleManager.myTurn == false)
                {
                    if (spawnNumber == battleManager.heroNumber[0])
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

    private IEnumerator TimeForReady() 
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Forward");
        while (MoveToStartPosition()) { 
            yield return null; 
        }
        animator.SetTrigger("Idle");
        
        yield return new WaitForSeconds(1f);
        if (spawnNumber == battleManager.heroesInBattle.Count - 1) {
            battleManager.GetReady();
        }
        yield break;
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

        MonsterController target = targetObject.GetComponent<MonsterController>();

        animator.SetTrigger("Forward");
        while (MoveTowardEnemy()) { yield return null; }

        // TODO: attack 애니메이션 실행
        int damage = character.GetDamage();
        if (damage == character.GetMaxDamage()) animator.SetTrigger("Critical");
        else animator.SetTrigger("Attack");

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
            battleManager.monsterInBattle.Remove(targetObject);
            battleManager.monsterNumber.Remove(target.GetSpawnNumber());
            battleManager.monsterCps.Remove(target.GetCharacter().GetCP());
        }
        else
        {
            target.animator.SetTrigger("GetHit");
        }

        // 잠깐 기다림
        yield return new WaitForSeconds(1.2f);

        //제자리로 돌아옴
        animator.SetTrigger("Backward");
        while (MoveToStartPosition()) { yield return null; }

        animator.SetTrigger("Idle");

        battleManager.performList.RemoveAt(0);
        
        actionStarted = false;

        // performList를 Wait로 reset
        battleManager.battleStates = BattleManager.PerformAction.WAIT;
    }

    private bool MoveTowardEnemy()
    {
        Vector3 enemyPosition =
                    new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z + 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, animateSpeed * Time.deltaTime));
    }

    private bool MoveToStartPosition()
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

    public void SetStartPosition(Vector3 startPosition) {
        this.startPosition = startPosition;
    }
}
