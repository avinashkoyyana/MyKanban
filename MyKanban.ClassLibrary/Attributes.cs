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
 *  Class:      Attributes
 *  
 *  Purpose:    Custom compiler attributes used to tag classes so
 *              .NET reflection can be used to read those tags for conditional
 *              processing.
 *              
 *  By:         Mark Gerow
---------------------------------------------------------------------------- */
namespace MyKanban
{
    /// <summary>
    /// A MyKanban description that can be associated with a property, used by
    /// .NET reflection to perform property-specific processing.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class Description : Attribute
    {
        /// <summary>
        /// Constructor to initialize attribute with text of property description.
        /// </summary>
        /// <param name="text">Text of description</param>
        public Description(string text)
        {
            _text = text;
        }

        private string _text = "";

        /// <summary>
        /// Text of property description.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }

    /// <summary>
    /// Indicates whether a property should be hidden in the MyKanban Object Browser
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class Hidden : Attribute
    {
        /// <summary>
        /// Initialize with boolean value indicating whether this property
        /// should appear in MyKanban Object Browser
        /// </summary>
        /// <param name="hidden">True = hide this property</param>
        public Hidden(bool hidden)
        {
            _hide = hidden;
        }

        private bool _hide = false;

        /// <summary>
        /// Value for this attribute.
        /// </summary>
        public bool Hide
        {
            get { return _hide; }
            set { _hide = value; }
        }
    }

    /// <summary>
    /// Indicates whether a property should be treated as read-only in the
    /// MyKanban Object Browser
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ReadOnly : Attribute
    {
        /// <summary>
        /// Initialize with boolean value indicating whether this property
        /// should be treated as read-only.  Note, if property has no get{} method,
        /// will be read-only regardless of this setting.
        /// </summary>
        /// <param name="readOnly">True = make this property read-only</param>
        public ReadOnly(bool readOnly)
        {
            _value = readOnly;
        }

        private bool _value = false;

        /// <summary>
        /// Value for this attribute
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    /// <summary>
    /// Allowable values for ControlType attribute.
    /// </summary>
    public enum enumControlType {TextBox, StatusCode, DateTime, Numeric, Boolean}

    /// <summary>
    /// Indicates type of control to use when displaying this property in the
    /// MyKanban Object Browser.  Default is "TextBox" if not specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ControlType : Attribute
    {
        /// <summary>
        /// Initialize the control type.
        /// </summary>
        /// <param name="controlType">Must be a valid enumControlType value.</param>
        public ControlType(enumControlType controlType)
        {
            _type = controlType;
        }

        private enumControlType _type = enumControlType.TextBox;

        /// <summary>
        /// enumControlType value associated with this attribute.
        /// </summary>
        public enumControlType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
