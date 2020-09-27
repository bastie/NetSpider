/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

using org.apache.commons.compress.archivers;
using org.apache.commons.compress.utils;


/*
 * This package is based on the work done by Timothy Gerard Endres
 * (time@ice.com) to whom the Ant project is very grateful for his great code.
 */
namespace org.apache.commons.compress.archivers.tar
{

    /**
     * The TarInputStream reads a UNIX tar archive as an InputStream.
     * methods are provided to position at each successive entry in
     * the archive, and the read each entry as a normal input stream
     * using read().
     * @NotThreadSafe
     */
    public class TarArchiveInputStream : ArchiveInputStream {
        private const int SMALL_BUFFER_SIZE = 256;
        private const int BUFFER_SIZE = 8 * 1024;

        private bool hasHitEOF;
        private long entrySize;
        private long entryOffset;
        private byte[] readBuf;
        protected TarBuffer buffer;
        private TarArchiveEntry currEntry;

        /**
         * Constructor for TarInputStream.
         * @param is the input stream to use
         */
        public TarArchiveInputStream(java.io.InputStream inJ) {
            init(inJ, TarBuffer.DEFAULT_BLKSIZE, TarBuffer.DEFAULT_RCDSIZE);
        }

        /**
         * Constructor for TarInputStream.
         * @param is the input stream to use
         * @param blockSize the block size to use
         */
        public TarArchiveInputStream(java.io.InputStream inJ, int blockSize) {
            init(inJ, blockSize, TarBuffer.DEFAULT_RCDSIZE);
        }

        /**
         * Constructor for TarInputStream.
         * @param is the input stream to use
         * @param blockSize the block size to use
         * @param recordSize the record size to use
         */
        public TarArchiveInputStream(java.io.InputStream inJ, int blockSize, int recordSize) {
            this.init(inJ, blockSize, recordSize);
        }

        private void init(java.io.InputStream inJ, int blockSize, int recordSize)
        {
            this.buffer = new TarBuffer(inJ, blockSize, recordSize);
            this.readBuf = null;
            this.hasHitEOF = false;
        }

        /**
         * Closes this stream. Calls the TarBuffer's close() method.
         * @throws IOException on error
         */
        public override void close() //throws IOException 
        {
            buffer.close();
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
         * Get the available data that can be read from the current
         * entry in the archive. This does not indicate how much data
         * is left in the entire archive, only in the current entry.
         * This value is determined from the entry's size header field
         * and the amount of data already read from the current entry.
         * Integer.MAX_VALUE is returen in case more than Integer.MAX_VALUE
         * bytes are left in the current entry in the archive.
         *
         * @return The number of available bytes for the current entry.
         * @throws IOException for signature
         */
        public override int available() //throws IOException
        {
            if (entrySize - entryOffset > java.lang.Integer.MAX_VALUE) {
                return java.lang.Integer.MAX_VALUE;
            }
            return (int) (entrySize - entryOffset);
        }

        /**
         * Skip bytes in the input buffer. This skips bytes in the
         * current entry's data, not the entire archive, and will
         * stop at the end of the current entry's data if the number
         * to skip extends beyond that point.
         *
         * @param numToSkip The number of bytes to skip.
         * @return the number actually skipped
         * @throws IOException on error
         */
        public override long skip(long numToSkip) //throws IOException 
        {
            // REVIEW
            // This is horribly inefficient, but it ensures that we
            // properly skip over bytes via the TarBuffer...
            //
            byte[] skipBuf = new byte[BUFFER_SIZE];
            long skip = numToSkip;
            while (skip > 0) {
                int realSkip = (int) (skip > skipBuf.Length ? skipBuf.Length : skip);
                int numRead = read(skipBuf, 0, realSkip);
                if (numRead == -1) {
                    break;
                }
                skip -= numRead;
            }
            return (numToSkip - skip);
        }

        /**
         * Since we do not support marking just yet, we do nothing.
         */
        public override void reset() {
        }

        /**
         * Get the next entry in this tar archive. This will skip
         * over any remaining data in the current entry, if there
         * is one, and place the input stream at the header of the
         * next entry, and read the header and instantiate a new
         * TarEntry from the header bytes and return that entry.
         * If there are no more entries in the archive, null will
         * be returned to indicate that the end of the archive has
         * been reached.
         *
         * @return The next TarEntry in the archive, or null.
         * @throws IOException on error
         */
        public TarArchiveEntry getNextTarEntry() //throws IOException 
        {
            if (hasHitEOF) {
                return null;
            }

            if (currEntry != null) {
                long numToSkip = entrySize - entryOffset;

                while (numToSkip > 0) {
                    long skipped = skip(numToSkip);
                    if (skipped <= 0) {
                        throw new java.lang.RuntimeException("failed to skip current tar entry");
                    }
                    numToSkip -= skipped;
                }

                readBuf = null;
            }

            byte[] headerBuf = buffer.readRecord();

            if (headerBuf == null) {
                hasHitEOF = true;
            } else if (buffer.isEOFRecord(headerBuf)) {
                hasHitEOF = true;
            }

            if (hasHitEOF) {
                currEntry = null;
            } else {
                currEntry = new TarArchiveEntry(headerBuf);
                entryOffset = 0;
                entrySize = currEntry.getSize();
            }

            if (currEntry != null && currEntry.isGNULongNameEntry()) {
                // read in the name
                StringBuilder longName = new StringBuilder();
                byte[] buf = new byte[SMALL_BUFFER_SIZE];
                int length = 0;
                while ((length = read(buf)) >= 0) {
                    longName.Append(System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, length));
                }
                getNextEntry();
                if (currEntry == null) {
                    // Bugzilla: 40334
                    // Malformed tar file - long entry name not followed by entry
                    return null;
                }
                // remove trailing null terminator
                if (longName.Length > 0
                    && longName[longName.Length - 1] == 0) {
                    longName.deleteCharAt(longName.Length - 1);
                }
                currEntry.setName(longName.toString());
            }

            if (currEntry != null && currEntry.isPaxHeader()){ // Process Pax headers
                paxHeaders();
            }

            return currEntry;
        }

