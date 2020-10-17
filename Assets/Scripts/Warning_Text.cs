using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Warning_Text : MonoBehaviour
{
    float delay = 5.5f;
    public GameObject image;
    // Start is called before the first frame update
    void Awake()
    {
        image.SetActive(true);
        StartCoroutine(WaitToEnd());
    }

    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(delay);
        image.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
