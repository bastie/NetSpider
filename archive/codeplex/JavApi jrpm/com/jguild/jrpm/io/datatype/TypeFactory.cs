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
using System;
using java = biz.ritter.javapi;
using com.jguild.jrpm.io;
using com.jguild.jrpm.io.constant;

namespace com.jguild.jrpm.io.datatype
{

    /**
     * Factory to create rpm data types.
     * 
     * @version $Id: TypeFactory.java,v 1.4 2005/11/11 08:27:40 mkuss Exp $
     * @author kuss
     */
    public sealed class TypeFactory
    {
        /**
         * hide constructor
         */
        private TypeFactory()
        {
            // do nothing
        }

        /**
         * Create a data object of type BIN
         * 
         * @param data
         *            The raw data
         * @return The BIN object
         */
        public static BIN createBIN(byte[] data)
        {
            return new BIN(data);
        }

        /**
         * Create a data object of type CHAR
         * 
         * @param data
         *            The raw data
         * @return The CHAR object
         */
        public static CHAR createCHAR(char[] data)
        {
            return new CHAR(data);
        }

        /**
         * Create a data object of type I18NSTRING
         * 
         * @param str
         *            An array of language strings
         * @return The I18NSTRING object
         */
        public static I18NSTRING createI18NSTRING(String[] str)
        {
            return new I18NSTRING(str);
        }

        /**
         * Create a data object of type INT8
         * 
         * @param data
         *            The raw data
         * @return The INT8 object
         */
        public static INT8 createINT8(byte[] data)
        {
            return new INT8(data);
        }

        /**
         * Create a data object of type INT16
         * 
         * @param data
         *            The raw data
         * @return The INT16 object
         */
        public static INT16 createINT16(short[] data)
        {
            return new INT16(data);
        }

        /**
         * Create a data object of type INT32
         * 
         * @param data
         *            The raw data
         * @return The INT32 object
         */
        public static INT32 createINT32(int[] data)
        {
            return new INT32(data);
        }

        /**
         * Create a data object of type INT64
         * 
         * @param data
         *            The raw data
         * @return The INT64 object
         */
        public static INT64 createINT64(long[] data)
        {
            return new INT64(data);
        }

        /**
         * Create a data object of type NULL
         * 
         * @param size
         *            The length of the entry
         * @return The NULL object
         */
        public static NULL createNULL(int size)
        {
            return new NULL(size);
        }

        /**
         * Create a data object of type STRING
         * 
         * @param str
         *            A raw string
         * @return The STRING object
         */
        public static STRING createSTRING(String str)
        {
            return new STRING(str);
        }

        /**
         * Create a data object of type STRING_ARRAY
         * 
         * @param str
         *            Raw strings
         * @return The STRING_ARRAY object
         */
        public static STRING_ARRAY createSTRING_ARRAY(String[] str)
        {
            return new STRING_ARRAY(str);
        }

        /**
         * This method creates a rpm data type out of a input stream and an
         * IndexEntry. The object must at the current position of the input stream.
         * The length is only needed for string objects; the string objects will
         * read length bytes of the input stream and will try to convert the data
         * into a rpm data type.
         * 
         * @param inputStream
         *            The input stream
         * @param indexEntry
         *            The IndexEntry that should be read
         * @param length
         *            The number of bytes to read for string objects
         * 
         * @return One of the rpm data types coresponding with the type contained in
         *         the IndexEntry.
         * 
         * @throws IOException
         *             if something was wrong during reading of the input stream
         */
        public static DataTypeIf createFromStream(
                java.io.DataInputStream inputStream, IndexEntry indexEntry,
                long length)
        {//throws IOException {
            DataTypeIf ret = null;

            switch ((int)indexEntry.getType().getId())
            {
                case RPMIndexType._NULL:
                    ret = NULL.readFromStream(indexEntry);
                    break;

                case RPMIndexType._CHAR:
                    ret = CHAR.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._INT8:
                    ret = INT8.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._INT16:
                    ret = INT16.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._INT32:
                    ret = INT32.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._INT64:
                    ret = INT64.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._STRING:
                    ret = STRING.readFromStream(inputStream, indexEntry, length);
                    break;

                case RPMIndexType._BIN:
                    ret = BIN.readFromStream(inputStream, indexEntry);
                    break;

                case RPMIndexType._STRING_ARRAY:
                    ret = STRING_ARRAY.readFromStream(inputStream, indexEntry, length);
                    break;

                case RPMIndexType._I18NSTRING:
                    ret = I18NSTRING.readFromStream(inputStream, indexEntry, length);
                    break;

                default:
                // TODO: UNKNOWN
                    break;
            }

            return ret;
        }
    }
}