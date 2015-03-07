using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
 *  Class:      JSONForm
 *  
 *  Purpose:    Provides base methods an properties used by all other classes
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace SampleOB
{
    public partial class JSONForm : Form
    {
        public JSONForm()
        {
            InitializeComponent();
        }

        public string JSON = "";

        private void JSONForm_Load(object sender, EventArgs e)
        {
            txtJson.Text = JSON;
            this.CenterToParent();
            txtJson.SelectionStart = 0;
            txtJson.SelectionLength = 0;
            Cursor.Current = Cursors.Default;
        }
    }
}
