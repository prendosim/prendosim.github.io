using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class LoadModelFromOutside : MonoBehaviour
{
    public TMPro.TMP_InputField InpFilePath; 

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.B))
        {
            DirectoryInfo dir = new DirectoryInfo(InpFilePath.text + "/");
            FileInfo[] info = dir.GetFiles("*.*");

            foreach (FileInfo f in info)
            {
                Debug.Log(f.ToString());
            }

            Mesh lobj = (Mesh) Instantiate(Resources.Load(info[0].ToString())) as Mesh;
            
            GameObject newItem = new GameObject();
            MeshFilter filter = newItem.AddComponent<MeshFilter>();
            MeshRenderer renderer = newItem.AddComponent<MeshRenderer>();

            filter.sharedMesh = lobj; 
            newItem.transform.position = new Vector3(0f, 1f, 0f);
        }
    }
}
