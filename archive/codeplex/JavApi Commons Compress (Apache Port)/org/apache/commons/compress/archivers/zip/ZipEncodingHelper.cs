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
using System.Collections;
using java = biz.ritter.javapi;

namespace org.apache.commons.compress.archivers.zip {
    /**
     * Static helper functions for robustly encoding filenames in zip files. 
     */
    internal abstract class ZipEncodingHelper {


        /**
         * A class, which holds the high characters of a simple encoding
         * and lazily instantiates a Simple8BitZipEncoding instance in a
         * thread-safe manner.
         */

        private static readonly java.util.Map<String, SimpleEncodingHolder> simpleEncodings;

        static ZipEncodingHelper () {
            simpleEncodings = new java.util.HashMap<String, SimpleEncodingHolder>();

            char[] cp437_high_chars =
                new char[] { '\u00c7', '\u00fc', '\u00e9', '\u00e2', '\u00e4', '\u00e0',
                             '\u00e5', '\u00e7', '\u00ea', '\u00eb', '\u00e8', '\u00ef',
                             '\u00ee', '\u00ec', '\u00c4', '\u00c5', '\u00c9', '\u00e6',
                             '\u00c6', '\u00f4', '\u00f6', '\u00f2', '\u00fb', '\u00f9',
                             '\u00ff', '\u00d6', '\u00dc', '\u00a2', '\u00a3', '\u00a5',
                             '\u20a7', '\u0192', '\u00e1', '\u00ed', '\u00f3', '\u00fa',
                             '\u00f1', '\u00d1', '\u00aa', '\u00ba', '\u00bf', '\u2310',
                             '\u00ac', '\u00bd', '\u00bc', '\u00a1', '\u00ab', '\u00bb',
                             '\u2591', '\u2592', '\u2593', '\u2502', '\u2524', '\u2561',
                             '\u2562', '\u2556', '\u2555', '\u2563', '\u2551', '\u2557',
                             '\u255d', '\u255c', '\u255b', '\u2510', '\u2514', '\u2534',
                             '\u252c', '\u251c', '\u2500', '\u253c', '\u255e', '\u255f',
                             '\u255a', '\u2554', '\u2569', '\u2566', '\u2560', '\u2550',
                             '\u256c', '\u2567', '\u2568', '\u2564', '\u2565', '\u2559',
                             '\u2558', '\u2552', '\u2553', '\u256b', '\u256a', '\u2518',
                             '\u250c', '\u2588', '\u2584', '\u258c', '\u2590', '\u2580',
                             '\u03b1', '\u00df', '\u0393', '\u03c0', '\u03a3', '\u03c3',
                             '\u00b5', '\u03c4', '\u03a6', '\u0398', '\u03a9', '\u03b4',
                             '\u221e', '\u03c6', '\u03b5', '\u2229', '\u2261', '\u00b1',
                             '\u2265', '\u2264', '\u2320', '\u2321', '\u00f7', '\u2248',
                             '\u00b0', '\u2219', '\u00b7', '\u221a', '\u207f', '\u00b2',
                             '\u25a0', '\u00a0' };

            SimpleEncodingHolder cp437 = new SimpleEncodingHolder(cp437_high_chars);

            simpleEncodings.put("CP437",cp437);
            simpleEncodings.put("Cp437",cp437);
            simpleEncodings.put("cp437",cp437);
            simpleEncodings.put("IBM437",cp437);
            simpleEncodings.put("ibm437",cp437);

            char[] cp850_high_chars =
                new char[] { '\u00c7', '\u00fc', '\u00e9', '\u00e2', '\u00e4', '\u00e0',
                             '\u00e5', '\u00e7', '\u00ea', '\u00eb', '\u00e8', '\u00ef',
                             '\u00ee', '\u00ec', '\u00c4', '\u00c5', '\u00c9', '\u00e6',
                             '\u00c6', '\u00f4', '\u00f6', '\u00f2', '\u00fb', '\u00f9',
                             '\u00ff', '\u00d6', '\u00dc', '\u00f8', '\u00a3', '\u00d8',
                             '\u00d7', '\u0192', '\u00e1', '\u00ed', '\u00f3', '\u00fa',
                             '\u00f1', '\u00d1', '\u00aa', '\u00ba', '\u00bf', '\u00ae',
                             '\u00ac', '\u00bd', '\u00bc', '\u00a1', '\u00ab', '\u00bb',
                             '\u2591', '\u2592', '\u2593', '\u2502', '\u2524', '\u00c1',
                             '\u00c2', '\u00c0', '\u00a9', '\u2563', '\u2551', '\u2557',
                             '\u255d', '\u00a2', '\u00a5', '\u2510', '\u2514', '\u2534',
                             '\u252c', '\u251c', '\u2500', '\u253c', '\u00e3', '\u00c3',
                             '\u255a', '\u2554', '\u2569', '\u2566', '\u2560', '\u2550',
                             '\u256c', '\u00a4', '\u00f0', '\u00d0', '\u00ca', '\u00cb',
                             '\u00c8', '\u0131', '\u00cd', '\u00ce', '\u00cf', '\u2518',
                             '\u250c', '\u2588', '\u2584', '\u00a6', '\u00cc', '\u2580',
                             '\u00d3', '\u00df', '\u00d4', '\u00d2', '\u00f5', '\u00d5',
                             '\u00b5', '\u00fe', '\u00de', '\u00da', '\u00db', '\u00d9',
                             '\u00fd', '\u00dd', '\u00af', '\u00b4', '\u00ad', '\u00b1',
                             '\u2017', '\u00be', '\u00b6', '\u00a7', '\u00f7', '\u00b8',
                             '\u00b0', '\u00a8', '\u00b7', '\u00b9', '\u00b3', '\u00b2',
                             '\u25a0', '\u00a0' };

            SimpleEncodingHolder cp850 = new SimpleEncodingHolder(cp850_high_chars);

            simpleEncodings.put("CP850",cp850);
            simpleEncodings.put("Cp850",cp850);
            simpleEncodings.put("cp850",cp850);
            simpleEncodings.put("IBM850",cp850);
            simpleEncodings.put("ibm850",cp850);
        }

