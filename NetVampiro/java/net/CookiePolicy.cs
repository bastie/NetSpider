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
    public interface CookiePolicy
    {
        bool shouldAccept (URI uri, HttpCookie cookie);

        public static readonly CookiePolicy ACCEPT_ALL = new CookiePolicy_AcceptAll ();
        public static readonly CookiePolicy ACCEPT_NONE = new CookiePolicy_AcceptNone ();
        public static readonly CookiePolicy ACCEPT_ORIGINAL_SERVER = new CookiePolicy_AcceptOriginalServer ();
    }
    internal class CookiePolicy_AcceptAll : CookiePolicy{
      public bool shouldAccept (URI uri, HttpCookie cookie) {
        return true;
      }
    } 
    internal class CookiePolicy_AcceptNone : CookiePolicy{
      public bool shouldAccept (URI uri, HttpCookie cookie) {
        return false;
      }
    } 
    internal class CookiePolicy_AcceptOriginalServer : CookiePolicy{
      public bool shouldAccept (URI uri, HttpCookie cookie) {
        throw new java.lang.UnsupportedOperationException ("Not yet implemented");
      }
    } 
}
