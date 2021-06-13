using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Character> monsters;
    private List<GameObject> listItems;
    private int money;
    private RectTransform scrollViewContent;

    public GameObject btnCrown, btnTreasure;
    public TextMeshProUGUI countCrown, countTreasure;
    void Start()
    {
        listItems = new List<GameObject>();
        scrollViewContent = GameObject.Find("Content").GetComponent<RectTransform>();
        scrollViewContent.sizeDelta = new Vector2(145.2759f, 0);
        UpdateUI();
        InstantiateItems();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI() 
    {
        foreach (GameObject obj in listItems)
            obj.GetComponent<ListItemController>().SetButton();
        SetItemButton();
        GameObject.Find("Market Manager").GetComponent<MarketManager>().UpdateUI();
    }

    public void ReSpawnMonster(Character monster) 
    {
        Debug.Log(monster);
        GameObject.Find("Market Manager").GetComponent<MarketManager>().SpawnMonster(monster);
    }

    public void SetData(List<Character> monsters)
    {
        this.monsters = monsters;
    }

    public void InstantiateItems()
    {
        while (listItems.Count > 0)
            Destroy(listItems[listItems.Count - 1]);
        for (int i = 0; i < monsters.Count; i++) 
        {
            listItems.Add(Instantiate(Resources.Load<GameObject>("UI/listItem")));
            listItems[i].GetComponent<ListItemController>().SetText(monsters[i]);
            listItems[i].transform.SetParent(GameObject.Find("Content").transform);
        }

        scrollViewContent.sizeDelta = new Vector2(145.2759f, 60 * monsters.Count);
    }

    public void SetItemButton() 
    {

        if (500 > GameManager.instance.Player.GetGold())
        {

            btnTreasure.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
            btnTreasure.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;

        countTreasure.text = GameManager.instance.Player.GetItem(Item.TREASURE).ToString();

        if (1000 > GameManager.instance.Player.GetGold())
        {

            btnCrown.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        }
        else
            btnCrown.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;

        countCrown.text = GameManager.instance.Player.GetItem(Item.CROWN).ToString();
    }

    public void BuyCrown()
    {
        if (1000 > GameManager.instance.Player.GetGold())
            return;

        GameManager.instance.Player.AddItem(Item.CROWN, 1);
        GameManager.instance.Player.AddGold(-1 * 1000);
        GetComponentInParent<ListController>().UpdateUI();
    }

    public void BuyTreasure()
    {
        if (500 > GameManager.instance.Player.GetGold()) 
            return;

        GameManager.instance.Player.AddItem(Item.TREASURE, 1);
        GameManager.instance.Player.AddGold(-1 * 500);
        GetComponentInParent<ListController>().UpdateUI();
    }

}
