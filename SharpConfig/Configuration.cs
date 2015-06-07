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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace SharpConfig
{
    /// <summary>
    /// 代表一个配置文件：1个Configurations包含1个或者多个Section,
    /// 每一个Section包括一个或者多个Setting.配置类<see cref="Configuration"/> 可以处理多种文件格式，
    /// 如 .ini , .cfg以及其他配置格式.
    /// </summary>
    public partial class Configuration : IEnumerable<Section>
    {
        #region 字段
        /// <summary>数值格式</summary>
        private static NumberFormatInfo mNumberFormat;
        /// <summary>合法注释标识符号</summary>
        private static char[] mValidCommentChars;

        private static bool mIgnoreInlineComments;
        private static bool mIgnorePreComments;
        //Section集合，依次解析的时候添加进来
        private List<Section> mSections;
        #endregion

        #region 构造函数
        //静态构造函数，设置这些默认值，因此可以修改
        static Configuration()
        {
            mNumberFormat = CultureInfo.InvariantCulture.NumberFormat;
            mValidCommentChars = new[] { '#', ';', '\'' };
            mIgnoreInlineComments = false;
            mIgnorePreComments = false;
        }

        /// <summary>初始化一个<see cref="Configuration"/> 类的实例对象</summary>
        public Configuration()
        {
            mSections = new List<Section>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an enumerator that iterates through the configuration.
        /// </summary>
        public IEnumerator<Section> GetEnumerator()
        {
            return mSections.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the configuration.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a section to the configuration.
        /// </summary>
        /// <param name="section">The section to add.</param>
        public void Add(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            if (Contains(section))
            {
                throw new ArgumentException(
                    "The specified section already exists in the configuration.");
            }

            mSections.Add(section);
        }

        /// <summary>
        /// Clears the configuration of all sections.
        /// </summary>
        public void Clear()
        {
            mSections.Clear();
        }

        /// <summary>
        /// Determines whether a specified section is contained in the configuration.
        /// </summary>
        /// <param name="section">The section to check for containment.</param>
        /// <returns>True if the section is contained in the configuration; false otherwise.</returns>
        public bool Contains(Section section)
        {
            return mSections.Contains(section);
        }

        /// <summary>
        /// Determines whether a specifically named setting is contained in the section.
        /// </summary>
        /// <param name="sectionName">The name of the section.</param>
        /// <returns>True if the setting is contained in the section; false otherwise.</returns>
        public bool Contains(string sectionName)
        {
            return GetSection(sectionName) != null;
        }

        /// <summary>
        /// Removes a section from this section by its name.
        /// </summary>
        /// <param name="sectionName">The case-sensitive name of the section to remove.</param>
        public void Remove(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException("sectionName");

            var section = GetSection(sectionName);

            if (section == null)
            {
                throw new ArgumentException(
                    "The specified section does not exist in the section.");
            }

            Remove(section);
        }

        /// <summary>
        /// Removes a section from the configuration.
        /// </summary>
        /// <param name="section">The section to remove.</param>
        public void Remove(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            if (!Contains(section))
            {
                throw new ArgumentException(
                    "The specified section does not exist in the section.");
            }

            mSections.Remove(section);
        }

        #endregion

        #region 加载配置文件
        /// <summary>从配置文件直接加载，自动检测编码类型，以及使用默认的设置</summary>
        /// <param name="filename">本地配置文件名称</param>
        /// <returns>
        /// The loaded <see cref="Configuration"/> object.
        /// </returns>
        public static Configuration LoadFromFile(string filename)
        {
            return LoadFromFile(filename, null);
        }

        /// <summary>从配置文件直接加载</summary>
        /// <param name="filename">本地配置文件名称</param>
        /// <param name="encoding">文件的编码类型，如果为Null,则自动检测</param>
        /// <returns>
        /// The loaded <see cref="Configuration"/> object.
        /// </returns>
        public static Configuration LoadFromFile(string filename, Encoding encoding)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("Configuration file not found.", filename);

            Configuration cfg = null;

            if (encoding == null)
                cfg = LoadFromText(File.ReadAllText(filename));
            else
                cfg = LoadFromText(File.ReadAllText(filename, encoding));

            return cfg;
        }

        /// <summary>
        /// Loads a configuration from a text stream auto-detecting the encoding and
        /// using the default parsing settings.
        /// </summary>
        ///
        /// <param name="stream">The text stream to load the configuration from.</param>
        ///
        /// <returns>
        /// The loaded <see cref="Configuration"/> object.
        /// </returns>
        public static Configuration LoadFromStream(Stream stream)
        {
            return LoadFromStream(stream, null);
        }

        /// <summary>
        /// Loads a configuration from a text stream.
        /// </summary>
        ///
        /// <param name="stream">   The text stream to load the configuration from.</param>
        /// <param name="encoding"> The encoding applied to the contents of the stream. Specify null to auto-detect the encoding.</param>
        ///
        /// <returns>
        /// The loaded <see cref="Configuration"/> object.
        /// </returns>
        public static Configuration LoadFromStream(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            string source = null;

            var reader = encoding == null ?
                new StreamReader(stream) : new StreamReader(stream, encoding);

            using (reader)
            {
                source = reader.ReadToEnd();
                reader.Close();
            }

            return LoadFromText(source);
        }

        /// <summary>
        /// Loads a configuration from text (source code).
        /// </summary>
        ///
        /// <param name="source">   The text (source code) of the configuration.</param>
        ///
        /// <returns>
        /// The loaded <see cref="Configuration"/> object.
        /// </returns>
        public static Configuration LoadFromText(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException("source");

            return Parse(source);
        }

        #endregion

        #region LoadBinary

        /// <summary>
        /// Loads a configuration from a binary file using the <b>default</b> <see cref="BinaryReader"/>.
        /// </summary>
        ///
        /// <param name="filename">The location of the configuration file.</param>
        ///
        /// <returns>
        /// The loaded configuration.
        /// </returns>
        public static Configuration LoadBinary(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            return DeserializeBinary(null, filename);
        }

        /// <summary>
        /// Loads a configuration from a binary file using a specific <see cref="BinaryReader"/>.
        /// </summary>
        ///
        /// <param name="filename">The location of the configuration file.</param>
        /// <param name="reader">  The reader to use. Specify null to use the default <see cref="BinaryReader"/>.</param>
        ///
        /// <returns>
        /// The loaded configuration.
        /// </returns>
        public static Configuration LoadBinary(string filename, BinaryReader reader)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            return DeserializeBinary(reader, filename);
        }

        /// <summary>
        /// Loads a configuration from a binary stream, using the <b>default</b> <see cref="BinaryReader"/>.
        /// </summary>
        ///
        /// <param name="stream">The stream to load the configuration from.</param>
        ///
        /// <returns>
        /// The loaded configuration.
        /// </returns>
        public static Configuration LoadBinary(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return DeserializeBinary(null, stream);
        }

        /// <summary>
        /// Loads a configuration from a binary stream, using a specific <see cref="BinaryReader"/>.
        /// </summary>
        ///
        /// <param name="stream">The stream to load the configuration from.</param>
        /// <param name="reader">The reader to use. Specify null to use the default <see cref="BinaryReader"/>.</param>
        ///
        /// <returns>
        /// The loaded configuration.
        /// </returns>
        public static Configuration LoadBinary(Stream stream, BinaryReader reader)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return DeserializeBinary(reader, stream);
        }

        #endregion

        #region Save
        /// <summary>保存配置文件到指定文件，使用默认的 UTF8 编码</summary>
        /// <param name="filename">要保存的配置文件的名称</param>
        public void Save(string filename)
        {
            Save(filename, null);
        }

        /// <summary>保存配置文件到指定文件</summary>
        /// <param name="filename">配置文件名称</param>
        /// <param name="encoding">编码，如果为null，默认使用UTF8</param>
        public void Save(string filename, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            Serialize(filename, encoding);
        }

        /// <summary>保存配置信息到文件流中，使用默认的UTF8编码</summary>
        /// <param name="stream">保存配置信息的 流</param>
        public void Save(Stream stream)
        {
            Save(stream, null);
        }

        /// <summary>保存配置信息到文件流中</summary>
        /// <param name="stream">The stream to save the configuration to.</param>
        /// <param name="encoding">The character encoding to use. Specify null to use the default encoding, which is UTF8.</param>
        public void Save(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            Serialize(stream, encoding);
        }

        #endregion

        #region SaveBinary

        /// <summary>
        /// Saves the configuration to a binary file, using the default <see cref="BinaryWriter"/>.
        /// </summary>
        ///
        /// <param name="filename">The location of the configuration file.</param>
        public void SaveBinary(string filename)
        {
            SaveBinary(filename, null);
        }

        /// <summary>
        /// Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
        /// </summary>
        ///
        /// <param name="filename">The location of the configuration file.</param>
        /// <param name="writer">  The writer to use. Specify null to use the default writer.</param>
        public void SaveBinary(string filename, BinaryWriter writer)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            SerializeBinary(writer, filename);
        }

        /// <summary>
        /// Saves the configuration to a binary stream, using the default <see cref="BinaryWriter"/>.
        /// </summary>
        ///
        /// <param name="stream">The stream to save the configuration to.</param>
        public void SaveBinary(Stream stream)
        {
            SaveBinary(stream, null);
        }

        /// <summary>
        /// Saves the configuration to a binary file, using a specific <see cref="BinaryWriter"/>.
        /// </summary>
        ///
        /// <param name="stream">The stream to save the configuration to.</param>
        /// <param name="writer">The writer to use. Specify null to use the default writer.</param>
        public void SaveBinary(Stream stream, BinaryWriter writer)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            SerializeBinary(writer, stream);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the number format that is used for value conversion in Section.GetValue().
        /// The default value is <b>CultureInfo.InvariantCulture.NumberFormat</b>.
        /// </summary>
        public static NumberFormatInfo NumberFormat
        {
            get { return mNumberFormat; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                mNumberFormat = value;
            }
        }

        /// <summary>获取或者设置 注释标识字符</summary>
        public static char[] ValidCommentChars
        {
            get { return mValidCommentChars; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value.Length == 0)
                {
                    throw new ArgumentException("The comment chars array must not be empty.","value");
                }
                mValidCommentChars = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inline-comments
        /// should be ignored when parsing a configuration.
        /// </summary>
        public static bool IgnoreInlineComments
        {
            get { return mIgnoreInlineComments; }
            set { mIgnoreInlineComments = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pre-comments
        /// should be ignored when parsing a configuration.
        /// </summary>
        public static bool IgnorePreComments
        {
            get { return mIgnorePreComments; }
            set { mIgnorePreComments = value; }
        }

        /// <summary>
        /// Gets the number of sections that are in the configuration.
        /// </summary>
        public int SectionCount
        {
            get { return mSections.Count; }
        }

        /// <summary>根据索引获取Section</summary>
        /// <param name="index">要获取的section的位置</param>
        public Section this[int index]
        {
            get
            {
                if (index < 0 || index >= mSections.Count)
                    throw new ArgumentOutOfRangeException("index");

                return mSections[index];
            }
            set
            {
                if (index < 0 || index >= mSections.Count)
                    throw new ArgumentOutOfRangeException("index");

                mSections[index] = value;
            }
        }

        /// <summary>根据名称获取Section</summary>
        /// <param name="name">Section的名称</param>
        /// <returns>如果存在就返回，否则就会新建1个指定名称的Section，并默认加到配置文件中去</returns>
        public Section this[string name]
        {
            get
            {
                var section = GetSection(name);

                if (section == null)
                {
                    section = new Section(name);
                    Add(section);
                }

                return section;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                // Check if there already is a section by that name.
                var section = GetSection(name);

                int settingIndex = section != null ? mSections.IndexOf(section) : -1;

                if (settingIndex < 0)
                {
                    // A section with that name does not exist yet; add it.
                    mSections.Add(section);
                }
                else
                {
                    // A section with that name exists; overwrite.
                    mSections[settingIndex] = section;
                }
            }
        }

        private Section GetSection(string name)
        {
            foreach (var section in mSections)
            {
                if (string.Equals(section.Name, name, StringComparison.OrdinalIgnoreCase))
                    return section;
            }

            return null;
        }

        #endregion
    }
}