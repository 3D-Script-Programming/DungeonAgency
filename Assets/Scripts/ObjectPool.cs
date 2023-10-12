using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    // 오브젝트 풀의 큐
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    // 오브젝트 풀에서 사용할 프리팹
    private GameObject prefab;

    // ObjectPool 생성자
    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;

        // 초기에 풀에 추가할 객체 생성
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = InstantiateObject();
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    // 오브젝트 가져오기
    public GameObject GetObject()
    {
        if (poolQueue.Count == 0)
        {
            // 풀이 비어있으면 새로운 객체 생성
            GameObject obj = InstantiateObject();
            return obj;
        }

        // 풀에서 객체 가져오기
        GameObject pooledObject = poolQueue.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }

    // 오브젝트 반환
    public void ReturnObject(GameObject obj)
    {
        // 사용이 끝난 객체를 풀에 반환
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }

    // 오브젝트 생성
    private GameObject InstantiateObject()
    {
        // 새로운 객체 생성
        GameObject obj = GameObject.Instantiate(prefab);
        return obj;
    }
}
