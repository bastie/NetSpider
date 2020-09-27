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

using org.apache.commons.pool;

namespace org.apache.commons.pool.impl
{

    /**
     * A factory for creating {@link GenericKeyedObjectPool} instances.
     *
     * @param <K> the type of keys in this pool
     * @param <V> the type of objects held in this pool
     * 
     * @see GenericKeyedObjectPool
     * @see KeyedObjectPoolFactory
     *
     * @author Rodney Waldhoff
     * @author Dirk Verbeeck
     * @version $Revision: 1222396 $ $Date: 2011-12-22 14:02:25 -0500 (Thu, 22 Dec 2011) $
     * @since Pool 1.0
     */
    public class GenericKeyedObjectPoolFactory<K, V> : KeyedObjectPoolFactory<K, V>
    {
        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory) :
            this(factory, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_ACTIVE,
            GenericKeyedObjectPool<K, V>.DEFAULT_WHEN_EXHAUSTED_ACTION,
            GenericKeyedObjectPool<K, V>.DEFAULT_MAX_WAIT,
            GenericKeyedObjectPool<K, V>.DEFAULT_MAX_IDLE,
            GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param config a non-null GenericKeyedObjectPool<K,V>.Config describing the configuration.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, GenericKeyedObjectPool<K,V>.Config)
         * @throws NullPointerException when config is <code>null</code>.
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, GenericKeyedObjectPool<K, V>.Config config) ://throws NullPointerException {
            this(factory, config.maxActive, config.whenExhaustedAction, config.maxWait, config.maxIdle, config.maxTotal, config.minIdle, config.testOnBorrow, config.testOnReturn, config.timeBetweenEvictionRunsMillis, config.numTestsPerEvictionRun, config.minEvictableIdleTimeMillis, config.testWhileIdle, config.lifo)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive) :
            this(factory, maxActive, GenericKeyedObjectPool<K, V>.DEFAULT_WHEN_EXHAUSTED_ACTION, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_WAIT, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_IDLE, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait) :
            this(factory, maxActive, whenExhaustedAction, maxWait, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_IDLE, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, bool, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, bool testOnBorrow, bool testOnReturn) :
            this(factory, maxActive, whenExhaustedAction, maxWait, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_IDLE, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, testOnBorrow, testOnReturn, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param maxTotal the maximum number of objects that can exists at one time.
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, int maxTotal) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, maxTotal, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int, bool, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, bool testOnBorrow, bool testOnReturn) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, testOnBorrow, testOnReturn, GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS, GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @param timeBetweenEvictionRunsMillis the number of milliseconds to sleep between examining idle objects for eviction.
         * @param numTestsPerEvictionRun the number of idle objects to examine per run of the evictor.
         * @param minEvictableIdleTimeMillis the minimum number of milliseconds an object can sit idle in the pool before it is eligible for eviction.
         * @param testWhileIdle whether to validate objects in the idle object eviction thread.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int, bool, bool, long, int, long, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, bool testOnBorrow, bool testOnReturn, long timeBetweenEvictionRunsMillis, int numTestsPerEvictionRun, long minEvictableIdleTimeMillis, bool testWhileIdle) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL, testOnBorrow, testOnReturn, timeBetweenEvictionRunsMillis, numTestsPerEvictionRun, minEvictableIdleTimeMillis, testWhileIdle)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param maxTotal the maximum number of objects that can exists at one time.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @param timeBetweenEvictionRunsMillis the number of milliseconds to sleep between examining idle objects for eviction.
         * @param numTestsPerEvictionRun the number of idle objects to examine per run of the evictor.
         * @param minEvictableIdleTimeMillis the minimum number of milliseconds an object can sit idle in the pool before it is eligible for eviction.
         * @param testWhileIdle whether to validate objects in the idle object eviction thread.
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int, int, bool, bool, long, int, long, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, int maxTotal, bool testOnBorrow, bool testOnReturn, long timeBetweenEvictionRunsMillis, int numTestsPerEvictionRun, long minEvictableIdleTimeMillis, bool testWhileIdle) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, maxTotal, GenericKeyedObjectPool<K, V>.DEFAULT_MIN_IDLE, testOnBorrow, testOnReturn, timeBetweenEvictionRunsMillis, numTestsPerEvictionRun, minEvictableIdleTimeMillis, testWhileIdle)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param maxTotal the maximum number of objects that can exists at one time.
         * @param minIdle the minimum number of idle objects to have in the pool at any one time.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @param timeBetweenEvictionRunsMillis the number of milliseconds to sleep between examining idle objects for eviction.
         * @param numTestsPerEvictionRun the number of idle objects to examine per run of the evictor.
         * @param minEvictableIdleTimeMillis the minimum number of milliseconds an object can sit idle in the pool before it is eligible for eviction.
         * @param testWhileIdle whether to validate objects in the idle object eviction thread.
         * @since Pool 1.3
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int, int, int, bool, bool, long, int, long, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, int maxTotal, int minIdle, bool testOnBorrow, bool testOnReturn, long timeBetweenEvictionRunsMillis, int numTestsPerEvictionRun, long minEvictableIdleTimeMillis, bool testWhileIdle) :
            this(factory, maxActive, whenExhaustedAction, maxWait, maxIdle, maxTotal, minIdle, testOnBorrow, testOnReturn, timeBetweenEvictionRunsMillis, numTestsPerEvictionRun, minEvictableIdleTimeMillis, testWhileIdle, GenericKeyedObjectPool<K, V>.DEFAULT_LIFO)
        {
        }

        /**
         * Create a new GenericKeyedObjectPoolFactory.
         *
         * @param factory the KeyedPoolableObjectFactory to used by created pools.
         * @param maxActive the maximum number of objects that can be borrowed from pools at one time.
         * @param whenExhaustedAction the action to take when the pool is exhausted.
         * @param maxWait the maximum amount of time to wait for an idle object when the pool is exhausted.
         * @param maxIdle the maximum number of idle objects in the pools.
         * @param maxTotal the maximum number of objects that can exists at one time.
         * @param minIdle the minimum number of idle objects to have in the pool at any one time.
         * @param testOnBorrow whether to validate objects before they are returned by borrowObject.
         * @param testOnReturn whether to validate objects after they are returned to returnObject.
         * @param timeBetweenEvictionRunsMillis the number of milliseconds to sleep between examining idle objects for eviction.
         * @param numTestsPerEvictionRun the number of idle objects to examine per run of the evictor.
         * @param minEvictableIdleTimeMillis the minimum number of milliseconds an object can sit idle in the pool before it is eligible for eviction.
         * @param testWhileIdle whether to validate objects in the idle object eviction thread.
         * @param lifo whether or not objects are returned in last-in-first-out order from the idle object pool.
         * @since Pool 1.4
         * @see GenericKeyedObjectPool#GenericKeyedObjectPool(KeyedPoolableObjectFactory, int, byte, long, int, int, int, bool, bool, long, int, long, bool, bool)
         */
        public GenericKeyedObjectPoolFactory(KeyedPoolableObjectFactory<K, V> factory, int maxActive, byte whenExhaustedAction, long maxWait, int maxIdle, int maxTotal, int minIdle, bool testOnBorrow, bool testOnReturn, long timeBetweenEvictionRunsMillis, int numTestsPerEvictionRun, long minEvictableIdleTimeMillis, bool testWhileIdle, bool lifo)
        {
            _maxIdle = maxIdle;
            _maxActive = maxActive;
            _maxTotal = maxTotal;
            _minIdle = minIdle;
            _maxWait = maxWait;
            _whenExhaustedAction = whenExhaustedAction;
            _testOnBorrow = testOnBorrow;
            _testOnReturn = testOnReturn;
            _testWhileIdle = testWhileIdle;
            _timeBetweenEvictionRunsMillis = timeBetweenEvictionRunsMillis;
            _numTestsPerEvictionRun = numTestsPerEvictionRun;
            _minEvictableIdleTimeMillis = minEvictableIdleTimeMillis;
            _factory = factory;
            _lifo = lifo;
        }

        /**
         * Create a new GenericKeyedObjectPool with the currently configured properties.
         * 
         * @return GenericKeyedObjectPool with {@link GenericKeyedObjectPool<K,V>.Config Configuration} determined by
         * current property settings
         */
        public KeyedObjectPool<K, V> createPool()
        {
            return new GenericKeyedObjectPool<K, V>(_factory, _maxActive, _whenExhaustedAction, _maxWait, _maxIdle, _maxTotal, _minIdle, _testOnBorrow, _testOnReturn, _timeBetweenEvictionRunsMillis, _numTestsPerEvictionRun, _minEvictableIdleTimeMillis, _testWhileIdle, _lifo);
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMaxIdle() maxIdle} setting for pools created by this factory.
         * @since 1.5.5
         */
        public int getMaxIdle()
        {
            return _maxIdle;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMaxActive() maxActive} setting for pools created by this factory.
         * @since 1.5.5
         */
        public int getMaxActive()
        {
            return _maxActive;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMaxTotal() maxTotal} setting for pools created by this factory.
         * @since 1.5.5
         */
        public int getMaxTotal()
        {
            return _maxTotal;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMinIdle() minIdle} setting for pools created by this factory.
         * @since 1.5.5
         */
        public int getMinIdle()
        {
            return _minIdle;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMaxWait() maxWait} setting for pools created by this factory.
         * @since 1.5.5
         */
        public long getMaxWait()
        {
            return _maxWait;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getWhenExhaustedAction() whenExhaustedAction} setting for pools created by this factory.
         * @since 1.5.5
         */
        public byte getWhenExhaustedAction()
        {
            return _whenExhaustedAction;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getTestOnBorrow() testOnBorrow} setting for pools created by this factory.
         * @since 1.5.5
         */
        public bool getTestOnBorrow()
        {
            return _testOnBorrow;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getTestOnReturn() testOnReturn} setting for pools created by this factory.
         * @since 1.5.5
         */
        public bool getTestOnReturn()
        {
            return _testOnReturn;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getTestWhileIdle() testWhileIdle} setting for pools created by this factory.
         * @since 1.5.5
         */
        public bool getTestWhileIdle()
        {
            return _testWhileIdle;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getTimeBetweenEvictionRunsMillis() timeBetweenEvictionRunsMillis}
         * setting for pools created by this factory.
         * @since 1.5.5
         */
        public long getTimeBetweenEvictionRunsMillis()
        {
            return _timeBetweenEvictionRunsMillis;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getNumTestsPerEvictionRun() numTestsPerEvictionRun}
         * setting for pools created by this factory.
         * @since 1.5.5
         */
        public int getNumTestsPerEvictionRun()
        {
            return _numTestsPerEvictionRun;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getMinEvictableIdleTimeMillis() minEvictableIdleTimeMillis}
         * setting for pools created by this factory.
         * @since 1.5.5
         */
        public long getMinEvictableIdleTimeMillis()
        {
            return _minEvictableIdleTimeMillis;
        }

        /**
         * @return the {@link KeyedPoolableObjectFactory} used by pools created by this factory.
         * @since 1.5.5
         */
        public KeyedPoolableObjectFactory<K, V> getFactory()
        {
            return _factory;
        }

        /**
         * @return the {@link GenericKeyedObjectPool#getLifo() lifo} setting for pools created by this factory.
         * @since 1.5.5
         */
        public bool getLifo()
        {
            return _lifo;
        }

        //--- protected attributes - deprecated, use getters to access these properties

        /**
         * The {@link GenericKeyedObjectPool#getMaxIdle() maxIdle} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMaxIdle()}.
         */
        [Obsolete]
        protected int _maxIdle = GenericKeyedObjectPool<K, V>.DEFAULT_MAX_IDLE;

        /**
         * The {@link GenericKeyedObjectPool#getMaxActive() maxActive} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMaxActive()}.
         */
        [Obsolete]
        protected int _maxActive = GenericKeyedObjectPool<K, V>.DEFAULT_MAX_ACTIVE;

        /**
         * The {@link GenericKeyedObjectPool#getMaxTotal() maxTotal} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMaxTotal()}.
         */
        [Obsolete]
        protected int _maxTotal = GenericKeyedObjectPool<K, V>.DEFAULT_MAX_TOTAL;

        /**
         * The {@link GenericKeyedObjectPool#getMinIdle() minIdle} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMinIdle()}.
         */
        [Obsolete]
        protected int _minIdle = GenericKeyedObjectPool<K, V>.DEFAULT_MIN_IDLE;

        /**
         * The {@link GenericKeyedObjectPool#getMaxWait() maxWait} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMaxWait()}.
         */
        [Obsolete]
        protected long _maxWait = GenericKeyedObjectPool<K, V>.DEFAULT_MAX_WAIT;

        /**
         * The {@link GenericKeyedObjectPool#getWhenExhaustedAction() whenExhaustedAction} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getWhenExhaustedAction()}.
         */
        [Obsolete]
        protected byte _whenExhaustedAction = GenericKeyedObjectPool<K, V>.DEFAULT_WHEN_EXHAUSTED_ACTION;

        /**
         * The {@link GenericKeyedObjectPool#getTestOnBorrow() testOnBorrow} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getTestOnBorrow()}.
         */
        [Obsolete]
        protected bool _testOnBorrow = GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_BORROW;

        /**
         * The {@link GenericKeyedObjectPool#getTestOnReturn() testOnReturn} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getTestOnReturn()}.
         */
        [Obsolete]
        protected bool _testOnReturn = GenericKeyedObjectPool<K, V>.DEFAULT_TEST_ON_RETURN;

        /**
         * The {@link GenericKeyedObjectPool#getTestWhileIdle() testWhileIdle} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getTestWhileIdle()}.
         */
        [Obsolete]
        protected bool _testWhileIdle = GenericKeyedObjectPool<K, V>.DEFAULT_TEST_WHILE_IDLE;

        /**
         * The {@link GenericKeyedObjectPool#getTimeBetweenEvictionRunsMillis() timeBetweenEvictionRunsMillis} setting for
         * pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getTimeBetweenEvictionRunsMillis()}.
         */
        [Obsolete]
        protected long _timeBetweenEvictionRunsMillis = GenericKeyedObjectPool<K, V>.DEFAULT_TIME_BETWEEN_EVICTION_RUNS_MILLIS;

        /**
         * The {@link GenericKeyedObjectPool#getNumTestsPerEvictionRun() numTestsPerEvictionRun} setting for
         * pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getNumTestsPerEvictionRun()}.
         */
        [Obsolete]
        protected int _numTestsPerEvictionRun = GenericKeyedObjectPool<K, V>.DEFAULT_NUM_TESTS_PER_EVICTION_RUN;

        /**
         * The {@link GenericKeyedObjectPool#getMinEvictableIdleTimeMillis() minEvictableIdleTimeMillis} setting for
         * pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getMinEvictableIdleTimeMillis()}.
         */
        [Obsolete]
        protected long _minEvictableIdleTimeMillis = GenericKeyedObjectPool<K, V>.DEFAULT_MIN_EVICTABLE_IDLE_TIME_MILLIS;

        /**
         * The {@link KeyedPoolableObjectFactory} used by pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getFactory()}.
         */
        [Obsolete]
        protected KeyedPoolableObjectFactory<K, V> _factory = null;

        /**
         * The {@link GenericKeyedObjectPool#getLifo() lifo} setting for pools created by this factory.
         * [Obsolete] to be removed in pool 2.0. Use {@link #getLifo()}.
         */
        [Obsolete]
        protected bool _lifo = GenericKeyedObjectPool<K, V>.DEFAULT_LIFO;

    }
}