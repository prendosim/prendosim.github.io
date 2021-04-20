using UnityEngine;
using System.Collections;

public class HingeJointScriptV2 : MonoBehaviour
{

    public Transform target;
    [Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
    public bool x, y, z, invert;

    HingeJoint hj;
    float targPos;
    //JointSpring js;

    void Start()
    {
        hj = GetComponent<HingeJoint>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.P))
        {
            JointSpring js;
            targPos += 1f;
            js.targetPosition = targPos; 
            Debug.Log("TargetAng: " + targPos.ToString());
        }

        if (hj != null)
        {
            if (x)
            {
                JointSpring js;
                js = hj.spring;

                js.targetPosition = target.transform.localEulerAngles.x;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min + 5, hj.limits.max - 5);

                hj.spring = js;
            }
            if (y)
            {
                JointSpring js;
                js = hj.spring;
                js.targetPosition = target.transform.localEulerAngles.y;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min + 5, hj.limits.max - 5);

                hj.spring = js;
            }
            if (z)
            {
                JointSpring js;
                js = hj.spring;
                js.targetPosition = target.transform.localEulerAngles.z;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min + 5, hj.limits.max - 5);

                hj.spring = js;
            }
        }
    }
}