        /**
         * Grow a byte buffer, so it has a minimal capacity or at least
         * the double capacity of the original buffer 
         * 
         * @param b The original buffer.
         * @param newCapacity The minimal requested new capacity.
         * @return A byte buffer <code>r</code> with
         *         <code>r.capacity() = max(b.capacity()*2,newCapacity)</code> and
         *         all the data contained in <code>b</code> copied to the beginning
         *         of <code>r</code>.
         *
         */
        internal static java.nio.ByteBuffer growBuffer(java.nio.ByteBuffer b, int newCapacity) {
            b.limit(b.position());
            b.rewind();

            int c2 = b.capacity() * 2;
            java.nio.ByteBuffer on = java.nio.ByteBuffer.allocate(c2 < newCapacity ? newCapacity : c2);

            on.put(b);
            return on;
        }

 
        /**
         * The hexadecimal digits <code>0,...,9,A,...,F</code> encoded as
         * ASCII bytes.
         */
        private static readonly byte[] HEX_DIGITS =
            new byte [] {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41,
            0x42, 0x43, 0x44, 0x45, 0x46
        };

        /**
         * Append <code>%Uxxxx</code> to the given byte buffer.
         * The caller must assure, that <code>bb.remaining()&gt;=6</code>.
         * 
         * @param bb The byte buffer to write to.
         * @param c The character to write.
         */
        internal static void appendSurrogate(java.nio.ByteBuffer bb, char c) {

            bb.put((byte) '%');
            bb.put((byte) 'U');

            bb.put(HEX_DIGITS[(c >> 12)&0x0f]);
            bb.put(HEX_DIGITS[(c >> 8)&0x0f]);
            bb.put(HEX_DIGITS[(c >> 4)&0x0f]);
            bb.put(HEX_DIGITS[c & 0x0f]);
        }


        /**
         * name of the encoding UTF-8
         */
        internal static readonly String UTF8 = "UTF8";

        /**
         * variant name of the encoding UTF-8 used for comparisions.
         */
        private static readonly String UTF_DASH_8 = "utf-8";

        /**
         * name of the encoding UTF-8
         */
        protected internal static readonly ZipEncoding UTF8_ZIP_ENCODING = new FallbackZipEncoding(UTF8);

        /**
         * Instantiates a zip encoding.
         * 
         * @param name The name of the zip encoding. Specify <code>null</code> for
         *             the platform's default encoding.
         * @return A zip encoding for the given encoding name.
         */
        protected internal static ZipEncoding getZipEncoding(String name) {
 
            // fallback encoding is good enough for utf-8.
            if (isUTF8(name)) {
                return UTF8_ZIP_ENCODING;
            }

            if (name == null) {
                return new FallbackZipEncoding();
            }

            SimpleEncodingHolder h =
                (SimpleEncodingHolder) simpleEncodings.get(name);

            if (h!=null) {
                return h.getEncoding();
            }

            try {

                java.nio.charset.Charset cs = java.nio.charset.Charset.forName(name);
                return new NioZipEncoding(cs);

            }
            catch (java.nio.charset.UnsupportedCharsetException)
            {
                return new FallbackZipEncoding(name);
            }
        }

        /**
         * Whether a given encoding - or the platform's default encoding
         * if the parameter is null - is UTF-8.
         */
        internal static bool isUTF8(String encoding) {
            if (encoding == null) {
                // check platform's default encoding
                encoding = java.lang.SystemJ.getProperty("file.encoding");
            }
            return UTF8.equalsIgnoreCase(encoding)
                || UTF_DASH_8.equalsIgnoreCase(encoding);
        }
    }
    internal class SimpleEncodingHolder
    {

        private Object lockO = new Object();
        private readonly char[] highChars;
        private Simple8BitZipEncoding encoding;

        /**
         * Instantiate a simple encoding holder.
         * 
         * @param highChars The characters for byte codes 128 to 255.
         * 
         * @see Simple8BitZipEncoding#Simple8BitZipEncoding(char[])
         */
        internal SimpleEncodingHolder(char[] highChars)
        {
            this.highChars = highChars;
        }

        /**
         * @return The associated {@link Simple8BitZipEncoding}, which
         *         is instantiated if not done so far.
         */
        public Simple8BitZipEncoding getEncoding()
        {
            lock (this.lockO)
            {
                if (this.encoding == null)
                {
                    this.encoding = new Simple8BitZipEncoding(this.highChars);
                }
                return this.encoding;
            }
        }
    }
}