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
using java = biz.ritter.javapi;
using org.apache.commons.compress.archivers;

namespace org.apache.commons.compress.archivers.zip {

    /**
     * Reimplementation of {@link java.util.zip.ZipOutputStream
     * java.util.zip.ZipOutputStream} that does handle the extended
     * functionality of this package, especially internal/external file
     * attributes and extra fields with different layouts for local file
     * data and central directory entries.
     *
     * <p>This class will try to use {@link java.io.RandomAccessFile
     * RandomAccessFile} when you know that the output is going to go to a
     * file.</p>
     *
     * <p>If RandomAccessFile cannot be used, this implementation will use
     * a Data Descriptor to store size and CRC information for {@link
     * #DEFLATED DEFLATED} entries, this means, you don't need to
     * calculate them yourself.  Unfortunately this is not possible for
     * the {@link #STORED STORED} method, here setting the CRC and
     * uncompressed size information is required before {@link
     * #putArchiveEntry(ArchiveEntry)} can be called.</p>
     * @NotThreadSafe
     */
    public class ZipArchiveOutputStream : ArchiveOutputStream {

        protected internal static readonly int BYTE_MASK = 0xFF;
        private static readonly int SHORT = 2;
        private static readonly int WORD = 4;
        protected internal static readonly int BUFFER_SIZE = 512;

        /** indicates if this archive is finished. protected for use in Jar implementation */
        protected bool finished = false;

        /* 
         * Apparently Deflater.setInput gets slowed down a lot on Sun JVMs
         * when it gets handed a really big buffer.  See
         * https://issues.apache.org/bugzilla/show_bug.cgi?id=45396
         *
         * Using a buffer size of 8 kB proved to be a good compromise
         */
        private static readonly int DEFLATER_BLOCK_SIZE = 8192;

        /**
         * Compression method for deflated entries.
         */
        public static readonly int DEFLATED = java.util.zip.ZipEntry.DEFLATED;

        /**
         * Default compression level for deflated entries.
         */
        public static readonly int DEFAULT_COMPRESSION = java.util.zip.Deflater.DEFAULT_COMPRESSION;

        /**
         * Compression method for stored entries.
         */
        public static readonly int STORED = java.util.zip.ZipEntry.STORED;

        /**
         * default encoding for file names and comment.
         */
        static readonly String DEFAULT_ENCODING = ZipEncodingHelper.UTF8;

        /**
         * General purpose flag, which indicates that filenames are
         * written in utf-8.
         * @deprecated use {@link GeneralPurposeBit#UFT8_NAMES_FLAG} instead
         */
        public static readonly int EFS_FLAG = GeneralPurposeBit.UFT8_NAMES_FLAG;

        /**
         * Current entry.
         */
        private ZipArchiveEntry entry;

        /**
         * The file comment.
         */
        private String comment = "";

        /**
         * Compression level for next entry.
         */
        private int level = DEFAULT_COMPRESSION;

        /**
         * Has the compression level changed when compared to the last
         * entry?
         */
        private bool hasCompressionLevelChanged = false;

        /**
         * Default compression method for next entry.
         */
        private int method = java.util.zip.ZipEntry.DEFLATED;

        /**
         * List of ZipArchiveEntries written so far.
         */
        private readonly java.util.List<ZipArchiveEntry> entries = new java.util.LinkedList<ZipArchiveEntry>();

        /**
         * CRC instance to avoid parsing DEFLATED data twice.
         */
        private readonly java.util.zip.CRC32 crc = new java.util.zip.CRC32();

        /**
         * Count the bytes written to out.
         */
        private long written = 0;

        /**
         * Data for local header data
         */
        private long dataStart = 0;

        /**
         * Offset for CRC entry in the local file header data for the
         * current entry starts here.
         */
        private long localDataStart = 0;

        /**
         * Start of central directory.
         */
        private long cdOffset = 0;

        /**
         * Length of central directory.
         */
        private long cdLength = 0;

        /**
         * Helper, a 0 as ZipShort.
         */
        private static readonly byte[] ZERO = {0, 0};

