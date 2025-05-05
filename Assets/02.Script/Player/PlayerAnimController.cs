using UnityEngine;
public class PlayerAnimController : MonoBehaviour
{
    private Player _player;
    private Animator _animator;
    private PlayerLocomotion _locomotion;

    private ECameraType _currentCameraType;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }
    void Start()
    {
        UpdateMoveAnimation();
        WeaponBase.OnTriggerFireStart += PlayFire;

        PlayerWeaponController.OnWeaponChange += OnChangeWeaponType;

        MyCamera.OnCameraTypeChanged += OnCameraTypeChanged;
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


        if (_currentCameraType == ECameraType.QuarterView)
            UpdateMoveAnimation_QuarterView(inputDir);
        else
            UpdateMoveAnimation_Directional(inputDir);
        

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

    private void UpdateMoveAnimation_QuarterView(Vector3 inputDir)
    {
        float x = Vector3.Dot(inputDir, transform.right);
        float z = Vector3.Dot(inputDir, transform.forward);

        _animator.SetFloat("MoveX", x, 0.1f, Time.deltaTime);
        _animator.SetFloat("MoveY", z, 0.1f, Time.deltaTime);
    }

    private void UpdateMoveAnimation_Directional(Vector3 inputDir)
    {
        _animator.SetFloat("MoveX", inputDir.x);
        _animator.SetFloat("MoveY", inputDir.z);
    }
    private void OnChangeWeaponType(EWeaponType currentType)
    {
        _animator.SetInteger("WeaponType", (int)currentType);
    }
    private void OnCameraTypeChanged(ECameraType cameraType)
    {
        _currentCameraType = cameraType;
    }
}
