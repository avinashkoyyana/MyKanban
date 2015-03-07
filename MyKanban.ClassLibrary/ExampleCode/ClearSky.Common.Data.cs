using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data.Common;
using System.IO;

namespace ClearSky.Common.Data
{
    //public class ClearSkyFileInfo
    //{
    //    public FileInfo Info;
    //    public long Length;
    //    public DateTime CreationTime;
    //    public string Name;

    //    public ClearSkyFileInfo() { }

    //    public ClearSkyFileInfo(string filePath)
    //    {
    //        FileInfo fileInfo = new FileInfo(filePath);
    //        this.Info = fileInfo;
    //        this.Length = fileInfo.Length;
    //        this.Name = fileInfo.Name;
    //        this.CreationTime = fileInfo.CreationTime;
    //    }
    //}

    //public class PropertySet
    //{
    //    public string Properties;
    //    public string Type;
    //}

    //public static class DataAccess
    //{
    //    /// <summary>
    //    /// Connection string to use when connecting to MySql database
    //    /// </summary>
    //    public static string MySqlConnectionString
    //    {
    //        get
    //        {
    //            //return "server=markgwin7-pc;database=clearsky;port=3306;uid=mgerow;pwd=mgerow";
    //            return "server=devdbtest2;database=clearsky;port=3306;uid=mysqlsvc;pwd=St0mp3r";
    //        }
    //    }

    //    /// <summary>
    //    /// Add a new document object to the database
    //    /// </summary>
    //    /// <param name="path">Path to the document (e.g. c:\mydocs)</param>
    //    /// <param name="name">Name of document (e.g. mydoc.xlsx</param>
    //    /// <param name="authorId">ID# of the user adding the document</param>
    //    /// <param name="description">Description of the document</param>
    //    /// <returns>ID# of the newly added document object</returns>
    //    public static long AddDocument(string path, string name, long authorId, string description, long proxy = 0)
    //    {

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("path", path);
    //        parameters.Add("name", name);
    //        parameters.Add("auth_id", authorId);
    //        parameters.Add("description", description);
    //        parameters.Add("dproxy", proxy);

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_document", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                return long.Parse(dt.Rows[0]["id"].ToString());
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Add a new folder to the database
    //    /// </summary>
    //    /// <param name="name">Name of the folder</param>
    //    /// <param name="authorId">ID# of the user creating this folder</param>
    //    /// <param name="description">Description of this folder</param>
    //    /// <returns>ID# of the newly added folder object</returns>
    //    public static long AddFolder(string name, long authorId, string description)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("name", name);
    //        parameters.Add("auth_id", authorId);
    //        parameters.Add("description", description);

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_folder", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                return long.Parse(dt.Rows[0]["id"].ToString());
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Add an item (focument, sub-folder, or other object type) to the specified folder
    //    /// </summary>
    //    /// <param name="folderId">ID# of the folder to which the item is being added</param>
    //    /// <param name="objectId">ID# of the object being added to the folder</param>
    //    /// <returns>ID# of the newly added folder item</returns>
    //    public static long AddFolderItem(long folderId, long objectId)
    //    {

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("col_obj_id", folderId);
    //        parameters.Add("memb_obj_id", objectId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_folder_item", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                return long.Parse(dt.Rows[0]["id"].ToString());
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Add a group to the database
    //    /// </summary>
    //    /// <param name="name">Name of the group</param>
    //    /// <param name="authorId">ID# of the user adding this group</param>
    //    /// <param name="description">Description of this group</param>
    //    /// <returns>ID# of the newly added group</returns>
    //    public static long AddGroup(string name, long authorId, string description)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("gname", name);
    //        parameters.Add("descrip", description);
    //        parameters.Add("aid", authorId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_group", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                return long.Parse(dt.Rows[0]["id"].ToString());
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Add a new proxy to the database
    //    /// </summary>
    //    /// <param name="name">Name of the proxy</param>
    //    /// <param name="proxy">Proxy to add</param>
    //    /// <returns></returns>
    //    public static long AddProxy(string name, object proxy)
    //    {
    //        string s = SetProperties(proxy).Replace("'", "''");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pname", name);
    //        parameters.Add("pproperties", s);
    //        parameters.Add("ptype", proxy.GetType().ToString());

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_proxy", parameters);

    //        return (long)ds.Tables["results"].Rows[0]["id"];        
    //    }

    //    /// <summary>
    //    /// Add a new user to the database
    //    /// </summary>
    //    /// <param name="name">User name</param>
    //    /// <param name="title">User title</param>
    //    /// <param name="company">User company</param>
    //    /// <param name="phone">User phone #</param>
    //    /// <param name="email">User email address</param>
    //    /// <param name="authorId">ID# of individual who added the user</param>
    //    /// <returns></returns>
    //    public static long AddUser(string name, string title, string company, string phone, string email, long authorId)
    //    {
    //        return AddUser(name, title, company, phone, email, authorId, false);
    //    }

