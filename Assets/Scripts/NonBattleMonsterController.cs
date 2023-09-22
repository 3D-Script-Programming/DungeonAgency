using UnityEngine;

public class NonBattleMonsterController : MonoBehaviour
{
    // 캐릭터 체력 바 객체
    [SerializeField] private GameObject slider;

    // 애니메이터 객체
    private Animator animator;

    private void Awake()
    {
        // animator를 가져옴
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        DeactivateSlider();
        PlayVictoryAnimation();
    }

    // 슬라이더 비활성화 함수
    private void DeactivateSlider()
    {
        // 슬라이더를 비활성화
        slider.SetActive(false);
    }

    // 등장 애니메이션 재생 함수
    private void PlayVictoryAnimation()
    {
        // 'Victory' 트리거를 사용하여 등장 애니메이션 재생
        animator.SetTrigger("Victory");
    }
}