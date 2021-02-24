﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System;
using System.Drawing;
using UnityEngine.UI;

public class databaseSync : MonoBehaviour
{
    // 0: connecting 1: connection successful 2: logged in 3: connection failed
    public int SQLconnectionState = 0; 
    [SerializeField] string SaveFileName = "SaveData.es3";
    [SerializeField] MessageBoxManager msgBox;
    [SerializeField] GameObject OnlineLocalSavePanel;
    [SerializeField] Button PlayButton;
    [SerializeField] Button ExitButton;


    [Space]
    [Header("Messages")]
    [SerializeField] MessageBoxScriptableObject failedToConnectToDatabase;
    [SerializeField] MessageBoxScriptableObject PasswordInvalid; 
    [SerializeField] MessageBoxScriptableObject RegisteredSuccessfully;
    [SerializeField] MessageBoxScriptableObject UserDoesNotExist;
    [SerializeField] MessageBoxScriptableObject UsernameAlreadyTaken;
    [SerializeField] MessageBoxScriptableObject UsernameTooShort;
    [SerializeField] MessageBoxScriptableObject UserBanned;
    [SerializeField] MessageBoxScriptableObject noOnlineSaveFile;
    
    MySqlConnection conn;

    ES3File usrFile;
    string TempUsername;
    string TempPassword;
    bool LoginButtonClicked = false;
    bool RegisterButtonClicked = false;
    string LocalLastTimeSaved;
    string LocalSaveFilePath;
    string LocalSaveFileData;
    bool StartUpWithoutLocalSaveFile = false;
    bool StartUpWithoutOnlineSaveFile = false;
    bool Userbanned;
    bool FirstLogin = true;
    System.DateTime convertedLocalSaveFileTime;
    System.DateTime convertedOnlineSaveFile;
    int SaveFileConflictDecision = 0;

    //0=connecting to Database 1=Connected to database 2=Loggin in 3=Logged 4= Syncing 5=Synced in 6=Registering 7=Registered
    public int StatusID = 0;

    // Start is called before the first frame update
    void Start()
    {
        //TODO how to deal with first Startup when there is no SaveFile (we're getting an Exeption here)
        if (ES3.FileExists("usr.es3"))
        {
            LocalLastTimeSaved = ES3.Load<string>("LastSaved");
            LocalSaveFilePath = Application.persistentDataPath + "/" + SaveFileName;
            LocalSaveFileData = ES3.LoadRawString(SaveFileName);
        }
        else
        {
            StartUpWithoutLocalSaveFile = true;
        }

        if (ES3.KeyExists("FirstLogin"))
        {
            FirstLogin = ES3.Load<bool>("FirstLogin");
        }

        
        if (ES3.FileExists("usr.es3") == false)
        {
            
            // Create a file and write a default Json profile
            using (StreamWriter sw = File.CreateText(Application.persistentDataPath + "/usr.es3"))
            {
                sw.WriteLine("{}");
                sw.Close();
            }
            
            Debug.Log("File Created"); 
            
        }
        else
        {
            Debug.Log("File already exists");
        }

        OnlineLocalSavePanel.SetActive(false);

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
            StatusID = 1;

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
            Dispatcher.RunOnMainThread(() => PopUpWindow(failedToConnectToDatabase));
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

        //Disable Play and Login Buttons so the User can't interrupt the Login process
        Dispatcher.RunOnMainThread(() => PlayButton.interactable = false);
        Dispatcher.RunOnMainThread(() => ExitButton.interactable = false);

        if (SQLconnectionState == 1)
        {
            //declare it here so we can close the reader even when we get a exeption
            MySqlDataReader rdr = null;
            StatusID = 2;

            try
            {
                string sql = "SELECT User_name FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" + Password + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();


                Debug.Log(rdr[0]);
                //this is kinda useless c its already checked in the sql query
                if (rdr[0].ToString() == Username)
                {
                    SQLconnectionState = 2;
                    rdr.Close();
                    Debug.Log("User does exist");
                    StatusID = 1;

                }
                else
                {
                    //This actually never gets called because it always throws an exeption when the query output is null

                    Debug.LogError("User does not exist");
                    StatusID = 1;
                    rdr.Close();
                    Dispatcher.RunOnMainThread(() => PopUpWindow(UserDoesNotExist));
                    
                    //reenable Play and Exit Button again 
                    Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
                    Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);
                }

            }

            catch (System.Exception ex)
            {
                StatusID = 1;
                Debug.LogError(ex.ToString());
                rdr.Close();

                Dispatcher.RunOnMainThread(() => PopUpWindow(UserDoesNotExist));

                //reenable Play and Exit Button again 
                Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
                Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);
                return;

            }

