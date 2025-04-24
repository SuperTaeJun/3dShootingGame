using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySapwner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<EnemySpawnInfo> _enemyTypes;
    [SerializeField] private float _spawnInterval = 8f;

    private void Start()
    {
        if (_enemyTypes == null || _enemyTypes.Count == 0) Debug.LogWarning("EnemySapwner에 타입이 아무것도 없어요");

        StartCoroutine(SpawnRoutine(_spawnInterval));
    }

    private IEnumerator SpawnRoutine(float interval)
    {
        while (true)
        {
            var enemyInfo = GetRandomEnemyInfo();

            GameObject enemy = ObjectPool.Instance.GetObject(enemyInfo.prefab);
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(interval);
        }
    }

    private EnemySpawnInfo GetRandomEnemyInfo()
    {
        float totalWeight = 0f;
        foreach (var info in _enemyTypes)
            totalWeight += info.spawnWeight;

        float random = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var info in _enemyTypes)
        {
            current += info.spawnWeight;
            if (random <= current)
                return info;
        }

        return _enemyTypes[0];
    }
}