        private void paxHeaders() //throws IOException
        {
            java.io.BufferedReader br = new java.io.BufferedReader(new java.io.InputStreamReader(this, "UTF-8"));
            java.util.Map<String,String> headers = new java.util.HashMap<String,String>();
            // Format is "length keyword=value\n";
            while(true){ // get length
                int ch;
                int len=0;
                int read=0;
                while((ch = br.read()) != -1){
                    read++;
                    if (ch == ' '){ // End of length string
                        // Get keyword
                        StringBuilder sb = new StringBuilder();
                        while((ch = br.read()) != -1){
                            read++;
                            if (ch == '='){ // end of keyword
                                String keyword = sb.toString();
                                // Get rest of entry
                                char[] cbuf = new char[len-read];
                                int got = br.read(cbuf);
                                if (got != len-read){
                                    throw new java.io.IOException("Failed to read Paxheader. Expected "+(len-read)+" chars, read "+got);
                                }
                                String value = new String(cbuf, 0 , len-read-1); // Drop trailing NL
                                headers.put(keyword, value);
                                break;
                            }
                            sb.Append((char)ch);
                        }
                        break; // Processed single header
                    }
                    len *= 10;
                    len += ch - '0';
                }
                if (ch == -1){ // EOF
                    break;
                }
            }
            getNextEntry(); // Get the actual file entry
            /*
             * The following headers are defined for Pax.
             * atime, ctime, mtime, charset: cannot use these without changing TarArchiveEntry fields
             * comment
             * gid, gname
             * linkpath
             * size
             * uid,uname
             */
            java.util.Iterator<java.util.MapNS.Entry<String,String>> hdrs = headers.entrySet().iterator();
            while(hdrs.hasNext()){
                java.util.MapNS.Entry<String,String> ent = hdrs.next();
                String key = ent.getKey();
                String val = ent.getValue();
                if ("path".equals(key)){
                    currEntry.setName(val);
                } else if ("linkpath".equals(key)){
                    currEntry.setLinkName(val);
                } else if ("gid".equals(key)){
                    currEntry.setGroupId(java.lang.Integer.parseInt(val));
                } else if ("gname".equals(key)){
                    currEntry.setGroupName(val);
                } else if ("uid".equals(key)){
                    currEntry.setUserId(java.lang.Integer.parseInt(val));
                } else if ("uname".equals(key)){
                    currEntry.setUserName(val);
                } else if ("size".equals(key)){
                    currEntry.setSize(java.lang.Long.parseLong(val));
                }
            }
        }

        public override ArchiveEntry getNextEntry() //throws IOException 
        {
            return getNextTarEntry();
        }

