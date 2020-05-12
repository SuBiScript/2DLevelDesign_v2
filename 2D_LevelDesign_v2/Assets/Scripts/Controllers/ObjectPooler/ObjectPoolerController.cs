using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerController
{
    private List<GameObject> pool;

    public ObjectPoolerController()
    {
        pool = new List<GameObject>();
    }

    public void StorePoolerObject(int num, GameObject prefab)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject item = Object.Instantiate(prefab);
            pool.Add(item);
            item.SetActive(false);
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy && pool[i].tag == prefab.tag ) // if objet is set to false  
                return pool[i];
        }
        GameObject item = Object.Instantiate(prefab);
        item.SetActive(false);
        pool.Add(item);
        return item;
    }

    public void ActivatePrefab(GameObject prefab, Vector2 position, Vector2 direction)
    {
        GameObject temp = GetPooledObject(prefab);
        temp.GetComponent<Projectiles>().direction = direction;
        temp.transform.position = position;
        temp.SetActive(true);
    }
}
