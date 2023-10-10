using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, -3 }, { 0, -6.5f, -3 }, { -3, -6.5f, -3 },
        { 3, -6.5f, -6 }, { 0, -6.5f, -6 }, { -3, -6.5f, -6 },
    };

    public List<Character> Monsters { get; set; }

    void OnEnable()
    {
        for (int locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            if (Monsters[locationNumber] != null)
            {
                Character spawnCharacter = Monsters[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;
                spawnCharacter.SetResetHp();

                Vector3 spawnPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);
                GameObject spawnUnit = Instantiate(prefab, spawnPosition, Quaternion.identity);
                MonsterController monsterController = spawnUnit.GetComponent<MonsterController>();
                monsterController.SetCharacter(spawnCharacter);
                monsterController.SetBattleManager(battleManager);
                monsterController.SpawnNumber = locationNumber;
                monsterController.gameObject.SetActive(true);

                battleManager.monsterInBattle.Add(spawnUnit);
                battleManager.monsterNumber.Add(locationNumber);
                battleManager.monsterNumber.Sort();
                battleManager.monsterCps.Add(spawnCharacter.GetCP());
                battleManager.sumMonsterCp += spawnCharacter.GetCP();
                battleManager.monsterCount++;
            }
        }
        battleManager.reloadMonsterLock = false;
    }
}
