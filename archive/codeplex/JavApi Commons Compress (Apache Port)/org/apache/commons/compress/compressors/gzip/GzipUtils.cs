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
using System.Collections;
using java = biz.ritter.javapi;

namespace org.apache.commons.compress.compressors.gzip {

    /**
     * Utility code for the gzip compression format.
     * @ThreadSafe
     */
    public class GzipUtils {

        /**
         * Map from common filename suffixes to the suffixes that identify gzipped
         * versions of those file types. For example: from ".tar" to ".tgz".
         */
        private static readonly System.Collections.Hashtable compressSuffix = new System.Collections.Hashtable();

        /**
         * Map from common filename suffixes of gzipped files to the corresponding
         * suffixes of uncompressed files. For example: from ".tgz" to ".tar".
         * <p>
         * This map also contains gzip-specific suffixes like ".gz" and "-z".
         * These suffixes are mapped to the empty string, as they should simply
         * be removed from the filename when the file is uncompressed.
         */
        private static readonly System.Collections.Hashtable uncompressSuffix = new System.Collections.Hashtable();

        static GzipUtils (){
            compressSuffix.Add(".tar", ".tgz");
            compressSuffix.Add(".svg", ".svgz");
            compressSuffix.Add(".cpio", ".cpgz");
            compressSuffix.Add(".wmf", ".wmz");
            compressSuffix.Add(".emf", ".emz");

            uncompressSuffix.Add(".tgz", ".tar");
            uncompressSuffix.Add(".taz", ".tar");
            uncompressSuffix.Add(".svgz", ".svg");
            uncompressSuffix.Add(".cpgz", ".cpio");
            uncompressSuffix.Add(".wmz", ".wmf");
            uncompressSuffix.Add(".emz", ".emf");
            uncompressSuffix.Add(".gz", "");
            uncompressSuffix.Add(".z", "");
            uncompressSuffix.Add("-gz", "");
            uncompressSuffix.Add("-z", "");
            uncompressSuffix.Add("_z", "");
        }
        // N.B. if any shorter or longer keys are added, ensure the for loop limits are changed

        /** Private constructor to prevent instantiation of this utility class. */
        private GzipUtils() {
        }

        /**
         * Detects common gzip suffixes in the given filename.
         *
         * @param filename name of a file
         * @return <code>true</code> if the filename has a common gzip suffix,
         *         <code>false</code> otherwise
         */
        public static bool isCompressedFilename(String filename) {
            String lower = filename.ToLower(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en"));
            int n = lower.length();
            // Shortest suffix is two letters (_z), longest is five (.svgz)
            for (int i = 2; i <= 5 && i < n; i++) {
                if (uncompressSuffix.ContainsKey(lower.Substring(n - i))) {
                    return true;
                }
            }
            return false;
        }

        /**
         * Maps the given name of a gzip-compressed file to the name that the
         * file should have after uncompression. Commonly used file type specific
         * suffixes like ".tgz" or ".svgz" are automatically detected and
         * correctly mapped. For example the name "package.tgz" is mapped to
         * "package.tar". And any filenames with the generic ".gz" suffix
         * (or any other generic gzip suffix) is mapped to a name without that
         * suffix. If no gzip suffix is detected, then the filename is returned
         * unmapped.
         *
         * @param filename name of a file
         * @return name of the corresponding uncompressed file
         */
        public static String getUncompressedFilename(String filename) {
            String lower = filename.ToLower(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en"));
            int n = lower.length();
            // Shortest suffix is two letters (_z), longest is five (.svgz)
            for (int i = 2; i <= 5 && i < n; i++) {
                Object suffix = uncompressSuffix[lower.Substring(n - i)];
                if (suffix != null) {
                    return filename.substring(0, n - i) + suffix;
                }
            }
            return filename;
        }

        /**
         * Maps the given filename to the name that the file should have after
         * compression with gzip. Common file types with custom suffixes for
         * compressed versions are automatically detected and correctly mapped.
         * For example the name "package.tar" is mapped to "package.tgz". If no
         * custom mapping is applicable, then the default ".gz" suffix is appended
         * to the filename.
         *
         * @param filename name of a file
         * @return name of the corresponding compressed file
         */
        public static String getCompressedFilename(String filename) {
            String lower = filename.ToLower(System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en"));
            int n = lower.length();
            // Shortest suffix is four letters (.svg), longest is five (.cpio)
            for (int i = 4; i <= 5 && i < n; i++) {
                Object suffix = compressSuffix[lower.Substring(n - i)];
                if (suffix != null) {
                    return filename.substring(0, n - i) + suffix;
                }
            }
            // No custom suffix found, just append the default .gz
            return filename + ".gz";
        }

    }
}