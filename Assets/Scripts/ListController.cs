using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Character> monsters;
    private List<GameObject> ListItems;
    private int money;
    private RectTransform scrollViewContent;

    public GameObject btnCrown, btnTreasure;
    public TextMeshProUGUI countCrown, countTreasure;
    void Start()
    {
        ListItems = new List<GameObject>();
        scrollViewContent = GameObject.Find("Content").GetComponent<RectTransform>();
        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, 60 * monsters.Count);
        InstantiateItems();
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        foreach (GameObject obj in ListItems)
            obj.GetComponent<ListItemController>().SetButton();
        SetItemButton();
        GameObject.Find("Market Manager").GetComponent<MarketManager>().UpdateUI();
    }

    public void ReSpawnMonster(Character monster)
    {
        GameObject.Find("Market Manager").GetComponent<MarketManager>().SpawnMonster(monster);
    }

    public void SetData(List<Character> monsters)
    {
        this.monsters = monsters;
    }

    public void InstantiateItems()
    {
        while (ListItems.Count > 0)
            Destroy(ListItems[ListItems.Count - 1]);
        for (int i = 0; i < monsters.Count; i++)
        {
            ListItems.Add(Instantiate(Resources.Load<GameObject>("UI/listItem")));
            ListItems[i].GetComponent<ListItemController>().SetText(monsters[i]);
            ListItems[i].transform.SetParent(GameObject.Find("Content").transform);
        }

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, 60 * monsters.Count);
    }

    public void SetItemButton()
    {

        if (500 > GameManager.s_Instance.player.GetGold())
        {

            btnTreasure.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
            btnTreasure.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;

        countTreasure.text = GameManager.s_Instance.player.GetItem(Item.TREASURE).ToString();

        if (1000 > GameManager.s_Instance.player.GetGold())
        {

            btnCrown.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
            btnCrown.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;

        countCrown.text = GameManager.s_Instance.player.GetItem(Item.CROWN).ToString();
    }

    public void BuyCrown()
    {
        if (1000 > GameManager.s_Instance.player.GetGold())
            return;

        GameManager.s_Instance.player.AddItem(Item.CROWN, 1);
        GameManager.s_Instance.player.AddGold(-1 * 1000);
        GetComponentInParent<ListController>().UpdateUI();
    }

    public void BuyTreasure()
    {
        if (500 > GameManager.s_Instance.player.GetGold())
            return;

        GameManager.s_Instance.player.AddItem(Item.TREASURE, 1);
        GameManager.s_Instance.player.AddGold(-1 * 500);
        GetComponentInParent<ListController>().UpdateUI();
    }

}
