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
     * Predicate implementation that transforms the given object before invoking
     * another <code>Predicate</code>.
     * 
     * @since Commons Collections 3.1
     * @version $Revision: 7136 $ $Date: 2011-06-04 21:03:12 +0200 (Sa, 04. Jun 2011) $
     * @author Alban Peignier
     * @author Stephen Colebourne
     */
    public sealed class TransformedPredicate : Predicate, PredicateDecorator, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -5596090919668315834L;

        /** The transformer to call */
        private readonly Transformer iTransformer;
        /** The predicate to call */
        private readonly Predicate iPredicate;

        /**
         * Factory to create the predicate.
         * 
         * @param transformer  the transformer to call
         * @param predicate  the predicate to call with the result of the transform
         * @return the predicate
         * @throws IllegalArgumentException if the transformer or the predicate is null
         */
        public static Predicate getInstance(Transformer transformer, Predicate predicate)
        {
            if (transformer == null)
            {
                throw new java.lang.IllegalArgumentException("The transformer to call must not be null");
            }
            if (predicate == null)
            {
                throw new java.lang.IllegalArgumentException("The predicate to call must not be null");
            }
            return new TransformedPredicate(transformer, predicate);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param transformer  the transformer to use
         * @param predicate  the predicate to decorate
         */
        public TransformedPredicate(Transformer transformer, Predicate predicate)
        {
            iTransformer = transformer;
            iPredicate = predicate;
        }

        /**
         * Evaluates the predicate returning the result of the decorated predicate
         * once the input has been transformed
         * 
         * @param object  the input object which will be transformed
         * @return true if decorated predicate returns true
         */
        public bool evaluate(Object obj)
        {
            Object result = iTransformer.transform(obj);
            return iPredicate.evaluate(result);
        }

        /**
         * Gets the predicate being decorated.
         * 
         * @return the predicate as the only element in an array
         * @since Commons Collections 3.1
         */
        public Predicate[] getPredicates()
        {
            return new Predicate[] { iPredicate };
        }

        /**
         * Gets the transformer in use.
         * 
         * @return the transformer
         */
        public Transformer getTransformer()
        {
            return iTransformer;
        }

    }
}