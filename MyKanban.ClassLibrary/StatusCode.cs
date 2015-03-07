using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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
 *  Class:      StatusCode
 *  
 *  Purpose:    Represents a single status code that may be associated
 *              with a BoardSet, Project or Task object
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single status code that may be associated with a 
    /// board set, project or task
    /// </summary>
    public class StatusCode : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new empty StatusCode object
        /// </summary>
        /// <param name="credential">Credentials to use when creating this StatusCode object</param>
        public StatusCode(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new StatusCode object and initialize its Name property
        /// </summary>
        /// <param name="name">Name to assign to this status code</param>
        /// <param name="credential">Credentials to use when creating this StatusCode object</param>
        public StatusCode(string name, Credential credential)
        {
            if (credential != null) _credential = credential;

            _name = name;
            _status = name;
            _columnHeading = name;
        }

        /// <summary>
        /// Create a new StatusCode object and initialize its data from the database based on the
        /// provided ID#
        /// </summary>
        /// <param name="statusCodeId">ID# of status code to read from database</param>
        /// <param name="credential">Credentials to use when creating this StatusCode object</param>
        public StatusCode(long statusCodeId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = statusCodeId;
            LoadData();
        }

        #endregion

        #region Properties

        string _columnHeading = "";

        /// <summary>
        /// Column heading to use when displaying this status code on pages 
        /// or in drop-down lists
        /// </summary>
        [MyKanban.Description("Display heading to use for this status")]
        public string ColumnHeading
        {
            get { return _columnHeading; }
            set { _columnHeading = value; _isDirty = true; }
        }

        // Default colors to use
        private string _foreColor = "black";
        private string _backColor = "white";

        /// <summary>
        /// Background color to use for tasks with this status
        /// </summary>
        [MyKanban.Description("Background color to use for tasks with this status")]
        public string BackColor
        {
            get { return _backColor; }
            set { _backColor = value; _isDirty = true; }
        }

        /// <summary>
        /// Foreground color to use for tasks with this status
        /// </summary>
        [MyKanban.Description("Foreground color to use for tasks with this status")]
        public string ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; _isDirty = true; }
        }

        /// <summary>
        /// Synonym for the Status property
        /// </summary>
        [MyKanban.Description("Synonym for the Status property")]
        public new string Name
        {
            get { return _status; }
            set { _status = value; _isDirty = true; }
        }

        int _sequence = 0;

        /// <summary>
        /// Ordinal position of this status code within list and on boards
        /// </summary>
        [MyKanban.Description("Ordinal position of this status code within list and on boards")]
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; _isDirty = true; }
        }

        string _status = "";

        /// <summary>
        /// Name of this status code
        /// </summary>
        [MyKanban.Description("Name of this status code")]
        [MyKanban.ReadOnly(true)]
        public string Status
        {
            get { return _status; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this status code from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteStatusCode(_id, _credential.Id);
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
        /// Is this Project object in a valid state
        /// </summary>
        /// <returns>True if Project object is in a valid state</returns>
        public override bool IsValid()
        {
            return base.IsValid();
        }

        /// <summary>
        /// Populate the StatusCode instance with data from the database
        /// </summary>
        /// <param name="force">If true, populate this StatusCode regardless of state</param>
        /// <returns>True if data was successfully loaded</returns>
        public override bool LoadData(bool force = false)
        {
            try 
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsStatusCode = Data.GetStatusCodeById(_id, _credential.Id);
                    DataTable dtStatusCode = dsStatusCode.Tables[0];

                    // Assign to properties
                    _name = dtStatusCode.Rows[0]["status"].ToString();
                    long.TryParse(dtStatusCode.Rows[0]["board_set_id"].ToString(), out _parentId);
                    int.TryParse(dtStatusCode.Rows[0]["sequence"].ToString(), out _sequence);
                    _columnHeading = dtStatusCode.Rows[0]["column_heading"].ToString();
                    Name = dtStatusCode.Rows[0]["status"].ToString();
                    long.TryParse(dtStatusCode.Rows[0]["board_set_id"].ToString(), out _parentId);
                    long.TryParse(dtStatusCode.Rows[0]["id"].ToString(), out _id);

                    DateTime.TryParse(dtStatusCode.Rows[0]["created"].ToString(), out _created);
                    DateTime.TryParse(dtStatusCode.Rows[0]["modified"].ToString(), out _modified);
                    long.TryParse(dtStatusCode.Rows[0]["created_by"].ToString(), out _createdBy);
                    long.TryParse(dtStatusCode.Rows[0]["modified_by"].ToString(), out _modifiedBy);

                    _foreColor = dtStatusCode.Rows[0]["fore_color"].ToString();
                    _backColor = dtStatusCode.Rows[0]["back_color"].ToString();
                }

                _isLoaded = true;
                _isDirty = false;

                return true;
            }
            catch { return false; }
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
                        Data.UpdateStatusCode(
                            _parentId,
                            _id,
                            _columnHeading,
                            _status,
                            _sequence, 
                            _credential.Id,
                            _foreColor,
                            _backColor
                            );
                    }
                    else
                    {
                        // Add a new object
                        DataSet dsNewStatusCode = Data.AddStatusCode(
                            _parentId,
                            _columnHeading,
                            _status,
                            _sequence, 
                            _credential.Id,
                            _foreColor,
                            _backColor
                            );

                        _id = long.Parse(dsNewStatusCode.Tables[0].Rows[0]["id"].ToString());
                    }

                    // Reload the data for this board
                    LoadData(true);
                }
                _isDirty = false;
                return true; 
            }
            catch { return false; }
        }

        #endregion
    }
}
