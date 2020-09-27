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

using com.jguild.jrpm.io.constant;
namespace com.jguild.jrpm.io
{

    /**
     * RPM Index Entry (16 byte).
     * <p/>
     * <ol>
     * <li>tag; what the data is for (4 bytes)</li>
     * <li>type; what type of data is stored (see RPMFileType.java) (4 bytes)
     * </li>
     * <li>offset; where to the data begins (4 bytes)</li>
     * <li>count; how much data of the type is there (4 bytes)</li>
     * </ol>
     *
     * @version $Id: IndexEntry.java,v 1.5 2004/05/06 20:59:22 mkuss Exp $
     */
    public class IndexEntry
    {

        private const int INDEX_LENGTH = 16;

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private RPMIndexType type;

        private long count;

        private long offset;

        private long tag;

        /**
         * Constructs a IndexEntry out of a InputStream. This will read the tag id,
         * the data type, the offset of the data and the number of elements of
         * data. Note that the number of elements varies in its meaning depending
         * on the data type.
         *
         * @param inputStream An InputStream containing a rpm file.
         * @throws IOException If an error occurs during reading from stream
         */
        public IndexEntry(java.io.DataInputStream inputStream)
        {//throws IOException {
            tag = inputStream.readInt();

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("tag:" + tag);
            }

            type = RPMIndexType.getRPMIndexType(inputStream.readInt());

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("type: " + type);
            }

            offset = inputStream.readInt();

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("offset: " + offset);
            }

            count = inputStream.readInt();

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("count: " + count);
            }

            check(!type.equals(RPMIndexType.UNKNOWN));
        }

        /**
         * Get the number of elements of this data type
         *
         * @return The number of elements
         */
        public long getCount()
        {
            return count;
        }

        /**
         * Returns the offset of this data type
         *
         * @return The offset
         */
        public long getOffset()
        {
            return offset;
        }

        /**
         * Get the size of this index entry in bytes
         *
         * @return The size
         */
        public int getSize()
        {
            return INDEX_LENGTH;
        }

        /**
         * Get the tag id as a long
         *
         * @return The tag id
         */
        public long getTag()
        {
            return tag;
        }

        /**
         * Get the data type of this entry
         *
         * @return The data type
         */
        public RPMIndexType getType()
        {
            return type;
        }

        /**
         * Asserts a bool value and throws an exception if it is false
         *
         * @param test A bool test variable
         * @throws IOException if the variable test is false
         */
        private static void check(bool test)
        {// throws IOException {
            if (!test)
            {
                throw new java.io.IOException("Corrupted archive");
            }
        }
    }
}