/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using java = biz.ritter.javapi;
using org.apache.commons.compress.archivers;
using org.apache.commons.compress.utils;

namespace org.apache.commons.compress.archivers.tar {


    /**
     * The TarOutputStream writes a UNIX tar archive as an OutputStream.
     * Methods are provided to put entries, and then write their contents
     * by writing to this stream using write().
     * @NotThreadSafe
     */
    public class TarArchiveOutputStream : ArchiveOutputStream{//, TarConstants {
        /** Fail if a long file name is required in the archive. */
        public static readonly int LONGFILE_ERROR = 0;

        /** Long paths will be truncated in the archive. */
        public static readonly int LONGFILE_TRUNCATE = 1;

        /** GNU tar extensions are used to store long file names in the archive. */
        public static readonly int LONGFILE_GNU = 2;

        private long      currSize;
        private String    currName;
        private long      currBytes;
        private byte[]    recordBuf;
        private int       assemLen;
        private byte[]    assemBuf;
        protected TarBuffer buffer;
        private int       longFileMode = LONGFILE_ERROR;

        private bool closed = false;

        /** Indicates if putArchiveEntry has been called without closeArchiveEntry */
        private bool haveUnclosedEntry = false;
    
        /** indicates if this archive is finished */
        private bool finished = false;
    
        private java.io.OutputStream outJ;

        /**
         * Constructor for TarInputStream.
         * @param os the output stream to use
         */
        public TarArchiveOutputStream(java.io.OutputStream os) {
            init(os, TarBuffer.DEFAULT_BLKSIZE, TarBuffer.DEFAULT_RCDSIZE);
        }

        /**
         * Constructor for TarInputStream.
         * @param os the output stream to use
         * @param blockSize the block size to use
         */
        public TarArchiveOutputStream(java.io.OutputStream os, int blockSize) {
            init(os, blockSize, TarBuffer.DEFAULT_RCDSIZE);
        }

        /**
         * Constructor for TarInputStream.
         * @param os the output stream to use
         * @param blockSize the block size to use
         * @param recordSize the record size to use
         */
        public TarArchiveOutputStream(java.io.OutputStream os, int blockSize, int recordSize) {
            init (os,blockSize,recordSize);
        }
        /// <summary>
        /// Init the instance
        /// </summary>
        /// <param name="os">os the output stream to use</param>
        /// <param name="blockSize">blockSize the block size to use</param>
        /// <param name="recordSize">recordSize the record size to use</param>
        protected void init (java.io.OutputStream os, int blockSize, int recordSize) {
            outJ = os;

            this.buffer = new TarBuffer(os, blockSize, recordSize);
            this.assemLen = 0;
            this.assemBuf = new byte[recordSize];
            this.recordBuf = new byte[recordSize];
        }

        /**
         * Set the long file mode.
         * This can be LONGFILE_ERROR(0), LONGFILE_TRUNCATE(1) or LONGFILE_GNU(2).
         * This specifies the treatment of long file names (names >= TarConstants.NAMELEN).
         * Default is LONGFILE_ERROR.
         * @param longFileMode the mode to use
         */
        public void setLongFileMode(int longFileMode) {
            this.longFileMode = longFileMode;
        }


        /**
         * Ends the TAR archive without closing the underlying OutputStream.
         * 
         * An archive consists of a series of file entries terminated by an
         * end-of-archive entry, which consists of two 512 blocks of zero bytes. 
         * POSIX.1 requires two EOF records, like some other implementations.
         * 
         * @throws IOException on error
         */
        public override void finish() //throws IOException 
        {
            if (finished) {
                throw new java.io.IOException("This archive has already been finished");
            }
        
            if(haveUnclosedEntry) {
                throw new java.io.IOException("This archives contains unclosed entries.");
            }
            writeEOFRecord();
            writeEOFRecord();
            finished = true;
        }

        /**
         * Closes the underlying OutputStream.
         * @throws IOException on error
         */
        public override void close() //throws IOException 
        {
            if(!finished) {
                finish();
            }
        
            if (!closed) {
                buffer.close();
                outJ.close();
                closed = true;
            }
        }

        /**
         * Get the record size being used by this stream's TarBuffer.
         *
         * @return The TarBuffer record size.
         */
        public int getRecordSize() {
            return buffer.getRecordSize();
        }

        /**
         * Put an entry on the output stream. This writes the entry's
         * header record and positions the output stream for writing
         * the contents of the entry. Once this method is called, the
         * stream is ready for calls to write() to write the entry's
         * contents. Once the contents are written, closeArchiveEntry()
         * <B>MUST</B> be called to ensure that all buffered data
         * is completely written to the output stream.
         *
         * @param archiveEntry The TarEntry to be written to the archive.
         * @throws IOException on error
         * @throws ClassCastException if archiveEntry is not an instance of TarArchiveEntry
         */
        public override void putArchiveEntry(ArchiveEntry archiveEntry) //throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            TarArchiveEntry entry = (TarArchiveEntry) archiveEntry;
            if (entry.getName().length() >= TarConstants.NAMELEN) {

                if (longFileMode == LONGFILE_GNU) {
                    // create a TarEntry for the LongLink, the contents
                    // of which are the entry's name
                    TarArchiveEntry longLinkEntry = new TarArchiveEntry(TarConstants.GNU_LONGLINK,
                                                                        TarConstants.LF_GNUTYPE_LONGNAME);

                    byte[] nameBytes = ArchiveUtils.toAsciiBytes(entry.getName());
                    longLinkEntry.setSize(nameBytes.Length + 1); // +1 for NUL
                    putArchiveEntry(longLinkEntry);
                    write(nameBytes);
                    write(0); // NUL terminator
                    closeArchiveEntry();
                } else if (longFileMode != LONGFILE_TRUNCATE) {
                    throw new java.lang.RuntimeException("file name '" + entry.getName()
                                               + "' is too long ( > "
                                               + TarConstants.NAMELEN + " bytes)");
                }
            }

