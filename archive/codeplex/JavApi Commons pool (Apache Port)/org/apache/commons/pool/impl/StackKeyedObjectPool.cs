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

namespace org.apache.commons.pool.impl
{

    /**
     * A simple, <code>Stack</code>-based <code>KeyedObjectPool</code> implementation.
     * <p>
     * Given a {@link KeyedPoolableObjectFactory}, this class will maintain
     * a simple pool of instances.  A finite number of "sleeping"
     * or inactive instances is enforced, but when the pool is
     * empty, new instances are created to support the new load.
     * Hence this class places no limit on the number of "active"
     * instances created by the pool, but is quite useful for
     * re-using <code>Object</code>s without introducing
     * artificial limits.
     * </p>
     *
     * @param <K> the type of keys in this pool
     * @param <V> the type of objects held in this pool
     * 
     * @author Rodney Waldhoff
     * @author Sandy McArthur
     * @version $Revision: 1222710 $ $Date: 2011-12-23 10:58:12 -0500 (Fri, 23 Dec 2011) $
     * @see Stack
     * @since Pool 1.0
     */
    public class StackKeyedObjectPool<K, V> : BaseKeyedObjectPool<K, V>, KeyedObjectPool<K, V>
    {
        /**
         * Create a new pool using no factory.
         * Clients must first set the {@link #setFactory factory} or
         * may populate the pool using {@link #returnObject returnObject}
         * before they can be {@link #borrowObject borrowed}.
         *
         * @see #StackKeyedObjectPool(KeyedPoolableObjectFactory)
         * @see #setFactory(KeyedPoolableObjectFactory)
         */
        public StackKeyedObjectPool() :
            this(null, DEFAULT_MAX_SLEEPING, DEFAULT_INIT_SLEEPING_CAPACITY)
        {
        }

        /**
         * Create a new pool using no factory.
         * Clients must first set the {@link #setFactory factory} or
         * may populate the pool using {@link #returnObject returnObject}
         * before they can be {@link #borrowObject borrowed}.
         *
         * @param max cap on the number of "sleeping" instances in the pool
         * @see #StackKeyedObjectPool(KeyedPoolableObjectFactory, int)
         * @see #setFactory(KeyedPoolableObjectFactory)
         */
        public StackKeyedObjectPool(int max) :
            this(null, max, DEFAULT_INIT_SLEEPING_CAPACITY)
        {
        }

        /**
         * Create a new pool using no factory.
         * Clients must first set the {@link #setFactory factory} or
         * may populate the pool using {@link #returnObject returnObject}
         * before they can be {@link #borrowObject borrowed}.
         *
         * @param max cap on the number of "sleeping" instances in the pool
         * @param init initial size of the pool (this specifies the size of the container,
         *             it does not cause the pool to be pre-populated.)
         * @see #StackKeyedObjectPool(KeyedPoolableObjectFactory, int, int)
         * @see #setFactory(KeyedPoolableObjectFactory)
         */
        public StackKeyedObjectPool(int max, int init) :
            this(null, max, init)
        {
        }

        /**
         * Create a new <code>SimpleKeyedObjectPool</code> using
         * the specified <code>factory</code> to create new instances.
         *
         * @param factory the {@link KeyedPoolableObjectFactory} used to populate the pool
         */
        public StackKeyedObjectPool(KeyedPoolableObjectFactory<K, V> factory) :
            this(factory, DEFAULT_MAX_SLEEPING)
        {
        }

        /**
         * Create a new <code>SimpleKeyedObjectPool</code> using
         * the specified <code>factory</code> to create new instances.
         * capping the number of "sleeping" instances to <code>max</code>
         *
         * @param factory the {@link KeyedPoolableObjectFactory} used to populate the pool
         * @param max cap on the number of "sleeping" instances in the pool
         */
        public StackKeyedObjectPool(KeyedPoolableObjectFactory<K, V> factory, int max) :
            this(factory, max, DEFAULT_INIT_SLEEPING_CAPACITY)
        {
        }

