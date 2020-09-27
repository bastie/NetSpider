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

namespace org.apache.commons.pool { 

    /**
     * A base implementation of <code>PoolableObjectFactory</code>.
     * <p>
     * All operations defined here are essentially no-op's.
     *
     * @param <T> the type of objects held in this pool
     * 
     * @see PoolableObjectFactory
     * @see BaseKeyedPoolableObjectFactory
     *
     * @author Rodney Waldhoff
     * @version $Revision: 1222388 $ $Date: 2011-12-22 13:28:27 -0500 (Thu, 22 Dec 2011) $
     * @since Pool 1.0
     */
    public abstract class BasePoolableObjectFactory<T> : PoolableObjectFactory<T>
    {

        /**
         * {@inheritDoc}
         */
        public abstract T makeObject();// throws Exception;

        /**
         *  No-op.
         *  
         *  @param obj ignored
         */
        public void destroyObject(T obj)
        {
            //throws Exception  {
        }

        /**
         * This implementation always returns <tt>true</tt>.
         * 
         * @param obj ignored
         * @return <tt>true</tt>
         */
        public bool validateObject(T obj)
        {
            return true;
        }

        /**
         *  No-op.
         *  
         *  @param obj ignored
         */
        public void activateObject(T obj)
        {// throws Exception {
        }

        /**
         *  No-op.
         *  
         * @param obj ignored
         */
        public void passivateObject(T obj)
        {
            //throws Exception {
        }
    }
}