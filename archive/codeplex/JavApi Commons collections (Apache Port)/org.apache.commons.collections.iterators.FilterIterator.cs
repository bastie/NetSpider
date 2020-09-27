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
     * Decorates another {@link Iterator} using a predicate to filter elements.
     * <p>
     * This iterator decorates the underlying iterator, only allowing through
     * those elements that match the specified {@link Predicate Predicate}.
     *
     * @since Commons Collections 1.0
     * @version $Revision: 7135 $ $Date: 2011-06-04 20:58:43 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author James Strachan
     * @author Jan Sorensen
     * @author Ralph Wagner
     * @author Stephen Colebourne
     */
    public class FilterIterator : java.util.Iterator<Object>
    {

        /** The iterator being used */
        private java.util.Iterator<Object> iterator;
        /** The predicate being used */
        private Predicate predicate;
        /** The next object in the iteration */
        private Object nextObject;
        /** Whether the next object has been calculated yet */
        private bool nextObjectSet = false;

        //-----------------------------------------------------------------------
        /**
         * Constructs a new <code>FilterIterator</code> that will not function
         * until {@link #setIterator(Iterator) setIterator} is invoked.
         */
        public FilterIterator()
            : base()
        {
        }

        /**
         * Constructs a new <code>FilterIterator</code> that will not function
         * until {@link #setPredicate(Predicate) setPredicate} is invoked.
         *
         * @param iterator  the iterator to use
         */
        public FilterIterator(java.util.Iterator<Object> iterator)
            : base()
        {
            this.iterator = iterator;
        }

        /**
         * Constructs a new <code>FilterIterator</code> that will use the
         * given iterator and predicate.
         *
         * @param iterator  the iterator to use
         * @param predicate  the predicate to use
         */
        public FilterIterator(java.util.Iterator<Object> iterator, Predicate predicate)
            : base()
        {
            this.iterator = iterator;
            this.predicate = predicate;
        }

        //-----------------------------------------------------------------------
        /** 
         * Returns true if the underlying iterator contains an object that 
         * matches the predicate.
         *
         * @return true if there is another object that matches the predicate
         * @throws NullPointerException if either the iterator or predicate are null
         */
        public bool hasNext()
        {
            if (nextObjectSet)
            {
                return true;
            }
            else
            {
                return setNextObject();
            }
        }

        /** 
         * Returns the next object that matches the predicate.
         *
         * @return the next object which matches the given predicate
         * @throws NullPointerException if either the iterator or predicate are null
         * @throws NoSuchElementException if there are no more elements that
         *  match the predicate 
         */
        public Object next()
        {
            if (!nextObjectSet)
            {
                if (!setNextObject())
                {
                    throw new java.util.NoSuchElementException();
                }
            }
            nextObjectSet = false;
            return nextObject;
        }

        /**
         * Removes from the underlying collection of the base iterator the last
         * element returned by this iterator.
         * This method can only be called
         * if <code>next()</code> was called, but not after
         * <code>hasNext()</code>, because the <code>hasNext()</code> call
         * changes the base iterator.
         *
         * @throws IllegalStateException if <code>hasNext()</code> has already
         *  been called.
         */
        public void remove()
        {
            if (nextObjectSet)
            {
                throw new java.lang.IllegalStateException("remove() cannot be called");
            }
            iterator.remove();
        }

        //-----------------------------------------------------------------------
        /** 
         * Gets the iterator this iterator is using.
         *
         * @return the iterator
         */
        public java.util.Iterator<Object> getIterator()
        {
            return iterator;
        }

        /** 
         * Sets the iterator for this iterator to use.
         * If iteration has started, this effectively resets the iterator.
         *
         * @param iterator  the iterator to use
         */
        public void setIterator(java.util.Iterator<Object> iterator)
        {
            this.iterator = iterator;
            nextObject = null;
            nextObjectSet = false;
        }

        //-----------------------------------------------------------------------
        /** 
         * Gets the predicate this iterator is using.
         *
         * @return the predicate
         */
        public Predicate getPredicate()
        {
            return predicate;
        }

        /** 
         * Sets the predicate this the iterator to use.
         *
         * @param predicate  the predicate to use
         */
        public void setPredicate(Predicate predicate)
        {
            this.predicate = predicate;
            nextObject = null;
            nextObjectSet = false;
        }

        //-----------------------------------------------------------------------
        /**
         * Set nextObject to the next object. If there are no more 
         * objects then return false. Otherwise, return true.
         */
        private bool setNextObject()
        {
            while (iterator.hasNext())
            {
                Object obj = iterator.next();
                if (predicate.evaluate(obj))
                {
                    nextObject = obj;
                    nextObjectSet = true;
                    return true;
                }
            }
            return false;
        }

    }
}