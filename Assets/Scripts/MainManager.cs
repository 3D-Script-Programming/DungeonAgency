﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    GameManager gameManager;
    private List<Character> monsters;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        
        monsters = gameManager.Player.GetMonsterList();
        Debug.Log(monsters.Count);
        Shuffle(monsters);

        monsters = monsters.GetRange(0, 3);
        for (int i = 0; i < monsters.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(-3.5f + (i * 3.5f), 0, 0);
            SpawnMonster(monsters[i], spawnPosition);
        }
    }
     
    private void Shuffle<T>(List<T> list)
    {
        int random;
        T tmp;
 
        for (int i = 0; i < list.Count; i++)
        {
            random = Random.Range(0, list.Count);

            tmp = list[random];
            list[random] = list[i];
            list[i] = tmp;
        }
    }

    private void SpawnMonster(Character monster, Vector3 spawnPosition) {
        GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition,Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }
}
