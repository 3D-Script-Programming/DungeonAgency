using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonUIManager UIManager { get; private set; }

    // 몬스터 스포너 오브젝트 배열
    public List<GameObject> monsterSpawners;

    public AudioClip backgroundSound;

    private void Awake()
    {
        // DungeonUIManager 컴포넌트를 가져와서 UIManager에 할당
        UIManager = gameObject.GetComponent<DungeonUIManager>();
    }

    private void Start()
    {
        // 초기화 함수 호출
        intialize();

        // 아이템 생성 함수 호출
        UIManager.InstantiateItems();
        // 배경음악을 설정합니다.
        GameManager.s_Instance.SetMusic(backgroundSound);
    }

    private void intialize()
    {
        // GameManager의 player가 속한 첫 번째 룸의 몬스터를 스폰
        DungeonRoom room = GameManager.s_Instance.player.GetRoom(0);
        // 0 번 룸 몬스터 스폰
        for (int i = 0; i < 6; i++)
        {
            Character monster = room.Monsters[i];
            SpawnMonster(monster, i);
        }
    }

    // SetMonsterSpawnerColor 함수는 몬스터 스포너의 색상을 설정합니다.
    // 인자로 받은 인덱스에 해당하는 몬스터 스포너 오브젝트의 색상을 주어진 색상으로 변경합니다.
    // Parameters:
    //   index: 몬스터 스포너 배열에서 설정할 인덱스
    //   color: 설정할 몬스터 스포너의 색상
    public void SetMonsterSpawnerColor(int index, Color color)
    {
        if (index != -1)
        {
            // 인덱스에 해당하는 몬스터 스포너 오브젝트의 렌더러 컴포넌트의 색상을 변경합니다.
            monsterSpawners[index].GetComponent<Renderer>().material.color = color;
        }
    }

    // ResetMonsterSpawnerColors 함수는 모든 몬스터 스포너의 색상을 흰색으로 초기화 설정합니다.
    public void ResetMonsterSpawnerColors()
    {
        for (int i = 0; i < 6; i++)
        {
            SetMonsterSpawnerColor(i, new Color(255, 255, 255));
        }
    }

    // RemovePreviousMonster 함수는 몬스터를 배치할 때, Replace를 선택할 경우 호출되는 함수입니다. 
    // 해당하는 몬스터를 찾아서, 오브젝트를 제거해주고, 룸에서 몬스터를 지워줍니다.
    // Parameters:
    //   selectedMonster: 제거할 몬스터 Character 객체
    //   selectedRoom: 몬스터가 존재하는 룸
    public void RemovePreviousMonster(Character selectedMonster, DungeonRoom selectedRoom)
    {
        // Monster의 태그를 가진 오브젝트들을 찾습니다.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        // 모든 몬스터 오브젝트를 순회하여 UniqueID에 해당하는 몬스터를 제거합니다.
        foreach (GameObject monster in monsters)
        {
            Character monsterInfo = monster.GetComponent<NonBattleMonsterController>().MonsterInfo;
            if (monsterInfo.UniqueID == selectedMonster.UniqueID)
            {
                // 몬스터 오브젝트 제거
                Destroy(monster);
                break;
            }
        }

        // 룸에서 monster를 제거해줍니다.
        selectedRoom.RemoveMonster(selectedMonster);
    }

    // SpawnMonster 함수는 몬스터를 스폰하는 함수입니다.
    // 몬스터를 지정된 위치에 스폰하고 활성화합니다.
    // Parameters:
    //   selectedMonster: 스폰할 몬스터의 Character 객체
    //   selectedPosition: 몬스터 스포너 번호
    public void SpawnMonster(Character selectedMonster, int selectedPosition)
    {
        if (selectedPosition == -1)
            return;

        if (selectedMonster != null)
        {
            Transform spawnTransform = monsterSpawners[selectedPosition].transform;

            // 자식 오브젝트가 있는지 확인 후 제거
            foreach (Transform child in spawnTransform)
            {
                Destroy(child.gameObject);
            }

            // 몬스터를 스폰합니다.
            GameObject spawnUnit = Instantiate(selectedMonster.Prefab, spawnTransform.position, Quaternion.identity);
            spawnUnit.transform.parent = spawnTransform;

            // 몬스터 전투 컨트롤러를 비활성화합니다.
            spawnUnit.GetComponent<MonsterController>().enabled = false;
            // 비전투 몬스터 컨트롤러를 활성화하고, Character 정보를 넘겨줍니다.
            NonBattleMonsterController nonBattleMonsterController = spawnUnit.GetComponent<NonBattleMonsterController>();
            nonBattleMonsterController.enabled = true;
            nonBattleMonsterController.MonsterInfo = selectedMonster;

            // 몬스터를 활성화합니다.
            spawnUnit.SetActive(true);
        }

        // 몬스터 스포너의 색상을 초기화 (흰색으로 변경)
        SetMonsterSpawnerColor(selectedPosition, new Color(255, 255, 255));
        UIManager.selectedPosition = -1;
    }

    // RemoveSpawnMonster 함수는 스폰된 몬스터를 제거하는 함수입니다.
    // 지정된 위치에 스폰된 몬스터 오브젝트를 제거합니다.
    // Parameters:
    //   selectedPosition: 몬스터 스포너 번호
    public void RemoveSpawnMonster(int selectedPosition)
    {
        if (selectedPosition == -1)
            return;

        GameObject targetObject = monsterSpawners[selectedPosition].transform.GetChild(0).gameObject;
        if (targetObject != null)
        {
            Character targetMonsterInfo = targetObject.GetComponent<NonBattleMonsterController>().MonsterInfo;
            // 룸에서 monster를 제거해줍니다.
            UIManager.selectedRoom.RemoveMonster(targetMonsterInfo);
            // 몬스터 오브젝트 제거
            Destroy(targetObject);
        }

        UIManager.selectedPosition = -1;
    }

    // RemoveSpawnMonsters 함수는 스폰된 몬스터를 제거하는 함수입니다.
    // 태그가 "Monster"인 모든 스폰된 몬스터 오브젝트를 제거합니다.
    public void RemoveSpawnMonsters()
    {
        // 스폰된 몬스터들을 제거합니다.
        foreach (GameObject spawnedMonster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            // 몬스터 오브젝트 제거
            Destroy(spawnedMonster);
        }
    }

    // ChangeRoom 함수는 현재 룸을 변경하는 함수입니다.
    // 이전에 스폰된 몬스터들을 제거하고, 새로운 룸에 있는 몬스터를 스폰합니다.
    // Parameters:
    //   newRoom: 변경할 새로운 던전 룸의 DungeonRoom 객체
    public void ChangeRoom(DungeonRoom newRoom)
    {
        // 이전에 스폰된 몬스터들을 제거
        RemoveSpawnMonsters();

        // 새로운 룸에 있는 몬스터를 스폰
        for (int i = 0; i < 6; i++)
        {
            Character monster = newRoom.Monsters[i];
            SpawnMonster(monster, i);
        }

        // 모든 몬스터 스포너의 색상을 초기화 (흰색으로 변경)
        ResetMonsterSpawnerColors();
    }
}