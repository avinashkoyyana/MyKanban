using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MyKanban;

/* ----------------------------------------------------------------------------- /
// File:        MyKanbanWeb.cs
// Purpose:     Provides some utility methods for application
// By:          Mark E. Gerow
// Date:        3/20/2015
// ----------------------------------------------------------------------------- /
// Mode:
// ----------------------------------------------------------------------------- /
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
// -----------------------------------------------------------------------------*/

public static class MyKanbanWeb
{
    public static void SetDbConnection(string dbType, string connectionString)
    {
        if (dbType == "MySql")
        {
            MyKanban.Data.DatabaseType = Data.DbType.MySql;
            MyKanban.Data.MySqlConnectionString = connectionString;
        }
        else
        {
            MyKanban.Data.DatabaseType = Data.DbType.SqlServer;
            MyKanban.Data.SqlServerConnectionString = connectionString;
        }
    }

    public static string SpecialDecoding(string s)
    {
        return s.Replace("~", "&").Replace("`", "#").Replace("^", "+");
    }
}
