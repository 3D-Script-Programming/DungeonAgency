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
    private GameObject spawnUnit;
    private AudioSource audioSource;

    public GameObject statusGold;
    public GameObject statusInfamy;
    public GameObject monsterList;
    public GameObject positionPivot;
    public Button homeButton;
    public Button manageButton;
    public AudioClip backgroundSound;
    public GameObject popup_Warning;


    // Start is called before the first frame update
    void Awake()
    {
        ApplyUIEvents();
        newMonsters = CharacterFactory.CreateMonsterList(10);
        player = GameManager.s_Instance.player;
        statusGold.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.Gold;
        statusInfamy.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.Infamy;
        //GameObject.Find("MonsterList").GetComponent<ListController>().SetData(player.GetMonsterList(), player.GetGold());
        monsterList.GetComponent<ListController>().SetData(newMonsters);
        SpawnMonster(newMonsters[0]);
        GameManager.s_Instance.SetMusic(backgroundSound);
        audioSource = GetComponent<AudioSource>();
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
        statusGold.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.Gold;
        statusInfamy.GetComponentInChildren<TextMeshProUGUI>().text = "" + player.Infamy;
    }

    private void ApplyUIEvents()
    {
        homeButton.onClick.AddListener(() => GameManager.MoveScene("MainScene")); ;
        manageButton.onClick.AddListener(ChangeScreen);
    }

    private void ChangeScreen()
    {
        if (GameManager.s_Instance.player.GetMonsterList() == null)
        {
            Debug.Log("aaa");
            popup_Warning.SetActive(true);
            return;
        }
        GameManager.MoveScene("ManageScene");
    }
}
