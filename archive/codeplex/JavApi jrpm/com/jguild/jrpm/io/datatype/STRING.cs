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
     * A representation of a rpm string data object.
     *
     * @author kuss
     * @version $Id: STRING.java,v 1.4 2005/11/11 08:27:40 mkuss Exp $
     */
    public sealed class STRING : DataTypeIf
    {

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private String data;

        private int size;

        internal STRING(String data)
        {
            this.data = data;
            // add one to size for the \0 in C strings.
            this.size = data.length() + 1;
        }

        /**
         * Get the rpm string as a java string
         *
         * @return A string
         */
        public String getData()
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
            return RPMIndexType.STRING;
        }

        /**
         * Constructs a type froma stream
         *
         * @param inputStream An input stream
         * @param indexEntry  The index informations
         * @param length      the length of the data
         * @return The size of the read data
         * @throws IOException if an I/O error occurs.
         */
        public static STRING readFromStream(java.io.DataInputStream inputStream,
                                            IndexEntry indexEntry, long length)
        {//throws IOException {
            if (indexEntry.getType() != RPMIndexType.STRING)
            {
                throw new java.lang.IllegalArgumentException("Type <" + indexEntry.getType()
                        + "> does not match <" + RPMIndexType.STRING + ">");
            }

            if (indexEntry.getCount() != 1)
            {
                throw new java.lang.IllegalArgumentException(
                        "There can be only one string per tag of type STRING");
            }

            // initialize temporary space for data
            byte[] stringData = new byte[(int)length];

            // and read it from stream
            inputStream.readFully(stringData);

            String str = RPMUtil.cArrayToString(stringData, 0);
            STRING stringObject = new STRING(str);

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer(stringObject.toString());
                if (stringObject.size != stringData.Length)
                {
                    logger.warning("STRING size differs (is:" + stringData.Length
                            + ";should:" + stringObject.size + ")");
                }
            }

            stringObject.size = stringData.Length;

            return stringObject;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#isArray()
         */
        public bool isArray()
        {
            return false;
        }

        /*
         * @see com.jguild.jrpm.io.datatype.DataTypeIf#getElementCount()
         */
        public long getElementCount()
        {
            return 1;
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
            if (i != 0)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }

            return this.data;
        }

        /*
         * @see java.lang.Object#toString()
         */
        public override String ToString()
        {
            return this.data;
        }
    }
}