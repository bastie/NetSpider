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
using System.Text;
using java = biz.ritter.javapi;
using org.apache.commons.compress.archivers;
using org.apache.commons.compress.utils;

namespace org.apache.commons.compress.archivers.cpio {

    /**
     * CPIOArchiveOutputStream is a stream for writing CPIO streams. All formats of
     * CPIO are supported (old ASCII, old binary, new portable format and the new
     * portable format with CRC).
     * <p/>
     * <p/>
     * An entry can be written by creating an instance of CpioArchiveEntry and fill
     * it with the necessary values and put it into the CPIO stream. Afterwards
     * write the contents of the file into the CPIO stream. Either close the stream
     * by calling finish() or put a next entry into the cpio stream.
     * <p/>
     * <code><pre>
     * CpioArchiveOutputStream out = new CpioArchiveOutputStream(
     *         new FileOutputStream(new File("test.cpio")));
     * CpioArchiveEntry entry = new CpioArchiveEntry();
     * entry.setName("testfile");
     * String contents = &quot;12345&quot;;
     * entry.setFileSize(contents.length());
     * entry.setMode(CpioConstants.C_ISREG); // regular file
     * ... set other attributes, e.g. time, number of links
     * out.putArchiveEntry(entry);
     * out.write(testContents.getBytes());
     * out.close();
     * </pre></code>
     * <p/>
     * Note: This implementation should be compatible to cpio 2.5
     * 
     * This class uses mutable fields and is not considered threadsafe.
     * 
     * based on code from the jRPM project (jrpm.sourceforge.net)
     */
    public class CpioArchiveOutputStream : ArchiveOutputStream {//, CpioConstants {

        private CpioArchiveEntry entry;

        private bool closed = false;

        /** indicates if this archive is finished */
        private bool finished;

        /**
         * See {@link CpioArchiveEntry#setFormat(short)} for possible values.
         */
        private short entryFormat;

        private readonly java.util.HashMap<String,ArchiveEntry> names = new java.util.HashMap<String,ArchiveEntry>();

        private long crc = 0;

        private long written;

        private java.io.OutputStream outJ;

        private int blockSize;

        private long nextArtificalDeviceAndInode = 1;

        /**
         * Construct the cpio output stream with a specified format and a
         * blocksize of {@link CpioConstants#BLOCK_SIZE BLOCK_SIZE}.
         * 
         * @param out
         *            The cpio stream
         * @param format
         *            The format of the stream
         */
        public CpioArchiveOutputStream(java.io.OutputStream outJ, short format) {
            init(outJ, format, CpioConstants.BLOCK_SIZE);
        }

        /**
         * Construct the cpio output stream with a specified format
         * 
         * @param out
         *            The cpio stream
         * @param format
         *            The format of the stream
         * @param blockSize
         *            The block size of the archive.
         *            
         * @since Apache Commons Compress 1.1
         */
        public CpioArchiveOutputStream(java.io.OutputStream outJ, short format, int blockSize) {
        }

        /**
         * Construct the cpio output stream. The format for this CPIO stream is the
         * "new" format
         * 
         * @param out
         *            The cpio stream
         */
        public CpioArchiveOutputStream(java.io.OutputStream outJ) {
            init(outJ, CpioConstants.FORMAT_NEW, CpioConstants.BLOCK_SIZE);
        }
        private void init (java.io.OutputStream outJ, short format, int blockSize) {
            this.outJ = outJ;
            switch (format) {
                case CpioConstants.FORMAT_NEW:
                case CpioConstants.FORMAT_NEW_CRC:
                case CpioConstants.FORMAT_OLD_ASCII:
                case CpioConstants.FORMAT_OLD_BINARY:
                break;
            default:
                throw new java.lang.IllegalArgumentException("Unknown format: "+format);

            }
            this.entryFormat = format;
            this.blockSize = blockSize;
        }

        /**
         * Check to make sure that this stream has not been closed
         * 
         * @throws IOException
         *             if the stream is already closed
         */
        private void ensureOpen()// throws IOException 
        {
            if (this.closed) {
                throw new java.io.IOException("Stream closed");
            }
        }

