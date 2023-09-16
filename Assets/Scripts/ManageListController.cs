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
            GameObject listItem = Instantiate(Resources.Load<GameObject>("UI/MangeListItem"));
            listItem.GetComponent<ManageListItemController>().SetText(monster);
            listItem.transform.SetParent(GameObject.Find("Content").transform);
        }

        scrollViewContent.sizeDelta = new Vector2(145.2759f, 60 * monsters.Count);
    }
}
