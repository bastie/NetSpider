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

using com.jguild.jrpm.io.datatype;
using org.apache.commons.compress.archivers.cpio;
using org.apache.commons.compress.compressors.bzip2;
using org.apache.commons.compress.compressors.gzip;
using SevenZip.Compression.LZMA;

namespace com.jguild.jrpm.io
{

    /**
     * This class allows IO access to an RPM file.
     *
     * @todo Implement equals()
     */
    public class RPMFile
    {
        public static readonly java.util.logging.Logger logger = java.util.logging.Logger.getLogger("jrpm.io");

        private Header header = null;

        private RPMLead lead = null;

        private Header signature = null;

        private int localePosition;

        private java.io.File rpmFile = null;

        private bool editingRpmFile = false;

        private Store store = new Store();

        /**
         * Creates a new empty RPMFile object.
         */
        public RPMFile()
        {
        }

        /**
         * Creates a new RPMFile object out of a file.
         *
         * @param fh The file object representing a rpm file
         */
        public RPMFile(java.io.File fh)
        {
            rpmFile = fh;
        }

        private void reset()
        {
            lock (this)
            {
                header = null;
                lead = null;
                signature = null;
                editingRpmFile = false;
            }
        }

        /**
         * Set the file this RPMFile should represent
         *
         * @param fh The file object representing a rpm file
         */
        public void setFile(java.io.File fh)
        {
            lock (this)
            {
                if (editingRpmFile)
                {
                    throw new java.lang.IllegalStateException("RPM file is currently edited");
                }
                rpmFile = fh;
                reset();
            }
        }

        /**
         * Parse the RPMFile and will extract all informations. This must be called
         * before any informations can be read from the rpm file.
         *
         * @throws IOException If an error occurs during read of the rpm file
         */
        public void parse()
        {// throws IOException {
            lock (this)
            {
                if (rpmFile == null)
                    throw new java.lang.IllegalStateException("A file must be specified");

                if (!rpmFile.exists())
                    throw new java.lang.IllegalStateException("The specified file does not exist");

                try
                {
                    readFromStream(new java.io.BufferedInputStream(
                            new java.io.FileInputStream(rpmFile), 4096));
                }
                catch (java.io.IOException e)
                {
                    reset();
                    throw e;
                }
                editingRpmFile = true;
            }
        }

        /**
         * Get the header section of this rpm file.
         *
         * @return The rpm header
         */
        public Header getHeader()
        {
            lock (this)
            {
                if (header == null)
                    throw new java.lang.IllegalStateException("There are no header informations");

                return header;
            }
        }

        /**
         * Get all known tags of this rpm file. This is equivalent to the
         * --querytags option in rpm.
         *
         * @return An array of all tag names
         */
        public static String[] getKnownTagNames()
        {
            return Store.getKnownTagNames();
        }

        /**
         * Get the lead section of this rpm file
         *
         * @return The rpm lead
         */
        public RPMLead getLead()
        {
            lock (this)
            {
                if (lead == null)
                    throw new java.lang.IllegalStateException("There are no lead informations");

                return lead;
            }
        }

        /**
         * Set the locale as int for all I18N strings that are returned by getTag().
         * The position has to correspond with the same position in the array
         * returned by getLocales().
         *
         * @param pos The position in the array returned by getLocales().
         */
        public void setLocale(int pos)
        {
            lock (this)
            {
                localePosition = pos;
            }
        }

        /**
         * Set the locale as string for all I18N strings that are returned by
         * getTag(). The string must match with a string returned by getLocales().
         *
         * @param locale A locale matching a locale returned by getLocales()
         * @throws IllegalArgumentException If the locale is not defined by getLocales().
         */
        public void setLocale(String locale)
        {
            lock (this)
            {
                String[] locales = ((STRING_ARRAY)getTag("HEADERI18NTABLE")).getData();

                for (int pos = 0; pos < locales.Length; pos++)
                {
                    if (locales[pos].equals(locale))
                    {
                        setLocale(pos);

                        return;
                    }
                }

                throw new java.lang.IllegalArgumentException("Unknown locale <" + locale + ">");
            }
        }

