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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.buffer
{

    /**
     * Decorates another <code>Buffer</code> to ensure a fixed maximum size.
     * <p>
     * Note: This class should only be used if you need to add bounded
     * behaviour to another buffer. If you just want a bounded buffer then
     * you should use {@link BoundedFifoBuffer} or {@link CircularFifoBuffer}.
     * <p>
     * The decoration methods allow you to specify a timeout value.
     * This alters the behaviour of the add methods when the buffer is full.
     * Normally, when the buffer is full, the add method will throw an exception.
     * With a timeout, the add methods will wait for up to the timeout period
     * to try and add the elements.
     *
     * @author James Carman
     * @author Stephen Colebourne
     * @version $Revision: 7191 $ $Date: 2011-06-07 21:49:19 +0200 (Di, 07. Jun 2011) $
     * @since Commons Collections 3.2
     */
    [Serializable]
    public class BoundedBuffer : SynchronizedBuffer, BoundedCollection
    {

        /** The serialization version. */
        private static readonly long serialVersionUID = 1536432911093974264L;

        /** The maximum size. */
        private readonly int maximumSize;
        /** The timeout milliseconds. */
        private readonly long timeout;

        /**
         * Factory method to create a bounded buffer.
         * <p>
         * When the buffer is full, it will immediately throw a
         * <code>BufferOverflowException</code> on calling <code>add()</code>.
         *
         * @param buffer  the buffer to decorate, must not be null
         * @param maximumSize  the maximum size, must be size one or greater
         * @return a new bounded buffer
         * @throws IllegalArgumentException if the buffer is null
         * @throws IllegalArgumentException if the maximum size is zero or less
         */
        public static BoundedBuffer decorate(Buffer buffer, int maximumSize)
        {
            return new BoundedBuffer(buffer, maximumSize, 0L);
        }

        /**
         * Factory method to create a bounded buffer that blocks for a maximum
         * amount of time.
         *
         * @param buffer  the buffer to decorate, must not be null
         * @param maximumSize  the maximum size, must be size one or greater
         * @param timeout  the maximum amount of time to wait in milliseconds
         * @return a new bounded buffer
         * @throws IllegalArgumentException if the buffer is null
         * @throws IllegalArgumentException if the maximum size is zero or less
         */
        public static BoundedBuffer decorate(Buffer buffer, int maximumSize, long timeout)
        {
            return new BoundedBuffer(buffer, maximumSize, timeout);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies) another buffer, making it bounded
         * waiting only up to a maximum amount of time.
         *
         * @param buffer  the buffer to wrap, must not be null
         * @param maximumSize  the maximum size, must be size one or greater
         * @param timeout  the maximum amount of time to wait
         * @throws IllegalArgumentException if the buffer is null
         * @throws IllegalArgumentException if the maximum size is zero or less
         */
        protected BoundedBuffer(Buffer buffer, int maximumSize, long timeout)
            : base(buffer)
        {
            if (maximumSize < 1)
            {
                throw new java.lang.IllegalArgumentException();
            }
            this.maximumSize = maximumSize;
            this.timeout = timeout;
        }

        //-----------------------------------------------------------------------
        public override Object remove()
        {
            lock (lockJ)
            {
                Object returnValue = getBuffer().remove();
                lockJ.notifyAll();
                return returnValue;
            }
        }

        public override bool add(Object o)
        {
            lock (lockJ)
            {
                timeoutWait(1);
                return getBuffer().add(o);
            }
        }

        public override bool addAll(java.util.Collection<Object> c)
        {
            lock (lockJ)
            {
                timeoutWait(c.size());
                return getBuffer().addAll(c);
            }
        }

        public override java.util.Iterator<Object> iterator()
        {
            return new NotifyingIterator(collection.iterator(), this);
        }

        private void timeoutWait(int nAdditions)
        {
            // method synchronized by callers
            if (nAdditions > maximumSize)
            {
                throw new BufferOverflowException(
                        "Buffer size cannot exceed " + maximumSize);
            }
            if (timeout <= 0)
            {
                // no wait period (immediate timeout)
                if (getBuffer().size() + nAdditions > maximumSize)
                {
                    throw new BufferOverflowException(
                            "Buffer size cannot exceed " + maximumSize);
                }
                return;
            }
            long expiration = java.lang.SystemJ.currentTimeMillis() + timeout;
            long timeLeft = expiration - java.lang.SystemJ.currentTimeMillis();
            while (timeLeft > 0 && getBuffer().size() + nAdditions > maximumSize)
            {
                try
                {
                    lockJ.wait(timeLeft);
                    timeLeft = expiration - java.lang.SystemJ.currentTimeMillis();
                }
                catch (java.lang.InterruptedException ex)
                {
                    java.io.PrintWriter outJ = new java.io.PrintWriter(new java.io.StringWriter());
                    ex.printStackTrace(outJ);
                    throw new BufferUnderflowException(
                        "Caused by InterruptedException: " + outJ.toString());
                }
            }
            if (getBuffer().size() + nAdditions > maximumSize)
            {
                throw new BufferOverflowException("Timeout expired");
            }
        }

        public virtual bool isFull()
        {
            // size() is synchronized
            return (size() == maxSize());
        }

        public virtual int maxSize()
        {
            return maximumSize;
        }

        //-----------------------------------------------------------------------
        /**
         * BoundedBuffer iterator.
         */
        private class NotifyingIterator : AbstractIteratorDecorator
        {
            private BoundedBuffer root;
            public NotifyingIterator(java.util.Iterator<Object> it, BoundedBuffer root)
                : base(it)
            {
                this.root = root;
            }

            public override void remove()
            {
                lock (root.lockJ)
                {
                    iterator.remove();
                    root.lockJ.notifyAll();
                }
            }
        }
    }
}