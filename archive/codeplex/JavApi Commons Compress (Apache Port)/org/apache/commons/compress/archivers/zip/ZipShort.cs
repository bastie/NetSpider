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

namespace org.apache.commons.compress.archivers.zip {

    /**
     * Utility class that represents a two byte integer with conversion
     * rules for the big endian byte order of ZIP files.
     * @Immutable
     */
    public sealed class ZipShort : java.lang.Cloneable {
        private static readonly int BYTE_MASK = 0xFF;
        private static readonly int BYTE_1_MASK = 0xFF00;
        private static readonly int BYTE_1_SHIFT = 8;

        private readonly int value;

        /**
         * Create instance from a number.
         * @param value the int to store as a ZipShort
         */
        public ZipShort (int value) {
            this.value = value;
        }

        /**
         * Create instance from bytes.
         * @param bytes the bytes to store as a ZipShort
         */
        public ZipShort (byte[] bytes)  {
            value = ZipShort.getValue(bytes, 0);
        }

        /**
         * Create instance from the two bytes starting at offset.
         * @param bytes the bytes to store as a ZipShort
         * @param offset the offset to start
         */
        public ZipShort (byte[] bytes, int offset) {
            value = ZipShort.getValue(bytes, offset);
        }

        /**
         * Get value as two bytes in big endian byte order.
         * @return the value as a a two byte array in big endian byte order
         */
        public byte[] getBytes() {
            byte[] result = new byte[2];
            result[0] = (byte) (value & BYTE_MASK);
            result[1] = (byte) ((value & BYTE_1_MASK) >> BYTE_1_SHIFT);
            return result;
        }

        /**
         * Get value as Java int.
         * @return value as a Java int
         */
        public int getValue() {
            return value;
        }

        /**
         * Get value as two bytes in big endian byte order.
         * @param value the Java int to convert to bytes
         * @return the converted int as a byte array in big endian byte order
         */
        public static byte[] getBytes(int value) {
            byte[] result = new byte[2];
            result[0] = (byte) (value & BYTE_MASK);
            result[1] = (byte) ((value & BYTE_1_MASK) >> BYTE_1_SHIFT);
            return result;
        }

        /**
         * Helper method to get the value as a java int from two bytes starting at given array offset
         * @param bytes the array of bytes
         * @param offset the offset to start
         * @return the correspondanding java int value
         */
        public static int getValue(byte[] bytes, int offset) {
            int value = (bytes[offset + 1] << BYTE_1_SHIFT) & BYTE_1_MASK;
            value += (bytes[offset] & BYTE_MASK);
            return value;
        }

        /**
         * Helper method to get the value as a java int from a two-byte array
         * @param bytes the array of bytes
         * @return the correspondanding java int value
         */
        public static int getValue(byte[] bytes) {
            return getValue(bytes, 0);
        }

        /**
         * Override to make two instances with same value equal.
         * @param o an object to compare
         * @return true if the objects are equal
         */
        public override bool Equals(Object o) {
            if (o == null || !(o is ZipShort)) {
                return false;
            }
            return value == ((ZipShort) o).getValue();
        }

        /**
         * Override to make two instances with same value equal.
         * @return the value stored in the ZipShort
         */
        public override int GetHashCode() {
            return value;
        }

        public Object clone() {
            try {
                return MemberwiseClone();
                //return base.clone();
            } catch (java.lang.CloneNotSupportedException cnfe) {
                // impossible
                throw new java.lang.RuntimeException(cnfe);
            }
        }
    }
}