        /**
         * Return all known locales that are supported by this RPM file. The array
         * is read out of the RPM file with the tag "HEADERI18NTABLE". The RPM has
         * one entry for all I18N strings defined by this tag.
         *
         * @return A string array of all defined locales
         */
        public String[] getLocales()
        {
            lock (this)
            {
                return ((STRING_ARRAY)getTag("HEADERI18NTABLE")).getData();
            }
        }

        /**
         * Get the signature section of this rpm file
         *
         * @return The rpm signature
         */
        public Header getSignature()
        {
            lock (this)
            {
                if (signature == null)
                    throw new java.lang.IllegalStateException(
                            "There are no signature informations");

                return signature;
            }
        }

        /**
         * Get a tag by id as a Long
         *
         * @param tag A tag id as a Long
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(java.lang.Long tag)
        {
            lock (this)
            {
                DataTypeIf data = store.getTag(tag);

                // set the locale for all I18N strings
                if (data is I18NSTRING)
                {
                    ((I18NSTRING)data).setLocaleIndex(localePosition);
                }

                return data;
            }
        }

        /**
         * Get a tag by id as a long
         *
         * @param tag A tag id as a long
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(long tag)
        {
            lock (this)
            {
                return getTag(new java.lang.Long(tag));
            }
        }

        /**
         * Get a tag by name
         *
         * @param tagname A tag name
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(String tagname)
        {
            lock (this)
            {
                return getTag(getTagIdForName(tagname));
            }
        }

        /**
         * Read a tag with a given tag name.
         *
         * @param tagname A RPM tag name
         * @return The id of the RPM tag
         * @throws IllegalArgumentException if the tag name was not found
         */
        public long getTagIdForName(String tagname)
        {
            lock (this)
            {
                return store.getTagIdForName(tagname);
            }
        }

        /**
         * Get all tag ids contained in this rpm file.
         *
         * @return All tag ids contained in this rpm file.
         */
        public long[] getTagIds()
        {
            lock (this)
            {
                return store.getTagIds();
            }
        }

        /**
         * Read a tag with a given tag id.
         *
         * @param tagid A RPM tag id
         * @return The name of the RPM tag
         * @throws IllegalArgumentException if the tag id was not found
         */
        public String getTagNameForId(long tagid)
        {
            lock (this)
            {
                return store.getTagNameForId(tagid);
            }
        }

        /**
         * Get all tag names contained in this rpm file.
         *
         * @return All tag names contained in this rpm file.
         */
        public String[] getTagNames()
        {
            lock (this)
            {
                return store.getTagNames();
            }
        }

        /**
         * Read informations of a rpm file out of an input stream.
         *
         * @param rpmInputStream The input stream representing the rpm file
         * @throws IOException if an error occurs during read of the rpm file
         */
        private void readFromStream(java.io.InputStream rpmInputStream)
        {//throws IOException {
            ByteCountInputStream allCountInputStream = new ByteCountInputStream(
                    rpmInputStream);
            java.io.InputStream inputStream = new java.io.DataInputStream(allCountInputStream);

            lead = new RPMLead((java.io.DataInputStream)inputStream);
            signature = new RPMSignature((java.io.DataInputStream)inputStream, store);

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("Signature Size: " + signature.getSize());
            }

            header = new RPMHeader((java.io.DataInputStream)inputStream, store);

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("Header Size: " + header.getSize());
            }

            DataTypeIf payloadTag = getTag("PAYLOADFORMAT");
            String payloadFormat = payloadTag != null ? payloadTag.toString()
                    : "cpio";
            DataTypeIf payloadCompressionTag = getTag("PAYLOADCOMPRESSOR");
            String payloadCompressor = payloadCompressionTag != null ? payloadCompressionTag
                    .toString()
                    : "gzip";

