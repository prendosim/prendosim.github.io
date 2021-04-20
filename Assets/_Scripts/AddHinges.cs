/*
Hinge Joint System Adder (Runtime script!!!)

This code takes place in Unity's "Awake" state, before the actual "game play". 

 1.) This script will add three hinge joints with predetermined axes and locations (anchor).
 2.) For each added joint we will attempt to connect the corresponding gameobjects (either by name or by tag).
 3.) Finally we assign each joint to three instances of the "HingeJointScript.cs" also assgined to this gameobject through this script.

 Note: It is important to add these objects/scripts in the right sequence!!!

 The game object this script is assigned to requires a Rigidbody and a collider component, attempted to be added in script

 Author: Diar Karim
 Contact: diarkarim@gmail.com
 Date: 11/06/2020
 Version: 0.1
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))] // No gravity/ freeze all positions and rotations 
//[RequireComponent(typeof(Collider))] // Needs to be the size of the gameobject it is attached to i.e. this one 
public class AddHinges : MonoBehaviour
{

    HingeJoint hj_idx;
    HingeJoint hj_rng;
    HingeJoint hj_tmb;

    // Predetermined locations
    Vector3 idxJointAnchor = new Vector3(-0.0024f, 0.0036f, 0f);
    Vector3 rngJointAnchor = new Vector3(0.0026f, 0.0036f, 0f);
    Vector3 tmbJointAnchor = new Vector3(0f, 0.0086f, -0.00215f);
    // Predetermined axes
    Vector3Int idxJointAx = new Vector3Int(0, 0, -1);
    Vector3Int rngJointAx = new Vector3Int(0, 0, 1);
    Vector3Int tmbJointAx = new Vector3Int(-1, 0, 0);

    HingeJointScript idxHJScript;
    HingeJointScript rngHJScript;
    HingeJointScript tmbHJScript;

    // Alternative to above, using HingeJointCurve instead
    HingeJointCurve idxHJCurve;
    HingeJointCurve rngHJCurve;
    HingeJointCurve tmbHJCurve;

    public float speedMultiplier;


    void Awake()
    {
        // Index prox joint 
        PrepIndexFinger();

        // Ring prox joint
        PrepRingFinger();

        // Thumb prox-mid joint 
        PrepThumbFinger();
    }

    void PrepIndexFinger()
    {
        // Index joint 
        // 1st joint step (maybe later convert this to a loop with joint arrays)
        // Create first joint and configure as stated above with axes and location/anchor
        hj_idx = gameObject.AddComponent<HingeJoint>();
        hj_idx.anchor = idxJointAnchor;
        hj_idx.axis = idxJointAx;

        // Also configure joint with spring and limits (the "on" and then "off" step below is related to a Unity bug and is necessary)
        JointSpring spring = hj_idx.spring;
        spring.spring = 50f;
        spring.damper = 0.5f;
        spring.targetPosition = 0f;
        hj_idx.spring = spring;
        hj_idx.useSpring = true;

        // Unity bug related step (on/off limits) 
        JointLimits limits = hj_idx.limits;
        limits.min = -180f;
        limits.max = 180f;
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        hj_idx.limits = limits;

        hj_idx.useLimits = true;
        hj_idx.useLimits = false;
        hj_idx.useLimits = true;

        // Connect to affected gameobject (i.e. next joint down the hierarchy aka the joint that this hinge spring will control)
        // hj_idx.connectedBody = GameObject.FindGameObjectWithTag("idxPrx").GetComponent<Rigidbody>(); // Tag convention: first 3 letters are the joints finger e.g. idx, next 3 letters are the joints position e.g. Mid with captical first letter (M)
        hj_idx.connectedBody = GameObject.Find("Idx_Prox").GetComponent<Rigidbody>(); 

        // Normally do this part, but for now we are randomly assigning these joint angles in the "GraspSimulator.cs" script. 
        //// Alternative to the above ^ part. Add HingeJointCuve instead of HingeJointScript. This version here doesn't need a target 
        //idxHJCurve = gameObject.AddComponent<HingeJointCurve>();
        //idxHJCurve.ThisHinge = hj_idx; // This hinge joint
        //idxHJCurve.MaxDist = 180f;
        //idxHJCurve.SpeedMultiplier = 1.0f;
        //idxHJCurve.invert = true;
    }

    void PrepRingFinger()
    {
        // Index joint 
        // 1st joint step (maybe later convert this to a loop with joint arrays)
        // Create first joint and configure as stated above with axes and location/anchor
        hj_rng = gameObject.AddComponent<HingeJoint>();
        hj_rng.anchor = rngJointAnchor;
        hj_rng.axis = rngJointAx;

        // Also configure joint with spring and limits (the "on" and then "off" step below is related to a Unity bug and is necessary)
        JointSpring spring = hj_rng.spring;
        spring.spring = 50f;
        spring.damper = 0.5f;
        spring.targetPosition = 0f;
        hj_rng.spring = spring;
        hj_rng.useSpring = true;

        // Unity bug related step (on/off limits) 
        JointLimits limits = hj_rng.limits;
        limits.min = -180f;
        limits.max = 180f;
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        hj_rng.limits = limits;

        hj_rng.useLimits = true;
        hj_rng.useLimits = false;
        hj_rng.useLimits = true;

        // Connect to affected gameobject (i.e. next joint down the hierarchy aka the joint that this hinge spring will control)
        // hj_rng.connectedBody = GameObject.FindGameObjectWithTag("rngPrx").GetComponent<Rigidbody>(); // Tag convention: first 3 letters are the joints finger e.g. idx, next 3 letters are the joints position e.g. Mid with captical first letter (M)
        hj_rng.connectedBody = GameObject.Find("Rng_Prox").GetComponent<Rigidbody>();
        
        // Normally do this part, but for now we are randomly assigning these joint angles in the "GraspSimulator.cs" script. 
        //// Alternative to the above ^ part. Add HingeJointCuve instead of HingeJointScript. This version here doesn't need a target 
        //rngHJCurve = gameObject.AddComponent<HingeJointCurve>();
        //rngHJCurve.ThisHinge = hj_rng; // This hinge joint
        //rngHJCurve.MaxDist = 180f;
        //rngHJCurve.SpeedMultiplier = 1.0f;
        //rngHJCurve.invert = true;
    }

    void PrepThumbFinger()
    {
        // Index joint 
        // 1st joint step (maybe later convert this to a loop with joint arrays)
        // Create first joint and configure as stated above with axes and location/anchor
        hj_tmb = gameObject.AddComponent<HingeJoint>();
        hj_tmb.anchor = tmbJointAnchor;
        hj_tmb.axis = tmbJointAx;

        // Also configure joint with spring and limits (the "on" and then "off" step below is related to a Unity bug and is necessary)
        JointSpring spring = hj_tmb.spring;
        spring.spring = 50f;
        spring.damper = 0.5f;
        spring.targetPosition = 0f;
        hj_tmb.spring = spring;
        hj_tmb.useSpring = true;

        // Unity bug related step (on/off limits) 
        JointLimits limits = hj_tmb.limits;
        limits.min = -180f;
        limits.max = 180f;
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        hj_tmb.limits = limits;

        hj_tmb.useLimits = true;
        hj_tmb.useLimits = false;
        hj_tmb.useLimits = true;

        // Connect to affected gameobject (i.e. next joint down the hierarchy aka the joint that this hinge spring will control)
        // hj_tmb.connectedBody = GameObject.FindGameObjectWithTag("tmbPrx").GetComponent<Rigidbody>(); // Tag convention: first 3 letters are the joints finger e.g. idx, next 3 letters are the joints position e.g. Mid with captical first letter (M)
        hj_tmb.connectedBody = GameObject.Find("Tmb_Prox_Mid").GetComponent<Rigidbody>(); // Tag convention: first 3 letters are the joints finger e.g. idx, next 3 letters are the joints position e.g. Mid with captical first letter (M)


        //(Depricated)
        //// Now add the "HingeJointScript" and assign this hinge joint and the target object (find by Tag) to it
        //tmbHJScript = gameObject.AddComponent<HingeJointScript>();
        //tmbHJScript.ThisHinge = hj_tmb; // This hinge joint
        //tmbHJScript.target = GameObject.FindGameObjectWithTag("tmbPrxT").GetComponent<Transform>(); // Same Tag convention as above, but with the added captical (T) for target. 

        // (Depricated)
        //// (Depricated, now use manual setting by programmer) Use the above Predetermined axis variable to work out which axis to set to true on the "HingeJointScript"
        //tmbHJScript.x = true;
        //tmbHJScript.y = false;
        //tmbHJScript.z = false;
        //tmbHJScript.invert = false;

        // Alternative to the above ^ part. Add HingeJointCuve instead of HingeJointScript. This version here doesn't need a target 
        tmbHJCurve = gameObject.AddComponent<HingeJointCurve>();
        tmbHJCurve.ThisHinge = hj_tmb; // This hinge joint
        tmbHJCurve.MaxDist = 140f;
        tmbHJCurve.SpeedMultiplier = speedMultiplier;
        tmbHJCurve.invert = true; 

    }

}
