using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    // MarketUIManager 참조를 저장하는 변수
    public MarketUIManager UIManager { get; private set; }

    // 새로운 몬스터 목록을 저장하는 리스트
    private List<Character> newMonsters;

    // 스폰된 몬스터의 게임 오브젝트를 저장하는 변수
    private GameObject spawnUnit;

    // 몬스터 스폰 위치를 지정하는 게임 오브젝트
    public GameObject positionPivot;

    // 배경 음악을 저장하는 오디오 클립
    public AudioClip backgroundSound;

    private void Awake()
    {
        UIManager = gameObject.GetComponent<MarketUIManager>();
    }

    private void Start()
    {
        // 10개의 새로운 몬스터 목록을 생성하고 UI에 표시
        newMonsters = CharacterFactory.CreateMonsterList(10);
        UIManager.InstantiateListItems(newMonsters);

        // 게임 매니저를 통해 배경 음악 설정
        GameManager.s_Instance.SetMusic(backgroundSound);
    }

    // 몬스터 스폰 함수
    public void SpawnMonster(Character monster)
    {
        // 이미 스폰된 몬스터가 있다면 제거
        if (spawnUnit != null)
            Destroy(spawnUnit);

        // 선택한 몬스터의 프리팹을 인스턴스화하고 위치 및 회전 설정
        spawnUnit = Instantiate(monster.Prefab);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.transform.position = positionPivot.transform.position;
        spawnUnit.transform.rotation = positionPivot.transform.rotation;
        spawnUnit.SetActive(true);
    }
}