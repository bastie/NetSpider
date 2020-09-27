/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
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

using org.apache.commons.codec;

namespace org.apache.commons.codec.binary
{

    /**
     * Hex encoder and decoder. The charset used for certain operation can be set, the default is set in
     * {@link #DEFAULT_CHARSET_NAME}
     * 
     * @since 1.1
     * @author Apache Software Foundation
     * @version $Id: Hex.java 801639 2009-08-06 13:15:10Z niallp $
     */
    public class Hex : BinaryEncoder, BinaryDecoder
    {

        /**
         * Default charset name is {@link CharEncoding#UTF_8}
         */
        public static readonly String DEFAULT_CHARSET_NAME = CharEncoding.UTF_8;

        /**
         * Used to build output as Hex
         */
        private static readonly char[] DIGITS_LOWER = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        /**
         * Used to build output as Hex
         */
        private static readonly char[] DIGITS_UPPER = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /**
         * Converts an array of characters representing hexadecimal values into an array of bytes of those same values. The
         * returned array will be half the length of the passed array, as it takes two characters to represent any given
         * byte. An exception is thrown if the passed char array has an odd number of elements.
         * 
         * @param data
         *            An array of characters containing hexadecimal digits
         * @return A byte array containing binary data decoded from the supplied char array.
         * @throws DecoderException
         *             Thrown if an odd number or illegal of characters is supplied
         */
        public static byte[] decodeHex(char[] data)
        {// throws DecoderException {

            int len = data.Length;

            if ((len & 0x01) != 0)
            {
                throw new DecoderException("Odd number of characters.");
            }

            byte[] outJ = new byte[len >> 1];

            // two characters form the hex value.
            for (int i = 0, j = 0; j < len; i++)
            {
                int f = toDigit(data[j], j) << 4;
                j++;
                f = f | toDigit(data[j], j);
                j++;
                outJ[i] = (byte)(f & 0xFF);
            }

            return outJ;
        }

        /**
         * Converts an array of bytes into an array of characters representing the hexadecimal values of each byte in order.
         * The returned array will be double the length of the passed array, as it takes two characters to represent any
         * given byte.
         * 
         * @param data
         *            a byte[] to convert to Hex characters
         * @return A char[] containing hexadecimal characters
         */
        public static char[] encodeHex(byte[] data)
        {
            return encodeHex(data, true);
        }

        /**
         * Converts an array of bytes into an array of characters representing the hexadecimal values of each byte in order.
         * The returned array will be double the length of the passed array, as it takes two characters to represent any
         * given byte.
         * 
         * @param data
         *            a byte[] to convert to Hex characters
         * @param toLowerCase
         *            <code>true</code> converts to lowercase, <code>false</code> to uppercase
         * @return A char[] containing hexadecimal characters
         * @since 1.4
         */
        public static char[] encodeHex(byte[] data, bool toLowerCase)
        {
            return encodeHex(data, toLowerCase ? DIGITS_LOWER : DIGITS_UPPER);
        }

        /**
         * Converts an array of bytes into an array of characters representing the hexadecimal values of each byte in order.
         * The returned array will be double the length of the passed array, as it takes two characters to represent any
         * given byte.
         * 
         * @param data
         *            a byte[] to convert to Hex characters
         * @param toDigits
         *            the output alphabet
         * @return A char[] containing hexadecimal characters
         * @since 1.4
         */
        protected static char[] encodeHex(byte[] data, char[] toDigits)
        {
            int l = data.Length;
            char[] outJ = new char[l << 1];
            // two characters form the hex value.
            for (int i = 0, j = 0; i < l; i++)
            {
                outJ[j++] = toDigits[java.dotnet.lang.Operator.shiftRightUnsignet((0xF0 & data[i]), 4)];
                outJ[j++] = toDigits[0x0F & data[i]];
            }
            return outJ;
        }

        /**
         * Converts an array of bytes into a String representing the hexadecimal values of each byte in order. The returned
         * String will be double the length of the passed array, as it takes two characters to represent any given byte.
         * 
         * @param data
         *            a byte[] to convert to Hex characters
         * @return A String containing hexadecimal characters
         * @since 1.4
         */
        public static String encodeHexString(byte[] data)
        {
            return new String(encodeHex(data));
        }

        /**
         * Converts a hexadecimal character to an integer.
         * 
         * @param ch
         *            A character to convert to an integer digit
         * @param index
         *            The index of the character in the source
         * @return An integer
         * @throws DecoderException
         *             Thrown if ch is an illegal hex character
         */
        protected static int toDigit(char ch, int index)
        {// throws DecoderException {
            int digit = java.lang.Character.digit(ch, 16);
            if (digit == -1)
            {
                throw new DecoderException("Illegal hexadecimal charcter " + ch + " at index " + index);
            }
            return digit;
        }

