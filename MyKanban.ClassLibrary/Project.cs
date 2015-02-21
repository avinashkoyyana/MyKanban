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
 *  Class:      Project
 *  
 *  Purpose:    Represents a single project tracked by the system 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Project : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        public Project(Credential credential) 
        {
            if (credential != null) _credential = credential;

            _tasks = new Tasks(_credential);
        }

        public Project(long projectId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = projectId;
            _tasks = new Tasks(this, _credential);
            LoadData();
        }

        #endregion

        #region Properties

        private double _actHours;

        [MyKanban.Description("Actual hours required to complete this project")]
        [MyKanban.ControlType(enumControlType.Numeric)]
        public double ActHours
        {
            get { return _actHours; }
            set
            {
                _actHours = value;
                _isDirty = true;
            }
        }

        private long _boardId = 0;

        [MyKanban.Description("ID# of board this project is displayed on")]
        [MyKanban.ReadOnly(true)]
        public long BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        private long _boardSetId = 0;

        [MyKanban.Description("ID# of board set this project is associated with")]
        public long BoardSetId
        {
            get { return _boardSetId; }
        }

        private string _boardSetName = "";

        [MyKanban.Description("Display name of board set this project is associated with")]
        public string BoardSetName
        {
            get { return _boardSetName; }
        }

        private string _defineDone = "";

        [MyKanban.Description("Description of expected deliverable(s) or outcomes when project is completed")]
        public string DefineDone
        {
            get { return _defineDone; }
            set 
            { 
                _defineDone = value; 
                _isDirty = true; 
            }
        }

        private DateTime _earliestTaskStartDate = new DateTime(1900,1,1);

        [MyKanban.Description("Earlest start date for all tasks in this project")]
        [MyKanban.ReadOnly(true)]
        public DateTime EarliestTaskStartDate
        {
            get { return _earliestTaskStartDate; }
            set { _earliestTaskStartDate = value; _isDirty = true; }
        }

        private double _estHours;

        [MyKanban.Description("Estimated total hours for project")]
        [MyKanban.ControlType(enumControlType.Numeric)]
        public double EstHours
        {
            get { return _estHours; }
            set
            {
                _estHours = value;
                _isDirty = true;
            }
        }

        private DateTime _expectedEndDate = new DateTime(1900,1,1);

        [MyKanban.Description("Date project is expected to end")]
        public DateTime ExpectedEndDate
        {
            get { return _expectedEndDate; }
            set { _expectedEndDate = value; _isDirty = true; }
        }

        private DateTime _expectedStartDate = new DateTime(1900,1,1);

        [MyKanban.Description("Date project is expected to start")]
        public DateTime ExpectedStartDate
        {
            get { return _expectedStartDate; }
            set { _expectedStartDate = value; _isDirty = true; }
        }

        private DateTime _latestTaskEndDate = new DateTime(1900,1,1);

        [MyKanban.Description("Latest end date for all tasks in this project")]
        [MyKanban.ReadOnly(true)]
        public DateTime LatestTaskEndDate
        {
            get { return _latestTaskEndDate; }
            set { _latestTaskEndDate = value; _isDirty = true; }
        }

        private string _parentName;

        [MyKanban.Description("Name of board containing this sprint object")]
        public string ParentName
        {
            get
            {
                if (_boardId > 0 && string.IsNullOrEmpty(_parentName))
                {
                    Board board = new Board(_boardId, _credential);
                    _parent = board;
                    _parentId = board.Id;
                    _parentName = board.Name;
                    _parentType = board.GetType().FullName;
                }
                return _parentName;
            }
        }

        private string _parentType;

        [MyKanban.Description("Type of parent object")]
        public string ParentType
        {
            get
            {
                if (_boardId > 0 && string.IsNullOrEmpty(_parentType))
                {
                    Board board = new Board(_boardId, _credential);
                    _parent = board;
                    _parentId = board.Id;
                    _parentName = board.Name;
                    _parentType = board.GetType().FullName;
                }
                return _parentType;
            }
        }
        
        private long _projectLead = 0;
        public long ProjectLead
        {
            get { return _projectLead; }
            set { _projectLead = value; _isDirty = true; }
        }
        
        private long _status;

        [MyKanban.Description("Current status of this project")]
        [MyKanban.ControlType(enumControlType.StatusCode)]
        public long Status
        {
            get { return _status; }
            set { _status = value; _isDirty = true; }
        }

        private Tasks _tasks;

        [MyKanban.Hidden(true)]
        public Tasks Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    if (_id > 0)
                    {
                        _tasks = new Tasks(this, _credential);
                    }
                    else
                    {
                        _tasks = new Tasks(_credential);
                    }
                }
                _tasks.ProjectId = _id;
                return _tasks;
            }
        }

        private Properties _properties = null;

        [MyKanban.Description("Collection of properties associated with this project")]
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
                return _properties;
            }
        }

        private People _stakeholders;

        [MyKanban.Hidden(true)]
        public People Stakeholders
        {
            get
            {
                //LoadData();
                if (_stakeholders == null)
                {
                    if (_id > 0)
                    {
                        _stakeholders = new People(this, _credential);
                    }
                    else
                    {
                        _stakeholders = new People(_credential);
                    }
                }
                return _stakeholders;
            }
        }

        private StatusCodes _statusCodes;

        [JsonIgnore]
        [MyKanban.Hidden(true)]
        public StatusCodes StatusCodes
        {
            get
            {
                if (_statusCodes == null)
                {
                    if (_id > 0)
                    {
                        BoardSet boardSet = new BoardSet(_boardSetId, _credential);
                        _statusCodes = new StatusCodes(boardSet, _credential);
                    }
                    else
                    {
                        _statusCodes = new StatusCodes(_credential);
                    }
                }
                return _statusCodes;
            }
        }

        #endregion

        #region Methods

        public void Delete()
        {
            Data.DeleteProject(_id, _credential.Id);

            // Delete any associated tasks
            foreach (Task task in Tasks.Items)
            {
                task.Delete();
            }

            // Delete any associated properties
            foreach (Property property in Properties.Items)
            {
                property.Delete();
            }
        }

        public bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return true;
        }

        public bool IsValid()
        {
            return true;
        }

        // Populate the object instance with data from the database
        public bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsProject = MyKanban.Data.GetProjectById(_id, _credential.Id);

                    if (dsProject != null && dsProject.Tables.Count > 0 && dsProject.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtProject = dsProject.Tables[0];

                        // Assign to properties
                        _name = dtProject.Rows[0]["name"].ToString();
                        long.TryParse(dtProject.Rows[0]["project_lead"].ToString(), out _projectLead);
                        DateTime.TryParse(dtProject.Rows[0]["expected_start_date"].ToString(), out _expectedStartDate);
                        DateTime.TryParse(dtProject.Rows[0]["expected_end_date"].ToString(), out _expectedEndDate);
                        DateTime.TryParse(dtProject.Rows[0]["earliest_task_start_date"].ToString(), out _earliestTaskStartDate);
                        DateTime.TryParse(dtProject.Rows[0]["latest_task_end_date"].ToString(), out _latestTaskEndDate);
                        long.TryParse(dtProject.Rows[0]["status"].ToString(), out _status);
                        _defineDone = dtProject.Rows[0]["define_done"].ToString();
                        double.TryParse(dtProject.Rows[0]["task_est_hours"].ToString(), out _estHours);
                        double.TryParse(dtProject.Rows[0]["task_act_hours"].ToString(), out _actHours);

                        DateTime.TryParse(dtProject.Rows[0]["created"].ToString(), out _created);
                        long.TryParse(dtProject.Rows[0]["created_by"].ToString(), out _createdBy);
                        DateTime.TryParse(dtProject.Rows[0]["modified"].ToString(), out _modified);
                        long.TryParse(dtProject.Rows[0]["modified_by"].ToString(), out _modifiedBy);

                        long.TryParse(dtProject.Rows[0]["board_set_id"].ToString(), out _boardSetId);
                        _boardSetName = dtProject.Rows[0]["board_set_name"].ToString();
                    }
                    else
                    {
                        _id = 0;
                    }

                }

                // Force sub-collections to reload when first called
                _stakeholders = null;
                _tasks = null;

                _isLoaded = true;
                _isDirty = false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Update the database with data from this object instance
        public bool Update( bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        // Save data for existing object
                        Data.UpdateProject(
                            _id,
                            _name,
                            _projectLead,
                            _expectedStartDate,
                            _expectedEndDate,
                            _earliestTaskStartDate,
                            _latestTaskEndDate,
                            _status,
                            _defineDone,
                            _estHours,
                            _actHours, 
                            _credential.Id
                            );
                    }
                    else
                    {
                        // Add a new object
                        DataSet dsNewProject = Data.AddProject(
                            _name,
                            _projectLead,
                            _expectedStartDate,
                            _expectedEndDate,
                            _earliestTaskStartDate,
                            _latestTaskEndDate,
                            _status,
                            _defineDone,
                            _estHours,
                            _actHours, 
                            _credential.Id
                            );

                        _id = long.Parse(dsNewProject.Tables[0].Rows[0]["id"].ToString());
                    }

                }

                // Save data for any constituent people
                if (_stakeholders != null)
                {
                    foreach (Person person in Stakeholders.Items)
                    {
                        person.ParentId = _id;
                        person.Update(true);
                    }
                }
                else
                {
                    _stakeholders = new People(_credential);
                }

                // Save each child task
                foreach (Task task in Tasks.Items)
                {
                    task.ParentId = _id;
                    task.ProjectId = _id;
                    task.Update(true);
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