            try
            {
                // _sync saveFile_
                StatusID = 4;

                //Get Online SaveFile DateTime

                //Get UserID
                MySqlDataReader rdr2 = null;
                string sql2 = "SELECT User_id FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" + Password + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                rdr2 = cmd2.ExecuteReader();
                rdr2.Read();
                UserID = rdr2[0].ToString();
                Debug.Log(UserID);
                Dispatcher.RunOnMainThread(() => ES3.Save<string>("UserID", UserID, "usr.es3"));
                rdr2.Close();


                //Get User Banned status
                Debug.Log("Getting User Banned Status");
                MySqlDataReader rdr3 = null;
                string sql3 = "SELECT User_banned FROM User WHERE User_id = '" + UserID + "'";
                MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                rdr3 = cmd3.ExecuteReader();
                rdr3.Read();
                
                //MySQL uses TinyInts as booleans
                if (Convert.ToInt32(rdr3[0]) == 0)
                {
                    //User is not banned
                    Debug.Log("User is not Banned");
                }
                if (Convert.ToInt32(rdr3[0]) > 0)
                {
                    //User is banned
                    SQLconnectionState = 1;
                    StatusID = 1;

                    //Wipe UserID from temp File so the main game doesnt try to Sync
                    Dispatcher.RunOnMainThread(() => ES3.Save<string>("UserID", "", "usr.es3"));

                    Dispatcher.RunOnMainThread(() => PopUpWindow(UserBanned));
                    
                    //Close the reader here as well because we're returning out of this function
                    rdr3.Close();
                    return;
                }
                rdr3.Close();

                //Get SaveFile TimeDate
                MySqlDataReader rdr4 = null;
                string sql4 = "SELECT SaveFile_datum FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd4 = new MySqlCommand(sql4, conn);
                rdr4 = cmd4.ExecuteReader();
                rdr4.Read();
                OnlineLastTimeSaved = rdr4[0].ToString();
                Debug.Log(OnlineLastTimeSaved);
                rdr4.Close();

                //Get SaveFile Data
                MySqlDataReader rdr5 = null;
                string sql5 = "SELECT SaveFile_file FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd5 = new MySqlCommand(sql5, conn);
                rdr5 = cmd5.ExecuteReader();
                rdr5.Read();
                OnlineSaveFileData = rdr5[0].ToString();
                Debug.Log(OnlineSaveFileData);
                rdr5.Close();
            }

            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
                StatusID = 1;

