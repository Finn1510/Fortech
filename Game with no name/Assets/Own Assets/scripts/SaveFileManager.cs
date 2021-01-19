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
    [Space]
    [SerializeField] string SaveFileName = "SaveData.es3";
    
    int AutoSaveIntervallSeconds;
    
    MySqlConnection conn;
    string LocalLastTimeSaved;
    string LocalSaveFilePath;
    string LocalSaveFileData;
    
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
        //We have to do this here because SaveFileSync runs on another Thread
        LocalLastTimeSaved = ES3.Load<string>("LastSaved");
        LocalSaveFileData = ES3.LoadRawString(SaveFileName);

        SaveEverything();
        StartCoroutine(AutoSaveDelay(Delay));
    }

    IEnumerator AutoSaveDelay(int Delay)
    {
        yield return new WaitForSeconds(Delay);
        AutoSave(Delay);
    }
    
    void SaveEverything()
    {
        foreach (GameObject GO in SaveGameObjects)
        {
            GO.SendMessage("Save");
        }
    }

    public void Save()
    {
        currentTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        ES3.Save<string>("LastSaved", currentTime);
    }

    private void OnApplicationQuit()
    {
        SaveEverything();
    }


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
            SQLconnectionState = 2;
        }
    }

    void SaveFileSync(string UserID)
    {
        if(SQLconnectionState == 1)
        {
            try
            {
                //Update Online SaveFiles (we have to encode the SaveFile into Base64 format because the " symbols in the SaveFile confuse MySQL)
                string sql = "UPDATE SaveFiles SET SaveFile_file = '" + Base64Encode(LocalSaveFileData) + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                //Update Online SaveFile date
                string sql2 = "UPDATE SaveFiles SET SaveFile_datum = '" + LocalLastTimeSaved + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                cmd2.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        else
        {
            Debug.Log("Cant Sync SaveFile: we're no logged in");
        }

    }

    string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }


}
