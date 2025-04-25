using System.Collections;
using UnityEngine;

public class ScreenEffectController : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] Material _hitScreenEffect;

    Coroutine hitScreanEffectCoroutine;

    public void PlayHitEffect(float power, float duration)
    {
        if(hitScreanEffectCoroutine ==null)
            hitScreanEffectCoroutine = StartCoroutine(HitEffectCoroutine(power, duration));
    }

    private IEnumerator HitEffectCoroutine(float power, float duration)
    {
        float elapsed = 0f;
        float startValue = 0f;
        _hitScreenEffect.SetFloat("_vignettePower", power);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float current = Mathf.Lerp(power, startValue, t);
            _hitScreenEffect.SetFloat("_vignettePower", current);
            yield return null;
        }

        _hitScreenEffect.SetFloat("_vignettePower", startValue);
        hitScreanEffectCoroutine = null;
    }


}
