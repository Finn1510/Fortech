using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient; 


public class databaseSync : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        //connection parameters
        string connparams = "server=johnny.heliohost.org;user=finn15_FortechUser;database=finn15_InformatikProjekt;port=3306;password=averystrongpassword";

        MySqlConnection conn = new MySqlConnection(connparams);
        
        //try to connect to database 
        try
        {
            
            conn.Open();
            Debug.Log("Connected to database");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
        }

    } 

    void CreateAccount(string Username, string Password)
    {

    } 

    void Login(string Username, string Password)
    {

    }
}
