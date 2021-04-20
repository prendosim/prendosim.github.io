using UnityEngine;
using System.Collections;

public class HingeJointScript : MonoBehaviour
{
    public HingeJoint ThisHinge;

    public Transform target;
    [Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
    public bool x, y, z, invert;

    void Update()
    {
        if (ThisHinge != null)
        {
            if (x)
            {
                JointSpring js;
                js = ThisHinge.spring;
                js.targetPosition = target.transform.localEulerAngles.x;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, ThisHinge.limits.min + 5, ThisHinge.limits.max - 5);

                ThisHinge.spring = js;
            }
            if (y)
            {
                JointSpring js;
                js = ThisHinge.spring;
                js.targetPosition = target.transform.localEulerAngles.y;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, ThisHinge.limits.min + 5, ThisHinge.limits.max - 5);

                ThisHinge.spring = js;
            }
            if (z)
            {
                JointSpring js;
                js = ThisHinge.spring;
                js.targetPosition = target.transform.localEulerAngles.z;
                if (js.targetPosition > 180)
                    js.targetPosition = js.targetPosition - 360;
                if (invert)
                    js.targetPosition = js.targetPosition * -1;

                js.targetPosition = Mathf.Clamp(js.targetPosition, ThisHinge.limits.min + 5, ThisHinge.limits.max - 5);

                ThisHinge.spring = js;
            }
        }
    }
}
