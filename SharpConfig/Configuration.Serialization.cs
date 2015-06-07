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
using System.IO;

namespace SharpConfig
{
    public partial class Configuration
    {
        private void Serialize(string filename, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                Serialize(stream, encoding);
                stream.Close();
            }
        }

        private void Serialize(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var sb = new StringBuilder();

            //写入所有节
            bool isFirstSection = true;

            foreach (var section in this)
            {
                // Leave some space between this section and the element that is above,
                // if this section has pre-comments and isn't the first section in the configuration.
                if (!isFirstSection &&
                    section.mPreComments != null && section.mPreComments.Count > 0)
                {
                    sb.AppendLine();
                }

                sb.AppendLine(section.ToString(true));

                // Write all settings.
                foreach (var setting in section)
                {
                    // Leave some space between this setting and the element that is above,
                    // if this element has pre-comments.
                    if (setting.mPreComments != null && setting.mPreComments.Count > 0)
                    {
                        sb.AppendLine();
                    }

                    sb.AppendLine(setting.ToString(true));
                }

                sb.AppendLine();

                isFirstSection = false;
            }

            // Replace triple new-lines with double new-lines.
            sb.Replace("\r\n\r\n\r\n", "\r\n\r\n");

            // Write to stream.
            var writer = encoding == null ?
                new StreamWriter(stream) : new StreamWriter(stream, encoding);

            using (writer)
            {
                writer.Write(sb.ToString());
                writer.Close();
            }
        }

        private void SerializeBinary(BinaryWriter writer, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                SerializeBinary(writer, stream);
            }
        }

        private void SerializeBinary(BinaryWriter writer, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            bool ownWriter = false;

            if (writer == null)
            {
                writer = new BinaryWriter(stream);
                ownWriter = true;
            }

            try
            {
                writer.Write(SectionCount);

                foreach (var section in this)
                {
                    writer.Write(section.Name);
                    writer.Write(section.SettingCount);

                    SerializeComments(writer, section);

                    // Write the section's settings.
                    foreach (var setting in section)
                    {
                        writer.Write(setting.Name);
                        writer.Write(setting.Value);

                        SerializeComments(writer, setting);
                    }
                }
            }
            finally
            {
                if (ownWriter)
                    writer.Close();
            }
        }

        private static void SerializeComments(BinaryWriter writer, ConfigurationElement element)
        {
            // Write the comment.
            var comment = element.Comment;

            writer.Write(comment != null);
            if (comment != null)
            {
                writer.Write(comment.Symbol);
                writer.Write(comment.Value);
            }

            // Write the pre-comments.
            var preComments = element.mPreComments;

            bool hasPreComments = preComments != null && preComments.Count > 0;

            writer.Write(hasPreComments ? preComments.Count : 0);

            if (hasPreComments)
            {
                foreach (var preComment in preComments)
                {
                    writer.Write(preComment.Symbol);
                    writer.Write(preComment.Value);
                }
            }
        }

    }
}