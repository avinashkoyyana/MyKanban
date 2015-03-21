using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using Newtonsoft.Json;

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
 *  Class:      Projects
 *  
 *  Purpose:    Represents a collection of Project objects 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Projects : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Projects(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Projects(Board board, Credential credential)
        {
            ProjectsBoardConstructor(board, credential);
        }

        private void ProjectsBoardConstructor(Board board, Credential credential)
        {
            if (credential != null) _credential = credential;

            _items.Clear();
            DataSet dsProjects = MyKanban.Data.GetProjectsByBoard(board.Id, _credential.Id);
            foreach (DataRow drProject in dsProjects.Tables["results"].Rows)
            {
                Project project = new Project(long.Parse(drProject["id"].ToString()), _credential);
                project.BoardId = board.Id;
                _items.Add(project);
            }
            _parent = board;
            _parentId = board.Id;
        }

        public Projects(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsProjects = MyKanban.Data.GetProjectsByName(nameFilter, _credential.Id);
            foreach (DataRow drProject in dsProjects.Tables["results"].Rows)
            {
                Project project = new Project(long.Parse(drProject["id"].ToString()), _credential);
                _items.Add(project);
            }
        }

        #endregion

        #region Properties

        public Project this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        private List<Project> _items = new List<Project>();
        public List<Project> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(Project project)
        {
            project.ParentId = _parentId > 0 ? _parentId : project.ParentId;
            project.BoardId = project.ParentId;
            _items.Add(project);
        }

        public void Clear(bool delete = false)
        {
            if (delete)
            {
                foreach (Project project in _items)
                {
                    project.Delete();
                }
            }

            _items.Clear();
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Project)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public override void Reload()
        {
            base.Reload();

            if (_parent != null) ProjectsBoardConstructor((Board)_parent, _credential);
        }

        public void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(Project project, bool delete = false)
        {
            _items.Remove(project);
        }

        public bool Update(bool force = false)
        {
            try
            {
                foreach (Project project in _items)
                {
                    // Update the underlying project
                    project.Update(force);

                    // There might be some instances where a project is
                    // created independantly of a parent collection, in which
                    // case we should use the parent Id of the project,
                    // instead of the project collection.
                    long pId = _parentId > 0 ? _parentId : project.ParentId;
                    Data.AddProjectToBoard(pId, project.Id, _credential.Id);
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
