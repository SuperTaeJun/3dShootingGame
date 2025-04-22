using UnityEngine;

public class Bomb : MonoBehaviour
{
    //마우스의 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶음,
    // 1. 슈류탄 오브젝트 만들기
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
    public GameObject ExplotionVfxPrefab;
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject vfx = GameObject.Instantiate(ExplotionVfxPrefab);
        vfx.transform.position = gameObject.transform.position;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
