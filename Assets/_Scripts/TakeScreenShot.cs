using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour
{
    public string view;
    public GameObject obj;
    string paths;

    void Start()
    {
        paths = Application.dataPath + "/Resources/";
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            ScreenCapture.CaptureScreenshot(paths + obj.name +"_"+ view + ".png", 4);
        }
    }
}
