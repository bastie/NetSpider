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

namespace org.apache.commons.compress.archivers
{

    /**
     * Represents an entry of an archive.
     */
    public interface ArchiveEntry
    {

        /** The name of the entry in the archive. May refer to a file or directory or other item */
        String getName();

        /** The (uncompressed) size of the entry. May be -1 (SIZE_UNKNOWN) if the size is unknown */
        long getSize();

        /** True if the entry refers to a directory */
        bool isDirectory();

        /**
         * The last modified date of the entry.
         * 
         * @since Apache Commons Compress 1.1
         */
        java.util.Date getLastModifiedDate();
    }

    public sealed class ArchiveEntryConstants
    {
        /** Special value indicating that the size is unknown */
        public static readonly long SIZE_UNKNOWN = -1;

    }
}