using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    // 이기거나 지는 상황에서 표시될 금화 및 악명 텍스트
    public TextMeshProUGUI winGoldText;
    public TextMeshProUGUI WinInfamyText;
    public TextMeshProUGUI failGoldText;
    public TextMeshProUGUI failInfamyText;

    // 이기거나 지는 팝업 및 설정과 관련된 UI 요소들
    public GameObject popupWin;
    public GameObject popupFail;
    public GameObject popupSetting;
    public GameObject popupQuit;
    public Slider soundSliderBar;

    // 다양한 UI 상호작용을 위한 버튼들
    public Button settingButton;
    public Button settingCloseButton;
    public Button quitButton;
    public Button yesQuit;
    public Button noQuit;
    public Button winHome;
    public Button winRestart;
    public Button winNext;
    public Button failHome;
    public Button failRestart;
    public Button failNext;

    // 플레이어 객체
    private Player player;

    private void Start()
    {
        // GameManager를 통해 플레이어에 대한 참조 획득
        player = GameManager.s_Instance.player;
        // UI 이벤트 설정
        ApplyUIEvents();
    }

    // UI 이벤트 적용
    private void ApplyUIEvents()
    {
        quitButton.onClick.AddListener(onClickQuitButton);
        yesQuit.onClick.AddListener(onClickQuitYesButton);
        noQuit.onClick.AddListener(onClickQuitNoButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton);

        winHome.onClick.AddListener(onClickHomeButton);
        winRestart.onClick.AddListener(onClickRestartButton);
        winNext.onClick.AddListener(onClickNextButton);
        failHome.onClick.AddListener(onClickHomeButton);
        failRestart.onClick.AddListener(onClickRestartButton);
        failNext.onClick.AddListener(onClickNextButton);
    }

    // 종료 버튼 클릭 시 호출
    private void onClickQuitButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupQuit.SetActive(true); // 종료 팝업 활성화
    }

    // 종료 확인 버튼 클릭 시 호출
    private void onClickQuitYesButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupQuit.SetActive(false); // 종료 팝업 비활성화
        GameManager.MoveScene("MainScene"); // 메인 씬으로 이동
    }

    // 종료 취소 버튼 클릭 시 호출
    private void onClickQuitNoButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupQuit.SetActive(false); // 종료 팝업 비활성화
    }

    // 홈 버튼 클릭 시 호출
    private void onClickHomeButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("MainScene"); // 메인 씬으로 이동
    }

    // 재시작 버튼 클릭 시 호출
    private void onClickRestartButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("BattleScene"); // 전투 씬으로 이동
    }

    // 다음 단계 버튼 클릭 시 호출
    private void onClickNextButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("ManageScene"); // 관리 씬으로 이동
    }

    // 설정 버튼 클릭 시 호출
    private void OnClickSettingButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(true); // 설정 팝업 활성화
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value)); // 볼륨 조절 슬라이더 이벤트 등록
    }

    // 설정 팝업의 닫기 버튼 클릭 시 호출
    private void OnClickSettingCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.s_Instance.TogglePause(); // 일시 정지 토글
        popupSetting.SetActive(false); // 설정 팝업 비활성화
    }

    // 이기는 상황에서 텍스트 설정 메서드
    public void SetWinText(int gold, int infamy)
    {
        // 플레이어에게 금화 및 악명 추가
        player.AddGold(gold);
        player.AddInfamy(infamy);
        // 이기는 상황에서 화면에 표시될 텍스트 설정
        winGoldText.text = player.Gold.ToString() + "(+" + gold.ToString() + ")";
        WinInfamyText.text = player.Infamy.ToString() + "(+" + infamy.ToString() + ")";
    }

    // 지는 상황에서 텍스트 설정 메서드
    public void SetFailText(int gold, int infamy)
    {
        // 플레이어에게 금화 및 악명 추가
        player.AddGold(gold);
        player.AddInfamy(infamy);
        // 지는 상황에서 화면에 표시될 텍스트 설정
        failGoldText.text = player.Gold.ToString() + "(+" + gold.ToString() + ")";
        failInfamyText.text = player.Infamy.ToString() + "(" + infamy.ToString() + ")";
    }
}
