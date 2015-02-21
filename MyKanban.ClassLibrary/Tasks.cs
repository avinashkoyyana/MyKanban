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
 *  Class:      Tasks
 *  
 *  Purpose:    Represents a collection of Task objects
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class Tasks : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public Tasks(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public Tasks(Board board, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsTasks = MyKanban.Data.GetTasksByBoard(board.Id, _credential.Id);
            foreach (DataRow drTask in dsTasks.Tables["results"].Rows)
            {
                Task task = new Task(long.Parse(drTask["id"].ToString()), _credential);
                task.Parent = board;
                task.ParentId = board.Id;
                task.ProjectName = drTask["project_name"].ToString();
                _items.Add(task);
            }
            _parent = board;
            _parentId = board.Id;
        }

        public Tasks(Project project, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsTasks = MyKanban.Data.GetTasksByProject(project.Id, _credential.Id);
            foreach (DataRow drTask in dsTasks.Tables["results"].Rows)
            {
                Task task = new Task(long.Parse(drTask["id"].ToString()), _credential);
                task.Parent = project;
                task.ParentId = project.Id;
                _items.Add(task);
            }
            _parent = project;
            _parentId = project.Id;
        }

        public Tasks(Task task, Credential credential)
        {
            if (credential != null) _credential = credential;

            if (task.Id > 0)
            {
                DataSet dsTasks = MyKanban.Data.GetSubTasksForTask(task.Id, _credential.Id);
                foreach (DataRow drTask in dsTasks.Tables["results"].Rows)
                {
                    Task subTask = new Task(long.Parse(drTask["id"].ToString()), _credential);
                    _items.Add(subTask);
                }
            }

            _parent = task;
            _parentId = task.Id;
        }

        public Tasks(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsTasks = MyKanban.Data.GetTasksByName(nameFilter, _credential.Id);
            foreach (DataRow drTask in dsTasks.Tables["results"].Rows)
            {
                Task task = new Task(long.Parse(drTask["id"].ToString()), _credential);
                _items.Add(task);
            }
        }

        #endregion

        #region Properties

        public Task this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        private List<Task> _items = new List<Task>();
        public List<Task> Items
        {
            get { return _items; }
        }

        private long _projectId = 0;
        public long ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        #endregion

        #region Methods

        public void Add(Task task)
        {
            _isDirty = true;
            _items.Add(task);
            task.Parent = _parent;
            if (_parent != null)
            {
                task.ParentId = _parent.Id;
                task.ProjectId = _projectId;

                if (_parent.GetType().ToString() == "MyKanban.Project")
                {
                    task.ProjectId = ((Project)_parent).Id;
                    task.ParentTaskId = 0;
                }
                else if (_parent.GetType().ToString() == "MyKanban.Task")
                {
                    task.ProjectId = ((Task)_parent).ProjectId;
                    task.ParentTaskId = _parent.Id;
                }
            }

            UpdateProjectId(task);
        }

        public void Clear(bool delete = false)
        {
            _items.Clear();
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Task)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(Task task, bool delete = false)
        {
            _items.Remove(task);

            task.ProjectId = 0;
            UpdateProjectId(task);
        }

        public bool Update(bool force = false)
        {
            try
            {
                foreach (Task task in _items)
                {
                    if (_parentId != 0) task.ParentId = _parentId;
                    task.ProjectId = _projectId;
                    task.Update(true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Propogate project id down task tree
        private void UpdateProjectId(Task task)
        {
            foreach (Task subTask in task.SubTasks.Items)
            {
                subTask.ProjectId = task.ProjectId;
                UpdateProjectId(subTask);
            }
            task.Update(true);
        }

        #endregion
    }
}
