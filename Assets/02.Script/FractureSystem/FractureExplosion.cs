using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Collider))]
public class FractureExplosion : MonoBehaviour
{
    [Header("Prefracture Reference")]
    public Prefracture prefracture;

    [Header("Fracture Settings")]
    public int fragmentCount = 10;
    public Material insideMaterial;
    public bool detectFloatingFragments = false;
    public Vector2 textureScale = Vector2.one;
    public Vector2 textureOffset = Vector2.zero;

    [Header("Explosion Settings")]
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 0f;

    private List<GameObject> fragments = new List<GameObject>();
    private Transform fragmentParent;

    void Awake()
    {
        if (prefracture == null)
            prefracture = GetComponent<Prefracture>();

        GenerateFragments();
    }

    /// 메쉬를 파편화하여 fragmentParent 아래에 생성 후, fragments 리스트에 저장하고 비활성화합니다.
    void GenerateFragments()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("FractureExplosion: MeshFilter에 메쉬가 없습니다.");
            return;
        }

        // 파편 루트 생성
        var root = new GameObject(name + "_Fragments");
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

        // 임시 템플릿 생성 (Prefracture(CreateFragmentTemplate)와 동일)
        var template = new GameObject("FragmentTemplate") { tag = tag };
        template.AddComponent<MeshFilter>();
        var mr = template.AddComponent<MeshRenderer>();
        mr.sharedMaterials = new[]
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

        // 파편화 실행 (원본 비활성화 없음)
        Fragmenter.Fracture(
            gameObject,
            prefracture.fractureOptions,
            template,
            fragmentParent,
            false,
            string.Empty
        );

        DestroyImmediate(template);

        // 파편 저장 및 비활성화
        foreach (Transform t in fragmentParent)
        {
            var frag = t.gameObject;
            frag.SetActive(false);
            fragments.Add(frag);
        }
    }

    /// <summary>
    /// fragments에 저장된 각 파편을 활성화하고 Rigidbody에 폭발력을 줍니다.
    /// </summary>
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
