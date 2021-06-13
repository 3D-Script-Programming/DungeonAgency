using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] monsterSpanwers = new GameObject[6];
    public Text[] monsterSpawnButtons = new Text[6];
    public int selected = -1;

    void Start()
    {
        ApplyEvents();
    }

    void ApplyEvents() {
        for (int i = 0; i < 6; i++) {
            int now = i;
            monsterSpawnButtons[i].GetComponent<Button>().onClick.AddListener(() => OnClickSpawner(now));
        }
    }

    void OnClickSpawner(int selected) {
        foreach (GameObject spawner in monsterSpanwers) {
            spawner.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }

        if (this.selected == selected) {
            this.selected = -1;
            return;
        }
        this.selected = selected;
        monsterSpanwers[selected].GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }
}

