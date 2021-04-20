using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelGripperScript : MonoBehaviour
{
    public StateObject roboState; 

    public AnimationCurve JointMotionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    //public Animation GripperAnimation;
    public bool GripperClosed = false;
    public Transform Target;
    public GameObject InitPose; 

    public float MaxDist = 0.5f;
    public float SpeedMultiplier = 1f;
    public bool invert;

    float curveAmount;
    float curveTime = 0f;
    bool targetReached = false;
    bool inContact = false; 

    Coroutine openRoutine, closeRoutine; 

    #region Test Code

    private void Start()
    {
        InitPose.transform.localPosition = transform.localPosition;
    }

    private void Update()
    {
        if(roboState.pGripperClose) // Input.GetKeyDown(KeyCode.Y) | 
        {
            roboState.SpawnObjects = true; 

            if (openRoutine!=null)
                StopCoroutine(openRoutine);
            closeRoutine = StartCoroutine(Closer());
            roboState.pGripperClose = false; 
        }
        if (roboState.pGripperOpen) // Input.GetKeyDown(KeyCode.U) | 
        {
            if (closeRoutine != null)
                StopCoroutine(closeRoutine); 
            openRoutine = StartCoroutine(Opener());
            roboState.pGripperOpen = false; 
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            roboState.SpawnObjects = true;
            openRoutine = StartCoroutine(Opener());
        }
    }
    #endregion

    public IEnumerator Closer()
    {
        float dist = Mathf.Abs(Vector3.Distance(transform.localPosition, Target.localPosition));

        while (dist > 0f)
        {
            float step = SpeedMultiplier * Time.fixedDeltaTime; // calculate distance to move
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Target.localPosition, step);

            dist -= step;

            yield return null; 
        }
    }
    public IEnumerator Opener()
    {
        float dist = Mathf.Abs(Vector3.Distance(transform.localPosition, InitPose.transform.localPosition));
        //Debug.Log("Opening Dist: " + dist.ToString("F2"));

        while (dist > 0f)
        {
            float step = SpeedMultiplier * Time.fixedDeltaTime; // calculate distance to move
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, InitPose.transform.localPosition, step);

            dist -= step;

            yield return null;
        }
    }

    public IEnumerator CloseHandRoutine()
    {
        curveAmount = JointMotionCurve.Evaluate(curveTime);

        while (curveTime < 1.0f)
        {
            curveTime += Time.fixedDeltaTime * SpeedMultiplier;
            curveAmount = JointMotionCurve.Evaluate(curveTime);

            if (!invert)
            {
                curveTime -= Time.fixedDeltaTime * SpeedMultiplier;
                curveAmount = JointMotionCurve.Evaluate(curveTime);
                transform.localPosition = new Vector3(transform.localPosition.x,
                                                      transform.localPosition.y,
                                                      transform.localPosition.z + curveAmount);
            }
            else
            {
                curveTime -= Time.fixedDeltaTime * SpeedMultiplier;
                curveAmount = JointMotionCurve.Evaluate(curveTime);
                transform.localPosition = new Vector3(transform.localPosition.x,
                                                      transform.localPosition.y,
                                                      transform.localPosition.z - curveAmount);
            }
            yield return null;
        }

        curveTime = 0f;
    }

    public IEnumerator OpenHandRoutine()
    {
        curveAmount = JointMotionCurve.Evaluate(curveTime);
        curveTime = 1f;

        while (curveTime > 0f)
        {
            if (!invert)
            {
                curveTime -= Time.fixedDeltaTime * SpeedMultiplier;
                curveAmount = JointMotionCurve.Evaluate(curveTime);
                transform.localPosition = new Vector3(transform.localPosition.x,
                                                      transform.localPosition.y, 
                                                      transform.localPosition.z - curveAmount);
            }
            else
            {
                curveTime -= Time.fixedDeltaTime * SpeedMultiplier;
                curveAmount = JointMotionCurve.Evaluate(curveTime);
                transform.position = new Vector3(transform.localPosition.x, 
                                                 transform.localPosition.y, 
                                                 transform.localPosition.z + curveAmount);
            }
            //Debug.Log(gameObject.name + "\t" + curveTime);

            yield return null;
        }

        curveTime = 0f;

    }
    
    #region Old Code
    //IEnumerator CloseGrip()
    //{
    //    GripperAnimation.Play();

    //    while (GripperAnimation.isPlaying)
    //        yield return null;

    //    GripperClosed = true;
    //}

    //IEnumerator OpenGrip()
    //{
    //    GripperAnimation.Rewind();

    //    while (GripperAnimation.isPlaying)
    //        yield return null;

    //    GripperClosed = false;
    //}
    #endregion

}
