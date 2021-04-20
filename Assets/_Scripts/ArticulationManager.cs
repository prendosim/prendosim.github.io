using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationManager : MonoBehaviour
{
    public StateObject roboState; 
    // ArticulationBody[] articulationBodies; 
    public ArticulationJointController[] artJOintCnt;
    int sumOfContactPoints = 0; 

    // void Start()
    // {
    //     articulationBodies = GetComponentsInChildren<ArticulationBody>(); 
    //     for(int i = 0; i<articulationBodies.Length; i++)
    //     {    
    //         artJOintCnt[i] = articulationBodies[i].GetComponent<ArticulationJointController>();
    //     }
    // }

    // void Update()
    // {
    //     for(int i = 0; i<artJOintCnt.Length; i++)
    //     {
    //         // Vector3 angVel = articulationBodies[i].angularVelocity;
    //         sumOfContactPoints+=artJOintCnt[i].inContact; 
    //     }
    //     if(sumOfContactPoints>3)
    //     {
    //         roboState.activateGravityMass = true;
    //     }
    // }
}
