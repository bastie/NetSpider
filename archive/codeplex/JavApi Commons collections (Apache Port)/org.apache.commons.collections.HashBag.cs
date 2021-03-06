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
 */

using System;
using java = biz.ritter.javapi;

namespace org.apache.commons.collections
{

    /**
     * A {@link Bag} that is backed by a {@link HashMap}.
     *
     * @deprecated Moved to bag subpackage and rewritten internally. Due to be removed in v4.0.
     * @since Commons Collections 2.0
     * @version $Revision: 7148 $ $Date: 2011-06-05 14:51:23 +0200 (So, 05. Jun 2011) $
     * 
     * @author Chuck Burdick
     */
    [Obsolete]
    public class HashBag : DefaultMapBag, Bag
    {

        /**
         * Constructs an empty <Code>HashBag</Code>.
         */
        public HashBag()
            : base(new java.util.HashMap<Object, Object>())
        {
        }

        /**
         * Constructs a {@link Bag} containing all the members of the given
         * collection.
         * 
         * @param coll  a collection to copy into this bag
         */
        public HashBag(java.util.Collection<Object> coll)
            : this()
        {
            addAll(coll);
        }

    }
}