using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    // UI 버튼 : 씬 이동 또는 설정 
    public Button playButton;
    public Button marketButton;
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

    // UI 초기화 및 이벤트 등록을 처리하는 메서드입니다.
    public void Initialize()
    {
        // UI 업데이트
        UpdateUI();
        // UI 이벤트 등록
        ApplyUIEvents();
    }

    private void UpdateUI()
    {
        // 골드 텍스트 업데이트
        goldText.text = GameManager.s_Instance.player.Gold.ToString();
        // 악명 텍스트 업데이트
        infamyText.text = GameManager.s_Instance.player.Infamy.ToString();
    }

    // UI 버튼에 이벤트 리스너를 등록하는 메서드입니다.
    private void ApplyUIEvents()
    {
        // 각 버튼에 클릭 이벤트 리스너 등록
        playButton.onClick.AddListener(OnClickPlayButton);
        marketButton.onClick.AddListener(OnClickMarketButton);
        manageButton.onClick.AddListener(OnClickManageButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton);
        warningCloseButton.onClick.AddListener(OnClickWarningCloseButton);
    }

    // Play 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickPlayButton()
    {
        // Play 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();

        // 팝업 관련 처리
        if (GameManager.s_Instance.player.NotReadyForBattle())
        {
            GameManager.s_Instance.TogglePause(); // 일시 정지 토글
            // 경고 팝업 표시
            popupWarning.SetActive(true);
        }
        else
        {
            // 관리 씬으로 이동
            GameManager.MoveScene("BattleScene");
        }
    }

    // Market 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickMarketButton()
    {
        // Market 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        GameManager.MoveScene("MarketScene");
    }

    // Manage 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickManageButton()
    {
        // Manage 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        GameManager.MoveScene("ManageScene");
    }

    // Setting 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingButton()
    {
        // Setting 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(true);
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value));
    }

    // Setting 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingCloseButton()
    {
        // Setting 팝업 닫기 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(false);
    }

    // Warning 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickWarningCloseButton()
    {
        // Warning 팝업 닫기 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupWarning.SetActive(false);
    }
}