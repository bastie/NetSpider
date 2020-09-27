/*
 *  Copyright 2003 jRPM Team
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System; 
using java = biz.ritter.javapi;

namespace com.jguild.jrpm.io
{

    /**
     * RPM Signature.
     */
    public class RPMSignature : Header
    {
        private static readonly java.util.logging.Logger logger = RPMFile.logger;

        /**
             * Creates a new RPMSignature object from an input stream
             * 
             * @param inputStream
             *                The input stream
             * @throws IOException
             *                 if an error occurs on reading informations out of the
             *                 stream
             */
        public RPMSignature(java.io.DataInputStream inputStream, Store store)
            ://throws IOException {
        base(inputStream, store)
        {

            // Make signature size modulo 8 = 0
            long fill = (size % 8L);

            if (fill != 0)
            {
                fill = 8 - fill;
            }

            if (logger.isLoggable(java.util.logging.Level.FINER))
            {
                logger.finer("skip " + fill + " bytes for signature");
            }

            size += inputStream.skip(fill);
        }
    }
}