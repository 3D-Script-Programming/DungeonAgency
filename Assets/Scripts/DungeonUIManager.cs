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

    // 게임 셋팅 팝업 오브젝트
    public GameObject popupSetting;
    // 게임 스타트 경고 오브젝트 : 몬스터가 룸에 배치되어 있지 않은 경우 배틀에 입장할 수 없음
    public GameObject popupWarning;

    // 몬스터 배치 경고창 : 해당 몬스터는 이미 배치되어 있음
    public GameObject popupAlreadyMonsterInRoom;
    public Button alreadyMonsterYesButton;
    public Button alreadyMonsterNoButton;

    // 몬스터 배치 경고창 : 해당 몬스터를 룸에 제거할 것인가
    public GameObject popupRemoveMonster;
    public Button removeMonsterYesButton;
    public Button removeMonsterNoButton;

    // 팝업 닫기 버튼
    public Button settingCloseButton;
    public Button warningCloseButton;

    // 선택된 던전 룸 번호
    private int selectedRoomNumber = 0;

    // 선택된 몬스터 스포너 위치
    public int selectedPosition = -1;

    // 선택된 몬스터
    public Character selectedMonster;

    // 던전 룸 객체 
    public DungeonRoom selectedRoom;

    // 몬스터 리스트를 생성할 위치
    public Transform scrollViewContent;

    // Player 정보를 불러올 객체 
    private Player player;

    private void Awake()
    {
        // DungeonManager 컴포넌트를 가져와서 Manager에 할당
        Manager = gameObject.GetComponent<DungeonManager>();
        // GameManager 클래스에 있는 Player 정보 할당
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

        // 셋팅 버튼에 대한 이벤트 리스너를 등록합니다.
        settingButton.onClick.AddListener(OnClickSettingButton);
        settingCloseButton.onClick.AddListener(OnClickSettingCloseButton);

        // 경고창 팝업을 닫는 버튼에 대한 이벤트 리스너를 등록합니다.
        warningCloseButton.onClick.AddListener(OnClickWarningCloseButton);

        // 몬스터 배치 경고창 버튼에 대한 이벤트 리스너를 등록합니다.
        alreadyMonsterYesButton.onClick.AddListener(OnClickReplaceButton);
        alreadyMonsterNoButton.onClick.AddListener(OnClickReplaceCancelButton);
        removeMonsterYesButton.onClick.AddListener(onClickRemoveButton);
        removeMonsterNoButton.onClick.AddListener(onClickRemoveCancelButton);

        // 왕관 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        crownButton.onClick.AddListener(OnClickCrown);

        // 보물 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        treasureButton.onClick.AddListener(OnClickTreasure);
    }

    // OnClickSpawner 함수는 몬스터 스포너 버튼을 클릭했을 때 호출됩니다.
    // 선택된 몬스터 스포너의 색상을 변경하고, 선택된 위치를 저장합니다.
    // Parameters:
    //   selected: 클릭된 몬스터 스포너의 인덱스
    private void OnClickSpawner(int selected)
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 몬스터 스포너의 위치를 찾아옵니다.
        Transform spawnerTransform = Manager.monsterSpawners[selected].transform;

        // 몬스터 스포너에 몬스터가 이미 배치되어 있는지 확인합니다.
        // 자식 오브젝트가 있으면 이미 배치되어 있음.
        if (spawnerTransform.childCount > 0)
        {
            selectedPosition = selected;
            popupRemoveMonster.SetActive(true);
        }
        else
        {
            // 이미 선택된 몬스터 스포너라면 선택을 해제하고 함수를 종료합니다.
            if (selectedPosition == selected)
            {
                selectedPosition = -1;
                Manager.SetMonsterSpawnerColor(selected, new Color(255, 255, 255)); // 선택 해제된 스포너의 색상을 흰색으로 변경
                return;
            }

            // 모든 몬스터 스포너의 색상을 흰색으로 초기화합니다.
            Manager.ResetMonsterSpawnerColors();

            // 선택한 몬스터 스포너의 인덱스를 저장하고 해당 몬스터 스포너의 색상을 빨간색으로 변경합니다.
            selectedPosition = selected;
            Manager.SetMonsterSpawnerColor(selected, new Color(255, 0, 0));
        }
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
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

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

        // 선택한 몬스터가 이미 배치되어 있는지 확인
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
    private void OnClickReplaceButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        if (selectedMonster != null)
        {
            // 선택한 위치에 몬스터가 있는지 확인
            Character existingMonster = selectedRoom.Monsters[selectedPosition];

            if (existingMonster != null)
            {
                // 몬스터의 위치를 서로 교체합니다.
                SwapMonsters(selectedMonster, existingMonster);

                // 경고 팝업을 닫습니다.
                popupAlreadyMonsterInRoom.SetActive(false);
            }
            else
            {
                // 선택한 위치에 몬스터가 없는 경우, 교체를 진행합니다.
                // 이전 몬스터를 제거하고 새로운 위치에 몬스터를 배치합니다.
                Manager.RemovePreviousMonster(selectedMonster, player.GetRoom(selectedMonster.CurrentRoomNumber));
                selectedRoom.PlaceMonster(selectedPosition, selectedMonster);
                Manager.SpawnMonster(selectedMonster, selectedPosition);
                popupAlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
            }
        }

        selectedMonster = null;
    }

    // SwapMonsters 함수는 두 몬스터의 룸 배치를 교체할때 호출됩니다.
    // 두 몬스터의 위치를 변경합니다.
    // Parameters:
    //   monster1: 위치를 변경할 몬스터 객체
    //   monster2: 위치를 변경할 몬스터 객체
    private void SwapMonsters(Character monster1, Character monster2)
    {
        // 몬스터들이 배치된 룸 정보를 불러옵니다.
        DungeonRoom room1 = player.GetRoom(monster1.CurrentRoomNumber);
        DungeonRoom room2 = player.GetRoom(monster2.CurrentRoomNumber);
        // 몬스터들이 룸에 위치한 번호를 불러옵니다.
        int position1 = room1.GetMonsterPosition(monster1);
        int position2 = room2.GetMonsterPosition(monster2);

        // 스폰되어 있는 두 몬스터 오브젝트를 제거합니다.
        Manager.RemovePreviousMonster(monster1, room1);
        Manager.RemovePreviousMonster(monster2, room2);

        // 몬스터를 던전 룸에 다시 배치합니다.
        room1.PlaceMonster(position1, monster2);
        room2.PlaceMonster(position2, monster1);

        // 두 몬스터가 같은 룸에 있었을 경우 : selectedRoom과 동일합니다.
        if (room1.RoomNumber == room2.RoomNumber)
        {
            // 두 몬스터 오브젝트를 스폰합니다.
            Manager.SpawnMonster(monster2, position1);
            Manager.SpawnMonster(monster1, position2);

        }
        // 첫번째 몬스터만 현재 룸에 있는 경우
        else if (selectedRoom.RoomNumber == room1.RoomNumber)
            // 교체된 위치에 두번째 몬스터 오브젝트를 스폰합니다.
            Manager.SpawnMonster(monster2, position1);
        // 두번째 몬스터만 현재 룸에 있는 경우
        else
            // 교체된 위치에 첫번째 몬스터 오브젝트를 스폰합니다.
            Manager.SpawnMonster(monster1, position2);
    }

    // OnClickReplaceCancelButton 함수는 몬스터 교체 경고창이 떴을 때, No 버튼을 클릭한 경우 호출됩니다.
    // 경고창을 닫습니다. 
    private void OnClickReplaceCancelButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        selectedMonster = null; // 선택된 몬스터 초기화
        popupAlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
    }

    // OnClickCrown 함수는 왕관 아이템 버튼을 클릭했을 때 호출됩니다.
    // 왕관 아이템을 사용하거나 사용 취소하고 UI를 업데이트합니다.
    private void OnClickCrown()
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
    private void OnClickTreasure()
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
    private void OnClickPrevRoomButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 이전 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber - 1 + player.GetRoomCount()) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = "Room Number : " + selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();

        // DungeonManager에 있는 룸 변경 함수를 호출합니다.
        Manager.ChangeRoom(selectedRoom);

        // 선택된 몬스터 스포너 위치 초기화 (-1로 설정)
        this.selectedPosition = -1;
    }

    // OnClickNextRoomButton 함수는 다음 룸으로 이동하는 버튼을 클릭했을 때 호출됩니다.
    // 선택된 던전 룸 번호를 다음 번호로 갱신하고, UI를 업데이트하며 해당 룸의 정보로 변경합니다.
    private void OnClickNextRoomButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생

        // 다음 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber + 1) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = "Room Number : " + selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();

        // DungeonManager에 있는 룸 변경 함수를 호출합니다.
        Manager.ChangeRoom(selectedRoom);

        // 선택된 몬스터 스포너 위치 초기화 (-1로 설정)
        this.selectedPosition = -1;
    }

    // Setting 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        popupSetting.SetActive(true); // 팝업창 활성화

        // GameManager에 있는 ChangeVolume() 함수를 호출합니다.
        soundSliderBar.onValueChanged.AddListener((value) => GameManager.s_Instance.ChangeVolume(value));
    }

    // Setting 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickSettingCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        popupSetting.SetActive(false); // 팝업창 비활성화
    }

    // Warning 팝업의 닫기 버튼 클릭 시 호출되는 메서드입니다.
    private void OnClickWarningCloseButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        popupWarning.SetActive(false); // 팝업창 비활성화
    }

    // onClickRemoveButton 함수는 룸에 배치된 몬스터를 제거할때 호출되는 함수입니다.
    // Remove 팝업창이 활성화 될때 Yes 버튼을 클릭할 경우 몬스터를 제거합니다.
    private void onClickRemoveButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        Manager.RemoveSpawnMonster(selectedPosition); // DungeonManager에 있는 RemoveSpawnMonster() 함수 호출
        popupRemoveMonster.SetActive(false); // 팝업창을 비활성화 합니다.
    }

    // onClickRemoveCancelButton 함수는 몬스터 제거를 취소할때 호출되는 함수입니다.
    // Remove 팝업창이 활성화 될때 No 버튼을 클릭할 경우 몬스터 제거를 취소합니다.
    private void onClickRemoveCancelButton()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        selectedPosition = -1; // 선택된 위치 초기화
        popupRemoveMonster.SetActive(false); // 팝업창을 비활성화 합니다.
    }


    // InstantiateItems 함수는 새로운 몬스터 리스트를 생성할 때 호출되는 함수입니다. 
    // player 객체에 저장되어 있는 몬스터 정보를 가져와서 리스트로 생성해줍니다.
    public void InstantiateItems()
    {
        // 플레이어 정보를 불러옵니다.
        List<Character> monsters = player.GetMonsterList();
        foreach (Character monster in monsters)
        {
            // 몬스터 리스트를 생성합니다.
            GameObject listItem = Instantiate(Resources.Load<GameObject>("UI/ManageListItem"), scrollViewContent);
            ManageListItemController listItemController = listItem.GetComponent<ManageListItemController>();
            listItemController.SetText(monster);
            listItemController.SetDungeonManager(Manager);
        }
    }

}
