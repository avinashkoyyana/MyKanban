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
 *  Class:      Credential
 *  
 *  Purpose:    A security credential provided to user after successful login 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Credential
    {

        #region Constructors

        public Credential()
        { }

        public Credential(string userName, string password)
        {
            _id = Data.Login(userName, password);
        }

        #endregion

        #region Properties

        public string UserName
        {
            get 
            {
                if (_id > 0)
                {
                    _person = new Person(_id, this);
                    return _person.UserName;
                } 
                else
                {
                    return "";
                }
            }
        }

        public string Name
        {
            get
            {
                if (_id > 0)
                {
                    _person = new Person(_id, this);
                    return _person.Name;
                }
                else
                {
                    return "";
                }
            }
        }

        private long _id = 0;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Person _person;

        private List<BoardPermissions> _boards = null;

        [JsonIgnore]
        public List<BoardPermissions> Boards
        {
            get 
            {
                if (_boards == null)
                {
                    _boards = new List<BoardPermissions>();
                    if (_id > 0)
                    {
                        _person = new Person(_id, this);

                        // Get all boards this person has access to
                        DataSet dsBoards = Data.GetBoardsByUserId(_id);
                        //_boards = new Boards();

                        foreach (DataRow drBoard in dsBoards.Tables[0].Rows)
                        {
                            long boardId = (long)drBoard["id"];
                            Board _board = new Board(boardId, this);
                            BoardPermissions boardPermissions = new BoardPermissions();
                            boardPermissions.Name = _board.Name;
                            boardPermissions.Id = _board.Id;
                            boardPermissions.CanRead = (bool)drBoard["can_read"];
                            boardPermissions.CanAdd = (bool)drBoard["can_add"];
                            boardPermissions.CanEdit = (bool)drBoard["can_edit"];
                            boardPermissions.CanDelete = (bool)drBoard["can_delete"];
                            _boards.Add(boardPermissions);
                        }
                    }
                }
                return _boards; 
            }
        }

        #endregion

    }

    public class BoardPermissions
    {
        public string Name = "";
        public long Id = 0;
        public bool CanEdit = false;
        public bool CanAdd = false;
        public bool CanDelete = false;
        public bool CanRead = false;    
    }
}
