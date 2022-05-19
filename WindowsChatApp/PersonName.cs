using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsChatApp
{
    public partial class PersonName : Form
    {
        public PersonName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Set")
            {
                this.Hide();
            }
            else
            {            
                 if (!string.IsNullOrEmpty(txtUserName.Text) && txtPass.Text == "admin")
                {
                    Form1 form1 = new Form1(); 
                    form1.userName = txtUserName.Text;
                    form1.Text = "Logged In As "+ txtUserName.Text;
                    form1.Show();
                    form1.nameF = this;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password incorrect","Wrong Credential");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
