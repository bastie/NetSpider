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

namespace org.apache.commons.pool
{

    /**
     * This class consists exclusively of static methods that operate on or return ObjectPool
     * or KeyedObjectPool related interfaces.
     *
     * @author Sandy McArthur
     * @version $Revision: 1222670 $ $Date: 2011-12-23 08:18:25 -0500 (Fri, 23 Dec 2011) $
     * @since Pool 1.3
     */
    public sealed class PoolUtils
    {

        /**
         * Timer used to periodically check pools idle object count.
         * Because a {@link Timer} creates a {@link Thread} this is lazily instantiated.
         */
        private static java.util.Timer MIN_IDLE_TIMER; //@GuardedBy("this")

        /**
         * PoolUtils instances should NOT be constructed in standard programming.
         * Instead, the class should be used procedurally: PoolUtils.adapt(aPool);.
         * This constructor is public to permit tools that require a JavaBean instance to operate.
         */
        public PoolUtils()
        {
        }

        /**
         * Should the supplied Throwable be re-thrown (eg if it is an instance of
         * one of the Throwables that should never be swallowed). Used by the pool
         * error handling for operations that throw exceptions that normally need to
         * be ignored.
         * @param t The Throwable to check
         * @throws ThreadDeath if that is passed in
         * @throws VirtualMachineError if that is passed in
         * @since Pool 1.5.5
         */
        public static void checkRethrow(System.Exception t) // Basties note: change Throwable to System.Exception
        {
            #region java world
                if (t is java.lang.ThreadDeath)
                {
                    throw (java.lang.ThreadDeath)t;
                }
                if (t is java.lang.VirtualMachineError)
                {
                    throw (java.lang.VirtualMachineError)t;
                }
            #endregion

            #region .net world
                if (t is System.Threading.ThreadAbortException)
                {
                    throw t;
                }
                else if (t is System.ExecutionEngineException)
                {
                    throw t;
                }
            #endregion
            // All other instances of Throwable will be silently swallowed
        }

        /**
         * Adapt a <code>KeyedPoolableObjectFactory</code> instance to work where a <code>PoolableObjectFactory</code> is
         * needed. This method is the equivalent of calling
         * {@link #adapt(KeyedPoolableObjectFactory, Object) PoolUtils.adapt(aKeyedPoolableObjectFactory, new Object())}.
         *
         * @param <V> the type of object
         * @param keyedFactory the {@link KeyedPoolableObjectFactory} to delegate to.
         * @return a {@link PoolableObjectFactory} that delegates to <code>keyedFactory</code> with an internal key.
         * @throws IllegalArgumentException when <code>keyedFactory</code> is <code>null</code>.
         * @see #adapt(KeyedPoolableObjectFactory, Object)
         * @since Pool 1.3
         */
        public static PoolableObjectFactory<V> adapt<V>(KeyedPoolableObjectFactory<Object, V> keyedFactory)
        {//throws IllegalArgumentException {
            return adapt(keyedFactory, new Object());
        }

        /**
         * Adapt a <code>KeyedPoolableObjectFactory</code> instance to work where a <code>PoolableObjectFactory</code> is
         * needed using the specified <code>key</code> when delegating.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedFactory the {@link KeyedPoolableObjectFactory} to delegate to.
         * @param key the key to use when delegating.
         * @return a {@link PoolableObjectFactory} that delegates to <code>keyedFactory</code> with the specified key.
         * @throws IllegalArgumentException when <code>keyedFactory</code> or <code>key</code> is <code>null</code>.
         * @see #adapt(KeyedPoolableObjectFactory)
         * @since Pool 1.3
         */
        public static PoolableObjectFactory<V> adapt<K, V>(KeyedPoolableObjectFactory<K, V> keyedFactory, K key)
        {// throws IllegalArgumentException {
            return new PoolableObjectFactoryAdaptor<K, V>(keyedFactory, key);
        }

        /**
         * Adapt a <code>PoolableObjectFactory</code> instance to work where a <code>KeyedPoolableObjectFactory</code> is
         * needed. The key is ignored.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param factory the {@link PoolableObjectFactory} to delegate to.
         * @return a {@link KeyedPoolableObjectFactory} that delegates to <code>factory</code> ignoring the key.
         * @throws IllegalArgumentException when <code>factory</code> is <code>null</code>.
         * @since Pool 1.3
         */
        public static KeyedPoolableObjectFactory<K, V> adapt<K, V>(PoolableObjectFactory<V> factory)
        {//throws IllegalArgumentException {
            return new KeyedPoolableObjectFactoryAdaptor<K, V>(factory);
        }

        /**
         * Adapt a <code>KeyedObjectPool</code> instance to work where an <code>ObjectPool</code> is needed. This is the
         * equivalent of calling {@link #adapt(KeyedObjectPool, Object) PoolUtils.adapt(aKeyedObjectPool, new Object())}.
         *
         * @param <V> the type of object
         * @param keyedPool the {@link KeyedObjectPool} to delegate to.
         * @return an {@link ObjectPool} that delegates to <code>keyedPool</code> with an internal key.
         * @throws IllegalArgumentException when <code>keyedPool</code> is <code>null</code>.
         * @see #adapt(KeyedObjectPool, Object)
         * @since Pool 1.3
         */
        public static ObjectPool<V> adapt<V>(KeyedObjectPool<Object, V> keyedPool)
        {//throws IllegalArgumentException {
            return adapt(keyedPool, new Object());
        }

        /**
         * Adapt a <code>KeyedObjectPool</code> instance to work where an <code>ObjectPool</code> is needed using the
         * specified <code>key</code> when delegating.
         *
         * @param <V> the type of object
         * @param keyedPool the {@link KeyedObjectPool} to delegate to.
         * @param key the key to use when delegating.
         * @return an {@link ObjectPool} that delegates to <code>keyedPool</code> with the specified key.
         * @throws IllegalArgumentException when <code>keyedPool</code> or <code>key</code> is <code>null</code>.
         * @see #adapt(KeyedObjectPool)
         * @since Pool 1.3
         */
        public static ObjectPool<V> adapt<V>(KeyedObjectPool<Object, V> keyedPool, Object key)
        {//throws IllegalArgumentException {
            return new ObjectPoolAdaptor<V>(keyedPool, key);
        }

        /**
         * Adapt an <code>ObjectPool</code> to work where an <code>KeyedObjectPool</code> is needed.
         * The key is ignored.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param pool the {@link ObjectPool} to delegate to.
         * @return a {@link KeyedObjectPool} that delegates to <code>pool</code> ignoring the key.
         * @throws IllegalArgumentException when <code>pool</code> is <code>null</code>.
         * @since Pool 1.3
         */
        public static KeyedObjectPool<K, V> adapt<K, V>(ObjectPool<V> pool)
        {// throws IllegalArgumentException {
            return new KeyedObjectPoolAdaptor<K, V>(pool);
        }

        /**
         * Wraps an <code>ObjectPool</code> and dynamically checks the type of objects borrowed and returned to the pool.
         * If an object is passed to the pool that isn't of type <code>type</code> a {@link ClassCastException} will be thrown.
         *
         * @param <T> the type of object
         * @param pool the pool to enforce type safety on
         * @param type the class type to enforce.
         * @return an <code>ObjectPool</code> that will only allow objects of <code>type</code>
         * @since Pool 1.3
         */
        public static ObjectPool<T> checkedPool<T>(ObjectPool<T> pool, java.lang.Class type)
        { // Basties note: java.lang.Class<T> expected
            if (pool == null)
            {
                throw new java.lang.IllegalArgumentException("pool must not be null.");
            }
            if (type == null)
            {
                throw new java.lang.IllegalArgumentException("type must not be null.");
            }
            return new CheckedObjectPool<T>(pool, type);
        }

