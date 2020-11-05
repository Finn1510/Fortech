using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FortechAdminUtilityTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SelectUserbutton_Click(object sender, EventArgs e)
        {
            Microsoft.VisualBasic.Interaction.InputBox("Input a Username", "Select new User", "Username");
           
        }
    }
}
