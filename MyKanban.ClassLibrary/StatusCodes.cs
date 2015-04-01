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
 *  Class:      StatusCodes
 *  
 *  Purpose:    Represents a collection of StatusCode objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class StatusCodes : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public StatusCodes(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public StatusCodes(BoardSet boardSet, Credential credential)
        {
            StatusCodesBoardSetConstructor(boardSet, credential);
        }

        public StatusCodes(long boardSetId, Credential credential)
        {
            StatusCodesBoardSetIdConstructor(boardSetId, credential);
        }

        private void StatusCodesBoardSetConstructor(BoardSet boardSet, Credential credential)
        {
            if (credential != null) _credential = credential;

            _items.Clear();
            DataSet dsStatusCodes = MyKanban.Data.GetStatusCodesByBoardSet(boardSet.Id, _credential.Id);
            foreach (DataRow drStatusCode in dsStatusCodes.Tables["results"].Rows)
            {
                StatusCode statusCode = new StatusCode(long.Parse(drStatusCode["id"].ToString()), _credential);
                _items.Add(statusCode);
            }
            _parent = boardSet;
            _parentId = boardSet.Id;
        }

        private void StatusCodesBoardSetIdConstructor(long boardSetId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _items.Clear();
            DataSet dsStatusCodes = MyKanban.Data.GetStatusCodesByBoardSet(boardSetId, _credential.Id);
            foreach (DataRow drStatusCode in dsStatusCodes.Tables["results"].Rows)
            {
                StatusCode statusCode = new StatusCode(long.Parse(drStatusCode["id"].ToString()), _credential);
                _items.Add(statusCode);
            }
            _parentId = boardSetId;
        }

        public StatusCodes(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsStatusCodes = MyKanban.Data.GetStatusCodesByName(nameFilter, _credential.Id);
            foreach (DataRow drStatusCode in dsStatusCodes.Tables["results"].Rows)
            {
                StatusCode statusCode = new StatusCode(long.Parse(drStatusCode["id"].ToString()), _credential);
                _items.Add(statusCode);
            }
        }

        #endregion

        #region Properties

        public StatusCode this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        Credential _credential = new Credential();
        public Credential Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }
        
        bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
        }

        bool _isLoaded = false;
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        private List<StatusCode> _items = new List<StatusCode>();
        public List<StatusCode> Items
        {
            get { return _items; }
        }

        private MyKanban.IDataItem _parent;

        [JsonIgnore]
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

        #endregion

        #region Methods

        public void Add(StatusCode statusCode)
        {
            _isDirty = true;
            statusCode.ParentId = _parentId;
            _items.Add(statusCode);
        }

        public void Clear(bool delete = false)
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
                BaseItem baseItem = (StatusCode)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public override void Reload()
        {
            base.Reload();

            if (_parent != null) StatusCodesBoardSetConstructor((BoardSet)_parent, _credential);
        }

        public void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(StatusCode statusCode, bool delete = false)
        {
            _items.Remove(statusCode);
        }

        public bool Update(bool force = false)
        {
            try
            {
                // TODO: handle update
                foreach (StatusCode statusCode in _items)
                {
                    if (_parentId != 0) statusCode.ParentId = _parentId;
                    statusCode.Update();
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
