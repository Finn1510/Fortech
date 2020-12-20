using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime;

namespace FortechAdminUtilityTool
{
    public partial class mainWindow : Form
    {
        string chosenUserName;
        bool userBanned;
        int SQLconnectionState = 0;
        MySqlConnection conn;


        public mainWindow()
        {
            InitializeComponent();
            
        }

        public void connectToDatabase()
        {
            // connection parameters
            string connparams = "server=johnny.heliohost.org;user=finn15_FortechUser;database=finn15_InformatikProjekt;port=3306;password=averystrongpassword";

            conn = new MySqlConnection(connparams);

            //try to connect to database 
            try
            {
                conn.Open();
                SQLconnectionState = 1;
                ConnectionStateText.Text = "Database connected";
                ConnectionStateText.ForeColor = Color.Green;



            }
            catch (System.Exception ex)
            {
                SQLconnectionState = 2;
                ConnectionStateText.Text = "connection failed";
                ConnectionStateText.ForeColor = Color.Red;


                Console.WriteLine(ex.ToString());
                return;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                connectToDatabase();
            });
            
        }

        private void SelectUserbutton_Click(object sender, EventArgs e)
        {
            chosenUserName = Microsoft.VisualBasic.Interaction.InputBox("Input a Username", "Select new User", "Username");
            Console.WriteLine(chosenUserName); 

            if(SQLconnectionState == 1)
            {
                //declare it here so we can close the reader even when we get a exeption
                MySqlDataReader rdr = null;

                try
                {
                    string sql = "SELECT User_name FROM User WHERE User_name = '" + chosenUserName + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    rdr.Read();
                    

                    Console.WriteLine(rdr[0]);
                    if(rdr[0].ToString() == chosenUserName)
                    {
                        Console.WriteLine("Username found");
                        rdr.Close();
                        filloutInformation();
                    }
                    else
                    {
                        Console.WriteLine("User does not exist");
                        rdr.Close();
                        string message = "The User" + chosenUserName + " does not exist";
                        string title = "User does not exist";
                        MessageBox.Show(message, title);
                    }

                    
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    rdr.Close();
                    string message = "The User " + chosenUserName + " does not exist";
                    string title = "User does not exist";
                    MessageBox.Show(message, title);
                }
                
            }
           
        }
        
        void filloutInformation()
        {
            UsernameText.Text = chosenUserName;
            
            UserIDtext.Text = "User ID: " + GetUserID();
            
            string sql2 = "SELECT User_password FROM User WHERE User_name = '" + chosenUserName + "'";
            MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
            MySqlDataReader rdr2 = cmd2.ExecuteReader();
            rdr2.Read();
            UserPasswordtext.Text = "Password: " + rdr2[0].ToString();
            rdr2.Close();

            string sql3 = "SELECT User_banned FROM User WHERE User_name = '" + chosenUserName + "'";
            MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
            MySqlDataReader rdr3 = cmd3.ExecuteReader();
            rdr3.Read();
           
            //MySQL uses TinyInts as booleans
            if(Convert.ToInt32(rdr3[0]) == 0)
            {
                userBanned = false;
                Console.WriteLine("user got banned" + userBanned);
                UserStatusText.Text = "Status: not banned";
                UserBanbutton.Text = "Bann User";
            } 
            if(Convert.ToInt32(rdr3[0]) > 0)
            {
                userBanned = true;
                Console.WriteLine("user got banned" + userBanned);
                UserStatusText.Text = "Status: banned";
                UserBanbutton.Text = "Unbann User";
            }
            rdr3.Close();

            ///TODO figure out how link tables work
            string sql5 = "SELECT SaveFile_file FROM SaveFiles WHERE SaveFile_id = '" + GetUserID() + "'";
            MySqlCommand cmd5 = new MySqlCommand(sql5, conn);
            MySqlDataReader rdr5 = cmd5.ExecuteReader();
            rdr5.Read();
            SaveFileDataTextBox.Text = Base64Decode(rdr5[0].ToString());
            rdr5.Close();

        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (SQLconnectionState == 1 && chosenUserName != null)
            {
                try 
                {
                    //dont know why I have to do that but it wants me to do that
                    string sql = "";
                    
                    if (userBanned == false)
                    {
                        Console.WriteLine("User banned");
                        sql = "UPDATE User SET User_banned = 1 WHERE User_name = '" + chosenUserName + "'";
                        UserStatusText.Text = "Status: banned";
                        Console.WriteLine("User banned");
                    } 
                    if(userBanned == true)
                    {
                        Console.WriteLine("User unbanned");
                        sql = "UPDATE User SET User_banned = 0 WHERE User_name = '" + chosenUserName + "'";
                        UserStatusText.Text = "Status: not banned";
                    }
                    
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr3 = cmd.ExecuteReader();
                    rdr3.Close();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    
                }

                
            }
            
            else
            {
                string message = "Can't ban user: make sure you connect to the database and select a User first";
                string title = "Can't ban user";
                MessageBox.Show(message, title);
            }
        }

        private void UsernameText_Click(object sender, EventArgs e)
        {

        }

        private void clearSaveFileButton_Click(object sender, EventArgs e)
        {
            if(SQLconnectionState == 1 && chosenUserName != null)
            {
                
                //clear SaveFile
                string sql2 = "UPDATE SaveFiles SET SaveFile_file = '' WHERE SaveFile_id = '" + GetUserID() + "'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                MySqlDataReader rdr2 = cmd2.ExecuteReader();
                rdr2.Close();

                SaveFileDataTextBox.Text = "";

                string message = "Successfully cleared SaveFile";
                string title = "Cleared SaveFile";
                MessageBox.Show(message, title);

                //TODO figure out how to deal with dates (SaveFile_date)
            }
            else
            {
                string message = "Can't clear SaveFile: make sure you connect to the database and select a User first";
                string title = "Can't clear SaveFile";
                MessageBox.Show(message, title);
            }



        }

        string GetUserID()
        {
            string sql = "SELECT User_id FROM User WHERE User_name = '" + chosenUserName + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            string UserID = rdr[0].ToString();
            rdr.Close();
            return UserID;
        } 

        private void UpdateSaveFileButton_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE SaveFiles SET SaveFile_file = " + "'" + Base64Encode(SaveFileDataTextBox.Text) + "'" + " WHERE SaveFile_id = " + GetUserID();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();

            string message = "Successfully updated SaveFile";
            string title = "Updated SaveFile";
            MessageBox.Show(message, title);
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
}
