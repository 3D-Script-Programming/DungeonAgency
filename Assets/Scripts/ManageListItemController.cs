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

    void Start() {
        dungeonManager = GameObject.Find("Dungeon Manager").GetComponent<DungeonManager>();
    }

    public void SetText(Character monster)
    {
        if (monster.GetNature() == Nature.FIRE)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/fire", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_red", typeof(Sprite)) as Sprite;
        }
        else if (monster.GetNature() == Nature.WATER)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/water", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_blue", typeof(Sprite)) as Sprite;
        }
        else if (monster.GetNature() == Nature.WIND)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/wind", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_green", typeof(Sprite)) as Sprite;
        }

        nameText.text = monster.GetName();
        levelText.text = monster.GetLevel().ToString();
        strText.text = monster.GetSTR().ToString();
        balText.text = monster.GetBAL().ToString();
        vtpText.text = monster.GetVTP().ToString();
        cpText.text = monster.GetCP().ToString();
        this.monster = monster;
    }

    public void OnClickItem() {
        dungeonManager.OnClickListItem(monster);
    }
}
