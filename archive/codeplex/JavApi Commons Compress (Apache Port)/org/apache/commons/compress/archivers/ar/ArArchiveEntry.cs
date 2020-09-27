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

namespace org.apache.commons.compress.archivers.ar {


    /**
     * Represents an archive entry in the "ar" format.
     * 
     * Each AR archive starts with "!<arch>" followed by a LF. After these 8 bytes
     * the archive entries are listed. The format of an entry header is as it follows:
     * 
     * <pre>
     * START BYTE   END BYTE    NAME                    FORMAT      LENGTH
     * 0            15          File name               ASCII       16
     * 16           27          Modification timestamp  Decimal     12
     * 28           33          Owner ID                Decimal     6
     * 34           39          Group ID                Decimal     6
     * 40           47          File mode               Octal       8
     * 48           57          File size (bytes)       Decimal     10
     * 58           59          File magic              \140\012    2
     * </pre>
     * 
     * This specifies that an ar archive entry header contains 60 bytes.
     * 
     * Due to the limitation of the file name length to 16 bytes GNU and BSD has
     * their own variants of this format. This formats are currently not supported
     * and file names with a bigger size than 16 bytes are not possible at the
     * moment.
     * 
     * @Immutable
     */
    public class ArArchiveEntry : ArchiveEntry {

        /** The header for each entry */
        public static readonly String HEADER = "!<arch>\n";

        /** The trailer for each entry */
        public static readonly String TRAILER = "`\012";

        /**
         * SVR4/GNU adds a trailing / to names; BSD does not.
         * They also vary in how names longer than 16 characters are represented.
         * (Not yet supported by this implementation)
         */
        private readonly String name;
        private readonly int userId;
        private readonly int groupId;
        private readonly int mode;
        private static readonly int DEFAULT_MODE = 33188; // = (octal) 0100644 
        private readonly long lastModified;
        private readonly long length;

        /**
         * Create a new instance using a couple of default values.
         *
         * <p>Sets userId and groupId to 0, the octal file mode to 644 and
         * the last modified time to the current time.</p>
         *
         * @param name name of the entry
         * @param length length of the entry in bytes
         */
        public ArArchiveEntry(String name, long length) {
            this.name = name;
            this.length = length;
            this.userId = 0;
            this.groupId = 0;
            this.mode = DEFAULT_MODE;
            this.lastModified = java.lang.SystemJ.currentTimeMillis() / 1000;
        }

        /**
         * Create a new instance.
         *
         * @param name name of the entry
         * @param length length of the entry in bytes
         * @param userId numeric user id
         * @param groupId numeric group id
         * @param mode file mode
         * @param lastModified last modified time in seconds since the epoch
         */
        public ArArchiveEntry(String name, long length, int userId, int groupId,
                              int mode, long lastModified) {
            this.name = name;
            this.length = length;
            this.userId = userId;
            this.groupId = groupId;
            this.mode = mode;
            this.lastModified = lastModified;
        }

        /**
         * Create a new instance using the attributes of the given file
         */
        public ArArchiveEntry(java.io.File inputFile, String entryName) {
            // TODO sort out mode
            this.name = entryName;
            this.length = inputFile.isFile() ? inputFile.length() : 0;
            this.userId = 0;
            this.groupId = 0;
            this.mode = DEFAULT_MODE;
            this.lastModified = inputFile.lastModified() / 1000;
        }

        /** {@inheritDoc} */
        public long getSize() {
            return this.getLength();
        }

        /** {@inheritDoc} */
        public String getName() {
            return name;
        }

        public int getUserId() {
            return userId;
        }

        public int getGroupId() {
            return groupId;
        }

        public int getMode() {
            return mode;
        }

        /**
         * Last modified time in seconds since the epoch.
         */
        public long getLastModified() {
            return lastModified;
        }

        /** {@inheritDoc} */
        public java.util.Date getLastModifiedDate() {
            return new java.util.Date(1000 * getLastModified());
        }

        public long getLength() {
            return length;
        }

        /** {@inheritDoc} */
        public bool isDirectory() {
            return false;
        }

        /** {@inheritDoc} */
        public int hashCode() {
            int prime = 31;
            int result = 1;
            result = prime * result + ((name == null) ? 0 : name.GetHashCode());
            return result;
        }

        /** {@inheritDoc} */
        public bool equals(Object obj) {
            if (this == obj) {
                return true;
            }
            if (obj == null || this.GetType() != obj.GetType()) {
                return false;
            }
            ArArchiveEntry other = (ArArchiveEntry) obj;
            if (name == null) {
                if (other.name != null) {
                    return false;
                }
            } else if (!name.equals(other.name)) {
                return false;
            }
            return true;
        }
    }
}