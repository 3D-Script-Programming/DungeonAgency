using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // 전투 매니저에 대한 참조를 인스펙터에서 설정할 수 있도록 하는 변수
    [SerializeField]
    private BattleManager battleManager;

    // 스폰된 보스 캐릭터에 대한 속성 및 메서드를 사용할 수 있도록 하는 프로퍼티
    public Character Boss { get; set; }

    void OnEnable()
    {
        // 보스 캐릭터를 설정하는 메서드 호출
        Boss.SetBoss();

        // 보스 캐릭터의 프리팹을 가져와서 인스턴스화하고 위치 및 회전 설정
        GameObject prefab = Boss.Prefab;
        GameObject spawnUnit = Instantiate(prefab, transform.position, Quaternion.identity);

        // 스폰된 유닛의 크기를 조절
        spawnUnit.transform.localScale = new Vector3(2f, 2f, 2f);

        // 스폰된 몬스터의 컨트롤러를 가져와서 캐릭터 및 전투 매니저 설정
        MonsterController monsterController = spawnUnit.GetComponent<MonsterController>();
        monsterController.SetCharacter(Boss);
        monsterController.SetBattleManager(battleManager);
        monsterController.SpawnNumber = 0;
        monsterController.gameObject.SetActive(true);

        // 전투 매니저에 스폰된 몬스터 정보 추가
        battleManager.monsterInBattle.Add(spawnUnit);
        battleManager.monsterNumber.Add(0);
        battleManager.monsterCps.Add(Boss.GetCP());
        battleManager.sumMonsterCp += Boss.GetCP();
        battleManager.monsterCount++;
        battleManager.reloadMonsterLock = false;
    }
}
