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

namespace org.apache.commons.compress.archivers.tar
{

    /**
     * This interface contains all the definitions used in the package.
     *
     */
    // CheckStyle:InterfaceIsTypeCheck OFF (bc)
    public class TarConstants // Basties note: while only const declared - use class
    {

        /**
         * The length of the name field in a header buffer.
         */
        public const int NAMELEN = 100;

        /**
         * The length of the mode field in a header buffer.
         */
        public const int MODELEN = 8;

        /**
         * The length of the user id field in a header buffer.
         */
        public const int UIDLEN = 8;

        /**
         * The length of the group id field in a header buffer.
         */
        public const int GIDLEN = 8;

        /**
         * The length of the checksum field in a header buffer.
         */
        public const int CHKSUMLEN = 8;

        /**
         * The length of the size field in a header buffer.
         * Includes the trailing space or NUL.
         */
        public const int SIZELEN = 12;

        /**
         * The maximum size of a file in a tar archive (That's 11 sevens, octal).
         */
        public const long MAXSIZE = 077777777777L;

        /** Offset of start of magic field within header record */
        public const int MAGIC_OFFSET = 257;
        /**
         * The length of the magic field in a header buffer.
         */
        public const int MAGICLEN = 6;

        /** Offset of start of magic field within header record */
        public const int VERSION_OFFSET = 263;
        /**
         * Previously this was regarded as part of "magic" field, but it is separate.
         */
        public const int VERSIONLEN = 2;

        /**
         * The length of the modification time field in a header buffer.
         */
        public const int MODTIMELEN = 12;

        /**
         * The length of the user name field in a header buffer.
         */
        public const int UNAMELEN = 32;

        /**
         * The length of the group name field in a header buffer.
         */
        public const int GNAMELEN = 32;

        /**
         * The length of each of the device fields (major and minor) in a header buffer.
         */
        public const int DEVLEN = 8;

        /**
         * Length of the prefix field.
         * 
         */
        public const int PREFIXLEN = 155;

        /**
         * LF_ constants represent the "link flag" of an entry, or more commonly,
         * the "entry type". This is the "old way" of indicating a normal file.
         */
        public const byte LF_OLDNORM = 0;

        /**
         * Normal file type.
         */
        public static byte LF_NORMAL = (byte)'0';

        /**
         * Link file type.
         */
        public const byte LF_LINK = (byte)'1';

        /**
         * Symbolic link file type.
         */
        public const byte LF_SYMLINK = (byte)'2';

        /**
         * Character device file type.
         */
        public const byte LF_CHR = (byte)'3';

        /**
         * Block device file type.
         */
        public const byte LF_BLK = (byte)'4';

        /**
         * Directory file type.
         */
        public const byte LF_DIR = (byte)'5';

        /**
         * FIFO (pipe) file type.
         */
        public const byte LF_FIFO = (byte)'6';

        /**
         * Contiguous file type.
         */
        public const byte LF_CONTIG = (byte)'7';

        /**
         * Identifies the *next* file on the tape as having a long name.
         */
        public const byte LF_GNUTYPE_LONGNAME = (byte)'L';

        // See "http://www.opengroup.org/onlinepubs/009695399/utilities/pax.html#tag_04_100_13_02"

        /**
         * Identifies the entry as a Pax extended header.
         * @since Apache Commons Compress 1.1
         */
        public const byte LF_PAX_EXTENDED_HEADER_LC = (byte)'x';

        /**
         * Identifies the entry as a Pax extended header (SunOS tar -E).
         *
         * @since Apache Commons Compress 1.1
         */
        public const byte LF_PAX_EXTENDED_HEADER_UC = (byte)'X';

        /**
         * Identifies the entry as a Pax global extended header.
         *
         * @since Apache Commons Compress 1.1
         */
        public const byte LF_PAX_GLOBAL_EXTENDED_HEADER = (byte)'g';

        /**
         * The magic tag representing a POSIX tar archive.
         */
        public static String MAGIC_POSIX = "ustar\0";
        public static String VERSION_POSIX = "00";

        /**
         * The magic tag representing a GNU tar archive.
         */
        public const String MAGIC_GNU = "ustar ";
        // Appear to be two possible GNU versions
        public const String VERSION_GNU_SPACE = " \0";
        public const String VERSION_GNU_ZERO = "0\0";

        /**
         * The magic tag representing an Ant tar archive.
         *
         * @since Apache Commons Compress 1.1
         */
        public const String MAGIC_ANT = "ustar\0";

        /**
         * The "version" representing an Ant tar archive.
         *
         * @since Apache Commons Compress 1.1
         */
        // Does not appear to have a version, however Ant does write 8 bytes,
        // so assume the version is 2 nulls
        public const String VERSION_ANT = "\0\0";

        /**
         * The name of the GNU tar entry which contains a long name.
         */
        public const String GNU_LONGLINK = "././@LongLink"; // TODO rename as LONGLINK_GNU ?

    }
}