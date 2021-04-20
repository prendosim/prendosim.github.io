using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DataClass
{
    // Length of the float is the trial duration (15s) divided by the frame time step (1/60th) plus a few extra frames for safety
    [SerializeField] public string conditionInfo = string.Empty;

    [SerializeField] public string[] headPose = new string[1001];
    [SerializeField] public string[] jointData = new string[1001];
    [SerializeField] public string[] objectPose = new string[1001];
    [SerializeField] public string[] categoricalDir = new string[1001];
}
public class GripperDataClass
{
    // Length of the float is the trial duration (15s) divided by the frame time step (1/60th) plus a few extra frames for safety
    [SerializeField] public string conditionInfo = string.Empty;

    [SerializeField] public string headPose;
    [SerializeField] public string gripperData;
    [SerializeField] public string objectPose;
    [SerializeField] public string categoricalDir;
}
[System.Serializable]
public enum MyEnum
{
    scissors, screwdriver, hammer
};

public class GraspSimulator : MonoBehaviour
{
    //public MyEnum usedObjects;
    public bool[] objBool = new bool[3];
    bool drySimTest;
    private DataClass expData = new DataClass();
    private GripperDataClass gripData = new GripperDataClass();

    public Transform CameraTransform;

    #region Variables
    public StateObject roboState;

    // Instead of animators, we should use coroutines. 
    // This will avoid a lot of the rotation issues I 
    // have been experiencing, which is a result of quaternion 
    //to Euler conversion done under the hood by unity between 
    // inspector values (set in the animation) and the actual Euler angles of the objects

    public GameObject Roots;
    public GameObject Parallel;
    //public ParallelGripperScript parallelAnimator; 
    //public Transform[] parallelAnchors; 
    public ParallelGripperScript LeftparallalScript;
    public ParallelGripperScript RightparallalScript;
    public GameObject LeftGripper;
    public GameObject RightGripper;

    public GameObject BarrettHand;
    public Transform Gripper;
    public GameObject[] TargetObjects;
    public Vector3 ObjectOffset;
    public SkinnedMeshRenderer handMesh;

    GameObject targObj_prefab;
    HingeJoint[] proxJoints = new HingeJoint[2];

    public GameObject[] AllJoints;

    float trialStartTime = 0f;
    float trialEndTiem = 10f;

    public int loopCounter = 0;
    public int NumberOfTrials = 3;
    public TMP_InputField InpTrials;
    public TMP_InputField InpPath;
    public TMP_Text SimCounter;
    public UnityEngine.UI.Slider FrameRateSlider;
    public TMP_Text FPSMonitor;
    public UnityEngine.UI.Slider PhysicsSlider;
    public TMP_Text PhysicsLoops;
    public GameObject PanelCurtain;
    public TMP_Text GripperName;
    public TMP_Text SamplingRateDisplay;
    public Toggle tog_takeImages, tog_scissors, tog_screwdriver, tog_hammer;

    Coroutine co;

    HingeJointCurve[] allHjCurves;
    string trialName;

    List<string> headInfo = new List<string>();
    List<string> jointInfo = new List<string>();
    List<string> objectInfo = new List<string>();
    List<string> categoricalInfo = new List<string>();

    string paths;
    string categoricalOrientation;

    Transform cameraDirection;
    public Vector3 ObjectTipDirection;
    Quaternion rotation;
    bool inSimulation = false;
    bool animatingParallel = false;
    int imageResolutionFactor = 8;
    bool userFrienly;
    bool handClosed, handOpened; 
    #endregion

    void Start()
    {
        //parallelAnimator = Parallel.GetComponent<ParallelGripperScript>(); 
        LeftparallalScript = LeftGripper.GetComponent<ParallelGripperScript>();
        RightparallalScript = RightGripper.GetComponent<ParallelGripperScript>();

        PanelCurtain.SetActive(false);

        //paths = Application.dataPath + "/Resources/";
        proxJoints = Roots.GetComponents<HingeJoint>();
        allHjCurves = gameObject.GetComponentsInChildren<HingeJointCurve>();

        cameraDirection = GameObject.FindWithTag("CameraParent").transform;
    }

