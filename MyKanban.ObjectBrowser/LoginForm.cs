using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MyKanban;

/* ----------------------------------------------------------------------------
    Copyright (c) 2015 Mark E. Gerow

    This file is part of MyKanban.

    MyKanban is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MyKanban is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MyKanban.  If not, see <http://www.gnu.org/licenses/>.
 *  ------------------------------------------------------------------------
 *  Class:      LoginForm
 *  
 *  Purpose:    Provides base methods an properties used by all other classes
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace SampleOB
{
    public struct Globals
    {
        public static long BoardSetId = 0;
        public static long BoardId = 0;
        public static long ProjectId = 0;
        public static long TaskId = 0;
        public static long UserId = 0;
        public static Credential Credential = new Credential();
    }

    public partial class LoginForm : Form
    {
        public Person User = new Person(Globals.Credential);
        public Credential Credential = new Credential();
        public string Server = "Azure";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                Server = ddlServer.SelectedItem.ToString();
                if (this.Server == "Azure")
                {
                    MyKanban.Data.DatabaseType = Data.DbType.SqlServer;
                }
                else
                {
                    MyKanban.Data.DatabaseType = Data.DbType.MySql;
                }

                Globals.Credential = new Credential(txtUserName.Text, txtPassword.Text);
                Credential = Globals.Credential;
                if (Globals.Credential.Id != 0)
                {
                    User = new Person(Globals.Credential.Id, Globals.Credential);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid user name or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
            else
            {
                MessageBox.Show("User name and password fields required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (Globals.UserId != 0)
            {
                User = new Person(Globals.UserId, Globals.Credential);
                txtUserName.Text = User.UserName;
            }
            ddlServer.SelectedIndex = 0;
            this.CenterToParent();
        }
    }
}
