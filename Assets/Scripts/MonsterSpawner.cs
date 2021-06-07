using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int locationNumber; // 012 전열 345 후열 
    public int currentRoom;
    /*private DungeonRoom room;*/

    void Start()
    {
        /*room = GameManager.instance.Player.GetRoom(currentRoom);*/
        /*GameObject unit = room.Monsters[locationNumber].Prefab;*/
        GameObject unit = GameManager.instance.Player.GetMonster(locationNumber).Prefab;
        Instantiate(unit, transform.position, Quaternion.identity);
    }
}
