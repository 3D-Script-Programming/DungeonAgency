using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonManager : MonoBehaviour
{
    // 몬스터 스포너 오브젝트 배열과 해당 버튼 텍스트 배열
    public GameObject[] monsterSpawners = new GameObject[6];
    public Text[] monsterSpawnButtons = new Text[6];

    // 왕관 및 보물 버튼과 체크 아이콘 게임 오브젝트
    public GameObject crownButton;
    public GameObject crownCheckIcon;
    public GameObject treasureButton;
    public GameObject treasureCheckIcon;

    // 방 이동 버튼 및 홈, 플레이, 마켓 버튼
    public Button prevRoomButton;
    public Button nextRoomButton;
    public Button homeButton;
    public Button playButton;
    public Button marketButton;

    // 현재 방 번호를 표시하는 텍스트
    public Text roomNumberText;

    // 왕관과 보물의 개수를 표시하는 TextMeshProUGUI 텍스트
    public TextMeshProUGUI crownCountText;
    public TextMeshProUGUI treasureCountText;

    // 악명 및 골드를 표시하는 TextMeshProUGUI 텍스트
    public TextMeshProUGUI evilPointText;
    public TextMeshProUGUI goldText;

    // 배경 음악 및 버튼 클릭 소리
    public AudioClip backgroundSound;
    public AudioClip buttonSound;

    // 경고창 : 배치된 몬스터가 없음, 몬스터가 이미 배치되어 있음
    public GameObject Popup_NoMonsters;
    public GameObject Popup_AlreadyMonsterInRoom;

    // 선택된 던전 룸 번호
    private int selectedRoomNumber = 0;

    // 선택된 몬스터 스포너 위치
    private int selectedPosition = -1;

    // 선택된 몬스터
    private Character selectedMonster;

    // 던전 룸 객체 
    private DungeonRoom selectedRoom;

    // 플레이어 객체 
    private Player player;

    // 오디오 소스 객체 
    private AudioSource audioSource;

    void Start()
    {
        // GameManager에서 플레이어 객체를 가져옵니다.
        player = GameManager.s_Instance.player;

        // 초기 선택된 던전 룸은 0번째 룸입니다.
        selectedRoom = player.GetRoom(0);

        // 이벤트 리스너를 등록합니다.
        ApplyEvents();

        // UI를 업데이트합니다.
        UpdateUI();

        // 배경음악을 설정합니다.
        GameManager.s_Instance.SetMusic(backgroundSound);

        // 오디오 소스를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
    }

    // ApplyEvents 함수는 버튼 및 몬스터 스포너에 대한 이벤트 리스너를 등록합니다.
    void ApplyEvents()
    {
        // 몬스터 스포너 버튼에 대한 이벤트 리스너를 등록합니다.
        for (int i = 0; i < 6; i++)
        {
            int now = i;
            // 람다 표현식을 사용하여 몬스터 스포너 버튼 클릭 이벤트를 설정합니다.
            monsterSpawnButtons[i].GetComponent<Button>().onClick.AddListener(() => OnClickSpawner(now));
        }

        // 왕관 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        crownButton.GetComponent<Button>().onClick.AddListener(OnClickCrown);

        // 보물 아이템 버튼에 대한 이벤트 리스너를 등록합니다.
        treasureButton.GetComponent<Button>().onClick.AddListener(OnClickTreasure);

        // 이전 룸으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        prevRoomButton.onClick.AddListener(OnClickPrevRoomButton);

        // 다음 룸으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        nextRoomButton.onClick.AddListener(OnClickNextRoomButton);

        // 홈으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        homeButton.onClick.AddListener(() => GameManager.MoveScene("MainScene"));

        // 시장으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        marketButton.onClick.AddListener(() => GameManager.MoveScene("MarketScene"));

        // 전투 화면으로 이동하는 버튼에 대한 이벤트 리스너를 등록합니다.
        playButton.onClick.AddListener(OnClickPlayButton);
    }

    public void OnClickPlayButton()
    {
        bool noMonstersInAnyRoom = true;

        // player 객체에 있는 모든 룸을 순회하면서 몬스터가 있는지 확인합니다.
        foreach (DungeonRoom room in player.GetRooms())
        {
            if (!room.NoMonstersInRoom())
            {
                noMonstersInAnyRoom = false;
                break; // 하나의 룸에서라도 몬스터가 있다면 루프를 종료합니다.
            }
        }

        if (noMonstersInAnyRoom)
        {
            Popup_NoMonsters.SetActive(true);
        }
        else
        {
            GameManager.MoveScene("BattleScene");
        }
    }

    // SetMonsterSpawnerColor 함수는 몬스터 스포너의 색상을 설정합니다.
    // 인자로 받은 인덱스에 해당하는 몬스터 스포너 오브젝트의 색상을 주어진 색상으로 변경합니다.
    // Parameters:
    //   index: 몬스터 스포너 배열에서 설정할 인덱스
    //   color: 설정할 몬스터 스포너의 색상
    void SetMonsterSpawnerColor(int index, Color color)
    {
        // 인덱스에 해당하는 몬스터 스포너 오브젝트의 렌더러 컴포넌트의 색상을 변경합니다.
        monsterSpawners[index].GetComponent<Renderer>().material.color = color;
    }

    // ResetMonsterSpawnerColors 함수는 모든 몬스터 스포너의 색상을 흰색으로 초기화 설정합니다.
    private void ResetMonsterSpawnerColors()
    {
        for (int i = 0; i < 6; i++)
        {
            SetMonsterSpawnerColor(i, new Color(255, 255, 255));
        }
    }

    // OnClickSpawner 함수는 몬스터 스포너 버튼을 클릭했을 때 호출됩니다.
    // 선택된 몬스터 스포너의 색상을 변경하고, 선택된 위치를 저장합니다.
    // Parameters:
    //   selected: 클릭된 몬스터 스포너의 인덱스
    void OnClickSpawner(int selected)
    {
        // 이미 선택된 몬스터 스포너라면 선택을 해제하고 함수를 종료합니다.
        if (this.selectedPosition == selected)
        {
            this.selectedPosition = -1;
            SetMonsterSpawnerColor(selected, new Color(255, 255, 255)); // 선택 해제된 스포너의 색상을 흰색으로 변경
            return;
        }

        // 모든 몬스터 스포너의 색상을 흰색으로 초기화합니다.
        ResetMonsterSpawnerColors();

        // 선택한 몬스터 스포너의 인덱스를 저장하고 해당 몬스터 스포너의 색상을 빨간색으로 변경합니다.
        this.selectedPosition = selected;
        SetMonsterSpawnerColor(selected, new Color(255, 0, 0));
    }

    // OnClickListItem 함수는 몬스터 아이템 리스트에서 몬스터 아이템을 클릭했을 때 호출됩니다.
    // 선택된 몬스터를 현재 선택된 몬스터 스포너 위치에 배치하고 UI를 업데이트합니다.
    // Parameters:
    //   monster: 클릭된 몬스터 아이템에 해당하는 Character 객체
    public void OnClickListItem(Character monster)
    {
        // 선택된 몬스터 스포너 위치가 없으면 함수를 종료합니다.
        if (selectedPosition == -1)
            return;

        // 선택한 몬스터 스포너 위치에 이미 몬스터가 배치되어 있는지 확인
        if (monster.IsPlacedInRoom)
        {
            // 이미 배치된 몬스터라면 경고창을 띄웁니다.
            Popup_AlreadyMonsterInRoom.SetActive(true);
            selectedMonster = monster;
        }
        else
        {
            // 이미 배치되지 않은 몬스터인 경우 배치 처리를 수행
            selectedRoom.PlaceMonster(selectedPosition, monster);
            SpawnMonster(monster, monsterSpawners[selectedPosition].transform.position);
        }
    }

    // RemovePreviousMonster 함수는 몬스터를 배치할때, Replace를 선택할 경우 호출되는 함수입니다. 
    // 해당하는 몬스터를 찾아서, 오브젝트를 제거해주고, 룸에서 몬스터를 지워줍니다.
    // Parameters:
    //   monster: 제거할 몬스터 Character 객체
    private void RemovePreviousMonster(Character monster)
    {
        // 이전 몬스터의 GameObject를 찾아서 제거합니다.
        GameObject monsterObject = GameObject.Find(monster.Name + "(Clone)");
        if (monsterObject != null)
        {
            Destroy(monsterObject);
        }

        // 룸에서 monster를 제거해줍니다.
        selectedRoom.RemoveMonster(monster);
    }

    // OnClickReplaceButton 함수는 몬스터 교체 경고창이 떴을 때, Yes 버튼을 클릭한 경우 호출됩니다.
    // 몬스터를 이전 배치에서 새로운 배치로 옮깁니다.
    public void OnClickReplaceButton()
    {
        if (selectedMonster != null)
        {
            // 이미 배치된 몬스터를 제거하고 새 몬스터를 배치합니다.
            RemovePreviousMonster(selectedMonster);
            selectedRoom.PlaceMonster(selectedPosition, selectedMonster);
            SpawnMonster(selectedMonster, monsterSpawners[selectedPosition].transform.position);
        }
        selectedMonster = null;
        Popup_AlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
    }

    // OnClickCancelButton 함수는 몬스터 교체 경고창이 떴을 때, No 버튼을 클릭한 경우 호출됩니다.
    // 경고창을 닫습니다. 
    public void OnClickCancelButton()
    {
        selectedMonster = null;
        Popup_AlreadyMonsterInRoom.SetActive(false); // 경고창 닫기
    }

    // OnClickCrown 함수는 왕관 아이템 버튼을 클릭했을 때 호출됩니다.
    // 왕관 아이템을 사용하거나 사용 취소하고 UI를 업데이트합니다.
    public void OnClickCrown()
    {
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
        // 이전 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber - 1 + player.GetRoomCount()) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();
        ChangeRoom(selectedRoom);
    }

    // OnClickNextRoomButton 함수는 다음 룸으로 이동하는 버튼을 클릭했을 때 호출됩니다.
    // 선택된 던전 룸 번호를 다음 번호로 갱신하고, UI를 업데이트하며 해당 룸의 정보로 변경합니다.
    public void OnClickNextRoomButton()
    {
        // 다음 룸 번호를 계산하고 선택된 던전 룸을 변경합니다.
        selectedRoomNumber = (selectedRoomNumber + 1) % player.GetRoomCount();
        selectedRoom = player.GetRoom(selectedRoomNumber);

        // 선택된 던전 룸 번호를 화면에 표시합니다.
        roomNumberText.text = selectedRoomNumber.ToString();

        // UI를 업데이트하고, 해당 룸의 정보로 변경합니다.
        UpdateUI();
        ChangeRoom(selectedRoom);
    }

    // SpawnMonster 함수는 몬스터를 스폰하는 함수입니다.
    // 몬스터를 지정된 위치에 스폰하고 활성화합니다.
    // Parameters:
    //   monster: 스폰할 몬스터의 Character 객체
    //   spawnPosition: 몬스터를 스폰할 위치 (Vector3)
    private void SpawnMonster(Character monster, Vector3 spawnPosition)
    {
        if (monster != null)
        {
            // 몬스터를 스폰합니다.
            GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition, Quaternion.identity);

            // 몬스터 컨트롤러를 비활성화하고 비전투 몬스터 컨트롤러를 활성화합니다.
            spawnUnit.GetComponent<MonsterController>().enabled = false;
            spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;

            // 몬스터를 활성화합니다.
            spawnUnit.SetActive(true);
        }
    }

    // RemoveSpawnMonsters 함수는 스폰된 몬스터들을 제거하는 함수입니다.
    // 태그가 "Monster"인 모든 스폰된 몬스터 오브젝트를 제거합니다.
    private void RemoveSpawnMonsters()
    {
        // 스폰된 몬스터들을 제거합니다.
        foreach (GameObject spawnedMonster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(spawnedMonster);
        }
    }

    // ChangeRoom 함수는 현재 룸을 변경하는 함수입니다.
    // 이전에 스폰된 몬스터들을 제거하고, 새로운 룸에 있는 몬스터를 스폰합니다.
    // Parameters:
    //   newRoom: 변경할 새로운 던전 룸의 DungeonRoom 객체
    private void ChangeRoom(DungeonRoom newRoom)
    {
        // 이전에 스폰된 몬스터들을 제거
        RemoveSpawnMonsters();

        // 새로운 룸에 있는 몬스터를 스폰
        for (int i = 0; i < 6; i++)
        {
            Character monster = newRoom.Monsters[i];
            Vector3 spawnPosition = monsterSpawners[i].transform.position;
            SpawnMonster(monster, spawnPosition);
        }

        // 모든 몬스터 스포너의 색상을 초기화 (흰색으로 변경)
        ResetMonsterSpawnerColors();

        // 선택된 몬스터 스포너 위치 초기화 (-1로 설정)
        this.selectedPosition = -1;
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
        evilPointText.text = player.Infamy.ToString();

        // UI에 플레이어의 골드를 업데이트합니다.
        goldText.text = player.Gold.ToString();
    }

    // ButtonSound 함수는 버튼 클릭 시 버튼 사운드를 재생하는 함수입니다.
    public void ButtonSound()
    {
        // 버튼 사운드를 한 번만 재생합니다.
        audioSource.PlayOneShot(buttonSound);
    }
}