    void Update()
    {
        objBool[0] = tog_scissors.isOn;
        objBool[1] = tog_screwdriver.isOn;
        objBool[2] = tog_hammer.isOn;

        //if(roboState.SpawnObjects)
        //{
        //    RandomObjectSpawner();
        //    Invoke("ActivateGravity", 1f);
        //    roboState.SpawnObjects = false; 
        //}

        if (inSimulation)
        {
            int currentTrialz = loopCounter + 1;
            SimCounter.text = "Trial #: " + currentTrialz.ToString();
        }

        float fpsComp = 1f / FrameRateSlider.value;
        FPSMonitor.text = "Set FPS: " + FrameRateSlider.value.ToString("F0");
        SamplingRateDisplay.text = "Measured FPS: " + (1f / Time.fixedDeltaTime).ToString("F0");
        Time.fixedDeltaTime = fpsComp;

        Physics.defaultSolverIterations = int.Parse(PhysicsSlider.value.ToString("F0"));
        PhysicsLoops.text = "Solver Iterations: " + PhysicsSlider.value.ToString("F0");

        try
        {
            NumberOfTrials = int.Parse(InpTrials.text);
            paths = InpPath.text + "/";
        }
        catch
        { }

        if (loopCounter >= NumberOfTrials)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
            StopAllCoroutines();
        }
    }

    #region Button functions
    public void StartSimulation()
    {
        loopCounter = 0;
        if (InpPath.text == string.Empty)
        {
            Debug.Log("Path empty!!!");
            InpPath.text = Application.persistentDataPath;
            Debug.Log("Default path: " + InpPath.text);
        }

        if (BarrettHand.activeInHierarchy)
        {
            //co = StartCoroutine("LoopSimulation");
            co = StartCoroutine(BarrettSimulation());
        }
        else if (Parallel.activeInHierarchy)
        {
            co = StartCoroutine(ParallelSimulation());
            animatingParallel = true;
        }
        inSimulation = true;
    }
    public void TestSimulation()
    {
        // drySimTest = true;
        if (BarrettHand.activeInHierarchy)
            co = StartCoroutine(BarrettSimulation());
        else if (Parallel.activeInHierarchy)
            co = StartCoroutine(ParallelSimulation());
    }
    public void StopSimulation()
    {
        StopAllCoroutines();
        inSimulation = false;
    }
    public void QuitApplication()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void MaxVisibility()
    {
        /* This function will:  
         * 1. Clear the scene
         * 2. Load each image 
         * 3. Take screen-shots from the prescribed angle(s) 
         * 4. Indicate the task is complete
        */
        StartCoroutine(MaxVisiRoutine());
    }
    public void ChangeRobotGripper()
    {
        /* This function will:  
        > Change which robo gripper is used
        */
        if (BarrettHand.activeInHierarchy)
        {
            BarrettHand.SetActive(false);
            Parallel.SetActive(true);
            GripperName.text = "Barrett Hand";
        }
        else if (Parallel.activeInHierarchy)
        {
            Parallel.SetActive(false);
            BarrettHand.SetActive(true);
            GripperName.text = "Jaw Gripper";
        }

        loopCounter = 0;
    }
    IEnumerator MaxVisiRoutine()
    {

        // 1. Clear the scene
        if (handMesh != null)
        { handMesh.enabled = false; }
        //PanelCurtain.SetActive(true);

        // 2. Load each image
        foreach (GameObject tObj in TargetObjects)
        {
            GameObject tobject = Instantiate(tObj);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(TakeImages(paths + tObj.name + "_maxVis_"));

            SimCounter.text = "Max Visibility of: " + "\n" + tObj.name + " taken.";
            yield return new WaitForSeconds(1.5f);
            Destroy(tobject);
            yield return new WaitForSeconds(0.5f);
        }

        if (handMesh != null)
        { handMesh.enabled = true; }
        //PanelCurtain.SetActive(false);

        yield return null;
    }
    #endregion

    void SaveData(string filepath, DataClass expInfo)
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string jsonString = JsonConvert.SerializeObject(expInfo, Formatting.Indented);

        File.WriteAllText(filepath + "_" + timeStamp + ".json", jsonString);
    }
    void SaveData(string filepath, GripperDataClass expInfo)
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string jsonString = JsonConvert.SerializeObject(expInfo, Formatting.Indented);

        File.WriteAllText(filepath + "_" + timeStamp + ".json", jsonString);
    }
    void RecordGripperData(string categoryDir)
    {
        gripData.headPose = "View" + "," +
                      CameraTransform.position.x.ToString() + "," + CameraTransform.position.y.ToString() + "," + CameraTransform.position.z.ToString() + "," +
                      CameraTransform.rotation.x.ToString() + "," + CameraTransform.rotation.y.ToString() + "," + CameraTransform.rotation.z.ToString() + "," + CameraTransform.rotation.w.ToString();
        gripData.gripperData = "LeftGripperPos: " + LeftGripper.transform.localPosition.ToString("F3") + "," +
                               "RightGripperPos: " + RightGripper.transform.localPosition.ToString("F3");

        Rigidbody rb = targObj_prefab.GetComponentInChildren<Rigidbody>();
        gripData.objectPose = targObj_prefab.name + "," +
                                "ObjectPos: " + targObj_prefab.transform.position.ToString("F3") + "," +
                                "ObjectRot: " + targObj_prefab.transform.rotation.eulerAngles.ToString("F3") + "," +
                                "ObjectMass: " + rb.mass;
        gripData.categoricalDir = categoryDir;
    }
    void RecordBarrettData(string categoryDir)
    {
        headInfo.Add
                        (
                            "View" + "," +
                            CameraTransform.position.x.ToString() + "," + CameraTransform.position.y.ToString() + "," + CameraTransform.position.z.ToString() + "," +
                            CameraTransform.rotation.x.ToString() + "," + CameraTransform.rotation.y.ToString() + "," + CameraTransform.rotation.z.ToString() + "," + CameraTransform.rotation.w.ToString()
                        );

        for (int i = 0; i < AllJoints.Length; i++)
        {
            jointInfo.Add
           (
               AllJoints[i].gameObject.name + "," +
               AllJoints[i].gameObject.transform.position.x.ToString() + "," + AllJoints[i].gameObject.transform.position.y.ToString() + "," + AllJoints[i].gameObject.transform.position.z.ToString() + "," +
               AllJoints[i].gameObject.transform.localPosition.x.ToString() + "," + AllJoints[i].gameObject.transform.localPosition.y.ToString() + "," + AllJoints[i].gameObject.transform.localPosition.z.ToString() + "," +
               AllJoints[i].gameObject.transform.rotation.x.ToString() + "," + AllJoints[i].gameObject.transform.rotation.y.ToString() + "," + AllJoints[i].gameObject.transform.rotation.z.ToString() + "," + AllJoints[i].gameObject.transform.rotation.w.ToString() + "," +
               AllJoints[i].gameObject.transform.localRotation.x.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.y.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.z.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.w.ToString()
           );
        }

        Rigidbody rb = targObj_prefab.GetComponentInChildren<Rigidbody>();
        objectInfo.Add
            (
                targObj_prefab.name + "," +
                targObj_prefab.transform.position.x.ToString() + "," + targObj_prefab.transform.position.y.ToString() + "," + targObj_prefab.transform.position.z.ToString() + "," +
                targObj_prefab.transform.rotation.x.ToString() + "," + targObj_prefab.transform.rotation.y.ToString() + "," + targObj_prefab.transform.rotation.z.ToString() + "," + targObj_prefab.transform.rotation.w.ToString() + "," +
                rb.mass
            );

        categoricalInfo.Add
            (
                categoryDir + "," +
                rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString() + "," + rotation.w.ToString()
            );

        expData.headPose = headInfo.ToArray();
        expData.jointData = jointInfo.ToArray();
        expData.objectPose = objectInfo.ToArray();
        expData.categoricalDir = categoricalInfo.ToArray();

        SaveData(paths + loopCounter + "_barrettHand", expData);

        headInfo.Clear();
        jointInfo.Clear();
        objectInfo.Clear();
        categoricalInfo.Clear();

    }

    IEnumerator ParallelSimulation()
    {
        trialStartTime = Time.time;

        // Spawn object
        RandomObjectSpawner();
        yield return new WaitForSeconds(0.25f);

        // Close gripper 
        roboState.pGripperClose = true;
        yield return new WaitForSeconds(1f);

        // Take screenshots final
        StartCoroutine(TakeImages(paths + loopCounter.ToString() + "_jaw_image"));
        yield return new WaitForSeconds(0.5f);

        // Activate gravity <- Replace by increase mass 
        // StartCoroutine(IncreaseMass()); // ActivateGravity();
        // yield return new WaitForSeconds(1.75f);

        while (roboState.inContact)
            yield return null;

        // Record and save data
        // Take note of categorical orientation from viewers perspective (main camera) 
        string categoricaldir = CategoricalDirections();
        RecordGripperData(categoricaldir);

        //Save Data; 
        SaveData(paths + loopCounter + "_jawGripper", gripData);

        // Wait for the target object to be destroyed
        while (targObj_prefab != null)
            yield return null;
        yield return new WaitForSeconds(0.25f);

        // Open gripper 
        roboState.pGripperOpen = true;
        yield return new WaitForSeconds(1f);

        loopCounter++;
        if (loopCounter < NumberOfTrials)
        {
            StartCoroutine(ParallelSimulation());
        }
        else if (loopCounter >= NumberOfTrials)
        {
            loopCounter = 0;
        }

        animatingParallel = false;
        yield return null;
    }
    IEnumerator BarrettSimulation()
    {
        trialStartTime = Time.time;

        // Spawn object
        RandomObjectSpawner();
        yield return new WaitForSeconds(0.1f);

        // Reset values for next trial 
        roboState.fingerTipContacts[0] = 0;
        roboState.fingerTipContacts[1] = 0;
        roboState.fingerTipContacts[2] = 0;

        // Close Barrett Hand  
        StartCoroutine(BarrettClose());

        // Check when fingertips contact object
        float sTime = Time.time; 
        float eTime = 0f; 
        float maxTime = 1.5f; 
        while((roboState.fingerTipContacts[0] + roboState.fingerTipContacts[1] + roboState.fingerTipContacts[2]) != 3 | eTime < maxTime)
        {   
            eTime+=Time.time - sTime; 
            // Debug.Log("ETime: " + eTime);
            yield return null; 
        }
        // while(!roboState.handClosed)
        //     yield return null; 
        yield return new WaitForSeconds(1f);

        roboState.activateGravityMass = true; 
        Debug.Log("Gravity activated!!!");

        // Increase mass -> Formerly: Activate gravity 
        // ActivateGravity();
        // StartCoroutine(IncreaseMass());
        float startTime = Time.time;
        float startTime2 = Time.time;
        float elapsedTime = 0f; 
        float elapsedTime2 = 0f; 
        bool recordData = false; 

        while (targObj_prefab!=null & !recordData) // elapsedTime < roboState.maxTime // roboState.inContact
        {
            elapsedTime = Time.time - startTime; 
            elapsedTime2 = Time.time - startTime2; 

            // If the object is still in the gripper 60 per cent of the max time, then take it as a stable grasp and take image and record data 
            if(elapsedTime2 > (roboState.maxTime * 0.6f)) // Replace this with: If object moves, then take image. 
            {
                recordData = true;
                elapsedTime2 = 0f; 
                startTime2 = Time.time; 
            }

            if(recordData)
            {
                // Take screenshots final
                StartCoroutine(TakeImages(paths + loopCounter.ToString() + "_barrett_image"));
                // Record and save data
                // Take note of categorical orientation from viewers perspective (main camera) 
                string categoricaldir = CategoricalDirections();
                RecordBarrettData(categoricaldir);

                recordData = false; 
            }
            yield return null;
        }

        // yield return new WaitForSeconds(1f);

        // // Wait for the target object to be destroyed
        // while (targObj_prefab != null)
        //     yield return null;
        // yield return new WaitForSeconds(0.75f);

        // Open gripper 
        StartCoroutine(BarrettOpen());
        yield return new WaitForSeconds(3f);
        // StartCoroutine(HandState(false));
        // while(!HandState(false))
        //     yield return null; 
        // handOpened = false;
        

        loopCounter++;
        if (loopCounter < NumberOfTrials)
        {
            StartCoroutine(BarrettSimulation());
        }
        else if (loopCounter >= NumberOfTrials)
        {
            loopCounter = 0;
        }

        animatingParallel = false;
        yield return null;
    }

    // IEnumerator HandState(bool handClosing)
    // {        
    //     if(handClosing)
    //     {
    //         for (int i = 0; i < allHjCurves.Length; i++)
    //         {
    //             while(allHjCurves[i].closing == true)
    //                 yield return null;
    //         }    
    //         handClosed = true; 
    //     }
    //     else
    //     {
    //         for (int i = 0; i < allHjCurves.Length; i++)
    //         {
    //             while(allHjCurves[i].opening == true)
    //                 yield return null;
    //         }    
    //         handOpened = true; 
    //     }
    // }

    bool HandState(bool handClosing)
    {        
        if(handClosing)
        {
            for (int i = 0; i < allHjCurves.Length; i++)
            {
                if(allHjCurves[i].closing == true)
                    continue;
            }    
            handClosed = true;
            return handClosed; 
        }
        else
        {
            for (int i = 0; i < allHjCurves.Length; i++)
            {
                if(allHjCurves[i].opening == true)
                    continue; 
            }    
            handOpened = true; 
            return handOpened; 
        }
    }


    IEnumerator LoopSimulation()
    {
        trialStartTime = Time.time;

        //float percDone = loopCounter / NumberOfTrials; 
        //SimCounter.text = "Completed: " + percDone.ToString("F2") + "%";

        // Run the grasp simulation to (1) spawn object, (2) close hand, (3) Activate gravity, 
        GraspController(); // This takes 2.5 seconds
        yield return new WaitForSeconds(2.25f);

        // Take images 
        if (tog_takeImages.isOn)
        {
            // Hide the hand 
            handMesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            // Take another screenshot of the object without showing the hand 
            StartCoroutine(TakeImages(paths + trialName + "object_init"));

            yield return new WaitForSeconds(0.1f);
            // Show the hand again 
            handMesh.enabled = true;
        }

        // (4) Save object pose and all digit data 
        string targObjectName = targObj_prefab.name;
        Vector3 targPos = targObj_prefab.transform.position;
        Quaternion targRot = targObj_prefab.transform.rotation;

        // Categorical orientation from viewers perspective (main camera) 
        string categoricaldir = CategoricalDirections();

        // Take the final image of the scene from the front view?
        if (tog_takeImages.isOn)
        {
            StartCoroutine(TakeImages(paths + trialName + "final_"));
            yield return new WaitForSeconds(1f);
        }

        // File header
        jointInfo.Add
            (
                "Object, ObjectPosX, ObjectPosY ,ObjectPosZ, ObjectRotX, ObjectRotY, ObjectRotZ, ObjectRotW,  " +
                "Digit, DigitPosX, DigitPosY, DigitPosZ, DigitLocPosX, DigitLocPosY, DigitLocPosZ, " +
                "DigitRotX, DigitRotY, DigitRotZ, DigitRotW, DigitLocRotX, DigitLocRotY, DigitLocRotZ, DigitLocRotW, " +
                "Category, Object2FacingRotTformX, Object2FacingRotTformY, Object2FacingRotTformZ, Object2FacingRotTformW"
            );

        // Record joint pose data 
        for (int i = 0; i < AllJoints.Length; i++)
        {
            headInfo.Add
                (
                    "View" + "," +
                    CameraTransform.position.x.ToString() + "," + CameraTransform.position.y.ToString() + "," + CameraTransform.position.z.ToString() + "," +
                    CameraTransform.rotation.x.ToString() + "," + CameraTransform.rotation.y.ToString() + "," + CameraTransform.rotation.z.ToString() + "," + CameraTransform.rotation.w.ToString()
                );

            jointInfo.Add
               (
                   AllJoints[i].gameObject.name + "," +
                   AllJoints[i].gameObject.transform.position.x.ToString() + "," + AllJoints[i].gameObject.transform.position.y.ToString() + "," + AllJoints[i].gameObject.transform.position.z.ToString() + "," +
                   AllJoints[i].gameObject.transform.localPosition.x.ToString() + "," + AllJoints[i].gameObject.transform.localPosition.y.ToString() + "," + AllJoints[i].gameObject.transform.localPosition.z.ToString() + "," +
                   AllJoints[i].gameObject.transform.rotation.x.ToString() + "," + AllJoints[i].gameObject.transform.rotation.y.ToString() + "," + AllJoints[i].gameObject.transform.rotation.z.ToString() + "," + AllJoints[i].gameObject.transform.rotation.w.ToString() + "," +
                   AllJoints[i].gameObject.transform.localRotation.x.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.y.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.z.ToString() + "," + AllJoints[i].gameObject.transform.localRotation.w.ToString()
               );

            objectInfo.Add
                (
                    targObjectName + "," +
                    targPos.x.ToString() + "," + targPos.y.ToString() + "," + targPos.z.ToString() + "," +
                    targRot.x.ToString() + "," + targRot.y.ToString() + "," + targRot.z.ToString() + "," + targRot.w.ToString()
                );

            categoricalInfo.Add
                (
                    categoricaldir + "," +
                    rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString() + "," + rotation.w.ToString()
                );

        }

        #region Save data to file
        // Save list joint info and target object pose from above
        expData.headPose = headInfo.ToArray();
        expData.jointData = jointInfo.ToArray();
        expData.objectPose = objectInfo.ToArray();
        expData.categoricalDir = categoricalInfo.ToArray();
        string jsonString = JsonConvert.SerializeObject(expData, Formatting.Indented);
        //File.WriteAllLines(paths + trialName + ".txt", jsonString);
        File.WriteAllText(paths + trialName + ".json", jsonString);

        headInfo.Clear();
        jointInfo.Clear();
        objectInfo.Clear();
        categoricalInfo.Clear();
        #endregion

        yield return new WaitForSeconds(1f); // We give the system an extra second (3+1 = 4) to allow for the target object to stabilize in the hand 

        // Open the hand to allow for the object to fall in case of a stable grasp and to reset the hand for the next trial 
        if (BarrettHand.activeInHierarchy)
        {
            for (int i = 0; i < allHjCurves.Length; i++)
            {
                allHjCurves[i].OpenHandEvent = true;
            }
        }
        else
        {
            roboState.pGripperOpen = true;
        }

        yield return new WaitForSeconds(1.25f);

        loopCounter++;
        if (loopCounter <= NumberOfTrials)
        {
            StartCoroutine(LoopSimulation());
        }
        else if (loopCounter > NumberOfTrials)
        {
            loopCounter = 0;
        }
    }

    IEnumerator BarrettClose()
    {
        // Create random index and ring finger prox joint angles and assign them to the joints 
        float IndexRingProx_rand = Random.Range(0f, -180f);
        // Apply the same angles to each, index and ring digit proximal joints rather than two separate angles 
        if (BarrettHand.activeInHierarchy)
        {
            HingeMover(proxJoints[0], IndexRingProx_rand, false);
            HingeMover(proxJoints[1], IndexRingProx_rand, false);
        }
        for (int i = 0; i < allHjCurves.Length; i++)
        {
            allHjCurves[i].CloseHandEvent = true;
            yield return null;
        }
    }
    IEnumerator BarrettOpen()
    {
        for (int i = 0; i < allHjCurves.Length; i++)
        {
            allHjCurves[i].OpenHandEvent = true;
            yield return null;
        }
    }

    // Spawns objects and commands the gripper to close to test whether the grip is stable
    void GraspController()
    {

        // Create random index and ring finger prox joint angles and assign them to the joints 
        float IndexRingProx_rand = Random.Range(0f, -180f);
        //float RingProx_rand = Random.Range(0f, -180f);

        // Apply the same angles to each, index and ring digit proximal joints rather than two separate angles 
        if (BarrettHand.activeInHierarchy)
        {
            HingeMover(proxJoints[0], IndexRingProx_rand, false);
            HingeMover(proxJoints[1], IndexRingProx_rand, false);
        }

        RandomObjectSpawner();

        // Function to ensure increased grasp-ability for the screwdriver and scissors (implemented on: 21st Oct 2020)
        //MakeMoreGraspable(); 

        // Run coroutine to close the hand (takes 1 second)
        //StartCoroutine(CloseSequence()); // This coroutine needs to take care of all the digits. 
        //HingeJointCurve.OpenHandEvent = true;

        if (BarrettHand.activeInHierarchy)
        {
            for (int i = 0; i < allHjCurves.Length; i++)
            {
                allHjCurves[i].CloseHandEvent = true;
            }
        }
        else
        {
            roboState.pGripperClose = true;
        }

        // (4) Save initial target object pose after it is being grasped 
        trialName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + targObj_prefab.name + "_" + loopCounter.ToString(); // Name the current trial 

        string inittargObjectName = targObj_prefab.name;
        Vector3 inittargPos = targObj_prefab.transform.position;
        Quaternion inittargRot = targObj_prefab.transform.rotation;

        // Save list joint info and target object pose from above
        string initObjectState = "Object, ObjectPosX, ObjectPosY ,ObjectPosZ, ObjectRotX, ObjectRotY, ObjectRotZ, ObjectRotW" + "\n" +
                                 inittargObjectName + "," + inittargPos.x.ToString() + "," + inittargPos.y.ToString() + "," + inittargPos.z.ToString() + "," +
                                 inittargRot.x.ToString() + "," + inittargRot.y.ToString() + "," + inittargRot.z.ToString() + "," + inittargRot.w.ToString();

        File.WriteAllText(paths + trialName + "_init" + ".txt", initObjectState);
        // Take a screenshot of the hand holding the object
        //PanelCurtain.SetActive(true);
        StartCoroutine(TakeImages(paths + trialName + "_init"));

        // After 2 seconds (time for animation to complete), activate target object gravity to see if the grasp holds 
        // (if the objects falls to the table then the grasp is not successful) 
        Invoke("ActivateGravity", 1.25f);
    }

    IEnumerator TakeImages(string pathfile)
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        if (tog_takeImages.isOn)
        {
            PanelCurtain.SetActive(true);
            ScreenCapture.CaptureScreenshot(pathfile + "_" + timeStamp + ".png", imageResolutionFactor);
            yield return new WaitForSeconds(0.5f);
            PanelCurtain.SetActive(false);
        }
    }

    public int SumArray(int[] toBeSummed)
    {
        int sum = 0;
        foreach (int item in toBeSummed)
        {
            sum += item;
        }
        return sum;
    }

    void RandomObjectSpawner()
    {
        int objectIndex = -1;

        // Spawn random object at a random orientation but set to around the position of the robot gripper 
        int[] activeIndex = new int[TargetObjects.Length];
        for (int indx = 0; indx < activeIndex.Length; indx++)
        {
            if (objBool[indx] == true)
                activeIndex[indx] = 1;
            else
                activeIndex[indx] = 0;
        }

        int numGameObjs = SumArray(activeIndex);
        List<GameObject> tempSpawn = new List<GameObject>();

        for (int indx = 0; indx < activeIndex.Length; indx++)
        {
            if (activeIndex[indx] == 1)
                tempSpawn.Add(TargetObjects[indx]);
        }

        GameObject[] tempSpawn2 = new GameObject[numGameObjs];
        tempSpawn2 = tempSpawn.ToArray();

        objectIndex = Random.Range(0, tempSpawn2.Length);
        Quaternion randRotation = Quaternion.identity;
        if(roboState.userFriendly)
        {
            randRotation = Quaternion.Euler(Random.Range(0f, 15f), Random.Range(75f, 135f), Random.Range(10f, 100f)); // This part is to make sure the objects rotate towards the viewer
        }
        else
        {
            randRotation = Random.rotation;
            // randRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
        targObj_prefab = Instantiate(tempSpawn2[objectIndex], Gripper.position + ObjectOffset, randRotation);
    }
    void ActivateGravity()
    {
        Rigidbody rb = targObj_prefab.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.useGravity = true;
    }

    // IEnumerator IncreaseMass()
    // {
    //     Rigidbody rb = targObj_prefab.GetComponent<Rigidbody>();
    //     rb.mass = 0f;
    //     rb.isKinematic = false;
    //     rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    //     rb.useGravity = true;

    //     // float startTime = Time.time; 
    //     // float elapsedTime = 0f; 

    //     // Increase mass by a certain step size ? 
    //     while (roboState.inContact) // | elapsedTime<roboState.maxTime
    //     {
    //         if (rb != null)
    //         {
    //             txt_Mass.text = "Mass: " + rb.mass.ToString("F1") + "g";
    //             rb.mass += 0.01f;
    //             // elapsedTime = Time.time-startTime; 
    //             // Debug.Log("Time: " + elapsedTime);
    //         }
    //         yield return null;
    //     }
    // }

    void ActivateGravity(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.useGravity = true;
        //PanelCurtain.SetActive(false);
    }
    void HingeMover(HingeJoint ThisHinge, float angle, bool invert)
    {
        if (ThisHinge != null)
        {
            JointSpring js;
            js = ThisHinge.spring;
            js.targetPosition = angle;

            if (js.targetPosition > 180)
                js.targetPosition = js.targetPosition - 360;
            if (invert)
                js.targetPosition = js.targetPosition * -1;

            js.targetPosition = Mathf.Clamp(js.targetPosition, ThisHinge.limits.min + 5, ThisHinge.limits.max - 5);

            ThisHinge.spring = js;
        }

    }
    string CategoricalDirections()
    {
        string categoricalDir = "";

        float angle = 0f;
        bool execute = true;

        ObjectTipDirection = targObj_prefab.GetComponent<RotationTest>().ObjectTipDirection;
        //Debug.Log(ObjectTipDirection); 

        // Another quaternion method of the above
        //Quaternion rotation = Quaternion.FromToRotation(transform.up, cameraDirection.forward);
        if (ObjectTipDirection.x == 1 & execute)
        {
            Vector3 direction = targObj_prefab.transform.position - cameraDirection.position;
            Vector3 upward = -targObj_prefab.transform.right;
            angle = Vector3.Angle(direction, upward);
            //Debug.Log("Angle: " + angle);

            rotation = Quaternion.FromToRotation(-targObj_prefab.transform.right, cameraDirection.forward);
            execute = false;
        }
        if (ObjectTipDirection.y == 1 & execute)
        {
            Vector3 direction = targObj_prefab.transform.position - cameraDirection.position;
            Vector3 upward = targObj_prefab.transform.up;
            angle = Vector3.Angle(direction, upward);
            //Debug.Log("Angle: " + angle);

            rotation = Quaternion.FromToRotation(targObj_prefab.transform.up, cameraDirection.forward);
            execute = false;
        }
        if (ObjectTipDirection.y == -1 & execute)
        {
            Vector3 direction = targObj_prefab.transform.position - cameraDirection.position;
            Vector3 upward = -targObj_prefab.transform.up;
            angle = Vector3.Angle(direction, upward);
            //Debug.Log("Angle: " + angle);

            rotation = Quaternion.FromToRotation(-targObj_prefab.transform.up, cameraDirection.forward);
            execute = false;
        }
        if (ObjectTipDirection.z == 1 & execute)
        {
            Vector3 direction = targObj_prefab.transform.position - cameraDirection.position;
            Vector3 upward = -targObj_prefab.transform.forward;
            angle = Vector3.Angle(direction, upward);
            //Debug.Log("Angle: " + angle);

            rotation = Quaternion.FromToRotation(-targObj_prefab.transform.forward, cameraDirection.forward);
            execute = false;
        }

        if (angle <= 45f)
        {
            //Debug.Log("Pointing toward!!!");
            categoricalDir = "facing";
        }
        if (angle > 45f & angle <= 135f)
        {
            //Debug.Log("Pointing sideways!");
            categoricalDir = "sideways";
        }
        if (angle > 135f)
        {
            //Debug.Log("Pointing away!");
            categoricalDir = "away";
        }

        return categoricalDir;
        //transform.rotation = rotation * transform.rotation;
    }
}


