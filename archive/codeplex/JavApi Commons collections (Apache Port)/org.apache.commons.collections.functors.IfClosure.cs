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
     * Closure implementation acts as an if statement calling one or other closure
     * based on a predicate.
     * 
     * @since Commons Collections 3.0
     * @version $Revision: 7136 $ $Date: 2011-06-04 21:03:12 +0200 (Sa, 04. Jun 2011) $
     *
     * @author Stephen Colebourne
     * @author Matt Benson
     */
    [Serializable]
    public class IfClosure : Closure, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = 3518477308466486130L;

        /** The test */
        private readonly Predicate iPredicate;
        /** The closure to use if true */
        private readonly Closure iTrueClosure;
        /** The closure to use if false */
        private readonly Closure iFalseClosure;

        /**
         * Factory method that performs validation.
         * <p>
         * This factory creates a closure that performs no action when
         * the predicate is false.
         * 
         * @param predicate  predicate to switch on
         * @param trueClosure  closure used if true
         * @return the <code>if</code> closure
         * @throws IllegalArgumentException if either argument is null
         * @since Commons Collections 3.2
         */
        public static Closure getInstance(Predicate predicate, Closure trueClosure)
        {
            return getInstance(predicate, trueClosure, NOPClosure.INSTANCE);
        }

        /**
         * Factory method that performs validation.
         * 
         * @param predicate  predicate to switch on
         * @param trueClosure  closure used if true
         * @param falseClosure  closure used if false
         * @return the <code>if</code> closure
         * @throws IllegalArgumentException if any argument is null
         */
        public static Closure getInstance(Predicate predicate, Closure trueClosure, Closure falseClosure)
        {
            if (predicate == null)
            {
                throw new java.lang.IllegalArgumentException("Predicate must not be null");
            }
            if (trueClosure == null || falseClosure == null)
            {
                throw new java.lang.IllegalArgumentException("Closures must not be null");
            }
            return new IfClosure(predicate, trueClosure, falseClosure);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * <p>
         * This constructor creates a closure that performs no action when
         * the predicate is false.
         * 
         * @param predicate  predicate to switch on, not null
         * @param trueClosure  closure used if true, not null
         * @since Commons Collections 3.2
         */
        public IfClosure(Predicate predicate, Closure trueClosure) :
            this(predicate, trueClosure, NOPClosure.INSTANCE)
        {
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param predicate  predicate to switch on, not null
         * @param trueClosure  closure used if true, not null
         * @param falseClosure  closure used if false, not null
         */
        public IfClosure(Predicate predicate, Closure trueClosure, Closure falseClosure)
            : base()
        {
            iPredicate = predicate;
            iTrueClosure = trueClosure;
            iFalseClosure = falseClosure;
        }

        /**
         * Executes the true or false closure accoring to the result of the predicate.
         * 
         * @param input  the input object
         */
        public void execute(Object input)
        {
            if (iPredicate.evaluate(input) == true)
            {
                iTrueClosure.execute(input);
            }
            else
            {
                iFalseClosure.execute(input);
            }
        }

        /**
         * Gets the predicate.
         * 
         * @return the predicate
         * @since Commons Collections 3.1
         */
        public Predicate getPredicate()
        {
            return iPredicate;
        }

        /**
         * Gets the closure called when true.
         * 
         * @return the closure
         * @since Commons Collections 3.1
         */
        public Closure getTrueClosure()
        {
            return iTrueClosure;
        }

        /**
         * Gets the closure called when false.
         * 
         * @return the closure
         * @since Commons Collections 3.1
         */
        public Closure getFalseClosure()
        {
            return iFalseClosure;
        }

    }
}