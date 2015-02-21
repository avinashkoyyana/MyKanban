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
 *  Class:      BoardSets
 *  
 *  Purpose:    Represents a collection of BoardSet objects 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class BoardSets : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public BoardSets(Credential credential)
        {
            if (credential != null) _credential = credential;
        }

        public BoardSets(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsBoardSsets = MyKanban.Data.GetBoardSetsByName(nameFilter, _credential.Id);
            foreach (DataRow drBoardSet in dsBoardSsets.Tables["results"].Rows)
            {
                BoardSet boardSet = new BoardSet(long.Parse(drBoardSet["id"].ToString()), _credential);
                _items.Add(boardSet);
            }
        }

        #endregion

        #region Properties

        public BoardSet this[int index]
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

        private List<BoardSet> _items = new List<BoardSet>();
        public List<BoardSet> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(BoardSet boardSet)
        {
            _items.Add(boardSet);
            _isDirty = true;
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
                BaseItem baseItem = (BoardSet)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public void Remove(BoardSet boardSet)
        {
            boardSet.Parent = null;
            boardSet.Update();

            _items.Remove(boardSet);
        }

        public void Remove(int index)
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
                    _items[i].Update();
                }
                return true; 
            }
            catch { return false; }
        }

        #endregion

    }
}
