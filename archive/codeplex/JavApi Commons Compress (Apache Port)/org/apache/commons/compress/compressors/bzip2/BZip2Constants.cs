/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace org.apache.commons.compress.compressors.bzip2
{

    /**
     * Constants for both the compress and decompress BZip2 classes.
     */
    public class BZip2Constants // Basties note: because only constants define - use class
    {

        internal protected static int BASEBLOCKSIZE = 100000;
        internal protected static int MAX_ALPHA_SIZE = 258;
        internal protected static int MAX_CODE_LEN = 23;
        internal protected static int RUNA = 0;
        internal protected static int RUNB = 1;
        internal protected static int N_GROUPS = 6;
        internal protected static int G_SIZE = 50;
        internal protected static int N_ITERS = 4;
        internal protected static int MAX_SELECTORS = (2 + (900000 / G_SIZE));
        internal protected static int NUM_OVERSHOOT_BYTES = 20;

    }
}