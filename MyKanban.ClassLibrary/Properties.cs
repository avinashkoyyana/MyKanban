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
 *  Class:      Properties
 *  
 *  Purpose:    Represents a collection of properties that may be associated
 *              with a Board, Project or Task object
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Properties: MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Properties(Credential credential)
        {
            _credential = credential;
            _parent = null;
            _parentId = 0;
        }

        public Properties(IDataItem parentObject, Credential credential)
        {
            PropertiesConstructor(parentObject, credential);
        }

        private void PropertiesConstructor(IDataItem parentObject, Credential credential)
        {
            _credential = credential;
            _parent = parentObject;
            _parentId = parentObject.Id;

            // Load any existing properties
            _items.Clear();
            DataSet ds = Data.GetPropertiesByObject(_parent.GetType().ToString(), _parentId, _credential.Id);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Property property = new Property(_credential);
                    property.Name = dr["name"].ToString();
                    string valueType = dr["value_type"].ToString();
                    property.Id = long.Parse(dr["id"].ToString());
                    property.Value = Convert.ChangeType(dr["value"].ToString(), Type.GetType(valueType));
                    property.Parent = _parent;
                    property.ParentId = _parentId;
                    property.ParentName = _parent.Name;
                    DateTime _created;
                    DateTime _modified;
                    long _createdBy;
                    long _modifiedBy;
                    DateTime.TryParse(dr["created"].ToString(), out _created);
                    DateTime.TryParse(dr["modified"].ToString(), out _modified);
                    long.TryParse(dr["created_by"].ToString(), out _createdBy);
                    long.TryParse(dr["modified_by"].ToString(), out _modifiedBy);
                    property.Created = _created;
                    property.Modified = _modified;
                    property.CreatedBy = _createdBy;
                    property.ModifiedBy = _modifiedBy;
                    _items.Add(property);
                }
            }
        }

        #endregion

        #region Properties

        new public Property this[int index]
        {
            get { return _items[index]; }
            set { _items[index].Value = value; }
        }

        public Property this[string name]
        {
            get 
            {
                List<Property> properties;
                properties = (from property in _items
                         where property.Name == name
                         select property).ToList<Property>();
                if (properties.Count > 0)
                    return properties[0];
                else
                {
                    Property property = new Property(_credential);
                    property.Name = name;
                    property.Value = null;
                    property.Parent = _parent;
                    property.ParentId = _parent.Id;
                    _items.Add(property);
                    return property;
                }
            }
            set 
            {
                List<Property> properties;
                properties = (from property in _items
                              where property.Name == name
                              select property).ToList<Property>();
                if (properties.Count > 0)
                {
                    properties[0].Value = value;
                }
                else
                {
                    Property property = new Property(_credential);
                    property.Name = name;
                    property.Value = value;
                    property.Parent = _parent;
                    property.ParentId = _parent.Id;
                }
            }
        }

        new public int Count
        {
            get { return _items.Count; }
        }

        new private List<Property> _items = new List<Property>();
        new public List<Property> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        #endregion

        #region Methods

        public void Add(Property property)
        {
            _isDirty = true;
            _items.Add(property);
            property.Parent = _parent;
            if (_parent != null) property.ParentId = _parent.Id;
        }

        new public void Clear(bool delete = false)
        {
            if (delete)
            {
                foreach (Property property in _items)
                {
                    property.Delete();
                }
            }
            _items.Clear();
        }

        public override List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in _items)
            {
                BaseItem baseItem = new BaseItem();
                baseItem.Id = item.Id;
                baseItem.Name = item.Name;
                baseItem.Created = item.Created;
                baseItem.CreatedBy = item.CreatedBy;
                baseItem.Modified = item.Modified;
                baseItem.ModifiedBy = item.ModifiedBy;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public override void Reload()
        {
            base.Reload();

            if (_parent != null) PropertiesConstructor(_parent, _credential);
        }

        new public void Remove(int index, bool delete = true)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(string name, bool delete = true)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Name == name) Remove(i, delete);
            }
        }

        new public bool Update(bool force = false)
        {
            try
            {
                foreach (Property property in _items)
                {
                    if (_parentId != 0) property.ParentId = _parentId;
                    property.Update(true);
                }
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
