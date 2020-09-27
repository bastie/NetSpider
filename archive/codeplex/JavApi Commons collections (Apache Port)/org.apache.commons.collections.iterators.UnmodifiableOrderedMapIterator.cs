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
namespace org.apache.commons.collections.iterators
{

    /** 
     * Decorates an ordered map iterator such that it cannot be modified.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7135 $ $Date: 2011-06-04 20:58:43 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableOrderedMapIterator : OrderedMapIterator, Unmodifiable
    {

        /** The iterator being decorated */
        private OrderedMapIterator iterator;

        //-----------------------------------------------------------------------
        /**
         * Decorates the specified iterator such that it cannot be modified.
         *
         * @param iterator  the iterator to decorate
         * @throws IllegalArgumentException if the iterator is null
         */
        public static OrderedMapIterator decorate(OrderedMapIterator iterator)
        {
            if (iterator == null)
            {
                throw new java.lang.IllegalArgumentException("OrderedMapIterator must not be null");
            }
            if (iterator is Unmodifiable)
            {
                return iterator;
            }
            return new UnmodifiableOrderedMapIterator(iterator);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor.
         *
         * @param iterator  the iterator to decorate
         */
        private UnmodifiableOrderedMapIterator(OrderedMapIterator iterator)
            : base()
        {
            this.iterator = iterator;
        }

        //-----------------------------------------------------------------------
        public bool hasNext()
        {
            return iterator.hasNext();
        }

        public Object next()
        {
            return iterator.next();
        }

        public bool hasPrevious()
        {
            return iterator.hasPrevious();
        }

        public Object previous()
        {
            return iterator.previous();
        }

        public Object getKey()
        {
            return iterator.getKey();
        }

        public Object getValue()
        {
            return iterator.getValue();
        }

        public Object setValue(Object value)
        {
            throw new java.lang.UnsupportedOperationException("setValue() is not supported");
        }

        public void remove()
        {
            throw new java.lang.UnsupportedOperationException("remove() is not supported");
        }

    }
}