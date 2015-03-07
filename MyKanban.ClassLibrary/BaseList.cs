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

        /// <summary>
        /// Indexer for this collection
        /// </summary>
        /// <param name="index">Ordinal position of BaseItem to return</param>
        /// <returns>BaseItem object at the specified index position</returns>
        virtual public BaseItem this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        /// <summary>
        /// Return the # of items in this collection
        /// </summary>
        [MyKanban.Description("Number of items in this collection")]
        [MyKanban.ReadOnly(true)]
        virtual public int Count
        {
            get { return _items.Count; }
        }

        protected List<BaseItem> _items = new List<BaseItem>();

        /// <summary>
        /// Return the underlying List<> upon which this collection is based
        /// </summary>
        [MyKanban.Description("A generic MyKanban collection of items")]
        [MyKanban.ReadOnly(true)]
        virtual public List<BaseItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new item to this collection
        /// </summary>
        /// <param name="item">Item to be added</param>
        virtual public void Add(BaseItem item)
        {
            _isDirty = true;
            item.Parent = this.Parent;
            item.ParentId = this.ParentId;
            _items.Add(item);
        }

        /// <summary>
        /// Remove all items from this collection
        /// </summary>
        /// <param name="delete">If true, delete all item data from database as well as removing them from the list</param>
        virtual public void Clear(bool delete = false)
        {
            _items.Clear();
        }

        /// <summary>
        /// Get a List<> containing basic data for all items in this collection
        /// </summary>
        /// <returns>A List<BaseItem> object containing all items in the list</BaseItem></returns>
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

        /// <summary>
        /// Remove item at the specified index position from the collection
        /// </summary>
        /// <param name="index">Ordinal position of the item to be removed</param>
        /// <param name="delete">If true, also delete the item's data from the database</param>
        virtual public void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        /// <summary>
        /// Remove a given item from the collection
        /// </summary>
        /// <param name="item">Object representing the item to be removed</param>
        /// <param name="delete">If true, delete the item's data from the database</param>
        virtual public void Remove(BaseItem item, bool delete = false)
        {
            _items.Remove(item);
        }

        /// <summary>
        /// Update the database with data from all items in this collection
        /// </summary>
        /// <param name="force">If true, update the database regardless of the state of the individual items</param>
        /// <returns>True if database is successfully updated</returns>
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
