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
    internal sealed class DefaultPath : java.nio.file.Path {

        private readonly java.net.URI uri;

       public DefaultPath (java.net.URI defaultURI) {
          this.uri = defaultURI;
       }

       public bool isAbsolute () => uri.isAbsolute();


       public String toString() {
           return this.uri.toURL().getFile();
       }

    }
}