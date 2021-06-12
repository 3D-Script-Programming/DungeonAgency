using UnityEngine;
using System;

public class HeroSpawner : MonoBehaviour
{
    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, 3 }, { 0, -6.5f, 3 }, { -3, -6.5f, 3 },
        { 3, -6.5f, 6 }, { 0, -6.5f, 6 }, { -3, -6.5f, 6 },
    };

    private Character[] heroes;

    public int locationNumber; // 012 전열 345 후열 
    BattleManager battleManager;

    void OnEnable()
    {
        for (locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            if (heroes[locationNumber].GetHP() == 0)
            {
                heroes[locationNumber] = null;
            }
            else {
                Vector3 startPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);
                Character spawnCharacter = heroes[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;
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
                battleManager.sumHeroCp += spawnCharacter.GetCP();
            }
        }
        battleManager.reloadHeroLock = false;
    }

    public Character[] GetHeroes()
    {
        return heroes;
    }

    public void SetHeroes(Character[] value)
    {
        heroes = value;
    }
}