        /**
         * Create a new <code>SimpleKeyedObjectPool</code> using
         * the specified <code>factory</code> to create new instances.
         * capping the number of "sleeping" instances to <code>max</code>,
         * and initially allocating a container capable of containing
         * at least <code>init</code> instances.
         *
         * @param factory the {@link KeyedPoolableObjectFactory} used to populate the pool
         * @param max cap on the number of "sleeping" instances in the pool
         * @param init initial size of the pool (this specifies the size of the container,
         *             it does not cause the pool to be pre-populated.)
         */
        public StackKeyedObjectPool(KeyedPoolableObjectFactory<K, V> factory, int max, int init)
        {
            _factory = factory;
            _maxSleeping = (max < 0 ? DEFAULT_MAX_SLEEPING : max);
            _initSleepingCapacity = (init < 1 ? DEFAULT_INIT_SLEEPING_CAPACITY : init);
            _pools = new java.util.HashMap<K, java.util.Stack<V>>();
            _activeCount = new java.util.HashMap<K, java.lang.Integer>();
        }

        /**
         * Borrows an object with the given key.  If there are no idle instances under the
         * given key, a new one is created.
         * 
         * @param key the pool key
         * @return keyed poolable object instance
         */
        public override V borrowObject(K key)
        {//throws Exception {
            lock (this)
            {
                assertOpen();
                java.util.Stack<V> stack = (_pools.get(key));
                if (null == stack)
                {
                    stack = new java.util.Stack<V>();
                    stack.ensureCapacity(_initSleepingCapacity > _maxSleeping ? _maxSleeping : _initSleepingCapacity);
                    _pools.put(key, stack);
                }
                V obj = default(V);
                do
                {
                    bool newlyMade = false;
                    if (!stack.empty())
                    {
                        obj = stack.pop();
                        _totIdle--;
                    }
                    else
                    {
                        if (null == _factory)
                        {
                            throw new java.util.NoSuchElementException("pools without a factory cannot create new objects as needed.");
                        }
                        else
                        {
                            obj = _factory.makeObject(key);
                            newlyMade = true;
                        }
                    }
                    if (null != _factory && null != obj)
                    {
                        try
                        {
                            _factory.activateObject(key, obj);
                            if (!_factory.validateObject(key, obj))
                            {
                                throw new java.lang.Exception("ValidateObject failed");
                            }
                        }
                        catch (java.lang.Throwable t)
                        {
                            PoolUtils.checkRethrow(t);
                            try
                            {
                                _factory.destroyObject(key, obj);
                            }
                            catch (java.lang.Throwable t2)
                            {
                                PoolUtils.checkRethrow(t2);
                                // swallowed
                            }
                            finally
                            {
                                obj = default(V);
                            }
                            if (newlyMade)
                            {
                                throw new java.util.NoSuchElementException(
                                    "Could not create a validated object, cause: " +
                                    t.getMessage());
                            }
                        }
                    }
                } while (obj == null);
                incrementActiveCount(key);
                return obj;
            }
        }

        /**
         * Returns <code>obj</code> to the pool under <code>key</code>.  If adding the
         * returning instance to the pool results in {@link #_maxSleeping maxSleeping}
         * exceeded for the given key, the oldest instance in the idle object pool
         * is destroyed to make room for the returning instance.
         * 
         * @param key the pool key
         * @param obj returning instance
         */

