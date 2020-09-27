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
using org.apache.commons.compress.archivers.ar;
using org.apache.commons.compress.archivers.cpio;
using org.apache.commons.compress.archivers.jar;
using org.apache.commons.compress.archivers.tar;
using org.apache.commons.compress.archivers.zip;

namespace org.apache.commons.compress.archivers {


    /**
     * <p>Factory to create Archive[In|Out]putStreams from names or the first bytes of
     * the InputStream. In order add other implementations you should extend
     * ArchiveStreamFactory and override the appropriate methods (and call their
     * implementation from super of course).</p>
     * 
     * Compressing a ZIP-File:
     * 
     * <pre>
     * readonly OutputStream out = new FileOutputStream(output); 
     * ArchiveOutputStream os = new ArchiveStreamFactory().createArchiveOutputStream(ArchiveStreamFactory.ZIP, out);
     * 
     * os.putArchiveEntry(new ZipArchiveEntry("testdata/test1.xml"));
     * IOUtils.copy(new FileInputStream(file1), os);
     * os.closeArchiveEntry();
     *
     * os.putArchiveEntry(new ZipArchiveEntry("testdata/test2.xml"));
     * IOUtils.copy(new FileInputStream(file2), os);
     * os.closeArchiveEntry();
     * os.close();
     * </pre>
     * 
     * Decompressing a ZIP-File:
     * 
     * <pre>
     * readonly InputStream is = new FileInputStream(input); 
     * ArchiveInputStream in = new ArchiveStreamFactory().createArchiveInputStream(ArchiveStreamFactory.ZIP, is);
     * ZipArchiveEntry entry = (ZipArchiveEntry)in.getNextEntry();
     * OutputStream out = new FileOutputStream(new File(dir, entry.getName()));
     * IOUtils.copy(in, out);
     * out.close();
     * in.close();
     * </pre>
     * 
     * @Immutable
     */
    public class ArchiveStreamFactory {

        /**
         * Constant used to identify the AR archive format.
         * @since Commons Compress 1.1
         */
        public static readonly String AR = "ar";
        /**
         * Constant used to identify the CPIO archive format.
         * @since Commons Compress 1.1
         */
        public static readonly String CPIO = "cpio";
        /**
         * Constant used to identify the JAR archive format.
         * @since Commons Compress 1.1
         */
        public static readonly String JAR = "jar";
        /**
         * Constant used to identify the TAR archive format.
         * @since Commons Compress 1.1
         */
        public static readonly String TAR = "tar";
        /**
         * Constant used to identify the ZIP archive format.
         * @since Commons Compress 1.1
         */
        public static readonly String ZIP = "zip";

        /**
         * Create an archive input stream from an archiver name and an input stream.
         * 
         * @param archiverName the archive name, i.e. "ar", "zip", "tar", "jar" or "cpio"
         * @param in the input stream
         * @return the archive input stream
         * @throws ArchiveException if the archiver name is not known
         * @throws IllegalArgumentException if the archiver name or stream is null
         */
        public ArchiveInputStream createArchiveInputStream(
                String archiverName, java.io.InputStream inJ)
                //throws ArchiveException 
                {
        
            if (archiverName == null) {
                throw new java.lang.IllegalArgumentException("Archivername must not be null.");
            }
        
            if (inJ == null) {
                throw new java.lang.IllegalArgumentException("InputStream must not be null.");
            }

            if (AR.equalsIgnoreCase(archiverName)) {
                return new ArArchiveInputStream(inJ);
            }
            if (ZIP.equalsIgnoreCase(archiverName)) {
                return new ZipArchiveInputStream(inJ);
            }
            if (TAR.equalsIgnoreCase(archiverName)) {
                return new TarArchiveInputStream(inJ);
            }
            if (JAR.equalsIgnoreCase(archiverName)) {
                return new JarArchiveInputStream(inJ);
            }
            if (CPIO.equalsIgnoreCase(archiverName)) {
                return new CpioArchiveInputStream(inJ);
            }
        
            throw new ArchiveException("Archiver: " + archiverName + " not found.");
        }

        /**
         * Create an archive output stream from an archiver name and an input stream.
         * 
         * @param archiverName the archive name, i.e. "ar", "zip", "tar", "jar" or "cpio"
         * @param out the output stream
         * @return the archive output stream
         * @throws ArchiveException if the archiver name is not known
         * @throws IllegalArgumentException if the archiver name or stream is null
         */
        public ArchiveOutputStream createArchiveOutputStream(String archiverName, java.io.OutputStream outJ)
                //throws ArchiveException 
                {
            if (archiverName == null) {
                throw new java.lang.IllegalArgumentException("Archivername must not be null.");
            }
            if (outJ == null) {
                throw new java.lang.IllegalArgumentException("OutputStream must not be null.");
            }

            if (AR.equalsIgnoreCase(archiverName)) {
                return new ArArchiveOutputStream(outJ);
            }
            if (ZIP.equalsIgnoreCase(archiverName)) {
                return new ZipArchiveOutputStream(outJ);
            }
            if (TAR.equalsIgnoreCase(archiverName)) {
                return new TarArchiveOutputStream(outJ);
            }
            if (JAR.equalsIgnoreCase(archiverName)) {
                return new JarArchiveOutputStream(outJ);
            }
            if (CPIO.equalsIgnoreCase(archiverName)) {
                return new CpioArchiveOutputStream(outJ);
            }
            throw new ArchiveException("Archiver: " + archiverName + " not found.");
        }

        /**
         * Create an archive input stream from an input stream, autodetecting
         * the archive type from the first few bytes of the stream. The InputStream
         * must support marks, like BufferedInputStream.
         * 
         * @param in the input stream
         * @return the archive input stream
         * @throws ArchiveException if the archiver name is not known
         * @throws IllegalArgumentException if the stream is null or does not support mark
         */
        public ArchiveInputStream createArchiveInputStream(java.io.InputStream inJ)
                //throws ArchiveException 
                {
            if (inJ == null) {
                throw new java.lang.IllegalArgumentException("Stream must not be null.");
            }

            if (!inJ.markSupported()) {
                throw new java.lang.IllegalArgumentException("Mark is not supported.");
            }

            byte[] signature = new byte[12];
            inJ.mark(signature.Length);
            try {
                int signatureLength = inJ.read(signature);
                inJ.reset();
                if (ZipArchiveInputStream.matches(signature, signatureLength)) {
                    return new ZipArchiveInputStream(inJ);
                } else if (JarArchiveInputStream.matches(signature, signatureLength)) {
                    return new JarArchiveInputStream(inJ);
                } else if (ArArchiveInputStream.matches(signature, signatureLength)) {
                    return new ArArchiveInputStream(inJ);
                } else if (CpioArchiveInputStream.matches(signature, signatureLength)) {
                    return new CpioArchiveInputStream(inJ);
                }
                // Tar needs a bigger buffer to check the signature; read the first block
                byte[] tarheader = new byte[512];
                inJ.mark(tarheader.Length);
                signatureLength = inJ.read(tarheader);
                inJ.reset();
                if (TarArchiveInputStream.matches(tarheader, signatureLength)) {
                    return new TarArchiveInputStream(inJ);
                }
            } catch (java.io.IOException e) {
                throw new ArchiveException("Could not use reset and mark operations.", e);
            }

            throw new ArchiveException("No Archiver found for the stream signature");
        }
    }
}