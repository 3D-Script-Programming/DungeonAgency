using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : CharacterActions
{
    // 초기화 함수
    private void Start()
    {
        // 초기 상태 설정: 대기 상태
        currentState = CharacterState.READY;
    }

    // 주기적인 업데이트
    private void Update()
    {
        // 몬스터 및 히어로 재로딩이 진행 중이 아니며 전투가 종료되지 않았을 때
        if (!battleManager.reloadMonsterLock && !battleManager.reloadHeroLock && !battleManager.isEnd)
        {
            switch (currentState)
            {
                case CharacterState.READY:
                    // 대기 상태에 진입하면 대기 시간 코루틴 시작
                    StartCoroutine(TimeForReady());
                    // 상태를 턴 체크로 변경
                    currentState = CharacterState.TURNCHECK;
                    break;
                case CharacterState.TURNCHECK:
                    // 턴 체크 상태 처리
                    HandleTurnCheck();
                    break;
                case CharacterState.WAITING:
                    // 대기 상태에서는 아무 동작도 하지 않음
                    break;
                case CharacterState.ACTION:
                    // 행동 상태 처리
                    StartCoroutine(HandleAction());
                    // 상태를 턴 체크로 변경
                    currentState = CharacterState.TURNCHECK;
                    break;
            }
        }
    }

    // 대기 상태에서 대기 시간을 가지며 초기 위치로 이동하는 코루틴
    private IEnumerator TimeForReady()
    {
        // 대기 시간
        yield return new WaitForSeconds(1f);

        // 전진 애니메이션 실행
        animatorController.MoveForward();

        // 초기 위치로 이동 중일 동안 대기
        while (MoveToStartPosition())
        {
            yield return null;
        }

        // 이동 중지 애니메이션 실행
        animatorController.StopMove();

        // 대기 시간
        yield return new WaitForSeconds(1f);

        // 마지막 히어로가 대기 상태로 전환될 때, 전투 상태를 대기로 변경
        if (SpawnNumber == battleManager.heroesInBattle.Count - 1)
        {
            battleManager.battleStates = BattleState.WAIT;
        }
    }

    // 용사 턴 체크 상태를 처리하는 함수
    private void HandleTurnCheck()
    {
        // 턴이 1이고, 용사의 SpawnNumber가 battleManager.heroNumber[0]과 일치할 때
        if (battleManager.Turn == 1 && SpawnNumber == battleManager.heroNumber[0])
        {
            // performList에 현재 용사 추가하고 대기 상태로 변경
            battleManager.performList.Add(gameObject);
            currentState = CharacterState.WAITING;
        }
    }

    // 용사의 행동 상태를 처리하는 코루틴
    private IEnumerator HandleAction()
    {
        // 이미 행동이 시작되었을 경우 종료
        if (actionStarted)
        {
            yield break;
        }

        // 행동이 시작됨을 표시
        actionStarted = true;

        // 대상은 몬스터 캐릭터로 설정
        MonsterController target = targetObject.GetComponent<MonsterController>();

        // 용사 애니메이션: 전진
        animatorController.MoveForward();

        // 몬스터 캐릭터에게 이동 중일 동안 대기
        while (MoveTowardEnemy())
        {
            yield return null;
        }

        // 데미지를 얻고 공격 애니메이션 실행
        int damage = character.GetDamage();
        PlayAttackAnimation(damage);

        yield return new WaitForSeconds(0.3f);

        GameObject effect = InstantiateEffect(targetObject.transform.position);

        // 몬스터에게 피격 처리
        yield return HandleTargetHit(target, damage, effect);

        // 뒤로 이동 애니메이션 재생
        animatorController.MoveBackward();

        // 초기 위치로 이동하는 동안 대기
        while (MoveToStartPosition())
        {
            yield return null;
        }

        // 이동 중지 애니메이션 실행
        animatorController.StopMove();

        // performList에서 현재 행동을 제거하고 대기 상태로 변경
        HandleActionEnd();
    }

    // 적에게 전진 이동하는 함수
    private bool MoveTowardEnemy()
    {
        // 대상 위치로 용사 이동
        Vector3 enemyPosition = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z + 2.5f);
        return enemyPosition != (transform.position = Vector3.MoveTowards(transform.position, enemyPosition, moveSpeed * Time.deltaTime));
    }

    // 용사의 대상에게 피격 처리를 하는 함수
    private IEnumerator HandleTargetHit(MonsterController target, int damage, GameObject effect)
    {
        // 대상에게 데미지를 입히고 UI 갱신 및 효과음 재생
        target.GetCharacter().GetHit(damage);
        target.healthSlider.value = target.GetCharacter().HP;
        audioSource.PlayOneShot(hitSound);

        // 대상이 사망한 경우
        if (target.GetCharacter().HP == 0)
        {
            // 대상의 사망 처리
            HandleTargetDeath(target);
        }
        else
        {
            // 대상이 살아있는 경우 피격 애니메이션 재생
            target.animatorController.GetHit();
        }

        // 이팩트 발생 시간 동안 대기
        yield return new WaitForSeconds(0.5f);

        // 공격 이펙트 반환
        ReturnEffectToPool(effect);
    }

    // 대상이 사망했을 때 처리를 하는 함수
    private void HandleTargetDeath(MonsterController target)
    {
        // 대상의 사망 애니메이션과 효과음 재생
        target.animatorController.Die();
        audioSource.PlayOneShot(target.deathSound);
        target.IsDead = true;

        // BattleManager에서 대상 제거
        battleManager.monsterInBattle.Remove(targetObject);
        battleManager.monsterNumber.Remove(target.SpawnNumber);
        battleManager.monsterCps.Remove(target.GetCharacter().GetCP());
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }
}
