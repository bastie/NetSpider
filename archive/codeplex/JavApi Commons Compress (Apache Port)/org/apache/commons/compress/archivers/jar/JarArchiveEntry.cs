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
using org.apache.commons.compress.archivers.zip;

namespace org.apache.commons.compress.archivers.jar{

    /**
     *
     * @NotThreadSafe
     */
    public class JarArchiveEntry : ZipArchiveEntry, ArchiveEntry {

        private java.util.jar.Attributes manifestAttributes = null;
        private java.security.cert.Certificate[] certificates = null;

        public JarArchiveEntry(java.util.zip.ZipEntry entry) : base (entry) 
        {
        }

        public JarArchiveEntry(String name) : base (name){
        }

        public JarArchiveEntry(ZipArchiveEntry entry) : base (entry) //throws ZipException 
        {
        }

        public JarArchiveEntry(java.util.jar.JarEntry entry) : base (entry) // throws ZipException 
        {
        }

        public java.util.jar.Attributes getManifestAttributes() {
            return manifestAttributes;
        }

        public java.security.cert.Certificate[] getCertificates() {
                if (certificates != null) {
                    java.security.cert.Certificate[] certs = new java.security.cert.Certificate[certificates.Length];
                    java.lang.SystemJ.arraycopy(certificates, 0, certs, 0, certs.Length);
                    return certs;
                }
                return null;
        }

    }
}