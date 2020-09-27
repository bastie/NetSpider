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
using org.apache.commons.compress.archivers;

namespace org.apache.commons.compress.changes {

    /**
     * Change holds meta information about a change.
     * 
     * @Immutable
     */
    internal class Change {
        private readonly String targetFileJ; // entry name to delete
        private readonly ArchiveEntry entry; // new entry to add
        private readonly java.io.InputStream input; // source for new entry
        private readonly bool replaceMode; // change should replaceMode existing entries

        // Type of change
        private readonly int typeJ;
        // Possible type values
        protected internal static readonly int TYPE_DELETE = 1;
        protected internal static readonly int TYPE_ADD = 2;
        protected internal static readonly int TYPE_MOVE = 3; // NOT USED
        protected internal static readonly int TYPE_DELETE_DIR = 4;

        /**
         * Constructor. Takes the filename of the file to be deleted
         * from the stream as argument.
         * @param pFilename the filename of the file to delete
         */
        internal protected Change(String pFilename, int type) {
            if(pFilename == null) {
                throw new java.lang.NullPointerException();
            }
            this.targetFileJ = pFilename;
            this.typeJ = type;
            this.input = null;
            this.entry = null;
            this.replaceMode = true;
        }

        /**
         * Construct a change which adds an entry.
         * 
         * @param pEntry the entry details
         * @param pInput the InputStream for the entry data
         */
        internal protected Change(ArchiveEntry pEntry, java.io.InputStream pInput, bool replace) {
            if(pEntry == null || pInput == null) {
                throw new java.lang.NullPointerException();
            }
            this.entry = pEntry;
            this.input = pInput;
            typeJ = TYPE_ADD;
            targetFileJ = null;
            this.replaceMode = replace;
        }

        internal protected ArchiveEntry getEntry()
        {
            return entry;
        }

        internal protected java.io.InputStream getInput()
        {
            return input;
        }

        internal protected String targetFile()
        {
            return targetFileJ;
        }

        internal protected int type()
        {
            return typeJ;
        }

        internal protected bool isReplaceMode()
        {
            return replaceMode;
        }
    }
}