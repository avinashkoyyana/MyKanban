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
 *  Class:      People
 *  
 *  Purpose:    Represents a collection of Person objects 
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    public class People : MyKanban.BaseList, MyKanban.IDataList
    {
        #region Constructors

        public People(Credential credential) 
        {
            if (credential != null) _credential = credential;
        }

        public People(MyKanban.IDataItem parent, Credential credential)
        {
            PeopleConstructor(parent, credential);
        }

        private void PeopleConstructor(IDataItem parent, Credential credential)
        {
            if (credential != null) _credential = credential;

            _parent = parent;
            string t = parent.GetType().ToString();
            _items.Clear();
            DataSet dsPeople = new DataSet();
            switch (t)
            {

                case "MyKanban.Project":
                    _parentId = ((Project)parent).Id;
                    dsPeople = MyKanban.Data.GetStakeholdersByProject(_parentId, _credential.Id);
                    if (dsPeople.Tables.Count > 0)
                    {
                        foreach (DataRow drPerson in dsPeople.Tables[0].Rows)
                        {
                            _items.Add(new Person(long.Parse(drPerson["id"].ToString()), _credential));
                        }
                    }
                    break;

                case "MyKanban.Task":
                    _parentId = ((Task)parent).Id;
                    dsPeople = MyKanban.Data.GetAssigneesByTask(_parentId, _credential.Id);
                    if (dsPeople.Tables.Count > 0)
                    {
                        foreach (DataRow drPerson in dsPeople.Tables[0].Rows)
                        {
                            _items.Add(new Person(long.Parse(drPerson["id"].ToString()), _credential));
                        }
                    }
                    break;


                default:
                    break;
            }
        }

        public People(string nameFilter, Credential credential)
        {
            if (credential != null) _credential = credential;

            DataSet dsPeople = MyKanban.Data.GetPeopleByName(nameFilter, _credential.Id);
            foreach (DataRow drPerson in dsPeople.Tables["results"].Rows)
            {
                Person person = new Person(long.Parse(drPerson["id"].ToString()), _credential);
                _items.Add(person);
            }
        }

        #endregion

        #region Properties

        public Person this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        private List<Person> _items = new List<Person>();
        public List<Person> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public void Add(Person person)
        {
            // Make sure person is added to database if
            // not already there
            person.Update();

            _items.Add(person);

            // Save to database
            if (_parent != null)
            {
                string t = _parent.GetType().ToString();
                switch (t)
                {
                    //case "MyKanban.Board":
                    //    Data.AddPersonToBoard(
                    //        _parentId, 
                    //        person.Id, 
                    //        _credential.Id,
                    //        person.CanAdd,
                    //        person.CanEdit,
                    //        person.CanDelete,
                    //        person.CanRead);
                    //    break;

                    case "MyKanban.Project":
                        Data.AddStakeholderToProject(_parentId, person.Id, _credential.Id);
                        break;

                    case "MyKanban.Task":
                        Data.AddAssigneeToTask(_parentId, person.Id, _credential.Id);
                        break;

                    default:
                        break;
                }
            }
        }

        public override void Clear(bool delete = false)
        {
            if (_items.Count > 0)
            {
                if (delete)
                {
                    string t = _parent.GetType().ToString();
                    DataSet dsPeople = new DataSet();
                    switch (t)
                    {
                        case "MyKanban.Task":
                            foreach (Person assignee in _items)
                            {
                                Data.DeleteAssigneeFromTask(_parent.Id, assignee.Id, _credential.Id);
                            }
                            break;
                        default:
                            break;
                    }
                }
                _items.Clear();
            }
        }

        override public List<BaseItem> GetBaseList()
        {
            List<BaseItem> baseList = new List<BaseItem>();
            foreach (var item in this._items)
            {
                BaseItem baseItem = (Person)item;
                baseList.Add(baseItem);
            }
            return baseList;
        }

        public override void Reload()
        {
            base.Reload();

            if (_parent != null) PeopleConstructor(_parent, _credential);
        }

        public override void Remove(int index, bool delete = false)
        {
            _items.Remove(_items[index]);
        }

        public void Remove(Person person, bool delete = false)
        {
            Person p = new Person(_credential);
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == person.Id)
                {
                    p = _items[i];
                    break;
                }
            }
            _items.Remove(p);

            if (_parent != null)
            {
                string t = _parent.GetType().ToString();
                DataSet dsPeople = new DataSet();
                switch (t)
                {
                    case "MyKanban.Board":
                        Data.DeleteUserFromBoard(_parentId, person.Id, _credential.Id);
                        break;

                    case "MyKanban.Project":
                        Data.DeleteStakeholderFromProject(_parent.Id, person.Id, _credential.Id);
                        break;

                    case "MyKanban.Task":
                        Data.DeleteAssigneeFromTask(_parent.Id, person.Id, _credential.Id);
                        break;

                    default:
                        break;
                }
            }
        }

        public bool Update(bool force = false)
        {
            try
            {
                string t = _parent.GetType().ToString();
                switch (t)
                {
                    case "MyKanban.Board":
                        foreach (Person person in _items)
                        {
                            Data.AddPersonToBoard(
                                ((Board)_parent).Id, 
                                person.Id, 
                                _credential.Id,
                                false,
                                false,
                                false,
                                true);
                        }
                        break;

                    case "MyKanban.Task":
                        foreach (Person person in _items)
                        {
                            Data.AddAssigneeToTask(_parent.Id,person.Id, _credential.Id);
                        }
                        break;
                    default:
                        break;
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
