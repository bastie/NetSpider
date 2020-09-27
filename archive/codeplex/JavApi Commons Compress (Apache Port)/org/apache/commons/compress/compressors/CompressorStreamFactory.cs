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
using org.apache.commons.compress.compressors.bzip2;
using org.apache.commons.compress.compressors.gzip;

namespace org.apache.commons.compress.compressors {

    /**
     * <p>Factory to create Compressor[In|Out]putStreams from names. To add other
     * implementations you should extend CompressorStreamFactory and override the
     * appropriate methods (and call their implementation from super of course).</p>
     * 
     * Example (Compressing a file):
     * 
     * <pre>
     * readonly OutputStream out = new FileOutputStream(output); 
     * CompressorOutputStream cos = 
     *      new CompressorStreamFactory().createCompressorOutputStream(CompressorStreamFactory.BZIP2, out);
     * IOUtils.copy(new FileInputStream(input), cos);
     * cos.close();
     * </pre>    
     * 
     * Example (Compressing a file):
     * <pre>
     * readonly InputStream is = new FileInputStream(input); 
     * CompressorInputStream in = 
     *      new CompressorStreamFactory().createCompressorInputStream(CompressorStreamFactory.BZIP2, is);
     * IOUtils.copy(in, new FileOutputStream(output));
     * in.close();
     * </pre>
     * 
     * @Immutable
     */
    public class CompressorStreamFactory {

        /**
         * Constant used to identify the BZIP2 compression algorithm.
         * @since Commons Compress 1.1
         */
        public static readonly String BZIP2 = "bzip2";
        /**
         * Constant used to identify the GZIP compression algorithm.
         * @since Commons Compress 1.1
         */
        public static readonly String GZIP = "gz";

        /**
         * Create an compressor input stream from an input stream, autodetecting
         * the compressor type from the first few bytes of the stream. The InputStream
         * must support marks, like BufferedInputStream.
         * 
         * @param in the input stream
         * @return the compressor input stream
         * @throws CompressorException if the compressor name is not known
         * @throws IllegalArgumentException if the stream is null or does not support mark
         * @since Commons Compress 1.1
         */
        public CompressorInputStream createCompressorInputStream(java.io.InputStream inJ)
                //throws CompressorException 
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
            
                if (BZip2CompressorInputStream.matches(signature, signatureLength)) {
                    return new BZip2CompressorInputStream(inJ);
                }
            
                if (GzipCompressorInputStream.matches(signature, signatureLength)) {
                    return new GzipCompressorInputStream(inJ);
                }

            } catch (java.io.IOException e) {
                throw new CompressorException("Failed to detect Compressor from InputStream.", e);
            }

            throw new CompressorException("No Compressor found for the stream signature.");
        }
    
        /**
         * Create a compressor input stream from a compressor name and an input stream.
         * 
         * @param name of the compressor, i.e. "gz" or "bzip2"
         * @param in the input stream
         * @return compressor input stream
         * @throws CompressorException if the compressor name is not known
         * @throws IllegalArgumentException if the name or input stream is null
         */
        public CompressorInputStream createCompressorInputStream(String name, java.io.InputStream inJ)// throws CompressorException 
        {
            if (name == null || inJ == null) {
                throw new java.lang.IllegalArgumentException(
                        "Compressor name and stream must not be null.");
            }

            try {
            
                if (GZIP.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    return new GzipCompressorInputStream(inJ);
                }
            
                if (BZIP2.Equals(name,StringComparison.OrdinalIgnoreCase)) {
                    return new BZip2CompressorInputStream(inJ);
                }
            
            } catch (java.io.IOException e) {
                throw new CompressorException(
                        "Could not create CompressorInputStream.", e);
            }
            throw new CompressorException("Compressor: " + name + " not found.");
        }

        /**
         * Create an compressor output stream from an compressor name and an input stream.
         * 
         * @param name the compressor name, i.e. "gz" or "bzip2"
         * @param out the output stream
         * @return the compressor output stream
         * @throws CompressorException if the archiver name is not known
         * @throws IllegalArgumentException if the archiver name or stream is null
         */
        public CompressorOutputStream createCompressorOutputStream(
                String name, java.io.OutputStream outJ)
                //throws CompressorException 
                {
            if (name == null || outJ == null) {
                throw new java.lang.IllegalArgumentException(
                        "Compressor name and stream must not be null.");
            }

            try {

                if (GZIP.Equals(name,StringComparison.OrdinalIgnoreCase)) {
                    return new GzipCompressorOutputStream(outJ);
                }
            
                if (BZIP2.Equals(name,StringComparison.OrdinalIgnoreCase)) {
                    return new BZip2CompressorOutputStream(outJ);
                }
        
            } catch (java.io.IOException e) {
                throw new CompressorException(
                        "Could not create CompressorOutputStream", e);
            }
            throw new CompressorException("Compressor: " + name + " not found.");
        }
    }
}