        /**
         * Reads bytes from the current tar archive entry.
         *
         * This method is aware of the boundaries of the current
         * entry in the archive and will deal with them as if they
         * were this stream's start and EOF.
         *
         * @param buf The buffer into which to place bytes read.
         * @param offset The offset at which to place bytes read.
         * @param numToRead The number of bytes to read.
         * @return The number of bytes read, or -1 at EOF.
         * @throws IOException on error
         */
        public override int read(byte[] buf, int offset, int numToRead) //throws IOException 
        {
            int totalRead = 0;

            if (entryOffset >= entrySize) {
                return -1;
            }

            if ((numToRead + entryOffset) > entrySize) {
                numToRead = (int) (entrySize - entryOffset);
            }

            if (readBuf != null) {
                int sz = (numToRead > readBuf.Length) ? readBuf.Length: numToRead;

                java.lang.SystemJ.arraycopy(readBuf, 0, buf, offset, sz);

                if (sz >= readBuf.Length) {
                    readBuf = null;
                } else {
                    int newLen = readBuf.Length - sz;
                    byte[] newBuf = new byte[newLen];

                    java.lang.SystemJ.arraycopy(readBuf, sz, newBuf, 0, newLen);

                    readBuf = newBuf;
                }

                totalRead += sz;
                numToRead -= sz;
                offset += sz;
            }

            while (numToRead > 0) {
                byte[] rec = buffer.readRecord();

                if (rec == null) {
                    // Unexpected EOF!
                    throw new java.io.IOException("unexpected EOF with " + numToRead
                                          + " bytes unread. Occured at byte: " + getBytesRead());
                }
                count(rec.Length);
                int sz = numToRead;
                int recLen = rec.Length;

                if (recLen > sz) {
                    java.lang.SystemJ.arraycopy(rec, 0, buf, offset, sz);

                    readBuf = new byte[recLen - sz];

                    java.lang.SystemJ.arraycopy(rec, sz, readBuf, 0, recLen - sz);
                } else {
                    sz = recLen;

                    java.lang.SystemJ.arraycopy(rec, 0, buf, offset, recLen);
                }

                totalRead += sz;
                numToRead -= sz;
                offset += sz;
            }

            entryOffset += totalRead;

            return totalRead;
        }

        protected TarArchiveEntry getCurrentEntry() {
            return currEntry;
        }

        protected void setCurrentEntry(TarArchiveEntry e) {
            currEntry = e;
        }

        protected bool isAtEOF() {
            return hasHitEOF;
        }

        protected void setAtEOF(bool b) {
            hasHitEOF = b;
        }

        /**
         * Checks if the signature matches what is expected for a tar file.
         * 
         * @param signature
         *            the bytes to check
         * @param length
         *            the number of bytes to check
         * @return true, if this stream is a tar archive stream, false otherwise
         */
        public static bool matches(byte[] signature, int length) {
            if (length < TarConstants.VERSION_OFFSET+TarConstants.VERSIONLEN) {
                return false;
            }

            if (ArchiveUtils.matchAsciiBuffer(TarConstants.MAGIC_POSIX,
                    signature, TarConstants.MAGIC_OFFSET, TarConstants.MAGICLEN)
                &&
                ArchiveUtils.matchAsciiBuffer(TarConstants.VERSION_POSIX,
                    signature, TarConstants.VERSION_OFFSET, TarConstants.VERSIONLEN)
                    ){
                return true;
            }
            if (ArchiveUtils.matchAsciiBuffer(TarConstants.MAGIC_GNU,
                    signature, TarConstants.MAGIC_OFFSET, TarConstants.MAGICLEN)
                &&
                (
                 ArchiveUtils.matchAsciiBuffer(TarConstants.VERSION_GNU_SPACE,
                    signature, TarConstants.VERSION_OFFSET, TarConstants.VERSIONLEN)
                ||
                ArchiveUtils.matchAsciiBuffer(TarConstants.VERSION_GNU_ZERO,
                    signature, TarConstants.VERSION_OFFSET, TarConstants.VERSIONLEN)
                )
                    ){
                return true;
            }
            // COMPRESS-107 - recognise Ant tar files
            if (ArchiveUtils.matchAsciiBuffer(TarConstants.MAGIC_ANT,
                    signature, TarConstants.MAGIC_OFFSET, TarConstants.MAGICLEN)
                &&
                ArchiveUtils.matchAsciiBuffer(TarConstants.VERSION_ANT,
                    signature, TarConstants.VERSION_OFFSET, TarConstants.VERSIONLEN)
                    ){
                return true;
            }
            return false;
        }

    }
}