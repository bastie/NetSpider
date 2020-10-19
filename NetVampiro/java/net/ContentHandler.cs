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

namespace biz.ritter.javapi.net
{
    public abstract class ContentHandler 
    {
        public abstract Object getContent(URLConnection con);

        public Object getContent (URLConnection con, java.lang.Class [] neededTypeOfContent) {
          Object o = this.getContent(con);
          foreach (java.lang.Class cls in neededTypeOfContent) {
            
            if (Type.GetTypeFromHandle(Type.GetTypeHandle(cls)).Equals (
                Type.GetTypeFromHandle(Type.GetTypeHandle(o)))) {
              return o;
            }
          }
          return null;
        }
    }
}
