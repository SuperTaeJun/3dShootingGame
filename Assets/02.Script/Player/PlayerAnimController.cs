using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAnimController : MonoBehaviour
{
    private Player _player;
    private Animator _animator;
    private PlayerLocomotion _locomotion;

    private int _upperBodyLayerIndex;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }
    void Start()
    {
        _upperBodyLayerIndex = _animator.GetLayerIndex("UpperBody");
        UpdateMoveAnimation();
        WeaponBase.OnTriggerFireStart += PlayFire;

        PlayerWeaponController.OnWeaponChange += OnChangeWeaponType;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateJumpAnimation();
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
    private void UpdateJumpAnimation()
    {
        _animator.SetBool("IsGrounded", _player.CharacterController.isGrounded);
        _animator.SetFloat("VerticalVelocity", _locomotion.VerticalVelocity);
    }
    public void PlayFire()
    {
        _animator.SetTrigger("Fire");
    }


    private void OnChangeWeaponType(EWeaponType currentType)
    {
        _animator.SetInteger("WeaponType", (int)currentType);
    }
}