            if (payloadFormat.equals("cpio"))
            {
                if (logger.isLoggable(java.util.logging.Level.FINER))
                {
                    logger.finer("PAYLOADCOMPRESSOR: " + payloadCompressor);
                }

                if (payloadCompressor.equals("gzip"))
                {
                    inputStream = new GzipCompressorInputStream(allCountInputStream);
                }
                else if (payloadCompressor.equals("bzip2"))
                {
                    inputStream = new BZip2CompressorInputStream(allCountInputStream);
                }
                else if (payloadCompressor.equals("lzma"))
                {
                    try
                    {
                        java.io.PipedOutputStream pout = new java.io.PipedOutputStream();
                        inputStream = new java.io.PipedInputStream(pout);
                        byte[] properties = new byte[5];
                        if (allCountInputStream.read(properties, 0, 5) != 5)
                            throw (new java.io.IOException("input .lzma is too short"));
                        SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
                        decoder.SetDecoderProperties(properties);
                        long outSize = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            int v = allCountInputStream.read();
                            if (v < 0)
                                throw (new java.io.IOException("lzma error : Can't Read 1"));
                            outSize |= ((long)v) << (8 * i);
                        }
                        if (outSize == -1)
                            outSize = java.lang.Long.MAX_VALUE;

                        Decode decoderRunnable = new Decode(decoder,
                                allCountInputStream, pout, outSize);
                        java.lang.Thread t = new java.lang.Thread(decoderRunnable, "LZMA Decoder");
                        t.start();
                    }
                    catch (java.lang.NoClassDefFoundError )
                    {
                        String message = "No LZMA library found. Attach p7zip library to classpath (http://p7zip.sourceforge.net/)";
                        logger.severe(message);
                        throw new java.io.IOException(message);
                    }
                }
                else if (payloadCompressor.equals("none"))
                {
                    inputStream = allCountInputStream;
                }
                else
                {
                    throw new java.io.IOException("Unsupported compressor type "
                            + payloadCompressor);
                }

                ByteCountInputStream countInputStream = new ByteCountInputStream(inputStream);
                CpioArchiveInputStream cpioInputStream = new CpioArchiveInputStream(countInputStream);
                CpioArchiveEntry readEntry;
                java.util.List<String> fileNamesList = new java.util.ArrayList<String>();
                String fileEntry;
                while ((readEntry = cpioInputStream.getNextCPIOEntry()) != null)
                {
                    if (logger.isLoggable(java.util.logging.Level.FINER))
                    {
                        logger.finer("Read CPIO entry: " + readEntry.getName()
                                + " ;mode:" + readEntry.getMode());
                    }
                    if (readEntry.isRegularFile() || readEntry.isSymbolicLink()
                            || readEntry.isDirectory())
                    {
                        fileEntry = readEntry.getName();
                        if (fileEntry.startsWith("./"))
                            fileEntry = fileEntry.substring(1);
                        fileNamesList.add(fileEntry);
                    }
                }
                store.setTag("FILENAMES", TypeFactory.createSTRING_ARRAY((String[])fileNamesList.toArray(new String[fileNamesList.size()])));

