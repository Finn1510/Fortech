using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient; 


public class databaseSync : MonoBehaviour
{
    // 0: connecting 1: connection successful 2: logged in 3: connection failed
    public int SQLconnectionState = 0;
    
    MySqlConnection conn;
    

    // Start is called before the first frame update
    void Start()
    {
        //connection parameters
        string connparams = "server=johnny.heliohost.org;user=finn15_FortechUser;database=finn15_InformatikProjekt;port=3306;password=averystrongpassword";

        MySqlConnection conn = new MySqlConnection(connparams);
        
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

    void CreateAccount(string Username, string Password)
    {

    } 

    void Login(string Username, string Password)
    {
        string UserID;

        if (SQLconnectionState == 1)
        {
            //declare it here so we can close the reader even when we get a exeption
            MySqlDataReader rdr = null;

            //TODO how to tell the User that his password is wrong without showing a raw exeption
            try
            {
                string sql = "SELECT User_name FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" +  Password + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();


                Debug.Log(rdr[0]);
                //this is kinda useless cuz its already checked in the sql command
                if (rdr[0].ToString() == Username)
                {
                    SQLconnectionState = 2;
                    rdr.Close();
                }
                else
                {
                    Debug.LogError("User does not exist");
                    rdr.Close();
                }


            }
            catch (System.Exception ex)
            { 
                Debug.LogError(ex.ToString());
                rdr.Close();
                
            }

            try
            {
                // _sync saveFile_

                //Get local SaveFile DateTime
                string LastTimeSaved = ES3.Load<string>("LastSaved");

                //Get Online SaveFile DateTime

                //Get UserID
                MySqlDataReader rdr2 = null;
                string sql2 = "SELECT User_id FROM User WHERE User_name = '" + Username + "' AND User_password = " + "'" + Password + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                rdr2 = cmd2.ExecuteReader();
                rdr2.Read();
                UserID = rdr2[0].ToString();
                Debug.Log(UserID);

                //Get SaveFile TimeDate
                MySqlDataReader rdr3 = null;
                string sql3 = "SELECT SaveFile_datum FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                rdr3 = cmd3.ExecuteReader();
                rdr3.Read();
                string OnlineLastTimeSaved = rdr3[0].ToString();
                Debug.Log(OnlineLastTimeSaved);

                //Get SaveFile Data
                MySqlDataReader rdr4 = null;
                string sql4 = "SELECT SaveFile_file FROM SaveFiles WHERE SaveFile_id = '" + UserID + "'";
                MySqlCommand cmd4 = new MySqlCommand(sql3, conn);
                rdr4 = cmd4.ExecuteReader();
                rdr4.Read();
                string SaveFileData = rdr4[0].ToString();
                Debug.Log(SaveFileData);
            }

            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }



        }
        else
        {
            //can not login when database is not connected
        }
    }
}
