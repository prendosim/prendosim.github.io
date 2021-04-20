using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelGripper : MonoBehaviour
{
    public StateObject roboState; 

    public AnimationCurve JointMotionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    //public Transform LeftGripper, RightGripper;
    public Transform LeftAnchor, RightAnchor;
    public Transform LeftTarget, RightTarget;

    public Transform initLeftAnchorPos, initRightAnchorPos;

    void Update()
    {
        // Close gripper 
        if(roboState.pGripperClose) // Input.GetKeyDown(KeyCode.Z
        {
            StartCoroutine(MoveObj(LeftAnchor, LeftTarget));
            StartCoroutine(MoveObj(RightAnchor, RightTarget));
            roboState.pGripperClose = false;
        }
        // Open gripper 
        if (roboState.pGripperOpen)
        {
            StartCoroutine(MoveObj(LeftAnchor, initLeftAnchorPos));
            StartCoroutine(MoveObj(RightAnchor, initRightAnchorPos));
            roboState.pGripperOpen = false;
        }
    }

    IEnumerator MoveObj(Transform obj, Transform target)
    {
        while(obj.localPosition != target.localPosition)
        {
            obj.localPosition = Vector3.MoveTowards(obj.localPosition, target.localPosition, Time.fixedDeltaTime);
            yield return null; 
        }
    }
}