            entry.writeEntryHeader(recordBuf);
            buffer.writeRecord(recordBuf);

            currBytes = 0;

            if (entry.isDirectory()) {
                currSize = 0;
            } else {
                currSize = entry.getSize();
            }
            currName = entry.getName();
            haveUnclosedEntry = true;
        }

        /**
         * Close an entry. This method MUST be called for all file
         * entries that contain data. The reason is that we must
         * buffer data written to the stream in order to satisfy
         * the buffer's record based writes. Thus, there may be
         * data fragments still being assembled that must be written
         * to the output stream before this entry is closed and the
         * next entry written.
         * @throws IOException on error
         */
        public override void closeArchiveEntry() //throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            if (!haveUnclosedEntry){
                throw new java.io.IOException("No current entry to close");
            }
            if (assemLen > 0) {
                for (int i = assemLen; i < assemBuf.Length; ++i) {
                    assemBuf[i] = 0;
                }

                buffer.writeRecord(assemBuf);

                currBytes += assemLen;
                assemLen = 0;
            }

            if (currBytes < currSize) {
                throw new java.io.IOException("entry '" + currName + "' closed at '"
                                      + currBytes
                                      + "' before the '" + currSize
                                      + "' bytes specified in the header were written");
            }
            haveUnclosedEntry = false;
        }

        /**
         * Writes bytes to the current tar archive entry. This method
         * is aware of the current entry and will throw an exception if
         * you attempt to write bytes past the length specified for the
         * current entry. The method is also (painfully) aware of the
         * record buffering required by TarBuffer, and manages buffers
         * that are not a multiple of recordsize in length, including
         * assembling records from small buffers.
         *
         * @param wBuf The buffer to write to the archive.
         * @param wOffset The offset in the buffer from which to get bytes.
         * @param numToWrite The number of bytes to write.
         * @throws IOException on error
         */
        public override void write(byte[] wBuf, int wOffset, int numToWrite) //throws IOException 
        {
            if ((currBytes + numToWrite) > currSize) {
                throw new java.io.IOException("request to write '" + numToWrite
                                      + "' bytes exceeds size in header of '"
                                      + currSize + "' bytes for entry '"
                                      + currName + "'");

                //
                // We have to deal with assembly!!!
                // The programmer can be writing little 32 byte chunks for all
                // we know, and we must assemble complete records for writing.
                // REVIEW Maybe this should be in TarBuffer? Could that help to
                // eliminate some of the buffer copying.
                //
            }

            if (assemLen > 0) {
                if ((assemLen + numToWrite) >= recordBuf.Length) {
                    int aLen = recordBuf.Length - assemLen;

                    java.lang.SystemJ.arraycopy(assemBuf, 0, recordBuf, 0,
                                     assemLen);
                    java.lang.SystemJ.arraycopy(wBuf, wOffset, recordBuf,
                                     assemLen, aLen);
                    buffer.writeRecord(recordBuf);

                    currBytes += recordBuf.Length;
                    wOffset += aLen;
                    numToWrite -= aLen;
                    assemLen = 0;
                } else {
                    java.lang.SystemJ.arraycopy(wBuf, wOffset, assemBuf, assemLen,
                                     numToWrite);

                    wOffset += numToWrite;
                    assemLen += numToWrite;
                    numToWrite = 0;
                }
            }

            //
            // When we get here we have EITHER:
            // o An empty "assemble" buffer.
            // o No bytes to write (numToWrite == 0)
            //
            while (numToWrite > 0) {
                if (numToWrite < recordBuf.Length) {
                    java.lang.SystemJ.arraycopy(wBuf, wOffset, assemBuf, assemLen,
                                     numToWrite);

                    assemLen += numToWrite;

                    break;
                }

                buffer.writeRecord(wBuf, wOffset);

                int num = recordBuf.Length;

                currBytes += num;
                numToWrite -= num;
                wOffset += num;
            }
        
            count(numToWrite);
        }

        /**
         * Write an EOF (end of archive) record to the tar archive.
         * An EOF record consists of a record of all zeros.
         */
        private void writeEOFRecord() //throws IOException
        {
            for (int i = 0; i < recordBuf.Length; ++i) {
                recordBuf[i] = 0;
            }

            buffer.writeRecord(recordBuf);
        }

        // used to be implemented via FilterOutputStream
        public override void flush() //throws IOException 
        {
            outJ.flush();
        }

        /** {@inheritDoc} */
        public override ArchiveEntry createArchiveEntry(java.io.File inputFile, String entryName)
                //throws IOException 
                {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            return new TarArchiveEntry(inputFile, entryName);
        }
    }
}