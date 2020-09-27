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

namespace org.apache.commons.collections
{

    /** 
     * Defines a list iterator that can be reset back to an initial state.
     * <p>
     * This interface allows an iterator to be repeatedly reused.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7148 $ $Date: 2011-06-05 14:51:23 +0200 (So, 05. Jun 2011) $
     * 
     * @author Stephen Colebourne
     */
    public interface ResettableListIterator : java.util.ListIterator<Object>, ResettableIterator
    {

        /**
         * Resets the iterator back to the position at which the iterator
         * was created.
         */
        //! we do not need declaration, because is in super type
        //void reset();

    }
}