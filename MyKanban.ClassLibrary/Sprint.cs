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
 *  Class:      Sprint
 *  
 *  Purpose:    Represents a single sprint interval that may be associated
 *              with a Board object
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single sprint interval that may be associated with a Board object
    /// </summary>
    public class Sprint : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create an empty Sprint object
        /// </summary>
        /// <param name="credential">Credentials to use when creating this Sprint object</param>
        public Sprint(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new Sprint object and load data for the provided ID# from the database
        /// </summary>
        /// <param name="sprintId">ID# of Sprint to load from database</param>
        /// <param name="credential">Credentials to use when creating this Sprint object</param>
        public Sprint(long sprintId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = sprintId;
            LoadData();
        }

        /// <summary>
        /// Create a new Sprint object and initialize its data with provided parameters
        /// </summary>
        /// <param name="boardId">ID# of Board this sprint belongs to</param>
        /// <param name="startDate">Start date of sprint</param>
        /// <param name="endDate">End date of sprint</param>
        /// <param name="sequence">Ordinal position of this sprint within list of sprints</param>
        /// <param name="credential">Credentials to use when creating this Sprint object</param>
        public Sprint(long boardId, DateTime startDate, DateTime endDate, int sequence, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parentId = boardId;
            _startDate = startDate;
            _endDate = endDate;
            _sequence = sequence;
        }

        #endregion

        #region Properties

        DateTime _endDate;

        /// <summary>
        /// End date of sprint
        /// </summary>
        [MyKanban.Description("End date of sprint")]
        [MyKanban.ControlType(enumControlType.DateTime)]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; _isDirty = true; }
        }

        /// <summary>
        /// Display name of this sprint
        /// </summary>
        [MyKanban.Description("Display name of this sprint")]
        public override string Name
        {
            get
            {
                string name = Sequence.ToString().PadLeft(2, '0')
                    + ": "
                    + _startDate.ToShortDateString()
                    + " - "
                    + _endDate.ToShortDateString();
                return name;
            }
        }

        /// <summary>
        /// Name of board containing this sprint object
        /// </summary>
        [MyKanban.Description("Name of board containing this sprint object")]
        public override string ParentName
        {
            get
            {
                if (ParentId > 0 && string.IsNullOrEmpty(_parentName))
                {
                    Board board = new Board(_parentId, _credential);
                    _parentName = board.Name;
                    _parentType = board.GetType().FullName;
                }
                return _parentName;
            }
        }

        private string _parentType;

        /// <summary>
        /// "Type of parent object
        /// </summary>
        [MyKanban.Description("Type of parent object")]
        public override string ParentType
        {
            get 
            {
                if (ParentId > 0 && string.IsNullOrEmpty(_parentType))
                {
                    Board board = new Board(_parentId, _credential);
                    _parentName = board.Name;
                    _parentType = board.GetType().FullName;
                }
                return _parentType; 
            }
        }
        
        int _sequence = 0;

        /// <summary>
        /// Ordinal position of this sprint within all sprints for this board
        /// </summary>
        [MyKanban.Description("Ordinal position of this sprint within all sprints for this board")]
        [MyKanban.ControlType(enumControlType.Numeric)]
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; _isDirty = true; }
        }

        DateTime _startDate;

        /// <summary>
        /// Start date of this sprint
        /// </summary>
        [MyKanban.Description("Start date of this sprint")]
        [MyKanban.ControlType(enumControlType.DateTime)]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; _isDirty = true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this Sprint from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteSprint(_id, _credential.Id);
        }

        /// <summary>
        /// Is the specified user authorized to perform the requested operation
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="authLevel">Operation to perform</param>
        /// <returns>True if user is authorized to perform the requested operation</returns>
        public override bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return true;
        }

        /// <summary>
        /// Is this Sprint object in a valid state
        /// </summary>
        /// <returns>True if this Sprint object is in a valid state</returns>
        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Load data for this Sprint object from the database
        /// </summary>
        /// <param name="force">If true, load data regardless of the state of this object</param>
        /// <returns>True if data successfully loaded</returns>
        public override bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsSprint = MyKanban.Data.GetSprintById(_id, _credential.Id);
                    if (dsSprint.Tables.Count > 0 && dsSprint.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtSprint = dsSprint.Tables[0];
                        DataRow drSprint = dtSprint.Rows[0];
                        long.TryParse(drSprint["board_id"].ToString(), out _parentId);
                        DateTime.TryParse(drSprint["start_date"].ToString(), out _startDate);
                        DateTime.TryParse(drSprint["end_date"].ToString(), out _endDate);
                        int.TryParse(drSprint["sequence"].ToString(), out _sequence);

                        DateTime.TryParse(drSprint["created"].ToString(), out _created);
                        DateTime.TryParse(drSprint["modified"].ToString(), out _modified);
                        long.TryParse(drSprint["created_by"].ToString(), out _createdBy);
                        long.TryParse(drSprint["modified_by"].ToString(), out _modifiedBy);
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
        /// Update the database with data from this Sprint object
        /// </summary>
        /// <param name="force">If true, write data to database regardless of the state of this object</param>
        /// <returns>True if data successfully written</returns>
        public override bool Update(bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        // Save data for existing board
                        Data.UpdateSprint(_id, _parentId, _startDate, _endDate, _sequence, _credential.Id);
                    }
                    else
                    {
                        // Add a new board
                        DataSet dsNewBoard = Data.AddSprint(_parentId, _startDate, _endDate, _sequence, _credential.Id);
                        _id = long.Parse(dsNewBoard.Tables[0].Rows[0]["id"].ToString());
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
