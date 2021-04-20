using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulateProximalJoints : MonoBehaviour
{
    private ArticulationBody articulation;
    float minRot, maxRot;
    Coroutine setRandomRotation;
    public float rotationSpeed = 2f;
    public bool invert = false;

    // void Start()
    // {
    //     articulation = GetComponent<ArticulationBody>();
    //     minRot = articulation.xDrive.lowerLimit;
    //     maxRot = articulation.xDrive.upperLimit;
    // }

    public void SetRandomJointAngle(float goalAngle)
    {
        articulation = GetComponent<ArticulationBody>();
        minRot = articulation.xDrive.lowerLimit;
        maxRot = articulation.xDrive.upperLimit;

        if (setRandomRotation != null)
        {
            StopCoroutine(setRandomRotation);
        }

        try
        {
            // float currRot = articulation.xDrive.target;
            setRandomRotation = StartCoroutine(ActuateGripper(minRot, goalAngle));
        }
        catch
        { }
    }

    IEnumerator ActuateGripper(float currRot, float goalRot)
    {

        if (invert)
        {
            RotateTo(-goalRot);
        }
        else
        {
            RotateTo(goalRot);
        }
        yield return null;
        // float incRot = 1f;

        // while (currRot != goalRot)
        // {
        //     if(invert)
        //     {
        //         currRot -= incRot * rotationSpeed;
        //     }
        //     else
        //     {
        //         currRot += incRot * rotationSpeed;
        //     }
        //     RotateTo(currRot);
        //     yield return null;
        // }
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }

}
