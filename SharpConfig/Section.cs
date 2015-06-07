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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SharpConfig
{
    /// <summary>
    /// 节代表一组 <see cref="Setting"/> 对象.
    /// </summary>
    public sealed class Section : ConfigurationElement, IEnumerable<Setting>
    {
        private List<Setting> mSettings;

        /// <summary>初始化 <see cref="Section"/> 的实例</summary>
        /// <param name="name">节的名称</param>
        public Section(string name): base(name)
        {
            mSettings = new List<Setting>();
        }

        /// <summary>
        /// Creates an object of a specific type, and maps the settings
        /// in this section to the public properties of the object.
        /// </summary>
        /// 
        /// <returns>The created object.</returns>
        /// 
        /// <remarks>
        /// The specified type must have a public default constructor
        /// in order to be created.
        /// </remarks>
        public T CreateObject<T>() where T : class
        {
            Type type = typeof(T);

            try
            {
                T obj = Activator.CreateInstance<T>();

                MapTo(obj);

                return obj;
            }
            catch (Exception)
            {
                throw new ArgumentException(string.Format(
                    "The type '{0}' does not have a default public constructor.",
                    type.Name));
            }
        }

        /// <summary>
        /// Assigns the values of this section to an object's public properties.
        /// </summary>
        /// 
        /// <param name="obj">The object that is modified based on the section.</param>
        public void MapTo<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                if (!prop.CanWrite)
                    continue;

                var setting = GetSetting(prop.Name);

                if (setting != null)
                {
                    object value = setting.GetValue(prop.PropertyType);

                    prop.SetValue(obj, value, null);
                }
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        public IEnumerator<Setting> GetEnumerator()
        {
            return mSettings.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a setting to the section.
        /// </summary>
        /// <param name="setting">The setting to add.</param>
        public void Add(Setting setting)
        {
            if (setting == null) throw new ArgumentNullException("setting");

            if (Contains(setting))
            {
                throw new ArgumentException("The specified setting already exists in the section.");
            }
            mSettings.Add(setting);
        }

        /// <summary>清除节中所有的设置项</summary>
        public void Clear()
        {
            mSettings.Clear();
        }

        /// <summary>检测节中是否存在某个特定的设置</summary>
        /// <param name="setting">The setting to check for containment.</param>
        /// <returns>True if the setting is contained in the section; false otherwise.</returns>
        public bool Contains(Setting setting)
        {
            return mSettings.Contains(setting);
        }

        /// <summary>检测节中是否存在某个特定名称的设置 </summary>
        /// <param name="settingName">设置项的名称</param>
        /// <returns>True if the setting is contained in the section; false otherwise.</returns>
        public bool Contains(string settingName)
        {
            return GetSetting(settingName) != null;
        }

        /// <summary>从本节中移除某个名称的设置</summary>
        public void Remove(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            var setting = GetSetting(settingName);

            if (setting == null)
            {
                throw new ArgumentException("The specified setting does not exist in the section.");
            }
            mSettings.Remove(setting);
        }

        /// <summary>从本节中移除某个设置</summary>
        /// <param name="setting">要移除的设置对象</param>
        public void Remove(Setting setting)
        {
            if (setting == null) throw new ArgumentNullException("setting");

            if (!Contains(setting))
            {
                throw new ArgumentException("The specified setting does not exist in the section.");
            }
            mSettings.Remove(setting);
        }

        /// <summary>获取节中设置项目的总数量</summary>
        public int SettingCount { get { return mSettings.Count; } }

        /// <summary>索引获取设置项</summary>
        /// <param name="index">设置项在节中的索引</param>
        public Setting this[int index]
        {
            get
            {
                if (index < 0 || index >= mSettings.Count) throw new ArgumentOutOfRangeException("index");

                return mSettings[index];
            }
            set
            {
                if (index < 0 || index >= mSettings.Count) throw new ArgumentOutOfRangeException("index");
                mSettings[index] = value;
            }
        }

        /// <summary>通过名称获取设置</summary>
        /// <param name="name">设置项的名称</param>
        /// <returns>如果存在该设置，返回，否则将会创建一个指定名称的设置，添加到节中并返回</returns>
        public Setting this[string name]
        {
            get
            {
                var setting = GetSetting(name);

                if (setting == null)
                {
                    setting = new Setting(name);
                    Add(setting);
                }

                return setting;
            }
            set
            {
                // Check if there already is a setting by that name.
                var setting = GetSetting(name);

                int settingIndex = setting != null ? mSettings.IndexOf(setting) : -1;

                if (settingIndex < 0)
                {
                    // A setting with that name does not exist yet; add it.
                    mSettings.Add(setting);
                }
                else
                {
                    // A setting with that name exists; overwrite.
                    mSettings[settingIndex] = setting;
                }
            }
        }

        private Setting GetSetting(string name)
        {
            foreach (var setting in mSettings)
            {
                if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
                    return setting;
            }

            return null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        ///
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Convert this object into a string representation.
        /// </summary>
        ///
        /// <param name="includeComment">True to include, false to exclude the comment.</param>
        ///
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
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
                    return string.Format("{0}\n[{1}] {2}",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Name, Comment.ToString());
                }
                else if (Comment != null)
                {
                    // Include only the inline comment.
                    return string.Format("[{0}] {1}", Name, Comment.ToString());
                }
                else if (hasPreComments)
                {
                    // Include only the pre-comments.
                    return string.Format("{0}\n[{1}]",
                        string.Join(Environment.NewLine, preCommentStrings),
                        Name);
                }
            }

            return string.Format("[{0}]", Name);
        }
    }
}