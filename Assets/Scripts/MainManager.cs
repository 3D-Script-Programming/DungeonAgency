using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    // 몬스터 목록
    private List<Character> monsters;

    // 경고 팝업 게임 오브젝트
    public GameObject popup_Warning;

    // UI 버튼 : 씬 이동 또는 설정 
    public Button playButton;
    public Button marketButton;
    public Button manageButton;
    public Button settingButton;

    // UI 텍스트 : 악명, 골드
    public TextMeshProUGUI infamyText;
    public TextMeshProUGUI goldText;

    // 오디오 관련 오브젝트
    private AudioSource audioSource;
    public AudioClip backgroundSound;

    private void Start()
    {
        // UI 업데이트
        infamyText.text = GameManager.s_Instance.player.Infamy.ToString();
        goldText.text = GameManager.s_Instance.player.Gold.ToString();

        // 배경음악 재생 
        GameManager.s_Instance.SetMusic(backgroundSound);

        // UI 이벤트 등록
        ApplyUIEvents();

        // 몬스터 스폰
        SpawnMonsters();
    }

    // 몬스터를 스폰하는 메서드
    private void SpawnMonsters()
    {
        // 몬스터 리스트 가져오기
        monsters = GameManager.s_Instance.player.GetMonsterList();

        if (monsters == null)
            return;

        // 몬스터 리스트 섞기
        Shuffle(monsters);

        // 최대 3개의 몬스터만 스폰
        int monsterCount = Mathf.Min(monsters.Count, 3);

        for (int i = 0; i < monsterCount; i++)
        {
            // 스폰 위치 계산
            Vector3 spawnPosition = new Vector3(-3.5f + (i * 3.5f), 0, 0);

            // 몬스터 스폰
            SpawnMonster(monsters[i], spawnPosition);
        }
    }

    // 리스트를 무작위로 섞는 메서드
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            T tmp = list[random];
            list[random] = list[i];
            list[i] = tmp;
        }
    }

    // 몬스터를 스폰하는 메서드
    private void SpawnMonster(Character monster, Vector3 spawnPosition)
    {
        GameObject spawnUnit = Instantiate(monster.Prefab, spawnPosition, Quaternion.identity);
        spawnUnit.GetComponent<MonsterController>().enabled = false;
        spawnUnit.GetComponent<NonBattleMonsterController>().enabled = true;
        spawnUnit.SetActive(true);
    }

    // UI 이벤트 처리 등록 메서드
    private void ApplyUIEvents()
    {
        playButton.onClick.AddListener(ChangeScreen);
        marketButton.onClick.AddListener(() => GameManager.MoveScene("MarketScene"));
        manageButton.onClick.AddListener(ChangeScreen);
    }

    // 화면 전환 메서드
    private void ChangeScreen()
    {
        if (monsters == null)
        {
            // 경고 팝업 표시
            popup_Warning.SetActive(true);
            return;
        }
        // 관리 씬으로 이동
        GameManager.MoveScene("ManageScene");
    }
}