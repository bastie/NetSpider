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

namespace org.apache.commons.collections.functors
{

    /**
     * Transformer implementation that returns the result of calling
     * <code>String.valueOf</code> on the input object.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7136 $ $Date: 2011-06-04 21:03:12 +0200 (Sa, 04. Jun 2011) $
     *
     * @author Stephen Colebourne
     */
    public sealed class StringValueTransformer : Transformer, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = 7511110693171758606L;

        /** Singleton predicate instance */
        public static readonly Transformer INSTANCE = new StringValueTransformer();

        /**
         * Factory returning the singleton instance.
         * 
         * @return the singleton instance
         * @since Commons Collections 3.1
         */
        public static Transformer getInstance()
        {
            return INSTANCE;
        }

        /**
         * Restricted constructor.
         */
        private StringValueTransformer()
            : base()
        {
        }

        /**
         * Transforms the input to result by calling <code>String.valueOf</code>.
         * 
         * @param input  the input object to transform
         * @return the transformed result
         */
        public Object transform(Object input)
        {
            return java.lang.StringJ.valueOf(input);
        }

    }
}