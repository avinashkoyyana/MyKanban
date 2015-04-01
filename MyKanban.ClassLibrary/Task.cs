using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
 *  Class:      Task
 *  
 *  Purpose:    Represents a single task being tracked by the MyKanban system
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single task being tracked by the MyKanban system
    /// </summary>
    public class Task : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new Task instance
        /// </summary>
        /// <param name="credential">Credentials to use when creating this Task instance</param>
        public Task(Credential credential) 
        {
            if (credential != null) _credential = credential;

            _subTasks = new Tasks(_credential);
            _tags = new Tags(_credential);
            _comments = new Comments(_credential);
        }

        /// <summary>
        /// Create a new Task object and initialize its Name property
        /// </summary>
        /// <param name="name">Name to assign to new task</param>
        /// <param name="credential">Credentials to use when creating this Task instance</param>
        public Task(string name, Credential credential)
        {
            if (credential != null) _credential = credential;

            _name = name;
            _subTasks = new Tasks(this, _credential);
            _tags = new Tags(this, _credential);
            _comments = new Comments(this, _credential);
        }

        /// <summary>
        /// Create a new Task instance and load its data from the database using
        /// the provided ID#
        /// </summary>
        /// <param name="taskId">ID# of task to load</param>
        /// <param name="credential">Credentials to use when creating this Task instance</param>
        public Task(long taskId, Credential credential)
        {
            TaskIdConstructor(taskId, credential);
        }

        private void TaskIdConstructor(long taskId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = taskId;
            _assignees = null;
            _approvers = null;
            _properties = null;
            _subTasks = null;
            _comments = null;
            LoadData(true);
        }

        #endregion

        #region Properties

        private double _actHours;

        /// <summary>
        /// Actual hours required to complete this task
        /// </summary>
        [MyKanban.Description("Actual hours required to complete this task")]
        [MyKanban.ControlType(enumControlType.Numeric)]
        public double ActHours
        {
            get 
            {
                if (SubTasks.Count > 0)
                {
                    _actHours = 0;
                    foreach (Task subTask in SubTasks.Items)
                    {
                        _actHours += subTask.ActHours;
                    }
                }
                return Math.Round(_actHours,2); 
            }
            set
            {
                _actHours = value;
                _isDirty = true;
            }
        }

        private string _approvedBy = null;

        [MyKanban.Description("List of individuals who will approve this task")]
        public string ApprovedBy
        {
            get
            {
                if (_approvedBy == null || _isDirty)
                {
                    _approvedBy = "";
                    foreach (Approver approver in Approvers.Items)
                    {
                        if (!string.IsNullOrEmpty(_approvedBy)) _approvedBy += ", ";
                        _approvedBy += approver.Name;
                    }
                }
                return _approvedBy;
            }
        }

        private string _assignedTo = null;

        /// <summary>
        /// List of individuals this task is assigned to
        /// </summary>
        [MyKanban.Description("List of individuals this task is assigned to")]
        public string AssignedTo
        {
            get 
            { 
                if (_assignedTo == null || _isDirty)
                {
                    _assignedTo = "";
                    foreach (Person assignee in Assignees.Items)
                    {
                        if (!string.IsNullOrEmpty(_assignedTo)) _assignedTo += ", ";
                        _assignedTo += assignee.Name;
                    }
                }
                return _assignedTo; 
            }
        }

        private Approvers _approvers;

        /// <summary>
        /// Approvers associated with this task
        /// </summary>
        [MyKanban.Hidden(true)]
        public Approvers Approvers
        {
            get 
            {
                if (_approvers == null)
                {
                    if (_id > 0)
                    {
                        _approvers = new Approvers(this, _credential);
                        _approvers.Parent = this;
                        _approvers.ParentId = _id;
                    }
                    else
                    {
                        _approvers = new Approvers(_credential);
                        _approvers.Parent = this;
                    }
                }

                return _approvers; 
            }
        }

        private People _assignees;

        /// <summary>
        /// Collection of individuals assigned to this task
        /// </summary>
        [MyKanban.Hidden(true)]
        public People Assignees
        {
            get 
            {
                if (_assignees == null)
                {
                    if (_id > 0)
                    {
                        _assignees = new People(this, _credential);
                    }
                    else
                    {
                        _assignees = new People(_credential);
                    }
                }
                return _assignees; 
            }
        }

        string _backColor = null;

        /// <summary>
        /// Background color to use when displaying this task
        /// </summary>
        [MyKanban.Description("Background color to use when displaying this task")]
        public string BackColor
        {
            get 
            {
                if (_backColor == null)
                {
                    _backColor = "white";
                    foreach (StatusCode statusCode in StatusCodes.Items)
                    {
                        if (statusCode.Id == _status)
                        {
                            _backColor = statusCode.BackColor;
                            break;
                        }
                    }
                }
                return _backColor; 
            }
        }

        private long _boardSetId = 0;

        /// <summary>
        /// ID# of board set this task belongs to
        /// </summary>
        [MyKanban.Description("ID# of board set this task belongs to")]
        [MyKanban.ReadOnly(true)]
        public long BoardSetId
        {
            get { return _boardSetId; }
            set { _boardSetId = value; }
        }

        private string _boardSetName = "";

        /// <summary>
        /// Name of board set this task belongs to
        /// </summary>
        [MyKanban.Description("Name of board set this task belongs to")]
        [MyKanban.ReadOnly(true)]
        public string BoardSetName
        {
            get { return _boardSetName; }
            set { _boardSetName = value; }
        }

        private string _defineDone;

        /// <summary>
        /// The expected deliverble(s) or outcome when task is complete
        /// </summary>
        [MyKanban.Description("The expected deliverble(s) or outcome when task is complete")]
        public string DefineDone
        {
            get { return _defineDone; }
            set { _defineDone = value; _isDirty = true; }
        }

        private int _sequence = 999;

        /// <summary>
        /// The ordinal position of this task when displayed on a board
        /// </summary>
        [MyKanban.Description("The ordinal position of this task when displayed on a board")]
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; _isDirty = true; }
        }

        private DateTime _startDate = DateTime.Now;

        /// <summary>
        /// Task start date
        /// </summary>
        [MyKanban.Description("Task start date")]
        public DateTime StartDate
        {
            get 
            { 
                return _startDate.Date; 
            }
            set 
            { 
                _startDate = (value > (new DateTime(1900,1,1))) ? value : (new DateTime(1900,1,1));
                _isDirty = true; 
            }
        }

        private double _estHours;

        /// <summary>
        /// Estimated hours to complete this task
        /// </summary>
        [MyKanban.Description("Estimated hours to complete this task")]
        [MyKanban.ControlType(enumControlType.Numeric)]
        public double EstHours
        {
            get 
            { 
                if (SubTasks.Count > 0)
                {
                    _estHours = 0;
                    foreach (Task subTask in SubTasks.Items)
                    {
                        _estHours += subTask.EstHours;
                    }
                }
                return Math.Round(_estHours,2); 
            }
            set
            {
                _estHours = value;
                _isDirty = true;
            }
        }

        private DateTime _endDate = DateTime.Now;

        /// <summary>
        /// Task end date
        /// </summary>
        [MyKanban.Description("Task end date")]
        public DateTime EndDate
        {
            get 
            { 
                return _endDate.Date; 
            }
            set 
            { 
                _endDate = (value > (new DateTime(1900,1,1))) ? value : (new DateTime(1900,1,1));
                _isDirty = true; 
            }
        }

        string _foreColor = null;

        /// <summary>
        /// The foreground color to use when displaying this task
        /// </summary>
        [MyKanban.Description("The foreground color to use when displaying this task")]
        public string ForeColor
        {
            get
            {
                if (_foreColor == null)
                {
                    _foreColor = "white";
                    foreach (StatusCode statusCode in StatusCodes.Items)
                    {
                        if (statusCode.Id == _status)
                        {
                            _foreColor = statusCode.ForeColor;
                            break;
                        }
                    }
                }
                return _foreColor;
            }
        }

        long _parentTaskId = 0;

        /// <summary>
        /// If this is a sub-task, the ID# of its parent task
        /// </summary>
        [MyKanban.Description("If this is a sub-task, the ID# of its parent task")]
        [MyKanban.ReadOnly(true)]
        public long ParentTaskId
        {
            get { return _parentTaskId; }
            set { _parentTaskId = value; _isDirty = true; }
        }

        long _projectId = 0;

        /// <summary>
        /// ID# of project this task belongs to
        /// </summary>
        [MyKanban.Description("ID# of project this task belongs to")]
        [MyKanban.ReadOnly(true)]
        public long ProjectId
        {
            get
            {
                // If project id not set, get it now
                if (_projectId == 0)
                {
                    if (_parentTaskId != 0)
                    {
                        Task parentTask = new Task(_parentTaskId, _credential);
                        _projectId = parentTask.ProjectId;
                        this.Update(true);
                    }
                }
                return _projectId;
            }
            set 
            { 
                _projectId = value; 
            }
        }

        private string _projectName = "";

        /// <summary>
        /// Display name of project this task belongs to
        /// </summary>
        [MyKanban.Description("Display name of project this task belongs to")]
        [MyKanban.ReadOnly(true)]
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        private Properties _properties = null;

        /// <summary>
        /// Collection of properties associated with this task
        /// </summary>
        [MyKanban.Description("Collection of properties associated with this task")]
        public Properties Properties
        {
            get
            {
                if (_properties == null)
                {
                    if (_id > 0)
                    {
                        _properties = new Properties(this, _credential);
                    }
                    else
                    {
                        _properties = new Properties(_credential);
                    }
                }
                _properties.Parent = this;
                return _properties;
            }
        }
        
        private long _status = -1;

        /// <summary>
        /// ID# of status code for this task
        /// </summary>
        [MyKanban.Description("ID# of status code for this task")]
        [MyKanban.ControlType(enumControlType.StatusCode)]
        public long Status
        {
            get 
            { 
                if (_status <= 0)
                {
                    if (StatusCodes.Count > 0)
                        _status = StatusCodes[0].Id;
                    else
                        _status = 0;
                }
                return _status; 
            }
            set { _status = value; _isDirty = true; }
        }

        /// <summary>
        /// Display name of current task status
        /// </summary>
        private string _statusName = "Unknown";
        [MyKanban.Description("Display name of current task status")]
        public string StatusName
        {
            get 
            {
                try
                {
                    if (_statusName == "Unknown")
                    {
                        foreach (StatusCode statusCode in StatusCodes.Items)
                        {
                            if (statusCode.Id == _status)
                            {
                                _statusName = statusCode.ColumnHeading;
                            }
                        }
                    }
                }
                catch
                {
                    _statusName = "Unknown";
                }
                return _statusName;
            }
        }

        private StatusCodes _statusCodes;

        /// <summary>
        /// List of StatusCodes that may be assigned to this task
        /// </summary>
        [JsonIgnore]
        [MyKanban.Hidden(true)]
        [MyKanban.ReadOnly(true)]
        public StatusCodes StatusCodes
        {
            get 
            {
                if (_statusCodes == null)
                {
                    if (_parent == null)
                    {
                        BoardSet boardSet = new BoardSet(_boardSetId, _credential);
                        _statusCodes = new StatusCodes(boardSet, _credential);
                    }
                    else if (_parent.GetType() == typeof(MyKanban.Project))
                    {
                        _statusCodes = ((Project)_parent).StatusCodes;
                    }
                    else if (_parent.GetType() == typeof(MyKanban.Task))
                    {
                        _statusCodes = ((Task)_parent).StatusCodes;
                    }
                    else
                    {
                        BoardSet boardSet = new BoardSet(_boardSetId, _credential);
                        _statusCodes = new StatusCodes(boardSet, _credential);
                    }
                }
                return _statusCodes;
            }
        }

        double _subTaskActHours = 0;

        /// <summary>
        /// Total actual hours associated with all sub-tasks
        /// </summary>
        [MyKanban.Description("Total actual hours associated with all sub-tasks")]
        [MyKanban.ReadOnly(true)]
        public double SubTaskActHours
        {
            get { return _subTaskActHours; }
            set { _subTaskActHours = value; _isDirty = true; }
        }
        
        double _subTaskEstHours = 0;

        /// <summary>
        /// Total estimated hours associated with all sub-tasks
        /// </summary>
        [MyKanban.Description("Total estimated hours associated with all sub-tasks")]
        [MyKanban.ReadOnly(true)]
        public double SubTaskEstHours
        {
            get { return _subTaskEstHours; }
            set { _subTaskEstHours = value; _isDirty = true; }
        }

        private Tasks _subTasks;

        /// <summary>
        /// Collection of all sub-tasks for this task
        /// </summary>
        [JsonIgnore]
        [MyKanban.Hidden(true)]
        public Tasks SubTasks
        {
            get 
            {
                if (_subTasks == null)
                {
                    if (_id > 0)
                    {
                        _subTasks = new Tasks(this, _credential);
                    }
                    else
                    {
                        _subTasks = new Tasks(_credential);
                    }
                }

                _subTasks.ProjectId = _projectId;
                return _subTasks; 
            }
            set { _subTasks = value; }
        }

        private Comments _comments;

        /// <summary>
        /// Collection of all comments associated with this task
        /// </summary>
        [JsonIgnore]
        [MyKanban.Hidden(true)]
        public Comments Comments
        {
            get 
            { 
                if (_comments == null)
                {
                    if (_id > 0)
                    {
                        _comments = new Comments(this, _credential);
                    }
                    else
                    {
                        _comments = new Comments(_credential);
                    }
                }
                return _comments; 
            }
            set
            {
                _comments = value;
                _comments.TaskId = _id;
            }
        }

        private Tags _tags;

        /// <summary>
        /// Collection of all tags associated with this task
        /// </summary>
        [JsonIgnore]
        [MyKanban.Hidden(true)]
        public Tags Tags
        {
            get 
            { 
                if (_tags == null)
                {
                    if (_id > 0)
                    {
                        _tags = new Tags(this, _credential);
                    }
                    else
                    {
                        _tags = new Tags(_credential);
                    }
                }
                _tags.TaskId = _id;
                return _tags; 
            }
            set 
            { 
                _tags = value;
                _tags.TaskId = _id;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this task from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteTask(_id, _credential.Id);

            // Now delete any sub-tasks of this task
            foreach (Task subTask in SubTasks.Items)
            {
                subTask.Delete();
            }

            // Delete any properties associated with this task
            foreach (Property property in Properties.Items)
            {
                property.Delete();
            }
        }

        /// <summary>
        /// Does specified user have permission to perform the requested operation
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="authLevel">Requested operation</param>
        /// <returns>True if user has permission to perform the requested operation</returns>
        public override bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return base.IsAuthorized(userId, authLevel);
        }

        /// <summary>
        /// Is this Task object in a valid state
        /// </summary>
        /// <returns>True if Task object is in a valid state</returns>
        public override bool IsValid()
        {
            return base.IsValid();
        }

        /// <summary>
        /// Populate the Task instance with data from the database
        /// </summary>
        /// <param name="force">If true, populate this Task regardless of state</param>
        /// <returns>True if data was successfully loaded</returns>
        public bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsTask = MyKanban.Data.GetTaskById(_id, _credential.Id);
                    DataTable dtTask = dsTask.Tables[0];

                    // Assign to properties
                    _name = dtTask.Rows[0]["name"].ToString();
                    DateTime.TryParse(dtTask.Rows[0]["start_date"].ToString(), out _startDate);
                    DateTime.TryParse(dtTask.Rows[0]["end_date"].ToString(), out _endDate);
                    long.TryParse(dtTask.Rows[0]["status_id"].ToString(), out _status);
                    _defineDone = dtTask.Rows[0]["define_done"].ToString();
                    double.TryParse(dtTask.Rows[0]["est_hours"].ToString(), out _estHours);
                    double.TryParse(dtTask.Rows[0]["act_hours"].ToString(), out _actHours);
                    long.TryParse(dtTask.Rows[0]["project_id"].ToString(), out _projectId);
                    long.TryParse(dtTask.Rows[0]["parent_task_id"].ToString(), out _parentTaskId);

                    long.TryParse(dtTask.Rows[0]["created_by"].ToString(), out _createdBy);
                    DateTime.TryParse(dtTask.Rows[0]["created"].ToString(), out _created);
                    long.TryParse(dtTask.Rows[0]["modified_by"].ToString(), out _modifiedBy);
                    DateTime.TryParse(dtTask.Rows[0]["modified"].ToString(), out _modified);

                    long.TryParse(dtTask.Rows[0]["board_set_id"].ToString(), out _boardSetId);
                    _boardSetName = dtTask.Rows[0]["board_set_name"].ToString();
                    _projectName = dtTask.Rows[0]["project_name"].ToString();

                    _sequence = int.Parse(dtTask.Rows[0]["sequence"].ToString());
                }

                // Force sub-collections to reload on next call
                _assignees = null;
                _comments = null;
                _subTasks = null;
                _tags = null;

                _isLoaded = true;
                _isDirty = false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reload data for this Task from the database
        /// </summary>
        public override void Reload()
        {
            base.Reload();

            TaskIdConstructor(_id, _credential);
        }

        /// <summary>
        /// Update the database with data from this object instance
        /// </summary>
        /// <param name="force">If true, save data to database regardless of the state of this StatusCode object</param>
        /// <returns>True if data successfully written to database</returns>
        public override bool Update(bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        // Save data for existing object
                        Data.UpdateTask(
                            _projectId,
                            _id,
                            _name,
                            _startDate,
                            _endDate,
                            _status,
                            _defineDone,
                            _estHours,
                            _actHours,
                            _parentTaskId,
                            _subTaskEstHours,
                            _subTaskActHours, 
                            _sequence,
                            _credential.Id
                            );
                    }
                    else
                    {
                        // Add a new object
                        DataSet dsNewTask = Data.AddTask(
                            _projectId,
                            _name,
                            _startDate,
                            _endDate,
                            _status,
                            _defineDone,
                            _estHours,
                            _actHours,
                            _parentTaskId,
                            _subTaskEstHours,
                            _subTaskActHours, 
                            _sequence,
                            _credential.Id
                            );

                        _id = long.Parse(dsNewTask.Tables[0].Rows[0]["id"].ToString());
                    }
                }

                // Make sure any comments are updated with current task #
                foreach (Comment comment in Comments.Items)
                {
                    comment.TaskId = _id;
                    comment.Update(force);
                }

                // Make sure any tags are updated with current task #
                foreach (Tag tag in Tags.Items)
                {
                    tag.TaskId = _id;
                    tag.Update(force);
                }

                // Make sure any assignees are updated
                foreach (Person assignee in Assignees.Items)
                {
                    Data.AddAssigneeToTask(_id, assignee.Id, _credential.Id);
                }

                // Make sure any approvers are updated
                foreach (Approver approver in Approvers.Items)
                {
                    approver.TaskId = _id;
                    approver.Update(force);
                }

                // Make sure any properties are updated
                foreach (Property property in Properties.Items)
                {
                    property.Parent = this;
                    property.ParentId = this.Id;
                    property.Update(force);
                }

                // Reload the data for this board
                LoadData(true);

                _isDirty = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
