using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, 3 }, { 0, -6.5f, 3 }, { -3, -6.5f, 3 },
        { 3, -6.5f, 6 }, { 0, -6.5f, 6 }, { -3, -6.5f, 6 },
    };

    public int heroCount;
    public int locationNumber; // 012 전열 345 후열 
    BattleManager battleManager;

    void Start()
    {
        for (locationNumber = 0; locationNumber < heroCount; locationNumber++)
        {
            Vector3 startPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);
            Character spawnCharacter = CharacterFactory.CreateHero();
            GameObject prefab = spawnCharacter.Prefab;

            // 유닛 생성
            GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.Euler(0, 180, 0));
            spawnUnit.GetComponent<HeroController>().SetCharacter(spawnCharacter);
            spawnUnit.GetComponent<HeroController>().SetSpawnNumber(locationNumber);
            spawnUnit.GetComponent<HeroController>().SetStartPosition(startPosition);
            spawnUnit.GetComponent<HeroController>().gameObject.SetActive(true);

            battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
            battleManager.heroesInBattle.Add(spawnUnit);
            battleManager.heroNumber.Add(locationNumber);
            battleManager.heroNumber.Sort();
            battleManager.heroCps.Add(spawnCharacter.GetCP());
        }
    }
}
