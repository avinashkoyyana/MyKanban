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
 *  Class:      Users
 *  
 *  Purpose:    Represents a collection of User objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Users : BaseList, IDataList
    {
        #region Constructors

        public Users(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Users(Board board, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsUsers = MyKanban.Data.GetPeopleByBoard(board.Id, credential.Id);
            foreach (DataRow drUser in dsUsers.Tables["results"].Rows)
            {
                User user = new User(board.Id, long.Parse(drUser["person_id"].ToString()), _credential);
                _items.Add(user);
            }
            _parent = board;
            _parentId = board.Id;
        }

        #endregion

        #region Properties

        public User this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        private List<User> _items = new List<User>();
        public List<User> Items
        {
            get { return _items; }
        }

        private long _boardId = 0;
        public long BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        #endregion

        #region Methods

        public void Add(User user)
        {
            _isDirty = true;
            _items.Add(user);

            if (_parent != null)
            {
                user.Parent = _parent;
                _parentId = _parent.Id;
                _boardId = _parent.Id;
                user.BoardId = _boardId;
            }

            user.Update(true);

            Data.AddPersonToBoard(
                _parentId,
                user.Id,
                _credential.Id,
                user.CanAdd,
                user.CanEdit,
                user.CanDelete,
                user.CanRead);
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
                BaseItem baseItem = new BaseItem();
                baseItem.Id = item.Id;
                baseItem.Name = item.Name;
                baseItem.Created = item.Created;
                baseItem.CreatedBy = item.CreatedBy;
                baseItem.Modified = item.Modified;
                baseItem.ModifiedBy = item.ModifiedBy;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public void Remove(int index)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(User user)
        {
            _items.Remove(user);
            user.Delete();
        }

        public bool Update(bool force = false)
        {
            try
            {
                // TODO: handle update
                foreach (User User in _items)
                {
                    if (_parentId != 0)
                    {
                        User.ParentId = _parentId;
                        User.BoardId = _boardId;
                    }
                    User.Update(true);
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
