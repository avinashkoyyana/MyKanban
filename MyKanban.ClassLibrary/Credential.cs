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
    /// <summary>
    /// Represents an authorized user and their permissions for all Boards they have permission to access
    /// </summary>
    public class Credential
    {

        #region Constructors

        /// <summary>
        /// Null constructor
        /// </summary>
        public Credential()
        { }

        /// <summary>
        /// Obtain user credentials based on user name and password
        /// </summary>
        /// <param name="userName">User name stored in database</param>
        /// <param name="password">Password stored in database (in encrypted form)</param>
        public Credential(string userName, string password)
        {
            _id = Data.Login(userName, password);
        }

        #endregion

        #region Properties

        /// <summary>
        /// User name as entered during authentication
        /// </summary>
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

        /// <summary>
        /// Display name of this user
        /// </summary>
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

        /// <summary>
        /// ID# of this user
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Person _person;

        private List<BoardPermissions> _boards = null;

        /// <summary>
        /// List of Boards with associated permissions that this user has access to
        /// </summary>
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

    /// <summary>
    /// Represents a set of permissions that user has for a given Board
    /// </summary>
    public class BoardPermissions
    {
        /// <summary>
        /// Name of Board
        /// </summary>
        public string Name = "";

        /// <summary>
        /// ID# of Board
        /// </summary>
        public long Id = 0;

        /// <summary>
        /// Does user have permission to edit this Board
        /// </summary>
        public bool CanEdit = false;

        /// <summary>
        /// Does user have permission to add items to this Board
        /// </summary>
        public bool CanAdd = false;

        /// <summary>
        /// Does user have permission to delete items from this Board
        /// </summary>
        public bool CanDelete = false;

        /// <summary>
        /// Does user have permission to read data in this board
        /// </summary>
        public bool CanRead = false;    
    }
}
