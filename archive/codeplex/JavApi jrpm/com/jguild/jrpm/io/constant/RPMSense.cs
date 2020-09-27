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
namespace com.jguild.jrpm.io.constant
{


    /**
     * Bit flags for dependency fields. (e.g. RPMHeaderTag.REQUIRE_FLAGS,
     * RPMHeaderTag.CONFLICT_FLAGS)
     *
     * @author kuss
     */
    public class RPMSense : FlagIf
    {
        public const int _ANY = 0;
        public static readonly RPMSense ANY = new RPMSense(_ANY);

        /** @todo Legacy.  */
        public const int _SERIAL = (1 << 0);
        public static readonly RPMSense SERIAL = new RPMSense(_SERIAL);
        public const int _LESS = (1 << 1);
        public static readonly RPMSense LESS = new RPMSense(_LESS);
        public const int _GREATER = (1 << 2);
        public static readonly RPMSense GREATER = new RPMSense(_GREATER);
        public const int _EQUAL = (1 << 3);
        public static readonly RPMSense EQUAL = new RPMSense(_EQUAL);

        /** only used internally by builds */
        public const int _PROVIDES = (1 << 4);
        public static readonly RPMSense PROVIDES = new RPMSense(_PROVIDES);

        /** only used internally by builds */
        public const int _CONFLICTS = (1 << 5);
        public static readonly RPMSense CONFLICTS = new RPMSense(_CONFLICTS);

        /** @todo Legacy.  */
        public const int _PREREQ = (1 << 6);
        public static readonly RPMSense PREREQ = new RPMSense(_PREREQ);

        /** only used internally by builds */
        public const int _OBSOLETES = (1 << 7);
        public static readonly RPMSense OBSOLETES = new RPMSense(_OBSOLETES);

        /** Interpreter used by scriptlet.  */
        public const int _INTERP = (1 << 8);
        public static readonly RPMSense INTERP = new RPMSense(_INTERP);

        /** %pre dependency.  */
        public const int _SCRIPT_PRE = ((1 << 9) | _PREREQ);
        public static readonly RPMSense SCRIPT_PRE = new RPMSense(_SCRIPT_PRE);

        /** %post dependency.  */
        public const int _SCRIPT_POST = ((1 << 10) | _PREREQ);
        public static readonly RPMSense SCRIPT_POST = new RPMSense(_SCRIPT_POST);

        /** %preun dependency.  */
        public const int _SCRIPT_PREUN = ((1 << 11) | _PREREQ);
        public static readonly RPMSense SCRIPT_PREUN = new RPMSense(_SCRIPT_PREUN);

        /** %postun dependency.  */
        public const int _SCRIPT_POSTUN = ((1 << 12) | _PREREQ);
        public static readonly RPMSense SCRIPT_POSTUN = new RPMSense(_SCRIPT_POSTUN);

        /** %verify dependency.  */
        public const int _SCRIPT_VERIFY = (1 << 13);
        public static readonly RPMSense SCRIPT_VERIFY = new RPMSense(_SCRIPT_VERIFY);

        /** find-requires generated dependency.  */
        public const int _FIND_REQUIRES = (1 << 14);
        public static readonly RPMSense FIND_REQUIRES = new RPMSense(_FIND_REQUIRES);

        /** find-provides generated dependency.  */
        public const int _FIND_PROVIDES = (1 << 15);
        public static readonly RPMSense FIND_PROVIDES = new RPMSense(_FIND_PROVIDES);

        /** %triggerin dependency.  */
        public const int _TRIGGERIN = (1 << 16);
        public static readonly RPMSense TRIGGERIN = new RPMSense(_TRIGGERIN);

        /** %triggerun dependency.  */
        public const int _TRIGGERUN = (1 << 17);
        public static readonly RPMSense TRIGGERUN = new RPMSense(_TRIGGERUN);

        /** %triggerpostun dependency.  */
        public const int _TRIGGERPOSTUN = (1 << 18);
        public static readonly RPMSense TRIGGERPOSTUN = new RPMSense(_TRIGGERPOSTUN);

        // (1 << 19) unused.

        /** %prep build dependency.  */
        public const int _SCRIPT_PREP = (1 << 20);
        public static readonly RPMSense SCRIPT_PREP = new RPMSense(_SCRIPT_PREP);

        /** %build build dependency.  */
        public const int _SCRIPT_BUILD = (1 << 21);
        public static readonly RPMSense SCRIPT_BUILD = new RPMSense(_SCRIPT_BUILD);

        /** %install build dependency.  */
        public const int _SCRIPT_INSTALL = (1 << 22);
        public static readonly RPMSense SCRIPT_INSTALL = new RPMSense(_SCRIPT_INSTALL);

        /** %clean build dependency.  */
        public const int _SCRIPT_CLEAN = (1 << 23);
        public static readonly RPMSense SCRIPT_CLEAN = new RPMSense(_SCRIPT_CLEAN);

        /** rpmlib(feature) dependency.  */
        public const int _RPMLIB = ((1 << 24) | _PREREQ);
        public static readonly RPMSense RPMLIB = new RPMSense(_RPMLIB);

        /** @todo Implement %triggerprein.  */
        public const int _TRIGGERPREIN = (1 << 25);
        public static readonly RPMSense TRIGGERPREIN = new RPMSense(_TRIGGERPREIN);
        public const int _KEYRING = (1 << 26);
        public static readonly RPMSense KEYRING = new RPMSense(_KEYRING);
        public const int _PATCHES = (1 << 27);
        public static readonly RPMSense PATCHES = new RPMSense(_PATCHES);
        public const int _CONFIG = (1 << 28);
        public static readonly RPMSense CONFIG = new RPMSense(_CONFIG);

        // Some masks

        /**
         * Mask to get senses, ie serial,
         * less, greater, equal.
         */
        public const int _SENSEMASK = 15;
        public static readonly RPMSense SENSEMASK = new RPMSense(_SENSEMASK);
        public const int _TRIGGER = _TRIGGERIN | _TRIGGERUN | _TRIGGERPOSTUN;
        public static readonly RPMSense TRIGGER = new RPMSense(_TRIGGER);
        public const int _ALL_REQUIRES_MASK = _INTERP | _SCRIPT_PRE | _SCRIPT_POST | _SCRIPT_PREUN | _SCRIPT_POSTUN | _SCRIPT_VERIFY | _FIND_REQUIRES |
            _SCRIPT_PREP | _SCRIPT_BUILD | _SCRIPT_INSTALL | _SCRIPT_CLEAN | _RPMLIB | _KEYRING;
        public static readonly RPMSense ALL_REQUIRES_MASK = new RPMSense(_ALL_REQUIRES_MASK);
        private readonly int flag;

        private RPMSense(int flag)
        {
            this.flag = flag;
        }

        /*
         * @see com.jguild.jrpm.io.constant.FlagIf#value()
         */
        public int value()
        {
            return flag;
        }
    }
}