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
using com.jguild.jrpm.io.datatype;

namespace com.jguild.jrpm.io
{

    /**
     * This class represents the abstract definition of a header structur. It can be
     * either a signature or a header. The tags of such a structure can be accessed
     * by either their tag id or by their tag name. Also all available and all read
     * tag names in this structure can be accessed.
     * 
     * @author kuss
     * @version $Id: Header.java,v 1.10 2004/09/09 09:52:33 pnasrat Exp $
     */
    public abstract class Header
    {
        private const int HEADER_LENGTH = 16;

        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        private IndexEntry[] indexes;

        private int version;

        private long indexDataSize;

        private long indexNumber;

        private Store store;

        /**
             * The size in bytes of this structure.
             */
        protected long size;

        /**
             * Create a header structure from an input stream. <p/> The header
             * structure of a signature or a header can be read and also the index
             * entries containing the tags for this rpm section (signature or
             * header). <p/> Unless we have a raw header from headerUnload or the
             * database, a header is read consisting of the following fields:
             * <code><pre>
             * byte magic[3];      (3  byte)  (8e ad e8)
             * int version;        (1  byte)
             * byte reserved[4];   (4  byte)
             * long num_index;     (4  byte)
             * long num_data;      (4  byte)
             * </pre></code> <p/> Afterwards the index entries are read and then the tags
             * and the correspondig data entries are read.
             * 
             * @param inputStream
             *                An inputstream containing rpm file informations
             * @param rawHeader
             *                Are we a raw header (from headerUnload or rpmdb)
             * @throws IOException
             *                 if an error occurs on reading informations out of the
             *                 stream
             */
        public Header(java.io.DataInputStream inputStream, bool rawHeader, Store store)
        {//throws IOException {
            this.store = store;

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("Start Reading Header");
            }

            if (!rawHeader)
            {
                // Read header
                size = HEADER_LENGTH;

                int magic = 0;

                do
                {
                    magic = inputStream.readUnsignedByte();
                    if (magic == 0)
                        inputStream.skip(7);
                } while (magic == 0);

                check(magic == 0x8E, "Header magic 0x" + java.lang.Integer.toHexString(magic)
                    + " != 0x8E");
                magic = inputStream.readUnsignedByte();
                check(magic == 0xAD, "Header magic 0x" + java.lang.Integer.toHexString(magic)
                    + " != 0xAD");
                magic = inputStream.readUnsignedByte();
                check(magic == 0xE8, "Header magic 0x" + java.lang.Integer.toHexString(magic)
                    + " != 0xE8");
                version = inputStream.readUnsignedByte();

                if (logger.isLoggable(java.util.logging.Level.FINER))
                {
                    logger.finer("version: " + version);
                }

                // skip reserved bytes
                inputStream.skipBytes(4);
            }

            indexNumber = inputStream.readInt();

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("indexes available: " + indexNumber);
            }

            indexDataSize = inputStream.readInt();

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("index data size: " + indexDataSize);
            }

            // Read indexes
            // make sure to sort them in order of offset to
            // be able to read the store without jumping arround in
            // the file
            java.util.TreeSet<IndexEntry> _indexes = new java.util.TreeSet<IndexEntry>(new IAC_IndexesComparator());

            for (int i = 0; i < this.indexNumber; i++)
            {
                IndexEntry index = new IndexEntry(inputStream);

                _indexes.add(index);
                size += index.getSize();
            }

            indexes = new IndexEntry[0];
            indexes = (IndexEntry[])_indexes.toArray(indexes);

            // Read store
            for (int i = 0; i < indexes.Length; i++)
            {
                IndexEntry index = indexes[i];

                // if (index.getType().equals(RPMIndexType.STRING_ARRAY) ||
                // index.getType().equals(RPMIndexType.STRING) ||
                // index.getType().equals(RPMIndexType.I18NSTRING)) {
                // if (i < (indexes.length - 1)) {
                // IndexEntry next = indexes[i + 1];
                //
                // length = next.getOffset() - index.getOffset();
                // } else {
                // length = indexDataSize - index.getOffset();
                // }
                //
                // // and initialize temporary space for data
                // stringData = new byte[(int) length];
                //
                // // and read it from stream
                // inputStream.readFully(stringData);
                // }
                DataTypeIf dataObject = null;

                if (logger.isLoggable(java.util.logging.Level.FINER))
                {
                    logger.finer("Reading for tag '"
                        + store.getTagNameForId(index.getTag()) + "' '"
                        + index.getCount() + "' entries of type '"
                        + index.getType().getName() + "'");
                }

                dataObject = TypeFactory
                    .createFromStream(inputStream, index,
                        (i < (indexes.Length - 1)) ? (indexes[i + 1]
                            .getOffset() - index.getOffset())
                            : (indexDataSize - index.getOffset()));

                // adjust size
                size += dataObject.getSize();

                store.setTag(index.getTag(), dataObject);
            }

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("");
            }

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("Finished Reading Header");
            }
        }

        /**
             * Construct a header structure for the given input stream.
             * 
             * @param inputStream
             * @throws IOException
             */
        public Header(java.io.DataInputStream inputStream, Store store)
        :// throws IOException {
            this(inputStream, false, store){
        }

        /**
             * Get the size in bytes of this structure
             * 
             * @return The size in bytes.
             */
        public long getSize()
        {
            return size;
        }

        /**
             * Asserts a bool value and throws an exception if it is false
             * 
             * @param test
             *                A bool test variable
             * @throws IOException
             *                 if the variable test is false
             */
        private void check(bool test, String message)
        {//throws IOException {
            if (!test)
            {
                throw new java.io.IOException("Corrupted archive: " + message);
            }
        }

        class IAC_IndexesComparator : java.util.Comparator<IndexEntry>
        {
            public int compare(IndexEntry o1, IndexEntry o2)
            {
                return (int)(((IndexEntry)o1).getOffset() - ((IndexEntry)o2)
                    .getOffset());
            }

            public bool equals(Object o)
            {
                return false;
            }
        }
    }
}