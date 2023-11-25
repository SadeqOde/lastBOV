using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyObj());
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
