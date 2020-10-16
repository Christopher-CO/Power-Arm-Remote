using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//namespace TMPro.Examples

public class Anlge_Value : MonoBehaviour
{
    [SerializeField]
    private Angle_Measure Angle_Measure;
    [SerializeField]
    private TextMeshPro text;
    private TMP_TextInfo textInfo;
    //[Range(0, (Angle_Measure.angle % 1).ToString().Length - 2)]
    private int precision = 0;
    //limit to anglePrecision
    private int intCount, precisionLimit, charCount;
    private string angle;
    public float offset = 0;
    public bool keepTanget;

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
        intCount = Mathf.Floor(Angle_Measure.angle).ToString().Length;
        precisionLimit = 9 - intCount;
        precision = Mathf.Clamp(precision, 0, precisionLimit);
        charCount = intCount + precision + (precision > 0 ? 2 : 1); //+ (1)2 for decimal and degree symbol
        float roundedAngle = Mathf.Round(Angle_Measure.angle * Mathf.Pow(10, precision)) / Mathf.Pow(10, precision);
        //Debug.Log(precision + " " + (10 ^ precision));
        //Debug.Log("roundedAngle: " + roundedAngle);
        angle = roundedAngle.ToString("F"+precision.ToString());

        text.text = angle + "°";

        text.transform.position = Angle_Measure.lowerBone.transform.position;

        //move TMP object location to or from angle according to offset
        Vector3 tmpDir = Vector3.zero;
        tmpDir += Angle_Measure.upperBone.transform.up * (Angle_Measure.flipDir ^ Angle_Measure.flipStartDir ? -1 : 1);
        tmpDir += Angle_Measure.lowerBone.transform.up * (Angle_Measure.flipDir ? -1 : 1);
        tmpDir = tmpDir.normalized * (Angle_Measure.radius * (offset+1));
        //Debug.Log("tmpDir w/o bone offset: " + tmpDir);
        
        //account for offset from center of angle arc
        tmpDir += (Angle_Measure.flipStartDir ? -1 * Angle_Measure.upperBone.up : Angle_Measure.upperBone.up) * (Angle_Measure.flipDir?-Angle_Measure.offset:Angle_Measure.offset);
        tmpDir += Angle_Measure.lowerBone.up * (Angle_Measure.flipDir?-Angle_Measure.offset:Angle_Measure.offset);

        text.rectTransform.position += tmpDir;

        //**attempted to always keep text tangent/outisde of angle measure arc**//
        if (text.textInfo.characterCount >= 1) {
            float textWidth = text.textInfo.characterInfo[(text.textInfo.characterCount - 1)].bottomRight.x;
            //float textHeight = text.renderedHeight;
            //Debug.Log("Rightmost char xpos: " + text.renderedWidth + " " + text.textInfo.characterInfo[text.textInfo.characterCount - 1].bottomRight.x);
            //Debug.Log("max height: " + textHeight);
            if (keepTanget)
                text.rectTransform.position += text.rectTransform.right * textWidth / 2 * (Angle_Measure.upperBone.transform.right.normalized.x);
        }
        //text.rectTransform.position += text.rectTransform.up * textHeight/2;

        //**initial attempt to fill characters in the TMP object cahracter by character**//
        /* 
        text.rectTransform.position += Angle_Measure.lowerBone.transform.up * Angle_Measure.radius * (Angle_Measure.flipDir ? -1 : 1);
        text.rectTransform.position += Angle_Measure.upperBone.transform.up * Angle_Measure.radius * (Angle_Measure.flipDir? -1 : 1);

        textInfo.characterInfo = new TMP_CharacterInfo[charCount]; 

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topLeft = Vector3.zero;
        Vector3 bottomRight = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        Debug.Log((Angle_Measure.angle % 1).ToString() + " " + (Angle_Measure.angle % 1).ToString().Length);
        Debug.Log(angle + " " + intCount + " " + precision + " " + precisionLimit + " " + charCount);
           
        TMP_CharacterInfo currChar;
        for (int i = 0; i < intCount; i++)
        {
            currChar = new TMP_CharacterInfo();
            currChar.character = angle[i];
            textInfo.characterInfo[i] = currChar;
        }

        if (precision > 0)
        {
            currChar = new TMP_CharacterInfo();
            currChar.character = '.';
            textInfo.characterInfo[intCount] = currChar;
        }

        for (int i = intCount; i < intCount + precision; i++)
        {
            currChar = new TMP_CharacterInfo();
            currChar.character = angle[i];
            textInfo.characterInfo[i] = currChar;
        }

        currChar = new TMP_CharacterInfo();
        currChar.character = '°';
        textInfo.characterInfo[intCount + precision] = currChar;
        */
    }

    public void SetPrecision(float userPrecision)
    {
        precision = (int)Mathf.Floor(userPrecision);
    }
}

