using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, -3 }, { 0, -6.5f, -3 }, { -3, -6.5f, -3 },
        { 3, -6.5f, -6 }, { 0, -6.5f, -6 }, { -3, -6.5f, -6 },
    };

    private int locationNumber; // 012 전열 345 후열
    private Character[] monster;

    BattleManager battleManager;

    void OnEnable()
    {
        for (locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            if(monster[locationNumber] != null)
            {
                Character spawnCharacter = monster[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;

                Vector3 sqawnPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);
                GameObject spawnUnit = Instantiate(prefab, sqawnPosition, Quaternion.identity);
                spawnUnit.GetComponent<MonsterController>().SetCharacter(spawnCharacter);
                spawnUnit.GetComponent<MonsterController>().SetSpawnNumber(locationNumber);
                spawnUnit.GetComponent<MonsterController>().gameObject.SetActive(true);

                battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
                battleManager.monsterInBattle.Add(spawnUnit);
                battleManager.monsterNumber.Add(locationNumber);
                battleManager.monsterNumber.Sort();
                battleManager.monsterCps.Add(spawnCharacter.GetCP());
                battleManager.sumMonsterCp += spawnCharacter.GetCP();
            } 
        }
        battleManager.reloadMonsterLock = false;
    }

    public Character[] GetMonster()
    {
        return monster;
    }

    public void SetMonster(Character[] value)
    {
        monster = value;
    }
}
