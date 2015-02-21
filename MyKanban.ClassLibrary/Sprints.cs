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
 *  Class:      Sprints
 *  
 *  Purpose:    Represents a collection of sprints
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Sprints : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Sprints(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Sprints(Board board, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsSprints = MyKanban.Data.GetSprintsByBoard(board.Id, _credential.Id);
            foreach (DataRow drSprint in dsSprints.Tables["results"].Rows)
            {
                Sprint sprint = new Sprint(long.Parse(drSprint["id"].ToString()), _credential);
                _items.Add(sprint);
            }
            //_parentId = boardId;
            _parent = board;
            _parentId = board.Id;
        }

        #endregion

        #region Properties

        public Sprint this[int index]
        {
            get { return _items[index]; }
            set { 
                _items[index] = value;
                _isDirty = true;
            }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        private List<Sprint> _items = new List<Sprint>();
        public List<Sprint> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(Sprint sprint)
        {
            _items.Add(sprint);
            _isDirty = true;
        }

        public void Clear(bool delete = false)
        {
            _items.Clear();
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

        public void Remove(Sprint sprint, bool delete = false)
        {
            sprint.Parent = null;
            sprint.Update();

            _items.Remove(sprint);
        }

        public void Remove(int index, bool delete = false)
        {
            _items[index].Parent = null;
            _items[index].Update();

            _items.Remove(_items[index]);
        }

        public bool Update(bool force = false)
        {
            try 
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_parent != null)
                    {
                        _items[i].Parent = _parent;
                        _items[i].ParentId = _parent.Id;
                    }
                    _items[i].Update();
                }
                return true; 
            }
            catch { return false; }
        }

        #endregion
    }
}
