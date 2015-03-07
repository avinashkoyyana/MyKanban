using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClearSky.Common.Data;


namespace ClearSky.Common
{
    //public class FWFileCollection : FWObject
    //{
    //    int _Count;

    //    public int Count
    //    {
    //        get { return _Items.Count; }
    //    }

    //    List<FWFile> _Items = new List<FWFile>();

    //    public List<FWFile> Items
    //    {
    //        get { return _Items; }
    //        set { _Items = value; }
    //    }

    //    FWFolder _ParentFolder;

    //    public FWFolder ParentFolder
    //    {
    //        get { return _ParentFolder; }
    //        set { _ParentFolder = value; }
    //    }
    //    public FWFile this[int fileid]
    //    {
    //        get { return _Items[fileid]; }

    //    }
    //    public IEnumerator<FWFile> GetEnumerator()
    //    {
    //        return Items.GetEnumerator();
    //    }

    //    public FWFileCollection(FWFolder parentFolder)
    //    {
    //        _ParentFolder = parentFolder;
    //        DataSet documents = DataAccess.GetFolderDocuments(CurrentUser.ID, parentFolder.ID);
    //        foreach(DataRow row in documents.Tables["results"].Rows)
    //        {
    //            try
    //            {
    //                _Items.Add(new FWFile((long)row["id"]));
    //            }
    //            catch { 

    //                //TODO: Need to create an event logging strategy.
    //                //TODO: Log FWFileCollection event
    //            }
    //        }
    //    }

    //}
}
