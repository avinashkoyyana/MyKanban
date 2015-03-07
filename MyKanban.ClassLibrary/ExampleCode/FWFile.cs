using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ClearSky.Common.Data;
using ClearSky;


namespace ClearSky.Common
{
    //public class FWFile : FWObject
    //{
    //    //Unique ID from database for file
    //    long _ID;
    //    public long ID
    //    {
    //        get { return _ID; }
    //    }

    //    //File Name (most current)
    //    string _Name;
    //    public string Name
    //    {
    //        get { return _Name; }
    //        set { _Name = value; }
    //    }

    //    //ID for proxy server
    //    long _ProxyID;
    //    public long ProxyID
    //    {
    //        get { return _ProxyID; }
    //        set { _ProxyID = value; }
    //    }

    //    //String File Extension  
    //    string _FileExtension;
    //    public string FileExtension
    //    {
    //        get
    //        {
    //            if (_Name.Length > 3 && _Name.Contains('.'))
    //            {
    //                _FileExtension = _Name.Substring(_Name.LastIndexOf('.') + 1).ToLower();
    //            }
    //            return _FileExtension;
    //        }
    //    }

    //    //FileIcon
    //    FWIcon _FileIcon;
    //    public FWIcon FileIcon
    //    {
    //        get
    //        {
    //            if (_FileIcon == null && FileExtension.Length > 0)
    //            {
    //                _FileIcon = new FWIcon(FileExtension);
    //            }
    //            return _FileIcon;

    //        }
    //    }

    //    string _Path;
    //    /// <summary>
    //    /// Filepath for the file object. 
    //    /// </summary>
    //    public string Path
    //    {
    //        get { return _Path; }
    //        set { _Path = value; }
    //    }

    //    string _Description;

    //    public string Description
    //    {
    //        get { return _Description; }
    //        set { _Description = value; }
    //    }

    //    FWUser _Author;
    //    /// <summary>
    //    /// FWUser object for user who authored the document
    //    /// </summary>
    //    public FWUser Author
    //    {
    //        get { return _Author; }
    //        set { _Author = value; }
    //    }

    //    DateTime _DateTimeCreated;

    //    public DateTime DateTimeCreated
    //    {
    //        get { return _DateTimeCreated; }
    //        set { _DateTimeCreated = value; }
    //    }

    //    DateTime _DateTimeLastModified;

    //    public DateTime DateTimeLastModified
    //    {
    //        get { return _DateTimeLastModified; }
    //        set { _DateTimeLastModified = value; }
    //    }

    //    FWFileInfo _FileInfo;

    //    public FWFileInfo FileInfo
    //    {
    //        get
    //        {
    //            if (_FileInfo == null)
    //            {
    //                _FileInfo = new FWFileInfo();
    //                _FileInfo.ID = _ID;
    //                _FileInfo.Author = _Author.Name;
    //                _FileInfo.Name = _Name;
    //                _FileInfo.Date = _DateTimeCreated.ToString("MMM d, yyyy");
    //                _FileInfo.FileExt = FileExtension;
    //                return _FileInfo;
    //            }
    //            return _FileInfo;
    //        }
    //    }


    //    byte[] _FileBytes;
    //    /// <summary>
    //    /// Gets or sets the bytes for the file. 
    //    /// </summary>
    //    public byte[] FileBytes
    //    {
    //        get
    //        {
    //            if (_FileBytes == null)
    //            {
    //                GetFileBytes();
    //            }
    //            return _FileBytes;
    //        }
    //        set
    //        {
    //            _FileBytes = value;
    //        }
    //    }

    //    List<FWObjectPermission> _Permissions;
    //    public List<FWObjectPermission> Permissions
    //    {
    //        get
    //        {
    //            if (_Permissions == null)
    //            {
    //                GetObjectPermissions();
    //            }
    //            return _Permissions;
    //        }
    //    }

    //    FWProperties _Properties;

    //    public FWProperties Properties
    //    {
    //        get
    //        {
    //            if (_Properties == null)
    //            {
    //                _Properties = new FWProperties(_ID, "O");
    //            }
    //            return _Properties;
    //        }
    //    }

       


    //    /// <summary>
    //    /// Use to create a new file
    //    /// </summary>
    //    public FWFile()
    //    {

    //    }

    //    /// <summary>
    //    /// Fetches file from the datbase by the ID Specified
    //    /// </summary>
    //    /// <param name="FileID">(long) int of file/secure object in db</param>
    //    public FWFile(long FileID)
    //    {
    //        _ID = FileID;
    //        SetupFile();
    //    }

