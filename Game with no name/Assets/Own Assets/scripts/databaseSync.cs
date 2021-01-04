using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System;


public class databaseSync : MonoBehaviour
{
    // 0: connecting 1: connection successful 2: logged in 3: connection failed
    public int SQLconnectionState = 0; 
    [SerializeField] string SaveFileName = "SaveData.es3";
    [SerializeField] MessageBoxManager msgBox;

    [Space]
    [Header("Messages")]
    [SerializeField] MessageBoxScriptableObject failedToConnectToDatabase;
    [SerializeField] MessageBoxScriptableObject LoginSuccessfully;
    [SerializeField] MessageBoxScriptableObject PasswordInvalid; 
    [SerializeField] MessageBoxScriptableObject RegisteredSuccessfully;
    [SerializeField] MessageBoxScriptableObject UserDoesNotExist;
    [SerializeField] MessageBoxScriptableObject UsernameAlreadyTaken;
    [SerializeField] MessageBoxScriptableObject UsernameTooShort;
    [SerializeField] MessageBoxScriptableObject SaveFileSyncedSuccessfully;
    
    MySqlConnection conn;

    string TempUsername;
    string TempPassword;
    bool LoginButtonClicked = false;
    bool RegisterButtonClicked = false;
    string LocalLastTimeSaved;
    string LocalSaveFilePath;
    string LocalSaveFileData;