        /**
         * Begins writing a new CPIO file entry and positions the stream to the
         * start of the entry data. Closes the current entry if still active. The
         * current time will be used if the entry has no set modification time and
         * the default header format will be used if no other format is specified in
         * the entry.
         * 
         * @param entry
         *            the CPIO cpioEntry to be written
         * @throws IOException
         *             if an I/O error has occurred or if a CPIO file error has
         *             occurred
         * @throws ClassCastException if entry is not an instance of CpioArchiveEntry
         */
        public override void putArchiveEntry(ArchiveEntry entry) //throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }

            CpioArchiveEntry e = (CpioArchiveEntry) entry;
            ensureOpen();
            if (this.entry != null) {
                closeArchiveEntry(); // close previous entry
            }
            if (e.getTime() == -1) {
                e.setTime(java.lang.SystemJ.currentTimeMillis() / 1000);
            }

            short format = e.getFormat();
            if (format != this.entryFormat){
                throw new java.io.IOException("Header format: "+format+" does not match existing format: "+this.entryFormat);
            }

            if (this.names.put(e.getName(), e) != null) {
                throw new java.io.IOException("duplicate entry: " + e.getName());
            }

            writeHeader(e);
            this.entry = e;
            this.written = 0;
        }

        private void writeHeader(CpioArchiveEntry e) //throws IOException 
        {
            switch (e.getFormat()) {
                case CpioConstants.FORMAT_NEW:
                    outJ.write(ArchiveUtils.toAsciiBytes(CpioConstants.MAGIC_NEW));
                    count(6);
                    writeNewEntry(e);
                    break;
                case CpioConstants.FORMAT_NEW_CRC:
                    outJ.write(ArchiveUtils.toAsciiBytes(CpioConstants.MAGIC_NEW_CRC));
                    count(6);
                    writeNewEntry(e);
                    break;
                case CpioConstants.FORMAT_OLD_ASCII:
                    outJ.write(ArchiveUtils.toAsciiBytes(CpioConstants.MAGIC_OLD_ASCII));
                    count(6);
                    writeOldAsciiEntry(e);
                    break;
                case CpioConstants.FORMAT_OLD_BINARY:
                    bool swapHalfWord = true;
                    writeBinaryLong(CpioConstants.MAGIC_OLD_BINARY, 2, swapHalfWord);
                    writeOldBinaryEntry(e, swapHalfWord);
                    break;
            }
        }

        private void writeNewEntry(CpioArchiveEntry entry) //throws IOException 
        {
            long inode = entry.getInode();
            long devMin = entry.getDeviceMin();
            if (CpioConstants.CPIO_TRAILER.equals(entry.getName()))
            {
                inode = devMin = 0;
            } else {
                if (inode == 0 && devMin == 0) {
                    inode = nextArtificalDeviceAndInode & 0xFFFFFFFF;
                    devMin = (nextArtificalDeviceAndInode++ >> 32) & 0xFFFFFFFF;
                } else {
                    nextArtificalDeviceAndInode =
                        java.lang.Math.max(nextArtificalDeviceAndInode,
                                 inode + 0x100000000L * devMin) + 1;
                }
            }

            writeAsciiLong(inode, 8, 16);
            writeAsciiLong(entry.getMode(), 8, 16);
            writeAsciiLong(entry.getUID(), 8, 16);
            writeAsciiLong(entry.getGID(), 8, 16);
            writeAsciiLong(entry.getNumberOfLinks(), 8, 16);
            writeAsciiLong(entry.getTime(), 8, 16);
            writeAsciiLong(entry.getSize(), 8, 16);
            writeAsciiLong(entry.getDeviceMaj(), 8, 16);
            writeAsciiLong(devMin, 8, 16);
            writeAsciiLong(entry.getRemoteDeviceMaj(), 8, 16);
            writeAsciiLong(entry.getRemoteDeviceMin(), 8, 16);
            writeAsciiLong(entry.getName().length() + 1, 8, 16);
            writeAsciiLong(entry.getChksum(), 8, 16);
            writeCString(entry.getName());
            pad(entry.getHeaderPadCount());
        }