        /**
         * Helper, a 0 as ZipLong.
         */
        private static readonly byte[] LZERO = {0, 0, 0, 0};

        /**
         * Holds the offsets of the LFH starts for each entry.
         */
        private readonly java.util.Map<ZipArchiveEntry,byte[]> offsets = new java.util.HashMap<ZipArchiveEntry,byte[]>();

        /**
         * The encoding to use for filenames and the file comment.
         *
         * <p>For a list of possible values see <a
         * href="http://java.sun.com/j2se/1.5.0/docs/guide/intl/encoding.doc.html">http://java.sun.com/j2se/1.5.0/docs/guide/intl/encoding.doc.html</a>.
         * Defaults to UTF-8.</p>
         */
        private String encoding = DEFAULT_ENCODING;

        /**
         * The zip encoding to use for filenames and the file comment.
         *
         * This field is of internal use and will be set in {@link
         * #setEncoding(String)}.
         */
        private ZipEncoding zipEncoding =
            ZipEncodingHelper.getZipEncoding(DEFAULT_ENCODING);

        /**
         * This Deflater object is used for output.
         *
         */
        protected readonly java.util.zip.Deflater def = new java.util.zip.Deflater(DEFAULT_COMPRESSION, true);

        /**
         * This buffer servers as a Deflater.
         *
         */
        private readonly byte[] buf = new byte[BUFFER_SIZE];

        /**
         * Optional random access output.
         */
        private readonly java.io.RandomAccessFile raf;

        private readonly java.io.OutputStream outJ;

        /**
         * whether to use the general purpose bit flag when writing UTF-8
         * filenames or not.
         */
        private bool useUTF8Flag = true; 

        /**
         * Whether to encode non-encodable file names as UTF-8.
         */
        private bool fallbackToUTF8 = false;

        /**
         * whether to create UnicodePathExtraField-s for each entry.
         */
        private UnicodeExtraFieldPolicy createUnicodeExtraFields = UnicodeExtraFieldPolicy.NEVER;

        /**
         * Creates a new ZIP OutputStream filtering the underlying stream.
         * @param out the outputstream to zip
         */
        public ZipArchiveOutputStream(java.io.OutputStream output) {
            this.outJ = output;
            this.raf = null;
        }

        /**
         * Creates a new ZIP OutputStream writing to a File.  Will use
         * random access if possible.
         * @param file the file to zip to
         * @throws IOException on error
         */
        public ZipArchiveOutputStream(java.io.File file) //throws IOException 
        {
            java.io.OutputStream o = null;
            java.io.RandomAccessFile _raf = null;
            try {
                _raf = new java.io.RandomAccessFile(file, "rw");
                _raf.setLength(0);
            } catch (java.io.IOException ) {
                if (_raf != null) {
                    try {
                        _raf.close();
                    } catch (java.io.IOException ) {
                        // ignore
                    }
                    _raf = null;
                }
                o = new java.io.FileOutputStream(file);
            }
            outJ = o;
            raf = _raf;
        }

        /**
         * This method indicates whether this archive is writing to a
         * seekable stream (i.e., to a random access file).
         *
         * <p>For seekable streams, you don't need to calculate the CRC or
         * uncompressed size for {@link #STORED} entries before
         * invoking {@link #putArchiveEntry(ArchiveEntry)}.
         * @return true if seekable
         */
        public bool isSeekable() {
            return raf != null;
        }

        /**
         * The encoding to use for filenames and the file comment.
         *
         * <p>For a list of possible values see <a
         * href="http://java.sun.com/j2se/1.5.0/docs/guide/intl/encoding.doc.html">http://java.sun.com/j2se/1.5.0/docs/guide/intl/encoding.doc.html</a>.
         * Defaults to UTF-8.</p>
         * @param encoding the encoding to use for file names, use null
         * for the platform's default encoding
         */
        public void setEncoding(String encoding) {
            this.encoding = encoding;
            this.zipEncoding = ZipEncodingHelper.getZipEncoding(encoding);
            useUTF8Flag &= ZipEncodingHelper.isUTF8(encoding);
        }

