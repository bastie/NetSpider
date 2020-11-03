/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Copyright © 2020 Sebastian Ritter
 */

using System;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util {

    public sealed class Objects {
        
        public static bool equals (Object a, Object b) {
            if (null == a && null == b) return true;
            if (null == a && null != b) return false;
            if (null != a && null == b) return false;
            return a.equals(b);
        }

        public static int hash (params Object [] objs) {
            int result = 0;
            if (null != objs && objs.Length > 0) {
                result = objs[0].GetHashCode();
                Object lastObj = objs[0];
                for (int i = 1; i < objs.Length; i++) {
                    result = System.HashCode.Combine(lastObj, objs[i]);
                }
            }
            return result;
        }

        public static bool isNull(Object obj) { // Java8
            return null == obj;
        }
        public static bool nonNull(Object obj) { // Java8
            return null != obj;
        }

    }

}