        private void writeOldAsciiEntry(CpioArchiveEntry entry)
                //throws IOException 
                {
            long inode = entry.getInode();
            long device = entry.getDevice();
            if (CpioConstants.CPIO_TRAILER.equals(entry.getName()))
            {
                inode = device = 0;
            } else {
                if (inode == 0 && device == 0) {
                    inode = nextArtificalDeviceAndInode & 0777777;
                    device = (nextArtificalDeviceAndInode++ >> 18) & 0777777;
                } else {
                    nextArtificalDeviceAndInode =
                        java.lang.Math.max(nextArtificalDeviceAndInode,
                                 inode + 01000000 * device) + 1;
                }
            }

            writeAsciiLong(device, 6, 8);
            writeAsciiLong(inode, 6, 8);
            writeAsciiLong(entry.getMode(), 6, 8);
            writeAsciiLong(entry.getUID(), 6, 8);
            writeAsciiLong(entry.getGID(), 6, 8);
            writeAsciiLong(entry.getNumberOfLinks(), 6, 8);
            writeAsciiLong(entry.getRemoteDevice(), 6, 8);
            writeAsciiLong(entry.getTime(), 11, 8);
            writeAsciiLong(entry.getName().length() + 1, 6, 8);
            writeAsciiLong(entry.getSize(), 11, 8);
            writeCString(entry.getName());
        }

        private void writeOldBinaryEntry(CpioArchiveEntry entry, bool swapHalfWord) //throws IOException 
        {
            long inode = entry.getInode();
            long device = entry.getDevice();
            if (CpioConstants.CPIO_TRAILER.equals(entry.getName()))
            {
                inode = device = 0;
            } else {
                if (inode == 0 && device == 0) {
                    inode = nextArtificalDeviceAndInode & 0xFFFF;
                    device = (nextArtificalDeviceAndInode++ >> 16) & 0xFFFF;
                } else {
                    nextArtificalDeviceAndInode =
                        java.lang.Math.max(nextArtificalDeviceAndInode,
                                 inode + 0x10000 * device) + 1;
                }
            }

            writeBinaryLong(device, 2, swapHalfWord);
            writeBinaryLong(inode, 2, swapHalfWord);
            writeBinaryLong(entry.getMode(), 2, swapHalfWord);
            writeBinaryLong(entry.getUID(), 2, swapHalfWord);
            writeBinaryLong(entry.getGID(), 2, swapHalfWord);
            writeBinaryLong(entry.getNumberOfLinks(), 2, swapHalfWord);
            writeBinaryLong(entry.getRemoteDevice(), 2, swapHalfWord);
            writeBinaryLong(entry.getTime(), 4, swapHalfWord);
            writeBinaryLong(entry.getName().length() + 1, 2, swapHalfWord);
            writeBinaryLong(entry.getSize(), 4, swapHalfWord);
            writeCString(entry.getName());
            pad(entry.getHeaderPadCount());
        }

