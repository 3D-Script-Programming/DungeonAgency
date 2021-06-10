using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int locationNumber; // 012 전열 345 후열 
    public int currentRoom;
    BattleManager battleManager;

    void Start()
    {
        if (GameManager.instance.Player.GetMonsterList().Count > locationNumber)
        {
            Character spawnCharacter = GameManager.instance.Player.GetMonster(locationNumber);
            GameObject prefab = spawnCharacter.Prefab;

            // 유닛 생성
            GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.identity);
            spawnUnit.GetComponent<MonsterController>().SetCharacter(spawnCharacter);
            spawnUnit.GetComponent<MonsterController>().SetSpawnNumber(locationNumber);
            spawnUnit.GetComponent<MonsterController>().gameObject.SetActive(true);

            battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
            battleManager.monsterInBattle.Add(spawnUnit);
            battleManager.monsterNumber.Add(locationNumber);
            battleManager.monsterNumber.Sort();
            battleManager.monsterCps.Add(spawnCharacter.GetCP());
        }
    }
}
