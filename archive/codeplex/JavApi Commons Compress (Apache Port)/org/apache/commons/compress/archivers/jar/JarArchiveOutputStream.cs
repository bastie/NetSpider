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
using java = biz.ritter.javapi;
using org.apache.commons.compress.archivers;
using org.apache.commons.compress.archivers.zip;

namespace org.apache.commons.compress.archivers.jar{
    /**
     * Subclass that adds a special extra field to the very first entry
     * which allows the created archive to be used as an executable jar on
     * Solaris.
     * 
     * @NotThreadSafe
     */
    public class JarArchiveOutputStream : ZipArchiveOutputStream {

        private bool jarMarkerAdded = false;

        public JarArchiveOutputStream(java.io.OutputStream outJ) : base (outJ) {
        }

        // @throws ClassCastException if entry is not an instance of ZipArchiveEntry
        public override void putArchiveEntry(ArchiveEntry ze) //throws IOException 
        {
            if (!jarMarkerAdded) {
                ((ZipArchiveEntry)ze).addAsFirstExtraField(JarMarker.getInstance());
                jarMarkerAdded = true;
            }
            base.putArchiveEntry(ze);
        }
    }
}