using UnityEngine;

public class GoldItem : MonoBehaviour
{
    [Header("Currency Info")]
    [SerializeField] private ECurrencyType _currencyType;
    [SerializeField] private int _amount;

    [Header("Collect Settings")]
    [SerializeField] private float _collectDistance = 3f;
    [SerializeField] private float _moveSpeed = 10f;
    private bool _isCollecting = false;
    private Transform _playerTransform;

    [Header("Rotate Settings")]
    [SerializeField] private float _rotateSpeed = 90f; // 초당 90도 회전

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Rotate();

        if (!_isCollecting)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);
            if (distance <= _collectDistance)
            {
                _isCollecting = true;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime, Space.World);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _playerTransform.position) < 0.5f)
        {
            CurrencyManager.Instance.AddCurrency(_currencyType, _amount);
            Destroy(gameObject);
        }
    }

    public void Initialize(ECurrencyType type, int amount)
    {
        _currencyType = type;
        _amount = amount;
    }
}
