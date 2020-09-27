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
using org.apache.commons.collections.map;

namespace org.apache.commons.collections.bidimap
{

    /** 
     * Provides a base decorator that enables additional functionality to be added
     * to a BidiMap via decoration.
     * <p>
     * Methods are forwarded directly to the decorated map.
     * <p>
     * This implementation does not perform any special processing with the map views.
     * Instead it simply returns the set/collection from the wrapped map. This may be
     * undesirable, for example if you are trying to write a validating implementation
     * it would provide a loophole around the validation.
     * But, you might want that loophole, so this class is kept simple.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7174 $ $Date: 2011-06-06 22:24:01 +0200 (Mo, 06. Jun 2011) $
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractBidiMapDecorator
            : AbstractMapDecorator, BidiMap
    {

        /**
         * Constructor that wraps (not copies).
         *
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        protected AbstractBidiMapDecorator(BidiMap map)
            : base(map)
        {
        }

        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected virtual BidiMap getBidiMap()
        {
            return (BidiMap)map;
        }

        //-----------------------------------------------------------------------
        public virtual MapIterator mapIterator()
        {
            return getBidiMap().mapIterator();
        }

        public virtual Object getKey(Object value)
        {
            return getBidiMap().getKey(value);
        }

        public virtual Object removeValue(Object value)
        {
            return getBidiMap().removeValue(value);
        }

        public virtual BidiMap inverseBidiMap()
        {
            return getBidiMap().inverseBidiMap();
        }

    }
}