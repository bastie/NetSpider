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
     * <p>
     * Provides a shared idle object eviction timer for all pools. This class wraps
     * the standard {@link Timer} and keeps track of how many pools are using it.
     * If no pools are using the timer, it is canceled. This prevents a thread
     * being left running which, in application server environments, can lead to
     * memory leads and/or prevent applications from shutting down or reloading
     * cleanly.
     * </p>
     * <p>
     * This class has namespace scope to prevent its inclusion in the pool public API.
     * The class declaration below should *not* be changed to public.
     * </p>
     */
    class EvictionTimer
    {

        private static readonly Object lockJ = new Object();

        /** Timer instance */
        private static java.util.Timer _timer; //@GuardedBy("this")

        /** Static usage count tracker */
        private static int _usageCount; //@GuardedBy("this")

        /** Prevent instantiation */
        private EvictionTimer()
        {
            // Hide the default constuctor
        }

        /**
         * Add the specified eviction task to the timer. Tasks that are added with a
         * call to this method *must* call {@link #cancel(TimerTask)} to cancel the
         * task to prevent memory and/or thread leaks in application server
         * environments.
         * @param task      Task to be scheduled
         * @param delay     Delay in milliseconds before task is executed
         * @param period    Time in milliseconds between executions
         */
        static internal void schedule(java.util.TimerTask task, long delay, long period)
        {
            lock (lockJ)
            {
                if (null == _timer)
                {
                    // Force the new Timer thread to be created with a context class
                    // loader set to the class loader that loaded this library
                    java.lang.ClassLoader ccl = new PrivilegedGetTccl().run();
                    try
                    {
                        new PrivilegedSetTccl(typeof(EvictionTimer).getClass().getClassLoader());
                        _timer = new java.util.Timer(true);
                    }
                    finally
                    {
                        new PrivilegedSetTccl(ccl);
                    }
                }
                _usageCount++;
                _timer.schedule(task, delay, period);
            }
        }

        /**
         * Remove the specified eviction task from the timer.
         * @param task      Task to be scheduled
         */
        static internal void cancel(java.util.TimerTask task)
        {
            lock (lockJ)
                task.cancel();
            _usageCount--;
            if (_usageCount == 0)
            {
                _timer.cancel();
                _timer = null;
            }
        }
    }

    /** 
     * {@link PrivilegedAction} used to get the ContextClassLoader
     */
    class PrivilegedGetTccl
    {// implements PrivilegedAction<ClassLoader> {

        /** 
         * {@inheritDoc}
         */
        public java.lang.ClassLoader run()
        {
            return java.lang.Thread.currentThread().getContextClassLoader();
        }
    }

    /** 
     * {@link PrivilegedAction} used to set the ContextClassLoader
     */
    class PrivilegedSetTccl
    {// implements PrivilegedAction<ClassLoader> {

        /** ClassLoader */
        private readonly java.lang.ClassLoader cl;

        /**
         * Create a new PrivilegedSetTccl using the given classloader
         * @param cl ClassLoader to use
         */
        internal PrivilegedSetTccl(java.lang.ClassLoader cl)
        {
            this.cl = cl;
        }

        /** 
         * {@inheritDoc}
         */
        public java.lang.ClassLoader run()
        {
            java.lang.Thread.currentThread().setContextClassLoader(cl);
            return null;
        }
    }

}