    //            /// <summary>
    //    /// Add a new user to the database
    //    /// </summary>
    //    /// <param name="name">User name</param>
    //    /// <param name="title">User title</param>
    //    /// <param name="company">User company</param>
    //    /// <param name="phone">User phone #</param>
    //    /// <param name="email">User email address</param>
    //    /// <param name="authorId">ID# of individual who added the user</param>
    //    /// <param name="useActiveDirectory">Indicates whether this user should be authenticated against Active Directory</param>
    //    /// <returns></returns>
    //    public static long AddUser(string name, string title, string company, string phone, string email, long authorId, bool useActiveDirectory)
    //    {
    //        return AddUser(name, title, company, phone, email, authorId, useActiveDirectory, "");
    //    }

    //    /// <summary>
    //    /// Add a new user to the database
    //    /// </summary>
    //    /// <param name="name">User name</param>
    //    /// <param name="title">User title</param>
    //    /// <param name="company">User company</param>
    //    /// <param name="phone">User phone #</param>
    //    /// <param name="email">User email address</param>
    //    /// <param name="authorId">ID# of individual who added the user</param>
    //    /// <param name="useActiveDirectory">Indicates whether this user should be authenticated against Active Directory</param>
    //    /// <returns></returns>
    //    public static long AddUser(string name, string title, string company, string phone, string email, long authorId, bool useActiveDirectory, string alias = "")
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uname", name);
    //        parameters.Add("utitle", title);
    //        parameters.Add("ucompany", company);
    //        parameters.Add("uphone", phone);
    //        parameters.Add("uemail", email);
    //        parameters.Add("aid", authorId);
    //        parameters.Add("use_ad", useActiveDirectory ? 1 : 0);
    //        parameters.Add("ualias", string.IsNullOrEmpty(alias) ? name : alias);

    //        DataSet ds = GetDataViaStoredProcedure("sp_add_user", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                long userId = long.Parse(dt.Rows[0]["id"].ToString());
    //                EnsureGroupMemberships(userId);
    //                return userId;
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Delete a document object from the database
    //    /// </summary>
    //    /// <param name="docId">ID# of the document to delete</param>
    //    /// <returns>0 = success, -1 = error</returns>
    //    public static int DeleteDocument(long docId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("doc_id", docId);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_document", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Delete a folder object from the database
    //    /// </summary>
    //    /// <param name="documentId">ID# of the folder to delete</param>
    //    /// <returns>0 = success, -1 = error</returns>
    //    public static int DeleteFolder(long folderId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("fid", folderId);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_folder", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Delete an individual item from a folder
    //    /// </summary>
    //    /// <param name="folderId">ID# of folder containing item to be deleted</param>
    //    /// <param name="itemId">ID# of item to be deleted</param>
    //    /// <returns>0 = success, -1 = error</returns>
    //    public static int DeleteFolderItem(long folderId, long itemId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("col_obj_id", folderId);
    //        parameters.Add("memb_obj_id", itemId);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_folder_item", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Delete an existing group from the database
    //    /// </summary>
    //    /// <param name="groupId">ID# of group to delete</param>
    //    /// <returns>0 if successful, -1 otherwise</returns>
    //    public static int DeleteGroup(long groupId)
    //    {
    //        if (groupId <= 0) return -1;

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("gid", groupId);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_group", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Delete a proxy from the database
    //    /// </summary>
    //    /// <param name="proxyId">ID# of proxy to delete</param>
    //    /// <returns>0 = Success, -1 = Failure</returns>
    //    public static int DeleteProxy(long proxyId)
    //    {
    //        if (proxyId <= 0) return -1;

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", proxyId);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_proxy", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    public enum UserDeleteMode { Archive, Hard };

    //    /// <summary>
    //    /// Delete an existing user from the database
    //    /// </summary>
    //    /// <param name="userId">ID# of user to delete</param>
    //    /// <returns>0 if successful, -1 otherwise</returns>
    //    public static int DeleteUser(long userId)
    //    {
    //        return DeleteUser(userId, UserDeleteMode.Archive);
    //    }

    //    /// <summary>
    //    /// Delete an existing user from the database
    //    /// </summary>
    //    /// <param name="userId">ID# of user to delete</param>
    //    /// <param name="userDeleteMode">Indicates whether this should be an archive (default) or a hard delete</param>
    //    /// <returns>0 if successful, -1 otherwise</returns>
    //    public static int DeleteUser(long userId, UserDeleteMode userDeleteMode)
    //    {
    //        if (userId <= 0) return -1;

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("hard_delete", userDeleteMode == UserDeleteMode.Hard ? 1 : 0);

