using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // 배틀 매니저 객체
    [SerializeField]
    private BattleManager battleManager;

    // 몬스터 초기 위치 설정
    private static readonly float[,] POSITIONS = new float[,] {
        { 3, -6.5f, -3 }, { 0, -6.5f, -3 }, { -3, -6.5f, -3 },
        { 3, -6.5f, -6 }, { 0, -6.5f, -6 }, { -3, -6.5f, -6 },
    };

    // 몬스터 캐릭터 리스트
    public List<Character> Monsters { get; set; }

    void OnEnable()
    {
        // 각 위치별로 몬스터 생성
        for (int locationNumber = 0; locationNumber < 6; locationNumber++)
        {
            // 몬스터가 null이 아닌 경우
            if (Monsters[locationNumber] != null)
            {
                // 몬스터 정보 설정 및 초기화
                Character spawnCharacter = Monsters[locationNumber];
                GameObject prefab = spawnCharacter.Prefab;
                spawnCharacter.SetResetHp();

                // 몬스터 생성 위치 설정
                Vector3 spawnPosition = new Vector3(POSITIONS[locationNumber, 0], POSITIONS[locationNumber, 1], POSITIONS[locationNumber, 2]);

                // 몬스터 오브젝트 생성 및 설정
                GameObject spawnUnit = Instantiate(prefab, spawnPosition, Quaternion.identity);
                MonsterController monsterController = spawnUnit.GetComponent<MonsterController>();
                monsterController.SetCharacter(spawnCharacter);
                monsterController.SetBattleManager(battleManager);
                monsterController.SpawnNumber = locationNumber;
                monsterController.gameObject.SetActive(true);

                // 배틀 매니저에 몬스터 정보 추가
                battleManager.monsterInBattle.Add(spawnUnit);
                battleManager.monsterNumber.Add(locationNumber);
                battleManager.monsterNumber.Sort();
                battleManager.monsterCps.Add(spawnCharacter.GetCP());
                battleManager.sumMonsterCp += spawnCharacter.GetCP();
                battleManager.monsterCount++;
            }
        }

        // 몬스터 리로드 락 해제
        battleManager.reloadMonsterLock = false;
    }
}
