using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour
{
    public StateObject roboState; 
    public GameObject[] ObjectsToSpawn;
    public Transform RoboLocation;
    public Vector3 offsetPosition;
    Coroutine spawnRoutine; 
	bool spawnOnce = false; 
    GameObject targetObjectInstance; 

	void Update()
    {

        if(Input.GetKeyDown(KeyCode.Y) | roboState.activateGravityMass & !spawnOnce)
        {
            if(spawnRoutine!=null)
            {
                StopCoroutine(spawnRoutine);
            }
            spawnRoutine = StartCoroutine(SpawnObject());
            roboState.activateGravityMass = false; 
            spawnOnce= true; 
        }

        if(targetObjectInstance== null)
        {
            spawnOnce = false; 
        }
	}

    IEnumerator SpawnObject()
    {
        int randObj = Random.Range(0,ObjectsToSpawn.Length);
        Quaternion randRotation = Random.rotation;

        targetObjectInstance = Instantiate(ObjectsToSpawn[randObj], RoboLocation.position + offsetPosition, randRotation);
        yield return null; 
    }
}

    // public float Interval;
    // public float NumObjects;

    // private float SpawnTimer;
    // private int   SpawnCounter;

	// void Start()
    // {
    //     SpawnTimer   = 0.0f;
    //     SpawnCounter = 0;
	// }
        // if(SpawnCounter < NumObjects)
        // {
        //     SpawnTimer -= Time.deltaTime;

        //     if(SpawnTimer < 0.0f)
        //     {
        //         Instantiate(ObjectToSpawn, transform.position, transform.rotation);
        //         SpawnTimer = Interval;
        //         SpawnCounter++;
        //     }
        // }	