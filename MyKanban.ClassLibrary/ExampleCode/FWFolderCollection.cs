using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClearSky.Common.Data;


namespace ClearSky.Common
{
    //public class FWFolderCollection : FWObject
    //{

    //    public int Count
    //    {
    //        get { return _Items.Count; }
    //    }

    //    List<FWFolder> _Items = new List<FWFolder>();

    //    public List<FWFolder> Items
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
    //    public FWFolder this[int folderid]
    //    {
    //        get { return _Items[folderid]; }
            
    //    }
    //    public IEnumerator<FWFolder> GetEnumerator()
    //    {
    //        return Items.GetEnumerator();
    //    }


    //    public FWFolderCollection(FWFolder parentFolder)
    //    {
    //        _ParentFolder = parentFolder;
    //        DataSet documents = DataAccess.GetFolderSubfolders(CurrentUser.ID, parentFolder.ID);
    //        foreach(DataRow row in documents.Tables["results"].Rows)
    //        {
    //            try
    //            {
    //                _Items.Add(new FWFolder((long)row["id"]));
    //            }
    //            catch {

    //                //TODO: Log FWFolderCollection Event 
    //            }
    //        }
    //    }
    //    public void Add(FWFolder folder)
    //    {
    //        DataAccess.AddFolderItem(ParentFolder.ID, folder.ID);
    //        _Items.Add(folder);
    //    }
    //    public void Remove(FWFolder folder)
    //    {
    //        DataAccess.DeleteFolderItem(ParentFolder.ID, folder.ID);
    //        _Items.Remove(folder);
    //    }
    //}
}