        private readonly String charsetName;

        /**
         * Creates a new codec with the default charset name {@link #DEFAULT_CHARSET_NAME}
         */
        public Hex()
        {
            // use default encoding
            this.charsetName = DEFAULT_CHARSET_NAME;
        }

        /**
         * Creates a new codec with the given charset name.
         * 
         * @param csName
         *            the charset name.
         * @since 1.4
         */
        public Hex(String csName)
        {
            this.charsetName = csName;
        }

        /**
         * Converts an array of character bytes representing hexadecimal values into an array of bytes of those same values.
         * The returned array will be half the length of the passed array, as it takes two characters to represent any given
         * byte. An exception is thrown if the passed char array has an odd number of elements.
         * 
         * @param array
         *            An array of character bytes containing hexadecimal digits
         * @return A byte array containing binary data decoded from the supplied byte array (representing characters).
         * @throws DecoderException
         *             Thrown if an odd number of characters is supplied to this function
         * @see #decodeHex(char[])
         */
        public byte[] decode(byte[] array)
        {// throws DecoderException {
            try
            {
                return decodeHex(new java.lang.StringJ(array, getCharsetName()).ToString().toCharArray());
            }
            catch (java.io.UnsupportedEncodingException e)
            {
                throw new DecoderException(e.getMessage(), e);
            }
        }

        /**
         * Converts a String or an array of character bytes representing hexadecimal values into an array of bytes of those
         * same values. The returned array will be half the length of the passed String or array, as it takes two characters
         * to represent any given byte. An exception is thrown if the passed char array has an odd number of elements.
         * 
         * @param object
         *            A String or, an array of character bytes containing hexadecimal digits
         * @return A byte array containing binary data decoded from the supplied byte array (representing characters).
         * @throws DecoderException
         *             Thrown if an odd number of characters is supplied to this function or the object is not a String or
         *             char[]
         * @see #decodeHex(char[])
         */
        public Object decode(Object obj)
        {// throws DecoderException {
            try
            {
                char[] charArray = obj is String ? ((String)obj).toCharArray() : (char[])obj;
                return decodeHex(charArray);
            }
            catch (java.lang.ClassCastException e)
            {
                throw new DecoderException(e.getMessage(), e);
            }
        }

        /**
         * Converts an array of bytes into an array of bytes for the characters representing the hexadecimal values of each
         * byte in order. The returned array will be double the length of the passed array, as it takes two characters to
         * represent any given byte.
         * <p>
         * The conversion from hexadecimal characters to the returned bytes is performed with the charset named by
         * {@link #getCharsetName()}.
         * </p>
         * 
         * @param array
         *            a byte[] to convert to Hex characters
         * @return A byte[] containing the bytes of the hexadecimal characters
         * @throws IllegalStateException
         *             if the charsetName is invalid. This API throws {@link IllegalStateException} instead of
         *             {@link UnsupportedEncodingException} for backward compatibility.
         * @see #encodeHex(byte[])
         */
        public byte[] encode(byte[] array)
        {
            return StringUtils.getBytesUnchecked(encodeHexString(array), getCharsetName());
        }

        /**
         * Converts a String or an array of bytes into an array of characters representing the hexadecimal values of each
         * byte in order. The returned array will be double the length of the passed String or array, as it takes two
         * characters to represent any given byte.
         * <p>
         * The conversion from hexadecimal characters to bytes to be encoded to performed with the charset named by
         * {@link #getCharsetName()}.
         * </p>
         * 
         * @param object
         *            a String, or byte[] to convert to Hex characters
         * @return A char[] containing hexadecimal characters
         * @throws EncoderException
         *             Thrown if the given object is not a String or byte[]
         * @see #encodeHex(byte[])
         */
        public Object encode(Object obj)
        {//throws EncoderException {
            try
            {
                byte[] byteArray = obj is String ? ((String)obj).getBytes(getCharsetName()) : (byte[])obj;
                return encodeHex(byteArray);
            }
            catch (java.lang.ClassCastException e)
            {
                throw new EncoderException(e.getMessage(), e);
            }
            catch (java.io.UnsupportedEncodingException e)
            {
                throw new EncoderException(e.getMessage(), e);
            }
        }

        /**
         * Gets the charset name.
         * 
         * @return the charset name.
         * @since 1.4
         */
        public String getCharsetName()
        {
            return this.charsetName;
        }

        /**
         * Returns a string representation of the object, which includes the charset name.
         * 
         * @return a string representation of the object.
         */
        public override String ToString()
        {
            return base.ToString() + "[charsetName=" + this.charsetName + "]";
        }
    }
}