using UnityEngine;
using UnityEngine.UI;

public class NonBattleMonsterController : MonoBehaviour
{
    // 캐릭터 체력 바 객체
    [SerializeField] private Slider slider;

    // 몬스터 정보 속성
    public Character MonsterInfo { get; set; }

    // 애니메이터 객체
    private Animator animator;

    private void Awake()
    {
        // animator를 가져옴
        animator = GetComponent<Animator>();
        slider = gameObject.GetComponent<MonsterController>().healthSlider;
    }

    private void OnEnable()
    {
        // 체력 바 비활성화
        DeactivateSlider();
        // 승리 애니메이션 재생
        PlayVictoryAnimation();
    }

    // 슬라이더 비활성화 함수
    private void DeactivateSlider()
    {
        // 캐릭터 체력 바를 비활성화
        slider.gameObject.SetActive(false);
    }

    // 등장 애니메이션 재생 함수
    private void PlayVictoryAnimation()
    {
        // 'Victory' 트리거를 사용하여 등장 애니메이션을 재생
        animator.SetTrigger("Victory");
    }
}