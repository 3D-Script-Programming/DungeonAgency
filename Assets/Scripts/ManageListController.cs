using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageListController : MonoBehaviour
{
    private List<Character> monsters;
    private RectTransform scrollViewContent;

    void Start()
    {
        monsters = GameManager.s_Instance.player.GetMonsterList();
        scrollViewContent = GameObject.Find("Content").GetComponent<RectTransform>();
        scrollViewContent.sizeDelta = new Vector2(145.2759f, 0);
        InstantiateItems();
    }

    public void InstantiateItems()
    {
        foreach (Character monster in monsters)
        {
            GameObject listItem = Instantiate(Resources.Load<GameObject>("UI/ManageListItem"), GameObject.Find("Content").transform);
            listItem.GetComponent<ManageListItemController>().SetText(monster);
        }
    }
}
