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
 *  Class:      Comments
 *  
 *  Purpose:    Represents a collection of comments associated with a given task 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Comments : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Comments(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Comments(Task task, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsComments = MyKanban.Data.GetCommentsByTask(task.Id, _credential.Id);
            foreach (DataRow drComment in dsComments.Tables["results"].Rows)
            {
                Comment Comment = new Comment(long.Parse(drComment["id"].ToString()), _credential);
                Comment.TaskId = task.Id;
                Comment.ParentId = task.Id;
                Comment.Parent = task;
                _items.Add(Comment);
            }
            _parent = task;
            _parentId = task.Id;
            _taskId = task.Id;
        }

        public Comments(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsComments = MyKanban.Data.GetCommentsByName(nameFilter, _credential.Id);
            foreach (DataRow drComment in dsComments.Tables["results"].Rows)
            {
                Comment Comment = new Comment(long.Parse(drComment["id"].ToString()), _credential);
                _items.Add(Comment);
            }
        }

        #endregion

        #region Properties

        public Comment this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        private List<Comment> _items = new List<Comment>();
        public List<Comment> Items
        {
            get { return _items; }
        }

        private long _taskId = 0;
        public long TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        #endregion

        #region Methods

        public void Add(Comment Comment)
        {
            _isDirty = true;
            _items.Add(Comment);
            Comment.Parent = _parent;
            if (_parent != null)
            {
                Comment.ParentId = _parent.Id;
                Comment.TaskId = _parent.Id;
            }
            Comment.Update(true);
        }

        public void Clear()
        {
            _items.Clear();
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Comment)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public void Remove(int index)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(Comment Comment)
        {
            _items.Remove(Comment);
        }

        public bool Update(bool force = false)
        {
            try
            {
                foreach (Comment Comment in _items)
                {
                    if (_parentId != 0)
                    {
                        Comment.ParentId = _parentId;
                        Comment.TaskId = _parentId;
                    }
                    Comment.Update(true);
                }
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
