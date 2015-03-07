using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Reflection;

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
 *  Class:      Board
 *  
 *  Purpose:    Represents a single MyKanban board object 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single MyKanban board object
    /// </summary>
    public class Board : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new, empty Board object
        /// </summary>
        /// <param name="credential">Credentials to use when creating this object</param>
        public Board(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new Board object by loading data for the given ID#
        /// </summary>
        /// <param name="boardId">ID# of board to load</param>
        /// <param name="credential">Credentials to use when creating this object</param>
        public Board(long boardId, Credential credential)
        {
            BoardIdConstructor(boardId, credential);
        }

        private void BoardIdConstructor(long boardId, Credential credential)
        {
            if (credential != null) _credential = credential;
            _id = boardId;
            _projects = null;
            _properties = null;
            _statusCodes = null;
            _tasks = null;
            _users = null;
            LoadData(true);
        }

        #endregion

        #region Properties

        [MyKanban.Description("ID# of board set this board belongs to")]
        public long BoardSetId
        {
            get
            {
                return _parentId;
            }
        }

        private Users _users = null;

        [MyKanban.Description("List of all users associated with this board")]
        public Users Users
        {
            get
            {

                if (_users == null && _id > 0)
                {
                    _users = new Users(this, _credential);
                }
                else if (_users == null)
                {
                    _users = new Users(_credential);
                }

                return _users;
            }
        }

        private Projects _projects = null;
        public Projects Projects
        {
            get
            {
                if (_projects == null && _id > 0)
                {
                    _projects = new Projects(this, _credential);
                }
                else if (_projects == null)
                {
                    _projects = new Projects(_credential);
                }
                _projects.ParentId = _id;
                return _projects;
            }
        }

        private Properties _properties = null;

        [MyKanban.Description("Collection of properties associated with this board")]
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

        private Sprints _sprints = null;

        [MyKanban.ReadOnly(true)]
        public Sprints Sprints
        {
            get 
            {

                if (_sprints == null && _id > 0)
                {
                    _sprints = new Sprints(this, _credential);
                }
                else if (_sprints == null || _sprints.Count == 0)
                {
                    _sprints = new Sprints(_credential);
                }
                return _sprints; 
            }
            set { _sprints = value; _isDirty = true; }
        }

        private Tasks _tasks = null;

        [MyKanban.ReadOnly(true)]
        public Tasks Tasks
        {
            get
            {

                if (_tasks == null && _id > 0)
                {
                    _tasks = new Tasks(this, _credential);
                }
                else if (_tasks == null || _tasks.Count == 0)
                {
                    _tasks = new Tasks(_credential);
                }
                return _tasks;
            }
            set { _tasks = value; _isDirty = true; }
        }

        private List<StatusCode> _statusCodes = null;
        public List<StatusCode> StatusCodes
        {
            get
            {
                if (_statusCodes == null && _id > 0)
                {
                    DataSet dsStatusCodes = Data.GetStatusValuesForBoard(_id, _credential.Id);
                    _statusCodes = new List<StatusCode>();
                    if (dsStatusCodes.Tables.Count > 0 && dsStatusCodes.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drStatusCode in dsStatusCodes.Tables[0].Rows)
                        {
                            StatusCode status = new StatusCode(_credential);
                            status.ColumnHeading = drStatusCode["column_heading"].ToString();
                            status.Name = drStatusCode["status"].ToString();
                            status.Id = long.Parse(drStatusCode["id"].ToString());
                            _statusCodes.Add(status);
                        }
                    }
                }
                else if (_statusCodes == null)
                {
                    _statusCodes = new List<StatusCode>();
                }

                return _statusCodes;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this Board from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteBoard(_id, _credential.Id);

            // Delete any associated sprints
            if (_sprints != null)
            {
                foreach (Sprint sprint in _sprints.Items)
                {
                    sprint.Delete();
                }
            }

            // Delete any associated properties
            foreach (Property property in Properties.Items)
            {
                property.Delete();
            }
        }

        /// <summary>
        /// Is the current user authorized to perform the specified operation
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="authLevel">Operation being requested</param>
        /// <returns>True if user is authorized, false otherwise</returns>
        public bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            // TODO: implement authorization logic
            switch (authLevel)
            {
                case Data.AuthorizationType.Read:
                    return true;

                case Data.AuthorizationType.Add:
                    return false;

                case Data.AuthorizationType.Update:
                    return false;

                case Data.AuthorizationType.Delete:
                    return false;

                default:
                    return false;

            }
        }

        /// <summary>
        /// Is the Board in a valid state
        /// </summary>
        /// <returns>True if Board is in valid state, false otherwise</returns>
        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Load Board datra from database
        /// </summary>
        /// <param name="force">If true, load data regardless of state of this object</param>
        /// <returns>True if data has been successfully loaded</returns>
        public bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsBoard = MyKanban.Data.GetBoardById(_id, _credential.Id);
                    if (dsBoard.Tables.Count > 0 && dsBoard.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtBoard = dsBoard.Tables[0];
                        DataRow drBoard = dtBoard.Rows[0];
                        _name = drBoard["name"].ToString();
                        _parentId = long.Parse(drBoard["board_set_id"].ToString());

                        long.TryParse(drBoard["created_by"].ToString(), out _createdBy);
                        long.TryParse(drBoard["modified_by"].ToString(), out _modifiedBy);
                        DateTime.TryParse(drBoard["created"].ToString(), out _created);
                        DateTime.TryParse(drBoard["modified"].ToString(), out _modified);

                        if (!long.TryParse(drBoard["board_set_id"].ToString(), out _parentId))
                        {
                            _parentId = 0;
                            _parentName = null;
                        }
                        else
                        {
                            if (dsBoard.Tables.Count > 1)
                            {
                                try
                                {
                                    DataTable dtBoardSet = dsBoard.Tables[2];
                                    _parentName = dtBoardSet.Rows[0]["name"].ToString();
                                }
                                catch
                                {
                                    _parentId = 0;
                                    _parentName = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        _id = 0;
                    }

                }

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
        /// Reload data from database into this object
        /// </summary>
        public override void Reload()
        {
            base.Reload();

            if (_id > 0)
            {
                BoardIdConstructor(_id, _credential);
            }
        }

        /// <summary>
        /// Update the database with data from this Board
        /// </summary>
        /// <param name="force">If true, write data to database regardless of the state of this object</param>
        /// <returns>True if database has been successfully updated</returns>
        public bool Update(bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        // Save data for existing board
                        Data.UpdateBoard(_id, _name, _parentId, _credential.Id);
                    }
                    else
                    {
                        // Add a new board
                        DataSet dsNewBoard = Data.AddBoard(_name, _parentId, _credential.Id);
                        _id = long.Parse(dsNewBoard.Tables[0].Rows[0]["id"].ToString());
                        DateTime.TryParse(dsNewBoard.Tables[0].Rows[0]["created"].ToString(), out _created);
                        long.TryParse(dsNewBoard.Tables[0].Rows[0]["created_by"].ToString(), out _createdBy);
                        DateTime.TryParse(dsNewBoard.Tables[0].Rows[0]["modified"].ToString(), out _modified);
                        long.TryParse(dsNewBoard.Tables[0].Rows[0]["modified_by"].ToString(), out _modifiedBy);
                    }

                    // Save data for any constituent people
                    foreach (User user in Users.Items)
                    {
                        user.Update(true);
                    }

                    // Save data for any constituent projects
                    foreach (Project project in Projects.Items)
                    {
                        project.Parent = this;
                        project.ParentId = _id;
                        project.Update(true);
                    }

                    // Save the linkage of these projects to the board
                    Projects.Update(true);

                    // Save any constituent sprints
                    foreach (Sprint sprint in Sprints.Items)
                    {
                        sprint.ParentId = _id;
                        sprint.Update();
                    }
                    
                    // Reload the data for this board
                    LoadData(true);
                }
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
