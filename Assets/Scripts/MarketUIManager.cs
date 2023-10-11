using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketUIManager : MonoBehaviour
{
    // MarketManager 스크립트 참조를 저장하는 변수
    private MarketManager marketManager;

    // UI 버튼 : 씬 이동 또는 설정 
    public Button mainButton; // 메인 씬으로 이동하는 버튼
    public Button manageButton; // 관리 씬으로 이동하는 버튼
    public Button settingButton; // 설정 팝업을 열기 위한 버튼
    public Slider soundSliderBar; // 배경 음악 볼륨 조절 슬라이더

    // UI 텍스트 : 악명, 골드
    public TextMeshProUGUI infamyText; // 악명을 표시하는 텍스트
    public TextMeshProUGUI goldText; // 골드를 표시하는 텍스트

    // 팝업 게임 오브젝트
    public GameObject popupSetting; // 설정 팝업

    // 팝업 닫기 버튼
    public Button settingCloseButton; // 설정 팝업 닫기 버튼

    // 아이템 버튼
    public Button btnCrown; // 크라운 구매 버튼
    public Button btnTreasure; // 보물 구매 버튼
    public TextMeshProUGUI countCrown; // 크라운 갯수 표시 텍스트
    public TextMeshProUGUI countTreasure; // 보물 갯수 표시 텍스트

    public Transform scrollViewContent; // 몬스터 목록을 표시하는 스크롤 뷰 컨텐츠
    private List<MarketListItemController> listItems; // 몬스터 목록 아이템 컨트롤러를 저장하는 리스트

    private void Awake()
    {
        // 몬스터 목록 아이템 컨트롤러 리스트 초기화
        listItems = new List<MarketListItemController>();
        // MarketManager 스크립트 참조 저장
        marketManager = gameObject.GetComponent<MarketManager>();
        // UI 이벤트 등록
        ApplyUIEvents();
        // UI 업데이트
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 골드 텍스트 업데이트
        goldText.text = GameManager.s_Instance.player.Gold.ToString();
        // 악명 텍스트 업데이트
        infamyText.text = GameManager.s_Instance.player.Infamy.ToString();
        // 몬스터 전체 목록에 Button UI(색상 변경)를 업데이트합니다.
        foreach (MarketListItemController listItem in listItems)
        {
            listItem.SetButton();
        }
        // Item 버튼들의 UI(색상 변경)를 업데이트 합니다.
        SetItemButton();
    }

    // UI 이벤트 등록
    private void ApplyUIEvents()
    {
        mainButton.onClick.AddListener(OnClickMainButton); // 메인 버튼 클릭 이벤트 등록
        manageButton.onClick.AddListener(OnClickManageButton); // 관리 버튼 클릭 이벤트 등록
        settingButton.onClick.AddListener(OnClickSettingButton); // 설정 버튼 클릭 이벤트 등록
        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton); // 설정 팝업 닫기 버튼 클릭 이벤트 등록
        btnCrown.onClick.AddListener(OnClickBuyCrown); // 크라운 구매 버튼 클릭 이벤트 등록
        btnTreasure.onClick.AddListener(OnClickBuyTreasure); // 보물 구매 버튼 클릭 이벤트 등록
    }

    // 메인 버튼 클릭 시 호출되는 메서드
    private void OnClickMainButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("MainScene"); // 메인 씬으로 이동
    }

    // 관리 버튼 클릭 시 호출되는 메서드
    private void OnClickManageButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("ManageScene"); // 관리 씬으로 이동
    }

    // 설정 버튼 클릭 시 호출되는 메서드
    private void OnClickSettingButton()
    {
        // 설정 팝업 열기
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(true); // 설정 팝업 활성화
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value)); // 볼륨 조절 슬라이더 이벤트 등록
    }

    // 설정 팝업의 닫기 버튼 클릭 시 호출되는 메서드
    private void OnClickSettingCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(false); // 설정 팝업 비활성화
    }

    // 크라운 구매 버튼 클릭 시 호출되는 메서드
    public void OnClickBuyCrown()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        if (1000 > GameManager.s_Instance.player.Gold)
            return;

        GameManager.s_Instance.player.AddItem(Item.CROWN, 1); // 크라운 아이템을 추가
        GameManager.s_Instance.player.AddGold(-1000); // 골드 감소

        UpdateUI(); // UI 업데이트
    }

    // 보물 구매 버튼 클릭 시 호출되는 메서드
    public void OnClickBuyTreasure()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        if (500 > GameManager.s_Instance.player.Gold)
            return;

        GameManager.s_Instance.player.AddItem(Item.TREASURE, 1); // 보물 아이템을 추가
        GameManager.s_Instance.player.AddGold(-500); // 골드 감소

        UpdateUI(); // UI 업데이트
    }

    // 아이템 버튼 상태를 업데이트하는 메서드
    public void SetItemButton()
    {
        if (500 > GameManager.s_Instance.player.Gold)
        {
            btnTreasure.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
        {
            btnTreasure.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
        }

        countTreasure.text = GameManager.s_Instance.player.GetItem(Item.TREASURE).ToString();

        if (1000 > GameManager.s_Instance.player.Gold)
        {
            btnCrown.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
        {
            btnCrown.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
        }

        countCrown.text = GameManager.s_Instance.player.GetItem(Item.CROWN).ToString();
    }

    // 몬스터 목록을 생성하고 UI에 표시하는 메서드
    public void InstantiateListItems(List<Character> monsters)
    {
        // 기존 몬스터 목록 아이템 삭제
        while (listItems.Count > 0)
            Destroy(listItems[listItems.Count - 1].gameObject);

        // 새로운 몬스터 목록 아이템 생성 및 리스트에 추가
        foreach (Character monster in monsters)
        {
            GameObject listItemObject = Instantiate(Resources.Load<GameObject>("UI/MarketListItem"), scrollViewContent);
            MarketListItemController listItemController = listItemObject.GetComponent<MarketListItemController>();
            listItemController.SetText(monster);
            listItemController.SetMarketManager(marketManager);
            listItems.Add(listItemController);
        }
    }
}