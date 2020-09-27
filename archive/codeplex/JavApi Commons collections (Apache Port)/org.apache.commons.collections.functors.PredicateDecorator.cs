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

namespace org.apache.commons.collections.functors
{

    /**
     * Defines a predicate that decorates one or more other predicates.
     * <p>
     * This interface enables tools to access the decorated predicates.
     * 
     * @since Commons Collections 3.1
     * @version $Revision: 7136 $ $Date: 2011-06-04 21:03:12 +0200 (Sa, 04. Jun 2011) $
     * 
     * @author Stephen Colebourne
     */
    public interface PredicateDecorator : Predicate
    {

        /**
         * Gets the predicates being decorated as an array.
         * <p>
         * The array may be the internal data structure of the predicate and thus
         * should not be altered.
         * 
         * @return the predicates being decorated
         */
        Predicate[] getPredicates();

    }
}