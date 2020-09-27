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
     * A representation of a rpm 64 byte integer array data object.
     *
     * @author kuss
     * @version $Id: INT64.java,v 1.4 2005/11/11 08:27:40 mkuss Exp $
     */
    public sealed class INT64 : DataTypeIf
    {

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private const int DATA_WIDTH = 8;

        private long[] data;

        private long size;

        internal INT64(long[] data)
        {
            this.data = data;
            this.size = data.Length * DATA_WIDTH;
        }

        /**
         * Get the rpm int64 array as a java long array
         *
         * @return An array of longs
         */
        public long[] getData()
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
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getType()
         */
        public RPMIndexType getType()
        {
            return RPMIndexType.INT64;
        }

        /**
         * Constructs a type froma stream
         *
         * @param inputStream An input stream
         * @param indexEntry  The index informations
         * @return The size of the read data
         * @throws IOException if an I/O error occurs.
         */
        public static INT64 readFromStream(java.io.DataInputStream inputStream,
                                           IndexEntry indexEntry)
        {//throws IOException {
            if (indexEntry.getType() != RPMIndexType.INT64)
            {
                throw new java.lang.IllegalArgumentException("Type <" + indexEntry.getType()
                        + "> does not match <" + RPMIndexType.INT64 + ">");
            }

            long[] data = new long[(int)indexEntry.getCount()];

            for (int pos = 0; pos < indexEntry.getCount(); pos++)
            {
                data[pos] = inputStream.readLong();
            }

            INT64 int64Object = new INT64(data);

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer(int64Object.toString());
            }

            // int64Object.size = indexEntry.getType().getSize() *
            // indexEntry.getCount();

            return int64Object;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#isArray()
         */
        public bool isArray()
        {
            return true;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getElementCount()
         */
        public long getElementCount()
        {
            return this.data.Length;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getSize()
         */
        public long getSize()
        {
            return this.size;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#get(int)
         */
        public Object get(int i)
        {
            return new java.lang.Long(this.data[i]);
        }

        /*
         * @see java.lang.Object#toString()
         */
        public override String ToString()
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