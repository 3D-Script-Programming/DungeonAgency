using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private Character boss;

    BattleManager battleManager;

    void OnEnable()
    {
        GameObject prefab = boss.Prefab;
        GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.identity);
        spawnUnit.transform.localScale = new Vector3(2f, 2f, 2f);
        spawnUnit.GetComponent<MonsterController>().SetCharacter(boss);
        spawnUnit.GetComponent<MonsterController>().GetCharacter().SetBoss();
        spawnUnit.GetComponent<MonsterController>().SetSpawnNumber(0);
        spawnUnit.GetComponent<MonsterController>().gameObject.SetActive(true);

        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        battleManager.monsterInBattle.Add(spawnUnit);
        battleManager.monsterNumber.Add(0);
        battleManager.monsterCps.Add(spawnUnit.GetComponent<MonsterController>().GetCharacter().GetCP());

        battleManager.reloadMonsterLock = false;
    }

    public Character GetBoss()
    {
        return boss;
    }

    public void SetBoss(Character value)
    {
        boss = value;
    }
}
