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
namespace org.apache.commons.compress.archivers.cpio
{

    /**
     * All constants needed by CPIO.
     * 
     * based on code from the jRPM project (jrpm.sourceforge.net) 
     * 
     */
    public class CpioConstants // Basties note: while only constants is declared - use class
    {
        /** magic number of a cpio entry in the new format */
        public static readonly String MAGIC_NEW = "070701";

        /** magic number of a cpio entry in the new format with crc */
        public static readonly String MAGIC_NEW_CRC = "070702";

        /** magic number of a cpio entry in the old ascii format */
        public static readonly String MAGIC_OLD_ASCII = "070707";

        /** magic number of a cpio entry in the old binary format */
        public static readonly int MAGIC_OLD_BINARY = 070707;

        // These FORMAT_ constants are internal to the code

        /** write/read a CPIOArchiveEntry in the new format */
        public const short FORMAT_NEW = 1;

        /** write/read a CPIOArchiveEntry in the new format with crc */
        public const short FORMAT_NEW_CRC = 2;

        /** write/read a CPIOArchiveEntry in the old ascii format */
        public const short FORMAT_OLD_ASCII = 4;

        /** write/read a CPIOArchiveEntry in the old binary format */
        public const short FORMAT_OLD_BINARY = 8;

        /** Mask for both new formats */
        public static readonly short FORMAT_NEW_MASK = 3;

        /** Mask for both old formats */
        public static readonly short FORMAT_OLD_MASK = 12;

        /*
         * Constants for the MODE bits
         */

        /** Mask for all file type bits. */
        public const int S_IFMT = 0170000;

        // http://www.opengroup.org/onlinepubs/9699919799/basedefs/cpio.h.html
        // has a list of the C_xxx constatnts

        /** Defines a socket */
        public const int C_ISSOCK = 0140000;

        /** Defines a symbolic link */
        public const int C_ISLNK = 0120000;

        /** HP/UX network special (C_ISCTG) */
        public const int C_ISNWK = 0110000;

        /** Defines a regular file */
        public const int C_ISREG = 0100000;

        /** Defines a block device */
        public const int C_ISBLK = 0060000;

        /** Defines a directory */
        public const int C_ISDIR = 0040000;

        /** Defines a character device */
        public const int C_ISCHR = 0020000;

        /** Defines a pipe */
        public const int C_ISFIFO = 0010000;


        /** Set user ID */
        public const int C_ISUID = 0004000;

        /** Set group ID */
        public const int C_ISGID = 0002000;

        /** On directories, restricted deletion flag. */
        public const int C_ISVTX = 0001000;


        /** Permits the owner of a file to read the file */
        public const int C_IRUSR = 0000400;

        /** Permits the owner of a file to write to the file */
        public const int C_IWUSR = 0000200;

        /** Permits the owner of a file to execute the file or to search the directory */
        public const int C_IXUSR = 0000100;


        /** Permits a file's group to read the file */
        public const int C_IRGRP = 0000040;

        /** Permits a file's group to write to the file */
        public const int C_IWGRP = 0000020;

        /** Permits a file's group to execute the file or to search the directory */
        public const int C_IXGRP = 0000010;


        /** Permits others to read the file */
        public const int C_IROTH = 0000004;

        /** Permits others to write to the file */
        public const int C_IWOTH = 0000002;

        /** Permits others to execute the file or to search the directory */
        public const int C_IXOTH = 0000001;


        /** The special trailer marker */
        public const String CPIO_TRAILER = "TRAILER!!!";

        /**
         * The default block size.
         * 
         * @since Apache Commons Compress 1.1
         */
        public static readonly int BLOCK_SIZE = 512;
    }
}