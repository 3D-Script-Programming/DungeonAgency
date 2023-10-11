using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    // 배틀 매니저 객체
    [SerializeField]
    private BattleManager battleManager;

    // 각 용사의 초기 위치 설정
    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, 3 }, { 0, -6.5f, 3 }, { -3, -6.5f, 3 },
        { 3, -6.5f, 6 }, { 0, -6.5f, 6 }, { -3, -6.5f, 6 },
    };

    // 용사 캐릭터 리스트
    public List<Character> Heros { get; set; }

    void OnEnable()
    {
        // 용사 위치 순회
        for (int locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            // 용사가 null이거나 HP가 0인 경우
            if (Heros[locationNumber] == null || Heros[locationNumber].HP == 0)
            {
                Heros[locationNumber] = null;
            }
            else
            {
                // 용사의 시작 위치 설정
                Vector3 startPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);

                // 용사의 프리팹 및 게임 오브젝트 생성
                Character spawnCharacter = Heros[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;
                GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.Euler(0, 180, 0));

                // 용사 컨트롤러 설정
                HeroController heroController = spawnUnit.GetComponent<HeroController>();
                heroController.SetCharacter(spawnCharacter);
                heroController.SetBattleManager(battleManager);
                heroController.SpawnNumber = locationNumber;
                heroController.SetStartPosition(startPosition);
                heroController.gameObject.SetActive(true);

                // 배틀 매니저에 용사 정보 추가
                battleManager.heroesInBattle.Add(spawnUnit);
                battleManager.heroNumber.Add(locationNumber);
                battleManager.heroNumber.Sort();
                battleManager.heroCps.Add(spawnCharacter.GetCP());

                // 첫 번째 용사 스폰인 경우, 전체 용사 CP 합산
                if (battleManager.isFirstHeroSpawn)
                    battleManager.sumHeroCp += spawnCharacter.GetCP();
            }
        }

        // 용사 리로드 락 해제
        battleManager.reloadHeroLock = false;
    }
}
