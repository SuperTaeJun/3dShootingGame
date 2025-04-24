using System.Collections;
using UnityEngine;

public class Sapwner : MonoBehaviour
{
    [SerializeField] private GameObject _nomalEnemyPrefab;
    [SerializeField] private GameObject _chaseEnemyPrefab;

    private void Start()
    {
        StartCoroutine(SpawneRoutine(8f));
    }

    private IEnumerator SpawneRoutine(float time)
    {
        while(true)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                ObjectPool.Instance.GetObject(_nomalEnemyPrefab).transform.position = transform.position;
            }
            else
            {
                ObjectPool.Instance.GetObject(_chaseEnemyPrefab).transform.position = transform.position;
            }


            yield return new WaitForSeconds(time);


        }
    }
}
