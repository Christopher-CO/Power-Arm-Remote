using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public GameObject armIKTarget;
    private Transform targetTransform;
    [SerializeField]
    private float heightMax = 10.29f, heightMin = 4.706f, offset = 0; 
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = armIKTarget.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(height);
        height = Mathf.Clamp(targetTransform.position.y, heightMin, heightMax);
        transform.position = new Vector3(transform.position.x, height + offset, transform.position.z);
    }
}