        /**
         * The encoding to use for filenames and the file comment.
         *
         * @return null if using the platform's default character encoding.
         */
        public String getEncoding() {
            return encoding;
        }

        /**
         * Whether to set the language encoding flag if the file name
         * encoding is UTF-8.
         *
         * <p>Defaults to true.</p>
         */
        public void setUseLanguageEncodingFlag(bool b) {
            useUTF8Flag = b && ZipEncodingHelper.isUTF8(encoding);
        }

        /**
         * Whether to create Unicode Extra Fields.
         *
         * <p>Defaults to NEVER.</p>
         */
        public void setCreateUnicodeExtraFields(UnicodeExtraFieldPolicy b) {
            createUnicodeExtraFields = b;
        }

        /**
         * Whether to fall back to UTF and the language encoding flag if
         * the file name cannot be encoded using the specified encoding.
         *
         * <p>Defaults to false.</p>
         */
        public void setFallbackToUTF8(bool b) {
            fallbackToUTF8 = b;
        }

        /** {@inheritDoc} */
        public override void finish() //throws IOException 
        {
            if (finished) {
                throw new java.io.IOException("This archive has already been finished");
            }

            if (entry != null) {
                throw new java.io.IOException("This archives contains unclosed entries.");
            }

            cdOffset = written;
            for (java.util.Iterator<ZipArchiveEntry> i = entries.iterator(); i.hasNext(); )
            {
                writeCentralFileHeader((ZipArchiveEntry) i.next());
            }
            cdLength = written - cdOffset;
            writeCentralDirectoryEnd();
            offsets.clear();
            entries.clear();
            finished = true;
        }

        /**
         * Writes all necessary data for this entry.
         * @throws IOException on error
         */
        public override void closeArchiveEntry() //throws IOException 
        {
            if (finished) {
                throw new java.io.IOException("Stream has already been finished");
            }

            if (entry == null) {
                throw new java.io.IOException("No current entry to close");
            }

            long realCrc = crc.getValue();
            crc.reset();

            if (entry.getMethod() == DEFLATED) {
                def.finish();
                while (!def.finished()) {
                    deflate();
                }

                entry.setSize(ZipUtil.adjustToLong(def.getTotalIn()));
                entry.setCompressedSize(ZipUtil.adjustToLong(def.getTotalOut()));
                entry.setCrc(realCrc);

                def.reset();

                written += entry.getCompressedSize();
            } else if (raf == null) {
                if (entry.getCrc() != realCrc) {
                    throw new java.util.zip.ZipException("bad CRC checksum for entry "
                                           + entry.getName() + ": "
                                           + java.lang.Long.toHexString(entry.getCrc())
                                           + " instead of "
                                           + java.lang.Long.toHexString(realCrc));
                }

                if (entry.getSize() != written - dataStart) {
                    throw new java.util.zip.ZipException("bad size for entry "
                                           + entry.getName() + ": "
                                           + entry.getSize()
                                           + " instead of "
                                           + (written - dataStart));
                }
            } else { /* method is STORED and we used RandomAccessFile */
                long size = written - dataStart;

                entry.setSize(size);
                entry.setCompressedSize(size);
                entry.setCrc(realCrc);
            }

            // If random access output, write the local file header containing
            // the correct CRC and compressed/uncompressed sizes
            if (raf != null) {
                long save = raf.getFilePointer();

                raf.seek(localDataStart);
                writeOut(ZipLong.getBytes(entry.getCrc()));
                writeOut(ZipLong.getBytes(entry.getCompressedSize()));
                writeOut(ZipLong.getBytes(entry.getSize()));
                raf.seek(save);
            }

            writeDataDescriptor(entry);
            entry = null;
        }

