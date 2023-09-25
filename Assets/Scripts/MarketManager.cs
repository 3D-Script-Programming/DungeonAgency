using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    public MarketUIManager uiManager;
    private List<Character> newMonsters;
    private GameObject spawnUnit;

    public GameObject positionPivot; // 몬스터 스폰 위치
    public AudioClip backgroundSound; // 배경 음악

    private void Awake()
    {
        newMonsters = new List<Character>();
    }

    private void Start()
    {
        newMonsters = CharacterFactory.CreateMonsterList(10); // 새로운 몬스터 목록 생성
        uiManager.InstantiateListItems(newMonsters);
        GameManager.s_Instance.SetMusic(backgroundSound); // 배경 음악 설정
    }

    // 몬스터 스폰 함수
    public void SpawnMonster(Character monster)
    {
        if (spawnUnit != null)
            Destroy(spawnUnit);

        spawnUnit = Instantiate(monster.Prefab);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.transform.position = positionPivot.transform.position;
        spawnUnit.transform.rotation = positionPivot.transform.rotation;
        spawnUnit.SetActive(true);
    }
}