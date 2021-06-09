using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    public int locationNumber; // 012 전열 345 후열 
    BattleManager battleManager;

    void Start()
    {
        Character spawnCharacter = CharacterFactory.CreateHero();
        GameObject prefab = spawnCharacter.Prefab;

        // 유닛 생성
        GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.Euler(0, 180, 0));
        spawnUnit.GetComponent<HeroState>().SetCharacter(spawnCharacter);
        spawnUnit.GetComponent<HeroState>().SetSpawnNumber(locationNumber);

        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        battleManager.heroesInBattle.Add(spawnUnit);
        battleManager.heroNumber.Add(locationNumber);
        battleManager.heroNumber.Sort();
        battleManager.heroCps.Add(spawnCharacter.GetCP());
    }
}
