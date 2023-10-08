using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActions : MonoBehaviour
{
    // 배틀매니저 객체
    protected BattleManager battleManager;

    // 캐릭터 정보
    protected Character character;

    // 시작 위치
    protected Vector3 startPosition;

    // 현재 액션중인가
    protected bool actionStarted;

    // 애니메이션 속도
    protected float moveSpeed;

    // 애니메이터
    public AnimatorController animatorController;

    // 오디오 소스
    protected AudioSource audioSource;

    // 죽었는가?
    public bool IsDead { get; set; }

    // 생성된 위치 넘버: 012 전열 345 후열
    public int SpawnNumber { get; set; }

    // 캐릭터 상태
    public CharacterState currentState;

    // 어택 타겟
    public GameObject targetObject;

    // 쳋력바
    public Slider healthSlider;

    // 죽음 사운드
    public AudioClip deathSound;

    // 피격 사운드
    public AudioClip hitSound;

    private void Awake()
    {
        animatorController = new AnimatorController(gameObject.GetComponent<Animator>());
        audioSource = GetComponent<AudioSource>();
        IsDead = false;
        actionStarted = false;
        moveSpeed = 20f;
    }

    protected bool MoveToStartPosition()
    {
        // 현재 위치에서 목표 지점까지 이동
        transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
        // 현재 위치가 목표 지점과 일치하면 false 반환
        return transform.position != startPosition;
    }

    // 공격 애니메이션을 실행하는 함수
    protected void PlayAttackAnimation(int damage)
    {
        // 데미지가 최대 데미지와 같을 경우 크리티컬 애니메이션 실행, 그 외에는 공격 애니메이션 실행
        if (damage == character.GetMaxDamage())
            animatorController.Critical();
        else
            animatorController.Attack();
    }

    // 캐릭터의 행동이 종료될 때 처리를 하는 함수
    protected void HandleActionEnd()
    {
        // 행동이 종료되었음을 표시
        actionStarted = false;

        // 전투가 종료되지 않았을 때
        if (!battleManager.isEnd)
        {
            // performList에서 현재 행동을 제거
            battleManager.performList.RemoveAt(0);

            // 행동이 끝났으므로 대기 상태로 변경
            battleManager.battleStates = BattleState.WAIT;
        }
    }

    // 이펙트 오브젝트 생성 및 위치 설정
    protected GameObject InstantiateEffect(Vector3 position)
    {
        GameObject effect = null;

        // 이펙트 종류에 따라 풀을 선택
        switch (character.Nature)
        {
            case Nature.FIRE:
                if (battleManager.fireEffectPool != null)
                    effect = battleManager.fireEffectPool.GetObject();
                break;
            case Nature.WATER:
                if (battleManager.waterEffectPool != null)
                    effect = battleManager.waterEffectPool.GetObject();
                break;
            case Nature.WIND:
                if (battleManager.windEffectPool != null)
                    effect = battleManager.windEffectPool.GetObject();
                break;
            default:
                // 기본적으로 fireEffect 풀 사용
                if (battleManager.fireEffectPool != null)
                    effect = battleManager.fireEffectPool.GetObject();
                break;
        }

        effect.transform.position = position;
        return effect;
    }


    // 이펙트를 풀에 반환하는 함수
    protected void ReturnEffectToPool(GameObject effect)
    {
        // 이펙트 종류에 따라 풀 선택
        switch (character.Nature)
        {
            case Nature.FIRE:
                battleManager.fireEffectPool.ReturnObject(effect);
                break;
            case Nature.WATER:
                battleManager.waterEffectPool.ReturnObject(effect);
                break;
            case Nature.WIND:
                battleManager.windEffectPool.ReturnObject(effect);
                break;
            default:
                // 기본적으로 fireEffect 풀 사용
                battleManager.fireEffectPool.ReturnObject(effect);
                break;
        }
    }

    public void SetBattleManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        healthSlider.maxValue = character.GetMaxHP();
        healthSlider.value = character.HP;
    }

    public Character GetCharacter()
    {
        return character;
    }

}

