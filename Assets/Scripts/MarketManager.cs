using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketManager : MonoBehaviour
{
    private Player player;

    private List<Character> newMonsters;

    public GameObject goldUI;
    public GameObject monsterList;
    public GameObject positionPivot;


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        goldUI.GetComponentInChildren<TextMeshProUGUI>().text = "" + GameManager.instance.Player.GetGold();
        //GameObject.Find("MonsterList").GetComponent<ListController>().SetData(player.GetMonsterList(), player.GetGold());
        newMonsters = CharacterFactory.CreateMonsterList(10);
        Debug.Log(newMonsters);
        monsterList.GetComponent<ListController>().SetData(newMonsters, player.GetGold());
        SpawnMonster(newMonsters[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SpawnMonster(Character monster)
    {
        GameObject spawnUnit = Instantiate(monster.Prefab);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.transform.position = positionPivot.transform.position;
        spawnUnit.transform.rotation = positionPivot.transform.rotation;
        spawnUnit.SetActive(true);
    }
}