    //        try
    //        {
    //            SetDataViaStoredProcedure("sp_delete_user", parameters);
    //            return 0;
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Rebuild group membership list for user in database
    //    /// </summary>
    //    /// <param name="userId">User ID# to rebuild memberships for</param>
    //    /// <returns>List of user's group memberships</returns>
    //    public static DataSet EnsureGroupMemberships(long userId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("mid", userId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_ensure_group_memberships", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Escape strings to prevent SQL errors when sending to MySql
    //    /// </summary>
    //    /// <param name="s">String to escape</param>
    //    /// <returns>Escaped string</returns>
    //    public static string FixUp(string s)
    //    {
    //        s = s.Replace("\\", "\\\\");
    //        s = s.Replace("'", "''");

    //        return s;
    //    }

    //    /// <summary>
    //    /// Call a MySql stored procedure that returns data
    //    /// </summary>
    //    /// <param name="sql">SQL statement to execute</param>
    //    /// <returns>DataSet containing results</returns>
    //    private static DataSet GetData(string sql)
    //    {
    //        try
    //        {
    //            MySqlConnection con = new MySqlConnection(MySqlConnectionString);
    //            MySqlCommand comm = new MySqlCommand();

    //            con.Open();
    //            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //            da.Dispose();
    //            con.Close();
    //            con.Dispose();
    //            ds.Tables[0].TableName = "results";
    //            return ds;
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Run a given stored procedure against the database.  This is a better alternative in order to avoid
    //    /// SQL injection attacks
    //    /// </summary>
    //    /// <param name="ProcedureName">Name of stored procedure to run</param>
    //    /// <param name="parameters">Dictionary of key/value pairs representing the stored procedure parameters</param>
    //    /// <returns>DataSet containing results</returns>
    //    public static DataSet GetDataViaStoredProcedure(string procedureName, Dictionary<string, object> parameters)
    //    {
    //        MySqlConnection con = new MySqlConnection(MySqlConnectionString);

    //        MySqlCommand cmd = new MySqlCommand(procedureName, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        foreach (KeyValuePair<string, object> param in parameters)
    //        {
    //            cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
    //        }

    //        MySqlDataAdapter resultDA = new MySqlDataAdapter();
    //        resultDA.SelectCommand = cmd;
    //        resultDA.SelectCommand.Connection = con;

    //        con.Open();
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            resultDA.Fill(ds);
    //            ds.Tables[0].TableName = "results";
    //        }
    //        finally
    //        {
    //            con.Close();
    //        }
    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return document metadata for authorized users
    //    /// </summary>
    //    /// <param name="userId">ID# of requesting user, for permissions checking</param>
    //    /// <param name="docId">ID# of document being requested</param>
    //    /// <returns></returns>
    //    public static DataSet GetDocument(long userId, long docId, long folderId = 0)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("did", docId);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_document", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return folder metadata for authorized users
    //    /// </summary>
    //    /// <param name="userId">ID# of requesting user, for permissions checking</param>
    //    /// <param name="folderId">ID# of folder being requested</param>
    //    /// <returns></returns>
    //    public static DataSet GetFolder(long userId, long folderId, long parentFolderId = 0)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("fid", folderId);
    //        parameters.Add("parent_fid", parentFolderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_folder", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Get all documents contained in a folder
    //    /// </summary>
    //    /// <param name="userId">ID# of user, needed for security trimming</param>
    //    /// <param name="folderId">ID# of folder containing documents to return</param>
    //    /// <returns>DataSet containing all folder documents</returns>
    //    public static DataSet GetFolderDocuments(long userId, long folderId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", userId);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_folder_documents", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Get all sub-folders contained in a folder
    //    /// </summary>
    //    /// <param name="userId">ID# of user, needed for security trimming</param>
    //    /// <param name="folderId">ID# of folder containing sub-folders to return</param>
    //    /// <returns>DataSet containing all folder sub-folders</returns>
    //    public static DataSet GetFolderSubfolders(long userId, long folderId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", userId);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_folder_subfolders", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return file icon data for a given file extension
    //    /// </summary>
    //    /// <param name="extenstion">string representing extension of file</param>
    //    /// <returns>DataSet containing icon data</returns>
    //    public static DataSet GetIconByExtension(string extenstion)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("ext", extenstion);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_icon_by_extension", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return file icon data for a given Id#
    //    /// </summary>
    //    /// <param name="extenstion">Id# of the extension</param>
    //    /// <returns>DataSet containing icon data</returns>
    //    public static DataSet GetIconById(int id)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("iid", id);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_icon_by_id", parameters);

