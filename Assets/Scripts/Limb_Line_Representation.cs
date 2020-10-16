using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Limb_Line_Representation : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform upperBone;
    [SerializeField]
    private Transform lowerBone;
    [SerializeField]
    private Transform IKEmmitter;
    private Vector3[] points;
    public float width = .1f;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(3);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetWidth(width, width);
        lineRenderer.sortingOrder = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        lineRenderer.SetPositions(new Vector3[] { upperBone.position, lowerBone.position, IKEmmitter.position});
    }
}
