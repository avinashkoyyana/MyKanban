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
 *  Class:      Approvers
 *  
 *  Purpose:    Represents a collection of Approver objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Approvers : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Approvers(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Approvers(Task task, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parent = task;
            _parentId = task.Id;
            DataSet dsApprovers = MyKanban.Data.GetApproversByTask(task.Id, _credential.Id);
            if (dsApprovers.Tables.Count > 0)
            {
                foreach (DataRow drApprover in dsApprovers.Tables[0].Rows)
                {
                    _items.Add(new Approver(long.Parse(drApprover["approver_id"].ToString()), _credential));
                }
            }
        }

        public Approvers(long taskId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parentId = taskId;
            DataSet dsApprovers = MyKanban.Data.GetApproversByTask(taskId, _credential.Id);
            if (dsApprovers.Tables.Count > 0)
            {
                foreach (DataRow drApprover in dsApprovers.Tables[0].Rows)
                {
                    _items.Add(new Approver(long.Parse(drApprover["approver_id"].ToString()), _credential));
                }
            }
        }

        #endregion

        #region Properties

        new public Approver this[int index]
        {
            get { return (Approver)_items[index]; }
            set { _items[index] = value; }
        }

        #endregion

        #region Methods

        public override void Clear(bool delete = false)
        {
            if (delete)
            {
                foreach (Approver approver in _items)
                {
                    approver.Delete();
                }
            }

            base.Clear();
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Approver)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        #endregion
    }
}
