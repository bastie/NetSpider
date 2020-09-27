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
using org.apache.commons.collections;

namespace org.apache.commons.collections.bag
{

    /**
     * Decorates another <code>Bag</code> to validate that elements added
     * are of a specific type.
     * <p>
     * The validation of additions is performed via an is test against 
     * a specified <code>Class</code>. If an object cannot be added to the
     * collection, an IllegalArgumentException is thrown.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7140 $ $Date: 2011-06-04 21:12:48 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author Stephen Colebourne
     * @author Matthew Hawthorne
     */
    public class TypedBag
    {

        /**
         * Factory method to create a typed bag.
         * <p>
         * If there are any elements already in the bag being decorated, they
         * are validated.
         * 
         * @param bag  the bag to decorate, must not be null
         * @param type  the type to allow into the bag, must not be null
         * @return a new typed Bag
         * @throws IllegalArgumentException if bag or type is null
         * @throws IllegalArgumentException if the bag contains invalid elements
         */
        public static Bag decorate(Bag bag, java.lang.Class type)
        {
            return new PredicatedBag(bag, InstanceofPredicate.getInstance(type));
        }

        /**
         * Restrictive constructor.
         */
        protected TypedBag()
            : base()
        {
        }

    }
}