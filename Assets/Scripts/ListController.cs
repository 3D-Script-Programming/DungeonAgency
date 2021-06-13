using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Character> monsters;
    private List<GameObject> listItems;
    private int money;
    private RectTransform scrollViewContent;
    void Start()
    {
        scrollViewContent = GameObject.Find("Content").GetComponent<RectTransform>();
        scrollViewContent.sizeDelta = new Vector2(145.2759f, 30 * monsters.Count);
        listItems = new List<GameObject>();
        InstantiateItems();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(List<Character> monsters, int money)
    {
        this.monsters = monsters;
        this.money = money;
    }

    public void InstantiateItems()
    {
        while (listItems.Count > 0)
            Destroy(listItems[listItems.Count - 1]);

        for (int i = 0; i < monsters.Count; i++) 
        {
            listItems.Add(Instantiate(Resources.Load<GameObject>("UI/listItem")));
            listItems[i].GetComponent<listItemController>().SetText(monsters[i], money);
            listItems[i].transform.SetParent(GameObject.Find("Content").transform);
        }

        scrollViewContent.sizeDelta = new Vector2(145.2759f, 60 * monsters.Count);
    }
}
