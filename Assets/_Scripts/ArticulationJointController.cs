using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationJointController : MonoBehaviour
{
    StateObject roboState; 
    public float rotationSpeed = 1f; 
    private ArticulationBody articulation;
    float rotationalGoal = 0f;
    float minRot, maxRot; 
    Coroutine closeRoutine, openRoutine; 
    public int inContact = 0;  

    void Start()
    {
        roboState = Resources.Load<StateObject>("StateObject");
        articulation = GetComponent<ArticulationBody>(); 
        minRot = articulation.xDrive.lowerLimit; 
        maxRot = articulation.xDrive.upperLimit; 
    }
    
    public void CloseHand()
    {
            if(openRoutine!=null)
            {
                StopCoroutine(openRoutine); 
            }
            if(closeRoutine!=null)
            {
                StopCoroutine(closeRoutine); 
            }
            // rotationalGoal += 1f;
            // RotateTo(rotationalGoal);
            try
            {
                float currRot = articulation.xDrive.target;
                closeRoutine = StartCoroutine(ActuateGripper(currRot,maxRot));
            }
            catch
            {}
    }
    public void OpenHand() 
    {
        if(closeRoutine!=null)
            {
                StopCoroutine(closeRoutine); 
            }
            if(openRoutine!=null)
            {
                StopCoroutine(openRoutine); 
            }
            // rotationalGoal -= 1f;
            // RotateTo(rotationalGoal);
            try
            {
                float currRot = articulation.xDrive.target;
                openRoutine = StartCoroutine(ActuateGripper(currRot,minRot));
            }
            catch
            {}

    }
    public void StopHand()
    {
        StopAllCoroutines(); 
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Q) | roboState.closeHand)
        {
            if(openRoutine!=null)
            {
                StopCoroutine(openRoutine); 
            }
            if(closeRoutine!=null)
            {
                StopCoroutine(closeRoutine); 
            }
            // rotationalGoal += 1f;
            // RotateTo(rotationalGoal);
            float currRot = articulation.xDrive.target;
            closeRoutine = StartCoroutine(ActuateGripper(currRot,maxRot));
            roboState.closeHand = false; 
        }
        if(Input.GetKey(KeyCode.W) | roboState.openHand)
        {
            if(closeRoutine!=null)
            {
                StopCoroutine(closeRoutine); 
            }
            if(openRoutine!=null)
            {
                StopCoroutine(openRoutine); 
            }
            // rotationalGoal -= 1f;
            // RotateTo(rotationalGoal);
            float currRot = articulation.xDrive.target;
            openRoutine = StartCoroutine(ActuateGripper(currRot,minRot));
            roboState.openHand = false; 
        }
        if(Input.GetKey(KeyCode.R) | inContact == 1 | roboState.stopHand)
        {
            StopAllCoroutines(); 
            roboState.stopHand = false; 
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "targetObject")
        {
            inContact = 1; 
            Debug.Log("Finger contacted object !!! " + gameObject.name); 

        }
    }
    // private void OnCollisionExit(Collision other) 
    // {
    //     if(other.collider.tag == "targetObject")
    //     {
    //         inContact = false; 
    //     }
    // }

    IEnumerator ActuateGripper(float currRot, float goalRot)
    {   
        float incRot = 0f; 
        if(goalRot > currRot)
        {
            incRot = 1f; 
        }
        else
        {
            incRot = -1; 
        }

        while(currRot!=goalRot)
        {
            currRot += incRot * rotationSpeed;
            RotateTo(currRot);
            yield return null; 
        }
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }
}
