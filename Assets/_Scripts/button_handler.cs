using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class button_handler : MonoBehaviour
{
    public GameObject obj;

    // Change orientation
    public void changeOrientation()
    {
        // This returns the GameObject named Hand.
        obj = GameObject.Find("scissors_up_y");

        obj.transform.Rotate(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Space.World);
        
        Debug.Log("OK");
    }
}
