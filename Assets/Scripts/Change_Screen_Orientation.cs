using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Screen_Orientation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;   
    }
}
