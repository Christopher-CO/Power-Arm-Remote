using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Warning_Text : MonoBehaviour
{
    float delay = 5.5f;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(true);
        StartCoroutine(WaitToEnd());
    }

    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
