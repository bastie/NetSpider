/*
 *  Copyright 2003 jRPM Team
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System; using java = biz.ritter.javapi;

using com.jguild.jrpm.io;
using com.jguild.jrpm.io.constant;

namespace com.jguild.jrpm.io.datatype
{
    /**
     * A representation of a rpm null array data object.
     *
     * @author kuss
     * @version $Id: NULL.java,v 1.4 2005/11/11 08:27:40 mkuss Exp $
     */
    public sealed class NULL : DataTypeIf
    {

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private Object[] data;

        internal NULL(int size)
        {
            this.data = new Object[size];
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#isArray()
         */
        public bool isArray()
        {
            return true;
        }

        /**
         * Get the rpm NULL array as a java object array of null objects
         *
         * @return An array of null objects
         */
        public Object[] getData()
        {
            return this.data;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getData()
         */
        public Object getDataObject()
        {
            return this.data;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getSize()
         */
        public long getSize()
        {
            return 0;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getType()
         */
        public RPMIndexType getType()
        {
            return RPMIndexType.NULL;
        }

        /**
         * Constructs a type froma stream
         *
         * @param indexEntry The index informations
         * @return The size of the read data
         */
        public static NULL readFromStream(IndexEntry indexEntry)
        {
            if (indexEntry.getType() != RPMIndexType.NULL)
            {
                throw new java.lang.IllegalArgumentException("Type <" + indexEntry.getType()
                        + "> does not match <" + RPMIndexType.NULL + ">");
            }

            NULL nullObject = new NULL((int)indexEntry.getCount());

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer(nullObject.toString());
            }

            // nullObject.size = indexEntry.getType().getSize() *
            // indexEntry.getCount();

            return nullObject;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getElementCount()
         */
        public long getElementCount()
        {
            return this.data.Length;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#get(int)
         */
        public Object get(int i)
        {
            return this.data[i];
        }

        /*
         * @see java.lang.Object#toString()
         */
        public String toString()
        {
            java.lang.StringBuffer buf = new java.lang.StringBuffer();

            if (this.data.Length > 1)
            {
                buf.append("[");
            }

            for (int pos = 0; pos < this.data.Length; pos++)
            {
                buf.append(this.data[pos]);

                if (pos < (this.data.Length - 1))
                {
                    buf.append(", ");
                }
            }

            if (this.data.Length > 1)
            {
                buf.append("]");
            }

            return buf.toString();
        }
    }
}