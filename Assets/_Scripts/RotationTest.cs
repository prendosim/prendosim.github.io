using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    Transform targetDir; 
    public Vector3 ObjectTipDirection;

    Quaternion rotation;

    void Start()
    {
        targetDir = GameObject.FindWithTag("CameraParent").transform; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Another quaternion method of the above
            //Quaternion rotation = Quaternion.FromToRotation(transform.up, targetDir.forward);
            if (ObjectTipDirection.x == 1)
            {
                Vector3 direction = transform.position - targetDir.position;
                Vector3 upward = -transform.right;
                float angle = Vector3.Angle(direction, upward);
                Debug.Log("Angle: " + angle);

                rotation = Quaternion.FromToRotation(-transform.right, targetDir.forward);
            }
            if (ObjectTipDirection.y == 1)
            {
                Vector3 direction = transform.position - targetDir.position;
                Vector3 upward = transform.up;
                float angle = Vector3.Angle(direction, upward);
                Debug.Log("Angle: " + angle);

                rotation = Quaternion.FromToRotation(transform.up, targetDir.forward);
            }
            if (ObjectTipDirection.y == -1)
            {
                Vector3 direction = transform.position - targetDir.position;
                Vector3 upward = -transform.up;
                float angle = Vector3.Angle(direction, upward);
                Debug.Log("Angle: " + angle);

                rotation = Quaternion.FromToRotation(-transform.up, targetDir.forward);
            }
            if (ObjectTipDirection.z == 1)
            {
                Vector3 direction = transform.position - targetDir.position;
                Vector3 upward = -transform.forward;
                float angle = Vector3.Angle(direction, upward);
                Debug.Log("Angle: " + angle);

                rotation = Quaternion.FromToRotation(-transform.forward, targetDir.forward);
            }

            transform.rotation = rotation * transform.rotation; 
        }


    }
}
