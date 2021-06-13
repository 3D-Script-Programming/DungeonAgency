using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] monsterSpanwers = new GameObject[6];
    public Text[] monsterSpawnButtons = new Text[6];
    public Button prevRoomButton;
    public Button nextRoomButton;
    public Text roomNumberText;

    private int selectedRoomNumber = 0;
    private DungeonRoom selectedRoom;
    private int selectedPosition = -1;
    private Player player;

    void Start()
    {
        player = GameManager.instance.Player;
        selectedRoom = player.GetRoom(0);
        ApplyEvents();
        SpawnMonsters();
        foreach (GameObject spawner in monsterSpanwers) {
            spawner.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
    }

    void ApplyEvents() {
        for (int i = 0; i < 6; i++) {
            int now = i;
            monsterSpawnButtons[i].GetComponent<Button>().onClick.AddListener(() => OnClickSpawner(now));
        }
        prevRoomButton.onClick.AddListener(OnClickPrevRoomButton);
        nextRoomButton.onClick.AddListener(OnClickNextRoomButton);
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
        if (selectedPosition == -1) {
            return;
        }
        selectedRoom.RemoveMonster(monster);
        selectedRoom.PlaceMonster(selectedPosition, monster);
        SpawnMonsters();
    }

    public void OnClickPrevRoomButton() {
        selectedRoomNumber--;
        if (selectedRoomNumber < 0) {
            selectedRoomNumber = player.GetRoomCount() - 1;
        }
        if (selectedRoomNumber >= player.GetRoomCount()) {
            selectedRoomNumber = 0;
        }
        selectedRoom = player.GetRoom(selectedRoomNumber);
        roomNumberText.text = selectedRoomNumber.ToString();
        SpawnMonsters();
    }

    public void OnClickNextRoomButton() {
        selectedRoomNumber++;
        if (selectedRoomNumber < 0) {
            selectedRoomNumber = player.GetRoomCount() - 1;
        }
        if (selectedRoomNumber >= player.GetRoomCount()) {
            selectedRoomNumber = 0;
        }
        selectedRoom = player.GetRoom(selectedRoomNumber);
        roomNumberText.text = selectedRoomNumber.ToString();
        SpawnMonsters();
    }
    
    private void SpawnMonsters() {
        foreach(GameObject spawnedMonster in GameObject.FindGameObjectsWithTag("Monster")) {
            Destroy(spawnedMonster);
        }

        Character[] monsters = selectedRoom.Monsters;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
            {
                SpawnMonster(monsters[i], i);
            }
        }
    }

    private void SpawnMonster(Character monster, int position) {
        GameObject spawnUnit = Instantiate(monster.Prefab, monsterSpanwers[position].transform.position, Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }
}

