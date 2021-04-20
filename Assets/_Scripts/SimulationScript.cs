using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationScript : MonoBehaviour
{
    StateObject roboState; 

    public GameObject[] Grippers;
    public GameObject[] ProxyGrippers;
    public GameObject[] ObjectsToSpawn;
    public Vector3  offsetPosition; 
    public float massIncrementSize = 0.05f; 
    public float criticalMassSpeedThreshold = 0.01f;
    public float stabilityTestDuration = 2f;

    float criticalMass; 

    GameObject barrettHand, barrettHandProxy, jawGripper, jawGripperProxy; 
    GameObject targetObjectInstance; 

    Coroutine simulationRoutine, spawnRoutine, increaseMassRoutine, stabilityRoutine; 

    void Start()
    {
        roboState = Resources.Load<StateObject>("StateObject");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(simulationRoutine!=null)
                StopCoroutine(simulationRoutine);
            simulationRoutine = StartCoroutine(SimulationSequence());
        }
    }

    IEnumerator SimulationSequence()
    {
        // Spawn gripper or hand or what ever other robotic gripper 
        if(barrettHand!=null)
            Destroy(barrettHand);
        barrettHand = Instantiate(Grippers[0]); 

        // Create random index and ring finger prox joint angles and assign them to the joints 
        ArticulateProximalJoints[] artProxJoints = barrettHand.GetComponentsInChildren<ArticulateProximalJoints>();
        float goalAngle = Random.Range(1f, -110f); // -180f
        for(int i = 0; i<artProxJoints.Length; i++)
        {
            artProxJoints[i].BroadcastMessage("SetRandomJointAngle", goalAngle);
        } 

        yield return new WaitForSeconds(0.5f);

        // Spawn target object 
        if(spawnRoutine!=null)
        {
            if(targetObjectInstance!= null)
                Destroy(targetObjectInstance);
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = StartCoroutine(SpawnObject());
        yield return new WaitForSeconds(0.5f);

        // Close gripper
        // roboState.closeHand = true; 


        ArticulationJointController[] artjoints = barrettHand.GetComponentsInChildren<ArticulationJointController>();
        for(int i = 0; i<artjoints.Length; i++)
        {
            artjoints[i].BroadcastMessage("CloseHand");
        } 
        yield return new WaitForSeconds(2f);

        // Activate gravity i.e. reduce drag values from rigidbody 
        Rigidbody rb = targetObjectInstance.GetComponent<Rigidbody>(); 
        rb.drag = 0.05f; rb.angularDrag = 0.05f;

        // Check stability (i.e. wait 2 seconds to see if object is still present i.e. not destroyed)
        if(stabilityRoutine!=null)
            StopCoroutine(stabilityRoutine);
        stabilityRoutine = StartCoroutine(CheckStability(rb, stabilityTestDuration, false)); 
        yield return new WaitForSeconds(stabilityTestDuration);

        // Then test critical mass (mass increased gradually)
        if(increaseMassRoutine!=null)
            StopCoroutine(increaseMassRoutine);
        increaseMassRoutine = StartCoroutine(IncreaseMass(rb, massIncrementSize));

        // Again check stability while increasing the mass
        // This time if the stability speed changes beyond the critical mass threshold, then we record the critical mass
        if(stabilityRoutine!=null)
            StopCoroutine(stabilityRoutine);
        stabilityRoutine = StartCoroutine(CheckStability(rb, stabilityTestDuration, true)); 

        // If object is still in hand, then record data and take screenshot
        roboState.recordData = true; 
        yield return new WaitForSeconds(1f);

        // Open gripper
        // roboState.openHand = true; 
        for(int i = 0; i<artjoints.Length; i++)
        {
            artjoints[i].BroadcastMessage("OpenHand");
        } 
        yield return new WaitForSeconds(2f);
        Destroy(barrettHand);

        yield return null;
    }

    IEnumerator CheckStability(Rigidbody rigB, float duration, bool criticalMassTest)
    {
        float startTime = Time.time; 
        float elapsedTime = 0f; 
        float speed = 0f; 

        if(rigB!=null)
        {
            if(criticalMassTest)
            {
                while(elapsedTime<duration)
                {
                    if(speed < criticalMassSpeedThreshold)
                    {
                        speed = rigB.velocity.magnitude;
                    }
                    elapsedTime=Time.time-startTime;
                    yield return null; 
                }
                if(rigB!=null)
                {
                    criticalMass = rigB.mass; 
                }
            }
            else
            {
                while(elapsedTime<duration)
                {
                    if(rigB!=null)
                    {
                        speed = rigB.velocity.magnitude;
                    }
                    elapsedTime=Time.time-startTime;
                    yield return null; 
                }
            }
        }
    }

    IEnumerator IncreaseMass(Rigidbody rigB, float stepSize)
    {   
        while(rigB!=null)
        {
            rigB.mass = rigB.mass + stepSize; 
            yield return null; 
        }
    }

    IEnumerator SpawnObject()
    {
        int randObj = Random.Range(0,ObjectsToSpawn.Length);
        Quaternion randRotation = Random.rotation;

        targetObjectInstance = Instantiate(ObjectsToSpawn[randObj], barrettHand.transform.position + offsetPosition, randRotation);
        yield return null; 
    }
}
