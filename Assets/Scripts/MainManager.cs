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
    private AudioSource audioSource;

    public GameObject popup_Warning;

    public Button playButton;
    public Button marketButton;
    public Button manageButton;
    public Button settingButton;

    public TextMeshProUGUI evilPointText;
    public TextMeshProUGUI goldText;
    public AudioClip backgroundSound;
    public AudioClip buttonSound;

    void Start()
    {
        gameManager = GameManager.s_Instance;
        player = gameManager.player;
        monsters = player.GetMonsterList();
        evilPointText.text = player.GetEvilPoint().ToString();
        goldText.text = player.GetGold().ToString();

        SpawnMonsters();
        ApplyUIEvents();
        GameManager.s_Instance.SetMusic(backgroundSound);
        audioSource = GetComponent<AudioSource>();
    }

    private void SpawnMonsters()
    {
        if (monsters == null)
            return;
        Shuffle(monsters);
        monsters = monsters.GetRange(0, monsters.Count < 3 ? monsters.Count : 3);

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

    private void SpawnMonster(Character monster, Vector3 spawnPosition)
    {
        GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition, Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }

    private void ApplyUIEvents()
    {
        playButton.onClick.AddListener(ChangeScreen);
        marketButton.onClick.AddListener(() => GameManager.MoveScene("MarketScene"));
        manageButton.onClick.AddListener(ChangeScreen);
    }

    private void ChangeScreen()
    {
        if (monsters == null)
        {
            popup_Warning.SetActive(true);
            return;
        }
        GameManager.MoveScene("ManageScene");
    }

    public void ButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}
