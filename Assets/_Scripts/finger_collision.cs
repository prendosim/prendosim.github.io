using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finger_collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start finger_collision");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {

        print(this.name + " collids with " + collision.gameObject.name);

        if (collision.gameObject.name == "scissors_up_y")
        {

            HingeJoint hj = this.GetComponent<HingeJoint>();
            StopMotor(hj);

            if (this.name == "dist_left_link" || this.name == "dist_right_link")
            {
                GameObject med_link = this.transform.parent.gameObject;
                GameObject prox_link = med_link.transform.parent.gameObject;

                hj = med_link.GetComponent<HingeJoint>();
                StopMotor(hj);
                hj = prox_link.GetComponent<HingeJoint>();
                StopMotor(hj);
            }

            if (this.name == "med_left_link" || this.name == "med_right_link" || this.name == "dist_thumb_link")
            {
                GameObject prox_link = this.transform.parent.gameObject;

                hj = prox_link.GetComponent<HingeJoint>();
                StopMotor(hj);
            }
        }
    }


    void StopMotor(HingeJoint hj)
    {
        JointMotor m = new JointMotor();
        m.targetVelocity = 0;
        m.force = 500;
        m.freeSpin = false;
        hj.motor = m;
    }

}
