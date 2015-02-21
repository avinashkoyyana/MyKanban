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
 *  Class:      User
 *  
 *  Purpose:    Represents a single user who may access one or more boards
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class User : BaseItem, IDataItem
    {
        #region Constructors

        public User(Credential credential) 
        {
            if (credential != null) _credential = credential;
            Person = new Person(credential);
        }

        public User(long boardId, long personId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _personId = personId;
            _boardId = boardId;
            Person = new Person(_personId, credential);

            DataSet dsUser = Data.GetBoardUser(boardId, personId, credential.Id);
            if (dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
            {
                DataRow drUser = dsUser.Tables[0].Rows[0];
                _id = long.Parse(drUser["id"].ToString());
                LoadData(true);
            }
        }

        public User(long userId, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsUser = Data.GetBoardUser(userId, credential.Id);
            if (dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
            {
                DataRow drUser = dsUser.Tables[0].Rows[0];
                _personId = long.Parse(drUser["person_id"].ToString());
                _boardId = long.Parse(drUser["board_id"].ToString());
                _id = long.Parse(drUser["id"].ToString());
                Person = new Person(_personId, credential);
                LoadData(true);
            }
        }

        #endregion

        #region Properties

        long _boardId = 0;

        [MyKanban.Description("ID# of board this user has access to")]
        [MyKanban.ReadOnly(true)]
        public long BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        [MyKanban.Description("Date/time created")]
        [MyKanban.ReadOnly(true)]
        public DateTime Created
        {
            get { return Person.Created; }
            set { Person.Created = value; }
        }

        [MyKanban.Description("ID# of user who created this object")]
        [MyKanban.ReadOnly(true)]
        public long CreatedBy
        {
            get { return Person.CreatedBy; }
            set { Person.CreatedBy = value; }
        }

        [MyKanban.Description("Display name of user who created this object")]
        [MyKanban.ReadOnly(true)]
        public string CreatedByName
        {
            get { return Person.CreatedByName; }
        }

        Credential _credential;

        [MyKanban.Hidden(true)]
        public Credential Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        private long _personId = 0;

        [MyKanban.Description("ID# of this user")]
        public long PersonId
        {
            get 
            {
                if (Person != null) _personId = Person.Id;
                return _personId; 
            }
            set 
            { 
                _personId = value;
                Person = new Person(_personId, _credential);
            }
        }

        [MyKanban.Hidden(true)]
        public bool IsDirty
        {
            get { return Person.IsDirty; }
        }

        [MyKanban.Hidden(true)]
        public bool IsLoaded
        {
            get { return Person.IsLoaded; }
        }

        [MyKanban.Description("Date/time object was last modified")]
        [MyKanban.ReadOnly(true)]
        public DateTime Modified
        {
            get { return Person.Modified; }
            set { Person.Modified = value; }
        }

        [MyKanban.Description("ID# of user who last modified this object")]
        [MyKanban.ReadOnly(true)]
        public long ModifiedBy
        {
            get { return Person.ModifiedBy; }
            set { Person.ModifiedBy = value; }
        }

        [MyKanban.Description("Display name of user who last modified this object")]
        [MyKanban.ReadOnly(true)]
        public string ModifiedByName
        {
            get { return Person.ModifiedByName; }
        }

        [MyKanban.Description("Display name of user")]
        [MyKanban.ReadOnly(true)]
        public string Name
        {
            get { return Person.Name; }
        }

        public Person Person;

        bool _canAdd = false;

        [MyKanban.Description("Can this user add tasks to the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; }
        }

        bool _canEdit = false;

        [MyKanban.Description("Can this user edit tasks in the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }

        bool _canDelete = false;

        [MyKanban.Description("Can this user delete tasks from the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; }
        }

        bool _canRead = true;

        [MyKanban.Description("Can this user view this board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanRead
        {
            get { return _canRead; }
            set { _canRead = value; }
        }

        [MyKanban.Description("Login name of this user")]
        [MyKanban.ReadOnly(true)]
        public string UserName
        {
            get { return Person.UserName; }
        }

        #endregion

        #region Methods

        public void Delete() 
        {
            //Person.Delete();
            Data.DeleteUserFromBoard(_boardId, Person.Id, _credential.Id);
        }

        public bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return Person.IsAuthorized(userId, authLevel);
        }

        public string JSON() 
        {
            return Person.JSON();
        }

        public bool LoadData(bool force = false) 
        {
            Person.LoadData(force);
            if (_id > 0)
            {
                DataSet dsUser = Data.GetBoardUser(_id, _credential.Id);
                DataRow drUser = dsUser.Tables[0].Rows[0];
                long.TryParse(drUser["id"].ToString(), out _id);
                long.TryParse(drUser["board_id"].ToString(), out _boardId);
                long.TryParse(drUser["person_id"].ToString(), out _personId);
                bool.TryParse(drUser["can_add"].ToString(), out _canAdd);
                bool.TryParse(drUser["can_edit"].ToString(), out _canEdit);
                bool.TryParse(drUser["can_delete"].ToString(), out _canDelete);
                bool.TryParse(drUser["can_read"].ToString(), out _canRead);
            }
            return true;
        }

        public bool Update(bool force = false) 
        {
            //return Person.Update(force);
            if (_boardId > 0 && _personId > 0)
            {
                DataSet dsUser = Data.AddPersonToBoard(
                    _boardId,
                    Person.Id,
                    _credential.Id,
                    _canAdd,
                    _canEdit,
                    _canDelete,
                    _canRead);

                if (dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                {
                    _id = long.Parse(dsUser.Tables[0].Rows[0]["id"].ToString());
                }
            }
            return true;
        }

        #endregion
    }
}
