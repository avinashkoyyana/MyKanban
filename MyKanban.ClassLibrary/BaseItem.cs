﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 *  Class:      BaseItem
 *  
 *  Purpose:    Provides properties and methods common to all item-level classes
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Contains common methods and properties for all 
    /// item-level classes
    /// </summary>
    public class BaseItem : MyKanban.Base
    {
        #region Properties

        /// <summary>
        /// Unique ID# for this object in the database
        /// </summary>
        protected long _id = 0;

        /// <summary>
        /// Unique ID# of this item
        /// </summary>
        [MyKanban.Description("Unique ID# of this object in the database")]
        [MyKanban.ReadOnly(true)]
        virtual public long Id
        {
            get { return _id; }
            set { _id = value; _isDirty = true; }
        }

        protected string _name = "";

        /// <summary>
        /// Display name of this item
        /// </summary>
        [MyKanban.Description("Display name")]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _isDirty = true;
            }
        }

        protected DateTime _created;

        /// <summary>
        /// Date &amp; time this item was added to the system
        /// </summary>
        [MyKanban.Description("Date/time created")]
        [MyKanban.ReadOnly(true)]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        protected long _createdBy = 0;

        /// <summary>
        /// ID# of the user who added this item
        /// </summary>
        [MyKanban.Description("ID# of user who created this object")]
        [MyKanban.ReadOnly(true)]
        public long CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        protected string _createdByName = "";

        /// <summary>
        /// Name of the user who added this item
        /// </summary>
        [MyKanban.Description("Display name of user who created this object")]
        [MyKanban.ReadOnly(true)]
        public string CreatedByName
        {
            get
            {
                if (_createdBy > 0 && string.IsNullOrEmpty(_createdByName))
                {
                    Person createdBy = new Person(_createdBy, _credential);
                    _createdByName = createdBy.Name;
                }
                return _createdByName;
            }
        }

        protected long _modifiedBy = 0;

        /// <summary>
        /// ID# of user who last modified this item
        /// </summary>
        [MyKanban.Description("ID# of user who last modified this object")]
        [MyKanban.ReadOnly(true)]
        public long ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        protected DateTime _modified;

        /// <summary>
        /// Date &amp; time when this item was last modified
        /// </summary>
        [MyKanban.Description("Date/time object was last modified")]
        [MyKanban.ReadOnly(true)]
        public DateTime Modified
        {
            get { return _modified; }
            set { _modified = value; }
        }

        protected string _modifiedByName = "";

        /// <summary>
        /// Name of user who last modified this item
        /// </summary>
        [MyKanban.Description("Display name of user who last modified this object")]
        [MyKanban.ReadOnly(true)]
        public string ModifiedByName
        {
            get
            {
                if (_modifiedBy > 0 && string.IsNullOrEmpty(_modifiedByName))
                {
                    Person modifiedBy = new Person(_modifiedBy, _credential);
                    _modifiedByName = modifiedBy.Name;
                }
                return _modifiedByName;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this item from its parent collection
        /// </summary>
        virtual public void Delete()
        {
            // No Op - override in child class to perform operation
        }

        /// <summary>
        /// Is the current user authorized to perform the requestion operation
        /// </summary>
        /// <param name="id">ID# of user requesting operation</param>
        /// <param name="authType">Type of authorization being requested.  Note: currently this method always returns true.</param>
        /// <returns></returns>
        virtual public bool IsAuthorized(long id, Data.AuthorizationType authType)
        {
            // No Op - override in child class to perform operation
            return true;
        }

        /// <summary>
        /// Is the data in the object valid.
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        virtual public bool IsValid()
        {
            // No Op - override in child class to perform operation
            return true;
        }

        /// <summary>
        /// Placeholder method for class-specific version to populate object with data from database.
        /// </summary>
        /// <param name="force">If true, read data from database regardless of state of object.</param>
        /// <returns>True of data successfully loaded</returns>
        virtual public bool LoadData(bool force = false)
        {
            // No Op - override in child class to perform operation
            return true;
        }

        /// <summary>
        /// Placeholder method for class-specific version to save data from object back to database.
        /// </summary>
        /// <param name="force">If true, write data back to database regardless of state of object.</param>
        /// <returns>True if data successfully written</returns>
        virtual public bool Update(bool force = false)
        {
            // No Op - override in child class to perform operation
            return true;
        }

        #endregion
    }
}
