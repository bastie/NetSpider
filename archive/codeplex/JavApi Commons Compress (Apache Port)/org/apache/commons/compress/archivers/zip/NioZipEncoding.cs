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

namespace org.apache.commons.compress.archivers.zip {

    /**
     * A ZipEncoding, which uses a java.nio {@link
     * java.nio.charset.Charset Charset} to encode names.
     *
     * <p />This implementation works for all cases under java-1.5 or
     * later. However, in java-1.4, some charsets don't have a java.nio
     * implementation, most notably the default ZIP encoding Cp437.
     * 
     * <p />The methods of this class are reentrant.
     * @Immutable
     */
    internal class NioZipEncoding : ZipEncoding {
        private readonly java.nio.charset.Charset charset;

        /**
         * Construct an NIO based zip encoding, which wraps the given
         * charset.
         * 
         * @param charset The NIO charset to wrap.
         */
        public NioZipEncoding(java.nio.charset.Charset charset) {
            this.charset = charset;
        }

        /**
         * @see
         * org.apache.commons.compress.archivers.zip.ZipEncoding#canEncode(java.lang.String)
         */
        public bool canEncode(String name) {
            java.nio.charset.CharsetEncoder enc = this.charset.newEncoder();
            enc.onMalformedInput(java.nio.charset.CodingErrorAction.REPORT);
            enc.onUnmappableCharacter(java.nio.charset.CodingErrorAction.REPORT);

            return enc.canEncode(name);
        }

        /**
         * @see
         * org.apache.commons.compress.archivers.zip.ZipEncoding#encode(java.lang.String)
         */
        public java.nio.ByteBuffer encode(String name) {
            java.nio.charset.CharsetEncoder enc = this.charset.newEncoder();

            enc.onMalformedInput(java.nio.charset.CodingErrorAction.REPORT);
            enc.onUnmappableCharacter(java.nio.charset.CodingErrorAction.REPORT);

            java.nio.CharBuffer cb = java.nio.CharBuffer.wrap(name);
            java.nio.ByteBuffer outJ = java.nio.ByteBuffer.allocate(name.length()
                                                 + (name.length() + 1) / 2);

            while (cb.remaining() > 0) {
                java.nio.charset.CoderResult res = enc.encode(cb, outJ, true);

                if (res.isUnmappable() || res.isMalformed()) {

                    // write the unmappable characters in utf-16
                    // pseudo-URL encoding style to ByteBuffer.
                    if (res.length() * 6 > outJ.remaining()) {
                        outJ = ZipEncodingHelper.growBuffer(outJ, outJ.position()
                                                           + res.length() * 6);
                    }

                    for (int i=0; i<res.length(); ++i) {
                        ZipEncodingHelper.appendSurrogate(outJ,cb.get());
                    }

                } else if (res.isOverflow()) {

                    outJ = ZipEncodingHelper.growBuffer(outJ, 0);

                } else if (res.isUnderflow()) {

                    enc.flush(outJ);
                    break;

                }
            }

            outJ.limit(outJ.position());
            outJ.rewind();
            return outJ;
        }

        /**
         * @see
         * org.apache.commons.compress.archivers.zip.ZipEncoding#decode(byte[])
         */
        public String decode(byte[] data) //throws IOException 
        {
            return this.charset.newDecoder()
                .onMalformedInput(java.nio.charset.CodingErrorAction.REPORT)
                .onUnmappableCharacter(java.nio.charset.CodingErrorAction.REPORT)
                .decode(java.nio.ByteBuffer.wrap(data)).toString();
        }
    }
}