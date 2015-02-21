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
 *  Class:      Tags
 *  
 *  Purpose:    Represents a collection of Tag objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Tags : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Tags(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Tags(Task task, Credential credential)
        {
            if (credential != null) _credential = credential;

            if (task.Id > 0)
            {
                DataSet dsTags = MyKanban.Data.GetTagsByTask(task.Id, _credential.Id);
                foreach (DataRow drTag in dsTags.Tables["results"].Rows)
                {
                    Tag tag = new Tag(long.Parse(drTag["id"].ToString()), _credential);
                    _items.Add(tag);
                }
            }
            _parent = task;
            _parentId = task.Id;
        }

        public Tags(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsTags = MyKanban.Data.GetTagsByName(nameFilter, _credential.Id);
            foreach (DataRow drTag in dsTags.Tables["results"].Rows)
            {
                Tag tag = new Tag(long.Parse(drTag["id"].ToString()), _credential);
                _items.Add(tag);
            }
        }

        #endregion

        #region Properties

        public Tag this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }
        
        bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
        }

        Credential _credential = new Credential();
        public Credential Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        bool _isLoaded = false;
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public string JSON()
        {
            return Data.GetJson(this); 
        }

        private List<Tag> _items = new List<Tag>();
        public List<Tag> Items
        {
            get { return _items; }
        }

        private MyKanban.IDataItem _parent;
        public MyKanban.IDataItem Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                if (value == null)
                {
                    _parentId = 0;
                }
                else
                {
                    _parentId = _parent.Id;
                }
                _isDirty = true;
            }
        }

        private long _parentId;
        public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private long _taskId = 0;
        public long TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        #endregion

        #region Methods

        public void Add(Tag tag)
        {
            _isDirty = true;
            _items.Add(tag);
            tag.Parent = _parent;
            tag.TaskId = _taskId;
            tag.Update(true);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public int Count
        {
            get { return _items.Count; }
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Tag)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public void Remove(int index)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(Tag tag)
        {
            _items.Remove(tag);
        }

        public bool Update(bool force = false)
        {
            try
            {
                foreach (Tag tag in _items)
                {
                    if (_parentId != 0)
                    {
                        tag.ParentId = _parentId;
                        tag.TaskId = _taskId;
                    }
                    tag.Update(true);
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
