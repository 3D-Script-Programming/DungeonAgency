using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonUIManager UIManager { get; private set; }

    // 몬스터 스포너 오브젝트 배열
    public List<GameObject> monsterSpawners = new List<GameObject>(6);

    public AudioClip backgroundSound;

    private void Awake()
    {
        UIManager = gameObject.GetComponent<DungeonUIManager>();
    }

    private void Start()
    {
        UIManager.InstantiateItems();
        // 배경음악을 설정합니다.
        GameManager.s_Instance.SetMusic(backgroundSound);
    }

    // SetMonsterSpawnerColor 함수는 몬스터 스포너의 색상을 설정합니다.
    // 인자로 받은 인덱스에 해당하는 몬스터 스포너 오브젝트의 색상을 주어진 색상으로 변경합니다.
    // Parameters:
    //   index: 몬스터 스포너 배열에서 설정할 인덱스
    //   color: 설정할 몬스터 스포너의 색상
    public void SetMonsterSpawnerColor(int index, Color color)
    {
        // 인덱스에 해당하는 몬스터 스포너 오브젝트의 렌더러 컴포넌트의 색상을 변경합니다.
        monsterSpawners[index].GetComponent<Renderer>().material.color = color;
    }

    // ResetMonsterSpawnerColors 함수는 모든 몬스터 스포너의 색상을 흰색으로 초기화 설정합니다.
    public void ResetMonsterSpawnerColors()
    {
        for (int i = 0; i < 6; i++)
        {
            SetMonsterSpawnerColor(i, new Color(255, 255, 255));
        }
    }

    // RemovePreviousMonster 함수는 몬스터를 배치할때, Replace를 선택할 경우 호출되는 함수입니다. 
    // 해당하는 몬스터를 찾아서, 오브젝트를 제거해주고, 룸에서 몬스터를 지워줍니다.
    // Parameters:
    //   monster: 제거할 몬스터 Character 객체
    public void RemovePreviousMonster(Character monster, DungeonRoom selectedRoom)
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

    // SpawnMonster 함수는 몬스터를 스폰하는 함수입니다.
    // 몬스터를 지정된 위치에 스폰하고 활성화합니다.
    // Parameters:
    //   monster: 스폰할 몬스터의 Character 객체
    //   spawnPosition: 몬스터를 스폰할 위치 (Vector3)
    public void SpawnMonster(Character monster, int selectedPosition)
    {
        if (selectedPosition == -1)
            return;

        if (monster != null)
        {
            Vector3 spawnPosition = monsterSpawners[selectedPosition].transform.position;
            // 몬스터를 스폰합니다.
            GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition, Quaternion.identity);

            // 몬스터 컨트롤러를 비활성화하고 비전투 몬스터 컨트롤러를 활성화합니다.
            spawnUnit.GetComponent<MonsterController>().enabled = false;
            spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;

            // 몬스터를 활성화합니다.
            spawnUnit.SetActive(true);
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

    // RemoveSpawnMonsters 함수는 스폰된 몬스터들을 제거하는 함수입니다.
    // 태그가 "Monster"인 모든 스폰된 몬스터 오브젝트를 제거합니다.
    public void RemoveSpawnMonsters()
    {
        // 스폰된 몬스터들을 제거합니다.
        foreach (GameObject spawnedMonster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(spawnedMonster);
        }
    }
}
