using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

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
 *  Class:      Tag
 *  
 *  Purpose:    Represents a single tag that may be associated with a task
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single tag that may be associated with a task
    /// </summary>
    public class Tag : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new Tag object
        /// </summary>
        /// <param name="credential">Credentials to use when creating this Tag object</param>
        public Tag(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new Tag object and initialize its Name property
        /// </summary>
        /// <param name="name">Name to assign to this Tag object</param>
        /// <param name="credential">Credentials to use when creating this Tag object</param>
        public Tag(string name, Credential credential)
        {
            if (credential != null) _credential = credential;

            _name = name;
        }

        /// <summary>
        /// Create a new Tag object and initialize its data from the database
        /// based on the provided ID#
        /// </summary>
        /// <param name="tagId">ID# of tag to read from the database</param>
        /// <param name="credential">Credentials to use when creating this Tag object</param>
        public Tag(long tagId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = tagId;
            LoadData();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Text of this Tag object, is a synonym for the Name property
        /// </summary>
        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        long _tagId = 0;

        /// <summary>
        /// ID# of this Tag
        /// </summary>
        public long TagId
        {
            get { return _tagId; }
            set { _tagId = value; }
        }

        long _taskId = 0;

        /// <summary>
        /// ID# of parent Task object
        /// </summary>
        public long TaskId
        {
            get { return _taskId; }
            set { _taskId = value; _isDirty = true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this tag from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteTaskTag(_taskId, _name, _credential.Id);
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
        /// Populate the Tag instance with data from the database
        /// </summary>
        /// <param name="force">If true, populate this Tag regardless of state</param>
        /// <returns>True if data was successfully loaded</returns>
        public override bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsTag = MyKanban.Data.GetTagById(_id, _credential.Id);
                    DataTable dtTag = dsTag.Tables[0];

                    // Assign to properties
                    long.TryParse(dsTag.Tables[0].Rows[0]["id"].ToString(), out _id);
                    long.TryParse(dsTag.Tables[0].Rows[0]["tag_id"].ToString(), out _tagId);
                    long.TryParse(dsTag.Tables[0].Rows[0]["task_id"].ToString(), out _taskId);
                    _name = dsTag.Tables[0].Rows[0]["tag"].ToString();

                    DateTime.TryParse(dsTag.Tables[0].Rows[0]["created"].ToString(), out _created);
                    DateTime.TryParse(dsTag.Tables[0].Rows[0]["modified"].ToString(), out _modified);
                    long.TryParse(dsTag.Tables[0].Rows[0]["created_by"].ToString(), out _createdBy);
                    long.TryParse(dsTag.Tables[0].Rows[0]["modified_by"].ToString(), out _modifiedBy);

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
                    // Save data for tag.  Note that tag will only
                    // be added once, so don't need to worry about adding duplicates
                    DataSet dsTag = Data.AddTag(
                        _taskId,
                        _name, 
                        _credential.Id
                        );
                    long.TryParse(dsTag.Tables[0].Rows[0]["id"].ToString(), out _id);

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
