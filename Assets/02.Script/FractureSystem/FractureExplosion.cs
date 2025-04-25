using UnityEngine;
using System.Collections.Generic;


public class FractureExplosion : MonoBehaviour
{
    [Header("Prefracture Reference")]
    private Prefracture prefracture;

    [Header("Fracture Settings")]
    [SerializeField] private int fragmentCount = 10;
    [SerializeField] private Material insideMaterial;
    [SerializeField] private bool detectFloatingFragments = false;
    [SerializeField] private Vector2 textureScale = Vector2.one;
    [SerializeField] private Vector2 textureOffset = Vector2.zero;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float upwardsModifier = 0f;

    private List<GameObject> fragments = new List<GameObject>();
    private Transform fragmentParent;

    void Awake()
    {
        if (prefracture == null)
            prefracture = GetComponent<Prefracture>();

        GenerateFragments();
    }

    ///메쉬를 파편화하여 fragmentParent 아래에 생성 후, fragments 리스트에 저장하고 비활성
    void GenerateFragments()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
        {
            return;
        }

        // 파편 루트 생성
        var root = new GameObject(name + "Fragments");
        root.transform.SetParent(transform.parent);
        root.transform.SetPositionAndRotation(transform.position, transform.rotation);
        fragmentParent = root.transform;
        // Prefracture 옵션 구성
        prefracture.fractureOptions.fragmentCount = fragmentCount;
        prefracture.fractureOptions.insideMaterial = insideMaterial;
        prefracture.fractureOptions.detectFloatingFragments = detectFloatingFragments;
        prefracture.fractureOptions.textureScale = textureScale;
        prefracture.fractureOptions.textureOffset = textureOffset;
        prefracture.prefractureOptions.saveFragmentsToDisk = false;
        prefracture.prefractureOptions.saveLocation = string.Empty;

        //Prefracture - OpenFreacture - CreateFragmentTemplate 코드 사용
        var template = new GameObject("FragmentTemplate") { tag = tag };
        template.AddComponent<MeshFilter>();
        var meshRenderer = template.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new[]
        {
            GetComponent<MeshRenderer>().sharedMaterial,
            insideMaterial
        };
        var col = GetComponent<Collider>();
        var mc = template.AddComponent<MeshCollider>();
        mc.convex = true;
        mc.sharedMaterial = col.sharedMaterial;
        mc.isTrigger = col.isTrigger;
        var rbTemp = template.AddComponent<Rigidbody>();
        rbTemp.constraints = RigidbodyConstraints.FreezeAll;
        var rbSrc = GetComponent<Rigidbody>();
        if (rbSrc)
        {
            rbTemp.linearDamping = rbSrc.linearDamping;
            rbTemp.angularDamping = rbSrc.angularDamping;
            rbTemp.useGravity = rbSrc.useGravity;
        }

        //파편화
        Fragmenter.Fracture(
            gameObject,
            prefracture.fractureOptions,
            template,
            fragmentParent,
            false,
            string.Empty
        );

        DestroyImmediate(template);

        // 파편 저장
        foreach (Transform t in fragmentParent)
        {
            var frag = t.gameObject;
            frag.SetActive(false);
            fragments.Add(frag);
        }
    }

    public List<GameObject> Explode()
    {
        foreach (var frag in fragments)
        {
            frag.SetActive(true);
            var rb = frag.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    upwardsModifier,
                    ForceMode.Impulse
                );
            }
        }
        return fragments;
    }
}
