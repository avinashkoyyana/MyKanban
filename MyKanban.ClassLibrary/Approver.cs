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
    public class Approver : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        public Approver(Credential credential)
        {
            if (credential != null) _credential = credential;
            _id = 0;
        }

        public Approver(long approverId, Credential credential)
        {
            if (credential != null) _credential = credential;
            _id = approverId;
            LoadData();
        }

        #endregion

        #region Properties

        [MyKanban.Description("Name of task approver")]
        [MyKanban.ReadOnly(true)]
        public new string Name
        {
            get 
            {
                if (_personId > 0)
                {
                    Person approver = new Person(_personId, _credential);
                    return approver.Name;
                }
                else
                {
                    return base.Name;
                }
            }
        }

        private long _personId = 0;

        [MyKanban.Description("ID# of task approver")]
        public long PersonId
        {
            get { return _personId; }
            set { _personId = value; }
        }

        [MyKanban.Description("ID# of parent task")]
        [MyKanban.ReadOnly(true)]
        public long TaskId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        #endregion

        #region Methods

        public override void Delete()
        {
            Data.DeleteApproverFromTask(_parentId, _personId, _credential.Id);
        }

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