//private void MakeMoreGraspable()
//{
//    if(targObj_prefab.name.Contains("scissor") | targObj_prefab.name.Contains("screwd"))
//    {
//        Collider objCol = targObj_prefab.GetComponent<Collider>();
//        objCol.material.staticFriction = 50f;
//    }
//}
//// This is saving the proximal joint angle and position of the index and ring digits 
//jointInfo.Add(IndexProxJoint.gameObject.name + "\t" +
//              IndexProxJoint.transform.position.ToString() + "\t" +
//              IndexProxJoint.transform.position.ToString() + "\t" +
//              IndexProxJoint.transform.rotation.ToString() + "\t" +
//              IndexProxJoint.transform.localRotation.ToString() + "\n" +
//              RingProxJoint.gameObject.name + "\t" +
//              RingProxJoint.transform.position.ToString() + "\t" +
//              RingProxJoint.transform.position.ToString() + "\t" +
//              RingProxJoint.transform.rotation.ToString() + "\t" +
//              RingProxJoint.transform.localRotation.ToString() + "\t" +
//              rotation.ToString() + "\t" + 
//              categoricaldir
//              );


//for (int i = 0; i < allHjCurves.Length; i++)
//{
//    jointInfo.Add
//        (
//            targObjectName + "," +
//            targPos.x.ToString() + "," + targPos.y.ToString() + "," + targPos.z.ToString() + "," +
//            targRot.x.ToString() + "," + targRot.y.ToString() + "," + targRot.z.ToString() + "," + targRot.w.ToString() + "," +
//            allHjCurves[i].gameObject.name + "," +
//            allHjCurves[i].gameObject.transform.position.x.ToString() + "," + allHjCurves[i].gameObject.transform.position.y.ToString() + "," + allHjCurves[i].gameObject.transform.position.z.ToString() + "," +
//            allHjCurves[i].gameObject.transform.localPosition.x.ToString() + "," + allHjCurves[i].gameObject.transform.localPosition.y.ToString() + "," + allHjCurves[i].gameObject.transform.localPosition.z.ToString() + "," +
//            allHjCurves[i].gameObject.transform.rotation.x.ToString() + "," + allHjCurves[i].gameObject.transform.rotation.y.ToString() + "," + allHjCurves[i].gameObject.transform.rotation.z.ToString() + "," + allHjCurves[i].gameObject.transform.rotation.w.ToString() + "," +
//            allHjCurves[i].gameObject.transform.localRotation.x.ToString() + "," + allHjCurves[i].gameObject.transform.localRotation.y.ToString() + "," + allHjCurves[i].gameObject.transform.localRotation.z.ToString() + "," + allHjCurves[i].gameObject.transform.localRotation.w.ToString() + "," +
//            categoricaldir + ","+
//            rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString() + "," + rotation.w.ToString()
//        );
//}