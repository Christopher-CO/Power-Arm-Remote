using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Set_Value_Text : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
    private TMP_TextInfo textInfo;
    private float value;
    //[Range(0, (Angle_Measure.angle % 1).ToString().Length - 2)]
    private int precision = 0;
    //limit to anglePrecision
    private int intCount, precisionLimit;

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshPro>();

        textInfo = text.textInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(float userValue)
    {
        intCount = Mathf.Floor(userValue).ToString().Length;
        precisionLimit = 7 - intCount;
        precision = Mathf.Clamp(precision, 0, precisionLimit);
        float roundedValue = Mathf.Round(userValue * Mathf.Pow(10, precision)) / Mathf.Pow(10, precision);
        //Debug.Log(precision + " " + (10 ^ precision));
        //Debug.Log("roundedAngle: " + roundedAngle);

        string format = "000";
        if (precision > 0)
            format += ".";
        for (int i = 0; i < precision; i++)
        {
            format += "0";
        }
        text.text = roundedValue.ToString(format);

        value = userValue;
    }

    public void SetPrecision(float userPrecision)
    {
        precision = Mathf.FloorToInt(userPrecision);
        SetValue(value);
    }
}
