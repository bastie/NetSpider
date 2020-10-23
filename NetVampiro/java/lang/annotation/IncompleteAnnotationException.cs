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

namespace biz.ritter.javapi.lang.annotation
{

    public class IncompleteAnnotationException : RuntimeException
    {
        private readonly Annotation type;
        private readonly String name;
        public IncompleteAnnotationException(Annotation reason, String elementName) : base() { 
            this.type = reason;
            this.name = elementName;
        }

        public String elementName()=> this.name;
        public Annotation annotationType => this.type;
    }
}
