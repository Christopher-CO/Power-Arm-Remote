using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Output : MonoBehaviour
{

    [SerializeField]
    private Transform IKTarget;
    private Vector3 lastPosition;
    [SerializeField]
    private UI_Set_Value_Text powerOutputUI;
    private float distance, velocity = 0, mass = 10, lastKE = 0, currKE = 0, work = 0, power = 0;
    private float divider = 4000000;
    // Start is called before the first frame update
    void Start()
    {
        if (powerOutputUI == null)
            powerOutputUI = GetComponent<UI_Set_Value_Text>();
        lastPosition = IKTarget.position;
        StartCoroutine(CalculatePower());
    }

    // Update is called once per frame
    IEnumerator CalculatePower()
    {
        while (true)
        {
            distance = Vector3.Distance(lastPosition, IKTarget.position);
            velocity = distance / Time.fixedDeltaTime;
            currKE = mass * Mathf.Pow(velocity, 2) / 2;
            work = currKE - lastKE;
            power = Mathf.Abs(work / Time.fixedDeltaTime);
            powerOutputUI.SetValue(power/divider);
            lastPosition = IKTarget.position;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
