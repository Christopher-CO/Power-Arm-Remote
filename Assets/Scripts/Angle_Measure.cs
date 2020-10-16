using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle_Measure : MonoBehaviour
{   
    [HideInInspector]
    public LineRenderer lineRenderer;
    public float width = .1f;
    //Bone transforms are public for angle value method to update text position with lowerbone position
    [SerializeField]
    public Transform upperBone;
    [SerializeField]
    public Transform lowerBone;
    public bool flipDir, flipStartDir;
    [HideInInspector]
    public Vector3[] points;
    public float degPerStep = 5f;
    public float radius = 2;
    [HideInInspector]
    public float angle;
    private int stepCount;
    [SerializeField][Range(0,5)]
    public float offset = .2f;
    private Vector3 oldStartDir;
    private bool isDimmed = false;
    private Gradient dimmedGradient;
    private Gradient undimmedGradient;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetWidth(width, width);
        undimmedGradient = lineRenderer.colorGradient;
        dimmedGradient = new Gradient();
        dimmedGradient.SetKeys(new GradientColorKey[] {new GradientColorKey(lineRenderer.colorGradient.colorKeys[0].color*.5f, 0.0f),
        new GradientColorKey(lineRenderer.colorGradient.colorKeys[1].color*.5f, 1.0f),}, lineRenderer.colorGradient.alphaKeys);
        lineRenderer.sortingOrder = 10;
    }


    // Update is called once per frame
    private void Update()
    {
        //get outter angle of joint
        angle = Vector3.SignedAngle(flipStartDir?upperBone.right:-1*upperBone.right, lowerBone.right, -1*upperBone.forward);
        angle += angle < 0 ? 360 : 0;

        //Debug.Log("Transformed Upper and Lower bone angle:" + angle);
        //angle += !flipStartDir ^ flipDir ? 180 : 0;
        //Debug.Log("Transformed angle:" + angle);
        //angle += !flipStartDir ? 180 : 0;
        //Debug.Log("flip start dir:" + !flipStartDir + angle);
        angle = flipDir ? (360 - angle) : angle;
        //Debug.Log("Upper and Lower bone angle: " + angle + "\nflip start dir: " + !flipStartDir + "\nflip dir " + flipDir);
        
        //prepare an array to store the points to represent the given angle at a resolution of stepsPerDeg
        stepCount = Mathf.RoundToInt(angle/degPerStep);
        points = new Vector3[stepCount + 1];
        
        //x0 and y0 represent the components of the direction vector along the upper bone facing away from the
        //joint which is the center of the arc
        float x0 = flipStartDir? upperBone.right.x : -1 * upperBone.right.x;
        float y0 = flipStartDir? upperBone.right.y : -1 * upperBone.right.y;

        //Debug.Log(x0 + ", " + y0);
        //anlge to rotate current point from the direction vector along the upper bone
        float beta;
        //components of direction vector of current point
        float xDir;
        float yDir;
        for (int i = 0; i <= stepCount; i++)
        {
            beta = i * angle * Mathf.Deg2Rad / stepCount;
            beta *= flipDir ? 1 : -1;
            xDir = Mathf.Cos(beta) * x0 - Mathf.Sin(beta) * y0;
            yDir = Mathf.Sin(beta) * x0 + Mathf.Cos(beta) * y0;
            points[i] = new Vector3(xDir, yDir, 0).normalized * radius;
            points[i] += lowerBone.position;
            points[i] += (flipStartDir?-1 * upperBone.up: upperBone.up) * (flipDir? -offset: offset);
            points[i] += lowerBone.up * (flipDir ? -offset : offset);

        }

        oldStartDir = upperBone.right;
        lineRenderer.SetVertexCount(stepCount + 1);
        lineRenderer.SetPositions(points);
    }

}
