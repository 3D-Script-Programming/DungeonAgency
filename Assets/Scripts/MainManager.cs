using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    private List<Character> monsters;

    public Button playButton;
    public Button shopButton;
    public Button settingButton;

    public TextMeshProUGUI evilPointText;
    public TextMeshProUGUI goldText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.Player;
        monsters = player.GetMonsterList();

        evilPointText.text = player.GetEvilPoint().ToString();
        goldText.text = player.GetGold().ToString();

        SpawnMonsters();
        ApplyUIEvents();
    }

    private void SpawnMonsters() {
        Shuffle(monsters);

        monsters = monsters.GetRange(0, 3);
        for (int i = 0; i < monsters.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(-3.5f + (i * 3.5f), 0, 0);
            SpawnMonster(monsters[i], spawnPosition);
        }
    }
     
    private void Shuffle<T>(List<T> list)
    {
        int random;
        T tmp;
 
        for (int i = 0; i < list.Count; i++)
        {
            random = Random.Range(0, list.Count);

            tmp = list[random];
            list[random] = list[i];
            list[i] = tmp;
        }
    }

    private void SpawnMonster(Character monster, Vector3 spawnPosition) {
        GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition,Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }

    private void ApplyUIEvents() {
        playButton.onClick.AddListener(GameManager.MoveManageScene);
        shopButton.onClick.AddListener(GameManager.MoveShopScene);
        settingButton.onClick.AddListener(GameManager.MoveSettingScene);
    }
}
