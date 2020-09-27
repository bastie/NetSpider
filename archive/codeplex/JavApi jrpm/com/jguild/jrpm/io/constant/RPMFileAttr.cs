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
     * File attributes. (e.g. RPMHeaderTag.FILEFLAGS)
     *
     * @author kuss
     */
    public class RPMFileAttr : FlagIf
    {
        public const int _NONE = 0;
        public static readonly RPMFileAttr NONE = new RPMFileAttr(_NONE);

        /** from %%config */
        public const int _CONFIG = (1 << 0);
        public static readonly RPMFileAttr CONFIG = new RPMFileAttr(_CONFIG);

        /** from %%doc */
        public const int _DOC = (1 << 1);
        public static readonly RPMFileAttr DOC = new RPMFileAttr(_DOC);

        /** from %%donotuse. */
        public const int _ICON = (1 << 2);
        public static readonly RPMFileAttr ICON = new RPMFileAttr(_ICON);

        /** from %%config(missingok) */
        public const int _MISSINGOK = (1 << 3);
        public static readonly RPMFileAttr MISSINGOK = new RPMFileAttr(_MISSINGOK);

        /** from %%config(noreplace) */
        public const int _NOREPLACE = (1 << 4);
        public static readonly RPMFileAttr NOREPLACE = new RPMFileAttr(_NOREPLACE);

        /** @todo (unnecessary) marks 1st file in srpm. */
        public const int _SPECFILE = (1 << 5);
        public static readonly RPMFileAttr SPECFILE = new RPMFileAttr(_SPECFILE);

        /** from %%ghost */
        public const int _GHOST = (1 << 6);
        public static readonly RPMFileAttr GHOST = new RPMFileAttr(_GHOST);

        /** from %%license */
        public const int _LICENSE = (1 << 7);
        public static readonly RPMFileAttr LICENSE = new RPMFileAttr(_LICENSE);

        /** from %%readme */
        public const int _README = (1 << 8);
        public static readonly RPMFileAttr README = new RPMFileAttr(_README);

        /** from %%exclude */
        public const int _EXCLUDE = (1 << 9);
        public static readonly RPMFileAttr EXCLUDE = new RPMFileAttr(_EXCLUDE);

        /** placeholder (SuSE) */
        public const int _UNPATCHED = (1 << 10);
        public static readonly RPMFileAttr UNPATCHED = new RPMFileAttr(_UNPATCHED);

        /** from %%pubkey */
        public const int _PUBKEY = (1 << 11);
        public static readonly RPMFileAttr PUBKEY = new RPMFileAttr(_PUBKEY);
        public const int _ALL = ~(_NONE);
        public static readonly RPMFileAttr ALL = new RPMFileAttr(_ALL);
        private readonly int flag;

        private RPMFileAttr(int flag)
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