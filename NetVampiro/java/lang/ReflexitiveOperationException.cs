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

namespace biz.ritter.javapi.lang
{

    public class ReflexitiveOperationException : Exception
    {
        public ReflexitiveOperationException () : base (){}
        public ReflexitiveOperationException(String message) : base (message){}
        public ReflexitiveOperationException(String message, Throwable cause) : base(message, cause) { }
        public ReflexitiveOperationException(Throwable cause) : base (cause){}
    }
}
