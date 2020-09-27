/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

using org.apache.commons.compress.archivers;

namespace org.apache.commons.compress.utils {

    /**
     * Generic Archive utilities
     */
    public class ArchiveUtils {
    
        /** Private constructor to prevent instantiation of this utility class. */
        private ArchiveUtils(){    
        }

        /**
         * Generates a string containing the name, isDirectory setting and size of an entry.
         * <p>
         * For example:<br/>
         * <tt>-    2000 main.c</tt><br/>
         * <tt>d     100 testfiles</tt><br/>
         * 
         * @return the representation of the entry
         */
        public static String toString(ArchiveEntry entry){
            StringBuilder sb = new StringBuilder();
            sb.Append(entry.isDirectory()? 'd' : '-');// c.f. "ls -l" output
            String size = entry.getSize().toString();
            sb.Append(' ');
            // Pad output to 7 places, leading spaces
            for(int i=7; i > size.length(); i--){
                sb.Append(' ');
            }
            sb.Append(size);
            sb.Append(' ').Append(entry.getName());
            return sb.toString();
        }

        /**
         * Check if buffer contents matches Ascii String.
         * 
         * @param expected
         * @param buffer
         * @param offset
         * @param length
         * @return <code>true</code> if buffer is the same as the expected string
         */
        public static bool matchAsciiBuffer(
                String expected, byte[] buffer, int offset, int length){
            byte[] buffer1;
            try {
                buffer1 = expected.getBytes("ASCII");
            } catch (java.io.UnsupportedEncodingException e) {
                throw new java.lang.RuntimeException(e); // Should not happen
            }
            return isEqual(buffer1, 0, buffer1.Length, buffer, offset, length, false);
        }
    
        /**
         * Check if buffer contents matches Ascii String.
         * 
         * @param expected
         * @param buffer
         * @return <code>true</code> if buffer is the same as the expected string
         */
        public static bool matchAsciiBuffer(String expected, byte[] buffer){
            return matchAsciiBuffer(expected, buffer, 0, buffer.Length);
        }
    
        /**
         * Convert a string to Ascii bytes.
         * Used for comparing "magic" strings which need to be independent of the default Locale.
         * 
         * @param inputString
         * @return the bytes
         */
        public static byte[] toAsciiBytes(String inputString){
            try {
                return inputString.getBytes("ASCII");
            } catch (java.io.UnsupportedEncodingException e) {
               throw new java.lang.RuntimeException(e); // Should never happen
            }
        }

        /**
         * Convert an input byte array to a String using the ASCII character set.
         * 
         * @param inputBytes
         * @return the bytes, interpreted as an Ascii string
         */
        public static String toAsciiString(byte[] inputBytes){
            return System.Text.ASCIIEncoding.ASCII.GetString (inputBytes);
        }

        /**
         * Convert an input byte array to a String using the ASCII character set.
         * 
         * @param inputBytes input byte array
         * @param offset offset within array
         * @param length length of array
         * @return the bytes, interpreted as an Ascii string
         */
        public static String toAsciiString(byte[] inputBytes, int offset, int length){
            return System.Text.ASCIIEncoding.ASCII.GetString (inputBytes,offset,length);
        }

        /**
         * Compare byte buffers, optionally ignoring trailing nulls
         * 
         * @param buffer1
         * @param offset1
         * @param length1
         * @param buffer2
         * @param offset2
         * @param length2
         * @param ignoreTrailingNulls
         * @return <code>true</code> if buffer1 and buffer2 have same contents, having regard to trailing nulls
         */
        public static bool isEqual(
                byte[] buffer1, int offset1, int length1,
                byte[] buffer2, int offset2, int length2,
                bool ignoreTrailingNulls){
            int minLen=length1 < length2 ? length1 : length2;
            for (int i=0; i < minLen; i++){
                if (buffer1[offset1+i] != buffer2[offset2+i]){
                    return false;
                }
            }
            if (length1 == length2){
                return true;
            }
            if (ignoreTrailingNulls){
                if (length1 > length2){
                    for(int i = length2; i < length1; i++){
                        if (buffer1[offset1+i] != 0){
                            return false;
                        }
                    }
                } else {
                    for(int i = length1; i < length2; i++){
                        if (buffer2[offset2+i] != 0){
                            return false;
                        }
                    }                
                }
                return true;
            }
            return false;
        }
    
        /**
         * Compare byte buffers
         * 
         * @param buffer1
         * @param offset1
         * @param length1
         * @param buffer2
         * @param offset2
         * @param length2
         * @return <code>true</code> if buffer1 and buffer2 have same contents
         */
        public static bool isEqual(
                byte[] buffer1, int offset1, int length1,
                byte[] buffer2, int offset2, int length2){
            return isEqual(buffer1, offset1, length1, buffer2, offset2, length2, false);
        }
    
        /**
         * Compare byte buffers
         * 
         * @param buffer1
         * @param buffer2
         * @return <code>true</code> if buffer1 and buffer2 have same contents
         */
        public static bool isEqual(byte[] buffer1, byte[] buffer2 ){
            return isEqual(buffer1, 0, buffer1.Length, buffer2, 0, buffer2.Length, false);
        }
    
        /**
         * Compare byte buffers, optionally ignoring trailing nulls
         * 
         * @param buffer1
         * @param buffer2
         * @param ignoreTrailingNulls
         * @return <code>true</code> if buffer1 and buffer2 have same contents
         */
        public static bool isEqual(byte[] buffer1, byte[] buffer2, bool ignoreTrailingNulls){
            return isEqual(buffer1, 0, buffer1.Length, buffer2, 0, buffer2.Length, ignoreTrailingNulls);
        }
    
        /**
         * Compare byte buffers, ignoring trailing nulls
         * 
         * @param buffer1
         * @param offset1
         * @param length1
         * @param buffer2
         * @param offset2
         * @param length2
         * @return <code>true</code> if buffer1 and buffer2 have same contents, having regard to trailing nulls
         */
        public static bool isEqualWithNull(
                byte[] buffer1, int offset1, int length1,
                byte[] buffer2, int offset2, int length2){
            return isEqual(buffer1, offset1, length1, buffer2, offset2, length2, true);
        }
    
    }
}