    //        return ds;
    //    }

    //    public static DataSet GetObjectPermissions(long objectId, long folderId = 0)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("obj_id", objectId);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_object_permissions", parameters);

    //        // Create a new table with structure of returned table, but with "can_" columns
    //        // of type boolean
    //        DataTable dtNew = new DataTable();
    //        foreach (DataColumn col in ds.Tables["results"].Columns)
    //        {
    //            //if (col.ColumnName.StartsWith("can_"))
    //            //{
    //            //    dtNew.Columns.Add(new DataColumn(col.ColumnName, typeof(bool)));
    //            //}
    //            //else
    //            //{
    //                dtNew.Columns.Add(new DataColumn(col.ColumnName, col.DataType));
    //            //}
    //        }

    //        // Fixup boolean fields
    //        foreach (DataRow row in ds.Tables["results"].Rows)
    //        {
    //            DataRow newRow = dtNew.NewRow();
    //            foreach (DataColumn col in ds.Tables["results"].Columns)
    //            {
    //                if (col.ColumnName.StartsWith("can_"))
    //                {
    //                    //newRow[col.ColumnName] = (row[col].ToString() == "1") ? true : false;
    //                    newRow[col.ColumnName] = (int)row[col];
    //                }
    //                else
    //                {
    //                    newRow[col.ColumnName] = row[col];
    //                }
    //            }
    //            dtNew.Rows.Add(newRow);
    //        }

    //        ds.Tables.Remove("results");
    //        dtNew.TableName = "results";
    //        ds.Tables.Add(dtNew);

    //        return ds;
    //    }

    //    public static DataSet GetObjectUserPermissions(long objectId, long userId, long folderId = 0)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("obj_id", objectId);
    //        parameters.Add("uid", userId);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_object_user_permissions", parameters);

    //        // Create a new table with structure of returned table, but with "can_" columns
    //        // of type boolean
    //        DataTable dtNew = new DataTable();
    //        foreach (DataColumn col in ds.Tables["results"].Columns)
    //        {
    //            if (col.ColumnName.StartsWith("can_"))
    //            {
    //                dtNew.Columns.Add(new DataColumn(col.ColumnName, typeof(bool)));
    //            }
    //            else
    //            {
    //                dtNew.Columns.Add(new DataColumn(col.ColumnName, col.DataType));
    //            }
    //        }

    //        // Fixup boolean fields
    //        foreach (DataRow row in ds.Tables["results"].Rows)
    //        {
    //            DataRow newRow = dtNew.NewRow();
    //            foreach (DataColumn col in ds.Tables["results"].Columns)
    //            {
    //                if (col.ColumnName.StartsWith("can_"))
    //                {
    //                    newRow[col.ColumnName] = (row[col].ToString() == "1") ? true : false;
    //                    //newRow[col.ColumnName] = int.Parse(row[col].ToString());
    //                }
    //                else
    //                {
    //                    newRow[col.ColumnName] = row[col];
    //                }
    //            }
    //            dtNew.Rows.Add(newRow);
    //        }

    //        ds.Tables.Remove("results");
    //        dtNew.TableName = "results";
    //        ds.Tables.Add(dtNew);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// De-serialize an object from its JSON representation
    //    /// </summary>
    //    /// <param name="obj">String containing JSON representation of object</param>
    //    /// <param name="t">Type of object</param>
    //    /// <returns>De-serialized properties object</returns>
    //    public static object GetPrincipalProperties(string obj, Type t)
    //    {
    //        return GetProperties(obj, t);
    //    }

    //    /// <summary>
    //    /// De-serialize an object from its JSON representation
    //    /// </summary>
    //    /// <param name="obj">String containing JSON representation of object</param>
    //    /// <param name="strType">String containing object type name</param>
    //    /// <returns>De-serialized properties object</returns>
    //    public static object GetProperties(string obj, string strType)
    //    {
    //        try
    //        {
    //            Type t = Type.GetType(strType);
    //            return JsonConvert.DeserializeObject(obj.Replace("\\", "\\\\"), t);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// De-serialize an object from its JSON representation
    //    /// </summary>
    //    /// <param name="obj">String containing JSON representation of object</param>
    //    /// <param name="t">Type of object</param>
    //    /// <returns>De-serialized properties object</returns>
    //    public static object GetProperties(string obj, Type t)
    //    {
    //        try
    //        {
    //            return JsonConvert.DeserializeObject(obj.Replace("\\", "\\\\"), t);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    ///// <summary>
    //    ///// Return properties for a given securable object in the database
    //    ///// </summary>
    //    ///// <param name="id">ID# of object for which to return properties</param>
    //    ///// <returns>Properties object</returns>
    //    //public static object GetProperties(long id)
    //    //{
    //    //    Dictionary<string, object> parameters = new Dictionary<string, object>();

