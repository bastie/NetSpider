/*
 *  Copyright 2003 jRPM Team
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System; using java = biz.ritter.javapi;
using com.jguild.jrpm.io.constant;
using com.jguild.jrpm.io.datatype;

namespace com.jguild.jrpm.io
{

    /**
     * @author kuss
     * 
     */
    public class Store
    {
        public readonly java.util.logging.Logger logger = java.util.logging.Logger.getLogger("jrpm.io");

        private java.util.HashMap<java.lang.Long, DataTypeIf> store = new java.util.HashMap<java.lang.Long, DataTypeIf>();

        /**
         * Get a tag by id as a Long
         * 
         * @param tag
         *            A tag id as a Long
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(java.lang.Long tag)
        {
            return (DataTypeIf)store.get(tag);
        }

        /**
         * Get a tag by id as a long
         * 
         * @param tag
         *            A tag id as a long
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(long tag)
        {
            return getTag(new java.lang.Long(tag));
        }

        /**
         * Get a tag by name
         * 
         * @param tagname
         *            A tag name
         * @return A data struct containing the data of this tag
         */
        public DataTypeIf getTag(String tagname)
        {
            return getTag(getTagIdForName(tagname));
        }

        /**
         * Set a tag by id as a Long
         * 
         * @param tag
         *            A tag id as a Long
         * @param data
         *            A data struct containing the data of this tag
         */
        public void setTag(java.lang.Long tag, DataTypeIf data)
        {
            isValidTag(tag.longValue());
            store.put(tag, data);
        }

        /**
         * Set a tag by id as a long
         * 
         * @param tag
         *            A tag id as a long
         * @param data
         *            A data struct containing the data of this tag
         */
        public void setTag(long tag, DataTypeIf data)
        {
            setTag(new java.lang.Long(tag), data);
        }

        /**
         * Set a tag by id as a string
         * 
         * @param tagname
         *            A tag id as a string
         * @param data
         *            A data struct containing the data of this tag
         */
        public void setTag(String tagname, DataTypeIf data)
        {
            setTag(getTagIdForName(tagname), data);
        }

        /**
         * Get all tag ids contained in this rpm file.
         * 
         * @return All tag ids contained in this rpm file.
         */
        public long[] getTagIds()
        {
            java.lang.Long[] tmp = (java.lang.Long[])store.keySet().toArray(new java.lang.Long[0]);
            long[] ret = new long[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                ret[i] = tmp[i].longValue();
            }

            return ret;
        }

        /**
         * Get all tag names contained in this rpm file.
         * 
         * @return All tag names contained in this rpm file.
         */
        public String[] getTagNames()
        {
            java.lang.Long[] tmp = (java.lang.Long[])store.keySet().toArray(new java.lang.Long[0]);
            String[] ret = new String[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                ret[i] = getTagNameForId(tmp[i].longValue());
            }

            return ret;
        }

        /**
         * Read a tag with a given tag name. The tag will be read out of the class
         * defined in getTagEnum().
         * 
         * @param tagname
         *            A RPM tag name
         * @return The id of the RPM tag
         * @throws IllegalArgumentException
         *             if the tag name was not found
         */
        public long getTagIdForName(String tagname)
        {
            EnumIf e = RPMHeaderTag.getEnumByName(tagname);

            if (e == null)
            {
                throw new java.lang.IllegalArgumentException("unknown tag with name <"
                        + tagname + ">");
            }

            return e.getId();
        }

        /**
         * Read a tag with a given tag id. The tag will be read out of the class
         * defined in getTagEnum().
         * 
         * @param tagid
         *            A RPM tag id
         * @return The name of the RPM tag
         * @throws IllegalArgumentException
         *             if the tag id was not found
         */
        public String getTagNameForId(long tagid)
        {
            EnumIf e = RPMHeaderTag.getEnumById(tagid);
            if (e == null)
            {
                throw new java.lang.IllegalArgumentException("unknown tag with id <" + tagid
                        + ">");
            }
            return e.getName();
        }

        /**
         * Test if the given tagid is associated with a valid tag
         * 
         * @param tagid
         *            The id of a tag
         * @return TRUE if the tagid is valid
         */
        public bool isValidTag(long tagid)
        {
            return RPMHeaderTag.getEnumById(tagid) != null;
        }

        /**
         * Test if the given tagname is associated with a valid tag
         * 
         * @param tagname
         *            The name of a tag
         * @return TRUE if the tagname is valid
         */
        public bool isValidTag(String tagname)
        {
            return RPMHeaderTag.getEnumByName(tagname) != null;
        }

        /**
         * Read all known tag names for this header structure.
         * 
         * @return An array of tag names
         */
        public static String[] getKnownTagNames()
        {
            return RPMHeaderTag.getEnumNames();
        }
    }
}