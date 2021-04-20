using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollower : MonoBehaviour
{
    // Class to attach to a collider that enables interaction with world space UI in VR. 
    // Collider follows the users tracked hand position (given an appropriate collider is attached)
    // and changes horizontal slider values based on this movement. 

    float ZedTglobal; 
    float ZedT;
    Vector3 StartPos;
    
    public GameObject child;
    public GameObject parent;

    RectTransform parentRect; // Slide area rect transform
    //RectTransform childRect; // slider handle rect transform

    Vector2 SlideLimits;
    Vector2 GlobalLimits;
    Vector2 ValRange;

    Vector3 coords; // Current coordinates of collider
    Vector3 coordsLoc;
    Vector3 LocalPhysicsPosition; // just read from the mesh / 3d Obj.

    Slider sld;          // Slider parent
    RectTransform sldT;           // Rect of Slider parent
    RectTransform selfRect;
    public GameObject S;        // Slider

    float val;
    float sldPercent;
    //float ValueUnit;     // Amount of movement needed to change value by 1 unit

    // Start is called before the first frame update
    void Start()
    {
        parentRect = parent.GetComponent<RectTransform>(); // Parent's rect transform

        sld = gameObject.GetComponentInParent<Slider>(); // Get Slider component from grandparent
        selfRect = this.GetComponentInParent<RectTransform>();

        val = sld.value;                          // Get current value of slider 
        //Debug.Log(" Slide limits : " + parentRect.rect.xMin.ToString() + " ," + parentRect.rect.xMax.ToString());
        SlideLimits.x = 0f; // parentRect.rect.xMin + transform.localPosition.x + selfRect.rect.width / 2F;
        SlideLimits.y = parentRect.rect.xMax * 2f; // + transform.localPosition.x - selfRect.rect.width / 2F;

        StartPos = transform.localPosition;
        GlobalLimits.x = parentRect.rect.xMin; // + transform.localPosition.x; //+ selfRect.rect.width / 2F;
        GlobalLimits.y = parentRect.rect.xMax; // + transform.localPosition.x; //- selfRect.rect.width / 2F;
        //Debug.Log(" Global limits : " + GlobalLimits.x.ToString() + " ," + GlobalLimits.y.ToString());
        ValRange.x = sld.minValue;
        ValRange.y = sld.maxValue;
        LocalPhysicsPosition = transform.localPosition;
        //Debug.Log(" Slide limits : "+SlideLimits.x.ToString()+" ,"+SlideLimits.y.ToString());
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        coords = other.gameObject.transform.position;
        coordsLoc = parent.transform.InverseTransformPoint(coords);
        coordsLoc.x = Mathf.Clamp(ZedT, SlideLimits.x, SlideLimits.y);
        ZedTglobal = other.gameObject.transform.position.z;  // Get collision's Z coordinates
        ZedT = this.transform.localPosition.z;              // Get Slider handle's current Z coordinates
        //Debug.Log(ZedT.ToString());
    }

    void OnTriggerStay(Collider other)
    {
        // 1. Read Global Z position of the collider (finger)
        ZedTglobal = other.gameObject.transform.position.z;
        // 2. Move / change the global collider position, check if position within slider bounds
        coords = this.transform.position;
        coords[2] = ZedTglobal;
        coordsLoc = transform.parent.InverseTransformPoint(coords);
        coordsLoc.x = Mathf.Clamp(coordsLoc.x, GlobalLimits.x, GlobalLimits.y);
        coordsLoc.y = 0f;
        coordsLoc.z = 0f;
        this.transform.localPosition = coordsLoc;
        // 3. Get local position
        LocalPhysicsPosition = transform.localPosition;
        //  4. Calculate and Change value based on location proportional to slider range
        sldPercent = Mathf.InverseLerp(StartPos.x + GlobalLimits.x, StartPos.x + GlobalLimits.y, LocalPhysicsPosition.x);
        // Actually, use transform point to get global limits and then clamp coords to that. 

        val = Mathf.Lerp(ValRange.x, ValRange.y, sldPercent);
        //Debug.Log(val.ToString());
        sld.value = val;
    }

    public void SliderOn(float v)
    {
        // Slider percentage
        sldPercent = Mathf.InverseLerp(ValRange.x, ValRange.y, v); // Inverse lerp returns percent given value
        // calculate where object should be given slider value
        // Lerp returns actual value given percent
        ZedT = Mathf.Lerp(GlobalLimits.x, GlobalLimits.y, sldPercent);

        //Debug.Log("Seting rigidbody position :   " + ZedT.ToString());
        this.transform.localPosition = new Vector3(ZedT, StartPos.x + this.transform.localPosition.y, this.transform.localPosition.z);

    }
}
