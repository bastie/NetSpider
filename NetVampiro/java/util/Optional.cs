/* C# reimpl 
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
 */

using System;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util {

    public sealed class Optional<T> : java.io.Serializable {

        private static readonly Optional<T> EMPTY = new Optional<T>();

        private readonly T value;

        private Optional() {
            this.value = default(T);
        }

        private Optional(T value) {
            this.value = value;
        }

        public static Optional<T> empty() {
            Optional<T> t = (Optional<T>) EMPTY;
            return t;
        }

        public static Optional<T> of(T value) {
            return new Optional<T>(value);
        }

        public static Optional<T> ofNullable(T value) {
            if (null == value) {
                return empty();
            }
            else {
                return of(value);
            }
        }

        public T get() {
            return value;
        }

        public T orElse(T other) {
            return value != null ? value : other;
        }

        public bool isPresent() {
            return value != null;
        }

        public T or(T other) {
            return value != null ? value : other;
        }


        public override bool Equals(Object obj) {
            if (!(obj is Optional<T>)) {
              return false;
            }
            Optional<T> other = (Optional<T>) obj;
            if (!other.isPresent() && !this.isPresent()) return true;
            if (other.isPresent()) {
                return (other.value.Equals(this.value));
            }
            else {
                return this.value.Equals(other.value);
            }
        }


        
        public override int GetHashCode() {
            return value == null ? 0 : value.GetHashCode();
        }

        public override String ToString() {
            return value == null ? "Optional.empty" : String.Format("Optional[{0}]", value);
        }

    }
}