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

namespace org.apache.commons.collections.map
{

    /**
     * A case-insensitive <code>Map</code>.
     * <p>
     * As entries are added to the map, keys are converted to all lowercase. A new 
     * key is compared to existing keys by comparing <code>newKey.toString().toLower()</code>
     * to the lowercase values in the current <code>KeySet.</code>
     * <p>
     * Null keys are supported.  
     * <p>
     * The <code>keySet()</code> method returns all lowercase keys, or nulls.
     * <p>
     * Example:
     * <pre><code>
     *  Map map = new CaseInsensitiveMap();
     *  map.put("One", "One");
     *  map.put("Two", "Two");
     *  map.put(null, "Three");
     *  map.put("one", "Four");
     * </code></pre>
     * creates a <code>CaseInsensitiveMap</code> with three entries.<br>
     * <code>map.get(null)</code> returns <code>"Three"</code> and <code>map.get("ONE")</code>
     * returns <code>"Four".</code>  The <code>Set</code> returned by <code>keySet()</code>
     * equals <code>{"one", "two", null}.</code>
     * <p>
     * <strong>Note that CaseInsensitiveMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedMap(Map)}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     *
     * @since Commons Collections 3.0
     * @version $Revision: 7263 $ $Date: 2011-06-12 13:28:27 +0200 (So, 12. Jun 2011) $
     *
     * @author Commons-Collections team
     */
    [Serializable]
    public class CaseInsensitiveMap : AbstractHashedMap, java.io.Serializable, java.lang.Cloneable
    {

        /** Serialisation version */
        private static readonly long serialVersionUID = -7074655917369299456L;

        /**
         * Constructs a new empty map with default size and load factor.
         */
        public CaseInsensitiveMap()
            : base(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR, DEFAULT_THRESHOLD)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity. 
         *
         * @param initialCapacity  the initial capacity
         * @throws IllegalArgumentException if the initial capacity is less than one
         */
        public CaseInsensitiveMap(int initialCapacity)
            : base(initialCapacity)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity and
         * load factor. 
         *
         * @param initialCapacity  the initial capacity
         * @param loadFactor  the load factor
         * @throws IllegalArgumentException if the initial capacity is less than one
         * @throws IllegalArgumentException if the load factor is less than zero
         */
        public CaseInsensitiveMap(int initialCapacity, float loadFactor)
            : base(initialCapacity, loadFactor)
        {
        }

        /**
         * Constructor copying elements from another map.
         * <p>
         * Keys will be converted to lower case strings, which may cause
         * some entries to be removed (if string representation of keys differ
         * only by character case).
         *
         * @param map  the map to copy
         * @throws NullPointerException if the map is null
         */
        public CaseInsensitiveMap(java.util.Map<Object, Object> map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Overrides convertKey() from {@link AbstractHashedMap} to convert keys to 
         * lower case.
         * <p>
         * Returns null if key is null.
         * 
         * @param key  the key convert
         * @return the converted key
         */
        protected override Object convertKey(Object key)
        {
            if (key != null)
            {
                return key.toString().ToLower();
            }
            else
            {
                return AbstractHashedMap.NULL;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Clones the map without cloning the keys or values.
         *
         * @return a shallow clone
         */
        public new Object clone()
        {
            return base.clone();
        }

        /**
         * Write the map out using a custom routine.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            doWriteObject(outJ);
        }

        /**
         * Read the map in using a custom routine.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            doReadObject(inJ);
        }

    }
}