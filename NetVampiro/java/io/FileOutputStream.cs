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
 *  Copyright © 2011-2020 Sebastian Ritter
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class FileOutputStream : java.io.OutputStream
    {
        private System.IO.FileStream delegateInstance;

        /// <summary>
        /// Construct new instance with given File object
        /// </summary>
        /// <param name="f"></param>
        public FileOutputStream(File f) : this (f.getName()){}
        public FileOutputStream(string name)
        {
            try
            {
                delegateInstance = System.IO.File.OpenWrite(name);
            }
            catch (UnauthorizedAccessException toThrow){
                throw new java.lang.SecurityException(toThrow.Message);
            }
        }

        /// <summary>
        /// Create a new instance and go to end to append data
        /// </summary>
        /// <param name="f"></param>
        /// <param name="append"></param>
        public FileOutputStream (File f, bool append) : this(f.getName(), append){}

        /// <summary>
        /// Create a new instance and go to end to append data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="append"></param>
        public FileOutputStream(string name, bool append) : this (name)
        {
            if (append) this.delegateInstance.Position = this.delegateInstance.Length;
        }

        public override void write(int value)
        {
            try
            {
                this.delegateInstance.WriteByte((byte)value);
            }
            catch (SystemException toThrow)
            {
                throw new java.io.IOException(toThrow.Message);
            }
        }
        public override void write(byte[] value, int beginOffset, int length) // throws IOException; 
        {
            try {
                this.delegateInstance.Write(value,beginOffset,length);
            }
            catch (SystemException toThrow)
            {
                throw new java.io.IOException(toThrow.Message);
            }
        }

        public override void flush()
        {
            this.delegateInstance.Flush();
        }
        public override void close()
        {
            this.flush();
            this.delegateInstance.Dispose();
            this.delegateInstance.Close();
        }

		private java.nio.channels.FileChannel channel;
		public java.nio.channels.FileChannel getChannel () {
			if (null == this.channel) {
				lock (this) {
					if (null == this.channel) {
						// TODO implement it
						throw new java.lang.UnsupportedOperationException("Not yet implemented");
					}
				}
			}
			return this.channel;
		}
    }
}