    //    //    parameters.Add("oid", id);

    //    //    DataSet ds = GetDataViaStoredProcedure("sp_get_properties", parameters);

    //    //    DataTable dt = ds.Tables["results"];

    //    //    if (dt != null && dt.Rows.Count > 0)
    //    //    {
    //    //        string s = dt.Rows[0]["properties"].ToString();
    //    //        Type t = Type.GetType(dt.Rows[0]["type"].ToString());
    //    //        return GetProperties(s, t);
    //    //    }
    //    //    else
    //    //    {
    //    //        return null;
    //    //    }
    //    //}

    //    /// <summary>
    //    /// Return raw data for a given proxy
    //    /// </summary>
    //    /// <param name="proxyId">ID# of proxy to return</param>
    //    /// <returns>DataSet containing proxy raw data</returns>
    //    public static DataSet GetProxy(long proxyId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", proxyId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_proxy", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Get a PropertySet for a specific user or group
    //    /// </summary>
    //    /// <param name="id">ID# of user or group</param>
    //    /// <returns>PropertySet containing associated properties of user or group</returns>
    //    public static PropertySet GetPrincipalPropertySet(long id)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", id);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_principal_properties", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        PropertySet propertyBag = new PropertySet();

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            propertyBag.Properties = dt.Rows[0]["properties"].ToString();
    //            propertyBag.Type = dt.Rows[0]["type"].ToString();
    //        }
    //        else
    //        {
    //            propertyBag = null;
    //        }

    //        dt.Dispose();

    //        return propertyBag;
    //    }

    //    /// <summary>
    //    /// Get an object containing the JSON-encoded properties object and the type of that object
    //    /// </summary>
    //    /// <param name="id">ID# of securable object for which to return properties</param>
    //    /// <returns>An object containing the JSON-encoded properties and its type</returns>
    //    public static PropertySet GetPropertySet(long id)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("oid", id);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_properties", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        PropertySet propertyBag = new PropertySet();

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            propertyBag.Properties = dt.Rows[0]["properties"].ToString();
    //            propertyBag.Type = dt.Rows[0]["type"].ToString();
    //        }
    //        else
    //        {
    //            propertyBag = null;
    //        }

    //        dt.Dispose();

    //        return propertyBag;
    //    }

    //    /// <summary>
    //    /// Return data for a specified group
    //    /// </summary>
    //    /// <param name="groupId">ID# of group to return</param>
    //    /// <returns>DataSet containing group's data</returns>
    //    public static DataSet GetGroup(long groupId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("gid", groupId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_group", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return data for a specific user alias
    //    /// </summary>
    //    /// <param name="alias">Alias of user to return</param>
    //    /// <returns>Dataset containing all data for the given user</returns>
    //    public static DataSet GetUser(string alias)
    //    {
    //        return GetUser(alias, false);
    //    }

    //    /// <summary>
    //    /// Return data for a specific user alias
    //    /// </summary>
    //    /// <param name="alias">Alias of user to return</param>
    //    /// <param name="includeArchived">Should archived user data be returned?</param>
    //    /// <returns>Dataset containing all data for the given user</returns>
    //    public static DataSet GetUser(string alias, bool includeArchived)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("ualias", alias);
    //        parameters.Add("include_archived", includeArchived ? 1 : 0);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_user_by_alias", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return data for a specified user
    //    /// </summary>
    //    /// <param name="userId">ID# of user to return</param>
    //    /// <returns>DataSet containing user's data</returns>
    //    public static DataSet GetUser(long userId)
    //    {
    //        return GetUser(userId, false);
    //    }

    //    /// <summary>
    //    /// Return data for a specified user
    //    /// </summary>
    //    /// <param name="userId">ID# of user to return</param>
    //    /// <param name="includeArchived">Should archived users be returned or ignored?</param>
    //    /// <returns>DataSet containing user's data</returns>
    //    public static DataSet GetUser(long userId, bool includeArchived)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("include_archived", includeArchived ? 1 : 0);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_user", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Return just the user password and salt value for a given user alias
    //    /// </summary>
    //    /// <param name="alias">Alias of user to return</param>
    //    /// <returns></returns>
    //    public static DataSet GetUserCredentials(string alias)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("ualias", alias);
    //        parameters.Add("include_archived", false);

    //        DataSet ds = GetDataViaStoredProcedure("sp_get_user_by_alias", parameters);

