/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 *  Copyright © 2020 Sebastian Ritter
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.nio.fs
{
    ///
    /// <summary>This FileSystem impl. use the underlying .net runtime environment</summary>
    /// <see>https://docs.oracle.com/javase/8/docs/technotes/guides/io/fsp/filesystemprovider.html</see>
    public sealed class DefaultFileSystem : java.nio.file.FileSystem {

        private readonly java.net.URI uri;

        internal DefaultFileSystem () {
            this.uri = new java.net.URI("file:///"+java.lang.SystemJ.getProperty("user.dir"));
        }
        public DefaultFileSystem (java.net.URI newURI) {
            this.uri = newURI;
        }

        public override java.nio.file.Path getPath (String first, params String [] next) {
            String result = first;
            String complete = String.Join(this.getSeparator(),next);
            if (complete.Length>0) {
                result = first + this.getSeparator() + complete;
            }
            // HACK:
            DefaultPath path = new DefaultPath (new java.net.URI("file:///"+complete));
            return path;
        }

        public String getSeparator() {
            // javadoc says: In the case of the default provider, this method returns the same separator as File.separator.
            // javadoc says: This string contains a single character, namely separatorChar.
            // javadoc says: This field is initialized to contain the first character of the value of the system property file.separator
            
            // I say: do not make dependencies from java.nio to java.io, read directly
            return java.lang.SystemJ.getProperty("file.separator");
        }

    }
}
