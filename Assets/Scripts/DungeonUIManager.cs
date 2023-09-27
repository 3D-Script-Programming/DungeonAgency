using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonUIManager : MonoBehaviour
{
    public DungeonManager Manager { get; private set; }

    // 몬스터 스포너 위치에 대응하는 UI 버튼
    public List<Button> monsterSpawnButtons;

    // 왕관 및 보물 버튼과 체크 아이콘 게임 오브젝트
    public Button crownButton;
    public GameObject crownCheckIcon;
    public Button treasureButton;
    public GameObject treasureCheckIcon;

    // 방 이동 버튼 및 홈, 플레이, 마켓 버튼
    public Button prevRoomButton;
    public Button nextRoomButton;
    public Button mainButton;
    public Button playButton;
    public Button marketButton;
    public Button settingButton;
    public Slider soundSliderBar;

    // 현재 방 번호를 표시하는 텍스트
    public TextMeshProUGUI roomNumberText;

    // 왕관과 보물의 개수를 표시하는 TextMeshProUGUI 텍스트
    public TextMeshProUGUI crownCountText;
    public TextMeshProUGUI treasureCountText;

    // 악명 및 골드를 표시하는 TextMeshProUGUI 텍스트
    public TextMeshProUGUI infamyText;
    public TextMeshProUGUI goldText;

    // 경고창 : 배치된 몬스터가 없음, 몬스터가 이미 배치되어 있음
    public GameObject popupSetting;
    public GameObject popupWarning;
    public GameObject popupAlreadyMonsterInRoom;
    public Button YesButton;
    public Button NoButton;

    // 팝업 닫기 버튼
    public Button settingCloseButton;
    public Button warningCloseButton;

    // 선택된 던전 룸 번호
    private int selectedRoomNumber = 0;

    // 선택된 몬스터 스포너 위치
    public int selectedPosition = -1;

    // 선택된 몬스터
    private Character selectedMonster;

    // 던전 룸 객체 
    private DungeonRoom selectedRoom;

    public Transform scrollViewContent;

    private Player player;

    private void Awake()
    {
        Manager = gameObject.GetComponent<DungeonManager>();
        player = GameManager.s_Instance.player;
    }

    private void Start()
    {
        // 초기 선택된 던전 룸은 0번째 룸입니다.
        selectedRoom = player.GetRoom(0);

        // UI를 업데이트합니다.
        UpdateUI();

        // 이벤트 리스너를 등록합니다.
        ApplyEvents();
    }

    // UpdateUI 함수는 UI 업데이트를 수행하는 함수입니다.
    // 현재 선택된 던전 룸의 정보에 따라 UI 요소들을 업데이트합니다.
    private void UpdateUI()
    {
        // 왕관 아이템 체크 아이콘을 선택된 던전 룸의 아이템에 따라 활성화 또는 비활성화합니다.
        crownCheckIcon.SetActive(selectedRoom.Items == Item.CROWN);

        // 보물 아이템 체크 아이콘을 선택된 던전 룸의 아이템에 따라 활성화 또는 비활성화합니다.
        treasureCheckIcon.SetActive(selectedRoom.Items == Item.TREASURE);

        // UI에 현재 보유한 왕관 아이템 개수를 업데이트합니다.
        crownCountText.text = player.GetItem(Item.CROWN).ToString();

        // UI에 현재 보유한 보물 아이템 개수를 업데이트합니다.
        treasureCountText.text = player.GetItem(Item.TREASURE).ToString();

        // UI에 플레이어의 악명 점수를 업데이트합니다.
        infamyText.text = player.Infamy.ToString();

        // UI에 플레이어의 골드를 업데이트합니다.
        goldText.text = player.Gold.ToString();
    }

    // ApplyEvents 함수는 버튼 및 몬스터 스포너에 대한 이벤트 리스너를 등록합니다.
    void ApplyEvents()
    {
        // 몬스터 스포너 버튼에 대한 이벤트 리스너를 등록합니다.
        for (int i = 0; i < 6; i++)
        {
            int index = i; // 인덱스를 복제하여 클로저 변수로 사용합니다.

            // 람다 표현식을 사용하여 몬스터 스포너 버튼 클릭 이벤트를 설정합니다.
            monsterSpawnButtons[i].onClick.AddListener(() => OnClickSpawner(index));
        }

        // 홈으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        mainButton.onClick.AddListener(OnClickMainButton);

        // 시장으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        marketButton.onClick.AddListener(OnClickMarketButton);

        // 전투 화면으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        playButton.onClick.AddListener(OnClickPlayButton);

        // 이전 룸으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        prevRoomButton.onClick.AddListener(OnClickPrevRoomButton);

        // 다음 룸으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        nextRoomButton.onClick.AddListener(OnClickNextRoomButton);

        settingButton.onClick.AddListener(OnClickSettingButton);

        // 왕관 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        crownButton.onClick.AddListener(OnClickCrown);

        // 보물 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        treasureButton.onClick.AddListener(OnClickTreasure);

        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton);
        warningCloseButton.onClick.AddListener(OnClickWarningCloseButton);

        YesButton.onClick.AddListener(OnClickReplaceButton);
        NoButton.onClick.AddListener(OnClickCancelButton);
    }

    // OnClickSpawner 함수는 몬스터 스포너 버튼을 클릭했을 때 호출됩니다.
    // 선택된 몬스터 스포너의 색상을 변경하고, 선택된 위치를 저장합니다.
    // Parameters:
    //   selected: 클릭된 몬스터 스포너의 인덱스
    void OnClickSpawner(int selected)
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 이미 선택된 몬스터 스포너라면 선택을 해제하고 함수를 종료합니다.
        if (this.selectedPosition == selected)
        {
            this.selectedPosition = -1;
            Manager.SetMonsterSpawnerColor(selected, new Color(255, 255, 255)); // 선택 해제된 스포너의 색상을 흰색으로 변경
            return;
        }

        // 모든 몬스터 스포너의 색상을 흰색으로 초기화합니다.
        Manager.ResetMonsterSpawnerColors();

        // 선택한 몬스터 스포너의 인덱스를 저장하고 해당 몬스터 스포너의 색상을 빨간색으로 변경합니다.
        this.selectedPosition = selected;
        Manager.SetMonsterSpawnerColor(selected, new Color(255, 0, 0));
    }

    // 메인 버튼 클릭 시 호출되는 메서드
    private void OnClickMainButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("MainScene"); // 메인 씬으로 이동
    }

    // 메인 버튼 클릭 시 호출되는 메서드
    private void OnClickMarketButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        GameManager.MoveScene("MarketScene"); // 마켓 씬으로 이동
    }

    // Play 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickPlayButton()
    {
        // Play 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();

        // 팝업 관련 처리
        if (player.NotReadyForBattle())
        {
            // 경고 팝업 표시
            popupWarning.SetActive(true);
        }
        else
        {
            // 관리 씬으로 이동
            GameManager.MoveScene("BattleScene");
        }
    }

    // OnClickListItem 함수는 몬스터 아이템 리스트에서 몬스터 아이템을 클릭했을 때 호출됩니다.
    // 선택된 몬스터를 현재 선택된 몬스터 스포너 위치에 배치하고 UI를 업데이트합니다.
    // Parameters:
    //   monster: 클릭된 몬스터 아이템에 해당하는 Character 객체
    public void OnClickListItem(Character monster)
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 선택된 몬스터 스포너 위치가 없으면 함수를 종료합니다.
        if (selectedPosition == -1)
            return;

        // 선택한 몬스터 스포너 위치에 이미 몬스터가 배치되어 있는지 확인
        if (monster.CurrentRoomNumber != -1)
        {
            // 이미 배치된 몬스터라면 경고창을 띄웁니다.
            popupAlreadyMonsterInRoom.SetActive(true);
            selectedMonster = monster;
        }
        else
        {
            // 이미 배치되지 않은 몬스터인 경우 배치 처리를 수행
            selectedRoom.PlaceMonster(selectedPosition, monster);
            Manager.SpawnMonster(monster, selectedPosition);
        }
    }

    // OnClickReplaceButton 함수는 몬스터 교체 경고창이 떴을 때, Yes 버튼을 클릭한 경우 호출됩니다.
    // 몬스터를 이전 배치에서 새로운 배치로 옮깁니다.
    public void OnClickReplaceButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        if (selectedMonster != null)
        {
            // 몬스터를 제거합니다.
            if (selectedMonster.CurrentRoomNumber == selectedRoom.RoomNumber)
            {
                // 이미 배치된 몬스터를 제거하고 새 몬스터를 배치합니다.
                Manager.RemovePreviousMonster(selectedMonster, selectedRoom);
                selectedRoom.RemoveMonster(selectedMonster);
            }
            else
            {
                DungeonRoom targetMonsterRoom = GameManager.s_Instance.player.GetRoom(selectedMonster.CurrentRoomNumber);
                targetMonsterRoom.RemoveMonster(selectedMonster);
            }
            // 몬스터를 다시 배치하고 생성합니다.
            selectedRoom.PlaceMonster(selectedPosition, selectedMonster);
            Manager.SpawnMonster(selectedMonster, selectedPosition);
        }
        selectedMonster = null;
        popupAlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
    }

    // OnClickCancelButton 함수는 몬스터 교체 경고창이 떴을 때, No 버튼을 클릭한 경우 호출됩니다.
    // 경고창을 닫습니다. 
    public void OnClickCancelButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        selectedMonster = null;
        popupAlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
    }

    // OnClickCrown 함수는 왕관 아이템 버튼을 클릭했을 때 호출됩니다.
    // 왕관 아이템을 사용하거나 사용 취소하고 UI를 업데이트합니다.
    public void OnClickCrown()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        // 왕관 아이템 체크 아이콘이 활성화되어 있다면, 아이템 사용을 취소하고 아이콘을 비활성화합니다.
        if (crownCheckIcon.activeSelf)
        {
            player.UnuseCrown();
            crownCheckIcon.SetActive(false);
        }
        else if (player.GetItem(Item.CROWN) > 0)
        {
            // 왕관 아이템이 보유 중이고, 보물 아이템 체크 아이콘이 활성화되어 있다면, 보물 아이템 사용을 취소하고 아이콘을 비활성화합니다.
            if (treasureCheckIcon.activeSelf)
            {
                player.UnuseTreasure();
                treasureCheckIcon.SetActive(false);
            }
            // 왕관 아이템을 사용하고 던전 룸에 아이템을 배치합니다.
            player.UseCrown();
            selectedRoom.PlaceItem(Item.CROWN);
            crownCheckIcon.SetActive(true);
        }
        // UI에 왕관 아이템 개수를 업데이트합니다.
        crownCountText.text = player.GetItem(Item.CROWN).ToString();
    }

    // OnClickTreasure 함수는 보물 아이템 버튼을 클릭했을 때 호출됩니다.
    // 보물 아이템을 사용하거나 사용 취소하고 UI를 업데이트합니다.
    public void OnClickTreasure()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        // 보물 아이템 체크 아이콘이 활성화되어 있다면, 아이템 사용을 취소하고 아이콘을 비활성화합니다.
        if (treasureCheckIcon.activeSelf)
        {
            player.UnuseTreasure();
            treasureCheckIcon.SetActive(false);
        }
        else if (player.GetItem(Item.TREASURE) > 0)
        {
            // 보물 아이템이 보유 중이고, 왕관 아이템 체크 아이콘이 활성화되어 있다면, 왕관 아이템 사용을 취소하고 아이콘을 비활성화합니다.
            if (crownCheckIcon.activeSelf)
            {
                player.UnuseCrown();
                crownCheckIcon.SetActive(false);
            }
            // 보물 아이템을 사용하고 던전 룸에 아이템을 배치합니다.
            player.UseTreasure();
            selectedRoom.PlaceItem(Item.TREASURE);
            treasureCheckIcon.SetActive(true);
        }
        // UI에 보물 아이템 개수를 업데이트합니다.
        treasureCountText.text = player.GetItem(Item.TREASURE).ToString();
    }

    // OnClickPrevRoomButton 함수는 이전 룸으로 이동하는 버튼을 클릭했을 때 호출됩니다.
    // 선택된 던전 룸 번호를 이전 번호로 갱신하고, UI를 업데이트하며 해당 룸의 정보로 변경합니다.
    public void OnClickPrevRoomButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 이전 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber - 1 + player.GetRoomCount()) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = "Room Number : " + selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();
        Manager.ChangeRoom(selectedRoom);

        // 선택된 몬스터 스포너 위치 초기화 (-1로 설정)
        this.selectedPosition = -1;
    }

    // OnClickNextRoomButton 함수는 다음 룸으로 이동하는 버튼을 클릭했을 때 호출됩니다.
    // 선택된 던전 룸 번호를 다음 번호로 갱신하고, UI를 업데이트하며 해당 룸의 정보로 변경합니다.
    public void OnClickNextRoomButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 다음 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber + 1) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = "Room Number : " + selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();
        Manager.ChangeRoom(selectedRoom);
        // 선택된 몬스터 스포너 위치 초기화 (-1로 설정)
        this.selectedPosition = -1;
    }

    // Setting 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingButton()
    {
        // Setting 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        popupSetting.SetActive(true);
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value));
    }

    // Setting 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingCloseButton()
    {
        // Setting 팝업 닫기 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        popupSetting.SetActive(false);
    }

    // Warning 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickWarningCloseButton()
    {
        // Warning 팝업 닫기 버튼 클릭 시 처리
        GameManager.s_Instance.PlayButtonSound();
        popupWarning.SetActive(false);
    }

    public void InstantiateItems()
    {
        List<Character> monsters = player.GetMonsterList();
        foreach (Character monster in monsters)
        {
            GameObject listItem = Instantiate(Resources.Load<GameObject>("UI/ManageListItem"), scrollViewContent);
            ManageListItemController listItemController = listItem.GetComponent<ManageListItemController>();
            listItemController.SetText(monster);
            listItemController.SetDungeonManager(Manager);
        }
    }

}
