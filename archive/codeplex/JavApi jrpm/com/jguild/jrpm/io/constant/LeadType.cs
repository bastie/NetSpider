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
using System;
using java = biz.ritter.javapi;

namespace com.jguild.jrpm.io.constant
{


    /**
     * Constants for Lead type.
     *
     * @version $Id: LeadType.java,v 1.3 2003/10/20 16:32:12 mkuss Exp $
     */
    public sealed class LeadType : EnumIf
    {
        public static readonly LeadType UNKNOWN = new LeadType(EnumIfConstants._UNKNOWN, "UNKNOWN");
        public const int _BINARY = 0;
        public static readonly LeadType BINARY = new LeadType(_BINARY, "BINARY");
        public const int _SOURCE = 1;
        public static readonly LeadType SOURCE = new LeadType(_SOURCE, "SOURCE");
        private EnumIf delegateJ;

        private LeadType(int type, String name)
        {
            delegateJ = new EnumDelegate(typeof(LeadType), type, name, this);
        }

        /**
         * Get a enum by id
         *
         * @param id The id of the enum
         * @return The enum object
         */
        public static EnumIf getEnumById(long id)
        {
            EnumIf result = EnumDelegate.getEnumById(typeof(LeadType), id);
            if (null == result)
            {
                switch (id)
                {
                    case _BINARY :
                        result = LeadType.BINARY;
                        break;
                    case _SOURCE:
                        result = LeadType.SOURCE;
                        break;
                    default:
                        result = LeadType.UNKNOWN;
                        break;
                }
            }
            return result;
        }

        /**
         * Get a enum by name
         *
         * @param name The name of the enum
         * @return The enum object
         */
        public static EnumIf getEnumByName(String name)
        {
            return EnumDelegate.getEnumByName(typeof(LeadType), name);
        }

        /**
         * Get all defined enums of this class
         *
         * @return An array of all defined enum objects
         */
        public static String[] getEnumNames()
        {
            return EnumDelegate.getEnumNames(typeof(LeadType));
        }

        /**
         * Get a enum of this class by id
         *
         * @param type The id
         * @return The enum object
         */
        public static LeadType getLeadType(int type)
        {
            return (LeadType)getEnumById(type);
        }

        /**
         * Check if this enum class contains a enum of a specified id
         *
         * @param id The id of the enum
         * @return TRUE if the enum is defined in this class
         */
        public static bool containsEnumId(java.lang.Long id)
        {
            return EnumDelegate.containsEnumId(typeof(LeadType), id);
        }

        /*
         * @see com.jguild.jrpm.io.constant.EnumIf#getId()
         */
        public long getId()
        {
            return delegateJ.getId();
        }

        /*
         * @see com.jguild.jrpm.io.constant.EnumIf#getName()
         */
        public String getName()
        {
            return delegateJ.getName();
        }

        /*
         * @see java.lang.Object#toString()
         */
        public override String ToString()
        {
            return delegateJ.toString();
        }
    }
}