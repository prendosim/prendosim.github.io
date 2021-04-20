using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopier : MonoBehaviour
{
    public Transform MainObject; 

    void Update()
    {
        transform.position = MainObject.position;
        transform.rotation= MainObject.rotation;
    }
}