        public override void returnObject(K key, V obj)
        {//throws Exception {
            lock (this)
            {
                decrementActiveCount(key);
                if (null != _factory)
                {
                    if (_factory.validateObject(key, obj))
                    {
                        try
                        {
                            _factory.passivateObject(key, obj);
                        }
                        catch (Exception ex)
                        {
                            _factory.destroyObject(key, obj);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (isClosed())
                {
                    if (null != _factory)
                    {
                        try
                        {
                            _factory.destroyObject(key, obj);
                        }
                        catch (Exception e)
                        {
                            // swallowed
                        }
                    }
                    return;
                }

                java.util.Stack<V> stack = _pools.get(key);
                if (null == stack)
                {
                    stack = new java.util.Stack<V>();
                    stack.ensureCapacity(_initSleepingCapacity > _maxSleeping ? _maxSleeping : _initSleepingCapacity);
                    _pools.put(key, stack);
                }
                int stackSize = stack.size();
                if (stackSize >= _maxSleeping)
                {
                    V staleObj;
                    if (stackSize > 0)
                    {
                        staleObj = stack.remove(0);
                        _totIdle--;
                    }
                    else
                    {
                        staleObj = obj;
                    }
                    if (null != _factory)
                    {
                        try
                        {
                            _factory.destroyObject(key, staleObj);
                        }
                        catch (Exception e)
                        {
                            // swallowed
                        }
                    }
                }
                stack.push(obj);
                _totIdle++;
            }
        }

        /**
         * {@inheritDoc}
         */

        public override void invalidateObject(K key, V obj)
        {//throws Exception {
            lock (this)
            {
                decrementActiveCount(key);
                if (null != _factory)
                {
                    _factory.destroyObject(key, obj);
                }
                this.notifyAll(); // _totalActive has changed
            }

        }

        /**
         * Create an object using the {@link KeyedPoolableObjectFactory#makeObject factory},
         * passivate it, and then placed in the idle object pool.
         * <code>addObject</code> is useful for "pre-loading" a pool with idle objects.
         *
         * @param key the key a new instance should be added to
         * @throws Exception when {@link KeyedPoolableObjectFactory#makeObject} fails.
         * @throws IllegalStateException when no {@link #setFactory factory} has been set or after {@link #close} has been called on this pool.
         */

        public void addObject(K key)
        {//throws Exception {
            lock (this)
            {
                assertOpen();
                if (_factory == null)
                {
                    throw new java.lang.IllegalStateException("Cannot add objects without a factory.");
                }
                V obj = _factory.makeObject(key);
                try
                {
                    if (!_factory.validateObject(key, obj))
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        _factory.destroyObject(key, obj);
                    }
                    catch (Exception e2)
                    {
                        // swallowed
                    }
                    return;
                }
                _factory.passivateObject(key, obj);

                java.util.Stack<V> stack = _pools.get(key);
                if (null == stack)
                {
                    stack = new java.util.Stack<V>();
                    stack.ensureCapacity(_initSleepingCapacity > _maxSleeping ? _maxSleeping : _initSleepingCapacity);
                    _pools.put(key, stack);
                }

                int stackSize = stack.size();
                if (stackSize >= _maxSleeping)
                {
                    V staleObj;
                    if (stackSize > 0)
                    {
                        staleObj = stack.remove(0);
                        _totIdle--;
                    }
                    else
                    {
                        staleObj = obj;
                    }
                    try
                    {
                        _factory.destroyObject(key, staleObj);
                    }
                    catch (Exception e)
                    {
                        // Don't swallow destroying the newly created object.
                        Object o1 = (Object)obj;
                        Object o2 = (Object)staleObj;
                        if (o1 == o2)
                        {
                            throw e;
                        }
                    }
                }
                else
                {
                    stack.push(obj);
                    _totIdle++;
                }
            }
        }

        /**
         * Returns the total number of instances currently idle in this pool.
         *
         * @return the total number of instances currently idle in this pool
         */

        public int getNumIdle()
        {
            lock (this)
            {
                return _totIdle;
            }
        }

        /**
         * Returns the total number of instances current borrowed from this pool but not yet returned.
         *
         * @return the total number of instances currently borrowed from this pool
         */

        public int getNumActive()
        {
            lock (this)
            {
                return _totActive;
            }
        }

        /**
         * Returns the number of instances currently borrowed from but not yet returned
         * to the pool corresponding to the given <code>key</code>.
         *
         * @param key the key to query
         * @return the number of instances corresponding to the given <code>key</code> currently borrowed in this pool
         */

        public int getNumActive(K key)
        {
            lock (this)
            {
                return getActiveCount(key);
            }
        }

        /**
         * Returns the number of instances corresponding to the given <code>key</code> currently idle in this pool.
         *
         * @param key the key to query
         * @return the number of instances corresponding to the given <code>key</code> currently idle in this pool
         */

        public int getNumIdle(K key)
        {
            lock (this)
            {
                try
                {
                    return (_pools.get(key)).size();
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        /**
         * Clears the pool, removing all pooled instances.
         */

        public void clear()
        {
            lock (this)
            {
                java.util.Iterator<K> it = _pools.keySet().iterator();
                while (it.hasNext())
                {
                    K key = it.next();
                    java.util.Stack<V> stack = _pools.get(key);
                    destroyStack(key, stack);
                }
                _totIdle = 0;
                _pools.clear();
                _activeCount.clear();
            }
        }

        /**
         * Clears the specified pool, removing all pooled instances corresponding to the given <code>key</code>.
         *
         * @param key the key to clear
         */

        public void clear(K key)
        {
            lock (this)
            {
                java.util.Stack<V> stack = _pools.remove(key);
                destroyStack(key, stack);
            }
        }

        /**
         * Destroys all instances in the stack and clears the stack.
         * 
         * @param key key passed to factory when destroying instances
         * @param stack stack to destroy
         */
        private void destroyStack(K key, java.util.Stack<V> stack)
        {
            lock (this)
            {
                if (null == stack)
                {
                    return;
                }
                else
                {
                    if (null != _factory)
                    {
                        java.util.Iterator<V> it = stack.iterator();
                        while (it.hasNext())
                        {
                            try
                            {
                                _factory.destroyObject(key, it.next());
                            }
                            catch (Exception e)
                            {
                                // ignore error, keep destroying the rest
                            }
                        }
                    }
                    _totIdle -= stack.size();
                    _activeCount.remove(key);
                    stack.clear();
                }
            }
        }

        /**
         * Returns a string representation of this StackKeyedObjectPool, including
         * the number of pools, the keys and the size of each keyed pool.
         * 
         * @return Keys and pool sizes
         */

        public override String ToString()
        {
            lock (this)
            {
                java.lang.StringBuffer buf = new java.lang.StringBuffer();
                buf.append(this.getClass().getName());
                buf.append(" contains ").append(_pools.size()).append(" distinct pools: ");
                java.util.Iterator<K> it = _pools.keySet().iterator();
                while (it.hasNext())
                {
                    K key = it.next();
                    buf.append(" |").append(key).append("|=");
                    java.util.Stack<V> s = _pools.get(key);
                    buf.append(s.size());
                }
                return buf.toString();
            }
        }

        /**
         * Close this pool, and free any resources associated with it.
         * <p>
         * Calling {@link #addObject addObject} or {@link #borrowObject borrowObject} after invoking
         * this method on a pool will cause them to throw an {@link IllegalStateException}.
         * </p>
         *
         * @throws Exception <strong>deprecated</strong>: implementations should silently fail if not all resources can be freed.
         */

        public void close()
        {// throws Exception {
            base.close();
            clear();
        }

        /**
         * Sets the {@link KeyedPoolableObjectFactory factory} the pool uses
         * to create new instances.
         * Trying to change the <code>factory</code> after a pool has been used will frequently
         * throw an {@link UnsupportedOperationException}.
         *
         * @param factory the {@link KeyedPoolableObjectFactory} used to manage object instances
         * @throws IllegalStateException when the factory cannot be set at this time
         * [Obsolete] to be removed in pool 2.0
         */
        [Obsolete]

        public void setFactory(KeyedPoolableObjectFactory<K, V> factory)
        {// throws IllegalStateException {
            lock (this)
            {
                if (0 < getNumActive())
                {
                    throw new java.lang.IllegalStateException("Objects are already active");
                }
                else
                {
                    clear();
                    _factory = factory;
                }
            }
        }

        /**
         * @return the {@link KeyedPoolableObjectFactory} used by this pool to manage object instances.
         * @since 1.5.5
         */
        public KeyedPoolableObjectFactory<K, V> getFactory()
        {
            lock (this)
            {
                return _factory;
            }
        }

        /**
         * Returns the active instance count for the given key.
         * 
         * @param key pool key
         * @return active count
         */
        private int getActiveCount(K key)
        {
            try
            {
                return _activeCount.get(key).intValue();
            }
            catch (java.util.NoSuchElementException e)
            {
                return 0;
            }
            catch (java.lang.NullPointerException e)
            {
                return 0;
            }
        }

        /**
         * Increment the active count for the given key. Also
         * increments the total active count.
         * 
         * @param key pool key
         */
        private void incrementActiveCount(K key)
        {
            _totActive++;
            java.lang.Integer old = _activeCount.get(key);
            if (null == old)
            {
                _activeCount.put(key, new java.lang.Integer(1));
            }
            else
            {
                _activeCount.put(key, new java.lang.Integer(old.intValue() + 1));
            }
        }

        /**
         * Decrements the active count for the given key.
         * Also decrements the total active count.
         * 
         * @param key pool key
         */
        private void decrementActiveCount(K key)
        {
            _totActive--;
            java.lang.Integer active = _activeCount.get(key);
            if (null == active)
            {
                // do nothing, either null or zero is OK
            }
            else if (active.intValue() <= 1)
            {
                _activeCount.remove(key);
            }
            else
            {
                _activeCount.put(key, new java.lang.Integer(active.intValue() - 1));
            }
        }


        /**
         * @return map of keyed pools
         * @since 1.5.5
         */
        public java.util.Map<K, java.util.Stack<V>> getPools()
        {
            return _pools;
        }

        /**
         * @return the cap on the number of "sleeping" instances in <code>each</code> pool.
         * @since 1.5.5
         */
        public int getMaxSleeping()
        {
            return _maxSleeping;
        }

        /**
         * @return the initial capacity of each pool.
         * @since 1.5.5
         */
        public int getInitSleepingCapacity()
        {
            return _initSleepingCapacity;
        }

        /**
         * @return the _totActive
         */
        public int getTotActive()
        {
            return _totActive;
        }

        /**
         * @return the _totIdle
         */
        public int getTotIdle()
        {
            return _totIdle;
        }

        /**
         * @return the _activeCount
         * @since 1.5.5
         */
        public java.util.Map<K, java.lang.Integer> getActiveCount()
        {
            return _activeCount;
        }


        /** The default cap on the number of "sleeping" instances in the pool. */
        protected internal const int DEFAULT_MAX_SLEEPING = 8;

        /**
         * The default initial size of the pool
         * (this specifies the size of the container, it does not
         * cause the pool to be pre-populated.)
         */
        protected internal const int DEFAULT_INIT_SLEEPING_CAPACITY = 4;

        /**
         *  My named-set of pools.
         *  [Obsolete] to be removed in pool 2.0.  Use {@link #getPools()}
         */
        [Obsolete]
        protected java.util.HashMap<K, java.util.Stack<V>> _pools = null;

        /**
         * My {@link KeyedPoolableObjectFactory}.
         * [Obsolete] to be removed in pool 2.0.  Use {@link #getFactory()}
         */
        [Obsolete]
        protected KeyedPoolableObjectFactory<K, V> _factory = null;

        /**
         *  The cap on the number of "sleeping" instances in <code>each</code> pool.
         *  [Obsolete] to be removed in pool 2.0.  Use {@link #getMaxSleeping()}
         */
        [Obsolete]
        protected int _maxSleeping = DEFAULT_MAX_SLEEPING;

        /**
         * The initial capacity of each pool.
         * [Obsolete] to be removed in pool 2.0.  Use {@link #getInitSleepingCapacity()}.
         */
        [Obsolete]
        protected int _initSleepingCapacity = DEFAULT_INIT_SLEEPING_CAPACITY;

        /**
         * Total number of object borrowed and not yet returned for all pools.
         * [Obsolete] to be removed in pool 2.0.  Use {@link #getTotActive()}.
         */
        [Obsolete]
        protected int _totActive = 0;

        /**
         * Total number of objects "sleeping" for all pools
         * [Obsolete] to be removed in pool 2.0.  Use {@link #getTotIdle()}.
         */
        [Obsolete]
        protected int _totIdle = 0;

        /**
         * Number of active objects borrowed and not yet returned by pool
         * [Obsolete] to be removed in pool 2.0.  Use {@link #getActiveCount()}.
         */
        [Obsolete]
        protected java.util.HashMap<K, java.lang.Integer> _activeCount = null;

    }
}