using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile_Input : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    public GameObject target;
    private Transform targetTransform;
    public float upperLimitX = -3.8f;
    public float lowerLimitX = -14.18f;
    public float upperLimitY = 18.61f;
    public float lowerLimitY = -4.0f;
    public float topSpeed = 0;


    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveIKTarget(joystick.Horizontal, joystick.Vertical);
    }

    void MoveIKTarget(float throttleX, float throttleY)
    {
        Vector3 displacementX = new Vector3(throttleX, 0, 0);
        Vector3 displacementY = new Vector3(0, throttleY, 0);
        displacementX *= topSpeed * 100 * Time.fixedDeltaTime;
        displacementY *= topSpeed * 100 * Time.fixedDeltaTime;
        Vector3 newPosition = targetTransform.position + displacementX + displacementY;
        if (newPosition.x <= upperLimitX && newPosition.x >= lowerLimitX)
        {
            //targetTransform.Translate(displacementX, Space.World);
            targetTransform.position += displacementX;
        }
        if (newPosition.y <= upperLimitY && newPosition.y >= lowerLimitY)
        {
            //targetTransform.Translate(displacementY, Space.World);
            targetTransform.position += displacementY;
        }
        Debug.Log(displacementX + " " + displacementY + " " + targetTransform.position);

    }

    public void SetTopSpeed(float userTopSpeed)
    {
        topSpeed = userTopSpeed / 10; //convert from cm/s using scaling factor
    }

}
