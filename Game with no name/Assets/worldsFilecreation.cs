using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class worldsFilecreation : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Awake()
    {
        string saveFilePath = string.Concat(Application.persistentDataPath, "/worlds");

        if (ES3.FileExists("worlds"))
        {
            Debug.Log("Worlds file already exists"); 
        }
        else
        {
            File.WriteAllText(saveFilePath, "{}");
            Debug.Log("Created Worlds File"); 
        }
    }   

    
}
