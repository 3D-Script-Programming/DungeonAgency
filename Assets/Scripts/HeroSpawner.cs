using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, 3 }, { 0, -6.5f, 3 }, { -3, -6.5f, 3 },
        { 3, -6.5f, 6 }, { 0, -6.5f, 6 }, { -3, -6.5f, 6 },
    };

    public List<Character> Heros { get; set; }

    void OnEnable()
    {
        for (int locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            if (Heros[locationNumber] == null) continue;
            if (Heros[locationNumber].HP == 0)
            {
                Heros[locationNumber] = null;
            }
            else
            {
                Vector3 startPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);
                Character spawnCharacter = Heros[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;
                GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.Euler(0, 180, 0));
                HeroController heroController = spawnUnit.GetComponent<HeroController>();
                heroController.SetCharacter(spawnCharacter);
                heroController.SetBattleManager(battleManager);
                heroController.SpawnNumber = locationNumber;
                heroController.SetStartPosition(startPosition);
                heroController.gameObject.SetActive(true);

                battleManager.heroesInBattle.Add(spawnUnit);
                battleManager.heroNumber.Add(locationNumber);
                battleManager.heroNumber.Sort();
                battleManager.heroCps.Add(spawnCharacter.GetCP());
                battleManager.sumHeroCp += spawnCharacter.GetCP();
            }
        }
        battleManager.reloadHeroLock = false;
    }
}
