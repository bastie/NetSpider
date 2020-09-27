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
     * Constants for OS type.
     */
    public sealed class LeadOS : EnumIf
    {
        public static readonly LeadOS UNKNOWN = new LeadOS(EnumIfConstants._UNKNOWN, "UNKNOWN");
        public const int _LINUX = 1;
        public static readonly LeadOS LINUX = new LeadOS(_LINUX, "Linux");
        public const int _IRIX = 2;
        public static readonly LeadOS IRIX = new LeadOS(_IRIX, "IRIX");
        public const int _SUNOS5 = 3;
        public static readonly LeadOS SUNOS5 = new LeadOS(_SUNOS5, "SunOS 5");
        public const int _SUNOS4 = 4;
        public static readonly LeadOS SUNOS4 = new LeadOS(_SUNOS4, "SunOS 4");
        public const int _AIX = 5;
        public static readonly LeadOS AIX = new LeadOS(_AIX, "AIX");
        public const int _HPUX = 6;
        public static readonly LeadOS HPUX = new LeadOS(_HPUX, "HP-UX");
        public const int _OSF = 7;
        public static readonly LeadOS OSF = new LeadOS(_OSF, "OSF");
        public const int _FREEBSD = 8;
        public static readonly LeadOS FREEBSD = new LeadOS(_FREEBSD, "FreeBSD");
        public const int _SCO_SV = 9;
        public static readonly LeadOS SCO_SV = new LeadOS(_SCO_SV, "SVO SV");
        public const int _IRIX64 = 10;
        public static readonly LeadOS IRIX64 = new LeadOS(_IRIX64, "IRIX 64");
        public const int _NEXTSTEP = 11;
        public static readonly LeadOS NEXTSTEP = new LeadOS(_NEXTSTEP, "NextStep");
        public const int _BSD_OS = 12;
        public static readonly LeadOS BSD_OS = new LeadOS(_BSD_OS, "BSD OS");
        public const int _MACHTEN = 13;
        public static readonly LeadOS MACHTEN = new LeadOS(_MACHTEN, "machten");
        public const int _CYGWIN_NT = 14;
        public static readonly LeadOS CYGWIN_NT = new LeadOS(_CYGWIN_NT, "Cygwin NT");
        public const int _CYGWIN_9X = 15;
        public static readonly LeadOS CYGWIN_9X = new LeadOS(_CYGWIN_9X, "Cygwin 9x");
        public const int _UNIX_SV = 16;
        public static readonly LeadOS UNIX_SV = new LeadOS(_UNIX_SV, "UNIX SV");
        public const int _MINT = 17;
        public static readonly LeadOS MINT = new LeadOS(_MINT, "MiNT");
        public const int _OS_390 = 18;
        public static readonly LeadOS OS_390 = new LeadOS(_OS_390, "OS/390");
        public const int _VM_ESA = 19;
        public static readonly LeadOS VM_ESA = new LeadOS(_VM_ESA, "VM/ESA");
        public const int _LINUX_390 = 20;
        public static readonly LeadOS LINUX_390 = new LeadOS(_LINUX_390, "Linux OS/390");
        private EnumIf delegateJ;

        private LeadOS(int os, String name)
        {
            delegateJ = new EnumDelegate(typeof(LeadOS), os, name, this);
        }

        /**
         * Get a enum by id
         *
         * @param id The id of the enum
         * @return The enum object
         */
        public static EnumIf getEnumById(long id)
        {
            return EnumDelegate.getEnumById(typeof(LeadOS), id);
        }

        /**
         * Get a enum by name
         *
         * @param name The name of the enum
         * @return The enum object
         */
        public static EnumIf getEnumByName(String name)
        {
            return EnumDelegate.getEnumByName(typeof(LeadOS), name);
        }

        /**
         * Get all defined enums of this class
         *
         * @return An array of all defined enum objects
         */
        public static String[] getEnumNames()
        {
            return EnumDelegate.getEnumNames(typeof(LeadOS));
        }

        /**
         * Get a enum of this class by id
         *
         * @param os The id
         * @return The enum object
         */
        public static LeadOS getLeadOS(int os)
        {
            LeadOS result = (LeadOS)getEnumById(os);
            if (null == result)
            {
                switch (os)
                {
                    case _LINUX:
                        result = LINUX;
                        break;
                    case _IRIX:
                        result = IRIX;
                        break;
                    case _SUNOS5:
                        result = SUNOS5;
                        break;
                    case _SUNOS4:
                        result = SUNOS4;
                        break;
                    case _AIX:
                        result = AIX;
                        break;
                    case _HPUX:
                        result = HPUX;
                        break;
                    case _OSF:
                        result = OSF;
                        break;
                    case _FREEBSD:
                        result = FREEBSD;
                        break;
                    case _SCO_SV:
                        result = SCO_SV;
                        break;
                    case _IRIX64:
                        result = IRIX64;
                        break;
                    case _NEXTSTEP:
                        result = NEXTSTEP;
                        break;
                    case _BSD_OS:
                        result = BSD_OS;
                        break;
                    case _MACHTEN:
                        result = MACHTEN;
                        break;
                    case _CYGWIN_NT:
                        result = CYGWIN_NT;
                        break;
                    case _CYGWIN_9X:
                        result = CYGWIN_9X;
                        break;
                    case _UNIX_SV:
                        result = UNIX_SV;
                        break;
                    case _MINT:
                        result = MINT;
                        break;
                    case _OS_390:
                        result = OS_390;
                        break;
                    case _VM_ESA:
                        result = VM_ESA;
                        break;
                    case _LINUX_390:
                        result = LINUX_390;
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
            return EnumDelegate.containsEnumId(typeof(LeadOS), id);
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
            return delegateJ.ToString();
        }
    }
}