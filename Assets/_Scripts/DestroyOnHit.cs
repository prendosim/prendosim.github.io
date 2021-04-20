using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public StateObject roboState; 

    public static string targetObjectName;  

    private void Start() 
    {
         Destroy(gameObject, roboState.maxTime + 0.5f);
    }
    void OnTriggerEnter(Collider other)
    {
        targetObjectName = gameObject.name;

        if (other.gameObject.tag == "DestroyTag")
            {
                Destroy(gameObject, 0.05f);
            }
    }

    // void ReleaseContact()
    // {
    //     roboState.inContact = false; 
    // }

    // private void OnTriggerStay(Collider other) 
    // {
    //     if(other.gameObject.tag == "Robot")   
    //     {
    //         roboState.inContact = true; 
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     roboState.inContact = false; 
    // }
}
