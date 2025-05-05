using UnityEngine;

public class PlayerHideHead : MonoBehaviour
{
    private Animator _animator;
    private Transform _headBone;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _headBone = _animator.GetBoneTransform(HumanBodyBones.Head);

        MyCamera.OnCameraTypeChanged += OnCameraChanged;
    }

    private void OnDestroy()
    {
        MyCamera.OnCameraTypeChanged -= OnCameraChanged;
    }

    private void OnCameraChanged(ECameraType type)
    {
        bool showHead = type != ECameraType.FirstPerson;

        if (_headBone == null) return;

        foreach (Transform child in _headBone.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(showHead);
        }
    }
}
