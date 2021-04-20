using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTipContact : MonoBehaviour
{

    public StateObject roboState; 
    public int digitID; // ThumbTip = 0; IndexTip = 1; RngTip = 2;
    public HingeJointCurve hjCurve; 

    void Start()
    {
        for(int i = 0; i<roboState.fingerTipContacts.Length; i++)
        {
            roboState.fingerTipContacts[i] = 0;
        }
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        #region  Detect when the joint contacts an object's surface to stop the joint from bending further
        if(digitID == 0)
        {
            roboState.fingerTipContacts[0] = 1;
            hjCurve.jointClosedContact = true;
        }
        if(digitID == 1)
        {
            roboState.fingerTipContacts[1] = 1;
            hjCurve.jointClosedContact = true;
        }
        if(digitID == 2)
        {
            roboState.fingerTipContacts[2] = 1;
            hjCurve.jointClosedContact = true;
        }
        #endregion
    }
    //     private void OnCollisionExit(Collision other) 
    // {
    //     if(digitID == 0)
    //     {
    //         roboState.fingerTipContacts[0] = 0;
    //         hjCurve.jointClosedContact = false;
    //     }
    //     if(digitID == 1)
    //     {
    //         roboState.fingerTipContacts[1] = 0;
    //         hjCurve.jointClosedContact = false;
    //     }
    //     if(digitID == 2)
    //     {
    //         roboState.fingerTipContacts[2] = 0;
    //         hjCurve.jointClosedContact = false;
    //     }
    // }

}
