using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Idle_Mover : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    public Transform targetTransform;
    [SerializeField]
    private Mobile_Input mobileInput;
    public float maxDist = .8f;
    private bool done = false;

    public Image grayOut;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTarget());
        grayOut = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if((joystick.Horizontal + joystick.Vertical) > 0 && !done){
            StopCoroutine(MoveTarget());
            StartCoroutine(FadeOut());
            done = true;
            Debug.Log("test");
        }
    }

    IEnumerator MoveTarget()
    {
        Vector3 nextLocation = Vector3.zero;
        Vector3 lastLocation = Vector3.zero;
        while (true)
        {
            nextLocation = new Vector3(Random.Range(mobileInput.lowerLimitX, mobileInput.upperLimitX),
                Random.Range(mobileInput.lowerLimitY, mobileInput.upperLimitY));
            while (Vector3.Distance(targetTransform.position, nextLocation) > maxDist)
            {
                targetTransform.position = Vector3.Lerp(targetTransform.position, nextLocation, Time.deltaTime);
                yield return new WaitForSeconds(InverseLerp(nextLocation, lastLocation, targetTransform.position)*Time.deltaTime);
            }
            lastLocation = nextLocation;
        }

    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Mathf.Abs(Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB));
    }

    IEnumerator FadeOut()
    {
        grayOut.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
