using System.Collections;
using UnityEngine;

public class ExplosionAnimationController : MonoBehaviour
{
    public float duration;
    private void OnEnable()
    {
        StartCoroutine(DestroyExplosionEffect());
    }
    IEnumerator DestroyExplosionEffect()
    {
        yield return new WaitForSeconds(duration);
        DestroyImmediate(this.gameObject);
    }
}
