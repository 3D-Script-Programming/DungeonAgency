using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] monsterSpanwers = new GameObject[6];
    public Text[] monsterSpawnButtons = new Text[6];
    public int selectedRoomNumber = 0;
    public int selectedPosition = -1;
    public Player player;

    void Start()
    {
        player = GameManager.instance.Player;
        ApplyEvents();
    }

    void ApplyEvents() {
        for (int i = 0; i < 6; i++) {
            int now = i;
            monsterSpawnButtons[i].GetComponent<Button>().onClick.AddListener(() => OnClickSpawner(now));
        }
    }

    void OnClickSpawner(int selected) {
        foreach (GameObject spawner in monsterSpanwers) {
            spawner.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }

        if (this.selectedPosition == selected) {
            this.selectedPosition = -1;
            return;
        }
        this.selectedPosition = selected;
        monsterSpanwers[selected].GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }

    public void OnClickListItem(Character monster) {
        Character spawnedMonster = player.FindMonsterByPosition(selectedRoomNumber, selectedPosition);
        if (spawnedMonster != null) {
            spawnedMonster.ResetPosition();
        }
        monster.SetRoomNumber(selectedRoomNumber);
        monster.SetPosition(selectedPosition);
        SpawnMonsters();
    }

    private void SpawnMonsters() {
        foreach(GameObject spawnedMonster in GameObject.FindGameObjectsWithTag("Monster")) {
            Destroy(spawnedMonster);
        }
        foreach(Character monster in player.GetSpawnedMonster()) {
            SpawnMonster(monster);
        }
    }

    private void SpawnMonster(Character monster) {
        GameObject spawnUnit = Instantiate(monster.Prefab, monsterSpanwers[monster.GetPosition()].transform.position, Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }
}

