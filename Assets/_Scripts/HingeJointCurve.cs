using UnityEngine;
using System.Collections;

public class HingeJointCurve : MonoBehaviour
{
    public AnimationCurve JointMotionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    public bool OpenHandEvent;
    public bool CloseHandEvent;
    public bool stopHandRoutine; 

    public float MaxDist;
    public float SpeedMultiplier;
    
    //--------------------------------------
    public HingeJoint ThisHinge;

    //public Transform target;
    [Tooltip("Only use one of these values at a time. Toggle invert if the rotation is backwards.")]
    public bool invert;
    public bool RandoPose;

    float curveAmount;
    float curveTime; 
    
    bool contactObjSurface = false;
    public bool closeHand, closing, openHand, opening;

    public bool jointClosedContact = false; 

    Coroutine closeR; 
    Coroutine openR;
    Coroutine stopR; 

    void Start()
    {
        // if(roboState == null) // Assign 
        // {
        //     roboState = Resources.Load<StateObject>("StateObject");
        // }

        JointMotionCurve.preWrapMode = WrapMode.PingPong;
        JointMotionCurve.postWrapMode = WrapMode.PingPong;
    }

    private void OnCollisionEnter(Collision other) 
    {
        jointClosedContact = true;
    }
    private void OnCollisionStay(Collision other) 
    {
        jointClosedContact = true;
    }
    private void OnCollisionExit(Collision other) 
    {
        jointClosedContact = false;
    }

    public IEnumerator CloseHandRoutine()
    {
        closing = true;
        curveAmount = JointMotionCurve.Evaluate(curveTime);

        while (curveTime < 1.0f) //  & !jointClosedContact
        {
            curveTime += Time.fixedDeltaTime * SpeedMultiplier;
            curveAmount = JointMotionCurve.Evaluate(curveTime);

            yield return null;
        }
        closing = false;
    }
    public IEnumerator OpenHandRoutine()
    {
        opening = true; 
        curveAmount = JointMotionCurve.Evaluate(curveTime);

        while (curveTime > 0f)
        {
            curveTime -= Time.fixedDeltaTime * SpeedMultiplier;
            curveAmount = JointMotionCurve.Evaluate(curveTime);

            yield return null;
        }
        opening = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) | CloseHandEvent)
        {
            if(openR != null)
                StopCoroutine(openR);

            // curveTime = 0f;

            closeR = StartCoroutine(CloseHandRoutine());
            CloseHandEvent = false;
        }
        if (Input.GetKeyUp(KeyCode.O) | OpenHandEvent)
        {
            if(closeR != null)
                StopCoroutine(closeR);

            //curveTime = 1.1f;
            // StartCoroutine(OpenHandRoutine());
            openR = StartCoroutine(OpenHandRoutine());
            OpenHandEvent = false;
        }
        if(Input.GetKeyDown(KeyCode.R)) // | roboState.stopHandRoutine)
        {
            if(closeR != null)
                StopCoroutine(closeR);
            if(openR != null)
                StopCoroutine(openR);
        }
        if(stopHandRoutine)
        {
            if(closeR != null)
                StopCoroutine(closeR);
        }

        HingeMover();

    }

    void HingeMover()
    {
        if (ThisHinge != null)
        {
            JointSpring js;
            js = ThisHinge.spring;
            js.targetPosition = MaxDist * curveAmount;

            if (js.targetPosition > 180)
                js.targetPosition = js.targetPosition - 360;
            if (invert)
                js.targetPosition = js.targetPosition * -1;

            js.targetPosition = Mathf.Clamp(js.targetPosition, ThisHinge.limits.min + 5, ThisHinge.limits.max - 5);

            ThisHinge.spring = js;
        }
    }

}
