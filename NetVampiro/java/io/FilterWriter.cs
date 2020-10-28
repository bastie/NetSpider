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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io {

    public abstract class FilterWriter : Writer {

        protected Writer outJ;

        protected FilterWriter(Writer outJ) : base (outJ) {
            this.outJ = outJ;
        }

        public override void flush() //throws IOException 
        {
            lock (lockJ) {
                outJ.flush();
            }
        }

        public override void write(int c) {// throws IOException 
            lock (lockJ){
                this.outJ.write(c);
            }
        }

        public override void write(char [] cbuf, int off, int len) {// throws IOException 
            lock (lockJ){
                this.outJ.write(cbuf,off,len);
            }
        }

        public override void write(String str, int off, int len) {// throws IOException 
            lock (lockJ){
                this.outJ.write(str,off,len);
            }
        }

    }
}

