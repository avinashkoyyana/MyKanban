using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 *  Class:      Base
 *  
 *  Purpose:    Provides base methods an properties used by all other classes
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Contains properties and methods common to both item and list level classes
    /// </summary>
    public class Base
    {
        #region Properties

        protected Credential _credential = new Credential();

        /// <summary>
        /// Object representing user credentials in MyKanban system
        /// </summary>
        [MyKanban.Description("User credentials with which this object was retrieved or updated")]
        [MyKanban.Hidden(true)]
        public Credential Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        protected bool _isDirty = true;

        /// <summary>
        /// Indicates whether data in this object has changed since last Update()
        /// </summary>
        [MyKanban.Hidden(true)]
        public bool IsDirty
        {
            get { return _isDirty; }
        }

        protected bool _isLoaded = false;

        /// <summary>
        /// Indicates whether all data has been loaded into the object
        /// </summary>
        [MyKanban.Hidden(true)]
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        protected MyKanban.IDataItem _parent;

        /// <summary>
        /// Parent object of this object
        /// </summary>
        [JsonIgnore]
        [MyKanban.Description("Object that owns this object")]
        [MyKanban.ReadOnly(true)]
        [MyKanban.Hidden(true)]
        virtual public MyKanban.IDataItem Parent
        {
            get { return _parent; }
            set { _parent = value; _isDirty = true; }
        }

        protected long _parentId = 0;

        /// <summary>
        /// ID# of the parent of this object
        /// </summary>
        [MyKanban.Description("ID# of parent object")]
        [MyKanban.ReadOnly(true)]
        virtual public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; _isDirty = true; }
        }

        protected string _parentName = "";

        /// <summary>
        /// Name of the parent of this object
        /// </summary>
        [MyKanban.Description("Name of parent object")]
        [MyKanban.ReadOnly(true)]
        virtual public string ParentName
        {
            get
            {
                if (string.IsNullOrEmpty(_parentName) && _parent != null) _parentName = _parent.Name;
                return _parentName;
            }
            set { _parentName = value; _isDirty = true; }
        }

        private string _parentType = "None";

        [MyKanban.Description("The class name of this object's parent")]
        virtual public string ParentType
        {
            get 
            { 
                if (_parent != null) _parentType = _parent.GetType().ToString();
                return _parentType; 
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get JSON for this item
        /// </summary>
        /// <returns>String containing item JSON</returns>
        private string _JSON = "";
        public string JSON()
        {
            if (string.IsNullOrEmpty(_JSON))
            {
                _JSON = Data.GetJson(this);
            }
            return _JSON;
        }

        #endregion
    }
}
