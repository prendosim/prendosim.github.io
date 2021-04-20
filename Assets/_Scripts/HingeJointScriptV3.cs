using UnityEngine;
using System.Collections;

public class HingeJointScriptV3 : MonoBehaviour
{
    [Tooltip("Index = 0, Ring = 1, Thumb = 2")]
    public int idxID = 0; // 0 = index; 1 = ring; 2 = thumb
    public HingeJoint[] hj2 = new HingeJoint[3];

    public Transform target;
    [Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
    public bool x, y, z, invert;

    void Start()
    {
        hj2 = FindObjectsOfType<HingeJoint>();
        Debug.Log("Hinge joints: " + hj2.Length.ToString());
        //hj2[0] = GetComponent<HingeJoint>();
        //hj2[1] = GetComponent<HingeJoint>();
        //hj2[2] = GetComponent<HingeJoint>();
    }

    void Update()
    {
        if (hj2[idxID] != null)
        {
            if (x)
            {
                //Debug.Log("Target X angle: " + target.transform.localEulerAngles.x);

                JointSpring js;
                js = hj2[idxID].spring;
                js.targetPosition = target.transform.localEulerAngles.x;

                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj2[idxID].limits.min + 5, hj2[idxID].limits.max - 5);

                hj2[idxID].spring = js;
            }
            if (y)
            {
                //Debug.Log("Target Y angle: " + target.transform.localEulerAngles.y);

                JointSpring js;
                js = hj2[idxID].spring;
                js.targetPosition = target.transform.localEulerAngles.y;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj2[idxID].limits.min + 5, hj2[idxID].limits.max - 5);

                hj2[idxID].spring = js;
            }
            if (z)
            {
                //Debug.Log("Target Z angle: " + target.transform.localEulerAngles.z);

                JointSpring js;
                js = hj2[idxID].spring;
                js.targetPosition = target.transform.localEulerAngles.z;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj2[idxID].limits.min + 5, hj2[idxID].limits.max - 5);

                hj2[idxID].spring = js;
            }
        }
    }
}
