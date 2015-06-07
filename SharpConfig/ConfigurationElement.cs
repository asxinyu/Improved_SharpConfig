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
    /// <summary>
    /// Represents the base class of all elements
    /// that exist in a <see cref="Configuration"/>,
    /// for example sections and settings.
    /// </summary>
    public abstract class ConfigurationElement
    {
        private string mName;
        private Comment mComment;
        internal List<Comment> mPreComments;

        internal ConfigurationElement(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            mName = name;
        }

        /// <summary>
        /// Gets or sets the name of this element.
        /// </summary>
        public string Name
        {
            get { return mName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                mName = value;
            }
        }

        /// <summary>
        /// Gets or sets the comment of this element.
        /// </summary>
        public Comment Comment
        {
            get { return mComment; }
            set { mComment = value; }
        }

        /// <summary>
        /// Gets the list of comments above this element.
        /// </summary>
        public List<Comment> PreComments
        {
            get
            {
                if (mPreComments == null)
                    mPreComments = new List<Comment>();

                return mPreComments;
            }
        }
    }
}