using System;
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
 *  Class:      IDataItem
 *  
 *  Purpose:    Interface specifying properties and methods that any
 *              Item-level MyKanban class must implement
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public interface IDataItem
    {

        #region Properties

        DateTime Created { get; set; }
        long CreatedBy { get; set; }
        string CreatedByName { get; }
        Credential Credential { get; set; }
        long Id { get; set; }
        bool IsDirty { get; }
        bool IsLoaded { get; }
        DateTime Modified { get; set; }
        long ModifiedBy { get; set; }
        string ModifiedByName { get; }
        string Name { get; }
        MyKanban.IDataItem Parent { get; }
        long ParentId { get; set; }
        string ParentName { get; }

        #endregion

        #region Methods

        void Delete();
        string JSON();
        bool LoadData(bool force = false);
        bool Update(bool force = false);
        bool IsAuthorized(long userId, Data.AuthorizationType authLevel = Data.AuthorizationType.Read);
        bool IsValid();

        #endregion
    }
}
