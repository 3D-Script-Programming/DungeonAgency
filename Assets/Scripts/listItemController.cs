using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListItemController : MonoBehaviour
{
    public TextMeshProUGUI name, level, str, bal, vtp, cp, price;
    public Button btnListItem;
    public Button btnHire;
    public GameObject natureHolder;
    public GameObject natureIcon;

    private Character monster;
    private MarketManager marketManager;

    private void Awake()
    {
        ApplyUIEvents();
    }

    private void ApplyUIEvents()
    {
        btnListItem.onClick.AddListener(OnClickListItem);
        btnHire.onClick.AddListener(OnClickBtnHire);
    }

    public void OnClickListItem()
    {
        GameManager.s_Instance.PlayButtonSound();
        marketManager.SpawnMonster(monster);
    }

    public void OnClickBtnHire()
    {
        GameManager.s_Instance.PlayButtonSound();
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            return;
        GameManager.s_Instance.player.AddMonster(monster);
        GameManager.s_Instance.player.AddGold(-monster.GetPrice());
        btnHire.gameObject.SetActive(false);
        marketManager.uiManager.UpdateUI();
        SetButton();
    }

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
        price.text = monster.GetPrice().ToString();

        SetButton();
    }

    public void SetButton()
    {
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            btnHire.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        else
            btnHire.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
    }

    public void SetMarketManager(MarketManager marketManager)
    {
        this.marketManager = marketManager;
    }
}
