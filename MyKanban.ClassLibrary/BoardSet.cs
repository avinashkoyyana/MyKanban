using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
 *  Class:      BoardSet
 *  
 *  Purpose:    Represents a sing board set 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class BoardSet : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        public BoardSet(Credential credential)
        {
            if (credential != null) _credential = credential;

            _boards = new Boards(_credential);
            _statusCodes = new StatusCodes(_credential);
        }

        public BoardSet(long boardSetId, Credential credential)
        {
            _id = boardSetId;
            _credential = credential;
            _boards = new Boards(_credential);
            _statusCodes = new StatusCodes(_credential);

            LoadData();
        }

        #endregion

        #region Properties

        private Boards _boards;
        public Boards Boards
        {
            get
            {
                LoadData();
                return _boards;
            }
        }
        
        long _boardSetId = 0;
        public long BoardSetId
        {
            get
            {
                LoadData();
                return _boardSetId;
            }
            set
            {
                _boardSetId = value;
                _isDirty = true;
            }
        }

        string _boardSetName = "";
        public string BoardSetName
        {
            get
            {
                return _name;
            }
        }

        StatusCodes _statusCodes;
        public StatusCodes StatusCodes
        {
            get
            {
                LoadData();
                return _statusCodes;
            }
        }

        #endregion

        #region Methods

        public void Delete()
        {
            // First delete any boards that are part of this board set
            Boards boards = new Boards(this, _credential);
            foreach (Board board in boards.Items)
            {
                board.Delete();
            } 
            
            // Now delete the parent board
            Data.DeleteBoardSet(_id, _credential.Id);
        }

        public bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read)
        {
            return true;
        }

        public bool IsValid()
        {
            return true;
        }

        public bool LoadData(bool force = false)
        {
            try
            {
                if (!_isLoaded || force)
                {
                    // Basic data
                    DataSet dsBoardSet = MyKanban.Data.GetBoardSetById(_id, _credential.Id);
                    if (dsBoardSet != null && dsBoardSet.Tables.Count > 0 && dsBoardSet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtBoardSet = dsBoardSet.Tables[0];
                        _name = dtBoardSet.Rows[0]["name"].ToString();
                        _boardSetId = long.Parse(dtBoardSet.Rows[0]["id"].ToString());

                        DateTime.TryParse(dtBoardSet.Rows[0]["created"].ToString(), out _created);
                        DateTime.TryParse(dtBoardSet.Rows[0]["modified"].ToString(), out _modified);
                        long.TryParse(dtBoardSet.Rows[0]["created_by"].ToString(), out _createdBy);
                        long.TryParse(dtBoardSet.Rows[0]["modified_by"].ToString(), out _modifiedBy);
                    }
                    else
                    {
                        _id = 0;
                    }

                    if (_id > 0)
                    {
                        _boards = new Boards(this, _credential);
                        _statusCodes = new StatusCodes(this, _credential);
                    }
                    else
                    {
                        _boards = new Boards(_credential);
                        _statusCodes = new StatusCodes(_credential);
                    }
                }

                _isLoaded = true;
                _isDirty = false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(bool force = false)
        {
            try
            {
                if (_isDirty || force)
                {
                    if (_id > 0)
                    {
                        // Save data for existing board
                        Data.UpdateBoardSet(_id, _name, _credential.Id);
                    }
                    else
                    {
                        // Add a new board
                        DataSet dsNewBoardSet = Data.AddBoardSet(_name, _credential.Id);
                        _id = long.Parse(dsNewBoardSet.Tables[0].Rows[0]["id"].ToString());
                        DateTime.TryParse(dsNewBoardSet.Tables[0].Rows[0]["created"].ToString(), out _created);
                        long.TryParse(dsNewBoardSet.Tables[0].Rows[0]["created_by"].ToString(), out _createdBy);
                        DateTime.TryParse(dsNewBoardSet.Tables[0].Rows[0]["modified"].ToString(), out _modified);
                        long.TryParse(dsNewBoardSet.Tables[0].Rows[0]["modified_by"].ToString(), out _modifiedBy);
                    }
                }

                // Save any constituent boards
                foreach (Board board in _boards.Items)
                {
                    board.ParentId = _id;
                    board.Update(true);
                }

                // Save any constituent status codes
                foreach (StatusCode statusCode in _statusCodes.Items)
                {
                    statusCode.Parent = this;
                    statusCode.ParentId = _id;
                    statusCode.Update(true);
                }

                // Reload the data for this board
                LoadData(true);

                _isDirty = false;
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
