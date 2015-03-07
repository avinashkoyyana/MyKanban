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
    /// <summary>
    /// Represents a collection of all approvers associated with a given task or sub-task
    /// </summary>
    public class Approvers : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        /// <summary>
        /// Create a new approvers collection
        /// </summary>
        /// <param name="credential">Credentials to use when creating this collection.</param>
        public Approvers(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        /// <summary>
        /// Create a new approvers collection
        /// </summary>
        /// <param name="task">Parent task or sub-task this collection is for</param>
        /// <param name="credential">Credentials to use when creating this collection.</param>
        public Approvers(Task task, Credential credential)
        {
            ApproversTaskConstructor(task, credential);
        }

        /// <summary>
        /// Code pulled out of constructor so can also be called by Reload() method
        /// </summary>
        /// <param name="task">Parent task or sub-task this collection is for</param>
        /// <param name="credential">Credentials to use when creating this collection.</param>
        private void ApproversTaskConstructor(Task task, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parent = task;
            _parentId = task.Id;
            _items.Clear();
            DataSet dsApprovers = MyKanban.Data.GetApproversByTask(task.Id, _credential.Id);
            if (dsApprovers.Tables.Count > 0)
            {
                foreach (DataRow drApprover in dsApprovers.Tables[0].Rows)
                {
                    _items.Add(new Approver(long.Parse(drApprover["approver_id"].ToString()), _credential));
                }
            }
        }

        /// <summary>
        /// Create a new approvers collection
        /// </summary>
        /// <param name="taskId">ID# of parent task or sub-task</param>
        /// <param name="credential">Credentials to use when creating this collection</param>
        public Approvers(long taskId, Credential credential)
        {
            ApproversTaskIdConstructor(taskId, credential);
        }

        /// <summary>
        /// Code pulled out of constructor so could be called by Reload() method as well as by constructor
        /// </summary>
        /// <param name="taskId">ID# of parent task or sub-task</param>
        /// <param name="credential">Credentials to use when creating this collection</param>
        private void ApproversTaskIdConstructor(long taskId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parentId = taskId;
            _items.Clear();
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

        /// <summary>
        /// Indexer for collection
        /// </summary>
        /// <param name="index">Ordinal position of item to return</param>
        /// <returns>Approver at specified index position</returns>
        new public Approver this[int index]
        {
            get { return (Approver)_items[index]; }
            set { _items[index] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove all approvers from the collection
        /// </summary>
        /// <param name="delete">If true, will also cause removed items to be deleted from the database</param>
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

        /// <summary>
        /// Return a list of BaseItem objects corresponding to items in this collection
        /// </summary>
        /// <returns>A collection of BaseItems</returns>
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

        /// <summary>
        /// Reload all data for this collection from database
        /// </summary>
        public override void Reload()
        {
            base.Reload();

            if (_parent != null) ApproversTaskConstructor((Task)_parent, _credential);
            else if (_parentId > 0) ApproversTaskIdConstructor(_parentId, _credential);
        }

        #endregion
    }
}