        /**
         * {@inheritDoc} 
         * @throws ClassCastException if entry is not an instance of ZipArchiveEntry
         */
        public override void putArchiveEntry(ArchiveEntry archiveEntry) //throws IOException 
        {
            if (finished) {
                throw new java.io.IOException("Stream has already been finished");
            }

            if (entry != null) {
                closeArchiveEntry();
            }

            entry = ((ZipArchiveEntry) archiveEntry);
            entries.add(entry);

            if (entry.getMethod() == -1) { // not specified
                entry.setMethod(method);
            }

            if (entry.getTime() == -1) { // not specified
                entry.setTime(java.lang.SystemJ.currentTimeMillis());
            }

            // Size/CRC not required if RandomAccessFile is used
            if (entry.getMethod() == STORED && raf == null) {
                if (entry.getSize() == -1) {
                    throw new java.util.zip.ZipException("uncompressed size is required for"
                                           + " STORED method when not writing to a"
                                           + " file");
                }
                if (entry.getCrc() == -1) {
                    throw new java.util.zip.ZipException("crc checksum is required for STORED"
                                           + " method when not writing to a file");
                }
                entry.setCompressedSize(entry.getSize());
            }

            if (entry.getMethod() == DEFLATED && hasCompressionLevelChanged) {
                def.setLevel(level);
                hasCompressionLevelChanged = false;
            }
            writeLocalFileHeader(entry);
        }

        /**
         * Set the file comment.
         * @param comment the comment
         */
        public void setComment(String comment) {
            this.comment = comment;
        }

        /**
         * Sets the compression level for subsequent entries.
         *
         * <p>Default is Deflater.DEFAULT_COMPRESSION.</p>
         * @param level the compression level.
         * @throws IllegalArgumentException if an invalid compression
         * level is specified.
         */
        public void setLevel(int level) {
            if (level < java.util.zip.Deflater.DEFAULT_COMPRESSION
                || level > java.util.zip.Deflater.BEST_COMPRESSION)
            {
                throw new java.lang.IllegalArgumentException("Invalid compression level: "
                                                   + level);
            }
            hasCompressionLevelChanged = (this.level != level);
            this.level = level;
        }

        /**
         * Sets the default compression method for subsequent entries.
         *
         * <p>Default is DEFLATED.</p>
         * @param method an <code>int</code> from java.util.zip.ZipEntry
         */
        public void setMethod(int method) {
            this.method = method;
        }

        /**
         * Whether this stream is able to write the given entry.
         *
         * <p>May return false if it is set up to use encryption or a
         * compression method that hasn't been implemented yet.</p>
         * @since Apache Commons Compress 1.1
         */
        public override bool canWriteEntryData(ArchiveEntry ae) {
            if (ae is ZipArchiveEntry) {
                return ZipUtil.canHandleEntryData((ZipArchiveEntry) ae);
            }
            return false;
        }

        /**
         * Writes bytes to ZIP entry.
         * @param b the byte array to write
         * @param offset the start position to write from
         * @param length the number of bytes to write
         * @throws IOException on error
         */
        public override void write(byte[] b, int offset, int length) //throws IOException 
        {
            ZipUtil.checkRequestedFeatures(entry);
            if (entry.getMethod() == DEFLATED) {
                if (length > 0) {
                    if (!def.finished()) {
                        if (length <= DEFLATER_BLOCK_SIZE) {
                            def.setInput(b, offset, length);
                            deflateUntilInputIsNeeded();
                        } else {
                            int fullblocks = length / DEFLATER_BLOCK_SIZE;
                            for (int i = 0; i < fullblocks; i++) {
                                def.setInput(b, offset + i * DEFLATER_BLOCK_SIZE,
                                             DEFLATER_BLOCK_SIZE);
                                deflateUntilInputIsNeeded();
                            }
                            int done = fullblocks * DEFLATER_BLOCK_SIZE;
                            if (done < length) {
                                def.setInput(b, offset + done, length - done);
                                deflateUntilInputIsNeeded();
                            }
                        }
                    }
                }
            } else {
                writeOut(b, offset, length);
                written += length;
            }
            crc.update(b, offset, length);
            count(length);
        }

