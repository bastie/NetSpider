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
using org.apache.commons.collections.functors;

namespace org.apache.commons.collections.collection
{

    /**
     * Decorates a <code>Collection</code> to validate that elements added are of a specific type.
     * <p>
     * The validation of additions is performed via an is test against 
     * a specified <code>Class</code>. If an object cannot be added to the
     * collection, an IllegalArgumentException is thrown.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7137 $ $Date: 2011-06-04 21:07:32 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author Stephen Colebourne
     * @author Matthew Hawthorne
     */
    public class TypedCollection
    {

        /**
         * Factory method to create a typed collection.
         * <p>
         * If there are any elements already in the collection being decorated, they
         * are validated.
         * 
         * @param coll  the collection to decorate, must not be null
         * @param type  the type to allow into the collection, must not be null
         * @return a new typed collection
         * @throws IllegalArgumentException if collection or type is null
         * @throws IllegalArgumentException if the collection contains invalid elements
         */
        public static java.util.Collection<Object> decorate(java.util.Collection<Object> coll, java.lang.Class type)
        {
            return new PredicatedCollection(coll, InstanceofPredicate.getInstance(type));
        }

        /**
         * Restrictive constructor.
         */
        protected TypedCollection()
            : base()
        {
        }

    }
}