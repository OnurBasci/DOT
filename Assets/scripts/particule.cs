using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particule : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(destroy(3));
    }
    IEnumerator destroy(int t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
