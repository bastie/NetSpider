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
     * Constants for Architecture type.
     *
     * @version $Id: LeadArchitecture.java,v 1.3 2003/10/20 16:32:12 mkuss Exp $
     */
    public sealed class LeadArchitecture : EnumIf
    {
        public static readonly LeadArchitecture UNKNOWN = new LeadArchitecture(EnumIfConstants._UNKNOWN, "UNKNOWN");
        public const int _I386 = 1;
        public static readonly LeadArchitecture I386 = new LeadArchitecture(_I386, "i386");
        public const int _ALPHA = 2;
        public static readonly LeadArchitecture ALPHA = new LeadArchitecture(_ALPHA, "Alpha");
        public const int _SPARC = 3;
        public static readonly LeadArchitecture SPARC = new LeadArchitecture(_SPARC, "Sparc");
        public const int _MIPS = 4;
        public static readonly LeadArchitecture MIPS = new LeadArchitecture(_MIPS, "MIPS");
        public const int _POWERPC = 5;
        public static readonly LeadArchitecture POWERPC = new LeadArchitecture(_POWERPC, "PowerPC");
        public const int _A68000 = 6;
        public static readonly LeadArchitecture A68000 = new LeadArchitecture(_A68000, "68000");
        public const int _SGI = 7;
        public static readonly LeadArchitecture SGI = new LeadArchitecture(_SGI, "SGI");
        public const int _RS6000 = 8;
        public static readonly LeadArchitecture RS6000 = new LeadArchitecture(_RS6000, "RS6000");
        public const int _IA64 = 9;
        public static readonly LeadArchitecture IA64 = new LeadArchitecture(_IA64, "IA64");
        public const int _SPARC64 = 10;
        public static readonly LeadArchitecture SPARC64 = new LeadArchitecture(_SPARC64, "Sparc64");
        public const int _MIPSEL = 11;
        public static readonly LeadArchitecture MIPSEL = new LeadArchitecture(_MIPSEL, "Mipsel");
        public const int _ARM = 12;
        public static readonly LeadArchitecture ARM = new LeadArchitecture(_ARM, "ARM");
        public const int _M68KMINT = 13;
        public static readonly LeadArchitecture M68KMINT = new LeadArchitecture(_M68KMINT, "m68kmint");
        public const int _S390 = 14;
        public static readonly LeadArchitecture S390 = new LeadArchitecture(_S390, "S/390");
        public const int _S390X = 15;
        public static readonly LeadArchitecture S390X = new LeadArchitecture(_S390X, "S/390x");
        private EnumIf delegateJ;

        private LeadArchitecture(int architecture, String name)
        {
            delegateJ = new EnumDelegate(typeof(LeadArchitecture), architecture, name, this);
        }

        /**
         * Get a enum by id
         *
         * @param id The id of the enum
         * @return The enum object
         */
        public static EnumIf getEnumById(long id)
        {
            return EnumDelegate.getEnumById(typeof(LeadArchitecture), id);
        }

        /**
         * Get a enum by name
         *
         * @param name The name of the enum
         * @return The enum object
         */
        public static EnumIf getEnumByName(String name)
        {
            return EnumDelegate.getEnumByName(typeof(LeadArchitecture), name);
        }

        /**
         * Get all defined enums of this class
         *
         * @return An array of all defined enum objects
         */
        public static String[] getEnumNames()
        {
            return EnumDelegate.getEnumNames(typeof(LeadArchitecture));
        }

        /**
         * Get a enum of this class by id
         *
         * @param architecture The id
         * @return The enum object
         */
        public static LeadArchitecture getLeadArchitecture(int architecture)
        {
            LeadArchitecture result = (LeadArchitecture)getEnumById(architecture);
            if (null == result) {
                switch(architecture) {
                    case _I386:
                    result = I386 ;
                        break;
                    case _ALPHA:
                    result = ALPHA;
                        break;
                    case _SPARC:
                    result = SPARC ;
                        break;
                    case _MIPS :
                    result = MIPS ;
                        break;
                    case _POWERPC:
                    result = POWERPC;
                        break;
                    case _A68000 :
                    result = A68000 ;
                        break;
                    case _SGI :
                    result = SGI ;
                        break;
                    case _RS6000:
                    result = RS6000 ;
                        break;
                    case _IA64 :
                    result = IA64;
                        break;
                    case _SPARC64:
                    result = SPARC64 ;
                        break;
                    case _MIPSEL :
                    result = MIPSEL;
                        break;
                    case _ARM :
                    result = ARM;
                        break;
                    case _M68KMINT:
                    result = M68KMINT;
                        break;
                    case _S390 :
                    result = S390;
                        break;
                    case _S390X:
                    result = S390X;
                        break;
                    default:
                    result = UNKNOWN;
                        break;
                }
            }
            return result;
        }

        /**
         * Check if this enum class contains a enum of a specified id
         *
         * @param id The id of the enum
         * @return TRUE if the enum is defined in this class
         */
        public static bool containsEnumId(java.lang.Long id)
        {
            return EnumDelegate.containsEnumId(typeof(LeadArchitecture), id);
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
        public String toString()
        {
            return delegateJ.toString();
        }
    }
}