using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MonsterController : CharacterActions
{

    private void Start()
    {
        startPosition = transform.position;
        currentState = CharacterState.TURNCHECK;
    }

    private void Update()
    {
        if (!battleManager.reloadMonsterLock && !battleManager.reloadHeroLock)
        {
            switch (currentState)
            {
                case (CharacterState.TURNCHECK):
                    if (battleManager.Turn == 0)
                    {
                        if (SpawnNumber == battleManager.monsterNumber[0])
                        {
                            battleManager.CollectActions(gameObject);
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
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        HeroController target = targetObject.GetComponent<HeroController>();

        animatorController.MoveFoward();
        while (MoveTowardEnemy()) { yield return null; }

        // 공격 이펙트 실행과 데미지 계산
        int damage = character.GetDamage();
        if (damage == character.GetMaxDamage())
            animatorController.Critical();
        else
            animatorController.Attack();

        GameObject effect;
        if (character.Nature == Nature.FIRE) effect = battleManager.fireEffect;
        else if (character.Nature == Nature.WATER) effect = battleManager.waterEffect;
        else effect = battleManager.windEffect;

        yield return new WaitForSeconds(0.3f);
        GameObject gameObject = Instantiate(effect, targetObject.transform.position, Quaternion.identity);

        target.GetCharacter().GetHit(damage);
        target.healthSlider.value = target.GetCharacter().HP;
        audioSource.PlayOneShot(hitSound);

        if (target.GetCharacter().HP == 0)
        {
            target.animatorController.Die();
            audioSource.PlayOneShot(target.deathSound);
            target.IsDead = true;

            battleManager.heroesInBattle.Remove(targetObject);
            battleManager.heroNumber.Remove(target.SpawnNumber);
            battleManager.heroCps.Remove(target.GetCharacter().GetCP());

        }
        else
        {
            target.animatorController.GetHit();
        }
        Destroy(gameObject, 0.5f);

        // 잠깐 기다림
        yield return new WaitForSeconds(0.8f);

        //제자리로 돌아옴
        animatorController.MoveBackward();
        while (MoveToStartPosition()) { yield return null; }

        // 제자리로 돌아와서 idle 애니메이션 실행
        animatorController.StopMove();

        // BattleManager에 performList에서 하나를 제거
        battleManager.performList.RemoveAt(0);

        actionStarted = false;

        // performList를 Wait로 reset
        battleManager.battleStates = BattleState.WAIT;
    }

    private bool MoveTowardEnemy()
    {
        Vector3 enemyPosition =
            new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z - 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, moveSpeed * Time.deltaTime));
    }
}
