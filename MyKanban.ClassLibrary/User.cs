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
    /// <summary>
    /// Represents a single user who may access one or more boards
    /// </summary>
    public class User : BaseItem, IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new empty User object
        /// </summary>
        /// <param name="credential">Credentials to use when creating this User object</param>
        public User(Credential credential) 
        {
            if (credential != null) _credential = credential;
            Person = new Person(credential);
        }

        /// <summary>
        /// Create a new User object and populate its data from database based
        /// on provided board and person IDs
        /// </summary>
        /// <param name="boardId">ID# of board</param>
        /// <param name="personId">ID# of person</param>
        /// <param name="credential">Credentials to use when creating this User object</param>
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

        /// <summary>
        /// Create a new User object and populate its data from database based
        /// on the ID# provided
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="credential">Credentials to use when creating this User object</param>
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

        /// <summary>
        /// ID# of board this user has access to
        /// </summary>
        [MyKanban.Description("ID# of board this user has access to")]
        [MyKanban.ReadOnly(true)]
        public long BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        /// <summary>
        /// Date/time underlying Person object created
        /// </summary>
        [MyKanban.Description("Date/time created")]
        [MyKanban.ReadOnly(true)]
        public DateTime Created
        {
            get { return Person.Created; }
            set { Person.Created = value; }
        }

        /// <summary>
        /// ID# of user who created underlying Person object 
        /// </summary>
        [MyKanban.Description("ID# of user who created underlying Person object")]
        [MyKanban.ReadOnly(true)]
        public long CreatedBy
        {
            get { return Person.CreatedBy; }
            set { Person.CreatedBy = value; }
        }

        /// <summary>
        /// Display name of user who created underlying Person object
        /// </summary>
        [MyKanban.Description("Display name of user who created this object")]
        [MyKanban.ReadOnly(true)]
        public string CreatedByName
        {
            get { return Person.CreatedByName; }
        }

        Credential _credential;

        /// <summary>
        /// Credential associated with this object
        /// </summary>
        [MyKanban.Hidden(true)]
        public Credential Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        private long _personId = 0;

        /// <summary>
        /// ID# of underlying Person object
        /// </summary>
        [MyKanban.Description("ID# of underlying Person object")]
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

        /// <summary>
        /// iDirty value of underlying Person object
        /// </summary>
        [MyKanban.Hidden(true)]
        public bool IsDirty
        {
            get { return Person.IsDirty; }
        }

        /// <summary>
        /// isLoaded value of underlying Person object
        /// </summary>
        [MyKanban.Hidden(true)]
        public bool IsLoaded
        {
            get { return Person.IsLoaded; }
        }

        /// <summary>
        /// Date/time underlying Person object was last modified
        /// </summary>
        [MyKanban.Description("Date/time underlying Person object was last modified")]
        [MyKanban.ReadOnly(true)]
        public DateTime Modified
        {
            get { return Person.Modified; }
            set { Person.Modified = value; }
        }

        /// <summary>
        /// ID# of user who last modified underlying Person object
        /// </summary>
        [MyKanban.Description("ID# of user who last modified underlying Person object")]
        [MyKanban.ReadOnly(true)]
        public long ModifiedBy
        {
            get { return Person.ModifiedBy; }
            set { Person.ModifiedBy = value; }
        }

        /// <summary>
        /// Display name of user who last modified underlying Person object
        /// </summary>
        [MyKanban.Description("Display name of user who last modified underlying Person object")]
        [MyKanban.ReadOnly(true)]
        public string ModifiedByName
        {
            get { return Person.ModifiedByName; }
        }

        /// <summary>
        /// Display name of underlying Person
        /// </summary>
        [MyKanban.Description("Display name of underlying Person")]
        [MyKanban.ReadOnly(true)]
        public string Name
        {
            get { return Person.Name; }
        }

        /// <summary>
        /// Underlying Person object
        /// </summary>
        public Person Person;

        bool _canAdd = false;

        /// <summary>
        /// Can this user add tasks to the board
        /// </summary>
        [MyKanban.Description("Can this user add tasks to the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; }
        }

        bool _canEdit = false;

        /// <summary>
        /// Can this user edit tasks in the board
        /// </summary>
        [MyKanban.Description("Can this user edit tasks in the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }

        bool _canDelete = false;

        /// <summary>
        /// Can this user delete tasks from the board
        /// </summary>
        [MyKanban.Description("Can this user delete tasks from the board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; }
        }

        bool _canRead = true;

        /// <summary>
        /// Can this user view this board
        /// </summary>
        [MyKanban.Description("Can this user view this board")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanRead
        {
            get { return _canRead; }
            set { _canRead = value; }
        }

        /// <summary>
        /// Login name of this user
        /// </summary>
        [MyKanban.Description("Login name of the underlying Person")]
        [MyKanban.ReadOnly(true)]
        public string UserName
        {
            get { return Person.UserName; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this user from its parent board
        /// </summary>
        public override void Delete() 
        {
            Data.DeleteUserFromBoard(_boardId, Person.Id, _credential.Id);
        }

        /// <summary>
        /// Does specified user have permission to perform the requested operation
        /// </summary>
        /// <param name="userId">ID# of user</param>
        /// <param name="authLevel">Requested operation</param>
        /// <returns>True if user has permission to perform the requested operation</returns>
        public override bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return Person.IsAuthorized(userId, authLevel);
        }

        /// <summary>
        /// Return JSON for underlying Person object
        /// </summary>
        /// <returns>JSON of Person object</returns>
        public override string JSON() 
        {
            return Person.JSON();
        }

        /// <summary>
        /// Populate the Userk instance with data from the database
        /// </summary>
        /// <param name="force">If true, populate this User regardless of state</param>
        /// <returns>True if data was successfully loaded</returns>
        public override bool LoadData(bool force = false) 
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

        /// <summary>
        /// Update the database with data from this object instance
        /// </summary>
        /// <param name="force">If true, save data to database regardless of the state of this StatusCode object</param>
        /// <returns>True if data successfully written to database</returns>
        public override bool Update(bool force = false) 
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
