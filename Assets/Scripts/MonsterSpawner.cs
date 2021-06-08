using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int locationNumber; // 012 전열 345 후열 
    public int currentRoom;

    void Start()
    {
        Character spawnCharacter = GameManager.instance.Player.GetMonster(locationNumber);
        GameObject prefab = spawnCharacter.Prefab;
                
        // 유닛 생성
        GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.identity);
        spawnUnit.GetComponent<MonsterState>().SetCharacter(spawnCharacter);


    }
}