        /**
         * Wraps a <code>KeyedObjectPool</code> and dynamically checks the type of objects borrowed and returned to the keyedPool.
         * If an object is passed to the keyedPool that isn't of type <code>type</code> a {@link ClassCastException} will be thrown.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the keyedPool to enforce type safety on
         * @param type the class type to enforce.
         * @return a <code>KeyedObjectPool</code> that will only allow objects of <code>type</code>
         * @since Pool 1.3
         */
        public static KeyedObjectPool<K, V> checkedPool<K, V>(KeyedObjectPool<K, V> keyedPool, java.lang.Class type)
        {// Basties note: java.lang.Class<T> expected
            if (keyedPool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            if (type == null)
            {
                throw new java.lang.IllegalArgumentException("type must not be null.");
            }
            return new CheckedKeyedObjectPool<K, V>(keyedPool, type);
        }

        /**
         * Periodically check the idle object count for the pool. At most one idle object will be added per period.
         * If there is an exception when calling {@link ObjectPool#addObject()} then no more checks will be performed.
         *
         * @param <T> the type of object
         * @param pool the pool to check periodically.
         * @param minIdle if the {@link ObjectPool#getNumIdle()} is less than this then add an idle object.
         * @param period the frequency to check the number of idle objects in a pool, see
         *      {@link Timer#schedule(TimerTask, long, long)}.
         * @return the {@link TimerTask} that will periodically check the pools idle object count.
         * @throws IllegalArgumentException when <code>pool</code> is <code>null</code> or
         *      when <code>minIdle</code> is negative or when <code>period</code> isn't
         *      valid for {@link Timer#schedule(TimerTask, long, long)}.
         * @since Pool 1.3
         */
        public static java.util.TimerTask checkMinIdle<T>(ObjectPool<T> pool, int minIdle, long period)
        {//throws IllegalArgumentException {
            if (pool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            if (minIdle < 0)
            {
                throw new java.lang.IllegalArgumentException("minIdle must be non-negative.");
            }
            java.util.TimerTask task = new ObjectPoolMinIdleTimerTask<T>(pool, minIdle);
            getMinIdleTimer().schedule(task, 0L, period);
            return task;
        }

        /**
         * Periodically check the idle object count for the key in the keyedPool. At most one idle object will be added per period.
         * If there is an exception when calling {@link KeyedObjectPool#addObject(Object)} then no more checks for that key
         * will be performed.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the keyedPool to check periodically.
         * @param key the key to check the idle count of.
         * @param minIdle if the {@link KeyedObjectPool#getNumIdle(Object)} is less than this then add an idle object.
         * @param period the frequency to check the number of idle objects in a keyedPool, see
         *      {@link Timer#schedule(TimerTask, long, long)}.
         * @return the {@link TimerTask} that will periodically check the pools idle object count.
         * @throws IllegalArgumentException when <code>keyedPool</code>, <code>key</code> is <code>null</code> or
         *      when <code>minIdle</code> is negative or when <code>period</code> isn't
         *      valid for {@link Timer#schedule(TimerTask, long, long)}.
         * @since Pool 1.3
         */
        public static java.util.TimerTask checkMinIdle<K, V>(KeyedObjectPool<K, V> keyedPool, K key, int minIdle, long period)
        {// throws IllegalArgumentException {
            if (keyedPool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            if (key == null)
            {
                throw new java.lang.IllegalArgumentException("key must not be null.");
            }
            if (minIdle < 0)
            {
                throw new java.lang.IllegalArgumentException("minIdle must be non-negative.");
            }
            java.util.TimerTask task = new KeyedObjectPoolMinIdleTimerTask<K, V>(keyedPool, key, minIdle);
            getMinIdleTimer().schedule(task, 0L, period);
            return task;
        }

        /**
         * Periodically check the idle object count for each key in the <code>Collection</code> <code>keys</code> in the keyedPool.
         * At most one idle object will be added per period.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the keyedPool to check periodically.
         * @param keys a collection of keys to check the idle object count.
         * @param minIdle if the {@link KeyedObjectPool#getNumIdle(Object)} is less than this then add an idle object.
         * @param period the frequency to check the number of idle objects in a keyedPool, see
         *      {@link Timer#schedule(TimerTask, long, long)}.
         * @return a {@link Map} of key and {@link TimerTask} pairs that will periodically check the pools idle object count.
         * @throws IllegalArgumentException when <code>keyedPool</code>, <code>keys</code>, or any of the values in the
         *      collection is <code>null</code> or when <code>minIdle</code> is negative or when <code>period</code> isn't
         *      valid for {@link Timer#schedule(TimerTask, long, long)}.
         * @see #checkMinIdle(KeyedObjectPool, Object, int, long)
         * @since Pool 1.3
         */
        public static java.util.Map<K, java.util.TimerTask> checkMinIdle<K, V>(KeyedObjectPool<K, V> keyedPool, java.util.Collection<K> keys, int minIdle, long period)
        {//throws IllegalArgumentException {
            if (keys == null)
            {
                throw new java.lang.IllegalArgumentException("keys must not be null.");
            }
            java.util.Map<K, java.util.TimerTask> tasks = new java.util.HashMap<K, java.util.TimerTask>(keys.size());
            java.util.Iterator<K> iter = keys.iterator();
            while (iter.hasNext())
            {
                K key = iter.next();
                java.util.TimerTask task = checkMinIdle(keyedPool, key, minIdle, period);
                tasks.put(key, task);
            }
            return tasks;
        }

        /**
         * Call <code>addObject()</code> on <code>pool</code> <code>count</code> number of times.
         *
         * @param <T> the type of object
         * @param pool the pool to prefill.
         * @param count the number of idle objects to add.
         * @throws Exception when {@link ObjectPool#addObject()} fails.
         * @throws IllegalArgumentException when <code>pool</code> is <code>null</code>.
         * @since Pool 1.3
         */
        public static void prefill<T>(ObjectPool<T> pool, int count)
        {//throws Exception, IllegalArgumentException {
            if (pool == null)
            {
                throw new java.lang.IllegalArgumentException("pool must not be null.");
            }
            for (int i = 0; i < count; i++)
            {
                pool.addObject();
            }
        }

        /**
         * Call <code>addObject(Object)</code> on <code>keyedPool</code> with <code>key</code> <code>count</code>
         * number of times.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the keyedPool to prefill.
         * @param key the key to add objects for.
         * @param count the number of idle objects to add for <code>key</code>.
         * @throws Exception when {@link KeyedObjectPool#addObject(Object)} fails.
         * @throws IllegalArgumentException when <code>keyedPool</code> or <code>key</code> is <code>null</code>.
         * @since Pool 1.3
         */
        public static void prefill<K, V>(KeyedObjectPool<K, V> keyedPool, K key, int count)
        {//throws Exception, IllegalArgumentException {
            if (keyedPool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            if (key == null)
            {
                throw new java.lang.IllegalArgumentException("key must not be null.");
            }
            for (int i = 0; i < count; i++)
            {
                keyedPool.addObject(key);
            }
        }

        /**
         * Call <code>addObject(Object)</code> on <code>keyedPool</code> with each key in <code>keys</code> for
         * <code>count</code> number of times. This has the same effect as calling
         * {@link #prefill(KeyedObjectPool, Object, int)} for each key in the <code>keys</code> collection.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the keyedPool to prefill.
         * @param keys {@link Collection} of keys to add objects for.
         * @param count the number of idle objects to add for each <code>key</code>.
         * @throws Exception when {@link KeyedObjectPool#addObject(Object)} fails.
         * @throws IllegalArgumentException when <code>keyedPool</code>, <code>keys</code>, or
         *      any value in <code>keys</code> is <code>null</code>.
         * @see #prefill(KeyedObjectPool, Object, int)
         * @since Pool 1.3
         */
        public static void prefill<K, V>(KeyedObjectPool<K, V> keyedPool, java.util.Collection<K> keys, int count)
        {//throws Exception, IllegalArgumentException {
            if (keys == null)
            {
                throw new java.lang.IllegalArgumentException("keys must not be null.");
            }
            java.util.Iterator<K> iter = keys.iterator();
            while (iter.hasNext())
            {
                prefill(keyedPool, iter.next(), count);
            }
        }

        /**
         * Returns a synchronized (thread-safe) ObjectPool backed by the specified ObjectPool.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. Wrapping a pool that
         * {@link #wait() waits} for poolable objects to be returned before allowing another one to be
         * borrowed with another layer of synchronization will cause liveliness issues or a deadlock.
         * </p>
         *
         * @param <T> the type of object
         * @param pool the ObjectPool to be "wrapped" in a synchronized ObjectPool.
         * @return a synchronized view of the specified ObjectPool.
         * @since Pool 1.3
         */
        public static ObjectPool<T> synchronizedPool<T>(ObjectPool<T> pool)
        {
            if (pool == null)
            {
                throw new java.lang.IllegalArgumentException("pool must not be null.");
            }
            /*
            assert !(pool instanceof GenericObjectPool)
                    : "GenericObjectPool is already thread-safe";
            assert !(pool instanceof SoftReferenceObjectPool)
                    : "SoftReferenceObjectPool is already thread-safe";
            assert !(pool instanceof StackObjectPool)
                    : "StackObjectPool is already thread-safe";
            assert !"org.apache.commons.pool.composite.CompositeObjectPool".equals(pool.getClass().getName())
                    : "CompositeObjectPools are already thread-safe";
            */
            return new SynchronizedObjectPool<T>(pool);
        }

        /**
         * Returns a synchronized (thread-safe) KeyedObjectPool backed by the specified KeyedObjectPool.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. Wrapping a pool that
         * {@link #wait() waits} for poolable objects to be returned before allowing another one to be
         * borrowed with another layer of synchronization will cause liveliness issues or a deadlock.
         * </p>
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the KeyedObjectPool to be "wrapped" in a synchronized KeyedObjectPool.
         * @return a synchronized view of the specified KeyedObjectPool.
         * @since Pool 1.3
         */
        public static KeyedObjectPool<K, V> synchronizedPool<K, V>(KeyedObjectPool<K, V> keyedPool)
        {
            if (keyedPool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            /*
            assert !(keyedPool instanceof GenericKeyedObjectPool)
                    : "GenericKeyedObjectPool is already thread-safe";
            assert !(keyedPool instanceof StackKeyedObjectPool)
                    : "StackKeyedObjectPool is already thread-safe";
            assert !"org.apache.commons.pool.composite.CompositeKeyedObjectPool".equals(keyedPool.getClass().getName())
                    : "CompositeKeyedObjectPools are already thread-safe";
            */
            return new SynchronizedKeyedObjectPool<K, V>(keyedPool);
        }

        /**
         * Returns a synchronized (thread-safe) PoolableObjectFactory backed by the specified PoolableObjectFactory.
         *
         * @param <T> the type of object
         * @param factory the PoolableObjectFactory to be "wrapped" in a synchronized PoolableObjectFactory.
         * @return a synchronized view of the specified PoolableObjectFactory.
         * @since Pool 1.3
         */
        public static PoolableObjectFactory<T> synchronizedPoolableFactory<T>(PoolableObjectFactory<T> factory)
        {
            return new SynchronizedPoolableObjectFactory<T>(factory);
        }

        /**
         * Returns a synchronized (thread-safe) KeyedPoolableObjectFactory backed by the specified KeyedPoolableObjectFactory.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedFactory the KeyedPoolableObjectFactory to be "wrapped" in a synchronized KeyedPoolableObjectFactory.
         * @return a synchronized view of the specified KeyedPoolableObjectFactory.
         * @since Pool 1.3
         */
        public static KeyedPoolableObjectFactory<K, V> synchronizedPoolableFactory<K, V>(KeyedPoolableObjectFactory<K, V> keyedFactory)
        {
            return new SynchronizedKeyedPoolableObjectFactory<K, V>(keyedFactory);
        }

        /**
         * Returns a pool that adaptively decreases it's size when idle objects are no longer needed.
         * This is intended as an always thread-safe alternative to using an idle object evictor
         * provided by many pool implementations. This is also an effective way to shrink FIFO ordered
         * pools that experience load spikes.
         *
         * @param <T> the type of object
         * @param pool the ObjectPool to be decorated so it shrinks it's idle count when possible.
         * @return a pool that adaptively decreases it's size when idle objects are no longer needed.
         * @see #erodingPool(ObjectPool, float)
         * @since Pool 1.4
         */
        public static ObjectPool<T> erodingPool<T>(ObjectPool<T> pool)
        {
            return erodingPool(pool, 1f);
        }

        /**
         * Returns a pool that adaptively decreases it's size when idle objects are no longer needed.
         * This is intended as an always thread-safe alternative to using an idle object evictor
         * provided by many pool implementations. This is also an effective way to shrink FIFO ordered
         * pools that experience load spikes.
         *
         * <p>
         * The factor parameter provides a mechanism to tweak the rate at which the pool tries to shrink
         * it's size. Values between 0 and 1 cause the pool to try to shrink it's size more often.
         * Values greater than 1 cause the pool to less frequently try to shrink it's size.
         * </p>
         *
         * @param <T> the type of object
         * @param pool the ObjectPool to be decorated so it shrinks it's idle count when possible.
         * @param factor a positive value to scale the rate at which the pool tries to reduce it's size.
         * If 0 &lt; factor &lt; 1 then the pool shrinks more aggressively.
         * If 1 &lt; factor then the pool shrinks less aggressively.
         * @return a pool that adaptively decreases it's size when idle objects are no longer needed.
         * @see #erodingPool(ObjectPool)
         * @since Pool 1.4
         */
        public static ObjectPool<T> erodingPool<T>(ObjectPool<T> pool, float factor)
        {
            if (pool == null)
            {
                throw new java.lang.IllegalArgumentException("pool must not be null.");
            }
            if (factor <= 0f)
            {
                throw new java.lang.IllegalArgumentException("factor must be positive.");
            }
            return new ErodingObjectPool<T>(pool, factor);
        }

        /**
         * Returns a pool that adaptively decreases it's size when idle objects are no longer needed.
         * This is intended as an always thread-safe alternative to using an idle object evictor
         * provided by many pool implementations. This is also an effective way to shrink FIFO ordered
         * pools that experience load spikes.
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the KeyedObjectPool to be decorated so it shrinks it's idle count when
         * possible.
         * @return a pool that adaptively decreases it's size when idle objects are no longer needed.
         * @see #erodingPool(KeyedObjectPool, float)
         * @see #erodingPool(KeyedObjectPool, float, boolean)
         * @since Pool 1.4
         */
        public static KeyedObjectPool<K, V> erodingPool<K, V>(KeyedObjectPool<K, V> keyedPool)
        {
            return erodingPool(keyedPool, 1f);
        }

        /**
         * Returns a pool that adaptively decreases it's size when idle objects are no longer needed.
         * This is intended as an always thread-safe alternative to using an idle object evictor
         * provided by many pool implementations. This is also an effective way to shrink FIFO ordered
         * pools that experience load spikes.
         *
         * <p>
         * The factor parameter provides a mechanism to tweak the rate at which the pool tries to shrink
         * it's size. Values between 0 and 1 cause the pool to try to shrink it's size more often.
         * Values greater than 1 cause the pool to less frequently try to shrink it's size.
         * </p>
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the KeyedObjectPool to be decorated so it shrinks it's idle count when
         * possible.
         * @param factor a positive value to scale the rate at which the pool tries to reduce it's size.
         * If 0 &lt; factor &lt; 1 then the pool shrinks more aggressively.
         * If 1 &lt; factor then the pool shrinks less aggressively.
         * @return a pool that adaptively decreases it's size when idle objects are no longer needed.
         * @see #erodingPool(KeyedObjectPool, float, boolean)
         * @since Pool 1.4
         */
        public static KeyedObjectPool<K, V> erodingPool<K, V>(KeyedObjectPool<K, V> keyedPool, float factor)
        {
            return erodingPool(keyedPool, factor, false);
        }

        /**
         * Returns a pool that adaptively decreases it's size when idle objects are no longer needed.
         * This is intended as an always thread-safe alternative to using an idle object evictor
         * provided by many pool implementations. This is also an effective way to shrink FIFO ordered
         * pools that experience load spikes.
         *
         * <p>
         * The factor parameter provides a mechanism to tweak the rate at which the pool tries to shrink
         * it's size. Values between 0 and 1 cause the pool to try to shrink it's size more often.
         * Values greater than 1 cause the pool to less frequently try to shrink it's size.
         * </p>
         *
         * <p>
         * The perKey parameter determines if the pool shrinks on a whole pool basis or a per key basis.
         * When perKey is false, the keys do not have an effect on the rate at which the pool tries to
         * shrink it's size. When perKey is true, each key is shrunk independently.
         * </p>
         *
         * @param <K> the type of key
         * @param <V> the type of object
         * @param keyedPool the KeyedObjectPool to be decorated so it shrinks it's idle count when
         * possible.
         * @param factor a positive value to scale the rate at which the pool tries to reduce it's size.
         * If 0 &lt; factor &lt; 1 then the pool shrinks more aggressively.
         * If 1 &lt; factor then the pool shrinks less aggressively.
         * @param perKey when true, each key is treated independently.
         * @return a pool that adaptively decreases it's size when idle objects are no longer needed.
         * @see #erodingPool(KeyedObjectPool)
         * @see #erodingPool(KeyedObjectPool, float)
         * @since Pool 1.4
         */
        public static KeyedObjectPool<K, V> erodingPool<K, V>(KeyedObjectPool<K, V> keyedPool, float factor, bool perKey)
        {
            if (keyedPool == null)
            {
                throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
            }
            if (factor <= 0f)
            {
                throw new java.lang.IllegalArgumentException("factor must be positive.");
            }
            if (perKey)
            {
                return new ErodingPerKeyKeyedObjectPool<K, V>(keyedPool, factor);
            }
            else
            {
                return new ErodingKeyedObjectPool<K, V>(keyedPool, factor);
            }
        }

        private static Object lockObject = new Object();
        /**
         * Get the <code>Timer</code> for checking keyedPool's idle count. Lazily create the {@link Timer} as needed.
         *
         * @return the {@link Timer} for checking keyedPool's idle count.
         * @since Pool 1.3
         */
        private static java.util.Timer getMinIdleTimer()
        {
            lock (lockObject)
            {
                if (MIN_IDLE_TIMER == null)
                {
                    MIN_IDLE_TIMER = new java.util.Timer(true);
                }
                return MIN_IDLE_TIMER;
            }
        }

        /**
         * Adaptor class that wraps and converts a KeyedPoolableObjectFactory with a fixed
         * key to a PoolableObjectFactory.
         */
        private class PoolableObjectFactoryAdaptor<K, V> : PoolableObjectFactory<V>
        {
            /** Fixed key */
            private K key;

            /** Wrapped factory */
            private KeyedPoolableObjectFactory<K, V> keyedFactory;

            /**
             * Create a PoolableObjectFactoryAdaptor wrapping the provided KeyedPoolableObjectFactory with the 
             * given fixed key.
             * 
             * @param keyedFactory KeyedPoolableObjectFactory that will manage objects
             * @param key fixed key
             * @throws IllegalArgumentException if either of the parameters is null
             */
            public PoolableObjectFactoryAdaptor(KeyedPoolableObjectFactory<K, V> keyedFactory, K key)
            {//throws IllegalArgumentException {
                if (keyedFactory == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedFactory must not be null.");
                }
                if (key == null)
                {
                    throw new java.lang.IllegalArgumentException("key must not be null.");
                }
                this.keyedFactory = keyedFactory;
                this.key = key;
            }

            /**
             * Create an object instance using the configured factory and key.
             * 
             * @return new object instance
             */
            public V makeObject()
            {//throws Exception {
                return keyedFactory.makeObject(key);
            }

            /**
             * Destroy the object, passing the fixed key to the factory.
             * 
             * @param obj object to destroy
             */
            public void destroyObject(V obj)
            {//throws Exception {
                keyedFactory.destroyObject(key, obj);
            }

            /**
             * Validate the object, passing the fixed key to the factory.
             * 
             * @param obj object to validate
             * @return true if validation is successful
             */
            public bool validateObject(V obj)
            {
                return keyedFactory.validateObject(key, obj);
            }

            /**
             * Activate the object, passing the fixed key to the factory.
             * 
             * @param obj object to activate
             */
            public void activateObject(V obj)
            {//throws Exception {
                keyedFactory.activateObject(key, obj);
            }

            /**
             * Passivate the object, passing the fixed key to the factory.
             * 
             * @param obj object to passivate
             */
            public void passivateObject(V obj)
            {//throws Exception {
                keyedFactory.passivateObject(key, obj);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("PoolableObjectFactoryAdaptor");
                sb.append("{key=").append(key);
                sb.append(", keyedFactory=").append(keyedFactory);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Adaptor class that turns a PoolableObjectFactory into a KeyedPoolableObjectFactory by
         * ignoring keys.
         */
        private class KeyedPoolableObjectFactoryAdaptor<K, V> : KeyedPoolableObjectFactory<K, V>
        {

            /** Underlying PoolableObjectFactory */
            private PoolableObjectFactory<V> factory;

            /**
             * Create a new KeyedPoolableObjectFactoryAdaptor using the given PoolableObjectFactory to
             * manage objects.
             * 
             * @param factory wrapped PoolableObjectFactory 
             * @throws IllegalArgumentException if the factory is null
             */
            public KeyedPoolableObjectFactoryAdaptor(PoolableObjectFactory<V> factory)
            {//throws IllegalArgumentException {
                if (factory == null)
                {
                    throw new java.lang.IllegalArgumentException("factory must not be null.");
                }
                this.factory = factory;
            }

            /**
             * Create a new object instance, ignoring the key
             * 
             * @param key ignored
             * @return newly created object instance
             */
            public V makeObject(K key)
            {//throws Exception {
                return factory.makeObject();
            }

            /**
             * Destroy the object, ignoring the key.
             * 
             * @param key ignored
             * @param obj instance to destroy
             */
            public void destroyObject(K key, V obj)
            {//throws Exception {
                factory.destroyObject(obj);
            }

            /**
             * Validate the object, ignoring the key
             * 
             * @param key ignored
             * @param obj object to validate
             * @return true if validation is successful
             */
            public bool validateObject(K key, V obj)
            {
                return factory.validateObject(obj);
            }

            /**
             * Activate the object, ignoring the key.
             * 
             * @param key ignored
             * @param obj object to be activated
             */
            public void activateObject(K key, V obj)
            {//throws Exception {
                factory.activateObject(obj);
            }

            /**
             * Passivate the object, ignoring the key.
             * 
             * @param key ignored
             * @param obj object to passivate
             */
            public void passivateObject(K key, V obj)
            {//throws Exception {
                factory.passivateObject(obj);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("KeyedPoolableObjectFactoryAdaptor");
                sb.append("{factory=").append(factory);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Adapts a KeyedObjectPool to make it an ObjectPool by fixing restricting to
         * a fixed key.
         */
        private class ObjectPoolAdaptor<V> : ObjectPool<V>
        {

            /** Fixed key */
            private Object key;

            /** Underlying KeyedObjectPool */
            private KeyedObjectPool<Object, V> keyedPool;

            /**
             * Create a new ObjectPoolAdaptor using the provided KeyedObjectPool and fixed key.
             * 
             * @param keyedPool underlying KeyedObjectPool
             * @param key fixed key
             * @throws IllegalArgumentException if either of the parameters is null
             */
            public ObjectPoolAdaptor(KeyedObjectPool<Object, V> keyedPool, Object key)
            {//throws IllegalArgumentException {
                if (keyedPool == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
                }
                if (key == null)
                {
                    throw new java.lang.IllegalArgumentException("key must not be null.");
                }
                this.keyedPool = keyedPool;
                this.key = key;
            }

            /**
             * {@inheritDoc}
             */
            public V borrowObject()
            {//throws Exception, NoSuchElementException, IllegalStateException {
                return keyedPool.borrowObject(key);
            }

            /**
             * {@inheritDoc}
             */
            public void returnObject(V obj)
            {
                try
                {
                    keyedPool.returnObject(key, obj);
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * {@inheritDoc}
             */
            public void invalidateObject(V obj)
            {
                try
                {
                    keyedPool.invalidateObject(key, obj);
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject()
            {//throws Exception, IllegalStateException {
                keyedPool.addObject(key);
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumIdle(key);
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumActive(key);
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                keyedPool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    keyedPool.close();
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the PoolableObjectFactory for the pool.
             * 
             * @param factory new PoolableObjectFactory 
             * [Obsolete] to be removed in version 2.0
             */
            [Obsolete]
            public void setFactory(PoolableObjectFactory<V> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                throw new java.lang.UnsupportedOperationException("Basties note: this obsolete method are not implemented!");
                //keyedPool.setFactory(adapt(factory);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("ObjectPoolAdaptor");
                sb.append("{key=").append(key);
                sb.append(", keyedPool=").append(keyedPool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Adapts an ObjectPool to implement KeyedObjectPool by ignoring key arguments.
         */
        private class KeyedObjectPoolAdaptor<K, V> : KeyedObjectPool<K, V>
        {

            /** Underlying pool */
            private ObjectPool<V> pool;

            /**
             * Create a new KeyedObjectPoolAdaptor wrapping the given ObjectPool
             * 
             * @param pool underlying object pool
             * @throws IllegalArgumentException if pool is null
             */
            public KeyedObjectPoolAdaptor(ObjectPool<V> pool)
            {//throws IllegalArgumentException {
                if (pool == null)
                {
                    throw new java.lang.IllegalArgumentException("pool must not be null.");
                }
                this.pool = pool;
            }

            /**
             * Borrow and object from the pool, ignoring the key
             * 
             * @param key ignored
             * @return newly created object instance
             */
            public V borrowObject(K key)
            {//throws Exception, NoSuchElementException, IllegalStateException {
                return pool.borrowObject();
            }

            /**
             * Return and object to the pool, ignoring the key
             * 
             * @param key ignored
             * @param obj object to return
             */
            public void returnObject(K key, V obj)
            {
                try
                {
                    pool.returnObject(obj);
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Invalidate and object, ignoring the key
             * 
             * @param obj object to invalidate
             * @param key ignored
             */
            public void invalidateObject(K key, V obj)
            {
                try
                {
                    pool.invalidateObject(obj);
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Add an object to the pool, ignoring the key
             * 
             * @param key ignored
             */
            public void addObject(K key)
            {// throws Exception, IllegalStateException {
                pool.addObject();
            }

            /**
             * Return the number of objects idle in the pool, ignoring the key.
             * 
             * @param key ignored
             * @return idle instance count
             */
            public int getNumIdle(K key)
            {//throws UnsupportedOperationException {
                return pool.getNumIdle();
            }

            /**
             * Return the number of objects checked out from the pool, ignoring the key.
             * 
             * @param key ignored
             * @return active instance count
             */
            public int getNumActive(K key)
            {//throws UnsupportedOperationException {
                return pool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                return pool.getNumIdle();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                return pool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                pool.clear();
            }

            /**
             * Clear the pool, ignoring the key (has same effect as {@link #clear()}.
             * 
             * @param key ignored.
             */
            public void clear(K key)
            {//throws Exception, UnsupportedOperationException {
                pool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    pool.close();
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the factory used to manage objects.
             * 
             * @param factory new factory to use managing object instances
             * [Obsolete] to be removed in version 2.0
             */
            [Obsolete]
            public void setFactory(KeyedPoolableObjectFactory<K, V> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                pool.setFactory(adapt((KeyedPoolableObjectFactory<Object, V>)factory));
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("KeyedObjectPoolAdaptor");
                sb.append("{pool=").append(pool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * An object pool that performs type checking on objects passed
         * to pool methods.
         *
         */
        private class CheckedObjectPool<T> : ObjectPool<T>
        {
            /** 
             * Type of objects allowed in the pool. This should be a subtype of the return type of
             * the underlying pool's associated object factory.
             */
            private java.lang.Class type;//Basties note: java.lang.Class<T> expected

            /** Underlying object pool */
            private ObjectPool<T> pool;

            /**
             * Create a CheckedObjectPool accepting objects of the given type using
             * the given pool.
             * 
             * @param pool underlying object pool
             * @param type expected pooled object type
             * @throws IllegalArgumentException if either parameter is null
             */
            public CheckedObjectPool(ObjectPool<T> pool, java.lang.Class type)
            {//Basties note: java.lang.Class<T> expected
                if (pool == null)
                {
                    throw new java.lang.IllegalArgumentException("pool must not be null.");
                }
                if (type == null)
                {
                    throw new java.lang.IllegalArgumentException("type must not be null.");
                }
                this.pool = pool;
                this.type = type;
            }

            /**
             * Borrow an object from the pool, checking its type.
             * 
             * @return a type-checked object from the pool
             * @throws ClassCastException if the object returned by the pool is not of the expected type
             */
            public T borrowObject()
            {// throws Exception, NoSuchElementException, IllegalStateException {
                T obj = pool.borrowObject();
                if (type.isInstance(obj))
                {
                    return obj;
                }
                else
                {
                    throw new java.lang.ClassCastException("Borrowed object is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * Return an object to the pool, verifying that it is of the correct type.
             * 
             * @param obj object to return
             * @throws ClassCastException if obj is not of the expected type
             */
            public void returnObject(T obj)
            {
                if (type.isInstance(obj))
                {
                    try
                    {
                        pool.returnObject(obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
                else
                {
                    throw new java.lang.ClassCastException("Returned object is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * Invalidates an object from the pool, verifying that it is of the expected type.
             * 
             * @param obj object to invalidate
             * @throws ClassCastException if obj is not of the expected type
             */
            public void invalidateObject(T obj)
            {
                if (type.isInstance(obj))
                {
                    try
                    {
                        pool.invalidateObject(obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
                else
                {
                    throw new java.lang.ClassCastException("Invalidated object is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject()
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                pool.addObject();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                return pool.getNumIdle();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                return pool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                pool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    pool.close();
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the object factory associated with the pool
             * 
             * @param factory object factory
             * [Obsolete] to be removed in version 2.0
             */
            [Obsolete]
            public void setFactory(PoolableObjectFactory<T> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                pool.setFactory(factory);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("CheckedObjectPool");
                sb.append("{type=").append(type);
                sb.append(", pool=").append(pool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * A keyed object pool that performs type checking on objects passed
         * to pool methods.
         *
         */
        private class CheckedKeyedObjectPool<K, V> : KeyedObjectPool<K, V>
        {
            /** 
             * Expected type of objects managed by the pool.  This should be
             * a subtype of the return type of the object factory used by the pool.
             */
            private java.lang.Class type; //Basties note: java.lang.Class<V> expected

            /** Underlying pool */
            private KeyedObjectPool<K, V> keyedPool;

            /**
             * Create a new CheckedKeyedObjectPool from the given pool with given expected object type.
             * 
             * @param keyedPool underlying pool
             * @param type expected object type
             * @throws IllegalArgumentException if either parameter is null
             */
            public CheckedKeyedObjectPool(KeyedObjectPool<K, V> keyedPool, java.lang.Class type)
            {//Basties note: java.lang.Class<V> expected
                if (keyedPool == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
                }
                if (type == null)
                {
                    throw new java.lang.IllegalArgumentException("type must not be null.");
                }
                this.keyedPool = keyedPool;
                this.type = type;
            }

            /**
             * Borrow an object from the pool, verifying correct return type.
             * 
             * @param key pool key
             * @return type-checked object from the pool under the given key
             * @throws ClassCastException if the object returned by the pool is not of the expected type
             */
            public V borrowObject(K key)
            {//throws Exception, NoSuchElementException, IllegalStateException {
                V obj = keyedPool.borrowObject(key);
                if (type.isInstance(obj))
                {
                    return obj;
                }
                else
                {
                    throw new java.lang.ClassCastException("Borrowed object for key: " + key + " is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * Return an object to the pool, checking its type.
             * 
             * @param key the associated key (not type-checked)
             * @param obj the object to return (type-checked)
             * @throws ClassCastException if obj is not of the expected type
             */
            public void returnObject(K key, V obj)
            {
                if (type.isInstance(obj))
                {
                    try
                    {
                        keyedPool.returnObject(key, obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
                else
                {
                    throw new java.lang.ClassCastException("Returned object for key: " + key + " is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * Invalidate an object to the pool, checking its type.
             * 
             * @param key the associated key (not type-checked)
             * @param obj the object to return (type-checked)
             * @throws ClassCastException if obj is not of the expected type
             */
            public void invalidateObject(K key, V obj)
            {
                if (type.isInstance(obj))
                {
                    try
                    {
                        keyedPool.invalidateObject(key, obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
                else
                {
                    throw new java.lang.ClassCastException("Invalidated object for key: " + key + " is not of type: " + type.getName() + " was: " + obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject(K key)
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                keyedPool.addObject(key);
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle(K key)
            {//throws UnsupportedOperationException {
                return keyedPool.getNumIdle(key);
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive(K key)
            {//throws UnsupportedOperationException {
                return keyedPool.getNumActive(key);
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumIdle();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                keyedPool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public void clear(K key)
            {//throws Exception, UnsupportedOperationException {
                keyedPool.clear(key);
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    keyedPool.close();
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the object factory associated with the pool
             * 
             * @param factory object factory
             * [Obsolete] to be removed in version 2.0
             */
            [Obsolete]
            public void setFactory(KeyedPoolableObjectFactory<K, V> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                keyedPool.setFactory(factory);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("CheckedKeyedObjectPool");
                sb.append("{type=").append(type);
                sb.append(", keyedPool=").append(keyedPool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Timer task that adds objects to the pool until the number of idle
         * instances reaches the configured minIdle.  Note that this is not the
         * same as the pool's minIdle setting.
         * 
         */
        private class ObjectPoolMinIdleTimerTask<T> : java.util.TimerTask
        {

            /** Minimum number of idle instances.  Not the same as pool.getMinIdle(). */
            private int minIdle;

            /** Object pool */
            private ObjectPool<T> pool;

            /**
             * Create a new ObjectPoolMinIdleTimerTask for the given pool with the given minIdle setting.
             * 
             * @param pool object pool
             * @param minIdle number of idle instances to maintain
             * @throws IllegalArgumentException if the pool is null
             */
            public ObjectPoolMinIdleTimerTask(ObjectPool<T> pool, int minIdle)
            {//throws IllegalArgumentException {
                if (pool == null)
                {
                    throw new java.lang.IllegalArgumentException("pool must not be null.");
                }
                this.pool = pool;
                this.minIdle = minIdle;
            }

            /**
             * {@inheritDoc}
             */
            public override void run()
            {
                bool success = false;
                try
                {
                    if (pool.getNumIdle() < minIdle)
                    {
                        pool.addObject();
                    }
                    success = true;

                }
                catch (Exception )
                {
                    cancel();

                }
                finally
                {
                    // detect other types of Throwable and cancel this Timer
                    if (!success)
                    {
                        cancel();
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("ObjectPoolMinIdleTimerTask");
                sb.append("{minIdle=").append(minIdle);
                sb.append(", pool=").append(pool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Timer task that adds objects to the pool until the number of idle
         * instances for the given key reaches the configured minIdle.  Note that this is not the
         * same as the pool's minIdle setting.
         * 
         */
        private class KeyedObjectPoolMinIdleTimerTask<K, V> : java.util.TimerTask
        {
            /** Minimum number of idle instances.  Not the same as pool.getMinIdle(). */
            private int minIdle;

            /** Key to ensure minIdle for */
            private K key;

            /** Keyed object pool */
            private KeyedObjectPool<K, V> keyedPool;

            /**
             * Create a new KeyedObjecPoolMinIdleTimerTask.
             * 
             * @param keyedPool keyed object pool
             * @param key key to ensure minimum number of idle instances
             * @param minIdle minimum number of idle instances 
             * @throws IllegalArgumentException if the key is null
             */
            public KeyedObjectPoolMinIdleTimerTask(KeyedObjectPool<K, V> keyedPool, K key, int minIdle)
            {//throws IllegalArgumentException {
                if (keyedPool == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
                }
                this.keyedPool = keyedPool;
                this.key = key;
                this.minIdle = minIdle;
            }

            /**
             * {@inheritDoc}
             */
            public override void run()
            {
                bool success = false;
                try
                {
                    if (keyedPool.getNumIdle(key) < minIdle)
                    {
                        keyedPool.addObject(key);
                    }
                    success = true;

                }
                catch (Exception )
                {
                    cancel();

                }
                finally
                {
                    // detect other types of Throwable and cancel this Timer
                    if (!success)
                    {
                        cancel();
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("KeyedObjectPoolMinIdleTimerTask");
                sb.append("{minIdle=").append(minIdle);
                sb.append(", key=").append(key);
                sb.append(", keyedPool=").append(keyedPool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * A synchronized (thread-safe) ObjectPool backed by the specified ObjectPool.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. Wrapping a pool that
         * {@link #wait() waits} for poolable objects to be returned before allowing another one to be
         * borrowed with another layer of synchronization will cause liveliness issues or a deadlock.
         * </p>
         */
        private class SynchronizedObjectPool<T> : ObjectPool<T>
        {

            /** Object whose monitor is used to synchronize methods on the wrapped pool. */
            private Object lockJ;

            /** the underlying object pool */
            private ObjectPool<T> pool;

            /**
             * Create a new SynchronizedObjectPool wrapping the given pool.
             * 
             * @param pool the ObjectPool to be "wrapped" in a synchronized ObjectPool.
             * @throws IllegalArgumentException if the pool is null
             */
            public SynchronizedObjectPool(ObjectPool<T> pool)
            {//throws IllegalArgumentException {
                if (pool == null)
                {
                    throw new java.lang.IllegalArgumentException("pool must not be null.");
                }
                this.pool = pool;
                lockJ = new Object();
            }

            /**
             * {@inheritDoc}
             */
            public T borrowObject()
            {//throws Exception, NoSuchElementException, IllegalStateException {
                lock (lockJ)
                {
                    return pool.borrowObject();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void returnObject(T obj)
            {
                lock (lockJ)
                {
                    try
                    {
                        pool.returnObject(obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public void invalidateObject(T obj)
            {
                lock (lockJ)
                {
                    try
                    {
                        pool.invalidateObject(obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject()
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                lock (lockJ)
                {
                    pool.addObject();
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return pool.getNumIdle();
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return pool.getNumActive();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                lock (lockJ)
                {
                    pool.clear();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    lock (lockJ)
                    {
                        pool.close();
                    }
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the factory used by the pool.
             * 
             * @param factory new PoolableObjectFactory
             * [Obsolete] to be removed in pool 2.0
             */
            [Obsolete]
            public void setFactory(PoolableObjectFactory<T> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                lock (lockJ)
                {
                    pool.setFactory(factory);
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("SynchronizedObjectPool");
                sb.append("{pool=").append(pool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * A synchronized (thread-safe) KeyedObjectPool backed by the specified KeyedObjectPool.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. Wrapping a pool that
         * {@link #wait() waits} for poolable objects to be returned before allowing another one to be
         * borrowed with another layer of synchronization will cause liveliness issues or a deadlock.
         * </p>
         */
        private class SynchronizedKeyedObjectPool<K, V> : KeyedObjectPool<K, V>
        {

            /** Object whose monitor is used to synchronize methods on the wrapped pool. */
            private Object lockJ;

            /** Underlying object pool */
            private KeyedObjectPool<K, V> keyedPool;

            /**
             * Create a new SynchronizedKeyedObjectPool wrapping the given pool
             * 
             * @param keyedPool KeyedObjectPool to wrap
             * @throws IllegalArgumentException if keyedPool is null
             */
            public SynchronizedKeyedObjectPool(KeyedObjectPool<K, V> keyedPool)
            {//throws IllegalArgumentException {
                if (keyedPool == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
                }
                this.keyedPool = keyedPool;
                lockJ = new Object();
            }

            /**
             * {@inheritDoc}
             */
            public V borrowObject(K key)
            {//throws Exception, NoSuchElementException, IllegalStateException {
                lock (lockJ)
                {
                    return keyedPool.borrowObject(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void returnObject(K key, V obj)
            {
                lock (lockJ)
                {
                    try
                    {
                        keyedPool.returnObject(key, obj);
                    }
                    catch (Exception )
                    {
                        // swallowed
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public void invalidateObject(K key, V obj)
            {
                lock (lockJ)
                {
                    try
                    {
                        keyedPool.invalidateObject(key, obj);
                    }
                    catch (Exception )
                    {
                        // swallowed as of Pool 2
                    }
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject(K key)
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                lock (lockJ)
                {
                    keyedPool.addObject(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle(K key)
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return keyedPool.getNumIdle(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive(K key)
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return keyedPool.getNumActive(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return keyedPool.getNumIdle();
                }
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                lock (lockJ)
                {
                    return keyedPool.getNumActive();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                lock (lockJ)
                {
                    keyedPool.clear();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void clear(K key)
            {//throws Exception, UnsupportedOperationException {
                lock (lockJ)
                {
                    keyedPool.clear(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    lock (lockJ)
                    {
                        keyedPool.close();
                    }
                }
                catch (Exception )
                {
                    // swallowed as of Pool 2
                }
            }

            /**
             * Sets the object factory used by the pool.
             * 
             * @param factory KeyedPoolableObjectFactory used by the pool
             * [Obsolete] to be removed in pool 2.0
             */
            [Obsolete]
            public void setFactory(KeyedPoolableObjectFactory<K, V> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                lock (lockJ)
                {
                    keyedPool.setFactory(factory);
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("SynchronizedKeyedObjectPool");
                sb.append("{keyedPool=").append(keyedPool);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * A fully synchronized PoolableObjectFactory that wraps a PoolableObjectFactory and synchronizes
         * access to the wrapped factory methods.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. </p>
         */
        private class SynchronizedPoolableObjectFactory<T> : PoolableObjectFactory<T>
        {
            /** Synchronization lock */
            private Object lockJ;

            /** Wrapped factory */
            private PoolableObjectFactory<T> factory;

            /** 
             * Create a SynchronizedPoolableObjectFactory wrapping the given factory.
             * 
             * @param factory underlying factory to wrap
             * @throws IllegalArgumentException if the factory is null
             */
            public SynchronizedPoolableObjectFactory(PoolableObjectFactory<T> factory)
            {//throws IllegalArgumentException {
                if (factory == null)
                {
                    throw new java.lang.IllegalArgumentException("factory must not be null.");
                }
                this.factory = factory;
                lockJ = new Object();
            }

            /**
             * {@inheritDoc}
             */
            public T makeObject()
            {//throws Exception {
                lock (lockJ)
                {
                    return factory.makeObject();
                }
            }

            /**
             * {@inheritDoc}
             */
            public void destroyObject(T obj)
            {//throws Exception {
                lock (lockJ)
                {
                    factory.destroyObject(obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public bool validateObject(T obj)
            {
                lock (lockJ)
                {
                    return factory.validateObject(obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void activateObject(T obj)
            {//throws Exception {
                lock (lockJ)
                {
                    factory.activateObject(obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void passivateObject(T obj)
            {//throws Exception {
                lock (lockJ)
                {
                    factory.passivateObject(obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("SynchronizedPoolableObjectFactory");
                sb.append("{factory=").append(factory);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * A fully synchronized KeyedPoolableObjectFactory that wraps a KeyedPoolableObjectFactory and synchronizes
         * access to the wrapped factory methods.
         *
         * <p><b>Note:</b>
         * This should not be used on pool implementations that already provide proper synchronization
         * such as the pools provided in the Commons Pool library. </p>
         */
        private class SynchronizedKeyedPoolableObjectFactory<K, V> : KeyedPoolableObjectFactory<K, V>
        {
            /** Synchronization lock */
            private Object lockJ;

            /** Wrapped factory */
            private KeyedPoolableObjectFactory<K, V> keyedFactory;

            /** 
             * Create a SynchronizedKeyedPoolableObjectFactory wrapping the given factory.
             * 
             * @param keyedFactory underlying factory to wrap
             * @throws IllegalArgumentException if the factory is null
             */
            public SynchronizedKeyedPoolableObjectFactory(KeyedPoolableObjectFactory<K, V> keyedFactory)
            {//throws IllegalArgumentException {
                if (keyedFactory == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedFactory must not be null.");
                }
                this.keyedFactory = keyedFactory;
                lockJ = new Object();
            }

            /**
             * {@inheritDoc}
             */
            public V makeObject(K key)
            {//throws Exception {
                lock (lockJ)
                {
                    return keyedFactory.makeObject(key);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void destroyObject(K key, V obj)
            {//throws Exception {
                lock (lockJ)
                {
                    keyedFactory.destroyObject(key, obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public bool validateObject(K key, V obj)
            {
                lock (lockJ)
                {
                    return keyedFactory.validateObject(key, obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void activateObject(K key, V obj)
            {//throws Exception {
                lock (lockJ)
                {
                    keyedFactory.activateObject(key, obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public void passivateObject(K key, V obj)
            {//throws Exception {
                lock (lockJ)
                {
                    keyedFactory.passivateObject(key, obj);
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                java.lang.StringBuffer sb = new java.lang.StringBuffer();
                sb.append("SynchronizedKeyedPoolableObjectFactory");
                sb.append("{keyedFactory=").append(keyedFactory);
                sb.append('}');
                return sb.toString();
            }
        }

        /**
         * Encapsulate the logic for when the next poolable object should be discarded.
         * Each time update is called, the next time to shrink is recomputed, based on
         * the float factor, number of idle instances in the pool and high water mark.
         * Float factor is assumed to be between 0 and 1.  Values closer to 1 cause
         * less frequent erosion events.  Erosion event timing also depends on numIdle.
         * When this value is relatively high (close to previously established high water
         * mark), erosion occurs more frequently.
         */
        private class ErodingFactor
        {
            /** Determines frequency of "erosion" events */
            private float factor;

            /** Time of next shrink event */
            [NonSerialized]
            private long nextShrink; //volatile removed - search for 'private field volatile' to found changes

            /** High water mark - largest numIdle encountered */
            [NonSerialized]
            private volatile int idleHighWaterMark;

            /**
             * Create a new ErodingFactor with the given erosion factor.
             * 
             * @param factor erosion factor
             */
            public ErodingFactor(float factor)
            {
                this.factor = factor;
                lock (this) //private field volatile
                {
                    nextShrink = java.lang.SystemJ.currentTimeMillis() + (long)(900000 * factor); // now + 15 min * factor
                }
                idleHighWaterMark = 1;
            }

            /**
             * Updates internal state based on numIdle and the current time.
             * 
             * @param numIdle number of idle elements in the pool
             */
            public void update(int numIdle)
            {
                update(java.lang.SystemJ.currentTimeMillis(), numIdle);
            }

            /**
             * Updates internal state using the supplied time and numIdle.
             * 
             * @param now current time
             * @param numIdle number of idle elements in the pool
             */
            public void update(long now, int numIdle)
            {
                int idle = java.lang.Math.max(0, numIdle);
                idleHighWaterMark = java.lang.Math.max(idle, idleHighWaterMark);
                float maxInterval = 15f;
                float minutes = maxInterval + ((1f - maxInterval) / idleHighWaterMark) * idle;
                lock (this)
                {
                    nextShrink = now + (long)(minutes * 60000f * factor);
                }
            }

            /**
             * Returns the time of the next erosion event.
             * 
             * @return next shrink time
             */
            public long getNextShrink()
            {
                lock (this)
                {
                    return nextShrink;
                }
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                return "ErodingFactor{" +
                        "factor=" + factor +
                        ", idleHighWaterMark=" + idleHighWaterMark +
                        '}';
            }
        }

        /**
         * Decorates an object pool, adding "eroding" behavior.  Based on the
         * configured {@link #factor erosion factor}, objects returning to the pool
         * may be invalidated instead of being added to idle capacity.
         *
         */
        private class ErodingObjectPool<T> : ObjectPool<T>
        {
            /** Underlying object pool */
            private ObjectPool<T> pool;

            /** Erosion factor */
            private ErodingFactor factor;

            /** 
             * Create an ErodingObjectPool wrapping the given pool using the specified erosion factor.
             * 
             * @param pool underlying pool
             * @param factor erosion factor - determines the frequency of erosion events
             * @see #factor
             */
            public ErodingObjectPool(ObjectPool<T> pool, float factor)
            {
                this.pool = pool;
                this.factor = new ErodingFactor(factor);
            }

            /**
             * {@inheritDoc}
             */
            public T borrowObject()
            {//throws Exception, NoSuchElementException, IllegalStateException {
                return pool.borrowObject();
            }

            /**
             * Returns obj to the pool, unless erosion is triggered, in which
             * case obj is invalidated.  Erosion is triggered when there are idle instances in 
             * the pool and more than the {@link #factor erosion factor}-determined time has elapsed
             * since the last returnObject activation. 
             * 
             * @param obj object to return or invalidate
             * @see #factor
             */
            public void returnObject(T obj)
            {
                bool discard = false;
                long now = java.lang.SystemJ.currentTimeMillis();
                lock (pool)
                {
                    if (factor.getNextShrink() < now)
                    { // XXX: Pool 3: move test out of sync block
                        int numIdle = pool.getNumIdle();
                        if (numIdle > 0)
                        {
                            discard = true;
                        }

                        factor.update(now, numIdle);
                    }
                }
                try
                {
                    if (discard)
                    {
                        pool.invalidateObject(obj);
                    }
                    else
                    {
                        pool.returnObject(obj);
                    }
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * {@inheritDoc}
             */
            public void invalidateObject(T obj)
            {
                try
                {
                    pool.invalidateObject(obj);
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * {@inheritDoc}
             */
            public void addObject()
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                pool.addObject();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumIdle()
            {//throws UnsupportedOperationException {
                return pool.getNumIdle();
            }

            /**
             * {@inheritDoc}
             */
            public int getNumActive()
            {//throws UnsupportedOperationException {
                return pool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public void clear()
            {//throws Exception, UnsupportedOperationException {
                pool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public void close()
            {
                try
                {
                    pool.close();
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * {@inheritDoc}
             * [Obsolete] to be removed in pool 2.0
             */
            [Obsolete]
            public void setFactory(PoolableObjectFactory<T> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                pool.setFactory(factory);
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                return "ErodingObjectPool{" +
                        "factor=" + factor +
                        ", pool=" + pool +
                        '}';
            }
        }

        /**
         * Decorates a keyed object pool, adding "eroding" behavior.  Based on the
         * configured {@link #factor erosion factor}, objects returning to the pool
         * may be invalidated instead of being added to idle capacity.
         *
         */
        private class ErodingKeyedObjectPool<K, V> : KeyedObjectPool<K, V>
        {
            /** Underlying pool */
            private KeyedObjectPool<K, V> keyedPool;

            /** Erosion factor */
            private ErodingFactor erodingFactor;

            /** 
             * Create an ErodingObjectPool wrapping the given pool using the specified erosion factor.
             * 
             * @param keyedPool underlying pool
             * @param factor erosion factor - determines the frequency of erosion events
             * @see #erodingFactor
             */
            public ErodingKeyedObjectPool(KeyedObjectPool<K, V> keyedPool, float factor) :
                this(keyedPool, new ErodingFactor(factor))
            {
            }

            /** 
             * Create an ErodingObjectPool wrapping the given pool using the specified erosion factor.
             * 
             * @param keyedPool underlying pool - must not be null
             * @param erodingFactor erosion factor - determines the frequency of erosion events
             * @see #factor
             */
            protected ErodingKeyedObjectPool(KeyedObjectPool<K, V> keyedPool, ErodingFactor erodingFactor)
            {
                if (keyedPool == null)
                {
                    throw new java.lang.IllegalArgumentException("keyedPool must not be null.");
                }
                this.keyedPool = keyedPool;
                this.erodingFactor = erodingFactor;
            }

            /**
             * {@inheritDoc}
             */
            public virtual V borrowObject(K key)
            {//throws Exception, NoSuchElementException, IllegalStateException {
                return keyedPool.borrowObject(key);
            }

            /**
             * Returns obj to the pool, unless erosion is triggered, in which
             * case obj is invalidated.  Erosion is triggered when there are idle instances in 
             * the pool associated with the given key and more than the configured {@link #erodingFactor erosion factor}
             * time has elapsed since the last returnObject activation. 
             * 
             * @param obj object to return or invalidate
             * @param key key
             * @see #erodingFactor
             */
            public virtual void returnObject(K key, V obj)
            {//throws Exception {
                bool discard = false;
                long now = java.lang.SystemJ.currentTimeMillis();
                ErodingFactor factor = getErodingFactor(key);
                lock (keyedPool)
                {
                    if (factor.getNextShrink() < now)
                    {
                        int numIdleJ = numIdle(key);
                        if (numIdleJ > 0)
                        {
                            discard = true;
                        }

                        factor.update(now, numIdleJ);
                    }
                }
                try
                {
                    if (discard)
                    {
                        keyedPool.invalidateObject(key, obj);
                    }
                    else
                    {
                        keyedPool.returnObject(key, obj);
                    }
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * Returns the total number of instances currently idle in this pool (optional operation).
             * Returns a negative value if this information is not available.
             *
             * @param key ignored
             * @return the total number of instances currently idle in this pool or a negative value if unsupported
             * @throws UnsupportedOperationException <strong>deprecated</strong>: when this implementation doesn't support the operation
             */
            protected virtual int numIdle(K key)
            {
                return getKeyedPool().getNumIdle();
            }

            /**
             * Returns the eroding factor for the given key
             * @param key key
             * @return eroding factor for the given keyed pool
             */
            protected virtual ErodingFactor getErodingFactor(K key)
            {
                return erodingFactor;
            }

            /**
             * {@inheritDoc}
             */
            public virtual void invalidateObject(K key, V obj)
            {
                try
                {
                    keyedPool.invalidateObject(key, obj);
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * {@inheritDoc}
             */
            public virtual void addObject(K key)
            {//throws Exception, IllegalStateException, UnsupportedOperationException {
                keyedPool.addObject(key);
            }

            /**
             * {@inheritDoc}
             */
            public virtual int getNumIdle()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumIdle();
            }

            /**
             * {@inheritDoc}
             */
            public virtual int getNumIdle(K key)
            {//throws UnsupportedOperationException {
                return keyedPool.getNumIdle(key);
            }

            /**
             * {@inheritDoc}
             */
            public virtual int getNumActive()
            {//throws UnsupportedOperationException {
                return keyedPool.getNumActive();
            }

            /**
             * {@inheritDoc}
             */
            public virtual int getNumActive(K key)
            {//throws UnsupportedOperationException {
                return keyedPool.getNumActive(key);
            }

            /**
             * {@inheritDoc}
             */
            public virtual void clear()
            {//throws Exception, UnsupportedOperationException {
                keyedPool.clear();
            }

            /**
             * {@inheritDoc}
             */
            public virtual void clear(K key)
            {//throws Exception, UnsupportedOperationException {
                keyedPool.clear(key);
            }

            /**
             * {@inheritDoc}
             */
            public virtual void close()
            {
                try
                {
                    keyedPool.close();
                }
                catch (Exception )
                {
                    // swallowed
                }
            }

            /**
             * {@inheritDoc}
             * [Obsolete] to be removed in pool 2.0
             */
            [Obsolete]
            public virtual void setFactory(KeyedPoolableObjectFactory<K, V> factory)
            {//throws IllegalStateException, UnsupportedOperationException {
                keyedPool.setFactory(factory);
            }

            /**
             * Returns the underlying pool
             * 
             * @return the keyed pool that this ErodingKeyedObjectPool wraps
             */
            protected virtual KeyedObjectPool<K, V> getKeyedPool()
            {
                return keyedPool;
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                return "ErodingKeyedObjectPool{" +
                        "erodingFactor=" + erodingFactor +
                        ", keyedPool=" + keyedPool +
                        '}';
            }
        }

        /**
         * Extends ErodingKeyedObjectPool to allow erosion to take place on a per-key
         * basis.  Timing of erosion events is tracked separately for separate keyed pools.
         */
        private class ErodingPerKeyKeyedObjectPool<K, V> : ErodingKeyedObjectPool<K, V>
        {
            /** Erosion factor - same for all pools */
            private float factor;

            /** Map of ErodingFactor instances keyed on pool keys */
            private java.util.Map<K, ErodingFactor> factors = java.util.Collections<Object>.synchronizedMap(new java.util.HashMap<K, ErodingFactor>());

            /**
             * Create a new ErordingPerKeyKeyedObjectPool decorating the given keyed pool with
             * the specified erosion factor.
             * @param keyedPool underlying keyed pool
             * @param factor erosion factor
             */
            public ErodingPerKeyKeyedObjectPool(KeyedObjectPool<K, V> keyedPool, float factor) :
                base(keyedPool, null)
            {
                this.factor = factor;
            }

            /**
             * {@inheritDoc}
             */
            protected override int numIdle(K key)
            {
                return getKeyedPool().getNumIdle(key);
            }

            /**
             * {@inheritDoc}
             */
            protected override ErodingFactor getErodingFactor(K key)
            {
                ErodingFactor factor = factors.get(key);
                // this may result in two ErodingFactors being created for a key
                // since they are small and cheap this is okay.
                if (factor == null)
                {
                    factor = new ErodingFactor(this.factor);
                    factors.put(key, factor);
                }
                return factor;
            }

            /**
             * {@inheritDoc}
             */
            public override String ToString()
            {
                return "ErodingPerKeyKeyedObjectPool{" +
                        "factor=" + factor +
                        ", keyedPool=" + getKeyedPool() +
                        '}';
            }
        }
    }
}