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

namespace biz.ritter.javapi.net
{

    [Serializable]
    public class HttpRetryException : java.io.IOException {

        private static readonly long serialVersionUID = 0L;
        
        private readonly int RESPONSE_CODE;
        private readonly String LOCATION;

        public HttpRetryException(String detailMessage, int code) : base (detailMessage){
          this.RESPONSE_CODE = code;
        }

        public HttpRetryException(String detailMessage, int code, String location) : base (detailMessage) {
          this.RESPONSE_CODE = code;
          this.LOCATION = location;
        }
        
        public int responseCode () {
          return this.RESPONSE_CODE;
        }
        
        public String getReason() {
          return this.getMessage();
        }
        
        public String getLocation () {
          return this.LOCATION;
        }
    }
}
