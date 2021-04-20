using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHandController : MonoBehaviour
{
    // Instead of animators, we should use coroutines. 
    // This will avoid a lot of the rotation issues I 
    // have been experiencing, which is a result of quaternion 
    //to euler conversion done under the hood by unity between 
    // inspector values (set in the animation) and the actual euler angles of the objects
    public Animator myAnimator;
    public Transform indexProx, ringProx;
    public Transform Gripper;
    public GameObject[] TargetObjects;
    public Vector3 ObjectOffset;

    GameObject targObj_prefab; 

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            myAnimator.SetBool("CloseHand", true);
            myAnimator.SetBool("OpenHand", false);
            myAnimator.SetBool("IdleHand", false);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            myAnimator.SetBool("OpenHand", true);
            myAnimator.SetBool("CloseHand", false);
            myAnimator.SetBool("IdleHand", false);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            myAnimator.SetBool("IdleHand", true);
            myAnimator.SetBool("OpenHand", false);
            myAnimator.SetBool("CloseHand", false);
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            GraspController(); 
        }
    }

    // Spawns objects and commands the gripper to close to test whether the grip is stable
    void GraspController()
    {
        // Create random index and ring finger prox joint angles and assing them to the joints 
        float IndexProx_rand = Random.Range(5f, 90f);
        float RingProx_rand = Random.Range(5f, -90f);
        indexProx.localEulerAngles = new Vector3(IndexProx_rand, indexProx.localEulerAngles.y, indexProx.localEulerAngles.z);
        ringProx.localEulerAngles = new Vector3(RingProx_rand, ringProx.localEulerAngles.y, ringProx.localEulerAngles.z);

        // Spawn random object at a random orientation but set to around the position of the robot gripper 
        int objectIndex = Random.Range(0, TargetObjects.Length);
        Quaternion randRotation = Random.rotation;
        targObj_prefab = Instantiate(TargetObjects[objectIndex], Gripper.position + ObjectOffset, randRotation);

        // Animate hand to close 
        myAnimator.SetBool("CloseHand", true);
        myAnimator.SetBool("OpenHand", false);
        myAnimator.SetBool("IdleHand", false);

        // After 2 seconds (time for animation to complete), activate target object gravity to see if the grasp holds 
        // (if the objects falls to the table then the grasp is not successfull) 
        Invoke("ActivateGravity", 2.5f);
    }

    void ActivateGravity()
    {
        Rigidbody rb = targObj_prefab.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; 
        rb.useGravity = true;
    }


    IEnumerator CloseSequence(int rotationAngle)
    {
        for (int i = 0; i < 140; i++)
        {
            //thumbTargAng = 0 - i;
            yield return null;
        }
    }

    IEnumerator OpenSequence(int rotationAngle)
    {
        for (int i = 0; i < 140; i++)
        {
            //thumbTargAng = -140 + i;
            yield return null;
        }
    }

}
