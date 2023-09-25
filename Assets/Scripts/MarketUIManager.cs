using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketUIManager : MonoBehaviour
{
    private MarketManager marketManager;

    // UI 버튼 : 씬 이동 또는 설정 
    public Button mainButton;
    public Button manageButton;
    public Button settingButton;
    public Slider soundSliderBar;

    // UI 텍스트 : 악명, 골드
    public TextMeshProUGUI infamyText;
    public TextMeshProUGUI goldText;

    // 팝업 게임 오브젝트
    public GameObject popupSetting;
    public GameObject popupWarning;

    // 팝업 닫기 버튼
    public Button settingCloseButton;
    public Button warningCloseButton;

    // 아이템 버튼
    public Button btnCrown, btnTreasure;
    public TextMeshProUGUI countCrown, countTreasure;

    public Transform scrollViewContent;
    private List<ListItemController> listItems;

    private void Awake()
    {
        listItems = new List<ListItemController>();
        marketManager = gameObject.GetComponent<MarketManager>();
        // UI 이벤트 등록
        ApplyUIEvents();
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 골드 텍스트 업데이트
        goldText.text = GameManager.s_Instance.player.Gold.ToString();
        // 악명 텍스트 업데이트
        infamyText.text = GameManager.s_Instance.player.Infamy.ToString();
        foreach (ListItemController listItem in listItems)
        {
            listItem.SetButton();
        }
        SetItemButton();
    }

    // UI 이벤트 등록
    private void ApplyUIEvents()
    {
        mainButton.onClick.AddListener(OnClickMainButton);
        manageButton.onClick.AddListener(OnClickManageButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton);
        warningCloseButton.onClick.AddListener(OnClickWarningCloseButton);
        btnCrown.onClick.AddListener(OnClickBuyCrown);
        btnTreasure.onClick.AddListener(OnClickBuyTreasure);
    }

    // Play 버튼 클릭 시 호출되는 메서드
    private void OnClickMainButton()
    {
        GameManager.s_Instance.PlayButtonSound();
        GameManager.MoveScene("MainScene");
    }

    // Manage 버튼 클릭 시 호출되는 메서드
    private void OnClickManageButton()
    {
        GameManager.s_Instance.PlayButtonSound();
        GameManager.MoveScene("ManageScene");
    }

    // Setting 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingButton()
    {
        // Setting 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        popupSetting.SetActive(true);
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value));
    }

    // Setting 팝업의 닫기 버튼 클릭 시 호출되는 메서드
    private void OnClickSettingCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound();
        popupSetting.SetActive(false);
    }

    // Warning 팝업의 닫기 버튼 클릭 시 호출되는 메서드
    private void OnClickWarningCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound();
        popupWarning.SetActive(false);
    }

    // BuyCrown 버튼 클릭 시 호출되는 메서드
    public void OnClickBuyCrown()
    {
        GameManager.s_Instance.PlayButtonSound();
        if (1000 > GameManager.s_Instance.player.Gold)
            return;

        GameManager.s_Instance.player.AddItem(Item.CROWN, 1);
        GameManager.s_Instance.player.AddGold(-1000);

        UpdateUI();
    }

    // BuyTreasure 버튼 클릭 시 호출되는 메서드
    public void OnClickBuyTreasure()
    {
        GameManager.s_Instance.PlayButtonSound();
        if (500 > GameManager.s_Instance.player.Gold)
            return;

        GameManager.s_Instance.player.AddItem(Item.TREASURE, 1);
        GameManager.s_Instance.player.AddGold(-500);

        UpdateUI();
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

    public void InstantiateListItems(List<Character> monsters)
    {
        while (listItems.Count > 0)
            Destroy(listItems[listItems.Count - 1]);

        foreach (Character monster in monsters)
        {
            GameObject listItemObject = Instantiate(Resources.Load<GameObject>("UI/MarketListItem"), scrollViewContent);
            ListItemController listItemController = listItemObject.GetComponent<ListItemController>();
            listItemController.SetText(monster);
            listItemController.SetMarketManager(marketManager);
            listItems.Add(listItemController);
        }
    }
}
