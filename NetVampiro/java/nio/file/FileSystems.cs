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

namespace biz.ritter.javapi.nio.file {

    ///
    /// ClassFactory
    ///
    public sealed class FileSystems {

        private static readonly FileSystem defaultFileSystem;
        static FileSystems (){
            String envConfigDefaultFileSystemClass = java.lang.SystemJ.getProperty ("java.nio.file.spi.DefaultFileSystemProvider");
            FileSystem newDefaultFileSystem = default(FileSystem);
            if (null == envConfigDefaultFileSystemClass || envConfigDefaultFileSystemClass.Trim().Equals("")) {
                try {
                    var objectHandle = (FileSystem)
                    Activator.CreateInstanceFrom ("envConfigDefaultFileSystemClass",
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name).Unwrap();
                }
                catch {} // Holy shit...
            }
            if (newDefaultFileSystem == null) { // ...fallback
                newDefaultFileSystem = new dotnet.nio.fs.DefaultFileSystem(new java.net.URI("file:///"+java.lang.SystemJ.getProperty("user.dir")));
            }
            /// CHECKIT: is it better to go over DefaultFileSystemProvider to create an Instance?
            FileSystems.defaultFileSystem = newDefaultFileSystem;
        }

        public static FileSystem getDefault () {
            return FileSystems.defaultFileSystem;
        }

    }
}

