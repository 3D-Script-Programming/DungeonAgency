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
    protected bool actionStarted = false;

    // 애니메이션 속도
    protected float moveSpeed = 20;

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
    }

    protected bool MoveToStartPosition()
    {
        return startPosition != (transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime));
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

