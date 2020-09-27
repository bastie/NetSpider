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
using org.apache.commons.compress.archivers;

namespace org.apache.commons.compress.archivers {

    /**
     * Simple command line application that lists the contents of an archive.
     *
     * <p>The name of the archive must be given as a command line argument.</p>
     * <p>The optional second argument defines the archive type, in case the format is not recognised.</p>
     *
     * @since Apache Commons Compress 1.1
     */
    public sealed class Lister {
        private static readonly ArchiveStreamFactory factory = new ArchiveStreamFactory();
    
        public static void Main (String[] args) //throws Exception 
        {
            if (args.Length == 0) {
                usage();
                return;
            }
            java.lang.SystemJ.outJ.println("Analysing "+args[0]);
            java.io.File f = new java.io.File(args[0]);
            if (!f.isFile()) {
                java.lang.SystemJ.err.println(f + " doesn't exist or is a directory");
            }
            java.io.InputStream fis = new java.io.BufferedInputStream(new java.io.FileInputStream(f));
            ArchiveInputStream ais;
            if (args.Length > 1) {
                ais = factory.createArchiveInputStream(args[1], fis);
            } else {
                ais = factory.createArchiveInputStream(fis);
            }
            java.lang.SystemJ.outJ.println("Created "+ais.toString());
            ArchiveEntry ae;
            while((ae=ais.getNextEntry()) != null){
                java.lang.SystemJ.outJ.println(ae.getName());
            }
            ais.close();
            fis.close();
        }

        private static void usage() {
            java.lang.SystemJ.outJ.println("Parameters: archive-name [archive-type]");
        
        }

    }
}