using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    public Character Boss { get; set; }

    void OnEnable()
    {
        Boss.SetBoss();
        GameObject prefab = Boss.Prefab;
        GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.identity);
        spawnUnit.transform.localScale = new Vector3(2f, 2f, 2f);
        MonsterController monsterController = spawnUnit.GetComponent<MonsterController>();
        monsterController.SetCharacter(Boss);
        monsterController.SetBattleManager(battleManager);
        monsterController.SpawnNumber = 0;
        monsterController.gameObject.SetActive(true);

        battleManager.monsterInBattle.Add(spawnUnit);
        battleManager.monsterNumber.Add(0);
        battleManager.monsterCps.Add(Boss.GetCP());
        battleManager.sumMonsterCp += Boss.GetCP();
        battleManager.monsterCount++;
        battleManager.reloadMonsterLock = false;
    }
}
