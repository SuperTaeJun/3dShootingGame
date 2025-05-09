using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject Enemy1Prefab;
    [SerializeField] private GameObject Enemy2Prefab;
    [SerializeField] private GameObject Enemy3Prefab;
    [SerializeField] private GameObject Enemy4Prefab;

    public bool IsReady { get; private set; } = false;

    public const int BulletSize = 500;
    public const int EnemySize = 100;

    public const int DefaultSize = 20;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, int> objSizeDictionary = new Dictionary<string, int>();
    private Dictionary<string, Transform> parentDictionary = new Dictionary<string, Transform>();

    public float PoolPrepareProgress;
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

        objSizeDictionary.Add(BulletPrefab.name, BulletSize);
        InitPool(BulletPrefab);
        objSizeDictionary.Add(Enemy1Prefab.name, EnemySize);
        InitPool(Enemy1Prefab);
        objSizeDictionary.Add(Enemy2Prefab.name, EnemySize);
        InitPool(Enemy2Prefab);
        objSizeDictionary.Add(Enemy3Prefab.name, EnemySize);
        InitPool(Enemy3Prefab);
        objSizeDictionary.Add(Enemy4Prefab.name, EnemySize);
        InitPool(Enemy4Prefab);
    }
    private void Start()
    {


    }

    private void InitPool(GameObject prefab)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
            poolDictionary[key] = new Queue<GameObject>();

        if (!objSizeDictionary.ContainsKey(key))
            objSizeDictionary.Add(key, DefaultSize);

        for (int i = 0; i < objSizeDictionary[key]; i++)
        {
            CreateNewObjects(prefab, 1);
        }
    }

    private void CreateNewObjects(GameObject prefab, int count)
    {
        string key = prefab.name;

        if (!parentDictionary.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "_Pool");
            parentObj.transform.SetParent(transform);
            parentDictionary[key] = parentObj.transform;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.SetParent(parentDictionary[key]);
            newObject.AddComponent<PooledObject>().OrginPrefab = prefab;
            newObject.SetActive(false);
            poolDictionary[key].Enqueue(newObject);
        }
    }
    public GameObject GetObject(GameObject prefab , Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
        {
            InitPool(prefab);
        }

        if (poolDictionary[key].Count == 0)
        {
            CreateNewObjects(prefab, objSizeDictionary[key]);
        }

        GameObject ObjectToGet = poolDictionary[key].Dequeue();
        ObjectToGet.transform.position = position;
        ObjectToGet.transform.rotation = rotation;
        ObjectToGet.SetActive(true);
        return ObjectToGet;
    }
    public void ReturnToPool(GameObject objectToReturn)
    {
        GameObject OrignPrefab = objectToReturn.GetComponent<PooledObject>().OrginPrefab;
        string key = OrignPrefab.name;

        objectToReturn.SetActive(false);
        poolDictionary[key].Enqueue(objectToReturn);
    }


}
