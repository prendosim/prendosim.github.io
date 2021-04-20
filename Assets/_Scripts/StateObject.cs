using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateObject", menuName = "Robot States")]
public class StateObject : ScriptableObject
{
    public bool SpawnObjects = false;
    public bool pGripperOpen = false;
    public bool pGripperClose = false; 
    public bool inContact = false;
    public float maxTime = 5f;
    public bool gripperClosed; 
    public int[] fingerTipContacts = new int[3]{0,0,0}; 
    public bool userFriendly; 
    public bool handClosed = false;
    public bool activateGravityMass = false;
    // public int[] stopHandRoutine = new int[3]{0,0,0};  
    public bool stopHandRoutine = false; 

    public bool closeHand, openHand, stopHand, recordData; 
}
