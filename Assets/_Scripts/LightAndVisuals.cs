using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightAndVisuals : MonoBehaviour
{

    public Toggle tog_light; 
    public Light myDirLight;


    void Update()
    {
        if(tog_light.isOn)
        {
            myDirLight.enabled = true;
        }
        else
        {
            myDirLight.enabled = false;
        }
    }

}

