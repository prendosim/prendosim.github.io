using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TargetTrajectoryArchiv
{
    // #Info: JSON DOES NOT SUPPORT 2D ARRAYS OF ANY KIND. ONLY STUFF THAT IS ALSO SUPPORTED BY THE INSPECTOR IN UNITY  [SerializeField] public float[,] targetAngles = new float[54, 20]; // 54 = numTrial and 20 = numTargets
    [SerializeField] public float[] targetAngles = new float[1080]; // 54 = numTrial x 20 = numTargets
}

[CreateAssetMenu(fileName = "TargetTrajectories", menuName = "Target Angle Motions")]
public class TargetAngleTrajectories : ScriptableObject
{
    //public ExperimentState expState;
    private float preVal, val;
    public TargetTrajectoryArchiv jsonTargetArchiv = new TargetTrajectoryArchiv();
    public string jsonSaveLoadPath;// = Application.persistentDataPath + "/TargetTrajArix.txt";
    List<string> angleVals = new List<string>();

    private void OnEnable()
    {
        jsonSaveLoadPath = Application.persistentDataPath + "/Targ_10_max_v12.txt"; // Change this to persistentDataPath for build
        //Debug.Log("Not doing anything!!!");
        //jsonTargetArchiv.targetAngles = new float[expState.numTrials, expState.numTargets];
    }

    public void OneTimeCreateNSave()
    {
        //jsonTargetArchiv.targetAngles = new float[expState.numTrials, expState.numTargets];
        angleVals.Clear();

        // Randomize first target movement direction
        int idx = 0;

        for (int trial = 0; trial < 54; trial++) // 54 = Number of trials in the experiment 
        {
            // Randomize initial target motion direciton (left/right)
            #region
            int randDir = UnityEngine.Random.Range(0, 2);
            if (randDir == 0)
            {
                preVal = 135f;
            }
            else
            {
                preVal = 45f;
            }
            #endregion

            for (int tr = 0; tr < 10; tr++) // 20 = Number of targets per trial 
            {
                /* 
                 * The next target angle should be on the opposite side of the pressure indicator, 
                 * so as to encourage people to open and close their hands between every 2 targets 
                 * And also make sure that the next val is at least 20 degrees from the previous angle 
                 * to avoid hovering/flickering around the center of the dial by 
                 * creating targets between 
                 * 90+20=110 degrees 
                 * and
                 * 90-20=70 degrees
                */
                if (preVal >= 90f)
                    val = UnityEngine.Random.Range(-25f, 50f);
                else
                    val = UnityEngine.Random.Range(130f, 205f);

                // Clamp target angles between 0 and 180 degreess to avoid weird target rotations beyond this dedicated range
                val = Mathf.Clamp(val, 0.1f, 179.9f);

                angleVals.Add(val.ToString("F2"));
                //jsonTargetArchiv.targetAngles[idx] = val;

                preVal = val;

                idx++;
            }
        }

        Debug.Log("Trajectories created!!!");

        // Convert all this data into a json file
        //string jsonData = JsonUtility.ToJson(jsonTargetArchiv, true);

        string[] angleArrString = angleVals.ToArray();
        string jsonData = string.Join(",", angleArrString);

        File.WriteAllText(jsonSaveLoadPath, jsonData);
        Debug.Log("Trajectories Saved!!!");
    }

    //public string LoadJsonTargetData()
    //{
    //    string jsonData = File.ReadAllText(jsonSaveLoadPath);
    //    jsonTargetArchiv = JsonUtility.FromJson<TargetTrajectoryArchiv>(jsonData);
    //    Debug.Log("Trajectories Loaded!!!");

    //    return jsonData;
    //}

    //public string LoadJsonTargetData()
    //{
    //    string jsonData = File.ReadAllText(jsonSaveLoadPath);
    //    Debug.Log("Trajectories Loaded!!!");

    //    return jsonData;
    //}
}