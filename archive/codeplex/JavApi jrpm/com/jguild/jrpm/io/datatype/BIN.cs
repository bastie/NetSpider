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
     * A representation of a rpm binary array data object.
     *
     * @author kuss
     * @version $Id: BIN.java,v 1.4 2005/11/11 08:27:40 mkuss Exp $
     */
    public sealed class BIN : DataTypeIf
    {

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private byte[] data;

        private long size;

        internal BIN(byte[] data)
        {
            this.data = data;
            this.size = data.Length;
        }

        /**
         * Get the rpm byte array as a java byte array
         *
         * @return An array of bytes
         */
        public byte[] getData()
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
            return RPMIndexType.BIN;
        }

        /**
         * Constructs a type froma stream
         *
         * @param inputStream An input stream
         * @param indexEntry  The index informations
         * @return The size of the read data
         * @throws IOException if an I/O error occurs.
         */
        public static BIN readFromStream(java.io.DataInputStream inputStream,
                                         IndexEntry indexEntry)
        {//throws IOException {
            if (indexEntry.getType() != RPMIndexType.BIN)
            {
                throw new java.lang.IllegalArgumentException("Type <" + indexEntry.getType()
                        + "> does not match <" + RPMIndexType.BIN + ">");
            }

            byte[] data = new byte[(int)indexEntry.getCount()];

            for (int pos = 0; pos < indexEntry.getCount(); pos++)
            {
                data[pos] = inputStream.readByte();
            }

            BIN binObject = new BIN(data);

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer(binObject.toString());
            }

            // binObject.size = indexEntry.getType().getSize() *
            // indexEntry.getCount();

            return binObject;
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
            return new java.lang.Byte(this.data[i]);
        }

        /*
         * @see java.lang.Object#toString()
         */
        public override String ToString()
        {
            return RPMUtil.byteArrayToHexString(this.data);
        }
    }
}