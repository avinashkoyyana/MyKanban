using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MyKanban;

/* ----------------------------------------------------------------------------- /
// File:        DeleteTask.aspx.cs
// Purpose:     Delete a task from the MyKanban database
// By:          Mark E. Gerow
// Date:        3/20/2015
// ----------------------------------------------------------------------------- /
// Mode:
// ----------------------------------------------------------------------------- /
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
// -----------------------------------------------------------------------------*/

public partial class DeleteTask : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            long taskId = long.Parse(Request.QueryString["taskId"]);
            string callback = Request.QueryString["callback"];
            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];
            string token = Request.QueryString.AllKeys.Contains("token") ? Request.QueryString["token"] : "";

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            MyKanban.Task task = new Task(taskId, new Credential(token));
            task.Delete();
            Response.Write(callback + "({'TaskId' : " + taskId.ToString() + "});");
        }
        catch (Exception ex)
        {
            Response.Write("{'Error' : '" + ex.Message + "'}");
        }
    }
}