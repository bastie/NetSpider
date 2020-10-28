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
    [Serializable]
  public abstract class Enum<E> : java.io.Serializable, java.lang.Comparable<E>, java.lang.constant.Constable, java.lang.Cloneable {

        private readonly String enumName;
        private readonly int enumOrdinal;
  
        public Enum (String name, int ordinal) {
            this.enumName = name;
            this.enumOrdinal = ordinal;
        }

        public String name() {
            return this.enumName;
        }
        public int ordinal() {
            return this.enumOrdinal;
        }

        public abstract int compareTo (E other);
        public abstract java.util.Optional<java.lang.constant.ConstantDesc> describeConstable();

        public Object clone() {
            throw new java.lang.CloneNotSupportedException ("Enums never be clonable.");
        }
        protected new Enum<E> MemberwiseClone () {
            return (Enum<E>) this.clone(); // throws ever ever an Exception, see Javadoc
        }
  }

}