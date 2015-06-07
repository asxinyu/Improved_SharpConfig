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
    /// <summary>代表配置文件中的一次注释，或者叫做评注</summary>
    public sealed class Comment
    {
        private string mValue;
        private char mSymbol;

        /// <summary>
        /// 初始化一个<see cref="Comment"/> 对象
        /// </summary>
        /// <param name="value"> 注释值</param>
        /// <param name="symbol">先顶注释的标记符</param>
        public Comment(string value, char symbol)
        {
            if (symbol == '\0') throw new ArgumentNullException("symbol");

            mValue = value;
            mSymbol = symbol;
        }

        /// <summary>
        /// Gets or sets the value of the comment.
        /// </summary>
        public string Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        /// <summary>
        /// Gets or sets the delimiting symbol of the comment.
        /// </summary>
        public char Symbol
        {
            get { return mSymbol; }
            set
            {
                if (value == '\0')
                    throw new ArgumentNullException("value");

                mSymbol = value;
            }
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        ///
        /// <returns>
        /// A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Symbol, Value ?? string.Empty);
        }

        // Used by Setting and Section in ToString().
        internal static string ConvertToString(Comment comment)
        {
            return comment.ToString();
        }
    }
}