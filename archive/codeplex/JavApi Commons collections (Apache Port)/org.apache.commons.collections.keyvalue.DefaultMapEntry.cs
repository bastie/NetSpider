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
 */

using System;
using java = biz.ritter.javapi;
using org.apache.commons.collections;

namespace org.apache.commons.collections.keyvalue
{

    /**
     * A restricted implementation of {@link java.util.Map.Entry} that prevents
     * the <code>Map.Entry</code> contract from being broken.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7134 $ $Date: 2011-06-04 20:55:45 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author James Strachan
     * @author Michael A. Smith
     * @author Neil O'Toole
     * @author Stephen Colebourne
     */
    public sealed class DefaultMapEntry : AbstractMapEntry
    {

        /**
         * Constructs a new entry with the specified key and given value.
         *
         * @param key  the key for the entry, may be null
         * @param value  the value for the entry, may be null
         */
        public DefaultMapEntry(Object key, Object value)
            : base(key, value)
        {
        }

        /**
         * Constructs a new entry from the specified <code>KeyValue</code>.
         *
         * @param pair  the pair to copy, must not be null
         * @throws NullPointerException if the entry is null
         */
        public DefaultMapEntry(KeyValue pair)
            : base(pair.getKey(), pair.getValue())
        {
        }

        /**
         * Constructs a new entry from the specified <code>Map.Entry</code>.
         *
         * @param entry  the entry to copy, must not be null
         * @throws NullPointerException if the entry is null
         */
        public DefaultMapEntry(java.util.MapNS.Entry<Object, Object> entry)
            : base(entry.getKey(), entry.getValue())
        {
        }

    }
}