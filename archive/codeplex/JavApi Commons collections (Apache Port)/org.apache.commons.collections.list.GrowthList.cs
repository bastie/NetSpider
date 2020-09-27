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

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to make it seamlessly grow when
     * indices larger than the list size are used on add and set,
     * avoiding most IndexOutOfBoundsExceptions.
     * <p>
     * This class avoids errors by growing when a set or add method would
     * normally throw an IndexOutOfBoundsException.
     * Note that IndexOutOfBoundsException IS returned for invalid negative indices.
     * <p>
     * Trying to set or add to an index larger than the size will cause the list
     * to grow (using <code>null</code> elements). Clearly, care must be taken
     * not to use excessively large indices, as the internal list will grow to
     * match.
     * <p>
     * Trying to use any method other than add or set with an invalid index will
     * call the underlying list and probably result in an IndexOutOfBoundsException.
     * <p>
     * Take care when using this list with <code>null</code> values, as
     * <code>null</code> is the value added when growing the list.
     * <p>
     * All sub-lists will access the underlying list directly, and will throw
     * IndexOutOfBoundsExceptions.
     * <p>
     * This class differs from {@link LazyList} because here growth occurs on
     * set and add, where <code>LazyList</code> grows on get. However, they
     * can be used together by decorating twice.
     *
     * @see LazyList
     * @since Commons Collections 3.2
     * @version $Revision: 7263 $ $Date: 2011-06-12 13:28:27 +0200 (So, 12. Jun 2011) $
     *
     * @author Stephen Colebourne
     * @author Paul Legato
     */
    [Serializable]
    public class GrowthList : AbstractSerializableListDecorator
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -3620001881672L;

        /**
         * Factory method to create a growth list.
         *
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list)
        {
            return new GrowthList(list);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that uses an ArrayList internally.
         */
        public GrowthList()
            : base(new java.util.ArrayList<Object>())
        {
        }

        /**
         * Constructor that uses an ArrayList internally.
         *
         * @param initialSize  the initial size of the ArrayList
         * @throws IllegalArgumentException if initial size is invalid
         */
        public GrowthList(int initialSize)
            : base(new java.util.ArrayList<Object>(initialSize))
        {
        }

        /**
         * Constructor that wraps (not copies).
         *
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected GrowthList(java.util.List<Object> list)
            : base(list)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Decorate the add method to perform the growth behaviour.
         * <p>
         * If the requested index is greater than the current size, the list will
         * grow to the new size. Indices between the old size and the requested
         * size will be filled with <code>null</code>.
         * <p>
         * If the index is less than the current size, the value will be added to
         * the underlying list directly.
         * If the index is less than zero, the underlying list is called, which
         * will probably throw an IndexOutOfBoundsException.
         *
         * @param index  the index to add at
         * @param element  the object to add at the specified index
         * @throws UnsupportedOperationException if the underlying list doesn't implement set
         * @throws ClassCastException if the underlying list rejects the element
         * @throws IllegalArgumentException if the underlying list rejects the element
         */
        public override void add(int index, Object element)
        {
            int size = getList().size();
            if (index > size)
            {
                getList().addAll(java.util.Collections<Object>.nCopies<Object>(index - size, null));
            }
            getList().add(index, element);
        }

        //-----------------------------------------------------------------------
        /**
         * Decorate the addAll method to perform the growth behaviour.
         * <p>
         * If the requested index is greater than the current size, the list will
         * grow to the new size. Indices between the old size and the requested
         * size will be filled with <code>null</code>.
         * <p>
         * If the index is less than the current size, the values will be added to
         * the underlying list directly.
         * If the index is less than zero, the underlying list is called, which
         * will probably throw an IndexOutOfBoundsException.
         *
         * @param index  the index to add at
         * @param coll  the collection to add at the specified index
         * @return true if the list changed
         * @throws UnsupportedOperationException if the underlying list doesn't implement set
         * @throws ClassCastException if the underlying list rejects the element
         * @throws IllegalArgumentException if the underlying list rejects the element
         */
        public override bool addAll(int index, java.util.Collection<Object> coll)
        {
            int size = getList().size();
            bool result = false;
            if (index > size)
            {
                getList().addAll(java.util.Collections<Object>.nCopies<Object>(index - size, null));
                result = true;
            }
            return (getList().addAll(index, coll) | result);
        }

        //-----------------------------------------------------------------------
        /**
         * Decorate the set method to perform the growth behaviour.
         * <p>
         * If the requested index is greater than the current size, the list will
         * grow to the new size. Indices between the old size and the requested
         * size will be filled with <code>null</code>.
         * <p>
         * If the index is less than the current size, the value will be set onto
         * the underlying list directly.
         * If the index is less than zero, the underlying list is called, which
         * will probably throw an IndexOutOfBoundsException.
         *
         * @param index  the index to set
         * @param element  the object to set at the specified index
         * @return the object previously at that index
         * @throws UnsupportedOperationException if the underlying list doesn't implement set
         * @throws ClassCastException if the underlying list rejects the element
         * @throws IllegalArgumentException if the underlying list rejects the element
         */
        public override Object set(int index, Object element)
        {
            int size = getList().size();
            if (index >= size)
            {
                getList().addAll(java.util.Collections<Object>.nCopies<Object>((index - size) + 1, null));
            }
            return getList().set(index, element);
        }
    }
}