    //        DataTable dt = ds.Tables["results"];
    //        for (int i = dt.Columns.Count-1; i >= 0; i--)
    //        {
    //            if (dt.Columns[i].ColumnName != "salt_value" && dt.Columns[i].ColumnName != "pwd")
    //            {
    //                dt.Columns.Remove(dt.Columns[i]);
    //            }
    //        }
    //        dt.AcceptChanges();

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Move an object (Folder or Document) from one folder to another
    //    /// </summary>
    //    /// <param name="fromFolderId"></param>
    //    /// <param name="toFolderId"></param>
    //    /// <param name="objectId"></param>
    //    public static void MoveFolderItem(long fromFolderId, long toFolderId, long objectId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("fromFolderId", fromFolderId);
    //        parameters.Add("toFolderId", toFolderId);
    //        parameters.Add("objectId", objectId);

    //        SetDataViaStoredProcedure("sp_move_object", parameters);
    //    }

    //    /// <summary>
    //    /// Call MySql and run the provided SQL
    //    /// </summary>
    //    /// <param name="sql">SQL to execute on MySql</param>
    //    public static void SetData(string sql)
    //    {
    //        try
    //        {
    //            MySqlConnection con = new MySqlConnection(MySqlConnectionString);
    //            con.Open();
    //            MySqlCommand cmd = new MySqlCommand(sql, con);
    //            cmd.ExecuteNonQuery();
    //            cmd.Dispose();
    //            con.Close();
    //            con.Dispose();
    //        }
    //        catch { }
    //    }

    //    /// <summary>
    //    /// Write data to database without returning any data
    //    /// </summary>
    //    /// <param name="procedureName">Stored procedure to call</param>
    //    /// <param name="parameters">Dictionary of key/value pairs representing the parameters of the stored procedure</param>
    //    public static void SetDataViaStoredProcedure(string procedureName, Dictionary<string, object> parameters)
    //    {
    //        MySqlConnection con = new MySqlConnection(MySqlConnectionString);

    //        MySqlCommand cmd = new MySqlCommand(procedureName, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        foreach (KeyValuePair<string, object> param in parameters)
    //        {
    //            cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value);
    //        }

    //        con.Open();
    //        try
    //        {
    //            cmd.ExecuteNonQuery();
    //        }
    //        finally
    //        {
    //            con.Close();
    //        }
    //    }

    //    /// <summary>
    //    /// Set a user's password + salt value for decryption
    //    /// </summary>
    //    /// <param name="userId">ID# of user</param>
    //    /// <param name="password">Password value - Max 50 characters</param>
    //    /// <param name="saltValue">Salt value - Max 50 characters</param>
    //    public static void SetPassword(long userId, string password, string saltValue)
    //    {
    //        if (password.Length > 255) throw new Exception("Password exceeds maximum length of 255 characters");
    //        if (saltValue.Length > 50) throw new Exception("Salt value exceeds maximum length of 50 characters");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("upwd", password);
    //        parameters.Add("usalt", saltValue);

    //        SetDataViaStoredProcedure("sp_set_pwd", parameters);

    //    }

    //    /// <summary>
    //    /// Set (save) properties for a given user or group
    //    /// </summary>
    //    /// <param name="obj">Object containing properties to save to database</param>
    //    /// <param name="id">ID# of user or group</param>
    //    /// <returns>JSON representation of the properties stored</returns>
    //    public static string SetPrincipalProperties(object obj, long id)
    //    {
    //        string s = SetProperties(obj).Replace("'", "''");
    //        s = SetProperties(obj).Replace("\\\\", "\\");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", id);
    //        parameters.Add("oproperties", s);
    //        parameters.Add("otype", obj.GetType().ToString());

    //        SetDataViaStoredProcedure("sp_set_principal_properties", parameters);

    //        return s;
    //    }

    //    /// <summary>
    //    /// Serialize an object to its JSON-encoded equivalent
    //    /// </summary>
    //    /// <param name="obj">Object to serialize</param>
    //    /// <returns>JSON string representation of the object</returns>
    //    public static string SetProperties(object obj)
    //    {
    //        return JsonConvert.SerializeObject(obj);
    //    }

    //    /// <summary>
    //    /// Write a JSON-encoded string representation of an object to the database for a given object
    //    /// </summary>
    //    /// <param name="obj">Object to store in the database</param>
    //    /// <param name="id">ID# of the securable object to save these properties to</param>
    //    /// <returns></returns>
    //    public static string SetProperties(object obj, long id)
    //    {
    //        string s = SetProperties(obj).Replace("'", "''");
    //        s = SetProperties(obj).Replace("\\\\", "\\");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("oid", id);
    //        parameters.Add("oproperties", s);
    //        parameters.Add("otype", obj.GetType().ToString());

    //        SetDataViaStoredProcedure("sp_set_properties", parameters);

    //        return s;
    //    }

    //    /// <summary>
    //    /// Permission enum
    //    /// </summary>
    //    public enum PermissionLevel { Allow, Deny };

