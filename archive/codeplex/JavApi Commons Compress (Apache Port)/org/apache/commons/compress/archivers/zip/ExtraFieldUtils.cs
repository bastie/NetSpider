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
     * ZipExtraField related methods
     * @NotThreadSafe because the HashMap is not synch.
     */
    // CheckStyle:HideUtilityClassConstructorCheck OFF (bc)
    public class ExtraFieldUtils {

        private static readonly int WORD = 4;

        /**
         * Static registry of known extra fields.
         */
        private static readonly java.util.Map<Object,Object> implementations;

        static ExtraFieldUtils() {
            implementations = new java.util.HashMap<Object,Object>();
            register(typeof (AsiExtraField));
            register(typeof (JarMarker));
            register(typeof (UnicodePathExtraField));
            register(typeof (UnicodeCommentExtraField));
        }

        /**
         * Register a ZipExtraField implementation.
         *
         * <p>The given class must have a no-arg constructor and implement
         * the {@link ZipExtraField ZipExtraField interface}.</p>
         * @param c the class to register
         */
        public static void register(Type c) {
            try {
                ZipExtraField ze = (ZipExtraField) Activator.CreateInstance(c);
                implementations.put(ze.getHeaderId(), c);
            } catch (InvalidCastException ) {
                throw new java.lang.RuntimeException(c + " doesn\'t implement ZipExtraField");
            } catch (MethodAccessException ) {
                throw new java.lang.RuntimeException(c + "\'s no-arg constructor is not public");
            }
            catch (MemberAccessException )
            {
                throw new java.lang.RuntimeException(c + " is not a concrete class");
            }
        }

        /**
         * Create an instance of the approriate ExtraField, falls back to
         * {@link UnrecognizedExtraField UnrecognizedExtraField}.
         * @param headerId the header identifier
         * @return an instance of the appropiate ExtraField
         * @exception InstantiationException if unable to instantiate the class
         * @exception IllegalAccessException if not allowed to instatiate the class
         */
        public static ZipExtraField createExtraField(ZipShort headerId)
            //throws InstantiationException, IllegalAccessException 
            {
            Type c = (Type) implementations.get(headerId);
            if (c != null) {
                return (ZipExtraField) Activator.CreateInstance(c);
            }
            UnrecognizedExtraField u = new UnrecognizedExtraField();
            u.setHeaderId(headerId);
            return u;
        }

        /**
         * Split the array into ExtraFields and populate them with the
         * given data as local file data, throwing an exception if the
         * data cannot be parsed.
         * @param data an array of bytes as it appears in local file data
         * @return an array of ExtraFields
         * @throws ZipException on error
         */
        public static ZipExtraField[] parse(byte[] data) //throws ZipException
        {
            return parse(data, true, UnparseableExtraField.THROW);
        }

        /**
         * Split the array into ExtraFields and populate them with the
         * given data, throwing an exception if the data cannot be parsed.
         * @param data an array of bytes
         * @param local whether data originates from the local file data
         * or the central directory
         * @return an array of ExtraFields
         * @throws ZipException on error
         */
        public static ZipExtraField[] parse(byte[] data, bool local)
            //throws ZipException 
            {
            return parse(data, local, UnparseableExtraField.THROW);
        }

        /**
         * Split the array into ExtraFields and populate them with the
         * given data.
         * @param data an array of bytes
         * @param local whether data originates from the local file data
         * or the central directory
         * @param onUnparseableData what to do if the extra field data
         * cannot be parsed.
         * @return an array of ExtraFields
         * @throws ZipException on error
         *
         * @since Apache Commons Compress 1.1
         */
        public static ZipExtraField[] parse(byte[] data, bool local,
                                            UnparseableExtraField onUnparseableData)
            //throws ZipException 
            {
                java.util.List<ZipExtraField> v = new java.util.ArrayList<ZipExtraField>();
            int start = 0;
            LOOP:
            while (start <= data.Length - WORD) {
                ZipShort headerId = new ZipShort(data, start);
                int length = (new ZipShort(data, start + 2)).getValue();
                if (start + WORD + length > data.Length) {
                    switch(onUnparseableData.getKey()) {
                    case UnparseableExtraField.THROW_KEY:
                        throw new java.util.zip.ZipException("bad extra field starting at "
                                               + start + ".  Block length of "
                                               + length + " bytes exceeds remaining"
                                               + " data of "
                                               + (data.Length - start - WORD)
                                               + " bytes.");
                    case UnparseableExtraField.READ_KEY:
                        UnparseableExtraFieldData field =
                            new UnparseableExtraFieldData();
                        if (local) {
                            field.parseFromLocalFileData(data, start,
                                                         data.Length - start);
                        } else {
                            field.parseFromCentralDirectoryData(data, start,
                                                                data.Length - start);
                        }
                        v.add(field);
#region case UnparseableExtraField.SKIP_KEY:
                        goto LOOP;
#endregion
                        //$FALL-THROUGH$
                    case UnparseableExtraField.SKIP_KEY:
                        // since we cannot parse the data we must assume
                        // the extra field consumes the whole rest of the
                        // available data
                        goto LOOP; // Basties Note: Bad coder!!!
                    default:
                        throw new java.util.zip.ZipException("unknown UnparseableExtraField key: "
                                               + onUnparseableData.getKey());
                    }
                }
                try {
                    ZipExtraField ze = createExtraField(headerId);
                    if (local) {
                        ze.parseFromLocalFileData(data, start + WORD, length);
                    } else {
                        ze.parseFromCentralDirectoryData(data, start + WORD,
                                                         length);
                    }
                    v.add(ze);
                } catch (MethodAccessException iae) {
                    throw new java.util.zip.ZipException(iae.getMessage());
                }
                catch (MemberAccessException ie)
                {
                    throw new java.util.zip.ZipException(ie.getMessage());
                }
                start += (length + WORD);
            }

            ZipExtraField[] result = new ZipExtraField[v.size()];
            return (ZipExtraField[]) v.toArray(result);
        }

        /**
         * Merges the local file data fields of the given ZipExtraFields.
         * @param data an array of ExtraFiles
         * @return an array of bytes
         */
        public static byte[] mergeLocalFileDataData(ZipExtraField[] data) {
            bool lastIsUnparseableHolder = data.Length > 0 && data[data.Length - 1] is UnparseableExtraFieldData;
            int regularExtraFieldCount =
                lastIsUnparseableHolder ? data.Length - 1 : data.Length;

            int sum = WORD * regularExtraFieldCount;
            for (int i = 0; i < data.Length; i++) {
                sum += data[i].getLocalFileDataLength().getValue();
            }

            byte[] result = new byte[sum];
            int start = 0;
            for (int i = 0; i < regularExtraFieldCount; i++) {
                java.lang.SystemJ.arraycopy(data[i].getHeaderId().getBytes(),
                                 0, result, start, 2);
                java.lang.SystemJ.arraycopy(data[i].getLocalFileDataLength().getBytes(),
                                 0, result, start + 2, 2);
                byte[] local = data[i].getLocalFileDataData();
                java.lang.SystemJ.arraycopy(local, 0, result, start + WORD, local.Length);
                start += (local.Length + WORD);
            }
            if (lastIsUnparseableHolder) {
                byte[] local = data[data.Length - 1].getLocalFileDataData();
                java.lang.SystemJ.arraycopy(local, 0, result, start, local.Length);
            }
            return result;
        }

        /**
         * Merges the central directory fields of the given ZipExtraFields.
         * @param data an array of ExtraFields
         * @return an array of bytes
         */
        public static byte[] mergeCentralDirectoryData(ZipExtraField[] data) {
            bool lastIsUnparseableHolder = data.Length > 0 && data[data.Length - 1] is UnparseableExtraFieldData;
            int regularExtraFieldCount =
                lastIsUnparseableHolder ? data.Length - 1 : data.Length;

            int sum = WORD * regularExtraFieldCount;
            for (int i = 0; i < data.Length; i++) {
                sum += data[i].getCentralDirectoryLength().getValue();
            }
            byte[] result = new byte[sum];
            int start = 0;
            for (int i = 0; i < regularExtraFieldCount; i++) {
                java.lang.SystemJ.arraycopy(data[i].getHeaderId().getBytes(),
                                 0, result, start, 2);
                java.lang.SystemJ.arraycopy(data[i].getCentralDirectoryLength().getBytes(),
                                 0, result, start + 2, 2);
                byte[] local = data[i].getCentralDirectoryData();
                java.lang.SystemJ.arraycopy(local, 0, result, start + WORD, local.Length);
                start += (local.Length + WORD);
            }
            if (lastIsUnparseableHolder) {
                byte[] local = data[data.Length - 1].getCentralDirectoryData();
                java.lang.SystemJ.arraycopy(local, 0, result, start, local.Length);
            }
            return result;
        }

    }
    /**
     * "enum" for the possible actions to take if the extra field
     * cannot be parsed.
     *
     * @since Apache Commons Compress 1.1
     */
    public sealed class UnparseableExtraField
    {
        /**
         * Key for "throw an exception" action.
         */
        public const int THROW_KEY = 0;
        /**
         * Key for "skip" action.
         */
        public const int SKIP_KEY = 1;
        /**
         * Key for "read" action.
         */
        public const int READ_KEY = 2;

        /**
         * Throw an exception if field cannot be parsed.
         */
        public static readonly UnparseableExtraField THROW
            = new UnparseableExtraField(THROW_KEY);

        /**
         * Skip the extra field entirely and don't make its data
         * available - effectively removing the extra field data.
         */
        public static readonly UnparseableExtraField SKIP
            = new UnparseableExtraField(SKIP_KEY);

        /**
         * Read the extra field data into an instance of {@link
         * UnparseableExtraFieldData UnparseableExtraFieldData}.
         */
        public static readonly UnparseableExtraField READ
            = new UnparseableExtraField(READ_KEY);

        private readonly int key;

        private UnparseableExtraField(int k)
        {
            key = k;
        }

        /**
         * Key of the action to take.
         */
        public int getKey() { return key; }
    }
}