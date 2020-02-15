using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    #region Methods

    /// <summary>
    /// Create an object
    /// </summary>
    private static GameObject CreateAnInstance(GameObject prefab)
    {
        GameObject instance = Object.Instantiate(prefab);
        instance.name = prefab.name;

        return instance;
    }

    /// <summary>
    /// Create object(s) and add it/them to the pool
    /// </summary>
    public static void PreLoadPool(GameObject prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject instance = CreateAnInstance(prefab);
            AddToPool(instance);
        }

        Debug.Log($"Preload { number } of { prefab.name } to the pool");
    }

    /// <summary>
    /// Add an object to the pool
    /// </summary>
    public static void AddToPool(GameObject objectToStore)
    {
        objectToStore.SetActive(false);

        if (_pool.ContainsKey(objectToStore.name))
        {
            _pool[objectToStore.name].Add(objectToStore);
        }
        else
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(objectToStore);

            _pool.Add(objectToStore.name, newList);
        }
    }

    /// <summary>
    /// Get an instance from the pool, create one if none available
    /// </summary>
    public static GameObject GetFromPool(GameObject prefab)
    {
        GameObject instance;

        if (!_pool.ContainsKey(prefab.name))
        {
            instance = CreateAnInstance(prefab);

            return instance;
        }
        else
        {
            if (_pool[prefab.name].Count > 0)
            {
                instance = _pool[prefab.name][0];
                instance.SetActive(true);
                _pool[prefab.name].RemoveAt(0);

                return instance;
            }
            else
            {
                instance = CreateAnInstance(prefab);

                return instance;
            }
        }
    }

    /// <summary>
    /// Clear all the pool
    /// </summary>
    public static void EmptyPool()
    {
        _pool.Clear();
    }

    #endregion


    #region Private fields

    private static Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>();

    #endregion
}