                setHeaderTagFromSignature("ARCHIVESIZE", "PAYLOADSIZE");
                // check ARCHIVESIZE with countInputStream.getCount();
                Object archiveSizeObject = getTag("ARCHIVESIZE");
                if (archiveSizeObject != null)
                {
                    if (archiveSizeObject is INT32)
                    {
                        int archiveSize = ((INT32)archiveSizeObject).getData()[0];
                        if (archiveSize != countInputStream.getCount())
                        {
                            new java.io.IOException("ARCHIVESIZE not correct");
                        }
                    }
                }
                store.setTag("J_ARCHIVESIZE", TypeFactory
                        .createINT64(new long[] { countInputStream.getCount() }));
            }
            else
            {
                throw new java.io.IOException("Unsupported Payload type " + payloadFormat);
            }

            // filling in signatures
            // TODO: check signatures!
            setHeaderTagFromSignature("SIGSIZE", "SIZE");
            setHeaderTagFromSignature("SIGLEMD5_1", "LEMD5_1");
            setHeaderTagFromSignature("SIGPGP", "PGP");
            setHeaderTagFromSignature("SIGLEMD5_2", "LEMD5_2");
            setHeaderTagFromSignature("SIGMD5", "MD5");
            setHeaderTagFromSignature("SIGGPG", "GPG");
            setHeaderTagFromSignature("SIGPGP5", "PGP5");
            setHeaderTagFromSignature("DSAHEADER", "DSA");
            setHeaderTagFromSignature("RSAHEADER", "RSA");
            setHeaderTagFromSignature("SHA1HEADER", "SHA1");

            store.setTag("J_FILESIZE", TypeFactory
                    .createINT64(new long[] { allCountInputStream.getCount() }));

            rpmInputStream.close();
        }

        private void setHeaderTagFromSignature(String headerTag, String signatureTag)
        {
            if (store.getTag(headerTag) == null)
                store.setTag(headerTag, store.getTag(signatureTag));
        }

        /**
         * Release locked resources.
         */
        public void close()
        {
            reset();
        }

        /**
         * Same as doing toXML(true).
         *
         * @return String containing the XML representation of this RPM.
         * @see #toXML(bool)
         */
        public String toXML()
        {
            java.io.StringWriter buf = new java.io.StringWriter();
            try
            {
                toXML(buf, true);
                buf.flush();
                return buf.toString();
            }
            catch (java.io.IOException e)
            {
                throw new java.lang.RuntimeException(e);
            }
        }

        /**
         * Returns an XML version of this file
         *
         * @param excludePayload If this is true, the payload will not be included in the XML.
         * @return XML rpm.
         */
        public String toXML(bool excludePayload)
        {
            java.io.StringWriter buf = new java.io.StringWriter();
            try
            {
                toXML(buf, excludePayload);
                buf.flush();
                return buf.toString();
            }
            catch (java.io.IOException e)
            {
                throw new java.lang.RuntimeException(e);
            }
        }

        /**
         * Outputs this rpm in an XML format to the specified i/o writer.
         *
         * @param writer         Writer stream.
         * @param excludePayload If this is true, the payload will not be included in the XML.
         * @throws IOException If an error occurred writing to the writer.
         */
        public void toXML(java.io.Writer writer, bool excludePayload)
        {//throws IOException {
            // TODO
        }

        internal class ByteCountInputStream : java.io.FilterInputStream
        {
            private int count = 0;

            public ByteCountInputStream(java.io.InputStream isJ) :
                base(isJ)
            {
            }

            public int getCount()
            {
                return count;
            }

            public override int read()
            {//throws IOException {
                count++;
                return inJ.read();
            }

            public override int read(byte[] b)
            {//throws IOException {
                int size = read(b, 0, b.Length);
                count += size;
                return size;
            }

            public override int read(byte[] b, int off, int len)
            {// throws IOException {
                int size = inJ.read(b, off, len);
                count += size;
                return size;
            }

            public override long skip(long n)
            {//throws IOException {
                long size = inJ.skip(n);
                count += (int)size;
                return size;
            }
        }

        /**
         * Load an RPM file using the native rpm executables.
         *
         * @param file RPM file.
         */
        public static RPMFile loadUsingNative(java.io.File file)
        {
            return null; // TODO
        }

        sealed class Decode : java.lang.Runnable
        {
            private SevenZip.Compression.LZMA.Decoder decoder;

            private java.io.InputStream inputStream;

            private java.io.OutputStream outputStream;

            private long size;

            public Decode(SevenZip.Compression.LZMA.Decoder decoder, java.io.InputStream inputStream,
                          java.io.OutputStream outputStream, long size)
                : base()
            {
                this.decoder = decoder;
                this.inputStream = inputStream;
                this.outputStream = outputStream;
                this.size = size;
            }

            public void run()
            {
                try
                {
                    decoder.Code(new biz.ritter.io.StreamWrapper(inputStream,null), new biz.ritter.io.StreamWrapper(null,outputStream), size, size,null);
                    outputStream.close();
                }
                catch (java.io.IOException )
                {
                }
                try
                {
                    outputStream.close();
                }
                catch (java.io.IOException )
                {
                }
            }
        }
    }
}