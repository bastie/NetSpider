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
using org.apache.commons.compress.compressors;

namespace org.apache.commons.compress.compressors.gzip {

    public class GzipCompressorOutputStream : CompressorOutputStream {

        private readonly java.util.zip.GZIPOutputStream outJ;

        public GzipCompressorOutputStream( java.io.OutputStream outputStream ) //throws IOException 
        {
            outJ = new java.util.zip.GZIPOutputStream(outputStream);
        }

        /** {@inheritDoc} */
        public override void write(int b)// throws IOException 
        {
            outJ.write(b);
        }

        /**
         * {@inheritDoc}
         * 
         * @since Apache Commons Compress 1.1
         */
        public override void write(byte[] b) //throws IOException 
        {
            outJ.write(b);
        }

        /**
         * {@inheritDoc}
         * 
         * @since Apache Commons Compress 1.1
         */
        public override void write(byte[] b, int from, int length) //throws IOException 
        {
            outJ.write(b, from, length);
        }

        public override void close() //throws IOException 
        {
            outJ.close();
        }

    }
}