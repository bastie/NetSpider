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

namespace biz.ritter.javapi.nio.file
{
    [Serializable]
    public class InvalidPathException : java.lang.IllegalArgumentException // Java7
    {
        private readonly String input;
        private readonly String reason;
        private readonly int index;

        public InvalidPathException(String newInput, String newReason, int newIndex) : base (){
            if (null == newInput || null == newReason) throw new java.lang.NullPointerException();
            if (-1 > newIndex) throw new java.lang.IllegalArgumentException("Index to low");
            this.index = newIndex;
            this.input = newInput;
            this.reason = newReason;
        }
        public InvalidPathException(String input, String reason) : this(input, reason, -1) {}

        public String getInput() => this.input;
        public String getReason() => this.reason;
        public int getIndex() => this.index;

        public override string getMessage()
        {
            String result = this.index==-1 ?
              String.Format ("{0}: {1}",this.reason,this.input) :
              String.Format ("{0} at index {1}: {2}",this.reason,this.index,this.input);
            return result;
        }
    }
}
