using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : CharacterActions
{

    private void Start()
    {
        currentState = CharacterState.READY;
    }

    private void Update()
    {
        if (!battleManager.reloadMonsterLock && !battleManager.reloadHeroLock)
        {
            switch (currentState)
            {
                case (CharacterState.READY):
                    StartCoroutine(TimeForReady());
                    currentState = CharacterState.TURNCHECK;
                    break;
                case (CharacterState.TURNCHECK):
                    if (battleManager.Turn == 1)
                    {
                        if (SpawnNumber == battleManager.heroNumber[0])
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

    private IEnumerator TimeForReady()
    {
        yield return new WaitForSeconds(1f);
        animatorController.MoveFoward();
        while (MoveToStartPosition())
        {
            yield return null;
        }
        animatorController.StopMove();

        yield return new WaitForSeconds(1f);
        if (SpawnNumber == battleManager.heroesInBattle.Count - 1)
        {
            battleManager.GetReady();
        }
        yield break;
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        MonsterController target = targetObject.GetComponent<MonsterController>();

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

            battleManager.monsterInBattle.Remove(targetObject);
            battleManager.monsterNumber.Remove(target.SpawnNumber);
            battleManager.monsterCps.Remove(target.GetCharacter().GetCP());
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

        animatorController.StopMove();

        battleManager.performList.RemoveAt(0);

        actionStarted = false;

        // performList를 Wait로 reset
        battleManager.battleStates = BattleState.WAIT;
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    private bool MoveTowardEnemy()
    {
        Vector3 enemyPosition =
            new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z + 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, moveSpeed * Time.deltaTime));
    }
}
