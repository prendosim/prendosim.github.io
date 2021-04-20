using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseMass : MonoBehaviour
{
    public StateObject roboState; 
    TMP_Text txt_Mass;
    Rigidbody[] rbods = new Rigidbody[2];
    ArticulationBody[] artBods = new ArticulationBody[2];
    ArticulationBody rootBod = new ArticulationBody(); 

    public float massStepSize = 0.01f; 

    private void Start()
    {
        txt_Mass = GameObject.Find("txt_MassDisplay").GetComponent<TMP_Text>();
        // rbods = gameObject.GetComponent<Rigidbody>();
        rbods = gameObject.GetComponentsInChildren<Rigidbody>();
        // artBods = gameObject.GetComponentsInChildren<ArticulationBody>(); 
        // rootBod = GetComponent<ArticulationBody>(); 
        // rootBod.immovable = true;
        // rbods.mass = 0f;
    }

    void ActivateGravityMass()
    {
            // Increase the drag values before reactivatio of physics to avoid glitches
            // foreach(ArticulationBody rbod in artBods)
            foreach(Rigidbody rbod in rbods)
            {
                // rbod.linearDamping = 10f;
                // rbod.angularDamping = 10f; 
                
                rbod.drag = 10f;
                rbod.angularDrag = 10f;
                
                // // Reactivate physics 
                rbod.isKinematic = false;
                rbod.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rbod.useGravity = true;
            }
            // Start mass increase procedure 
            StartCoroutine(IncreaseMassRoutine());
    }

    private void Update() 
    {
        if(roboState.activateGravityMass)
        {
            ActivateGravityMass();
            roboState.activateGravityMass = false; 
        }
        
    }

    // private void FixedUpdate() 
    // {
    //     if(roboState.inContact == true)
    //     {
    //         Invoke("ActivateGravityMass", 2f); 
    //     }
    // }

    IEnumerator IncreaseMassRoutine()
    {
        // If object is stable (zero velocity) for 1 second, then start increasing the mass
        // yield return new WaitForSeconds(1f); 

        // Reset the drag values to their defaults. 
            // rootBod.immovable = false;
            // foreach(ArticulationBody rbod in artBods)
            foreach(Rigidbody rbod in rbods)
            {    
                // rbod.linearDamping = 0.05f;
                // rbod.angularDamping = 0.05f; 
                rbod.drag = 0.05f;
                rbod.angularDrag = 0.05f;
            }
        // float startTime = Time.time; 
        // float elapsedTime = 0f; 

        // Increase mass by a certain step size ? 
        while (roboState.inContact) // | elapsedTime<roboState.maxTime
        {
            if (rbods != null)
            // if(artBods != null)
            {
            //  foreach(Rigidbody rbod in rbods)
                foreach(ArticulationBody rbod in artBods)
                {
                    txt_Mass.text = "Mass: " + rbod.mass.ToString("F1") + "kg";
                    rbod.mass += massStepSize * Time.fixedDeltaTime;
                }
                // elapsedTime = Time.time-startTime; 
                // Debug.Log("Time: " + elapsedTime);
            }
            yield return null;
        }
    }

}
