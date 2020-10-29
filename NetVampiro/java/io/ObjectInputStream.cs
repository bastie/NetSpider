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
 *  Copyright © 2020 Sebastian Ritter
 */
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class ObjectInputStream : Closeable, DataInput, ObjectInput, ObjectStreamConstants
    {

        private DataInputStream delegateInstance;

        protected ObjectInputStream() {}
        public ObjectInputStream(InputStream input)
        {
            delegateInstance = input is DataInputStream ? (DataInputStream)input : new DataInputStream(input);
        }
        public void defaultReadObject()
        {
            // Empty implementation
        }
        public void close()
        {
            delegateInstance.close();
        }

        public int available()
        {
            return delegateInstance.available();
        }

        public int read()
        {
            return delegateInstance.read();
        }

        public int read(byte[] b)
        {
            return delegateInstance.read(b);
        }

        public int read(byte[] b, int startOffset, int length)
        {
            return delegateInstance.read(b, startOffset, length);
        }

        public object readObject()
        {
            return readObjectOverride();
        }
        protected object readObjectOverride()
        {
            byte[] bigObjectArray = new byte[131072]; //128kb => Java classes cannot be bigger than 64kb - think this is enough
            int count = this.read(bigObjectArray);

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream inMemoryStream = new MemoryStream(bigObjectArray, 0, count);
            Object result = formatter.Deserialize(inMemoryStream);
            inMemoryStream.Close();
            return result;
        }

        public long skip(long byteCount)
        {
            return delegateInstance.skip(byteCount);
        }

        public bool readBoolean()
        {
            return delegateInstance.readBoolean();
        }

        public byte readByte()
        {
            return delegateInstance.readByte();
        }

        public char readChar()
        {
            return delegateInstance.readChar();
        }

        public double readDouble()
        {
            return delegateInstance.readDouble();
        }

        public float readFloat()
        {
            return delegateInstance.readFloat();
        }

        public void readFully(byte[] buffer)
        {
            delegateInstance.readFully(buffer);
        }

        public void readFully(byte[] buffer, int offset, int count)
        {
            delegateInstance.readFully(buffer, offset, count);
        }

        public int readInt()
        {
            return delegateInstance.readInt();
        }

        public string readLine()
        {
            return delegateInstance.readLine();
        }

        public long readLong()
        {
            return delegateInstance.readLong();
        }

        public short readShort()
        {
            return delegateInstance.readShort();
        }

        public int readUnsignedByte()
        {
            return delegateInstance.readUnsignedByte();
        }

        public int readUnsignedShort()
        {
            return delegateInstance.readUnsignedShort();
        }

        public string readUTF()
        {
            return delegateInstance.readUTF();
        }

        public int skipBytes(int count)
        {
            return delegateInstance.skipBytes(count);
        }
    }
}
