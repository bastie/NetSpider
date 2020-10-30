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
    /// <summary>This FileSystemProvider returns the underlying .net runtime environment specific FileSystem</summary>
    /// <see>https://docs.oracle.com/javase/8/docs/technotes/guides/io/fsp/filesystemprovider.html</see>
    public class DefaultFileSystemProvider : java.nio.file.spi.FileSystemProvider {

        private java.util.HashMap<java.net.URI,java.nio.file.FileSystem> internalCacheToKeepTrackOfFileSystemsCreatedByThisProvider =
            new java.util.HashMap<java.net.URI, java.nio.file.FileSystem>();
        private const String URI_SCHEME_FILE = "file";

        public override String getScheme () => URI_SCHEME_FILE;


        public override java.nio.file.FileSystem getFileSystem (java.net.URI uri, java.util.Map<String,Object> env) {
            this.checkUriPreConditions(uri);
            if (this.internalCacheToKeepTrackOfFileSystemsCreatedByThisProvider.containsKey(uri)) {
                throw new java.nio.file.FileSystemAlreadyExistsException(uri.toString());
            }

            DefaultFileSystem result = new DefaultFileSystem (uri);
            this.internalCacheToKeepTrackOfFileSystemsCreatedByThisProvider.put(uri,result);
            return result;
        }
        public override java.nio.file.FileSystem getFileSystem (java.net.URI uri) {
            // javadoc says the default file system doesn't check permissions
            this.checkUriPreConditions(uri);
            java.nio.file.FileSystem result =this.internalCacheToKeepTrackOfFileSystemsCreatedByThisProvider.get(uri);
            if (null == result) {
                throw new java.nio.file.FileSystemNotFoundException("no file system for URI "+uri);
            }
            return result;
        }

        /// <summary>This method throws a java.lang.IllegalArgumentException if something is suspect.</summary>
        protected void checkUriPreConditions (java.net.URI toCkeck) {
            switch (toCkeck.getScheme().ToLower()) {
                case "file": break;
                case "": break;
            }
            throw new java.lang.IllegalArgumentException(String.Format("URI with scheme {0} not supported.", toCkeck.getScheme()));
        }
    }
    
}