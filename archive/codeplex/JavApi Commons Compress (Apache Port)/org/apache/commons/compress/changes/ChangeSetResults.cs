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
using System;
using java = biz.ritter.javapi;

namespace org.apache.commons.compress.changes {

    /* Basties note: Java has no elegant implementation, because generics not used. The descriptions
     * desribe String objects are returning, but "Object" lists are implemented as return values.
     */
    /// <summary>
    /// Stores the results of an performed ChangeSet operation. 
    /// </summary>
    public class ChangeSetResults {
        private java.util.List<Object> addedFromChangeSetList = new java.util.ArrayList<Object>();
        private java.util.List<Object> addedFromStreamList = new java.util.ArrayList<Object>();
        private java.util.List<Object> deletedList = new java.util.ArrayList<Object>();
    
        /**
         * Adds the filename of a recently deleted file to the result list.
         * @param fileName the file which has been deleted
         */
        protected internal void deleted(String fileName) {
            deletedList.add(fileName);
        }
    
        /**
         * Adds the name of a file to the result list which has been 
         * copied from the source stream to the target stream.
         * @param fileName the file name which has been added from the original stream
         */
        protected internal void addedFromStream(String fileName) {
            addedFromStreamList.add(fileName);
        }
    
        /**
         * Adds the name of a file to the result list which has been
         * copied from the changeset to the target stream
         * @param fileName the name of the file
         */
        protected internal void addedFromChangeSet(String fileName) {
            addedFromChangeSetList.add(fileName);
        }

        /**
         * Returns a list of filenames which has been added from the changeset
         * @return the list of filenames
         */
        public java.util.List<Object> getAddedFromChangeSet()
        {
            return addedFromChangeSetList;
        }

        /**
         * Returns a list of filenames which has been added from the original stream
         * @return the list of filenames
         */
        public java.util.List<Object> getAddedFromStream()
        {
            return addedFromStreamList;
        }

        /**
         * Returns a list of filenames which has been deleted
         * @return the list of filenames
         */
        public java.util.List<Object> getDeleted()
        {
            return deletedList;
        }
    
        /**
         * Checks if an filename already has been added to the result list
         * @param filename the filename to check
         * @return true, if this filename already has been added
         */
        protected internal bool hasBeenAdded(String filename) {
            if (addedFromChangeSetList.contains(filename) || addedFromStreamList.contains(filename))
            {
                return true;
            } 
            return false;
        }
    }
}