using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageListItemController : MonoBehaviour
{
    public TextMeshProUGUI nameText, levelText, strText, balText, vtpText, cpText, positionText;
    public GameObject natureHolder;
    public GameObject natureIcon;

    private DungeonManager dungeonManager;
    private Character monster;

    void Start()
    {
        dungeonManager = GameObject.Find("Dungeon Manager").GetComponent<DungeonManager>();
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

        nameText.text = monster.Nature.ToString();
        levelText.text = monster.Level.ToString();
        strText.text = monster.Strength.ToString();
        balText.text = monster.Balance.ToString();
        vtpText.text = monster.Vitality.ToString();
        cpText.text = monster.GetCP().ToString();
        this.monster = monster;
    }

    public void OnClickItem()
    {
        Console.WriteLine("OnClickItem 실행되니?");
        dungeonManager.OnClickListItem(monster);
    }
}
