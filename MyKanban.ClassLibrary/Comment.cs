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
 *  Class:      Comment
 *  
 *  Purpose:    Represents a single comment associated with a task 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single Comment associated with a task or sub-task
    /// </summary>
    public class Comment : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new Comment object
        /// </summary>
        /// <param name="credential">Credentials used to create this Comment</param>
        public Comment(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new Comment object
        /// </summary>
        /// <param name="name">Text of comment to create</param>
        /// <param name="credential">Credentials used to create this Comment</param>
        public Comment(string name, Credential credential)
        {
            if (credential != null) _credential = credential;

            _name = name;
        }

        /// <summary>
        /// Create a new Comment object
        /// </summary>
        /// <param name="CommentId">ID# of comment to load from database</param>
        /// <param name="credential">Credentials used to create this Comment</param>
        public Comment(long CommentId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = CommentId;
            LoadData();
        }

        #endregion

        #region Properties

        /// <summary>
        /// ID# of this Comment
        /// </summary>
        [MyKanban.Description("The ID# of this comment")]
        [MyKanban.ReadOnly(true)]
        public long CommentId
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Parent object of this comment - i.e. the parent task
        /// </summary>
        [MyKanban.Description("Parent object of this comment - i.e. the parent task")]
        [MyKanban.Hidden(true)]
        public override IDataItem Parent
        {
            get
            {
                if (base.Parent == null && _taskId != 0)
                {
                    Task parentTask = new Task(_taskId, _credential);
                    base.Parent = parentTask;
                    base.ParentName = parentTask.Name;
                    base.ParentId = parentTask.Id;
                }
                return base.Parent;
            }
        }

        /// <summary>
        /// Synonym for TaskId property; the task that this comment belongs to
        /// </summary>
        [MyKanban.Description("Synonym for TaskId property; the task that this comment belongs to")]
        public override long ParentId
        {
            get
            {
                if (base.ParentId == 0 && _taskId != 0)
                {
                    Task parentTask = new Task(_taskId, _credential);
                    base.Parent = parentTask;
                    base.ParentName = parentTask.Name;
                    base.ParentId = parentTask.Id;
                }
                return base.ParentId;
            }
        }

        /// <summary>
        /// Name of parent task for this comment
        /// </summary>
        [MyKanban.Description("Name of parent task for this comment")]
        public override string ParentName
        {
            get
            {
                if (string.IsNullOrEmpty(base.ParentName) && _taskId != 0)
                {
                    Task parentTask = new Task(_taskId, _credential);
                    base.Parent = parentTask;
                    base.ParentName = parentTask.Name;
                    base.ParentId = parentTask.Id;
                }
                return base.ParentName;
            }
        }

        /// <summary>
        /// Synonym for Name property
        /// </summary>
        [MyKanban.Description("Synonym for Name property")]
        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
                _isDirty = true;
            }
        }

        long _taskId = 0;

        /// <summary>
        /// Synonym for ID# of this comment
        /// </summary>
        [MyKanban.Description("Synonym for ID# of this comment")]
        [MyKanban.ReadOnly(true)]
        public long TaskId
        {
            get { return _taskId; }
            set { _taskId = value; _isDirty = true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this Comment object from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteTaskComment(_id, _credential.Id);
        }

        /// <summary>
        /// Is the user authorized to perform the specified operation
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="authLevel">Operation to perform</param>
        /// <returns>True if user is authorized to perform the requested operation</returns>
        public override bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return true;
        }

        /// <summary>
        /// Is this object in a valid state
        /// </summary>
        /// <returns>True if object is in a valid state, false otherwise</returns>
        public override bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Populate the object instance with data from the database
        /// </summary>
        /// <param name="force">If true, load data from database regardless of the state of this object</param>
        /// <returns>True if data successfully loaded</returns>
        public override bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsComment = MyKanban.Data.GetCommentById(_id, _credential.Id);
                    DataTable dtComment = dsComment.Tables[0];

                    // Assign to properties
                    long.TryParse(dsComment.Tables[0].Rows[0]["id"].ToString(), out _id);
                    long.TryParse(dsComment.Tables[0].Rows[0]["task_id"].ToString(), out _taskId);
                    _name = dsComment.Tables[0].Rows[0]["Comment"].ToString();

                    DateTime.TryParse(dsComment.Tables[0].Rows[0]["created"].ToString(), out _created);
                    DateTime.TryParse(dsComment.Tables[0].Rows[0]["modified"].ToString(), out _modified);
                    long.TryParse(dsComment.Tables[0].Rows[0]["created_by"].ToString(), out _createdBy);
                    long.TryParse(dsComment.Tables[0].Rows[0]["modified_by"].ToString(), out _modifiedBy);

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
        /// <param name="force">If true, update the database regardless of the state of this object</param>
        /// <returns>True if database successfully updated</returns>
        public override bool Update( bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        DataSet dsComment = Data.UpdateComment(
                            _taskId,
                            _id,
                            _name, _credential.Id
                            );
                        long.TryParse(dsComment.Tables[0].Rows[0]["id"].ToString(), out _id);
                    }
                    else
                    {
                        DataSet dsComment = Data.AddComment(
                            _taskId,
                            _name, _credential.Id
                            );
                       long.TryParse(dsComment.Tables[0].Rows[0]["id"].ToString(), out _id);
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
