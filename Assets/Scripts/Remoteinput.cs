#if (UNITY_STANDALONE || UNITY_EDITOR) //if not on Desktop, exclude from compile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Remoteinput : MonoBehaviour
{
    public GameObject target;
    private Transform targetTransform;
    float upperLimitX = -3.8f;
    float lowerLimitX = -14.18f;
    float upperLimitY = 18.61f;
    float lowerLimitY = -4.0f;
    public float topSpeed = 5;
    private SerialPort arduinoSerial = new SerialPort("COM5", 9600);
    private float batteryCharge, batteryUpdateDelay = 10f;
    private bool batteryUpdated = false;
    [SerializeField]
    private UI_Set_Value_Text batteryUI;
 
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        arduinoSerial.Open();
        arduinoSerial.ReadTimeout = 1;
        arduinoSerial.WriteTimeout = 1;
        Debug.Log("Start Function completed successfully");
        StartCoroutine(checkAndUpdateBatteryLevel());
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        float throttleX;
        float throttleY;
        if (arduinoSerial.IsOpen)
        {
            try
            {
                string input = arduinoSerial.ReadLine();
                Debug.Log("Line read: "  + input);
                string[] throttles = input.Split(',');

                if (throttles.Length > 1)
                {
                    throttleX = float.Parse(throttles[0]);
                    throttleY = float.Parse(throttles[1]);
                    MoveIKTarget(throttleX, throttleY);
                }

                // check to see if battery level was added (in the future consider using 
                // a switch statement and keywords to determine which values where sent
                // and in what order)
                if (throttles.Length > 2)
                {
                    batteryCharge = float.Parse(throttles[2]);
                    batteryUpdated = true;
                }
            }
            catch (System.Exception)
            {

            }
        }
        else
            Debug.Log("Serial not open\n");
    }

    void MoveIKTarget(float throttleX, float throttleY)
    {
        Vector3 displacementX = new Vector3(throttleX, 0, 0);
        Vector3 displacementY = new Vector3(0, throttleY, 0);
        displacementX *= topSpeed * Time.fixedDeltaTime;
        displacementY *= topSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = targetTransform.position + displacementX + displacementY;
        if(newPosition.x <= upperLimitX && newPosition.x >= lowerLimitX) 
        {
            //targetTransform.Translate(displacementX, Space.World);
            targetTransform.position += displacementX;
        }
        if (newPosition.y <= upperLimitY && newPosition.y >= lowerLimitY)
        {
            //targetTransform.Translate(displacementY, Space.World);
            targetTransform.position += displacementY;
        }
    }

    public void SetTopSpeed(float userTopSpeed)
    {
        topSpeed = userTopSpeed/10; //convert from cm/s using scaling factor
    }

    IEnumerator checkAndUpdateBatteryLevel()
    {
        while (true)
        {
            while (!batteryUpdated && arduinoSerial.IsOpen)
            {
                try
                {
                    arduinoSerial.Write(new char[] { 'B' }, 0, 1);
                }
                catch
                {

                }
                yield return new WaitForSeconds(2f); //wait for battery status update
            }
            batteryUpdated = false;
            batteryUI.SetValue(batteryCharge);
            yield return new WaitForSeconds(batteryUpdateDelay); //delay until next update
        }
    }

    public void OnApplicationQuit()
    {
        if (arduinoSerial.IsOpen)
        {
            print("serial port closed");
            arduinoSerial.Close();
        }
    }
}
#endif