    //    /// <summary>
    //    /// Use to create a new file
    //    /// </summary>
    //    public FWFile(string _name, string _description, string _path)
    //    {
    //        SetupFWFile(_name, _description, _path, CurrentUser);
    //    }

    //    /// <summary>
    //    /// Use to create a new file
    //    /// </summary>
    //    public FWFile(string _name, string _description, string _path, FWUser author)
    //    {
    //        SetupFWFile(_name, _description, _path, author);
    //    }

    //    /// <summary>
    //    /// Deletes the file from the database
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool Delete()
    //    {
    //        try
    //        {
    //            DataAccess.DeleteDocument(_ID);
    //            return true;
    //        }
    //        catch { return false; }
    //    }

    //    /// <summary>
    //    /// Saves a new file or updates changes to existing file
    //    /// </summary>
    //    /// <returns>true if successful, false if not.</returns>
    //    public bool Save()
    //    {
    //        if (_ID > 0)
    //        {
    //            //update existing file properties
    //            DataAccess.UpdateDocument(_ID, _Path, _Name, _Description, Author.ID);
    //            SetupFile();
    //        }
    //        else
    //        {
    //            //create a new file in the database
    //            _ID = DataAccess.AddDocument("", _Name, CurrentUser.ID, _Description, DefaultProxyID);
    //            SetFileBytes();
    //            SetupFile();
    //            return true;
    //        }
    //        return false;
    //    }


    //    private void SetupFWFile(string _name, string _description, string _path, FWUser _author)
    //    {
    //        _Name = _name;
    //        _Description = _description;
    //        _Path = _path;
    //        _Author = _author;
    //    }
    //    private void SetupFile()
    //    {
    //        DataSet ds = DataAccess.GetDocument(CurrentUser.ID, _ID);
    //        if (ds.Tables["results"].Rows.Count == 1)
    //        {
    //            if (ds.Tables["results"].Columns.Count == 2 && ds.Tables["results"].Rows[0]["permission_level"] as string == "Access Denied")
    //            {
    //                throw new Exception("You are not permitted to access this file");
    //            }
    //            else if (ds.Tables["results"].Columns.Count == 2 && ds.Tables["results"].Rows[0]["permission_level"] as string == "Document not found")
    //            {
    //                throw new Exception("The file does not exist");
    //            }
    //            DataRow dr = ds.Tables["results"].Rows[0];

    //            _Name = dr["name"] as string;
    //            _Path = dr["path"] as string;
    //            _Description = dr["description"] as string;
    //            _Author = new FWUser((long)dr["author_id"]);
    //            _DateTimeCreated = (DateTime)dr["create_datetime"];
    //            _DateTimeLastModified = (DateTime)dr["last_modified_datetime"];
    //            _ProxyID = (long)dr["proxy"];

    //        }
    //        else
    //        {
    //            throw new MissingPrimaryKeyException("The id returned multiple documents");
    //        }
    //    }
    //    private void GetObjectPermissions()
    //    {
    //        DataSet dset = DataAccess.GetObjectPermissions(_ID);
    //        if (_Permissions == null) _Permissions = new List<FWObjectPermission>();
    //        _Permissions.Clear();
    //        foreach (DataRow dr in dset.Tables["results"].Rows)
    //        {
    //            FWObjectPermission p = new FWObjectPermission(dr);
    //            if (!_Permissions.Contains(p))
    //            {
    //                _Permissions.Add(p);
    //            }
    //        }
    //    }

    //    public void SetPermissions(FWObjectPermission permissionset)
    //    {
    //        DataAccess.SetObjectPermissions(_ID, permissionset.PrincipalID, permissionset.CanRead, permissionset.CanEdit, permissionset.CanPrint, permissionset.CanDelete, permissionset.CanAdd);
    //        GetObjectPermissions();
    //    }
        


    //    private void GetFileBytes()
    //    {
    //        FWProxy proxy = new FWProxy(_ProxyID);
    //        byte[] a = proxy.GetDocumentBytes(CurrentUser.ID, _ID);
    //        _FileBytes = a;
    //    }
    //    private void SetFileBytes()
    //    {
    //        FWProxy proxy = new FWProxy(DefaultProxyID);
    //        proxy.SaveDocumentBytes(CurrentUser.ID, _ID, _FileBytes);
    //    }



    //}
}