    // Start is called before the first frame update
    void Start()
    {
        LocalLastTimeSaved = ES3.Load<string>("LastSaved");
        LocalSaveFilePath = Application.persistentDataPath + "/" + SaveFileName;
        LocalSaveFileData = ES3.LoadRawString(SaveFileName);


        ThreadStart ThreadRef = new ThreadStart(ConnectToDatabase);
        Thread ConnectThread = new Thread(ThreadRef);
        ConnectThread.Start();

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
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
            SQLconnectionState = 2;
        } 
    }
    
    

    public void ExecuteLogin(string Username, string Password)
    {
        ThreadStart ThreadRef = new ThreadStart(() => Login(Username, Password));
        Thread LoginThread = new Thread(ThreadRef);
        LoginThread.Start();
    }

    public void ExecuteRegister(string Username, string Password)
    {
        ThreadStart ThreadRef = new ThreadStart(() => Register(Username, Password));
        Thread RegisterThread = new Thread(ThreadRef);
        RegisterThread.Start();
    }

    public void Login(string Username, string Password)
    {
        //for some reason the compiler wants me to assign these local variables when I am trying to use it 
        string OnlineLastTimeSaved = null;
        string OnlineSaveFileData = null;
        string UserID = null;
        
        if (SQLconnectionState == 1)
        {
            //declare it here so we can close the reader even when we get a exeption
            MySqlDataReader rdr = null;


            try
            {
                string sql = "SELECT User_name FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" + Password + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();


                Debug.Log(rdr[0]);
                //this is kinda useless cuz its already checked in the sql command
                if (rdr[0].ToString() == Username)
                {
                    SQLconnectionState = 2;
                    rdr.Close();
                    Debug.Log("User does exist");
                    Dispatcher.RunOnMainThread(() => PopUpWindow(LoginSuccessfully));

                }
                else
                {
                    Debug.LogError("User does not exist");
                    rdr.Close();
                    Dispatcher.RunOnMainThread(() => PopUpWindow(UserDoesNotExist));
                }

            }

            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
                Dispatcher.RunOnMainThread(() => PopUpWindow(UserDoesNotExist));

            }

            try
            {
                // _sync saveFile_

                //Get local SaveFile DateTime
                Debug.Log("Local last Saved: " + LocalLastTimeSaved);

                //Get Online SaveFile DateTime

                //Get UserID
                MySqlDataReader rdr2 = null;
                string sql2 = "SELECT User_id FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" + Password + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                rdr2 = cmd2.ExecuteReader();
                rdr2.Read();
                UserID = rdr2[0].ToString();
                Debug.Log(UserID);
                rdr2.Close();

                //Get SaveFile TimeDate
                MySqlDataReader rdr3 = null;
                string sql3 = "SELECT SaveFile_datum FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                rdr3 = cmd3.ExecuteReader();
                rdr3.Read();
                OnlineLastTimeSaved = rdr3[0].ToString();
                Debug.Log(OnlineLastTimeSaved);
                rdr3.Close();

                //Get SaveFile Data
                MySqlDataReader rdr4 = null;
                string sql4 = "SELECT SaveFile_file FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd4 = new MySqlCommand(sql4, conn);
                rdr4 = cmd4.ExecuteReader();
                rdr4.Read();
                OnlineSaveFileData = rdr4[0].ToString();
                Debug.Log(OnlineSaveFileData);
                rdr4.Close();
            }

            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }

            System.DateTime convertedLocalSaveFileTime = System.DateTime.Parse(LocalLastTimeSaved);
            Debug.Log("Converted LocalTime: " + convertedLocalSaveFileTime);
            System.DateTime convertedOnlineSaveFile = System.DateTime.Parse(OnlineLastTimeSaved);
            Debug.Log("Converted OnlineTime: " + convertedOnlineSaveFile);

            //check which SaveFile is newer 
            int result = System.DateTime.Compare(convertedLocalSaveFileTime, convertedOnlineSaveFile);

            //Online saveFile is newer than local SaveFile
            if (result < 0)
            {
                Debug.Log("Online saveFile is newer than local SaveFile");

                //replace local Savefile data with Online SaveFile Data 
                File.WriteAllText(LocalSaveFilePath, Base64Decode(OnlineSaveFileData));

                //update LocalLast Saved DateTime
                Dispatcher.RunOnMainThread(() => ES3.Save<string>("LastSaved", OnlineLastTimeSaved));

            }
            
            //Both SaveFile are equally old
            else if (result == 0)
            {
                Debug.Log("Both SaveFiles are equally old"); 
            }

            //Online SaveFile is older than local SaveFile
            else
            {
                Debug.Log("Online SaveFile is older than local SaveFile");

                //Update Online SaveFiles (we have to encode the SaveFile into Base64 format because the " symbols in the SaveFile confuse MySQL)
                string sql5 = "UPDATE SaveFiles SET SaveFile_file = '" + Base64Encode(LocalSaveFileData) + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd5 = new MySqlCommand(sql5, conn);
                cmd5.ExecuteNonQuery();

                //Update Online SaveFile date
                string sql6 = "UPDATE SaveFiles SET SaveFile_datum = '" + LocalLastTimeSaved + "' WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd6 = new MySqlCommand(sql6, conn);
                cmd6.ExecuteNonQuery();
            }

        }
        else
        {
            //can not login when database is not connected 
            Dispatcher.RunOnMainThread(() => PopUpWindow(failedToConnectToDatabase));

            return;
        }

        //We can put this back in when the login process is moved to another thread
        Dispatcher.RunOnMainThread(() => PopUpWindow(SaveFileSyncedSuccessfully));

    }

    public void Register (string Username, string Password)
    {
        //TODO Check if username is valid 
        
        if(Username.Length > 2)
        {
            //Username lengh OK
        }
        else
        {
            //Username too short 
            msgBox.ErrorMessageBox(UsernameTooShort);
            return;
        }

        MySqlDataReader rdr = null;
        try
        {
            string sql = "SELECT User_name FROM User WHERE User_name = '" + Username + "' ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();
            rdr.Read();

            if (Username == rdr[0].ToString())
            {
                //Username already taken  
                msgBox.ErrorMessageBox(UsernameAlreadyTaken);
                return;
            }
            else
            {
                //Username available (it always throws an exeption when query output is Null so this is kinda pointless)
            }
            rdr.Close();
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex);
            rdr.Close();
        }
        

        //Check if Password has Upper- and Lowercase letters + digits
        bool hasUpper = false; bool hasLower = false; bool hasDigit = false;
        for (int i = 0; i < Password.Length && !(hasUpper && hasLower && hasDigit); i++)
        {
            char c = Password[i];
            if (!hasUpper) hasUpper = char.IsUpper(c);
            if (!hasLower) hasLower = char.IsLower(c);
            if (!hasDigit) hasDigit = char.IsDigit(c);
        }

        //Check if password is valid 
        if (Password.Length >= 4 && hasUpper == true && hasLower == true && hasDigit == true)
        {
            //Password is alright
        }
        else
        {
            //Password is not valid 
            msgBox.ErrorMessageBox(PasswordInvalid);
            return;
        }


        //Create new account
        string sql2 = "INSERT INTO User (User_name, User_password, User_banned) VALUES ('" + Username + "', '" + Password + "', '0')";
        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
        cmd2.ExecuteNonQuery();
        msgBox.MessageBox(RegisteredSuccessfully); 

        //TODO handle SaveFile Table
    }

    
    void PopUpWindow(MessageBoxScriptableObject msg)
    {
        msgBox.ErrorMessageBox(msg);
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
