using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserFriendlyGrasp : MonoBehaviour
{
    public StateObject roboState; 
    public Toggle tog_userFriendly; 


    void Update()
    {
        if(tog_userFriendly.isOn)
        {
           roboState.userFriendly = true;
        }
        else
        {
           roboState.userFriendly = false;
        }
    }

}