    //    /// <summary>
    //    /// Set permissions for a given object
    //    /// </summary>
    //    /// <param name="objectId">ID of object to set permissions for</param>
    //    /// <param name="principalID">User setting permissions</param>
    //    /// <param name="canRead">Read allowed</param>
    //    /// <param name="canEdit">Edit allowed</param>
    //    /// <param name="canPrint">Print allowed</param>
    //    /// <param name="canDelete">Delete allowed</param>
    //    /// <param name="canAdd">Add allowed (applies to folders only)</param>
    //    /// <returns>DataDet containing updated permissions for this object</returns>
    //    public static DataSet SetObjectPermissions(long objectId, long principalID, bool canRead, bool canEdit, bool canPrint, bool canDelete, bool canAdd, bool canShare = false, bool canSetPermissions = false, long folderId=0)
    //    {

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("obj_id", objectId);
    //        parameters.Add("uid", principalID);
    //        parameters.Add("oread", canRead ? 1 : 0);
    //        parameters.Add("oedit", canEdit ? 1 : 0);
    //        parameters.Add("oprint", canPrint ? 1 : 0);
    //        parameters.Add("odelete", canDelete ? 1 : 0);
    //        parameters.Add("oadd", canAdd ? 1 : 0);
    //        parameters.Add("oshare", canShare ? 1 : 0);
    //        parameters.Add("osetpermissions", canSetPermissions ? 1 : 0);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_set_object_permissions", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Set permissions using 0,1, or 2 values rather than booleans
    //    /// </summary>
    //    /// <param name="objectId">ID of object to set permissions for</param>
    //    /// <param name="principalID">User setting permissions</param>
    //    /// <param name="canRead">0 or 2 mean no permissions, 1 = allowed</param>
    //    /// <param name="canEdit">0 or 2 mean no permissions, 1 = allowed</param>
    //    /// <param name="canPrint">0 or 2 mean no permissions, 1 = allowed</param>
    //    /// <param name="canDelete">0 or 2 mean no permissions, 1 = allowed</param>
    //    /// <param name="canAdd">0 or 2 mean no permissions, 1 = allowed</param>
    //    /// <returns>DataDet containing updated permissions for this object</returns>
    //    public static DataSet SetObjectPermissions(long objectId, long principalID, int canRead, int canEdit, int canPrint, int canDelete, int canAdd, int canShare = 0, int canSetPermissions = 0, long folderId = 0)
    //    {
    //        // Make sure all permission values within acceptable range
    //        if (canRead < 0 || canRead > 2) throw new Exception("Acceptable permission values are 0, 1 or 2");
    //        if (canEdit < 0 || canEdit > 2) throw new Exception("Acceptable permission values are 0, 1 or 2");
    //        if (canPrint < 0 || canPrint > 2) throw new Exception("Acceptable permission values are 0, 1 or 2");
    //        if (canDelete < 0 || canDelete > 2) throw new Exception("Acceptable permission values are 0, 1 or 2");
    //        if (canAdd < 0 || canAdd > 2) throw new Exception("Acceptable permission values are 0, 1 or 2");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("obj_id", objectId);
    //        parameters.Add("uid", principalID);
    //        parameters.Add("oread", canRead);
    //        parameters.Add("oedit", canEdit);
    //        parameters.Add("oprint", canPrint);
    //        parameters.Add("odelete", canDelete);
    //        parameters.Add("oadd", canAdd);
    //        parameters.Add("oshare", canShare);
    //        parameters.Add("osetpermissions", canSetPermissions);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_set_object_permissions", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="id">ID# of object to set READ permission for</param>
    //    /// <param name="userId">ID# of user or group to set READ permission for</param>
    //    /// <param name="permissionLevel">Permission level - i.e. either Allow or Deny</param>
    //    /// <returns></returns>
    //    public static long SetReadPermission(long id, long userId, PermissionLevel permissionLevel, long folderId = 0)
    //    {
    //        int pLevel = 0;
    //        if (permissionLevel == PermissionLevel.Allow)
    //        {
    //            pLevel = 1;
    //        }

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("oid", id);
    //        parameters.Add("uid", userId);
    //        parameters.Add("plevel", pLevel);
    //        parameters.Add("fid", folderId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_set_securable_object_read_permission", parameters);

