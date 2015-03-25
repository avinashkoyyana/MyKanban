using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.IO;
using System.Runtime.Serialization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft;
using Microsoft.SharePoint.Client.Utilities;

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
 *  Class:      Data
 *  
 *  Purpose:    Provides base methods for all CRUD operations to persist
 *              data to, or retrieve data from, an external data store.
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public static class Data
    {
        // Some sample MySql connection strings:
        // -------------------------------------
        // Local instance: Server=localhost;Database=database;Uid=uid;Pwd=password;
        // -------------------------------------

        public static string MySqlConnectionString = "Server=localhost;Database=mykanban;Uid=mykanban;Pwd=megabase;";

        // Some sample SQL Server connections strings:
        // -------------------------------------------
        // Local instance: Data Source=server;Initial Catalog=databae;UID=uid;PWD=password
        // -------------------------------------------
        public static string SqlServerConnectionString = "Server=server;Database=databae;User ID=uid;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public static string SharePointSiteUrl = "";

        public static string encryptionKey = "key";

        public enum AuthorizationType { Read, Add, Update, Delete };

        /// <summary>
        /// Available connection types
        /// </summary>
        /// <example>
        /// <code>
        /// MyKanban.Data.DatabaseType = Data.DbType.SqlServer;
        /// </code>
        /// </example>
        public enum DbType 
        { 
            /// <summary>
            /// Connect to MySQL database using MySql.Data drivers
            /// </summary>
            MySql, 

            /// <summary>
            /// Connect to SQL Server database using System.Data.SqlClient drivers
            /// </summary>
            SqlServer, 

            /// <summary>
            /// Connect to SharePoint 2010+ using CSOM API
            /// </summary>
            SharePoint 
        };

        public enum SharePointOperation { Add, Delete, Get, Update };

        private static DbType _databaseType = DbType.MySql;

        /// <summary>
        /// Type of connection to use
        /// </summary>
        /// <example>
        /// <code>
        /// MyKanban.Data.DatabaseType = Data.DbType.SqlServer;
        /// </code>
        /// </example>
        public static DbType DatabaseType
        {
            get { return _databaseType; }
            set { _databaseType = value; }
        }

        /// <summary>
        /// Add an approver to a task
        /// </summary>
        /// <param name="taskId">ID# of task to add approver to</param>
        /// <param name="personId">ID# of person being added as an approver</param>
        /// <param name="userId">ID# of user making the assignment</param>
        /// <returns></returns>
        public static DataSet AddApproverToTask(long taskId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_person_id", personId);
            DataSet ds = GetDataViaStoredProcedure("sp_add_approver_to_task", parameters);
            return ds;
        }

        /// <summary>
        /// Add an assignee to a task
        /// </summary>
        /// <param name="taskId">ID# of task to assign to</param>
        /// <param name="personId">ID# of person being assigned</param>
        /// <param name="userId">ID# of user making the assignment</param>
        /// <returns></returns>
        public static DataSet AddAssigneeToTask(long taskId, long personId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_task_id", taskId);
                parameters.Add("t_person_id", personId);
                ds = GetDataViaStoredProcedure("sp_add_assignee_to_task", parameters);
            }
            else
            {
                parameters.Add("task_id", taskId);
                parameters.Add("person_id", personId);
                ds = ExecuteSharePointRequest("task_assignee", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddBoard(string boardName, long boardSetId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("name", boardName);
                parameters.Add("b_board_set_id", boardSetId);
                parameters.Add("b_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_board", parameters);
            }
            else
            {
                parameters.Add("name", boardName);
                parameters.Add("board_set_id", boardSetId);
                ds = ExecuteSharePointRequest("board", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddBoardSet(string boardSetName, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name", boardSetName);
            parameters.Add("bs_user_id", userId);
            if (DatabaseType != DbType.SharePoint)
            {
                ds = GetDataViaStoredProcedure("sp_add_board_set", parameters);
            }
            else
            {
                ds = ExecuteSharePointRequest("board_set", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddPerson(
	        string personName,
	        string pictureUrl,
	        string email,
	        string phone,
	        string userName,
	        string password,
            bool canLogin, 
            long userId
        )
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_name", personName);
                parameters.Add("p_picture_url", pictureUrl);
                parameters.Add("p_email", email);
                parameters.Add("p_phone", phone);
                parameters.Add("p_user_name", userName);
                parameters.Add("p_password", password);
                parameters.Add("p_can_login", canLogin);
                parameters.Add("p_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_person", parameters);
            }
            else
            {
                parameters.Add("name", personName);
                parameters.Add("picture_url", pictureUrl);
                parameters.Add("email", email);
                parameters.Add("phone", phone);
                parameters.Add("user_name", userName);
                parameters.Add("password", password);
                parameters.Add("can_login", canLogin);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("person", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddPersonToBoard(long boardId, long personId, long userId, bool canAdd, bool canEdit, bool canDelete, bool canRead)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_board_id", boardId);
                parameters.Add("p_person_id", personId);
                parameters.Add("p_can_add", canAdd);
                parameters.Add("p_can_edit", canEdit);
                parameters.Add("p_can_delete", canDelete);
                parameters.Add("p_can_read", canRead);
                ds = GetDataViaStoredProcedure("sp_add_person_to_board", parameters);
            }
            else
            {
                parameters.Add("board_id", boardId);
                parameters.Add("person_id", personId);
                parameters.Add("can_add", canAdd);
                parameters.Add("can_edit", canEdit);
                parameters.Add("can_delete", canDelete);
                parameters.Add("can_read", canRead);
                ds = ExecuteSharePointRequest("board_person", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddStakeholderToProject(long projectId, long personId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_project_id", projectId);
                parameters.Add("p_person_id", personId);
                ds = GetDataViaStoredProcedure("sp_add_stakeholder_to_project", parameters);
            }
            else
            {
                parameters.Add("project_id", projectId);
                parameters.Add("person_id", personId);
                ds = ExecuteSharePointRequest("project_stakeholder", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddProject(
            string projectName,
            long projectLead,
            DateTime expectedStartDate,
            DateTime expectedEndDate,
            DateTime earliestTaskStartDate,
            DateTime latestTeaskEndDate,
            long status,
            string defineDone,
            double taskEstHours,
            double taskActHours, 
            long userId
        )
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_name", projectName);
                parameters.Add("p_project_lead", projectLead);
                parameters.Add("p_expected_start_date", expectedStartDate);
                parameters.Add("p_expected_end_date", expectedEndDate);
                parameters.Add("p_earliest_task_start_date", earliestTaskStartDate);
                parameters.Add("p_latest_task_end_date", latestTeaskEndDate);
                parameters.Add("p_status", status);
                parameters.Add("p_define_done", (!string.IsNullOrEmpty(defineDone) ? defineDone : ""));
                parameters.Add("p_task_est_hours", taskEstHours);
                parameters.Add("p_task_act_hours", taskActHours);
                parameters.Add("p_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_project", parameters);
            }
            else
            {
                parameters.Add("name", projectName);
                parameters.Add("project_lead", projectLead);
                parameters.Add("expected_start_date", expectedStartDate);
                parameters.Add("expected_end_date", expectedEndDate);
                parameters.Add("earliest_task_start_date", earliestTaskStartDate);
                parameters.Add("latest_task_end_date", latestTeaskEndDate);
                parameters.Add("status", status);
                parameters.Add("define_done", (!string.IsNullOrEmpty(defineDone) ? defineDone : ""));
                parameters.Add("task_est_hours", taskEstHours);
                parameters.Add("task_act_hours", taskActHours);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("project", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddSprint(long boardId, DateTime startDate, DateTime endDate, int sequence, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("s_board_id", boardId);
                parameters.Add("s_start_date", startDate);
                parameters.Add("s_end_date", endDate);
                parameters.Add("s_sequence", sequence);
                parameters.Add("s_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_sprint", parameters);
            }
            else
            {
                parameters.Add("board_id", boardId);
                parameters.Add("start_date", startDate);
                parameters.Add("end_date", endDate);
                parameters.Add("sequence", sequence);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("sprint", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddStatusCode(long boardSetId, string columnHeading, string status, int sequence, long userId, string foreColor, string backColor)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("s_board_set_id", boardSetId);
                parameters.Add("s_column_heading", columnHeading);
                parameters.Add("s_status", status);
                parameters.Add("s_sequence", sequence);
                parameters.Add("s_user_id", userId);
                parameters.Add("s_fore_color", foreColor);
                parameters.Add("s_back_color", backColor);
                ds = GetDataViaStoredProcedure("sp_add_status_code", parameters);
            }
            else
            {
                parameters.Add("board_set_id", boardSetId);
                parameters.Add("column_heading", columnHeading);
                parameters.Add("status", status);
                parameters.Add("sequence", sequence);
                parameters.Add("user_id", userId);
                parameters.Add("fore_color", foreColor);
                parameters.Add("back_color", backColor);
                ds = ExecuteSharePointRequest("board_set_status", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddTag(long taskId, string tag, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_task_id", taskId);
                parameters.Add("t_tag", tag);
                parameters.Add("t_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_task_tag", parameters);
            }
            else
            {
                parameters.Add("task_id", taskId);
                parameters.Add("tag", tag);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("task_tag", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddComment(long taskId, string comment, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_task_id", taskId);
                parameters.Add("t_comment", comment);
                parameters.Add("t_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_task_comment", parameters);
            }
            else
            {
                parameters.Add("task_id", taskId);
                parameters.Add("comment", comment);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("task_comment", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddTask(
            long projectId,
            string taskName,
            DateTime startDate,
            DateTime endDate,
            long status,
            string defineDone,
            double estHours,
            double actHours,
            long parentTaskId,
            double subTaskEstHours,
            double subTaskActHours, 
            int sequence,
            long userId
)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_project_id", projectId);
                parameters.Add("t_name", taskName.Length > 100 ? taskName.Substring(0, 100) : taskName);
                parameters.Add("t_start_date", startDate);
                parameters.Add("t_end_date", endDate);
                parameters.Add("t_status_id", status);
                parameters.Add("t_define_done", (!string.IsNullOrEmpty(defineDone)) ? defineDone : "");
                parameters.Add("t_est_hours", estHours);
                parameters.Add("t_act_hours", actHours);
                parameters.Add("t_parent_task_id", parentTaskId);
                parameters.Add("t_sub_task_est_hours", subTaskEstHours);
                parameters.Add("t_sub_task_act_hours", subTaskEstHours);
                parameters.Add("t_sequence", sequence);
                parameters.Add("t_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_task", parameters);
            }
            else
            {
                parameters.Add("project_id", projectId);
                parameters.Add("name", taskName.Length > 100 ? taskName.Substring(0, 100) : taskName);
                parameters.Add("start_date", startDate);
                parameters.Add("end_date", endDate);
                parameters.Add("status_id", status);
                parameters.Add("define_done", (!string.IsNullOrEmpty(defineDone)) ? defineDone : "");
                parameters.Add("est_hours", estHours);
                parameters.Add("act_hours", actHours);
                parameters.Add("parent_task_id", parentTaskId);
                parameters.Add("sub_task_est_hours", subTaskEstHours);
                parameters.Add("sub_task_act_hours", subTaskEstHours);
                parameters.Add("sequence", sequence);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("task", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddProjectToBoard(long boardId, long projectId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_board_id", boardId);
                parameters.Add("p_project_id", projectId);
                ds = GetDataViaStoredProcedure("sp_add_project_to_board", parameters);
            }
            else
            {
                parameters.Add("board_id", boardId);
                parameters.Add("project_id", projectId);
                ds = ExecuteSharePointRequest("board_project", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddPropertyToObject(string name, object value, string parent_type, long parent_id, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            object val = (value == null ? "" : value);

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_name", name);
                parameters.Add("p_value", val);
                parameters.Add("p_value_type", val.GetType().ToString());
                parameters.Add("p_parent_type", parent_type);
                parameters.Add("p_parent_id", parent_id);
                parameters.Add("p_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_add_property", parameters);
            }
            else
            {
                parameters.Add("name", name);
                parameters.Add("value", val);
                parameters.Add("value_type", val.GetType().ToString());
                parameters.Add("parent_type", parent_type);
                parameters.Add("parent_id", parent_id);
                parameters.Add("user_id", userId);
                ds = ExecuteSharePointRequest("property", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        public static DataSet AddSprintToBoard(long boardId, long sprintId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_board_id", boardId);
                parameters.Add("p_sprint_id", sprintId);
                ds = GetDataViaStoredProcedure("sp_add_sprint_to_board", parameters);
            }
            else
            {
                parameters.Add("board_id", boardId);
                parameters.Add("sprint_id", sprintId);
                ds = ExecuteSharePointRequest("board_sprint", SharePointOperation.Add, parameters);
            }
            return ds;
        }

        /// <summary>
        /// Remove an approver from a task
        /// </summary>
        /// <param name="taskId">ID# of task to remove approver from</param>
        /// <param name="personId">ID# of approver being removed</param>
        /// <param name="userId">ID# of user removing the approver</param>
        public static void DeleteApproverFromTask(long taskId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_person_id", personId);
            SetDataViaStoredProcedure("sp_delete_approver_from_task", parameters);
        }

        /// <summary>
        /// Remove an assignee from a task
        /// </summary>
        /// <param name="taskId">ID# of task to remove assignee from</param>
        /// <param name="personId">ID# of assignee being removed</param>
        /// <param name="userId">ID# of user removing the assignee</param>
        public static void DeleteAssigneeFromTask(long taskId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_person_id", personId);
            SetDataViaStoredProcedure("sp_delete_assignee_from_task", parameters);
        }

        public static void DeleteBoard(long boardId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("b_board_id", boardId);
                SetDataViaStoredProcedure("sp_delete_board", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + boardId.ToString() + "</Value></Eq></Where>";
                ExecuteSharePointRequest("board", SharePointOperation.Delete, parameters, caml);
            }
        }

        public static void DeleteBoardSet(long boardSetId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("board_set_id", boardSetId); 
                SetDataViaStoredProcedure("sp_delete_board_set", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + boardSetId.ToString() + "</Value></Eq></Where>";
                ExecuteSharePointRequest("board_set", SharePointOperation.Delete, parameters, caml);
            }
        }

        public static void DeletePerson(long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_person_id", personId);
            SetDataViaStoredProcedure("sp_delete_person", parameters);
        }

        public static void DeleteProject(long projectId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_project_id", projectId);
                SetDataViaStoredProcedure("sp_delete_project", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + projectId.ToString() + "</Value></Eq></Where>";
                ExecuteSharePointRequest("project", SharePointOperation.Delete, parameters, caml);
            }
        }

        public static void DeleteProperty(long propertyId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_property_id", propertyId);
            SetDataViaStoredProcedure("sp_delete_property", parameters);
        }

        public static void DeleteSprint(long sprintId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("s_id", sprintId);
            SetDataViaStoredProcedure("sp_delete_sprint", parameters);
        }

        public static void DeleteStatusCode(long statusCodeId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("status_code_id", statusCodeId);
            SetDataViaStoredProcedure("sp_delete_status_code", parameters);
        }

        public static void DeleteStakeholderFromProject(long projectId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_project_id", projectId);
            parameters.Add("p_person_id", personId);
            SetDataViaStoredProcedure("sp_delete_stakeholder_from_project", parameters);
        }

        public static void DeleteUserFromBoard(long boardId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_board_id", boardId);
            parameters.Add("p_person_id", personId);
            SetDataViaStoredProcedure("sp_delete_user_from_board", parameters);
        }

        public static void DeleteTask(long taskId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            SetDataViaStoredProcedure("sp_delete_task", parameters);
        }

        public static void DeleteTaskTag(long taskId, string tag, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_tag", tag);
            SetDataViaStoredProcedure("sp_delete_task_tag", parameters);
        }

        public static void DeleteTaskComment(long taskCommentId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_comment_id", taskCommentId);
            SetDataViaStoredProcedure("sp_delete_task_comment", parameters);
        }

        /// <summary>
        /// Return a DataSet containing all data for a given approver
        /// </summary>
        /// <param name="approverId">ID# of approver to return</param>
        /// <param name="userId">ID# of user requesting this data</param>
        /// <returns></returns>
        public static DataSet GetApprover(long approverId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("approver_id", approverId);
                ds = GetDataViaStoredProcedure("sp_get_approver", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + approverId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("task_approver", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        /// <summary>
        /// Return a DataSet containing all approvers for a given task
        /// </summary>
        /// <param name="taskId">ID# of task to return approvers for</param>
        /// <param name="userId">ID# of user making request</param>
        /// <returns></returns>
        public static DataSet GetApproversByTask(long taskId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_task_id", taskId);
                ds = GetDataViaStoredProcedure("sp_get_approvers_by_task", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='task_id' /><Value Type='Text'>" + taskId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("task_approver", SharePointOperation.Get, parameters, caml);
            }

            return ds;
        }

        /// <summary>
        /// Return a DataSet containing all assignees for a given task
        /// </summary>
        /// <param name="taskId">ID# of task to return assignees for</param>
        /// <param name="userId">ID# of user making request</param>
        /// <returns></returns>
        public static DataSet GetAssigneesByTask(long taskId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("t_task_id", taskId);
                ds = GetDataViaStoredProcedure("sp_get_assignees_by_task", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='task_id' /><Value Type='Text'>" + taskId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("task_assignee", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetBoardById(long id, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("board_id", id);
                parameters.Add("b_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_get_board", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + id.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("board", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetBoardsByBoardSet(long boardSetId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("b_board_set_id", boardSetId);
                parameters.Add("b_user_id", userId); 
                ds = GetDataViaStoredProcedure("sp_get_boards_by_boardset_id", parameters);
            }
            else
            {
                string caml =  "<Where><Eq><FieldRef Name='board_set_id' /><Value Type='Text'>" + boardSetId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("board", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetBoardsByName(string nameFilter, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("name_filter", nameFilter);
                parameters.Add("b_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_get_boards_by_name", parameters);
            }
            else
            {
                string caml = "<Where><Contains><FieldRef Name='name' /><Value Type='Text'>" + nameFilter + "</Value></Contains></Where>";
                ds = ExecuteSharePointRequest("board", SharePointOperation.Get, parameters, caml);
            }

            return ds;
        }

        public static DataSet GetBoardSetById(long boardSetId, long userId)
        {
            DataSet ds = new DataSet();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("board_set_id", boardSetId); 
                ds = GetDataViaStoredProcedure("sp_get_board_set", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + boardSetId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("board_set", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetBoardSetsByName(string nameFilter, long userId)
        {
            DataSet ds = new DataSet();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("name_filter", nameFilter);
                ds = GetDataViaStoredProcedure("sp_get_board_sets_by_name", parameters);
            }
            else
            {
                string caml = "<Where><Contains><FieldRef Name='name' /><Value Type='Text'>" + nameFilter + "</Value></Contains></Where>";
                ds = ExecuteSharePointRequest("board_set", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetBoardsByUserId(long userId)
        {
            DataSet ds = new DataSet();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("b_user_id", userId);
                ds = GetDataViaStoredProcedure("sp_get_boards_by_user_id", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='person_id' /><Value Type='Text'>" + userId.ToString() + "</Value></Eq></Where>";
                DataSet dsUserBoards = ExecuteSharePointRequest("board_person", SharePointOperation.Get, parameters, caml);
                caml = "";
                DataSet dsBoards = ExecuteSharePointRequest("board", SharePointOperation.Get, parameters, caml);
                for (int i = dsBoards.Tables[0].Rows.Count - 1; i >= 0 ; i--)
                {
                    if (dsUserBoards.Tables[0].Select("board_id=" + dsBoards.Tables[0].Rows[i]["board_id"].ToString() + " AND user_id=" + userId.ToString()).Length == 0)
                    {
                        dsBoards.Tables[0].Rows[i].Delete();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// Call a MySql stored procedure that returns data
        /// </summary>
        /// <param name="sql">SQL statement to execute</param>
        /// <returns>DataSet containing results</returns>
        private static DataSet GetData(string sql)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(MySqlConnectionString);
                MySqlCommand comm = new MySqlCommand();

                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Close();
                con.Dispose();
                ds.Tables[0].TableName = "results";
                return ds;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Run a given stored procedure against the database.  This is a better alternative in order to avoid
        /// SQL injection attacks
        /// </summary>
        /// <param name="ProcedureName">Name of stored procedure to run</param>
        /// <param name="parameters">Dictionary of key/value pairs representing the stored procedure parameters</param>
        /// <returns>DataSet containing results</returns>
        public static DataSet GetDataViaStoredProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            DataSet ds = new DataSet();

            switch (_databaseType)
            {
                case DbType.MySql:

                    MySqlConnection con = new MySqlConnection(MySqlConnectionString);
                    MySqlCommand cmd = new MySqlCommand(procedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
                    }

                    MySqlDataAdapter resultDA = new MySqlDataAdapter();
                    resultDA.SelectCommand = cmd;
                    resultDA.SelectCommand.Connection = con;

                    con.Open();
                    try
                    {
                        resultDA.Fill(ds);
                        if (ds.Tables.Count > 0) ds.Tables[0].TableName = "results";
                    }
                    catch { }
                    finally
                    {
                        con.Close();
                    }
                    break;

                case DbType.SqlServer:

                    SqlConnection sqlCon = new SqlConnection(SqlServerConnectionString);
                    SqlCommand sqlCmd = new SqlCommand(procedureName, sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        sqlCmd.Parameters.AddWithValue("@" + param.Key.ToString(), param.Value);
                    }

                    SqlDataAdapter sqlDa = new SqlDataAdapter();
                    sqlDa.SelectCommand = sqlCmd;
                    sqlDa.SelectCommand.Connection = sqlCon;

                    sqlCon.Open();
                    try
                    {
                        sqlDa.Fill(ds);
                        if (ds.Tables.Count > 0) ds.Tables[0].TableName = "results";
                    }
                    catch { }
                    finally
                    {
                        sqlCon.Close();
                    }
                    break;
                default:
                    break;
            }

            return ds;
        }

        public static string GetJson(object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MaxDepth = 1;
            return JsonConvert.SerializeObject(obj, obj.GetType(), settings);
        }

        public static DataSet GetPeopleByBoard(long boardId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("b_board_id", boardId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_people_by_board", parameters);
            return ds;
        }

        public static DataSet GetStakeholdersByProject(long projectId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_project_id", projectId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_people_by_project", parameters);
            return ds;
        }

        public static DataSet GetPeopleByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_people_by_name", parameters);
            // If data was returned, make sure it's sorted by name
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0].Select("", "name").CopyToDataTable();
                dt.TableName = "results";
                ds.Tables.Clear();
                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary>
        /// Retrieve person table data by ID#
        /// </summary>
        /// <param name="id">ID# of person to look up</param>
        /// <param name="userId">ID# of user making the request</param>
        /// <returns>DataSet containing found data</returns>
        public static DataSet GetPersonById(long id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("person_id", id);
            DataSet ds = GetDataViaStoredProcedure("sp_get_person", parameters);
            return ds;
        }

        /// <summary>
        /// Retrieve person table data
        /// </summary>
        /// <param name="personUserName">User name of person to return</param>
        /// <param name="userId">ID# of user making the request</param>
        /// <returns>DataSet containing found data</returns>
        /// <example>
        /// <code>Person fred = new Person("fflintstone", TestCredential);</code>
        /// </example>
        public static DataSet GetPersonByUserName(string personUserName, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_user_name", personUserName);
            DataSet ds = GetDataViaStoredProcedure("sp_get_person_by_user_name", parameters);
            return ds;
        }

        public static DataSet GetProjectById(long id, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("project_id", id);
                ds = GetDataViaStoredProcedure("sp_get_project", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + id.ToString() + "</Value></Eq></Where></Query>";
                ds = ExecuteSharePointRequest("project", SharePointOperation.Get, parameters, caml);
            }

            return ds;
        }

        public static DataSet GetPropertyById(long id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_property_id", id);
            DataSet ds = GetDataViaStoredProcedure("sp_get_property", parameters);
            return ds;
        }

        public static DataSet GetPropertiesByObject(string parent_type, long parent_id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_parent_type", parent_type);
            parameters.Add("p_parent_id", parent_id);
            parameters.Add("p_user_id", userId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_properties_by_object", parameters);
            return ds;
        }

        public static DataSet GetProjectsByBoard(long boardId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("b_board_id", boardId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_projects_by_board", parameters);
            return ds;
        }

        public static DataSet GetProjectsByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_projects_by_name", parameters);
            return ds;
        }

        public static DataSet GetSprintsByBoard(long boardId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("b_board_id", boardId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_sprints_by_board", parameters);
            return ds;
        }

        public static DataSet GetSprintById(long id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("sprint_id", id);
            DataSet ds = GetDataViaStoredProcedure("sp_get_sprint", parameters);
            return ds;
        }

        public static DataSet GetStatusCodeById(long id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("status_code_id", id);
            DataSet ds = GetDataViaStoredProcedure("sp_get_status_code", parameters);
            return ds;
        }

        public static DataSet GetStatusCodesByBoardSet(long boardSetId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("b_board_set_id", boardSetId);
            if (DatabaseType != DbType.SharePoint)
            {
                ds = GetDataViaStoredProcedure("sp_get_status_codes_by_board_set", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='board_set_id' /><Value Type='Text'>" + boardSetId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("board_set_status", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        public static DataSet GetStatusCodesByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_status_codes_by_name", parameters);
            return ds;
        }

        public static DataSet GetTagById(long taskTagId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_tag_id", taskTagId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_task_tag", parameters);
            return ds;
        }

        public static DataSet GetCommentById(long taskCommentId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_comment_id", taskCommentId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_task_comment", parameters);
            return ds;
        }

        public static DataSet GetTagsByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_tags_by_name", parameters);
            return ds;
        }

        public static DataSet GetCommentsByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_task_comments_by_name", parameters);
            return ds;
        }

        public static DataSet GetTagsByTask(long taskId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_tags_by_task", parameters);
            return ds;
        }

        public static DataSet GetCommentsByTask(long taskId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_comments_by_task", parameters);
            return ds;
        }

        public static DataSet GetTaskById(long id, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("task_id", id);
            DataSet ds = GetDataViaStoredProcedure("sp_get_task", parameters);
            return ds;
        }

        public static DataSet GetTasksByBoard(long boardId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_board_id", boardId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_tasks_by_board", parameters);
            return ds;
        }

        public static DataSet GetTasksByName(string nameFilter, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name_filter", nameFilter);
            DataSet ds = GetDataViaStoredProcedure("sp_get_tasks_by_name", parameters);
            return ds;
        }

        public static DataSet GetTasksByProject(long projectId, long userId)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (DatabaseType != DbType.SharePoint)
            {
                parameters.Add("p_project_id", projectId);
                ds = GetDataViaStoredProcedure("sp_get_tasks_by_project", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='project_id' /><Value Type='Text'>" + projectId.ToString() + "</Value></Eq></Where>";
                ds = ExecuteSharePointRequest("task", SharePointOperation.Get, parameters, caml);
            }
            return ds;
        }

        //
        public static DataSet GetSubTasksForTask(long taskId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_parent_task_id", taskId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_subtasks_for_task", parameters);
            return ds;
        }

        public static DataSet GetStatusValuesForBoard(long boardId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("board_id", boardId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_board_status_codes", parameters);
            return ds;
        }

        public static DataSet GetStatusValuesForBoardSet(long boardSetId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("b_board_set_id", boardSetId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_board_set_status_codes", parameters);
            return ds;
        }

        public static DataSet GetBoardUser(long boardUserId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("user_id", boardUserId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_board_user", parameters);
            return ds;
        }

        public static DataSet GetBoardUser(long boardId, long personId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("u_board_id", boardId);
            parameters.Add("u_person_id", personId);
            DataSet ds = GetDataViaStoredProcedure("sp_get_board_user_by_board_person", parameters);
            return ds;
        }

        public static long Login(string userName, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_user_name", userName);
            parameters.Add("p_password", EncDec.Encrypt(password, encryptionKey));
            DataSet ds = GetDataViaStoredProcedure("sp_login", parameters);
            try
            {
                return long.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Call MySql and run the provided SQL
        /// </summary>
        /// <param name="sql">SQL to execute on MySql</param>
        public static void SetData(string sql)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(MySqlConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
            catch { }
        }

        private static bool isValidField(string listName, string fieldName)
        {
            switch (listName)
            {
                case "board_set":
                    return ("id,name").Contains(fieldName);
                default:
                    return false;
                    break;
            }
        }

        public static DataSet ExecuteSharePointRequest(string listName, SharePointOperation operation, Dictionary<string, object> parameters, string caml = null)
        {
            DataSet ds = new DataSet("Results");
            DataTable dt = new DataTable("Results");
            ds.Tables.Add(dt);
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("created");
            dt.Columns.Add("created_by");
            dt.Columns.Add("modified");
            dt.Columns.Add("modified_by");

            ClientContext context = new ClientContext(SharePointSiteUrl);
            context.Credentials = new System.Net.NetworkCredential("svcsharepointdb", "3uj1J@c0b", "fwnt");
            List list = context.Web.Lists.GetByTitle(listName);

            switch (operation)
            {
                case SharePointOperation.Add:

                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                    ListItem item = list.AddItem(itemCreateInfo);
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        if (isValidField(listName, parameter.Key))
                        {
                            item[parameter.Key] = parameter.Value;
                            if (!dt.Columns.Contains(parameter.Key)) dt.Columns.Add(parameter.Key);
                        }
                        if (parameter.Key == "name") item["Title"] = parameter.Value;
                    }
                    item.Update();
                    context.ExecuteQuery();
                    context.Load(item);
                    context.ExecuteQuery();

                    DataRow dr = dt.NewRow();
                    foreach (KeyValuePair<string, object> field in item.FieldValues)
                    {
                        if (!dt.Columns.Contains(field.Key)) dt.Columns.Add(field.Key);
                        dr[field.Key] = item[field.Key];
                    }

                    dr["id"] = item.Id;
                    dr["name"] = item["Title"];
                    dr["created"] = item["Created"];
                    dr["created_by"] = item["Author"];
                    dr["modified"] = item["Modified"];
                    dr["modified_by"] = item["Editor"];
                    dt.Rows.Add(dr);

                    break;

                case SharePointOperation.Delete:

                    CamlQuery query = CamlQuery.CreateAllItemsQuery(5000);
                    query.ViewXml = "<View><Query>" + caml + "</Query></View>";
                    Microsoft.SharePoint.Client.ListItemCollection items = list.GetItems(query);
                    context.Load(items);
                    context.ExecuteQuery();

                    if (items.Count > 0)
                    {
                        item = items[0];
                        item.DeleteObject();
                        context.ExecuteQuery();
                    }
                    break;

                case SharePointOperation.Get:
                                        
                    query = CamlQuery.CreateAllItemsQuery(5000);
                    query.ViewXml = "<View><Query>" + caml + "</Query></View>";
                    items = list.GetItems(query);
                    context.Load(items);
                    context.ExecuteQuery();

                    foreach (ListItem item0 in items)
                    {
                        dr = dt.NewRow();
                        foreach (KeyValuePair<string, object> field in item0.FieldValues)
                        {
                            if (!dt.Columns.Contains(field.Key)) dt.Columns.Add(field.Key);
                            dr[field.Key] = item0[field.Key];
                        }
                        dr["id"] = item0.Id;
                        dr["name"] = item0["Title"];
                        dr["created"] = item0["Created"];
                        dr["created_by"] = item0["Author"];
                        dr["modified"] = item0["Modified"];
                        dr["modified_by"] = item0["Editor"];
                        dt.Rows.Add(dr);
                    }

                    break;

                case SharePointOperation.Update:

                    query = CamlQuery.CreateAllItemsQuery(5000);
                    query.ViewXml = "<View><Query>" + caml + "</Query></View>";
                    items = list.GetItems(query);
                    context.Load(items);
                    context.ExecuteQuery();

                    if (items.Count > 0)
                    {
                        item = items[0];
                        foreach (KeyValuePair<string, object> parameter in parameters)
                        {
                            if (isValidField(listName, parameter.Key))
                            {
                                item[parameter.Key] = parameter.Value;
                                if (!dt.Columns.Contains(parameter.Key)) dt.Columns.Add(parameter.Key);
                            }
                            if (parameter.Key == "name") item["Title"] = parameter.Value;
                        }
                        item.Update();
                        context.ExecuteQuery();
                        context.Load(item);
                        context.ExecuteQuery();

                        dr = dt.NewRow();
                        foreach (KeyValuePair<string, object> parameter in parameters)
                        {
                            if (item.FieldValues.ContainsKey(parameter.Key))
                            {
                                dr[parameter.Key] = item[parameter.Key];
                            }
                        }
                        dr["id"] = item.Id;
                        dr["name"] = item["Title"];
                        dr["created"] = item["Created"];
                        dr["created_by"] = item["Author"];
                        dr["modified"] = item["Modified"];
                        dr["modified_by"] = item["Editor"];
                        dt.Rows.Add(dr);
                    }

                    break;

                default:
                    break;
            }
            return ds;
        }

        /// <summary>
        /// Write data to database without returning any data
        /// </summary>
        /// <param name="procedureName">Stored procedure to call</param>
        /// <param name="parameters">Dictionary of key/value pairs representing the parameters of the stored procedure</param>
        public static void SetDataViaStoredProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            switch (_databaseType)
            {
                case DbType.MySql:

                    MySqlConnection con = new MySqlConnection(MySqlConnectionString);

                    MySqlCommand cmd = new MySqlCommand(procedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
                    }

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        con.Close();
                    }
                    break;

                case DbType.SqlServer:

                    SqlConnection sqlCon = new SqlConnection(SqlServerConnectionString);

                    SqlCommand sqlCmd = new SqlCommand(procedureName, sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        sqlCmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
                    }

                    sqlCon.Open();
                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                    break;

                default:
                    break;
            }
        }

        public static void UpdateBoard(long boardId, string boardName, long boardSetId, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("board_id", boardId);
            parameters.Add("board_name", boardName);
            parameters.Add("b_board_set_id", boardSetId);
            parameters.Add("b_user_id", userId);
            SetDataViaStoredProcedure("sp_update_board", parameters);
        }

        public static DataSet UpdateBoardSet(long boardSetId, string boardSetName, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("board_set_id", boardSetId);
            parameters.Add("board_set_name", boardSetName);
            parameters.Add("bs_user_id", userId);
            if (DatabaseType != DbType.SharePoint)
            {
                return GetDataViaStoredProcedure("sp_update_board_set", parameters);
            }
            else
            {
                string caml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + boardSetId.ToString() + "</Value></Eq></Where>";
                return ExecuteSharePointRequest("board_set", SharePointOperation.Update, parameters, caml);
            }
        }

        public static void UpdatePerson(
            long personId,
            string personName,
            string pictureUrl,
            string email,
            string phone,
            string userName,
            string password,
            bool canLogin, 
            long userId
        )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("person_id", personId);
            parameters.Add("person_name", personName);
            parameters.Add("person_picture_url", pictureUrl);
            parameters.Add("person_email", email);
            parameters.Add("person_phone", phone);
            parameters.Add("person_user_name", userName);
            parameters.Add("person_password", password);
            parameters.Add("person_can_login", canLogin);
            parameters.Add("p_user_id", userId);

            SetDataViaStoredProcedure("sp_update_person", parameters);
        }

        public static void UpdateProject(
                long projectId, 
                string projectName, 
                long projectLead,
                DateTime expectedStartDate,
                DateTime expectedEndDate,
                DateTime earliestTaskStartDate,
                DateTime latestTeaskEndDate,
                long status,
                string defineDone,
                double taskEstHours,
                double taskActHours, 
                long userId
            )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("project_id", projectId);
            parameters.Add("p_name", projectName.Length > 100 ? projectName.Substring(0,100) : projectName);
            parameters.Add("p_project_lead", projectLead);
            parameters.Add("p_expected_start_date", expectedStartDate);
            parameters.Add("p_expected_end_date", expectedEndDate);
            parameters.Add("p_earliest_task_start_date", earliestTaskStartDate);
            parameters.Add("p_latest_task_end_date", latestTeaskEndDate);
            parameters.Add("p_status", status);
            parameters.Add("p_define_done", (!string.IsNullOrEmpty(defineDone) ? defineDone : ""));
            parameters.Add("p_task_est_hours", taskEstHours);
            parameters.Add("p_task_act_hours", taskActHours);
            parameters.Add("p_user_id", userId);

            SetDataViaStoredProcedure("sp_update_project", parameters);
        }

        public static DataSet UpdateProperty(long propertyId, string name, object value, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_property_id", propertyId);
            parameters.Add("p_name", name.Length > 100 ? name.Substring(0, 100) : name);
            parameters.Add("p_value", value);
            parameters.Add("p_value_type", value.GetType().ToString());
            parameters.Add("p_user_id", userId);

            return GetDataViaStoredProcedure("sp_update_property", parameters);
        }

        public static void UpdateSprint(long sprintId, long boardId, DateTime startDate, DateTime endDate, int sequence, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("s_id", sprintId);
            parameters.Add("s_board_id", boardId);
            parameters.Add("s_start_date", startDate);
            parameters.Add("s_end_date", endDate);
            parameters.Add("s_sequence", sequence);
            parameters.Add("s_user_id", userId);
            SetDataViaStoredProcedure("sp_update_sprint", parameters);
        }

        public static DataSet UpdateComment(long taskId, long commentId, string comment, long userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_comment_id", commentId);
            parameters.Add("t_comment", comment);
            parameters.Add("t_user_id", userId);
            DataSet ds = GetDataViaStoredProcedure("sp_update_comment", parameters);
            return ds;
        }

        public static DataSet UpdateStatusCode(long boardSetId, long statusCodeId, string columnHeading, string status, int sequence, long userId, string foreColor, string backColor)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("s_id", statusCodeId);
            parameters.Add("s_board_set_id", boardSetId);
            parameters.Add("s_column_heading", columnHeading);
            parameters.Add("s_status", status);
            parameters.Add("s_sequence", sequence);
            parameters.Add("s_user_id", userId);
            parameters.Add("s_fore_color", foreColor);
            parameters.Add("s_back_color", backColor);
            DataSet ds = GetDataViaStoredProcedure("sp_update_status_code", parameters);
            return ds;
        }

        public static void UpdateTask(
            long projectId,
            long taskId,
            string taskName,
            DateTime startDate,
            DateTime endDate,
            long status,
            string defineDone,
            double estHours,
            double actHours,
            long parentTaskId,
            double subTaskEstHours,
            double subTaskActHours, 
            int sequence,
            long userId
        )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("t_task_id", taskId);
            parameters.Add("t_project_id", projectId);
            parameters.Add("t_name", taskName.Length > 100 ? taskName.Substring(0,100) : taskName);
            parameters.Add("t_start_date", startDate);
            parameters.Add("t_end_date", endDate);
            parameters.Add("t_define_done", (!string.IsNullOrEmpty(defineDone) ? defineDone : ""));
            parameters.Add("t_status_id", status);
            parameters.Add("t_est_hours", estHours);
            parameters.Add("t_act_hours", actHours);
            parameters.Add("t_parent_task_id", parentTaskId);
            parameters.Add("t_sub_task_est_hours", subTaskEstHours);
            parameters.Add("t_sub_task_act_hours", subTaskActHours);
            parameters.Add("t_sequence", sequence);
            parameters.Add("t_user_id", userId);

            SetDataViaStoredProcedure("sp_update_task", parameters);
        }
    }
}
