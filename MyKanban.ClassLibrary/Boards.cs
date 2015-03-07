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
 *  Class:      Boards
 *  
 *  Purpose:    Represents a collection of Board objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Boards : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Boards(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Boards(BoardSet boardSet, Credential credential)
        {
            BoardsBoardSetConstructor(boardSet, credential);
        }

        public void BoardsBoardSetConstructor(BoardSet boardSet, Credential credential)
        {
            if (credential != null) _credential = credential;
            _items.Clear();
            DataSet dsBoards = MyKanban.Data.GetBoardsByBoardSet(boardSet.Id, _credential.Id);
            foreach (DataRow drBoard in dsBoards.Tables["results"].Rows)
            {
                Board board = new Board(long.Parse(drBoard["id"].ToString()), _credential);
                if (board.IsAuthorized(_credential.Id, Data.AuthorizationType.Read))
                {
                    _items.Add(board);
                }
            }

            _parent = boardSet;
            _parentId = boardSet.Id;
        }

        public Boards(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsBoards = MyKanban.Data.GetBoardsByName(nameFilter, _credential.Id);
            foreach (DataRow drBoard in dsBoards.Tables["results"].Rows)
            {
                Board board = new Board(long.Parse(drBoard["id"].ToString()), _credential);
                _items.Add(board);
            }
        }

        #endregion

        #region Properties

        public Board this[int index]
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
        
        private List<Board> _items = new List<Board>();
        public List<Board> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(Board board)
        {
            _items.Add(board);
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
                BaseItem baseItem = (Board)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public override void Reload()
        {
            base.Reload();

            if (_parent != null) BoardsBoardSetConstructor((BoardSet)_parent, _credential);
        }

        public void Remove(Board board, bool delete = false)
        {
            board.Parent = null;
            long boardId = board.Id;
            foreach (Board b in _items)
            {
                if (b.Id == boardId) board = b;
            }
            _items.Remove(board);
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
                    _items[i].ParentId = this.ParentId;
                    _items[i].Update();
                }
                return true; 
            }
            catch { return false; }
        }

        #endregion

    }
}
