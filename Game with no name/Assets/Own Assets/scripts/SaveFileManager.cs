using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class SaveFileManager : MonoBehaviour
{
    string currentTime;
    [SerializeField] string LastTimeSaved;
    [SerializeField] int AutoSaveIntervallMinutes;
    [SerializeField] GameObject[] SaveGameObjects;
    [SerializeField] bool OnlineSync = true;
    [Space]
    [SerializeField] string SaveFileName = "SaveData.es3";
    
    int AutoSaveIntervallSeconds;
    bool coreDestroyed = false;
    
    MySqlConnection conn;
    
    string LocalSaveFilePath;
    string UserID;
    
    // 0: connecting 1: connection successful 2: logged in 3: connection failed
    public int SQLconnectionState = 0;
    

    private void Start()
    {
        if (ES3.KeyExists("LastSaved"))
        {
            LastTimeSaved = ES3.Load<string>("LastSaved");
        }
        LocalSaveFilePath = Application.persistentDataPath + "/" + SaveFileName;

        AutoSaveIntervallSeconds = AutoSaveIntervallMinutes * 60;
        AutoSave(AutoSaveIntervallSeconds);

     }


    void AutoSave(int Delay)
    {
        SaveEverything();
        StartCoroutine(AutoSaveDelay(Delay));
    }

    IEnumerator AutoSaveDelay(int Delay)
    {
        yield return new WaitForSeconds(Delay);
        AutoSave(Delay);
    }
    
    //Sends the "Save" message to all GameObjects in the array
    void SaveEverything()
    {
        //dont save anything when the player lost the game
        if(coreDestroyed == false)
        {
            foreach (GameObject GO in SaveGameObjects)
            {
                GO.SendMessage("Save");
            }
        }
        
    }

    public void Save()
    {
        currentTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        Debug.Log("Current time is: " + currentTime);
        ES3.Save<string>("LastSaved", currentTime);
    }

    private void OnApplicationQuit()
    {
        SaveEverything();
        
        if(OnlineSync == true)
        {
            ConnectToDatabase();
            SaveFileSync();
        }
        else
        {
            Debug.Log("CloudSync has been disabled manually");
        }
        
    }

    //Connects to the Database
    void ConnectToDatabase()
    {
        //connection parameters
        string connparams = "server=johnny.heliohost.org;user=finn15_FortechUser;database=finn15_InformatikProjekt;port=3306;password=averystrongpassword";

        conn = new MySqlConnection(connparams);

        //try to connect to database 
        try
        {
            conn = new MySqlConnection(connparams);
            conn.Open();
            Debug.Log("Database connected");
            SQLconnectionState = 1;
            

        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.ToString());
            SQLconnectionState = 3;
        }

        if (ES3.FileExists("usr.es3"))
        {
            try
            {
                UserID = ES3.Load<string>("UserID", "usr.es3");
                SQLconnectionState = 2;
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex.ToString());
                SQLconnectionState = 3;
            }
        }
        else
        {
            Debug.Log("User is not logged in");
            SQLconnectionState = 3;
        }
    }

    //Syncs the SaveFile
    void SaveFileSync()
    {
        if(SQLconnectionState == 2)
        {
            try
            {
                //Update Online SaveFile date We Update the Date first so the other GameObjects have time to save their values so we dont upload an old SaveFile to the Database
                string sql2 = "UPDATE SaveFiles SET SaveFile_datum = '" + currentTime + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                cmd2.ExecuteNonQuery();

                //Update Online SaveFiles (we have to encode the SaveFile into Base64 format because the " symbols in the SaveFile confuse MySQL)
                string sql = "UPDATE SaveFiles SET SaveFile_file = '" + Base64Encode(ES3.LoadRawString(SaveFileName)) + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                Debug.Log("Synced SaveFile");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
        else
        {
            Debug.Log("Cant Sync SaveFile: we're not logged in");
        }

    }

    public void CoreDestroyed()
    {
        coreDestroyed = true;
    }

    //Function for encoding plain text into base64 format
    string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    //Function for decoding base64 data into plain text
    string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }


}
