using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    [Header("Prefabs")]
    public GameObject BombPrefab;
    public GameObject TrailPrefab;
    [Header("Size")]
    public int BombSize = 3;
    public int TrailSize = 50;
    public int DefaultSize = 50;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, int> objSizeDictionary = new Dictionary<GameObject, int>();
    private Dictionary<string, Transform> parentDictionary = new Dictionary<string, Transform>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        objSizeDictionary.Add(BombPrefab, BombSize);
        objSizeDictionary.Add(TrailPrefab, TrailSize);
        InitPool(BombPrefab);
        InitPool(TrailPrefab);

    }
    private void InitPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        if (objSizeDictionary.ContainsKey(prefab) == false)
            objSizeDictionary.Add(prefab, DefaultSize);

        for (int i = 0; i < objSizeDictionary[prefab]; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        string objName = prefab.name;

        if (!parentDictionary.ContainsKey(objName))
        {
            GameObject parentObj = new GameObject(objName);
            parentObj.transform.SetParent(transform);
            parentDictionary[objName] = parentObj.transform;
        }

        GameObject NewObejct = Instantiate(prefab);

        NewObejct.transform.SetParent(parentDictionary[objName]);

        NewObejct.AddComponent<PooledObject>().OrginPrefab = prefab;

        NewObejct.SetActive(false);
        poolDictionary[prefab].Enqueue(NewObejct);
    }
    public GameObject GetObject(GameObject prefab)
    {
        if (poolDictionary.ContainsKey(prefab) == false)
        {
            InitPool(prefab);
        }

        if (poolDictionary[prefab].Count == 0)
        {
            CreateNewObject(prefab);
        }

        GameObject ObjectToGet = poolDictionary[prefab].Dequeue();
        ObjectToGet.SetActive(true);
        //ObjectToGet.transform.parent = null;
        return ObjectToGet;
    }
    public void ReturnToPool(GameObject objectToReturn)
    {
        GameObject OrignPrefab = objectToReturn.GetComponent<PooledObject>().OrginPrefab;

        string objName = OrignPrefab.name;

        objectToReturn.SetActive(false);
        //objectToReturn.transform.SetParent(parentDictionary[objName]);

        poolDictionary[OrignPrefab].Enqueue(objectToReturn);

    }


}
