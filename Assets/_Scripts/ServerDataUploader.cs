using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DataClassing
{
    [SerializeField] public string conditionInfo = string.Empty;
    [SerializeField] public string[] dataTable = new string[1001];
}

public class ServerDataUploader : MonoBehaviour
{
    private DataClassing expData = new DataClassing();

    public bool startSave, endSave; 

    // Variables
    #region

    public GameObject[] Objects2Record;
    public string setURL = "http://www.bhamxr.com:1234/JoesTaskJson_v1.php";

    private List<string> recordDataList = new List<string>();

    private float startTime = 0f;
    private bool startOfTrial = false;
    private int frameNum = 0;
    private string conditionPost;
    private string userIDPost;
    private int trialnum = -1;
    #endregion

    private void Update()
    {
        // Test script 
        if (Input.GetKeyDown(KeyCode.V))
        {
            string userIDPost = ""; 

            conditionPost = "_";

            Debug.Log("Upload Command Sent!!!");
            StartCoroutine(Upload2());
        }

        if (startSave)
        {
            recordDataList.Clear();

            trialnum++;
            startOfTrial = true;
            startTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        /*---------------------------------------- Trial data collection start -------------------------------------------*/
        #region
        if (startOfTrial)
        {
            frameNum = 0;
            startTime = Time.time;
            startOfTrial = false;
        }

        // Trial duration 
        float currTime = Time.time - startTime;

        foreach (GameObject obj in Objects2Record)
        {
            recordDataList.Add(frameNum.ToString() + ";" +
                               obj.name + ";" +
                               obj.transform.position.x.ToString("F3") + ";" +
                               obj.transform.position.y.ToString("F3") + ";" +
                               obj.transform.position.z.ToString("F3") + ";" +
                               obj.transform.localEulerAngles.x.ToString("F3") + ";" +
                               obj.transform.localEulerAngles.y.ToString("F3") + ";" +
                               obj.transform.localEulerAngles.z.ToString("F3") + ";" +
                               currTime.ToString("F3")
                               );
        }
        frameNum++;

        #endregion
        /*---------------------------------------- Trial data collection end ---------------------------------------------*/
        if (endSave) // end of trial
        {
            StartCoroutine(Upload2());
            endSave = false; 
        }
    }

    private IEnumerator Upload2()
    {
        // Convert to json and send to another site on the server
        expData.conditionInfo = userIDPost + conditionPost;
        expData.dataTable = recordDataList.ToArray();

        WWWForm form2 = new WWWForm();
        string jsonString = JsonConvert.SerializeObject(expData, Formatting.Indented);
        form2.AddField("postUserID", expData.conditionInfo);
        form2.AddField("postJsonData", jsonString);

        UnityWebRequest www2 = UnityWebRequest.Post(setURL, form2);
        yield return www2.SendWebRequest();

        if (www2.isNetworkError || www2.isHttpError)
        {
            Debug.Log(www2.error);
        }

        // Empty text fields for next trials (potential for issues with next trial)
        recordDataList.Clear();
    }
}