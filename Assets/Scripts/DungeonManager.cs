using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] monsterSpanwers = new GameObject[6];
    public Text[] monsterSpawnButtons = new Text[6];
    public GameObject crownButton;
    public GameObject crownCheckIcon;
    public GameObject treasureButton;
    public GameObject treasureCheckIcon;
    public Button prevRoomButton;
    public Button nextRoomButton;
    public Button homeButton;
    public Button playButton;
    public Button marketButton;
    public Text roomNumberText;
    public TextMeshProUGUI crownCountText;
    public TextMeshProUGUI treasureCountText;
    public TextMeshProUGUI evilPointText;
    public TextMeshProUGUI goldText;
    public AudioClip backgroundSound;
    public AudioClip buttonSound;

    private int selectedRoomNumber = 0;
    private DungeonRoom selectedRoom;
    private int selectedPosition = -1;
    private Player player;
    private AudioSource audioSource;

    void Start()
    {
        player = GameManager.instance.Player;
        selectedRoom = player.GetRoom(0);
        ApplyEvents();
        CheckItems();
        SpawnMonsters();
        foreach (GameObject spawner in monsterSpanwers)
        {
            spawner.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
        crownCountText.text = player.GetItem(Item.CROWN).ToString();
        treasureCountText.text = player.GetItem(Item.TREASURE).ToString();
        evilPointText.text = player.GetEvilPoint().ToString();
        goldText.text = player.GetGold().ToString();
        GameManager.instance.SetMusic(backgroundSound);
        audioSource = GetComponent<AudioSource>();
    }

        void ApplyEvents() {
        for (int i = 0; i < 6; i++) {
            int now = i;
            monsterSpawnButtons[i].GetComponent<Button>().onClick.AddListener(() => OnClickSpawner(now));
        }
        crownButton.GetComponent<Button>().onClick.AddListener(OnClickCrown);
        treasureButton.GetComponent<Button>().onClick.AddListener(OnClickTreasure);
        prevRoomButton.onClick.AddListener(OnClickPrevRoomButton);
        nextRoomButton.onClick.AddListener(OnClickNextRoomButton);
        homeButton.onClick.AddListener(GameManager.MoveMainScene);
        marketButton.onClick.AddListener(GameManager.MoveMarketScene);
        playButton.onClick.AddListener(GameManager.MoveBattleScene);
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

    public void OnClickCrown() {
        if (crownCheckIcon.activeSelf) {
            player.UnuseCrown();
            crownCheckIcon.SetActive(false);
            crownCountText.text = player.GetItem(Item.CROWN).ToString();
            return;
        }
        if (player.GetItem(Item.CROWN) <= 0) {
            return;
        }
        if (treasureCheckIcon.activeSelf) {
            player.UnuseTreasure();
            treasureCheckIcon.SetActive(false);
            treasureCountText.text = player.GetItem(Item.TREASURE).ToString();
        }
        player.UseCrown();
        crownCountText.text = player.GetItem(Item.CROWN).ToString();
        selectedRoom.PlaceItem(Item.CROWN);
        crownCheckIcon.SetActive(true);
    }

    public void OnClickTreasure() {
        if (treasureCheckIcon.activeSelf) {
            player.UnuseTreasure();
            treasureCheckIcon.SetActive(false);
            treasureCountText.text = player.GetItem(Item.TREASURE).ToString();
            return;
        }
        if (player.GetItem(Item.TREASURE) <= 0) {
            return;
        }
        if (crownCheckIcon.activeSelf) {
            player.UnuseCrown();
            crownCheckIcon.SetActive(false);
            crownCountText.text = player.GetItem(Item.CROWN).ToString();
        }
        player.UseTreasure();
        treasureCountText.text = player.GetItem(Item.TREASURE).ToString();
        selectedRoom.PlaceItem(Item.TREASURE);
        treasureCheckIcon.SetActive(true);
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
        CheckItems();
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
        CheckItems();
        SpawnMonsters();
    }

    private void CheckItems() {
        crownCheckIcon.SetActive(false);
        treasureCheckIcon.SetActive(false);
        if (selectedRoom.GetItem() == Item.CROWN) {
            crownCheckIcon.SetActive(true);
            treasureCheckIcon.SetActive(false);
        }
        else if (selectedRoom.GetItem() == Item.TREASURE) {
            crownCheckIcon.SetActive(false);
            treasureCheckIcon.SetActive(true);
        }
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

    public void ButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}

