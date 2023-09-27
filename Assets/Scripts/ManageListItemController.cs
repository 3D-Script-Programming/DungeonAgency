using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageListItemController : MonoBehaviour
{
    public TextMeshProUGUI nameText, levelText, strText, balText, vtpText, cpText;
    public Button listItem;
    public GameObject checkOnImage;
    public GameObject checkOffImage;
    public GameObject natureHolder;
    public GameObject natureIcon;

    private DungeonManager dungeonManager;
    public Character monster;

    private void Start()
    {
        ApplyEvents();
    }

    private void Update()
    {
        if (monster != null)
        {
            // 몬스터가 룸에 배치되어 있지 않으면
            if (monster.CurrentRoomNumber == -1)
                DeactivateMonsterCheck();
            // 몬스터가 룸에 배치되어 있다면
            else
                ActivateMonsterCheck();
        }
    }

    void ApplyEvents()
    {
        listItem.onClick.AddListener(OnClickListItem);
    }

    public void SetText(Character monster)
    {
        if (monster.Nature == Nature.FIRE)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/fire", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_red", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WATER)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/water", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_blue", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WIND)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/wind", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_green", typeof(Sprite)) as Sprite;
        }

        nameText.text = monster.Name.ToString();
        levelText.text = monster.Level.ToString();
        strText.text = monster.Strength.ToString();
        balText.text = monster.Balance.ToString();
        vtpText.text = monster.Vitality.ToString();
        cpText.text = monster.GetCP().ToString();
        this.monster = monster;
    }

    public void OnClickListItem()
    {
        dungeonManager.UIManager.OnClickListItem(monster);
    }

    // MarketManager 스크립트 참조 설정
    public void SetDungeonManager(DungeonManager dungeonManager)
    {
        this.dungeonManager = dungeonManager;
    }

    public void ActivateMonsterCheck()
    {
        checkOnImage.SetActive(true);
        checkOffImage.SetActive(false);
    }

    public void DeactivateMonsterCheck()
    {
        checkOnImage.SetActive(false);
        checkOffImage.SetActive(true);
    }
}