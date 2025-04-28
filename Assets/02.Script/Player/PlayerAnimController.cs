using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _animator;
    private PlayerLocomotion _locomotion;

    private int _upperBodyLayerIndex;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }
    void Start()
    {
        _upperBodyLayerIndex = _animator.GetLayerIndex("UpperBody");
        UpdateMoveAnimation();
        WeaponBase.OnTriggerFireStart += PlayFire;
        WeaponBase.OnTriggerFireEnd += StopFire;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveAnimation();
    }
    private void UpdateMoveAnimation()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;
        float moveSpeed = inputDir.magnitude;

        _animator.SetFloat("MoveX", inputDir.x);
        _animator.SetFloat("MoveY", inputDir.z);
        _animator.SetFloat("MoveSpeed", moveSpeed);

        _animator.SetBool("IsRunning", _locomotion.IsRunning);
    }

    public void PlayFire()
    {
        // UpperBody 레이어 Weight 1로 올리기
        _animator.SetLayerWeight(_upperBodyLayerIndex, 1.0f);
        _animator.SetTrigger("Fire");
    }

    public void StopFire()
    {
        // UpperBody 레이어 Weight 0으로 내리기
        _animator.SetLayerWeight(_upperBodyLayerIndex, 0.0f);
    }

}
