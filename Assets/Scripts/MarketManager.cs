using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    private Player player;

    private List<Character> newMonsters;

    public GameObject statusGold;
    public GameObject statusEvil;
    public GameObject monsterList;
    public GameObject positionPivot;
    private GameObject spawnUnit;

    public Button homeButton;


    // Start is called before the first frame update
    void Start()
    {
        ApplyUIEvents();
        newMonsters = CharacterFactory.CreateMonsterList(10);
        player = GameManager.instance.Player;
        statusGold.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.GetGold();
        statusEvil.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.GetEvilPoint();
        //GameObject.Find("MonsterList").GetComponent<ListController>().SetData(player.GetMonsterList(), player.GetGold());
        monsterList.GetComponent<ListController>().SetData(newMonsters);
        SpawnMonster(newMonsters[0]);
    }


    public void SpawnMonster(Character monster)
    {
        if (spawnUnit != null)
            Destroy(spawnUnit);

        spawnUnit = Instantiate(monster.Prefab);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.transform.position = positionPivot.transform.position;
        spawnUnit.transform.rotation = positionPivot.transform.rotation;
        spawnUnit.SetActive(true);
    }

    public void UpdateUI()
    {
        Debug.Log(GameManager.instance.Player.GetGold());
        statusGold.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.GetGold();
        statusEvil.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.GetEvilPoint();
    }

    private void ApplyUIEvents()
    {
        homeButton.onClick.AddListener(GameManager.MoveMainScene);
    }
}
