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

namespace biz.ritter.javapi.io {

    public interface ObjectInputFilter {
        public enum Status {ALLOWED,REJECTED,UNDECIDED}

        public sealed class Config {
            private static ObjectInputFilter systemWideSerialationFilter = default(ObjectInputFilter);
            public static ObjectInputFilter getSerialFilter() => systemWideSerialationFilter;
            public static void setSerialFilter(ObjectInputFilter newFilter) {
                java.lang.SystemJ.getSecurityManager().checkPermission(java.lang.RuntimePermission.permissionToSerialFilter);
                if (systemWideSerialationFilter != default(ObjectInputFilter)) throw new java.lang.IllegalStateException ("filter has already been set");
                if (null == newFilter) throw new java.lang.NullPointerException (); // javadoc says non-null
                systemWideSerialationFilter = newFilter;
            }

            public static ObjectInputFilter createFilter(String pattern) {
                if (null == pattern) throw new java.lang.NullPointerException (); // javadoc says non-null
                if (pattern.Trim().Length == 0) return null;

                throw new java.lang.UnsupportedOperationException ("not yet implemented");
            }

        }
        public interface FilterInfo {
            
            java.lang.Class serialClass();
            long arrayLength();
            long depth();
            long references();
            long streamBytes();
        }

        Status checkInput (FilterInfo info);

    }
}

