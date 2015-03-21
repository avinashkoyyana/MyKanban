using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using MyKanban;

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
 *  Class:      Main
 *  
 *  Purpose:    Sample MyKanban Object Browser
 *  
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace SampleOB
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        #region Classes

        /// <summary>
        /// A specialized variant of the TreeNode class with properties
        /// to store MyKanban objects and collections
        /// </summary>
        private class MyKanbanTreeNode : TreeNode
        {
            public MyKanbanTreeNode(string Text)
            {
                this.Text = Text;
            }
            public bool propertiesHaveBeenFetched = false;
            public IDataItem MyKanbanObject = null;
            public IDataItem MyKanbanParent = null;
            public object MyKanbanCollectionObject = null;
            public bool ReadOnly = false;
            public List<BaseItem> MyKanbanCollection
            {
                get
                {
                    if (MyKanbanCollectionObject != null && isCollection)
                    {
                        var collection = new MyKanban.BaseList();
                        switch (CollectionType)
                        {
                            case "MyKanban.Approvers":
                                collection = ((MyKanban.Approvers)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Boards":
                                collection = ((MyKanban.Boards)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.BoardSets":
                                collection = ((MyKanban.BoardSets)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Comments": 
                                collection = ((MyKanban.Comments)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.People":
                                collection = ((MyKanban.People)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Projects":
                                collection = ((MyKanban.Projects)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Properties":
                                collection = ((MyKanban.Properties)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Sprints":
                                collection = ((MyKanban.Sprints)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.StatusCodes":
                                collection = ((MyKanban.StatusCodes)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Tasks":
                                collection = ((MyKanban.Tasks)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Tags":
                                collection = ((MyKanban.Tags)MyKanbanCollectionObject);
                                break;

                            case "MyKanban.Users":
                                collection = ((MyKanban.Users)MyKanbanCollectionObject);
                                break;

                            default: return null;
                        }

                        if (isDirty) collection.Reload();
                        isDirty = false;
                        return collection.GetBaseList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            public bool isDirty = true;

            private string _filter = "";
            public string Filter
            {
                get 
                {
                    if (isCollection) return _filter;
                    else return ""; 
                }
                set 
                { 
                    _filter = value; 
                }
            }

            public List<BaseItem> GetFilteredList()
            {
                List<BaseItem> items = MyKanbanCollection;
                if (!string.IsNullOrEmpty(Filter))
                {
                    // Use LINQ to filter the tasks based on user input
                    items = (from item in items
                             where item.Name.ToLower().Contains(Filter.ToLower())
                             select item).ToList<BaseItem>();
                }
                return items;
            }

            private bool _isCollection = false;
            public bool isCollection
            {
                get { return _isCollection; }
                set 
                { 
                    _isCollection = value; 
                    if (value)
                    {
                        this.NodeFont = new System.Drawing.Font("Tahoma", 10, FontStyle.Italic);
                        System.Drawing.ColorConverter colorConverter = new System.Drawing.ColorConverter();
                        this.ForeColor = (System.Drawing.Color)colorConverter.ConvertFromString("Navy");
                    }
                    else
                    {
                        this.NodeFont = DefaultFont;
                        this.ForeColor = DefaultForeColor;
                    }
                }
            }

            //public bool isCollection = false;

            public string CollectionType
            {
                get
                {
                    if (MyKanbanCollectionObject != null) return MyKanbanCollectionObject.GetType().ToString();
                    else return null;
                }
            }

            public string ParentType
            {
                get 
                {
                    if (MyKanbanParent != null) return MyKanbanParent.GetType().ToString();
                    else return null;
                }
            }

            public long ParentId
            {
                get
                {
                    if (MyKanbanParent != null) return ((IDataItem)MyKanbanParent).Id;
                    else return 0;
                }
            }
        }

        /// <summary>
        /// Renders a date-picker in the datagrid
        /// </summary>
        public class CalendarCell : DataGridViewTextBoxCell
        {

            public CalendarCell()
                : base()
            {
                // Use the short date format. 
                this.Style.Format = "d";
            }

            public override void InitializeEditingControl(int rowIndex, object
                initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                // Set the value of the editing control to the current cell value. 
                base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
                CalendarEditingControl ctl =
                    DataGridView.EditingControl as CalendarEditingControl;
                // Use the default row value when Value property is null. 
                if (this.Value == null)
                {
                    ctl.Value = (DateTime)this.DefaultNewRowValue;
                }
                else
                {
                    ctl.Value = (DateTime)this.Value;
                }

                ctl.TextChanged += ctl_TextChanged;
            }

            void ctl_TextChanged(object sender, EventArgs e)
            {
                //this.Style.BackColor = Color.Yellow;
            }

            public override Type EditType
            {
                get
                {
                    // Return the type of the editing control that CalendarCell uses. 
                    return typeof(CalendarEditingControl);
                }
            }

            public override Type ValueType
            {
                get
                {
                    // Return the type of the value that CalendarCell contains. 

                    return typeof(DateTime);
                }
            }

            public override object DefaultNewRowValue
            {
                get
                {
                    // Use the current date and time as the default value. 
                    return DateTime.Now;
                }
            }
        }

        class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
        {
            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            public CalendarEditingControl()
            {
                this.Format = DateTimePickerFormat.Short;
            }

            // Implements the IDataGridViewEditingControl.EditingControlFormattedValue  
            // property. 
            public object EditingControlFormattedValue
            {
                get
                {
                    return this.Value.ToShortDateString();
                }
                set
                {
                    if (value is String)
                    {
                        try
                        {
                            // This will throw an exception of the string is  
                            // null, empty, or not in the format of a date. 
                            this.Value = DateTime.Parse((String)value);
                        }
                        catch
                        {
                            // In the case of an exception, just use the  
                            // default value so we're not left with a null 
                            // value. 
                            this.Value = DateTime.Now;
                        }
                    }
                }
            }

            // Implements the  
            // IDataGridViewEditingControl.GetEditingControlFormattedValue method. 
            public object GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            // Implements the  
            // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method. 
            public void ApplyCellStyleToEditingControl(
                DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
                this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
            }

            // Implements the IDataGridViewEditingControl.EditingControlRowIndex  
            // property. 
            public int EditingControlRowIndex
            {
                get
                {
                    return rowIndex;
                }
                set
                {
                    rowIndex = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey  
            // method. 
            public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
            {
                // Let the DateTimePicker handle the keys listed. 
                switch (key & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                        return true;
                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit  
            // method. 
            public void PrepareEditingControlForEdit(bool selectAll)
            {
                // No preparation needs to be done.
            }

            // Implements the IDataGridViewEditingControl 
            // .RepositionEditingControlOnValueChange property. 
            public bool RepositionEditingControlOnValueChange
            {
                get
                {
                    return false;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingControlDataGridView property. 
            public DataGridView EditingControlDataGridView
            {
                get
                {
                    return dataGridView;
                }
                set
                {
                    dataGridView = value;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingControlValueChanged property. 
            public bool EditingControlValueChanged
            {
                get
                {
                    return valueChanged;
                }
                set
                {
                    valueChanged = value;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingPanelCursor property. 
            public Cursor EditingPanelCursor
            {
                get
                {
                    return base.Cursor;
                }
            }

            protected override void OnValueChanged(EventArgs eventargs)
            {
                // Notify the DataGridView that the contents of the cell 
                // have changed.
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
        }

        /// <summary>
        /// Renders a drop-down list for status codes in the datagrid
        /// </summary>
        public class StatusCell : DataGridViewTextBoxCell
        {
            private StatusCodes _statusCodes;
            private long _statusValue;

            public StatusCell() { }

            public StatusCell(StatusCodes statusCodes, long statusValue)
            {
                _statusCodes = statusCodes;
                _statusValue = statusValue;
                this.Value = statusValue;
            }

            public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

                //StatusEditingControl ddlStatus = new StatusEditingControl(_statusCodes, _statusValue);

                StatusEditingControl ctl =
                    DataGridView.EditingControl as StatusEditingControl;

                ctl.StatusCodes = _statusCodes;
                ctl.Status = _statusValue;
                ctl.DisplayMember = "ColumnHeading";
                ctl.ValueMember = "Status";
                ctl.Refresh();

                if (this.Value == null)
                {
                    // Set default value
                    ctl.SelectedIndex = 0;
                }
                else
                {
                    // Assign value
                    //ctl.SelectedValue = _statusValue;
                }
            }

            public override Type EditType
            {
                get
                {
                    return typeof(StatusEditingControl);
                }
            }

            public override Type ValueType
            {
                get
                {
                    return typeof(int);
                }
            }
        }

        class StatusEditingControl : ComboBox, IDataGridViewEditingControl
        {
            public StatusEditingControl()
            {
                this.DisplayMember = "ColumnHeading";
                this.ValueMember = "Status";
            }

            public StatusEditingControl(StatusCodes statusCodes, long statusValue)
            {
                this.DisplayMember = "ColumnHeading";
                this.ValueMember = "Status";
                this.DataSource = statusCodes;
                this.SelectedValue = statusValue;
            }

            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            private StatusCodes _statusCodes;

            public StatusCodes StatusCodes
            {
                get { return _statusCodes; }
                set
                {
                    _statusCodes = value;
                    this.DataSource = _statusCodes.Items;
                }
            }
            private long _status;

            public long Status
            {
                get { return _status; }
                set
                {
                    _status = value;
                    this.SelectedValue = _status;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlFormattedValue  
            // property. 
            public object EditingControlFormattedValue
            {
                get
                {
                    return this.SelectedValue;
                }
                set
                {

                    try
                    {
                        // This will throw an exception of the string is  
                        // null, empty, or not in the format of a date. 
                        this.SelectedValue = (int)value;
                    }
                    catch
                    {
                        // No op
                    }
                }
            }

            // Implements the  
            // IDataGridViewEditingControl.GetEditingControlFormattedValue method. 
            public object GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            // Implements the  
            // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method. 
            public void ApplyCellStyleToEditingControl(
                DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.ForeColor = dataGridViewCellStyle.ForeColor;
                this.BackColor = dataGridViewCellStyle.BackColor;
            }

            // Implements the IDataGridViewEditingControl.EditingControlRowIndex  
            // property. 
            public int EditingControlRowIndex
            {
                get
                {
                    return rowIndex;
                }
                set
                {
                    rowIndex = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey  
            // method. 
            public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
            {
                // Let the DateTimePicker handle the keys listed. 
                switch (key & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                        return true;
                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit  
            // method. 
            public void PrepareEditingControlForEdit(bool selectAll)
            {
                // No preparation needs to be done.
            }

            // Implements the IDataGridViewEditingControl 
            // .RepositionEditingControlOnValueChange property. 
            public bool RepositionEditingControlOnValueChange
            {
                get
                {
                    return false;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingControlDataGridView property. 
            public DataGridView EditingControlDataGridView
            {
                get
                {
                    return dataGridView;
                }
                set
                {
                    dataGridView = value;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingControlValueChanged property. 
            public bool EditingControlValueChanged
            {
                get
                {
                    return valueChanged;
                }
                set
                {
                    valueChanged = value;
                }
            }

            // Implements the IDataGridViewEditingControl 
            // .EditingPanelCursor property. 
            public Cursor EditingPanelCursor
            {
                get
                {
                    return base.Cursor;
                }
            }

            protected override void OnSelectedIndexChanged(EventArgs e)
            {
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnSelectedIndexChanged(e);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores the MyKanban object that the currently selected tree node represents
        /// </summary>
        object currentObj = null;

        /// <summary>
        /// Stores the credentials of the currently logged in user
        /// </summary>
        Credential user = new Credential();

        #endregion

        #region Methods

        /// <summary>
        /// Add a single child node to a collection node
        /// </summary>
        /// <param name="parentNode">Collection node to which to add child node</param>
        /// <param name="parentObject">MyKanban object that parent node represents</param>
        /// <param name="childObject">MyKanban object that child node represents</param>
        private void AddChildNode(MyKanbanTreeNode parentNode, IDataItem parentObject, IDataItem childObject)
        {
            if (string.IsNullOrEmpty(parentNode.Filter) || ((IDataItem)childObject).Name.ToLower().Contains(parentNode.Filter.ToLower()))
            {
                MyKanbanTreeNode childNode = new MyKanbanTreeNode(childObject.Name);
                childNode.MyKanbanObject = (IDataItem)childObject;
                childNode.MyKanbanParent = (IDataItem)parentObject;
                childNode.ReadOnly = parentNode.ReadOnly;
                parentNode.Nodes.Add(childNode);
            }
        }

        /// <summary>
        /// Checkbox to determine whether to show read-only properties (default)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbShowReadOnlyProperties_CheckedChanged(object sender, EventArgs e)
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            if (selectedNode.MyKanbanObject != null)
            {
                ShowProperties(currentObj, GetBoardSet(selectedNode));
            }
        }

        /// <summary>
        /// Called when the "Delete" button clicked for a given tree node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to permanetly delete this object?", "Warning: Action is Not Undoable", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
                object obj = selectedNode.MyKanbanObject;
                MyKanbanTreeNode parentCollectionNode = (MyKanbanTreeNode)treeView1.SelectedNode.Parent;
                parentCollectionNode.isDirty = true;
                string parentType = selectedNode.ParentType;
                long parentId = selectedNode.ParentId;

                if (obj != null)
                {
                    // Need some special processing for "Person" type objects, because
                    // in some instances they may reference links to a person, rather than
                    // the base Person object.
                    Type type = obj.GetType();
                    switch (type.ToString())
                    {
                        case "MyKanban.Person":
                            switch (parentType)
                            {
                                case "MyKanban.Project":
                                    Project parentProject = new Project(parentId, user);
                                    parentProject.Stakeholders.Remove((Person)obj);
                                    parentProject.Stakeholders.Update(true);
                                    break;
                                case "MyKanban.Task":
                                    MyKanban.Task parentTask = new MyKanban.Task(parentId, user);
                                    parentTask.Assignees.Remove((Person)obj);
                                    parentTask.Assignees.Update(true);
                                    break;
                                default:
                                    ((Person)obj).Delete();
                                    break;
                            }
                            break;

                        default:
                            try
                            {
                                ((BaseItem)obj).Delete();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Objects of this type cannot be deleted", "Warning - Unable to Delete Object", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                    }
                    treeView1.SelectedNode.Remove();
                    parentCollectionNode.Text = parentCollectionNode.Text.Split(' ')[0] + " (" + parentCollectionNode.Nodes.Count.ToString() + ")";
                }
                else
                {
                    MessageBox.Show("Objects of this type cannot be deleted", "Warning - Unable to Delete Object", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        /// <summary>
        /// Called when JSON button clicked for a given tree node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdJson_Click(object sender, EventArgs e)
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            object obj = null;

            if (selectedNode.MyKanbanObject != null) obj = selectedNode.MyKanbanObject;
            else if (selectedNode.MyKanbanCollection != null) obj = selectedNode.MyKanbanCollection;

            if (obj != null)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    JSONForm jsonForm = new JSONForm();
                    jsonForm.JSON = ((Base)obj).JSON();
                    jsonForm.ShowDialog();
                    this.Cursor = Cursors.Default;
                }
                catch
                {
                    MessageBox.Show("Cannot display JSON for objects of this type. If you wish to obtain JSON for a collection, please select its parent object", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Cannot display JSON for objects of this type. If you wish to obtain JSON for a collection, please select its parent object", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Called when "New" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNew_Click(object sender, EventArgs e)
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            string colType = selectedNode.Text.Split(' ')[0];

            switch (colType)
            {
                case "Approvers":
                    currentObj = new Approver(user);
                    break;

                case "Assignees":
                    currentObj = new Person(user);
                    break;

                case "BoardSets":
                    currentObj = new BoardSet(user);
                    break;

                case "Comments":
                    currentObj = new Comment(user);
                    break;

                case "Boards":
                    currentObj = new Board(user);
                    break;

                case "People":
                    currentObj = new Person(user);
                    break;

                case "Projects":
                    currentObj = new Project(user);
                    break;

                case "Properties":
                    currentObj = new Property(user);
                    break;

                case "Sprints":
                    currentObj = new Sprint(user);
                    break;

                case "Stakeholders":
                    currentObj = new Person(user);
                    break;

                case "StatusCodes":
                    currentObj = new StatusCode(user);
                    break;

                case "SubTasks":
                    currentObj = new MyKanban.Task(user);
                    break;

                case "Tags":
                    currentObj = new MyKanban.Tag(user);
                    break;

                case "Tasks":
                    currentObj = new MyKanban.Task(user);
                    break;

                case "Users":
                    currentObj = new User(user);
                    break;

                default:
                    MessageBox.Show("Adding to collections of this type is not supported", "Warning: Unsupported Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            MyKanbanTreeNode tnNewChildNode = new MyKanbanTreeNode("(new)");
            selectedNode.Nodes.Add(tnNewChildNode);
            treeView1.SelectedNode = tnNewChildNode;
            ((BaseItem)currentObj).Name = tnNewChildNode.Text;
            ShowProperties(currentObj, GetBoardSet(tnNewChildNode));
        }

        /// <summary>
        /// Called when "Save" button is clicked.  This is the most complex method, because
        /// it needs to take into account various processing requirements for the different
        /// MyKanban obect types.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            Type type = currentObj.GetType();

            // Each row in properties grid represents one property of object to save
            foreach (DataGridViewRow row in propertiesGridView.Rows)
            {
                // The color of the first cell is set to red when a property is edited,
                // this is how we know which properties to write back
                if (row.Cells["EditFlag"].Style.BackColor == Color.Red)
                {
                    string propertyName = row.Cells["Property"].Value.ToString();
                    string propertyValue = row.Cells["Value"].Value.ToString();
                    PropertyInfo prop = currentObj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite)
                    {
                        switch (prop.PropertyType.FullName)
                        {
                            case "System.Double":
                                prop.SetValue(currentObj, double.Parse(propertyValue), null);
                                break;
                            case "System.DateTime":
                                prop.SetValue(currentObj, DateTime.Parse(propertyValue), null);
                                break;
                            case "System.Int32":
                                prop.SetValue(currentObj, Int32.Parse(propertyValue.Split('.')[0]), null);
                                break;
                            case "System.Int64":
                                prop.SetValue(currentObj, Int64.Parse(propertyValue.Split('.')[0]), null);
                                break;
                            case "System.Boolean":
                                prop.SetValue(currentObj, bool.Parse(propertyValue), null);
                                break;
                            default:
                                prop.SetValue(currentObj, propertyValue, null);
                                break;
                        }
                    }

                    row.Cells["EditFlag"].Style.BackColor = Color.White;
                }
            }

            treeView1.SelectedNode.Text = ((BaseItem)currentObj).Name;

            MyKanbanTreeNode parentNode = (MyKanbanTreeNode)selectedNode.Parent.Parent;
            MyKanbanTreeNode collectionNode = (MyKanbanTreeNode)selectedNode.Parent;
            bool isNewChild = (((BaseItem)currentObj).Id == 0); ;
            long parentId = (parentNode != null && parentNode.MyKanbanObject != null ? ((BaseItem)parentNode.MyKanbanObject).Id : 0);
            string parentType = parentId != 0 ? parentNode.ParentType : "";

            // Now write data back to database
            switch (type.ToString())
            {

                case "MyKanban.Approver":

                    Approver approver = ((Approver)currentObj);
                    approver.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.Task parentTask = new MyKanban.Task(parentId, user);
                        parentTask.Approvers.Add(approver);
                        parentTask.Approvers.Update();
                        collectionNode.MyKanbanCollectionObject = parentTask.Approvers;
                        collectionNode.Text = "Approvers (" + parentTask.Approvers.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = approver;
                    selectedNode.Text = approver.Name;
                    selectedNode.isCollection = false;
                    break;

                case "MyKanban.Board":

                    Board board = ((Board)currentObj);
                    board.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.BoardSet parentBoardSet = new MyKanban.BoardSet(parentId, user);
                        parentBoardSet.Boards.Add(board);
                        parentBoardSet.Boards.Update();
                        collectionNode.MyKanbanCollectionObject = parentBoardSet.Boards;
                        collectionNode.Text = "Boards (" + parentBoardSet.Boards.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = board;
                    selectedNode.isCollection = false;
                    break;

                case "MyKanban.BoardSet":

                    BoardSet boardSet = ((BoardSet)currentObj);
                    boardSet.Update(true);
                    selectedNode.MyKanbanObject = boardSet;
                    selectedNode.isCollection = false;
                    break;

                case "MyKanban.Comment":

                    Comment comment = ((Comment)currentObj);
                    comment.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.Task parentTask = new MyKanban.Task(parentId, user);
                        parentTask.Comments.Add(comment);
                        parentTask.Comments.Update();
                        collectionNode.MyKanbanCollectionObject = parentTask.Comments;
                        collectionNode.Text = "Comments (" + parentTask.Comments.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = comment;
                    break;

                case "MyKanban.Person":

                    // Unless this is any entry in the root-level "People" collection,
                    // try to find the person by name, if name has not be edited from default,
                    // replace it with blank value before searching.
                    Person person = (Person)currentObj;
                    if (selectedNode.Level > 1)
                    {
                        People matches = new People(person.Name.Replace("(new)", ""), user);
                        if (matches.Count == 0)
                        {
                            MessageBox.Show("No matching name, please enter the name or username of an existing person to assign to this record", "Error - no match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        else if (matches.Count > 1)
                        {
                            bool userNameMatch = false;
                            foreach (Person match in matches.Items)
                            {
                                if (match.UserName.ToLower().Trim() == person.UserName.ToLower().Trim())
                                {
                                    userNameMatch = true;
                                    person = match;
                                    break;
                                }
                            }
                            if (!userNameMatch)
                            {
                                MessageBox.Show("No matching name, please enter the name or username of an existing person to assign to this record", "Error - no match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        else
                        {
                            person = matches[0];
                        }
                    }

                    // If we've got this far, we have a match
                    switch (collectionNode.ParentType)
                    {
                        case "MyKanban.Project":
                            Project p = new Project(parentId, user);
                            p.Stakeholders.Add(person);
                            p.Stakeholders.Update(true);
                            collectionNode.MyKanbanCollectionObject = p.Stakeholders;
                            collectionNode.Text = "Stakeholders (" + collectionNode.Nodes.Count.ToString() + ")";
                            treeView1.SelectedNode.Text = person.Name;
                            break;
                        case "MyKanban.Task":
                            MyKanban.Task t = new MyKanban.Task(parentId, user);
                            t.Assignees.Add(person);
                            t.Assignees.Update(true);
                            collectionNode.MyKanbanCollectionObject = t.Assignees;
                            collectionNode.Text = "Assignees (" + collectionNode.Nodes.Count.ToString() + ")";
                            treeView1.SelectedNode.Text = person.Name;
                            break;
                        default:
                            ((Person)currentObj).Update(true);
                            if (isNewChild) LoadData();
                            treeView1.Nodes[1].Expand();
                            break;
                    }
                    selectedNode.MyKanbanObject = person;
                    collectionNode.isDirty = true;
                    break;

                case "MyKanban.Project":

                    Project project = ((Project)currentObj);
                    project.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.Board parentBoard = new MyKanban.Board(parentId, user);
                        parentBoard.Projects.Add(project);
                        parentBoard.Projects.Update();
                        collectionNode.MyKanbanCollectionObject = parentBoard.Projects;
                        collectionNode.Text = "Projects (" + parentBoard.Projects.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = project;
                    break;

                case "MyKanban.Property":

                    Property property = ((Property)currentObj);
                    property.ParentId = ((IDataItem)parentNode.MyKanbanObject).Id;
                    switch (collectionNode.ParentType)
                    {
                        case "MyKanban.Board":
                            Board b = new Board(parentId, user);
                            b.Properties.Add(property);
                            b.Properties.Update(true);
                            collectionNode.MyKanbanCollectionObject = b.Properties;
                            break;
                        case "MyKanban.Person":
                            Person psn = new Person(parentId, user);
                            psn.Properties.Add(property);
                            psn.Properties.Update(true);
                            collectionNode.MyKanbanCollectionObject = psn.Properties;
                            break;
                        case "MyKanban.Project":
                            Project p = new Project(parentId, user);
                            p.Properties.Add(property);
                            p.Properties.Update(true);
                            collectionNode.MyKanbanCollectionObject = p.Properties;
                            break;
                        case "MyKanban.Task":
                            MyKanban.Task t = new MyKanban.Task(parentId, user);
                            t.Properties.Add(property);
                            t.Properties.Update(true);
                            collectionNode.MyKanbanCollectionObject = t.Properties;
                            break;
                        default:
                            break;
                    }
                    collectionNode.Text = "Properties (" + collectionNode.Nodes.Count.ToString() + ")";
                    collectionNode.isDirty = true;
                    selectedNode.MyKanbanObject = property;
                    break;

                case "MyKanban.Sprint":

                    Sprint sprint = ((Sprint)currentObj);
                    sprint.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.Board parentBoard = new MyKanban.Board(parentId, user);
                        parentBoard.Sprints.Add(sprint);
                        parentBoard.Sprints.Update(true);
                        collectionNode.MyKanbanCollectionObject = parentBoard.Sprints;
                        collectionNode.Text = "Sprints (" + parentBoard.Sprints.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = sprint;
                    break;

                case "MyKanban.StatusCode":

                    StatusCode statusCode = ((StatusCode)currentObj);
                    statusCode.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.BoardSet parentBoardSet = new MyKanban.BoardSet(parentId, user);
                        parentBoardSet.StatusCodes.Add(statusCode);
                        parentBoardSet.StatusCodes.Update();
                        collectionNode.MyKanbanCollectionObject = parentBoardSet.StatusCodes;
                        collectionNode.Text = "StatusCodes (" + parentBoardSet.StatusCodes.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = statusCode;

                    // Because of the way "Name" property is configured in StatusCode class, need
                    // to set text of node after object is updated.
                    selectedNode.Text = ((BaseItem)currentObj).Name;
                    break;

                case "MyKanban.Tag":

                    Tag tag = ((Tag)currentObj);
                    tag.Update(true);
                    if (isNewChild)
                    {
                        MyKanban.Task parentTask = new MyKanban.Task(parentId, user);
                        parentTask.Tags.Add(tag);
                        parentTask.Tags.Update(true);
                        collectionNode.MyKanbanCollectionObject = parentTask.Tags;
                        collectionNode.Text = "Tags (" + parentTask.Tags.Count.ToString() + ")";
                        collectionNode.isDirty = true;
                    }
                    selectedNode.MyKanbanObject = tag;
                    break;

                case "MyKanban.Task":

                    MyKanban.Task task = ((MyKanban.Task)currentObj);
                    task.Update(true);
                    if (isNewChild)
                    {
                        switch (collectionNode.ParentType)
                        {
                            case "MyKanban.Project":
                                Project p = new Project(parentId, user);
                                p.Tasks.Add(task);
                                p.Tasks.Update(true);
                                collectionNode.MyKanbanCollectionObject = p.Tasks;
                                collectionNode.Text = "Tasks (" + collectionNode.Nodes.Count.ToString() + ")";
                                break;
                            case "MyKanban.Task":
                                MyKanban.Task pt = new MyKanban.Task(parentId, user);
                                pt.SubTasks.Add(task);
                                pt.SubTasks.Update(true);
                                collectionNode.MyKanbanCollectionObject = pt.SubTasks;
                                collectionNode.Text = "SubTasks (" + collectionNode.Nodes.Count.ToString() + ")";
                                break;
                            default:
                                break;
                        }
                    }
                    collectionNode.isDirty = true;
                    selectedNode.MyKanbanObject = task;
                    break;

                case "MyKanban.User":

                    User boardUser = ((User)currentObj);
                    if (boardUser.Id > 0)
                    {
                        // Get copy of edited values
                        bool canAdd = boardUser.CanAdd;
                        bool canEdit = boardUser.CanEdit;
                        bool canDelete = boardUser.CanDelete;
                        bool canRead = boardUser.CanRead;

                        // Now get full user object based on id# entered
                        boardUser = new User(boardUser.Id, user);

                        // copy edited values to full object
                        boardUser.CanAdd = canAdd;
                        boardUser.CanEdit = canEdit;
                        boardUser.CanDelete = canDelete;
                        boardUser.CanRead = canRead;
                    }
                    boardUser.Update();
                    board = new Board(parentId, user);
                    board.Users.Add(boardUser);
                    board.Users.Update();
                    selectedNode.MyKanbanObject = boardUser;
                    selectedNode.Text = boardUser.Name;
                    collectionNode.Text = "Users (" + board.Users.Count.ToString() + ")";
                    collectionNode.isDirty = true;
                    break;

                default:

                    MessageBox.Show("Objects of this type cannot be saved", "Warning - Unable to Save Object", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            ShowProperties(currentObj, GetBoardSet(selectedNode));

        }

        /// <summary>
        /// Processes click on filter button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            // TODO : special handling for root collections
            try
            {
                MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
                string filter = txtFilter.Text;
                MyKanbanTreeNode parentNode = selectedNode.Parent != null ? (MyKanbanTreeNode)selectedNode.Parent : null;
                if (parentNode != null)
                {
                    parentNode.Nodes.Remove(selectedNode);
                }
                else
                {
                    treeView1.Nodes.Remove(selectedNode);
                }
                PopulateCollection(parentNode, selectedNode.Text.Split(' ')[0], selectedNode.MyKanbanParent, selectedNode.ReadOnly, selectedNode.MyKanbanCollectionObject.GetType().ToString(), selectedNode.MyKanbanCollectionObject, filter);
                selectedNode.Filter = filter;
                selectedNode.isDirty = true;
                collectionGridView.DataSource = selectedNode.GetFilteredList();
            }
            catch { }
        }

        /// <summary>
        /// Get the object the currently selected tree node represents
        /// </summary>
        /// <returns></returns>
        private object CurrentObject()
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            object obj = null;
            if (selectedNode.MyKanbanObject != null) obj = selectedNode.MyKanbanObject;
            return obj;
        }

        /// <summary>
        /// Called when user clicks on "Exit" menu option at top of form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// When clicking on a node, get the BoardSet the node is a member of
        /// by recursively walking up the tree until a BoardSet object is
        /// found, or the top of the tree reached.
        /// </summary>
        /// <param name="node">Node whose BoardSet we want to find</param>
        /// <returns>A MyKanban BoardSet object</returns>
        private BoardSet GetBoardSet(MyKanbanTreeNode node)
        {
            if (node.Level > 1) return GetBoardSet((MyKanbanTreeNode)node.Parent);
            else if (node.MyKanbanObject != null && node.MyKanbanObject.GetType().ToString() == "MyKanban.BoardSet") return (BoardSet)node.MyKanbanObject;
            else return null;
        }

        /// <summary>
        /// Get things started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Tidy things up a bit
            treeView1.Top = collectionGridView.Top;
            treeView1.Height = collectionGridView.Height;

            Login();
        }

        /// <summary>
        /// Get things started by loading the two top-level nodes
        /// for BoardSets and People
        /// </summary>
        private void LoadData()
        {
            propertiesGridView.Visible = false;
            propertiesGridView.Left = collectionGridView.Left;
            propertiesGridView.Top = collectionGridView.Top;
            propertiesGridView.Width = collectionGridView.Width;
            propertiesGridView.Height = collectionGridView.Height;

            treeView1.Nodes.Clear();

            // Load BoardSets
            MyKanbanTreeNode tnBoardSets = new MyKanbanTreeNode("BoardSets");
            BoardSets boardSets = new BoardSets("", user);
            tnBoardSets.isCollection = true;
            tnBoardSets.MyKanbanCollectionObject = boardSets;
            treeView1.Nodes.Add(tnBoardSets);
            foreach (BoardSet boardSet in boardSets.Items)
            {
                MyKanbanTreeNode tnBoardSet = new MyKanbanTreeNode(boardSet.Name);
                tnBoardSet.MyKanbanObject = boardSet;
                tnBoardSets.Nodes.Add(tnBoardSet);
            }
            tnBoardSets.Text = "BoardSets (" + tnBoardSets.Nodes.Count.ToString() + ")";

            // Load People
            MyKanbanTreeNode tnPeople = new MyKanbanTreeNode("People");
            People people = new People("", user);
            tnPeople.isCollection = true;
            tnPeople.MyKanbanCollectionObject = people;
            treeView1.Nodes.Add(tnPeople);
            foreach (Person person in people.Items)
            {
                MyKanbanTreeNode tnPerson = new MyKanbanTreeNode(person.Name);
                tnPerson.MyKanbanObject = person;
                tnPeople.Nodes.Add(tnPerson);
            }
            tnPeople.Text = "People (" + tnPeople.Nodes.Count.ToString() + ")";
        }

        /// <summary>
        /// Render login form and verify user name and password
        /// against MyKanban database
        /// </summary>
        private void Login()
        {
            // If user was previously logged in, should log them out first so
            // data is not visible on screen for others to see
            user = new Credential();
            LoadData();

            // Prompt user to login 
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            if (loginForm.Credential != null && loginForm.Credential.Id > 0)
            {
                // If user has entered a valid user name and password,
                // log them in and get their boards
                user = loginForm.Credential;
                toolStripStatusLabel5.Text = user.Name;
                toolStripStatusLabel5.Font = toolStripStatusLabel4.Font;
                toolStripStatusLabel5.ForeColor = toolStripStatusLabel4.ForeColor;
                loginToolStripMenuItem.Text = "Logout";
                LoadData();
            }
            else
            {
                // Otherwise, show that no user logged in
                toolStripStatusLabel5.Text = "Not logged in";
                toolStripStatusLabel5.Font = new Font(toolStripStatusLabel5.Font, FontStyle.Italic);
                toolStripStatusLabel5.ForeColor = Color.Red;
                loginToolStripMenuItem.Text = "Login";
            }
        }

        /// <summary>
        /// Called when the "Login/Logout" option is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login();
        }

        /// <summary>
        /// Display a collection node, along with all of its children objects
        /// </summary>
        /// <param name="parentNode">Node to add this collection to</param>
        /// <param name="propertyName">Text to use for collection node</param>
        /// <param name="parentObject">MyKanban object that owns this collection</param>
        /// <param name="readOnly">Should node be treated as read-only</param>
        /// <param name="propertyType">The MyKanban class name of this collection class</param>
        /// <param name="collectionObject">The MyKanban collection object</param>
        /// <param name="filter">Filter (if any) that should be applied to children of this collection</param>
        private void PopulateCollection(MyKanbanTreeNode parentNode, string propertyName, IDataItem parentObject, bool readOnly, string propertyType, object collectionObject, string filter = "")
        {
            MyKanbanTreeNode tnCollection = new MyKanbanTreeNode(propertyName);
            tnCollection.Filter = filter;
            tnCollection.isCollection = true;
            tnCollection.isDirty = false;
            tnCollection.MyKanbanParent = parentObject;
            if (parentNode != null)
            {
                parentNode.Nodes.Add(tnCollection);
            }
            else
            {
                treeView1.Nodes.Add(tnCollection);
            }
            if (readOnly) tnCollection.ReadOnly = true;

            // Use dynamic type so can be implicitly
            // cast to correct type of collection at runtime
            dynamic collection = null;
            switch (propertyType)
            {
                case "MyKanban.Approvers":
                    collection = (Approvers)collectionObject;
                    break;

                case "MyKanban.Boards":
                    collection = (Boards)collectionObject;
                    break;

                case "MyKanban.BoardSets":
                    collection = (BoardSets)collectionObject;
                    break;

                case "MyKanban.Comments":
                    collection = (Comments)collectionObject;
                    break;

                case "MyKanban.People":
                    collection = (People)collectionObject;
                    break;
                case "MyKanban.Projects":
                    collection = (Projects)collectionObject;
                    break;

                case "MyKanban.Properties":
                    collection = (MyKanban.Properties)collectionObject;
                    break;

                case "MyKanban.Sprints":
                    collection = (Sprints)collectionObject;
                    break;

                case "MyKanban.StatusCodes":
                    collection = (StatusCodes)collectionObject;
                    break;

                case "MyKanban.Tasks":
                    collection = (MyKanban.Tasks)collectionObject;
                    break;

                case "MyKanban.Tags":
                    collection = (Tags)collectionObject;
                    break;

                case "MyKanban.Users":
                    collection = (Users)collectionObject;
                    break;

                default:
                    break;
            }

            // Add the collection's children
            tnCollection.MyKanbanCollectionObject = collection;
            foreach (IDataItem childObject in collection.Items)
            {
                AddChildNode(tnCollection, parentObject, childObject);
            }

            // Update text of collection node to reflect # of children and
            // any filter
            tnCollection.Text += " (" + tnCollection.Nodes.Count.ToString() + ")";
            if (!string.IsNullOrEmpty(filter)) tnCollection.Text += " [\"" + filter + "\"]";
        }

        /// <summary>
        /// Use .NET reflection to display all properties for a given object
        /// </summary>
        /// <param name="obj">Object to display</param>
        private void PopulateNodes(object obj, MyKanbanTreeNode node)
        {
            if (node.Level > 20)
            {
                MessageBox.Show("Maximum tree depth reached", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Type type = obj.GetType();
            PropertyInfo[] _PropertyInfo = type.GetProperties();
            foreach (PropertyInfo property in _PropertyInfo)
            {
                object returnValue = null;
                try
                {
                    returnValue = property.GetValue(obj, null);
                }
                catch { }
                bool isCollection = false;

                // Determine if this is a collection, by searching for "Items" collection
                try
                {
                    Type t2 = returnValue.GetType();
                    PropertyInfo[] _pis = t2.GetProperties();
                    foreach (PropertyInfo _pi in _pis)
                    {
                        if (_pi.Name == "CustomAttributes") continue;
                        if (_pi.Name == "Items")
                        {
                            Type declaringType = _pi.DeclaringType;
                            isCollection = true;
                            break;
                        }
                    }
                }
                catch { }

                // Determine if this property is flagged as ReadOnly 
                object[] attributes = property.GetCustomAttributes(true);
                string description = string.Empty;
                bool readOnly = false;
                bool hidden = false;
                foreach (object attribute in attributes)
                {
                    // Hidden
                    var hiddenAttribute = attribute as Hidden;
                    if (hiddenAttribute != null && hiddenAttribute.Hide == true) hidden = true;

                    // Read-only
                    var readOnlyAttribute = attribute as ReadOnly;
                    if (readOnlyAttribute != null && readOnlyAttribute.Value) readOnly = true;

                    // Description
                    var descriptionAttribute = attribute as Description;
                    if (descriptionAttribute != null) description = descriptionAttribute.ToString();
                }

                if (returnValue == null)
                {
                    returnValue = string.Empty;
                }
                string propertyType = (property.PropertyType).FullName.ToString();

                // If this is a collection, and its child nodes have not yet
                // been populated, do so now
                if (isCollection && !node.propertiesHaveBeenFetched)
                {
                    //readOnly = readOnly && !(obj.GetType().ToString() == "MyKanban.Sprints");
                    readOnly = false;
                    PopulateCollection(node, property.Name, (IDataItem)obj, readOnly, propertyType, returnValue);
                }
            }
            node.propertiesHaveBeenFetched = true;
        }

        /// <summary>
        /// Here's where the edited rows have their edit flag cell set to red
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                propertiesGridView.Rows[e.RowIndex].Cells["EditFlag"].Style.BackColor = Color.Red;
            }
            catch { }
        }

        /// <summary>
        /// In some instances data may throw grid view data error, so added
        /// an empty handler so grid view doesn't display an ugly popup message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // No Op
        }

        /// <summary>
        /// Reload data for the selected MyKanban object
        /// </summary>
        /// <param name="obj"></param>
        private void Reload(object obj)
        {
            ((Base)obj).Reload();
        }

        /// <summary>
        /// Called when user clicks on "Reload" menu option at top of form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Called when user right-clicks on an object they wish to reload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)treeView1.SelectedNode;
            object obj = CurrentObject();

            if (obj != null)
            {
                selectedNode.Nodes.Clear();
                selectedNode.propertiesHaveBeenFetched = false;
                Reload(obj);
                PopulateNodes(obj, selectedNode);
            }
            else
            {
                MessageBox.Show("Only objects, not collections, may be reloaded.  Please selected an object to reload", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Use .NET reflection to display all properties for a given object
        /// </summary>
        /// <param name="obj">Object to display</param>
        private void ShowProperties(object obj, BoardSet boardSet)
        {
            collectionGridView.Visible = false;
            propertiesGridView.Visible = true;
            propertiesGridView.Rows.Clear();

            Type type = obj.GetType();

            PropertyInfo[] _PropertyInfo = type.GetProperties();

            foreach (PropertyInfo property in _PropertyInfo)
            {
                object[] attributes = property.GetCustomAttributes(true);

                string description = string.Empty;

                bool skipThisProperty = false;
                bool readOnly = false;
                enumControlType controlType = enumControlType.TextBox;
                foreach (object attribute in attributes)
                {
                    // Hidden
                    var hiddenAttribute = attribute as Hidden;
                    if (hiddenAttribute != null && hiddenAttribute.Hide == true) skipThisProperty = true;

                    // Description
                    var descriptionAttribute = attribute as Description;
                    if (descriptionAttribute != null)
                    {
                        description = descriptionAttribute.Text;
                    }

                    // Read-only
                    var readOnlyAttribute = attribute as ReadOnly;
                    if (readOnlyAttribute != null && readOnlyAttribute.Value) readOnly = true;
                    skipThisProperty = skipThisProperty || (readOnly && !cbShowReadOnlyProperties.Checked);

                    // Conrol type
                    var controlTypeAttribute = attribute as ControlType;
                    if (controlTypeAttribute != null) controlType = controlTypeAttribute.Type;
                }

                if (skipThisProperty) continue;

                object returnValue = null;

                try
                {
                    returnValue = property.GetValue(obj, null);
                }
                catch { }

                if (returnValue == null)
                {
                    returnValue = string.Empty;
                }

                string propertyType = (property.PropertyType).FullName.ToString();

                // Add a row
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewTextBoxCell t = new DataGridViewTextBoxCell();

                // EditFlag
                t.Selected = false;
                r.Cells.Add(t);

                // Property name
                t = new DataGridViewTextBoxCell();
                t.Value = property.Name;
                t.ToolTipText = description;
                r.Cells.Add(t);

                // Value
                if (propertyType == "System.DateTime" || propertyType == "System.Date")
                {
                    CalendarCell c = new CalendarCell();
                    DateTime d = DateTime.MinValue;
                    DateTime.TryParse(returnValue.ToString(), out d);
                    if (d < new DateTime(2000, 1, 1)) d = new DateTime(2000, 1, 1);
                    c.Value = d;
                    r.Cells.Add(c);
                }
                else
                {
                    switch (controlType)
                    {
                        case enumControlType.Numeric:
                            DataGridViewNumericUpDownCell n = new DataGridViewNumericUpDownCell();
                            n.Value = double.Parse(returnValue.ToString());
                            n.Minimum = 0;
                            n.Maximum = 1000;
                            n.DecimalPlaces = 2;
                            r.Cells.Add(n);
                            r.Height = Convert.ToInt16(r.Height * 1.25);
                            break;
                        case enumControlType.TextBox:
                            t = new DataGridViewTextBoxCell();
                            t.Value = returnValue.ToString();
                            r.Cells.Add(t);
                            break;
                        case enumControlType.StatusCode:
                            if (boardSet != null)
                            {
                                DataGridViewComboBoxCell cb = new DataGridViewComboBoxCell();
                                cb.DataSource = boardSet.StatusCodes.Items;
                                cb.DisplayMember = "ColumnHeading";
                                cb.ValueMember = "Id";
                                if ((long)returnValue > 0) cb.Value = (long)returnValue;
                                cb.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                cb.Style.BackColor = Color.Transparent;
                                r.Cells.Add(cb);
                            }
                            break;
                        case enumControlType.DateTime:
                            CalendarCell c = new CalendarCell();
                            DateTime d = DateTime.MinValue;
                            DateTime.TryParse(returnValue.ToString(), out d);
                            if (d < new DateTime(2000, 1, 1)) d = new DateTime(2000, 1, 1);
                            c.Value = d;
                            r.Cells.Add(c);
                            break;
                        case enumControlType.Boolean:
                            DataGridViewCheckBoxCell cbox = new DataGridViewCheckBoxCell();
                            cbox.Value = bool.Parse(returnValue.ToString());
                            r.Cells.Add(cbox);
                            break;
                        default:
                            break;
                    }

                }

                // Description
                t = new DataGridViewTextBoxCell();
                t.Value = description;
                r.Cells.Add(t);

                // Type
                t = new DataGridViewTextBoxCell();
                t.Value = (property.PropertyType).FullName.ToString();
                r.Cells.Add(t);

                propertiesGridView.Rows.Add(r);

                if (!property.CanWrite || readOnly)
                {
                    propertiesGridView.Rows[propertiesGridView.Rows.Count - 1].ReadOnly = true;
                    propertiesGridView.Rows[propertiesGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                }
            }

            propertiesGridView.Sort(propertiesGridView.Columns["Property"], ListSortDirection.Ascending);
            propertiesGridView.Rows[0].Cells["EditFlag"].Selected = false;
        }

        /// <summary>
        /// When user clicks on a node in the tree, check to see if child nodes need to be 
        /// added, and which buttons should be enabled for that particular node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            collectionGridView.Visible = false;
            propertiesGridView.Visible = false;

            MyKanbanTreeNode selectedNode = (MyKanbanTreeNode)e.Node;

            // If this is not a collection node, display the property sheet for this object
            if (!selectedNode.isCollection && selectedNode.MyKanbanObject != null)
            {
                if (!selectedNode.propertiesHaveBeenFetched)
                {
                    PopulateNodes(selectedNode.MyKanbanObject, selectedNode);
                }
                toolStripStatusLabel1.Text = selectedNode.MyKanbanObject.GetType().ToString();
                toolStripStatusLabel2.Text = selectedNode.MyKanbanObject.Id.ToString();
                toolStripStatusLabel3.Text = selectedNode.MyKanbanObject.Name;
                toolStripStatusLabel4.Text = "";
                currentObj = selectedNode.MyKanbanObject;

                propertiesGridView.Visible = true;
                ShowProperties(selectedNode.MyKanbanObject, GetBoardSet(selectedNode));
            }
            // If this is a collection node, display a list of child nodes
            else if (selectedNode.isCollection && selectedNode.MyKanbanCollectionObject != null)
            {
                if (selectedNode.MyKanbanParent != null)
                {
                    toolStripStatusLabel1.Text = selectedNode.MyKanbanParent.GetType().ToString();
                    toolStripStatusLabel2.Text = selectedNode.MyKanbanParent.Id.ToString();
                    toolStripStatusLabel3.Text = selectedNode.MyKanbanParent.Name;
                    toolStripStatusLabel4.Text = e.Node.Text.Split(' ')[0];
                }
                //collectionGridView.DataSource = selectedNode.MyKanbanCollection;
                collectionGridView.DataSource = selectedNode.GetFilteredList();
                collectionGridView.Visible = true;
            }

            // Enable/disable buttons and other controls
            cbShowReadOnlyProperties.Enabled = !selectedNode.isCollection;
            cmdJson.Enabled = !selectedNode.isCollection;
            cmdDelete.Enabled = !selectedNode.ReadOnly && !selectedNode.isCollection;
            cmdNew.Enabled = !selectedNode.ReadOnly && selectedNode.isCollection;
            cmdSave.Enabled = !selectedNode.ReadOnly && !selectedNode.isCollection;
            cmdRevert.Enabled = !selectedNode.ReadOnly && !selectedNode.isCollection;

            // Filter
            txtFilter.Enabled = selectedNode.isCollection;
            txtFilter.Text = selectedNode.Filter;
            cmdFilter.Enabled = selectedNode.isCollection;
        }

        #endregion

        /// <summary>
        /// This event handler purposely left blank.  Added to prevent missing data from
        /// causing gridview control from throwing exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void collectionGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // No Op
        }

        /// <summary>
        /// This event handler purposely left blank.  Added to prevent missing data from
        /// causing gridview control from throwing exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesGridView_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            // No Op
        }
    }
}