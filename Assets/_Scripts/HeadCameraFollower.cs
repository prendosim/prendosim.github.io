using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCameraFollower : MonoBehaviour
{

    [SerializeField] Transform MainCam;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MainCam.position;
        transform.rotation = MainCam.rotation; 
    }
}
