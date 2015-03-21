using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MyKanban;

/* ----------------------------------------------------------------------------- /
// File:        UpdateTaskSequence.aspx.cs
// Purpose:     Save task order when moved within a column on the Kanban board
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

public partial class UpdateTaskSequence : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string callback = Request.QueryString["callback"];
            long boardId = long.Parse(Request.QueryString["boardId"]);
            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];
            string token = Request.QueryString.AllKeys.Contains("token") ? Request.QueryString["token"] : "";

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            string[] taskIds = Request.QueryString["taskIds"].Split(';');

            for (int i = 0; i < taskIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(taskIds[i]))
                {
                    try
                    {
                        long taskId = long.Parse(taskIds[i]);
                        MyKanban.Task task = new Task(taskId, new Credential(token));
                        task.Sequence = i;
                        task.Update();
                    }
                    catch 
                    {
                        // No op
                    }
                }
            }

            Response.Write(callback + "({'response' : 'success'});");
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }
    }
}