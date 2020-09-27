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

using System;
using java = biz.ritter.javapi;

namespace org.apache.commons.collections
{

    ///<summary>
    /// Defines an iterator that can be reset back to an initial state.
    /// <p />
    /// This interface allows an iterator to be repeatedly reused.
    ///
    /// @since Commons Collections 3.0
    /// @version $Revision: 7263 $ $Date: 2011-06-12 13:28:27 +0200 (So, 12. Jun 2011) $
    /// 
    /// @author Stephen Colebourne
    ///</summary>
    public interface ResettableIterator : java.util.Iterator<Object>
    {

        /**
         * Resets the iterator back to the position at which the iterator
         * was created.
         */
        void reset();

    }
}