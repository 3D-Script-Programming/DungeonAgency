using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageListItemController : MonoBehaviour
{
    // UI 요소들
    public TextMeshProUGUI nameText, levelText, strText, balText, vtpText, cpText;
    public Button listItem;
    public GameObject checkOnImage;
    public GameObject checkOffImage;
    public GameObject natureHolder;
    public GameObject natureIcon;

    // 다른 클래스 참조
    private DungeonManager dungeonManager;
    public Character Monster { get; private set; }

    private void Start()
    {
        ApplyEvents(); // 이벤트 적용
    }

    private void Update()
    {
        if (Monster != null)
        {
            // 몬스터가 룸에 배치되어 있지 않으면
            if (Monster.CurrentRoomNumber == -1)
                DeactivateMonsterCheck(); // 몬스터 체크 비활성화
            // 몬스터가 룸에 배치되어 있다면
            else
                ActivateMonsterCheck(); // 몬스터 체크 활성화
        }
    }

    void ApplyEvents()
    {
        listItem.onClick.AddListener(OnClickListItem); // 리스트 아이템 클릭 이벤트 추가
    }

    // 몬스터 정보 설정
    public void SetText(Character monster)
    {
        // 몬스터의 속성에 따라 UI 업데이트
        if (monster.Nature == Nature.FIRE)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/fire", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_red", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WATER)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/water", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_blue", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WIND)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/wind", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_green", typeof(Sprite)) as Sprite;
        }

        // 몬스터 정보 텍스트 업데이트
        nameText.text = monster.Name.ToString();
        levelText.text = monster.Level.ToString();
        strText.text = monster.Strength.ToString();
        balText.text = monster.Balance.ToString();
        vtpText.text = monster.Vitality.ToString();
        cpText.text = monster.GetCP().ToString();
        this.Monster = monster; // 현재 몬스터 설정
    }

    // 리스트 아이템 클릭 시 호출
    public void OnClickListItem()
    {
        dungeonManager.UIManager.OnClickListItem(Monster); // 던전 매니저를 통해 UI 매니저 호출
    }

    // 던전 매니저 설정
    public void SetDungeonManager(DungeonManager dungeonManager)
    {
        this.dungeonManager = dungeonManager;
    }

    // 몬스터 체크 활성화
    public void ActivateMonsterCheck()
    {
        checkOnImage.SetActive(true);
        checkOffImage.SetActive(false);
    }

    // 몬스터 체크 비활성화
    public void DeactivateMonsterCheck()
    {
        checkOnImage.SetActive(false);
        checkOffImage.SetActive(true);
    }
}
