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
    public interface CookieStore {
      void add(URI uri, HttpCookie cookie);
      java.util.List<HttpCookie> get(URI uri);
      java.util.List<HttpCookie> getCookies();
      java.util.List<URI> getURIs();
      bool remove(URI uri, HttpCookie cookie);
      bool removeAll();
    }
}