    //        DataTable dt = ds.Tables["results"];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                return long.Parse(dt.Rows[0]["id"].ToString());
    //            }
    //            catch
    //            {
    //                return -1;
    //            }
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //    /// <summary>
    //    /// Update document properties
    //    /// </summary>
    //    /// <param name="documentId">ID# of document to update</param>
    //    /// <param name="documentPath">New document path</param>
    //    /// <param name="documentName">New document name</param>
    //    /// <param name="documentDescription">New document description</param>
    //    /// <param name="authorId">New author ID#</param>
    //    /// <returns></returns>
    //    public static DataSet UpdateDocument(long documentId, string documentPath, string documentName, string documentDescription, long authorId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("doc_id", documentId);
    //        parameters.Add("doc_path", documentPath);
    //        parameters.Add("doc_name", documentName);
    //        parameters.Add("doc_description", documentDescription);
    //        parameters.Add("doc_author_id", authorId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_update_document", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Update folder properties
    //    /// </summary>
    //    /// <param name="folderId">ID# of folder to update</param>
    //    /// <param name="folderName">New folder name</param>
    //    /// <param name="folderDescription">New folder description</param>
    //    /// <param name="authorId">New folder author ID#</param>
    //    /// <returns></returns>
    //    public static DataSet UpdateFolder(long folderId, string folderName, string folderDescription, long authorId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("folder_id", folderId);
    //        parameters.Add("folder_name", folderName);
    //        parameters.Add("folder_description", folderDescription);
    //        parameters.Add("folder_author_id", authorId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_update_folder", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Update group entry in the database
    //    /// </summary>
    //    /// <param name="groupId">ID# of group to update</param>
    //    /// <param name="groupName">New group name</param>
    //    /// <param name="groupDescription">New group description</param>
    //    /// <param name="authorId">New group author ID#</param>
    //    /// <returns></returns>
    //    public static DataSet UpdateGroup(long groupId, string groupName, string groupDescription, long authorId)
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("gid", groupId);
    //        parameters.Add("gname", groupName);
    //        parameters.Add("descrip", groupDescription);
    //        parameters.Add("aid", authorId);

    //        DataSet ds = GetDataViaStoredProcedure("sp_update_group", parameters);

    //        return ds;
    //    }

    //    /// <summary>
    //    /// Update data for a specified proxy in the database
    //    /// </summary>
    //    /// <param name="proxyId">ID# of proxy to update</param>
    //    /// <param name="name">Name of proxy</param>
    //    /// <param name="proxy">Proxy object</param>
    //    /// <returns></returns>
    //    public static DataSet UpdateProxy(long proxyId, string name, object proxy)
    //    {
    //        string s = SetProperties(proxy).Replace("'", "''");

    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("pid", proxyId);
    //        parameters.Add("pname", name);
    //        parameters.Add("pproperties", s);
    //        parameters.Add("ptype", proxy.GetType().ToString());

    //        DataSet ds = GetDataViaStoredProcedure("sp_update_proxy", parameters);

    //        return ds; 
    //    }

    //    /// <summary>
    //    /// Update user data in database for a given ID#
    //    /// </summary>
    //    /// <param name="userId">ID# of user record to update</param>
    //    /// <param name="name">New user name</param>
    //    /// <param name="title">New user title</param>
    //    /// <param name="company">New user company</param>
    //    /// <param name="phone">New user phone</param>
    //    /// <param name="email">New user email</param>
    //    /// <param name="authorId">New user author id</param>
    //    public static DataSet UpdateUser(long userId, string name, string title, string company, string phone, string email, long authorId)
    //    {
    //        return UpdateUser(userId, name, title, company, phone, email, authorId, false, false);
    //    }

    //    /// <summary>
    //    /// Update user data in database for a given ID#
    //    /// </summary>
    //    /// <param name="userId">ID# of user record to update</param>
    //    /// <param name="name">New user name</param>
    //    /// <param name="title">New user title</param>
    //    /// <param name="company">New user company</param>
    //    /// <param name="phone">New user phone</param>
    //    /// <param name="email">New user email</param>
    //    /// <param name="authorId">New user author id</param>
    //    /// <param name="useActiveDirectory">Indicates whether this user should be authenticated against Active Directory</param>
    //    public static DataSet UpdateUser(long userId, string name, string title, string company, string phone, string email, long authorId, bool useActiveDirectory, bool archived, string alias = "")
    //    {
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        parameters.Add("uid", userId);
    //        parameters.Add("uname", name);
    //        parameters.Add("utitle", title);
    //        parameters.Add("ucompany", company);
    //        parameters.Add("uphone", phone);
    //        parameters.Add("uemail", email);
    //        parameters.Add("aid", authorId);
    //        parameters.Add("use_ad", useActiveDirectory ? 1 : 0);
    //        parameters.Add("archvd", archived ? 1 : 0);
    //        parameters.Add("ualias", string.IsNullOrEmpty(alias) ? name : alias);

    //        DataSet ds = GetDataViaStoredProcedure("sp_update_user", parameters);

    //        return ds;
    //    }
    //}
}
