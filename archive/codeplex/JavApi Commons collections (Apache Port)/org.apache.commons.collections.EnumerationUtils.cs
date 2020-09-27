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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections
{

    /**
     * Provides utility methods for {@link Enumeration} instances.
     * 
     * @since Commons Collections 3.0
     * @version $Id: org.apache.commons.collections.EnumerationUtils.cs 7130 2011-06-04 18:48:14Z unknown $
     * 
     * @author <a href="mailto:ggregory@seagullsw.com">Gary Gregory</a>
     */
    public class EnumerationUtils
    {

        /**
         * EnumerationUtils is not normally instantiated.
         */
        public EnumerationUtils()
        {
            // no init.
        }

        /**
         * Creates a list based on an enumeration.
         * 
         * <p>As the enumeration is traversed, an ArrayList of its values is
         * created. The new list is returned.</p>
         *
         * @param enumeration  the enumeration to traverse, which should not be <code>null</code>.
         * @throws NullPointerException if the enumeration parameter is <code>null</code>.
         */
        public static java.util.List<Object> toList(java.util.Enumeration<Object> enumeration)
        {
            return IteratorUtils.toList(new EnumerationIterator(enumeration));
        }

    }
}