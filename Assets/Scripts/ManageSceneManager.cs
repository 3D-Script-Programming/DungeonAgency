using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageSceneManager : MonoBehaviour
{
    private Player player;

    public GameObject goldUI;
    public GameObject monsterList;


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        goldUI.GetComponentInChildren<TextMeshProUGUI>().text = ""+GameManager.instance.Player.GetGold();
        Console.Write(goldUI.GetComponentInChildren<TextMeshProUGUI>().text);
        GameObject.Find("MonsterList").GetComponent<ListController>().SetData(player.GetMonsterList(), player.GetGold());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