        /**
         * Closes this output stream and releases any system resources
         * associated with the stream.
         *
         * @exception  IOException  if an I/O error occurs.
         */
        public override void close() //throws IOException 
        {
            if (!finished) {
                finish();
            }

            if (raf != null) {
                raf.close();
            }
            if (outJ != null) {
                outJ.close();
            }
        }

        /**
         * Flushes this output stream and forces any buffered output bytes
         * to be written out to the stream.
         *
         * @exception  IOException  if an I/O error occurs.
         */
        public override void flush() //throws IOException 
        {
            if (outJ != null) {
                outJ.flush();
            }
        }

        /*
         * Various ZIP constants
         */
        /**
         * local file header signature
         */
        protected internal static readonly byte[] LFH_SIG = ZipLong.LFH_SIG.getBytes();
        /**
         * data descriptor signature
         */
        protected internal static readonly byte[] DD_SIG = ZipLong.DD_SIG.getBytes();
        /**
         * central file header signature
         */
        protected internal static readonly byte[] CFH_SIG = ZipLong.CFH_SIG.getBytes();
        /**
         * end of central dir signature
         */
        protected internal static readonly byte[] EOCD_SIG = ZipLong.getBytes(0X06054B50L);

        /**
         * Writes next block of compressed data to the output stream.
         * @throws IOException on error
         */
        protected void deflate() //throws IOException 
        {
            int len = def.deflate(buf, 0, buf.Length);
            if (len > 0) {
                writeOut(buf, 0, len);
            }
        }

        /**
         * Writes the local file header entry
         * @param ze the entry to write
         * @throws IOException on error
         */
        protected void writeLocalFileHeader(ZipArchiveEntry ze) //throws IOException 
        {

            bool encodable = zipEncoding.canEncode(ze.getName());

            ZipEncoding entryEncoding;

            if (!encodable && fallbackToUTF8) {
                entryEncoding = ZipEncodingHelper.UTF8_ZIP_ENCODING;
            } else {
                entryEncoding = zipEncoding;
            }

            java.nio.ByteBuffer name = entryEncoding.encode(ze.getName());

            if (createUnicodeExtraFields != UnicodeExtraFieldPolicy.NEVER) {

                if (createUnicodeExtraFields == UnicodeExtraFieldPolicy.ALWAYS
                    || !encodable) {
                    ze.addExtraField(new UnicodePathExtraField(ze.getName(),
                                                               (byte[])name.array(),
                                                               name.arrayOffset(),
                                                               name.limit()));
                }

                String comm = ze.getComment();
                if (comm != null && !"".equals(comm)) {

                    bool commentEncodable = this.zipEncoding.canEncode(comm);

                    if (createUnicodeExtraFields == UnicodeExtraFieldPolicy.ALWAYS
                        || !commentEncodable) {
                        java.nio.ByteBuffer commentB = entryEncoding.encode(comm);
                        ze.addExtraField(new UnicodeCommentExtraField(comm,
                                                                      (byte[])commentB.array(),
                                                                      commentB.arrayOffset(),
                                                                      commentB.limit())
                                         );
                    }
                }
            }

            offsets.put(ze, ZipLong.getBytes(written));

            writeOut(LFH_SIG);
            written += WORD;

            //store method in local variable to prevent multiple method calls
            int zipMethod = ze.getMethod();

            writeVersionNeededToExtractAndGeneralPurposeBits(zipMethod,
                                                             !encodable
                                                             && fallbackToUTF8);
            written += WORD;

            // compression method
            writeOut(ZipShort.getBytes(zipMethod));
            written += SHORT;

            // last mod. time and date
            writeOut(ZipUtil.toDosTime(ze.getTime()));
            written += WORD;

            // CRC
            // compressed length
            // uncompressed length
            localDataStart = written;
            if (zipMethod == DEFLATED || raf != null) {
                writeOut(LZERO);
                writeOut(LZERO);
                writeOut(LZERO);
            } else {
                writeOut(ZipLong.getBytes(ze.getCrc()));
                writeOut(ZipLong.getBytes(ze.getSize()));
                writeOut(ZipLong.getBytes(ze.getSize()));
            }
            // CheckStyle:MagicNumber OFF
            written += 12;
            // CheckStyle:MagicNumber ON

            // file name length
            writeOut(ZipShort.getBytes(name.limit()));
            written += SHORT;

            // extra field length
            byte[] extra = ze.getLocalFileDataExtra();
            writeOut(ZipShort.getBytes(extra.Length));
            written += SHORT;

            // file name
            writeOut((byte[])name.array(), name.arrayOffset(), name.limit());
            written += name.limit();

            // extra field
            writeOut(extra);
            written += extra.Length;

            dataStart = written;
        }

