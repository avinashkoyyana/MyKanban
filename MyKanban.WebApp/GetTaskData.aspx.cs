﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MyKanban;
using System.Data;

/* ----------------------------------------------------------------------------- /
// File:        GetTaskData.aspx.cs
// Purpose:     Return details for a single task
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

public partial class GetTaskData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MyKanban.Task task = null;
            string callback = Request.QueryString["callback"];
            long taskId = long.Parse(Request.QueryString["taskId"]);
            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];
            string token = Request.QueryString.AllKeys.Contains("token") ? Request.QueryString["token"] : "";

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            DataSet dsTask = MyKanban.Data.GetTaskById(taskId, new Credential(token).Id);

            // If 0 rows returned for task, set some default values
            if (dsTask.Tables[0].Rows.Count == 0)
            {
                DataRow drTask = dsTask.Tables[0].NewRow();
                drTask["id"] = 0;
                drTask["project_id"] = 0;
                drTask["start_date"] = "1900-01-01";
                drTask["end_date"] = "1900-01-01";
                drTask["est_hours"] = 8;
                drTask["act_hours"] = 0;
                dsTask.Tables[0].Rows.Add(drTask);
            }

            dsTask.Tables[0].TableName = "task";
            dsTask.Tables[1].TableName = "task_assignee";
            dsTask.Tables[2].TableName = "task_approver";
            Response.Write(callback + "(" + MyKanban.Data.GetJson(dsTask) + ");");
        }
        catch { }
    }
}