using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using MySql.Data.MySqlClient;

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
 *  Class:      IDataList
 *  
 *  Purpose:    Interface specifying all properties and methods that any
 *              list-level MyKanban class must implement
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// Specifies contract that all MyKanban list objects must meet
    /// </summary>
    public interface IDataList
    {

        #region Properties

        Credential Credential { get; set; }
        bool IsDirty { get; }
        bool IsLoaded { get; }
        int Count { get; }
        List<BaseItem> Items { get; }
        MyKanban.IDataItem Parent { get; set; }
        long ParentId { get; }

        #endregion

        #region Methods

        void Clear(bool delete = false);
        string JSON();
        void Reload();
        void Remove(int index, bool delete = false);
        bool Update(bool force = false);

        #endregion

    }
}
