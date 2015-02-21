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
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class Description : Attribute
    {
        public Description(string text)
        {
            _text = text;
        }

        private string _text = "";

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class Hidden : Attribute
    {
        public Hidden(bool hidden)
        {
            _hide = hidden;
        }

        private bool _hide = false;

        public bool Hide
        {
            get { return _hide; }
            set { _hide = value; }
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ReadOnly : Attribute
    {
        public ReadOnly(bool readOnly)
        {
            _value = readOnly;
        }

        private bool _value = false;

        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    public enum enumControlType {TextBox, StatusCode, DateTime, Numeric, Boolean}

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ControlType : Attribute
    {
        public ControlType(enumControlType controlType)
        {
            _type = controlType;
        }

        private enumControlType _type = enumControlType.TextBox;

        public enumControlType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
