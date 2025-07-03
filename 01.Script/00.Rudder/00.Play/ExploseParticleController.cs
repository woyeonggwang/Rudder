using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploseParticleController : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(LifeTime());
    }
    
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
