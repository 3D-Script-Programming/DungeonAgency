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

        name.text = monster.Name;
        level.text = monster.Level.ToString();
        str.text = monster.Strength.ToString();
        bal.text = monster.Balance.ToString();
        vtp.text = monster.Vitality.ToString();
        cp.text = monster.GetCP().ToString();

        btnHire.GetComponentInChildren<TextMeshProUGUI>().text = monster.GetPrice().ToString();
    }

    public void HireMonster()
    {
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            return;
        GameManager.s_Instance.player.AddMonster(monster);
        GameManager.s_Instance.player.AddGold(-1 * monster.GetPrice());
        btnHire.SetActive(false);
        GetComponentInParent<ListController>().UpdateUI();
        ListItemOnClick();
    }

    public void ListItemOnClick()
    {
        GetComponentInParent<ListController>().ReSpawnMonster(monster);
    }

    public void SetButton()
    {
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        else
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
    }
}
