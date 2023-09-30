using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationController : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DestroyExplosionEffect());
    }
    IEnumerator DestroyExplosionEffect()
    {
        yield return new WaitForSeconds(0.3f);
        DestroyImmediate(this.gameObject);
    }
}