        /**
         * Writes the data descriptor entry.
         * @param ze the entry to write
         * @throws IOException on error
         */
        protected void writeDataDescriptor(ZipArchiveEntry ze) //throws IOException 
        {
            if (ze.getMethod() != DEFLATED || raf != null) {
                return;
            }
            writeOut(DD_SIG);
            writeOut(ZipLong.getBytes(entry.getCrc()));
            writeOut(ZipLong.getBytes(entry.getCompressedSize()));
            writeOut(ZipLong.getBytes(entry.getSize()));
            // CheckStyle:MagicNumber OFF
            written += 16;
            // CheckStyle:MagicNumber ON
        }

        /**
         * Writes the central file header entry.
         * @param ze the entry to write
         * @throws IOException on error
         */
        protected void writeCentralFileHeader(ZipArchiveEntry ze) //throws IOException 
        {
            writeOut(CFH_SIG);
            written += WORD;

            // version made by
            // CheckStyle:MagicNumber OFF
            writeOut(ZipShort.getBytes((ze.getPlatform() << 8) | 20));
            written += SHORT;

            int zipMethod = ze.getMethod();
            bool encodable = zipEncoding.canEncode(ze.getName());
            writeVersionNeededToExtractAndGeneralPurposeBits(zipMethod,
                                                             !encodable
                                                             && fallbackToUTF8);
            written += WORD;

            // compression method
            writeOut(ZipShort.getBytes(zipMethod));
            written += SHORT;

            // last mod. time and date
            writeOut(ZipUtil.toDosTime(ze.getTime()));
            written += WORD;

            // CRC
            // compressed length
            // uncompressed length
            writeOut(ZipLong.getBytes(ze.getCrc()));
            writeOut(ZipLong.getBytes(ze.getCompressedSize()));
            writeOut(ZipLong.getBytes(ze.getSize()));
            // CheckStyle:MagicNumber OFF
            written += 12;
            // CheckStyle:MagicNumber ON

            // file name length
            ZipEncoding entryEncoding;

            if (!encodable && fallbackToUTF8) {
                entryEncoding = ZipEncodingHelper.UTF8_ZIP_ENCODING;
            } else {
                entryEncoding = zipEncoding;
            }

            java.nio.ByteBuffer name = entryEncoding.encode(ze.getName());

            writeOut(ZipShort.getBytes(name.limit()));
            written += SHORT;

            // extra field length
            byte[] extra = ze.getCentralDirectoryExtra();
            writeOut(ZipShort.getBytes(extra.Length));
            written += SHORT;

            // file comment length
            String comm = ze.getComment();
            if (comm == null) {
                comm = "";
            }

            java.nio.ByteBuffer commentB = entryEncoding.encode(comm);

            writeOut(ZipShort.getBytes(commentB.limit()));
            written += SHORT;

            // disk number start
            writeOut(ZERO);
            written += SHORT;

            // internal file attributes
            writeOut(ZipShort.getBytes(ze.getInternalAttributes()));
            written += SHORT;

            // external file attributes
            writeOut(ZipLong.getBytes(ze.getExternalAttributes()));
            written += WORD;

            // relative offset of LFH
            writeOut((byte[]) offsets.get(ze));
            written += WORD;

            // file name
            writeOut((byte[])name.array(), name.arrayOffset(), name.limit());
            written += name.limit();

            // extra field
            writeOut(extra);
            written += extra.Length;

            // file comment
            writeOut((byte[])commentB.array(), commentB.arrayOffset(), commentB.limit());
            written += commentB.limit();
        }

