using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
 *  Class:      Property
 *  
 *  Purpose:    Represents a single property that may be associated
 *              with a Board, Project or Task object
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Represents a single property key/value pair that may be associated with another object
    /// </summary>
    public class Property : MyKanban.BaseItem, MyKanban.IDataItem
    {
        #region Constructors

        /// <summary>
        /// Create a new Property object instance
        /// </summary>
        /// <param name="credential">Credentials to use when creating Property object</param>
        public Property (Credential credential)
        {
            _credential = credential;
        }

        /// <summary>
        /// Create a new Property object for a given parent object
        /// </summary>
        /// <param name="parentObject">Parent object to create property for</param>
        /// <param name="credential">Credentials to use when creating Property object</param>
        public Property(IDataItem parentObject, Credential credential)
        {
            _credential = credential;
            _parent = parentObject;
            _parentId = parentObject.Id;
        }

        #endregion

        #region Properties

        private object _value = null;

        /// <summary>
        /// Value to assign to thie property
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Type of this property
        /// </summary>
        public string ValueType
        {
            get 
            {
                if (_value != null)
                    return _value.GetType().ToString();
                else
                    return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this property from the database
        /// </summary>
        public override void Delete()
        {
            Data.DeleteProperty(_id, _credential.Id);
        }

        /// <summary>
        /// Load data for this property from the database
        /// </summary>
        /// <param name="force">If true, load this property regardless of its state</param>
        /// <returns>True if property successfully loaded</returns>
        public override bool LoadData(bool force = false)
        {
            // Property data will be loaded in collection constructor
            // not at individual level
            return base.LoadData(force);
        }

        /// <summary>
        /// Write this Property object's data back to the database
        /// </summary>
        /// <param name="force">If true, update the database regardless of the state of this property object</param>
        /// <returns>True if data successfully written</returns>
        public override bool Update(bool force = false)
        {
            if (_id > 0)
            {
                DataSet ds = Data.UpdateProperty(
                    _id, 
                    _name, 
                    _value, 
                    _credential.Id
                    );
            } 
            else
            {
                DataSet ds = Data.AddPropertyToObject(
                    _name,
                    _value,
                    ParentType,
                    _parentId,
                    _credential.Id);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _id = long.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
            }
            return true;
        }
        #endregion

    }
}
