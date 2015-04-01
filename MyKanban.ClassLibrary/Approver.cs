using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Newtonsoft.Json;

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
 *  Class:      Approver
 *  
 *  Purpose:    Represents a single task approver
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single approver associated with a task or sub-task.
    /// </summary>
    public class Approver : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new approver object instance
        /// </summary>
        /// <param name="credential">Credentials to use when creating this approver object</param>
        public Approver(Credential credential)
        {
            if (credential != null) _credential = credential;
            _id = 0;
        }

        /// <summary>
        /// Create a new approver object instance
        /// </summary>
        /// <param name="approverId">ID# of approver to read from database</param>
        /// <param name="credential">Credential to use when creating this approver object</param>
        public Approver(long approverId, Credential credential)
        {
            if (credential != null) _credential = credential;
            _id = approverId;
            LoadData();
        }

        #endregion

        #region Properties

        private Person _person = null;

        /// <summary>
        /// Email of approver
        /// </summary>
        [MyKanban.Description("Email of task approver")]
        [MyKanban.ReadOnly(true)]
        public string Email
        {
            get
            {
                if (_person != null)
                {
                    return _person.Email;
                }
                else if (_personId > 0)
                {
                    _person = new Person(_personId, _credential);
                    return _person.Email;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Name of approver
        /// </summary>
        [MyKanban.Description("Name of task approver")]
        [MyKanban.ReadOnly(true)]
        public string Name
        {
            get 
            {
                if (_person != null)
                {
                    return _person.Name;
                }
                else if (_personId > 0)
                {
                    _person = new Person(_personId, _credential);
                    return _person.Name;
                }
                else
                {
                    return base.Name;
                }
            }
        }

        private long _personId = 0;

        /// <summary>
        /// ID# of task approver
        /// </summary>
        [MyKanban.Description("ID# of task approver")]
        public long PersonId
        {
            get { return _personId; }
            set { _personId = value; }
        }

        /// <summary>
        /// Phone of approver
        /// </summary>
        [MyKanban.Description("Phone of task approver")]
        [MyKanban.ReadOnly(true)]
        public string Phone
        {
            get
            {
                if (_person != null)
                {
                    return _person.Phone;
                }
                else if (_personId > 0)
                {
                    _person = new Person(_personId, _credential);
                    return _person.Phone;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// ID# of parent task
        /// </summary>
        [MyKanban.Description("ID# of parent task")]
        [MyKanban.ReadOnly(true)]
        public long TaskId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this approver from task or sub-task
        /// </summary>
        /// <example>
        /// <code>
        /// Approver approver = new Approver(123, user);
        /// approver.Delete();        
        /// </code>
        /// </example>
        public override void Delete()
        {
            Data.DeleteApproverFromTask(_parentId, _personId, _credential.Id);
        }

        /// <summary>
        /// Retrieve data from database into this object
        /// </summary>
        /// <param name="force">If true, object will be reloaded even if no data has changed</param>
        /// <returns></returns>
        public override bool LoadData(bool force = false)
        {
            if (_id > 0)
            {
                DataSet dsApprover = Data.GetApprover(_id, _credential.Id);
                if (dsApprover != null && dsApprover.Tables.Contains("results") && dsApprover.Tables["results"].Rows.Count > 0)
                {
                    DataRow drApprover = dsApprover.Tables["results"].Rows[0];
                    long.TryParse(drApprover["id"].ToString(), out _personId);
                    long.TryParse(drApprover["task_id"].ToString(), out _parentId);
                    _name = drApprover["name"].ToString();
                }
                else
                {
                    _personId = 0;
                    _parentId = 0;
                    _id = 0;
                }
            }

            _isDirty = false;
            _isLoaded = true;

            return true;
        }

        /// <summary>
        /// Save data from object to database
        /// </summary>
        /// <param name="force">If true, data will be saved even if no data has changed since being loaded into object from the database.</param>
        /// <returns></returns>
        public override bool Update(bool force = false)
        {
            if (_parentId > 0)
            {
                DataSet dsApprover = Data.AddApproverToTask(_parentId, _personId, _credential.Id);
                long.TryParse(dsApprover.Tables[0].Rows[0]["id"].ToString(), out _id);
            }
            return true;
        }

        #endregion
    }
}
