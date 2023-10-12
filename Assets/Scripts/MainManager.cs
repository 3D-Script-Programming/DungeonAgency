using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // UI 관리 클래스
    public MainUIManager UIManager { get; private set; }

    // 몬스터 목록
    private List<Character> monsters;

    // 오디오 관련 오브젝트
    public AudioClip backgroundSound;

    private void Awake()
    {
        UIManager = gameObject.GetComponent<MainUIManager>();
    }

    private void Start()
    {
        // UI 관리 클래스 초기화
        UIManager.Initialize();

        // 배경음악 재생 
        GameManager.s_Instance.SetMusic(backgroundSound);

        // 몬스터 스폰
        SpawnMonsters();
    }

    // 몬스터를 스폰하는 메서드
    private void SpawnMonsters()
    {
        // 몬스터 리스트 가져오기
        monsters = GameManager.s_Instance.player.GetMonsterList();

        if (monsters == null)
            return;

        // 몬스터 리스트 섞기
        Shuffle(monsters);

        // 최대 3개의 몬스터만 스폰
        int monsterCount = Mathf.Min(monsters.Count, 3);

        for (int i = 0; i < monsterCount; i++)
        {
            // 스폰 위치 계산
            Vector3 spawnPosition = new Vector3(-3.5f + (i * 3.5f), 0, 0);

            // 몬스터 스폰
            SpawnMonster(monsters[i], spawnPosition);
        }
    }

    // 리스트를 무작위로 섞는 메서드
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            T tmp = list[random];
            list[random] = list[i];
            list[i] = tmp;
        }
    }

    // 몬스터를 스폰하는 메서드
    private void SpawnMonster(Character monster, Vector3 spawnPosition)
    {
        GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition, Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }
}