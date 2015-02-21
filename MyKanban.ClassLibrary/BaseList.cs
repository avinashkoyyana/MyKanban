using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

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
 *  Class:      BaseList
 *  
 *  Purpose:    Contains common properties and methods for all list-level classes 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Contains common properties and methods for all list-level
    /// classes
    /// </summary>
    public class BaseList : MyKanban.Base, MyKanban.IDataList
    {
        #region Properties

        virtual public BaseItem this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        [MyKanban.Description("Number of items in this collection")]
        [MyKanban.ReadOnly(true)]
        virtual public int Count
        {
            get { return _items.Count; }
        }

        protected List<BaseItem> _items = new List<BaseItem>();

        [MyKanban.Description("A generic MyKanban collection of items")]
        [MyKanban.ReadOnly(true)]
        virtual public List<BaseItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(BaseItem item)
        {
            _isDirty = true;
            item.Parent = this.Parent;
            item.ParentId = this.ParentId;
            _items.Add(item);
        }

        virtual public void Clear(bool delete = false)
        {
            _items.Clear();
        }

        virtual public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = _items;
            foreach (var item in baseList)
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

        virtual public void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        virtual public void Remove(BaseItem item, bool delete = false)
        {
            _items.Remove(item);
        }

        virtual public bool Update(bool force = false)
        {
            try
            {
                foreach (BaseItem item in _items)
                {
                    if (_parentId != 0) item.ParentId = _parentId;
                    item.Update(true);
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
