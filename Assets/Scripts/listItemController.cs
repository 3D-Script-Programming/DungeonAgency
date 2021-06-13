using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListItemController : MonoBehaviour
{
    public TextMeshProUGUI name, level, str, bal, vtp, cp;
    public GameObject btnHire;
    public GameObject natureHolder;
    public GameObject natureIcon;

    public Character monster;

    public void SetText(Character monster)
    {
        this.monster = monster;
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

        name.text = monster.GetName();
        level.text = monster.GetLevel().ToString();
        str.text = monster.GetSTR().ToString();
        bal.text = monster.GetBAL().ToString();
        vtp.text = monster.GetVTP().ToString();
        cp.text = monster.GetCP().ToString();

        btnHire.GetComponentInChildren<TextMeshProUGUI>().text = monster.GetPrice().ToString();
    }

    public void HireMonster()
    {
        if (monster.GetPrice() > GameManager.instance.Player.GetGold())
            return;
        GameManager.instance.Player.AddMonster(monster);
        GameManager.instance.Player.AddGold(-1 * monster.GetPrice());
        btnHire.SetActive(false);
        GetComponentInParent<ListController>().UpdateUI();
    }

    public void ListItemOnClick()
    {
        GetComponentInParent<ListController>().ReSpawnMonster(monster);
    }

    public void SetButton()
    {
        if (monster.GetPrice() > GameManager.instance.Player.GetGold())
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        else
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
    }
}
