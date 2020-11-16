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

        //TODO sync saveFile

        }
        else
        {
            //can not login when database is not connected
        }
    }
}