                //reenable Play and Exit Button again 
                Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
                Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);
            }

            int result;
            if (StartUpWithoutLocalSaveFile == true)
            {
                result = -1;
                System.DateTime convertedOnlineSaveFile = System.DateTime.Parse(OnlineLastTimeSaved);
                Debug.Log("Converted OnlineTime: " + convertedOnlineSaveFile);
            }
            else
            {
                //Get local SaveFile DateTime
                Debug.Log("Local last Saved: " + LocalLastTimeSaved);
                convertedLocalSaveFileTime = System.DateTime.Parse(LocalLastTimeSaved);
                Debug.Log("Converted LocalTime: " + convertedLocalSaveFileTime);
                convertedOnlineSaveFile = System.DateTime.Parse(OnlineLastTimeSaved);
                Debug.Log("Converted OnlineTime: " + convertedOnlineSaveFile);

                //check which SaveFile is newer 
                result = System.DateTime.Compare(convertedLocalSaveFileTime, convertedOnlineSaveFile);
            }
            
            if(FirstLogin == true && StartUpWithoutLocalSaveFile == false && StartUpWithoutOnlineSaveFile == false)
            {
                Debug.Log("First Login is true... summoning Panel");
                //Open GUI Panel  
                Dispatcher.RunOnMainThread(() => OnlineLocalSavePanel.SetActive(true));


                //set up an Array so we can send multiple parameters to the other script
                string[] Data = new string[2];
                Data[0] = convertedLocalSaveFileTime.ToString();
                Data[1] = convertedOnlineSaveFile.ToString();

                Dispatcher.RunOnMainThread(() => OnlineLocalSavePanel.SendMessage("EntryInformation" ,Data));

                while (SaveFileConflictDecision == 0)
                {
                    Debug.Log("User hasn't clicked on one of the buttons yet");
                    Thread.Sleep(500);
                }
                
                if(SaveFileConflictDecision == 1)
                {
                    //User wants to Use the Cloud-Saved SaveFile (replacing Local SaveFile with the online SaveFile) 
                    Debug.Log("User wants the Cloud SaveFile");

                    //Check if the Online SaveFile has Content
                    if (Base64Decode(OnlineSaveFileData) == "nothing here yet")
                    {
                        StartUpWithoutOnlineSaveFile = true;
                    }

                       
                    if (StartUpWithoutOnlineSaveFile == false)
                    {
                        //replace local Savefile data with Online SaveFile Data 
                        File.WriteAllText(LocalSaveFilePath, Base64Decode(OnlineSaveFileData));

                        //update LocalLast Saved DateTime
                        Dispatcher.RunOnMainThread(() => ES3.Save<string>("LastSaved", OnlineLastTimeSaved));
                    }
                    else
                    {
                        Dispatcher.RunOnMainThread(() => PopUpWindow(noOnlineSaveFile));
                    }
                }
                
                if(SaveFileConflictDecision == 2)
                {
                    //User wants to keep his localy-Saved SaveFile (replacing online SaveFile with the local SaveFile)
                    Debug.Log("User wants the Local SaveFile");

                    //Update Online SaveFiles (we have to encode the SaveFile into Base64 format because the " symbols in the SaveFile confuse MySQL)
                    string sql1 = "UPDATE SaveFiles SET SaveFile_file = '" + Base64Encode(LocalSaveFileData) + "' WHERE SaveFile_id = '" + UserID + "'";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();

                    //Update Online SaveFile date
                    string sql2 = "UPDATE SaveFiles SET SaveFile_datum = '" + LocalLastTimeSaved + "' WHERE SaveFile_id = '" + UserID + "'";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();

                }
            }
            else
            {
                // Yes, this can happen at the first Login but if there is no Online SaveFile or no Local SaveFile there is no point in choosing between the two
                FirstLogin = false;
            }

            
            //Online saveFile is newer than local SaveFile
            if (result < 0 && FirstLogin != true)
            {
                Debug.Log("Online saveFile is newer than local SaveFile");


                //Check if the Online SaveFile has Content
                if (Base64Decode(OnlineSaveFileData) == "nothing here yet")
                {
                    StartUpWithoutOnlineSaveFile = true;
                }

                
                if(StartUpWithoutOnlineSaveFile == false)
                {
                    //replace local Savefile data with Online SaveFile Data 
                    File.WriteAllText(LocalSaveFilePath, Base64Decode(OnlineSaveFileData));

                    //update LocalLast Saved DateTime
                    Dispatcher.RunOnMainThread(() => ES3.Save<string>("LastSaved", OnlineLastTimeSaved));
                }
            }
            
            //Both SaveFiles are equally old
            else if (result == 0 && FirstLogin != true)
            {
                Debug.Log("Both SaveFiles are equally old"); 
            }

            //Online SaveFile is older than local SaveFile
            else if(FirstLogin != true)
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

            
            if (FirstLogin == true)
            {
                FirstLogin = false;
                Dispatcher.RunOnMainThread(() => ES3.Save<bool>("FirstLogin", FirstLogin));
            }

            //reenable Play and Exit Button again 
            Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
            Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);

        }
        else if(FirstLogin != true)
        {
            //can not login when database is not connected 
            Dispatcher.RunOnMainThread(() => PopUpWindow(failedToConnectToDatabase));
            StatusID = 1;
            
            //reenable Play and Exit Button again 
            Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
            Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);

            return;
        }
        StatusID = 5;

    }

    public void Register (string Username, string Password)
    {
        StatusID = 6;

        //Disable Play and Login Buttons so the User can't interrupt the Registering process
        Dispatcher.RunOnMainThread(() => PlayButton.interactable = false);
        Dispatcher.RunOnMainThread(() => ExitButton.interactable = false);

        //Check if Username is valid
        if (Username.Length > 2)
        {
            //Username lengh OK
        }
        else
        {
            //Username too short 
            Dispatcher.RunOnMainThread(() => PopUpWindow(UsernameTooShort));
            StatusID = 1;
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
                Dispatcher.RunOnMainThread(() => PopUpWindow(UsernameAlreadyTaken));
                StatusID = 1;

                //reenable Play and Exit Button again 
                Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
                Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);

                return;
            }
            else
            {
                //Username available (it always throws an exeption when query output is null so this is kinda pointless)
            }
            rdr.Close();
        }
        catch(System.Exception ex)
        {
            //Username available (it always throws an exeption when query output is Null)
            Debug.Log(ex);
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
            Dispatcher.RunOnMainThread(() => PopUpWindow(PasswordInvalid));
            StatusID = 1;

            //reenable Play and Exit Button again 
            Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
            Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);

            return;
        }


        //Create new account
        string sql2 = "INSERT INTO User (User_name, User_password, User_banned) VALUES ('" + Username + "', '" + Password + "', '0')";
        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
        cmd2.ExecuteNonQuery();

        //get our User ID
        string sql3 = "SELECT User_id FROM User WHERE User_name = '" + Username + "' ";
        MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
        rdr = cmd3.ExecuteReader();
        rdr.Read();
        string UserID = rdr[0].ToString();
        rdr.Close();

        //handle SaveFile Table   (with placeholder DateTime and SaveFile data)
        string sql4 = "INSERT INTO SaveFiles (SaveFile_id, SaveFile_file, SaveFile_datum) VALUES ('" + UserID + "', '" + Base64Encode("nothing here yet") + "', '2015-01-01 00:00:00')";
        MySqlCommand cmd4 = new MySqlCommand(sql4, conn);
        cmd4.ExecuteNonQuery();


        Dispatcher.RunOnMainThread(() => PopUpWindow(RegisteredSuccessfully));

        //reenable Play and Exit Button again 
        Dispatcher.RunOnMainThread(() => PlayButton.interactable = true);
        Dispatcher.RunOnMainThread(() => ExitButton.interactable = true);

        StatusID = 7;
    }

    
    public void ExecuteSaveFileConflict(int decision)
    {
        // 1 = User decided to use the Online SaveFile   2 = User decided to use the Local SaveFile   0 = default Value (no choice made yet)
        SaveFileConflictDecision = decision;
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
