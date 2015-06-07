/*
 * Copyright (c) 2013-2015 Cemalettin Dervis
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
 * OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpConfig
{
    /// <summary>代表<see cref="Configuration"/>中的一个设置项目，Settings存储在<see cref="Section"/>中.</summary>
    public sealed class Setting : ConfigurationElement
    {
        #region Fields

        private string mRawValue;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting(string name)
            : this(name, string.Empty)
        {
            mRawValue = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        ///
        /// <param name="name"> The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public Setting(string name, string value)
            : base(name)
        {
            mRawValue = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the raw string value of this setting.
        /// </summary>
        public string Value
        {
            get { return mRawValue; }
            set { mRawValue = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this setting is an array.
        /// </summary>
        public bool IsArray
        {
            get { return ArraySize >= 0; }
        }

        /// <summary>
        /// Gets the size of the array that this setting represents.
        /// If this setting is not an array, -1 is returned.
        /// </summary>
        public int ArraySize
        {
            get
            {
                // First, check if we have a non-empty raw value,
                // and whether the value is declared as an array.
                if (string.IsNullOrEmpty(mRawValue) ||
                    mRawValue[0] != '{' ||
                    mRawValue[mRawValue.Length - 1] != '}')
                {
                    return -1;
                }

                if (mRawValue[mRawValue.Length - 2] == ',')
                    return -1;

                // Is this setting an empty array? (length = 0, e.g. {})
                if (mRawValue.Length == 2 &&
                    (mRawValue[0] == '{' && mRawValue[1] == '}'))
                {
                    return 0;
                }

                int oldCommaIndex = 0;
                int commaIndex = mRawValue.IndexOf(',');
                int size = 1;

                while (commaIndex >= 0)
                {
                    oldCommaIndex = commaIndex;
                    commaIndex = mRawValue.IndexOf(',', oldCommaIndex + 1);

                    if (commaIndex - oldCommaIndex == 1)
                        return -1;

                    size++;
                }

                return size;
            }
        }

        #endregion

        #region GetValue

        /// <summary>
        /// Gets this setting's value as a specific type.
        /// </summary>
        ///
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        /// <summary>
        /// Gets this setting's value as a specific type.
        /// </summary>
        ///
        /// <param name="type">The type.</param>
        public object GetValue(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsArray && !this.IsArray)
            {
                throw new InvalidOperationException(
                    "Trying to get the setting's array value, but the " +
                    "setting does not represent an array.");
            }

            if (!type.IsArray && this.IsArray)
            {
                throw new InvalidOperationException(
                    "Trying to get the setting's value as a single value, " +
                    "but the setting represents an array.");
            }

            if (type.IsArray)
            {
                // Parse as an array.

                Type elemType = type.GetElementType();

                var values = new object[this.ArraySize];
                int i = 0;

                int elemIndex = 1;
                int commaIndex = mRawValue.IndexOf(',');

                while (commaIndex >= 0)
                {
                    string sub = mRawValue.Substring(elemIndex, commaIndex - elemIndex);

                    values[i] = ConvertValue(sub, elemType);

                    elemIndex = commaIndex + 1;
                    commaIndex = mRawValue.IndexOf(',', elemIndex + 1);

                    i++;
                }

                // Read the last element.
                values[i] = ConvertValue(
                    mRawValue.Substring(elemIndex, mRawValue.Length - elemIndex - 1),
                    elemType);

                return values;
            }
            else
            {
                // Parse a single value.

                return ConvertValue(mRawValue, type);
            }
        }

        // Converts the value of a single element to a desired type.
        private static object ConvertValue(string value, Type type)
        {
            if (type == typeof(bool))
            {
                switch (value.ToLowerInvariant())
                {
                    case "off":
                    case "no":
                    case "0":
                        value = bool.FalseString;
                        break;
                    case "on":
                    case "yes":
                    case "1":
                        value = bool.TrueString;
                        break;
                }
            }
            else if (type.BaseType == typeof(Enum))
            {
                // It's possible that the value is something like:
                // UriFormat.Unescaped
                // We, and especially Enum.Parse do not want this format.
                // Instead, it wants the clean name like:
                // Unescaped
                //
                // Because of that, let's get rid of unwanted type names.
                int indexOfLastDot = value.LastIndexOf('.');

                if (indexOfLastDot >= 0)
                    value = value.Substring(indexOfLastDot + 1, value.Length - indexOfLastDot - 1).Trim();

                try
                {
                    return Enum.Parse(type, value);
                }
                catch
                {
                    throw new SettingValueCastException(value, type);
                }
            }

            try
            {
                return Convert.ChangeType(value, type, Configuration.NumberFormat);
            }
            catch
            {
                throw new SettingValueCastException(value, type);
            }
        }

        #endregion

        #region SetValue

        /// <summary>
        /// Sets the value of this setting via an object.
        /// </summary>
        /// 
        /// <param name="value">The value to set.</param>
        public void SetValue<T>(T value)
        {
            if (value == null)
                mRawValue = string.Empty;
            else
                mRawValue = value.ToString();
        }

        /// <summary>
        /// Sets the value of this setting via an array object.
        /// </summary>
        /// 
        /// <param name="values">The values to set.</param>
        public void SetValue<T>(T[] values)
        {
            if (values == null)
                mRawValue = string.Empty;
            else
            {
                var strings = new string[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    strings[i] = values[i].ToString();
                }

                mRawValue = string.Format("{{{0}}}", string.Join(",", strings));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a string that represents the setting. Comments not included.
        /// </summary>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Gets a string that represents the setting.
        /// </summary>
        ///
        /// <param name="includeComment">Specify true to include the comments in the string; false otherwise.</param>
        public string ToString(bool includeComment)
        {
            if (includeComment)
            {
                bool hasPreComments = mPreComments != null && mPreComments.Count > 0;

                string[] preCommentStrings = hasPreComments ?
                    mPreComments.ConvertAll<string>(Comment.ConvertToString).ToArray() : null;

                if (Comment != null && hasPreComments)
                {
                    // Include inline comment and pre-comments.
                    return string.Format("{0}\n{1}={2} {3}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Name, mRawValue, Comment.ToString());
                }
                else if (Comment != null)
                {
                    // Include only the inline comment.
                    return string.Format("{0}={1} {2}", Name, mRawValue, Comment.ToString());
                }
                else if (hasPreComments)
                {
                    // Include only the pre-comments.
                    return string.Format("{0}\n{1}={2}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Name, mRawValue);
                }
            }

            // In every other case, include just the assignment in the string.
            return string.Format("{0}={1}", Name, mRawValue);
        }

        #endregion
    }
}