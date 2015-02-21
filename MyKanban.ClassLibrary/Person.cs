using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Diagnostics;

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
 *  Class:      Person
 *  
 *  Purpose:    Represents a single person tracked by the system and available
 *              to login, or assign one or more roles
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Person : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        public Person(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Person(long personId, Credential credential)
        {
            if (credential != null) _credential = credential;

            _id = personId;
            LoadData();
        }
        #endregion

        #region Properties

        bool _canLogin = false;

        [MyKanban.Description("Indicates whether this user has login privileges")]
        [MyKanban.ControlType(enumControlType.Boolean)]
        public bool CanLogin
        {
            get { return _canLogin; }
            set { _canLogin = value; _isDirty = true; }
        }

        string _email = "";
        public string Email
        {
            get { return _email; }
            set { _email = value; _isDirty = true; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _password = "";

        [MyKanban.Hidden(true)]
        public string Password
        {
            set { _password = EncDec.Encrypt(value, Data.encryptionKey); _isDirty = true; }
        }

        string _pictureUrl = "";
        public string PictureUrl
        {
            get { return _pictureUrl; }
            set { _pictureUrl = value; _isDirty = true; }
        }

        string _phone = "";
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; _isDirty = true; }
        }

        private Properties _properties = null;

        [MyKanban.Description("Collection of properties associated with this person")]
        public Properties Properties
        {
            get
            {
                if (_properties == null)
                {
                    if (_id > 0)
                    {
                        _properties = new Properties(this, _credential);
                    }
                    else
                    {
                        _properties = new Properties(_credential);
                    }
                }
                return _properties;
            }
        }

        string _userName = "";
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName)) _userName = Guid.NewGuid().ToString();
                return _userName;
            }
            set { _userName = value; _isDirty = true; }
        }
        #endregion

        #region Methods

        public void Delete()
        {
            Data.DeletePerson(_id, _credential.Id);

            // Delete any associated properties
            foreach (Property property in Properties.Items)
            {
                property.Delete();
            }
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
                    DataSet dsPerson = MyKanban.Data.GetPersonById(_id, _credential.Id);
                    DataTable dtPerson = dsPerson.Tables[0];

                    // Assign to properties
                    _name = dtPerson.Rows[0]["name"].ToString();
                    _pictureUrl = dtPerson.Rows[0]["picture_url"].ToString();
                    _email = dtPerson.Rows[0]["email"].ToString();
                    _phone = dtPerson.Rows[0]["phone"].ToString();
                    _userName = dtPerson.Rows[0]["user_name"].ToString();
                    _password = dtPerson.Rows[0]["password"].ToString();
                    try
                    {
                        _canLogin = (dtPerson.Rows[0]["can_login"].ToString() == "1");
                    }
                    catch { }

                    DateTime.TryParse(dtPerson.Rows[0]["created"].ToString(), out _created);
                    DateTime.TryParse(dtPerson.Rows[0]["modified"].ToString(), out _modified);
                    long.TryParse(dtPerson.Rows[0]["created_by"].ToString(), out _createdBy);
                    long.TryParse(dtPerson.Rows[0]["modified_by"].ToString(), out _modifiedBy);

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
                        // Save data for existing object
                        Data.UpdatePerson(
                            _id,
                            _name,
                            _pictureUrl,
                            _email,
                            _phone,
                            _userName,
                            _password,
                            _canLogin, _credential.Id
                            );
                    }
                    else
                    {
                        // Add a new object
                        DataSet dsNewPerson = Data.AddPerson(
                            _name,
                            _pictureUrl,
                            _email,
                            _phone,
                            _userName,
                            _password,
                            _canLogin, _credential.Id
                            );

                        _id = long.Parse(dsNewPerson.Tables[0].Rows[0]["id"].ToString());
                    }

                    // Reload the data for this board
                    LoadData(true);
                }
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
