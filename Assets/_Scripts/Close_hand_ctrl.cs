using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class Close_hand_ctrl : MonoBehaviour
{

    Quaternion[] left_finger_origin = new Quaternion[3];
    Quaternion[] right_finger_origin = new Quaternion[3];
    Quaternion[] thumb_origin = new Quaternion[2];

    public GameObject obj;

   
    // hand's handle for each finger

    public GameObject proximal_left_handle;
    GameObject intermediate_left_handle;
    GameObject distal_left_handle;
    public GameObject proximal_right_handle;
    GameObject intermediate_right_handle;
    GameObject distal_right_handle;
    public GameObject intermediate_thumb_handle;
    GameObject distal_thumb_handle;

    // Start is called before the first frame update
    void Start()
    {
        /* 
        * Define handles
        */
        
        // Left finger
        intermediate_left_handle = proximal_left_handle.transform.GetChild(2).gameObject;
        distal_left_handle = intermediate_left_handle.transform.GetChild(2).gameObject;
        // Right finger
        intermediate_right_handle = proximal_right_handle.transform.GetChild(2).gameObject;
        distal_right_handle = intermediate_right_handle.transform.GetChild(2).gameObject;
        // Thumb
        distal_thumb_handle = intermediate_thumb_handle.transform.GetChild(2).gameObject;

        // Set origin configuration

        left_finger_origin[0] = new Quaternion(0.0f, 0.7071f, 0.0f, 0.7071f);
        left_finger_origin[1] = new Quaternion(0.0f, 0.0f, -0.7071f, 0.7071f);
        left_finger_origin[2] = new Quaternion(0.0f, 0.0f, 0.0f, -1.0f);

        right_finger_origin[0] = new Quaternion(0.0f, 0.7071f, 0.0f, 0.7071f);
        right_finger_origin[1] = new Quaternion(0.0f, 0.0f, -0.7071f, 0.7071f);
        right_finger_origin[2] = new Quaternion(0.0f, 0.0f, 0.0f, -1.0f);

        thumb_origin[0] = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        thumb_origin[1] = new Quaternion(0.0f, 0.0f, 0.0f, -1.0f);

        SetOriginConfiguration();

 
    }

   // Update is called once per frame
   void Update()
   {
   }

    public void CloseHand()
    {

        HingeJoint hj;

        //Thumb
        hj = intermediate_thumb_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);

        hj = distal_thumb_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);

        //Left Finger
        hj = intermediate_left_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);

        hj = distal_left_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);

        //Right finger
        hj = intermediate_right_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);

        hj = distal_right_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, -100);


    }


    public void OpenHand()
    {
        HingeJoint hj;

        //Thumb
        hj = intermediate_thumb_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);

        hj = distal_thumb_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);

        //Left Finger
        hj = intermediate_left_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);

        hj = distal_left_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);

        //Right finger
        hj = intermediate_right_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);

        hj = distal_right_handle.GetComponent<HingeJoint>();
        CloseHinge(hj, 100);


    }


    void CloseHinge(HingeJoint hj, float targetVel)
    {
        JointMotor m = new JointMotor();
        m.targetVelocity = targetVel;
        m.force = 1000;
        m.freeSpin = false;
        hj.motor = m;
    }
  

    public void activateObjGravity()
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    public void RandomiseProximal()
    {

        Quaternion angleQuaternion = Quaternion.Euler(0.0f, Random.Range(0f, 90f), 0.0f);

        proximal_left_handle.transform.localRotation = left_finger_origin[0] * angleQuaternion;
        proximal_right_handle.transform.localRotation = right_finger_origin[0] * Quaternion.Inverse(angleQuaternion);
    }




    void SetOriginConfiguration()
    {

        proximal_left_handle.transform.localRotation = left_finger_origin[0];
        intermediate_left_handle.transform.localRotation = left_finger_origin[1];
        distal_left_handle.transform.localRotation = left_finger_origin[2];

        proximal_right_handle.transform.localRotation = right_finger_origin[0];
        intermediate_right_handle.transform.localRotation = right_finger_origin[1];
        distal_right_handle.transform.localRotation = right_finger_origin[2];

        intermediate_thumb_handle.transform.localRotation = thumb_origin[0];
        distal_thumb_handle.transform.localRotation = thumb_origin[1];
    }

}