        /*(non-Javadoc)
         * 
         * @see
         * org.apache.commons.compress.archivers.ArchiveOutputStream#closeArchiveEntry
         * ()
         */
        public override void closeArchiveEntry() //throws IOException
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }

            ensureOpen();

            if (entry == null) {
                throw new java.io.IOException("Trying to close non-existent entry");
            }

            if (this.entry.getSize() != this.written) {
                throw new java.io.IOException("invalid entry size (expected "
                        + this.entry.getSize() + " but got " + this.written
                        + " bytes)");
            }
            pad(this.entry.getDataPadCount());
            if (this.entry.getFormat() == CpioConstants.FORMAT_NEW_CRC)
            {
                if (this.crc != this.entry.getChksum()) {
                    throw new java.io.IOException("CRC Error");
                }
            }
            this.entry = null;
            this.crc = 0;
            this.written = 0;
        }

        /**
         * Writes an array of bytes to the current CPIO entry data. This method will
         * block until all the bytes are written.
         * 
         * @param b
         *            the data to be written
         * @param off
         *            the start offset in the data
         * @param len
         *            the number of bytes that are written
         * @throws IOException
         *             if an I/O error has occurred or if a CPIO file error has
         *             occurred
         */
        public override void write(byte[] b, int off, int len)
                //throws IOException 
                {
            ensureOpen();
            if (off < 0 || len < 0 || off > b.Length - len) {
                throw new java.lang.IndexOutOfBoundsException();
            } else if (len == 0) {
                return;
            }

            if (this.entry == null) {
                throw new java.io.IOException("no current CPIO entry");
            }
            if (this.written + len > this.entry.getSize()) {
                throw new java.io.IOException("attempt to write past end of STORED entry");
            }
            outJ.write(b, off, len);
            this.written += len;
            if (this.entry.getFormat() == CpioConstants.FORMAT_NEW_CRC)
            {
                for (int pos = 0; pos < len; pos++) {
                    this.crc += b[pos] & 0xFF;
                }
            }
            count(len);
        }

        /**
         * Finishes writing the contents of the CPIO output stream without closing
         * the underlying stream. Use this method when applying multiple filters in
         * succession to the same output stream.
         * 
         * @throws IOException
         *             if an I/O exception has occurred or if a CPIO file error has
         *             occurred
         */
        public override void finish() //throws IOException 
        {
            ensureOpen();
            if (finished) {
                throw new java.io.IOException("This archive has already been finished");
            }

            if (this.entry != null) {
                throw new java.io.IOException("This archive contains unclosed entries.");
            }
            this.entry = new CpioArchiveEntry(this.entryFormat);
            this.entry.setName(CpioConstants.CPIO_TRAILER);
            this.entry.setNumberOfLinks(1);
            writeHeader(this.entry);
            closeArchiveEntry();

            int lengthOfLastBlock = (int) (getBytesWritten() % blockSize);
            if (lengthOfLastBlock != 0) {
                pad(blockSize - lengthOfLastBlock);
            }

            finished = true;
        }

        /**
         * Closes the CPIO output stream as well as the stream being filtered.
         * 
         * @throws IOException
         *             if an I/O error has occurred or if a CPIO file error has
         *             occurred
         */
        public override void close() //throws IOException 
        {
            if(!finished) {
                finish();
            }

            if (!this.closed) {
                outJ.close();
                this.closed = true;
            }
        }

        private void pad(int countJ) //throws IOException
        {
            if (countJ > 0){
                byte []buff = new byte[countJ];
                outJ.write(buff);
                count(countJ);
            }
        }

        private void writeBinaryLong(long number, int length, bool swapHalfWord) //throws IOException 
        {
            byte [] tmp = CpioUtil.long2byteArray(number, length, swapHalfWord);
            outJ.write(tmp);
            count(tmp.Length);
        }

        private void writeAsciiLong(long number, int length, int radix) //throws IOException 
        {
            StringBuilder tmp = new StringBuilder();
            String tmpStr;
            if (radix == 16) {
                tmp.Append(java.lang.Long.toHexString(number));
            } else if (radix == 8) {
                tmp.Append(java.lang.Long.toOctalString(number));
            } else {
                tmp.Append(""+number);
            }

            if (tmp.Length <= length) {
                long insertLength = length - tmp.Length;
                for (int pos = 0; pos < insertLength; pos++) {
                    tmp.Insert(0, "0");
                }
                tmpStr = tmp.toString();
            } else {
                tmpStr = tmp.ToString().Substring(tmp.Length - length);
            }
            byte[] b = ArchiveUtils.toAsciiBytes(tmpStr);
            outJ.write(b);
            count(b.Length);
        }

        /**
         * Writes an ASCII string to the stream followed by \0
         * @param str the String to write
         * @throws IOException if the string couldn't be written
         */
        private void writeCString(String str) //throws IOException 
        {
            byte[] b = ArchiveUtils.toAsciiBytes(str);
            outJ.write(b);
            outJ.write('\0');
            count(b.Length + 1);
        }

        /**
         * Creates a new ArchiveEntry. The entryName must be an ASCII encoded string.
         * 
         * @see org.apache.commons.compress.archivers.ArchiveOutputStream#createArchiveEntry(java.io.File, java.lang.String)
         */
        public override ArchiveEntry createArchiveEntry(java.io.File inputFile, String entryName)
                //throws IOException 
                {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            return new CpioArchiveEntry(inputFile, entryName);
        }

    }
}