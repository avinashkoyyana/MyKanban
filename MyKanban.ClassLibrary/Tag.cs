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
    public class Tag : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        public Tag(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Tag(string name, Credential credential)
        {
            if (credential != null) _credential = credential;

            _name = name;
        }

        public Tag(long tagId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = tagId;
            LoadData();
        }

        #endregion

        #region Properties

        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        long _tagId = 0;
        public long TagId
        {
            get { return _tagId; }
            set { _tagId = value; }
        }

        long _taskId = 0;
        public long TaskId
        {
            get { return _taskId; }
            set { _taskId = value; _isDirty = true; }
        }

        #endregion

        #region Methods

        public void Delete()
        {
            Data.DeleteTaskTag(_taskId, _name, _credential.Id);
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

        // Update the database with data from this object instance
        public bool Update( bool force = false)
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