        /**
         * Writes the &quot;End of central dir record&quot;.
         * @throws IOException on error
         */
        protected void writeCentralDirectoryEnd() //throws IOException 
        {
            writeOut(EOCD_SIG);

            // disk numbers
            writeOut(ZERO);
            writeOut(ZERO);

            // number of entries
            byte[] num = ZipShort.getBytes(entries.size());
            writeOut(num);
            writeOut(num);

            // length and location of CD
            writeOut(ZipLong.getBytes(cdLength));
            writeOut(ZipLong.getBytes(cdOffset));

            // ZIP file comment
            java.nio.ByteBuffer data = this.zipEncoding.encode(comment);
            writeOut(ZipShort.getBytes(data.limit()));
            writeOut((byte[])data.array(), data.arrayOffset(), data.limit());
        }

        /**
         * Write bytes to output or random access file.
         * @param data the byte array to write
         * @throws IOException on error
         */
        protected void writeOut(byte[] data) //throws IOException 
        {
            writeOut(data, 0, data.Length);
        }

        /**
         * Write bytes to output or random access file.
         * @param data the byte array to write
         * @param offset the start position to write from
         * @param length the number of bytes to write
         * @throws IOException on error
         */
        protected void writeOut(byte[] data, int offset, int length)
            //throws IOException 
            {
            if (raf != null) {
                raf.write(data, offset, length);
            } else {
                outJ.write(data, offset, length);
            }
        }

        private void deflateUntilInputIsNeeded() //throws IOException 
        {
            while (!def.needsInput()) {
                deflate();
            }
        }

        private void writeVersionNeededToExtractAndGeneralPurposeBits(int zipMethod,
                                                                      bool utfFallback)
            //throws IOException 
            {

            // CheckStyle:MagicNumber OFF
            int versionNeededToExtract = 10;
            GeneralPurposeBit b = new GeneralPurposeBit();
            b.useUTF8ForNames(useUTF8Flag || utfFallback);
            if (zipMethod == DEFLATED && raf == null) {
                // requires version 2 as we are going to store length info
                // in the data descriptor
                versionNeededToExtract =  20;
                b.useDataDescriptor(true);
            }
            // CheckStyle:MagicNumber ON

            // version needed to extract
            writeOut(ZipShort.getBytes(versionNeededToExtract));
            // general purpose bit flag
            writeOut(b.encode());
        }

        /**
         * Creates a new zip entry taking some information from the given
         * file and using the provided name.
         *
         * <p>The name will be adjusted to end with a forward slash "/" if
         * the file is a directory.  If the file is not a directory a
         * potential trailing forward slash will be stripped from the
         * entry name.</p>
         *
         * <p>Must not be used if the stream has already been closed.</p>
         */
        public override ArchiveEntry createArchiveEntry(java.io.File inputFile, String entryName)
                //throws IOException 
        {
            if (finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            return new ZipArchiveEntry(inputFile, entryName);
        }
    }
#region UnicodeExtraFieldPolicy
    /**
        * enum that represents the possible policies for creating Unicode
        * extra fields.
        */
    public sealed class UnicodeExtraFieldPolicy {
        /**
            * Always create Unicode extra fields.
            */
        public static readonly UnicodeExtraFieldPolicy ALWAYS = new UnicodeExtraFieldPolicy("always");
        /**
            * Never create Unicode extra fields.
            */
        public static readonly UnicodeExtraFieldPolicy NEVER = new UnicodeExtraFieldPolicy("never");
        /**
            * Create Unicode extra fields for filenames that cannot be
            * encoded using the specified encoding.
            */
        public static readonly UnicodeExtraFieldPolicy NOT_ENCODEABLE =
            new UnicodeExtraFieldPolicy("not encodeable");

        private readonly String name;
        private UnicodeExtraFieldPolicy(String n) {
            name = n;
        }
        public String toString() {
            return name;
        }
    }
#endregion
}