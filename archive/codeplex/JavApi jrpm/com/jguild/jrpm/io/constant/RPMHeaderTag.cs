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
     * Constants for tags.
     * 
     * @version $Id: RPMHeaderTag.java,v 1.4 2004/05/06 20:59:24 mkuss Exp $
     */
    public sealed class RPMHeaderTag : EnumIf
    {
        public static readonly RPMHeaderTag UNKNOWN = new RPMHeaderTag(EnumIfConstants._UNKNOWN,
            "UNKNOWN");

        public const int _HEADERIMAGE = 61;

        public static readonly RPMHeaderTag HEADERIMAGE = new RPMHeaderTag(
            _HEADERIMAGE, "HEADERIMAGE");

        public const int _HEADERSIGNATURES = 62;

        public static readonly RPMHeaderTag HEADERSIGNATURES = new RPMHeaderTag(
            _HEADERSIGNATURES, "HEADERSIGNATURES");

        public const int _HEADERIMMUTABLE = 63;

        public static readonly RPMHeaderTag HEADERIMMUTABLE = new RPMHeaderTag(
            _HEADERIMMUTABLE, "HEADERIMMUTABLE");

        public const int _HEADERREGIONS = 64;

        public static readonly RPMHeaderTag HEADERREGIONS = new RPMHeaderTag(
            _HEADERREGIONS, "HEADERREGIONS");

        public const int _HEADERI18NTABLE = 100;

        public static readonly RPMHeaderTag HEADERI18NTABLE = new RPMHeaderTag(
            _HEADERI18NTABLE, "HEADERI18NTABLE");

        public const int _SIG_BASE = 256;

        public static readonly RPMHeaderTag SIG_BASE = new RPMHeaderTag(_SIG_BASE,
            "SIG_BASE");

        public const int _SIGSIZE = _SIG_BASE + 1;

        public static readonly RPMHeaderTag SIGSIZE = new RPMHeaderTag(_SIGSIZE,
            "SIGSIZE");

        public const int _SIGLEMD5_1 = _SIG_BASE + 2;

        public static readonly RPMHeaderTag SIGLEMD5_1 = new RPMHeaderTag(_SIGLEMD5_1,
            "SIGLEMD5_1");

        public const int _SIGPGP = _SIG_BASE + 3;

        public static readonly RPMHeaderTag SIGPGP = new RPMHeaderTag(_SIGPGP,
            "SIGPGP");

        public const int _SIGLEMD5_2 = _SIG_BASE + 4;

        public static readonly RPMHeaderTag SIGLEMD5_2 = new RPMHeaderTag(_SIGLEMD5_2,
            "SIGLEMD5_2");

        public const int _SIGMD5 = _SIG_BASE + 5;

        public static readonly RPMHeaderTag SIGMD5 = new RPMHeaderTag(_SIGMD5,
            "SIGMD5");

        public static readonly RPMHeaderTag PKGID = new RPMHeaderTag(_SIGMD5, "PKGID");

        public const int _SIGGPG = _SIG_BASE + 6;

        public static readonly RPMHeaderTag SIGGPG = new RPMHeaderTag(_SIGGPG,
            "SIGGPG");

        public const int _SIGPGP5 = _SIG_BASE + 7;

        public static readonly RPMHeaderTag SIGPGP5 = new RPMHeaderTag(_SIGPGP5,
            "SIGPGP5");

        public const int _BADSHA1_1 = _SIG_BASE + 8;

        public static readonly RPMHeaderTag BADSHA1_1 = new RPMHeaderTag(_BADSHA1_1,
            "BADSHA1_1");

        public const int _BADSHA1_2 = _SIG_BASE + 9;

        public static readonly RPMHeaderTag BADSHA1_2 = new RPMHeaderTag(_BADSHA1_2,
            "BADSHA1_2");

        public const int _PUBKEYS = _SIG_BASE + 10;

        public static readonly RPMHeaderTag PUBKEYS = new RPMHeaderTag(_PUBKEYS,
            "PUBKEYS");

        public const int _DSAHEADER = _SIG_BASE + 11;

        public static readonly RPMHeaderTag DSAHEADER = new RPMHeaderTag(_DSAHEADER,
            "DSAHEADER");

        public const int _RSAHEADER = _SIG_BASE + 12;

        public static readonly RPMHeaderTag RSAHEADER = new RPMHeaderTag(_RSAHEADER,
            "RSAHEADER");

        public const int _SHA1HEADER = _SIG_BASE + 13;

        public static readonly RPMHeaderTag SHA1HEADER = new RPMHeaderTag(_SHA1HEADER,
            "SHA1HEADER");

        public static readonly RPMHeaderTag HDRID = new RPMHeaderTag(_SHA1HEADER,
            "HDRID");

        public const int _NAME = 1000;

        public static readonly RPMHeaderTag NAME = new RPMHeaderTag(_NAME, "NAME");

        public static readonly RPMHeaderTag N = new RPMHeaderTag(_NAME, "N");

        public const int _VERSION = 1001;

        public static readonly RPMHeaderTag VERSION = new RPMHeaderTag(_VERSION,
            "VERSION");

        public static readonly RPMHeaderTag V = new RPMHeaderTag(_VERSION, "V");

        public const int _RELEASE = 1002;

        public static readonly RPMHeaderTag RELEASE = new RPMHeaderTag(_RELEASE,
            "RELEASE");

        public static readonly RPMHeaderTag R = new RPMHeaderTag(_RELEASE, "R");

        public const int _EPOCH = 1003;

        public static readonly RPMHeaderTag EPOCH = new RPMHeaderTag(_EPOCH, "EPOCH");

        public static readonly RPMHeaderTag E = new RPMHeaderTag(_EPOCH, "E");

        /* backward comaptibility */
        public static readonly RPMHeaderTag SERIAL = new RPMHeaderTag(_EPOCH, "SERIAL");

        public const int _SUMMARY = 1004;

        public static readonly RPMHeaderTag SUMMARY = new RPMHeaderTag(_SUMMARY,
            "SUMMARY");

        public const int _DESCRIPTION = 1005;

        public static readonly RPMHeaderTag DESCRIPTION = new RPMHeaderTag(
            _DESCRIPTION, "DESCRIPTION");

        public const int _BUILDTIME = 1006;

        public static readonly RPMHeaderTag BUILDTIME = new RPMHeaderTag(_BUILDTIME,
            "BUILDTIME");

        public const int _BUILDHOST = 1007;

        public static readonly RPMHeaderTag BUILDHOST = new RPMHeaderTag(_BUILDHOST,
            "BUILDHOST");

        public const int _INSTALLTIME = 1008;

        public static readonly RPMHeaderTag INSTALLTIME = new RPMHeaderTag(
            _INSTALLTIME, "INSTALLTIME");

        public const int _SIZE = 1009;

        public static readonly RPMHeaderTag SIZE = new RPMHeaderTag(_SIZE, "SIZE");

        public const int _DISTRIBUTION = 1010;

        public static readonly RPMHeaderTag DISTRIBUTION = new RPMHeaderTag(
            _DISTRIBUTION, "DISTRIBUTION");

        public const int _VENDOR = 1011;

        public static readonly RPMHeaderTag VENDOR = new RPMHeaderTag(_VENDOR,
            "VENDOR");

        public const int _GIF = 1012;

        public static readonly RPMHeaderTag GIF = new RPMHeaderTag(_GIF, "GIF");

        public const int _XPM = 1013;

        public static readonly RPMHeaderTag XPM = new RPMHeaderTag(_XPM, "XPM");

        public const int _LICENSE = 1014;

        public static readonly RPMHeaderTag LICENSE = new RPMHeaderTag(_LICENSE,
            "LICENSE");

        /* backward comaptibility */
        public static readonly RPMHeaderTag COPYRIGHT = new RPMHeaderTag(_LICENSE,
            "COPYRIGHT");

        public const int _PACKAGER = 1015;

        public static readonly RPMHeaderTag PACKAGER = new RPMHeaderTag(_PACKAGER,
            "PACKAGER");

        public const int _GROUP = 1016;

        public static readonly RPMHeaderTag GROUP = new RPMHeaderTag(_GROUP, "GROUP");

        public const int _CHANGELOG = 1017;

        public static readonly RPMHeaderTag CHANGELOG = new RPMHeaderTag(_CHANGELOG,
            "CHANGELOG");

        public const int _SOURCE = 1018;

        public static readonly RPMHeaderTag SOURCE = new RPMHeaderTag(_SOURCE,
            "SOURCE");

        public const int _PATCH = 1019;

        public static readonly RPMHeaderTag PATCH = new RPMHeaderTag(_PATCH, "PATCH");

        public const int _URL = 1020;

        public static readonly RPMHeaderTag URL = new RPMHeaderTag(_URL, "URL");

        public const int _OS = 1021;

        public static readonly RPMHeaderTag OS = new RPMHeaderTag(_OS, "OS");

        public const int _ARCH = 1022;

        public static readonly RPMHeaderTag ARCH = new RPMHeaderTag(_ARCH, "ARCH");

        public const int _PREIN = 1023;

        public static readonly RPMHeaderTag PREIN = new RPMHeaderTag(_PREIN, "PREIN");

        public const int _POSTIN = 1024;

        public static readonly RPMHeaderTag POSTIN = new RPMHeaderTag(_POSTIN,
            "POSTIN");

        public const int _PREUN = 1025;

        public static readonly RPMHeaderTag PREUN = new RPMHeaderTag(_PREUN, "PREUN");

        public const int _POSTUN = 1026;

        public static readonly RPMHeaderTag POSTUN = new RPMHeaderTag(_POSTUN,
            "POSTUN");

        public const int _OLDFILENAMES = 1027;

        public static readonly RPMHeaderTag OLDFILENAMES = new RPMHeaderTag(
            _OLDFILENAMES, "OLDFILENAMES");

        public const int _FILESIZES = 1028;

        public static readonly RPMHeaderTag FILESIZES = new RPMHeaderTag(_FILESIZES,
            "FILESIZES");

        public const int _FILESTATES = 1029;

        public static readonly RPMHeaderTag FILESTATES = new RPMHeaderTag(_FILESTATES,
            "FILESTATES");

        public const int _FILEMODES = 1030;

        public static readonly RPMHeaderTag FILEMODES = new RPMHeaderTag(_FILEMODES,
            "FILEMODES");

        public const int _FILEUIDS = 1031;

        public static readonly RPMHeaderTag FILEUIDS = new RPMHeaderTag(_FILEUIDS,
            "FILEUIDS");

        public const int _FILEGIDS = 1032;

        public static readonly RPMHeaderTag FILEGIDS = new RPMHeaderTag(_FILEGIDS,
            "FILEGIDS");

        public const int _FILERDEVS = 1033;

        public static readonly RPMHeaderTag FILERDEVS = new RPMHeaderTag(_FILERDEVS,
            "FILERDEVS");

        public const int _FILEMTIMES = 1034;

        public static readonly RPMHeaderTag FILEMTIMES = new RPMHeaderTag(_FILEMTIMES,
            "FILEMTIMES");

        public const int _FILEMD5S = 1035;

        public static readonly RPMHeaderTag FILEMD5S = new RPMHeaderTag(_FILEMD5S,
            "FILEMD5S");

        public const int _FILELINKTOS = 1036;

        public static readonly RPMHeaderTag FILELINKTOS = new RPMHeaderTag(
            _FILELINKTOS, "FILELINKTOS");

        public const int _FILEFLAGS = 1037;

        public static readonly RPMHeaderTag FILEFLAGS = new RPMHeaderTag(_FILEFLAGS,
            "FILEFLAGS");

        public const int _ROOT = 1038;

        public static readonly RPMHeaderTag ROOT = new RPMHeaderTag(_ROOT, "ROOT");

        public const int _FILEUSERNAME = 1039;

        public static readonly RPMHeaderTag FILEUSERNAME = new RPMHeaderTag(
            _FILEUSERNAME, "FILEUSERNAME");

        public const int _FILEGROUPNAME = 1040;

        public static readonly RPMHeaderTag FILEGROUPNAME = new RPMHeaderTag(
            _FILEGROUPNAME, "FILEGROUPNAME");

        public const int _EXCLUDE = 1041;

        public static readonly RPMHeaderTag EXCLUDE = new RPMHeaderTag(_EXCLUDE,
            "EXCLUDE");

        public const int _EXCLUSIVE = 1042;

        public static readonly RPMHeaderTag EXCLUSIVE = new RPMHeaderTag(_EXCLUSIVE,
            "EXCLUSIVE");

        public const int _ICON = 1043;

        public static readonly RPMHeaderTag ICON = new RPMHeaderTag(_ICON, "ICON");

        public const int _SOURCERPM = 1044;

        public static readonly RPMHeaderTag SOURCERPM = new RPMHeaderTag(_SOURCERPM,
            "SOURCERPM");

        public const int _FILEVERIFYFLAGS = 1045;

        public static readonly RPMHeaderTag FILEVERIFYFLAGS = new RPMHeaderTag(
            _FILEVERIFYFLAGS, "FILEVERIFYFLAGS");

        public const int _ARCHIVESIZE = 1046;

        public static readonly RPMHeaderTag ARCHIVESIZE = new RPMHeaderTag(
            _ARCHIVESIZE, "ARCHIVESIZE");

        public const int _PROVIDENAME = 1047;

        public static readonly RPMHeaderTag PROVIDENAME = new RPMHeaderTag(
            _PROVIDENAME, "PROVIDENAME");

        /* backward comaptibility */
        public static readonly RPMHeaderTag PROVIDES = new RPMHeaderTag(_PROVIDENAME,
            "PROVIDES");

        public const int _REQUIREFLAGS = 1048;

        public static readonly RPMHeaderTag REQUIREFLAGS = new RPMHeaderTag(
            _REQUIREFLAGS, "REQUIREFLAGS");

        public const int _REQUIRENAME = 1049;

        public static readonly RPMHeaderTag REQUIRENAME = new RPMHeaderTag(
            _REQUIRENAME, "REQUIRENAME");

        public const int _REQUIREVERSION = 1050;

        public static readonly RPMHeaderTag REQUIREVERSION = new RPMHeaderTag(
            _REQUIREVERSION, "REQUIREVERSION");

        public const int _NOSOURCE = 1051;

        public static readonly RPMHeaderTag NOSOURCE = new RPMHeaderTag(_NOSOURCE,
            "NOSOURCE");

        public const int _NOPATCH = 1052;

        public static readonly RPMHeaderTag NOPATCH = new RPMHeaderTag(_NOPATCH,
            "NOPATCH");

        public const int _CONFLICTFLAGS = 1053;

        public static readonly RPMHeaderTag CONFLICTFLAGS = new RPMHeaderTag(
            _CONFLICTFLAGS, "CONFLICTFLAGS");

        public const int _CONFLICTNAME = 1054;

        public static readonly RPMHeaderTag CONFLICTNAME = new RPMHeaderTag(
            _CONFLICTNAME, "CONFLICTNAME");

        public const int _CONFLICTVERSION = 1055;

        public static readonly RPMHeaderTag CONFLICTVERSION = new RPMHeaderTag(
            _CONFLICTVERSION, "CONFLICTVERSION");

        public const int _DEFAULTPREFIX = 1056;

        public static readonly RPMHeaderTag DEFAULTPREFIX = new RPMHeaderTag(
            _DEFAULTPREFIX, "DEFAULTPREFIX");

        public const int _BUILDROOT = 1057;

        public static readonly RPMHeaderTag BUILDROOT = new RPMHeaderTag(_BUILDROOT,
            "BUILDROOT");

        public const int _INSTALLPREFIX = 1058;

        public static readonly RPMHeaderTag INSTALLPREFIX = new RPMHeaderTag(
            _INSTALLPREFIX, "INSTALLPREFIX");

        public const int _EXCLUDEARCH = 1059;

        public static readonly RPMHeaderTag EXCLUDEARCH = new RPMHeaderTag(
            _EXCLUDEARCH, "EXCLUDEARCH");

        public const int _EXCLUDEOS = 1060;

        public static readonly RPMHeaderTag EXCLUDEOS = new RPMHeaderTag(_EXCLUDEOS,
            "EXCLUDEOS");

        public const int _EXCLUSIVEARCH = 1061;

        public static readonly RPMHeaderTag EXCLUSIVEARCH = new RPMHeaderTag(
            _EXCLUSIVEARCH, "EXCLUSIVEARCH");

        public const int _EXCLUSIVEOS = 1062;

        public static readonly RPMHeaderTag EXCLUSIVEOS = new RPMHeaderTag(
            _EXCLUSIVEOS, "EXCLUSIVEOS");

        public const int _AUTOREQPROV = 1063;

        public static readonly RPMHeaderTag AUTOREQPROV = new RPMHeaderTag(
            _AUTOREQPROV, "AUTOREQPROV");

        public const int _RPMVERSION = 1064;

        public static readonly RPMHeaderTag RPMVERSION = new RPMHeaderTag(_RPMVERSION,
            "RPMVERSION");

        public const int _TRIGGERSCRIPTS = 1065;

        public static readonly RPMHeaderTag TRIGGERSCRIPT = new RPMHeaderTag(
            _TRIGGERSCRIPTS, "TRIGGERSCRIPTS");

        public const int _TRIGGERNAME = 1066;

        public static readonly RPMHeaderTag TRIGGERNAME = new RPMHeaderTag(
            _TRIGGERNAME, "TRIGGERNAME");

        public const int _TRIGGERVERSION = 1067;

        public static readonly RPMHeaderTag TRIGGERVERSION = new RPMHeaderTag(
            _TRIGGERVERSION, "TRIGGERVERSION");

        public const int _TRIGGERFLAGS = 1068;

        public static readonly RPMHeaderTag TRIGGERFLAGS = new RPMHeaderTag(
            _TRIGGERFLAGS, "TRIGGERFLAGS");

        public const int _TRIGGERINDEX = 1069;

        public static readonly RPMHeaderTag TRIGGERINDEX = new RPMHeaderTag(
            _TRIGGERINDEX, "TRIGGERINDEX");

        public const int _VERIFYSCRIPT = 1079;

        public static readonly RPMHeaderTag VERIFYSCRIPT = new RPMHeaderTag(
            _VERIFYSCRIPT, "VERIFYSCRIPT");

        public const int _CHANGELOGTIME = 1080;

        public static readonly RPMHeaderTag CHANGELOGTIME = new RPMHeaderTag(
            _CHANGELOGTIME, "CHANGELOGTIME");

        public const int _CHANGELOGNAME = 1081;

        public static readonly RPMHeaderTag CHANGELOGNAME = new RPMHeaderTag(
            _CHANGELOGNAME, "CHANGELOGNAME");

        public const int _CHANGELOGTEXT = 1082;

        public static readonly RPMHeaderTag CHANGELOGTEXT = new RPMHeaderTag(
            _CHANGELOGTEXT, "CHANGELOGTEXT");

        public const int _BROKENMD5 = 1083;

        public static readonly RPMHeaderTag BROKENMD5 = new RPMHeaderTag(_BROKENMD5,
            "BROKENMD5");

        public const int _PREREQ = 1084;

        public static readonly RPMHeaderTag PREREQ = new RPMHeaderTag(_PREREQ,
            "PREREQ");

        public const int _PREINPROG = 1085;

        public static readonly RPMHeaderTag PREINPROG = new RPMHeaderTag(_PREINPROG,
            "PREINPROG");

        public const int _POSTINPROG = 1086;

        public static readonly RPMHeaderTag POSTINPROG = new RPMHeaderTag(_POSTINPROG,
            "POSTINPROG");

        public const int _PREUNPROG = 1087;

        public static readonly RPMHeaderTag PREUNPROG = new RPMHeaderTag(_PREUNPROG,
            "PREUNPROG");

        public const int _POSTUNPROG = 1088;

        public static readonly RPMHeaderTag POSTUNPROG = new RPMHeaderTag(_POSTUNPROG,
            "POSTUNPROG");

        public const int _BUILDARCHS = 1089;

        public static readonly RPMHeaderTag BUILDARCHS = new RPMHeaderTag(_BUILDARCHS,
            "BUILDARCHS");

        public const int _OBSOLETENAME = 1090;

        public static readonly RPMHeaderTag OBSOLETENAME = new RPMHeaderTag(
            _OBSOLETENAME, "OBSOLETENAME");

        /* backward comaptibility */
        public static readonly RPMHeaderTag OBSOLETES = new RPMHeaderTag(
            _OBSOLETENAME, "OBSOLETES");

        public const int _VERIFYSCRIPTPROG = 1091;

        public static readonly RPMHeaderTag VERIFYSCRIPTPROG = new RPMHeaderTag(
            _VERIFYSCRIPTPROG, "VERIFYSCRIPTPROG");

        public const int _TRIGGERSCRIPTPROG = 1092;

        public static readonly RPMHeaderTag TRIGGERSCRIPTPROG = new RPMHeaderTag(
            _TRIGGERSCRIPTPROG, "TRIGGERSCRIPTPROG");

        public const int _DOCDIR = 1093;

        public static readonly RPMHeaderTag DOCDIR = new RPMHeaderTag(_DOCDIR,
            "DOCDIR");

        public const int _COOKIE = 1094;

        public static readonly RPMHeaderTag COOKIE = new RPMHeaderTag(_COOKIE,
            "COOKIE");

        public const int _FILEDEVICES = 1095;

        public static readonly RPMHeaderTag FILEDEVICES = new RPMHeaderTag(
            _FILEDEVICES, "FILEDEVICES");

        public const int _FILEINODES = 1096;

        public static readonly RPMHeaderTag FILEINODES = new RPMHeaderTag(_FILEINODES,
            "FILEINODES");

        public const int _FILELANGS = 1097;

        public static readonly RPMHeaderTag FILELANGS = new RPMHeaderTag(_FILELANGS,
            "FILELANGS");

        public const int _PREFIXES = 1098;

        public static readonly RPMHeaderTag PREFIXES = new RPMHeaderTag(_PREFIXES,
            "PREFIXES");

        public const int _INSTPREFIXES = 1099;

        public static readonly RPMHeaderTag INSTPREFIXES = new RPMHeaderTag(
            _INSTPREFIXES, "INSTPREFIXES");

        public const int _TRIGGERIN = 1100;

        public static readonly RPMHeaderTag TRIGGERIN = new RPMHeaderTag(_TRIGGERIN,
            "TRIGGERIN");

        public const int _TRIGGERUN = 1101;

        public static readonly RPMHeaderTag TRIGGERUN = new RPMHeaderTag(_TRIGGERUN,
            "TRIGGERUN");

        public const int _TRIGGERPOSTUN = 1102;

        public static readonly RPMHeaderTag TRIGGERPOSTUN = new RPMHeaderTag(
            _TRIGGERPOSTUN, "TRIGGERPOSTUN");

        public const int _AUTOREQ = 1103;

        public static readonly RPMHeaderTag AUTOREQ = new RPMHeaderTag(_AUTOREQ,
            "AUTOREQ");

        public const int _AUTOPROV = 1104;

        public static readonly RPMHeaderTag AUTOPROV = new RPMHeaderTag(_AUTOPROV,
            "AUTOPROV");

        public const int _CAPABILITY = 1105;

        public static readonly RPMHeaderTag CAPABILITY = new RPMHeaderTag(_CAPABILITY,
            "CAPABILITY");

        public const int _SOURCEPACKAGE = 1106;

        public static readonly RPMHeaderTag SOURCEnamespace = new RPMHeaderTag(
            _SOURCEPACKAGE, "SOURCEPACKAGE");

        public const int _OLDORIGFILENAMES = 1107;

        public static readonly RPMHeaderTag OLDORIGFILENAMES = new RPMHeaderTag(
            _OLDORIGFILENAMES, "OLDORIGFILENAMES");

        public const int _BUILDPREREQ = 1108;

        public static readonly RPMHeaderTag BUILDPREREQ = new RPMHeaderTag(
            _BUILDPREREQ, "BUILDPREREQ");

        public const int _BUILDREQUIRES = 1109;

        public static readonly RPMHeaderTag BUILDREQUIRES = new RPMHeaderTag(
            _BUILDREQUIRES, "BUILDREQUIRES");

        public const int _BUILDCONFLICTS = 1110;

        public static readonly RPMHeaderTag BUILDCONFLICTS = new RPMHeaderTag(
            _BUILDCONFLICTS, "BUILDCONFLICTS");

        public const int _BUILDMACROS = 1111;

        public static readonly RPMHeaderTag BUILDMACROS = new RPMHeaderTag(
            _BUILDMACROS, "BUILDMACROS");

        public const int _PROVIDEFLAGS = 1112;

        public static readonly RPMHeaderTag PROVIDEFLAGS = new RPMHeaderTag(
            _PROVIDEFLAGS, "PROVIDEFLAGS");

        public const int _PROVIDEVERSION = 1113;

        public static readonly RPMHeaderTag PROVIDEVERSION = new RPMHeaderTag(
            _PROVIDEVERSION, "PROVIDEVERSION");

        public const int _OBSOLETEFLAGS = 1114;

        public static readonly RPMHeaderTag OBSOLETEFLAGS = new RPMHeaderTag(
            _OBSOLETEFLAGS, "OBSOLETEFLAGS");

        public const int _OBSOLETEVERSION = 1115;

        public static readonly RPMHeaderTag OBSOLETEVERSION = new RPMHeaderTag(
            _OBSOLETEVERSION, "OBSOLETEVERSION");

        public const int _DIRINDEXES = 1116;

        public static readonly RPMHeaderTag DIRINDEXES = new RPMHeaderTag(_DIRINDEXES,
            "DIRINDEXES");

        public const int _BASENAMES = 1117;

        public static readonly RPMHeaderTag BASENAMES = new RPMHeaderTag(_BASENAMES,
            "BASENAMES");

        public const int _DIRNAMES = 1118;

        public static readonly RPMHeaderTag DIRNAMES = new RPMHeaderTag(_DIRNAMES,
            "DIRNAMES");

        public const int _ORIGDIRINDEXES = 1119;

        public static readonly RPMHeaderTag ORIGDIRINDEXES = new RPMHeaderTag(
            _ORIGDIRINDEXES, "ORIGDIRINDEXES");

        public const int _ORIGBASENAMES = 1120;

        public static readonly RPMHeaderTag ORIGBASENAMES = new RPMHeaderTag(
            _ORIGBASENAMES, "ORIGBASENAMES");

        public const int _ORIGDIRNAMES = 1121;

        public static readonly RPMHeaderTag ORIGDIRNAMES = new RPMHeaderTag(
            _ORIGDIRNAMES, "ORIGDIRNAMES");

        public const int _OPTFLAGS = 1122;

        public static readonly RPMHeaderTag OPTFLAGS = new RPMHeaderTag(_OPTFLAGS,
            "OPTFLAGS");

        public const int _DISTURL = 1123;

        public static readonly RPMHeaderTag DISTURL = new RPMHeaderTag(_DISTURL,
            "DISTURL");

        public const int _PAYLOADFORMAT = 1124;

        public static readonly RPMHeaderTag PAYLOADFORMAT = new RPMHeaderTag(
            _PAYLOADFORMAT, "PAYLOADFORMAT");

        public const int _PAYLOADCOMPRESSOR = 1125;

        public static readonly RPMHeaderTag PAYLOADCOMPRESSOR = new RPMHeaderTag(
            _PAYLOADCOMPRESSOR, "PAYLOADCOMPRESSOR");

        public const int _PAYLOADFLAGS = 1126;

        public static readonly RPMHeaderTag PAYLOADFLAGS = new RPMHeaderTag(
            _PAYLOADFLAGS, "PAYLOADFLAGS");

        public const int _INSTALLCOLOR = 1127;

        public static readonly RPMHeaderTag INSTALLCOLOR = new RPMHeaderTag(
            _INSTALLCOLOR, "INSTALLCOLOR");

        public const int _INSTALLTID = 1128;

        public static readonly RPMHeaderTag INSTALLTID = new RPMHeaderTag(_INSTALLTID,
            "INSTALLTID");

        public const int _REMOVETID = 1129;

        public static readonly RPMHeaderTag REMOVETID = new RPMHeaderTag(_REMOVETID,
            "REMOVETID");

        public const int _SHA1RHN = 1130;

        public static readonly RPMHeaderTag SHA1RHN = new RPMHeaderTag(_SHA1RHN,
            "SHA1RHN");

        public const int _RHNPLATFORM = 1131;

        public static readonly RPMHeaderTag RHNPLATFORM = new RPMHeaderTag(
            _RHNPLATFORM, "RHNPLATFORM");

        public const int _PLATFORM = 1132;

        public static readonly RPMHeaderTag PLATFORM = new RPMHeaderTag(_PLATFORM,
            "PLATFORM");

        public const int _PATCHESNAME = 1133;

        public static readonly RPMHeaderTag PATCHESNAME = new RPMHeaderTag(
            _PATCHESNAME, "PATCHESNAME");

        public const int _PATCHESFLAGS = 1134;

        public static readonly RPMHeaderTag PATCHESFLAGS = new RPMHeaderTag(
            _PATCHESFLAGS, "PATCHESFLAGS");

        public const int _PATCHESVERSION = 1135;

        public static readonly RPMHeaderTag PATCHESVERSION = new RPMHeaderTag(
            _PATCHESVERSION, "PATCHESVERSION");

        public const int _CACHECTIME = 1136;

        public static readonly RPMHeaderTag CACHECTIME = new RPMHeaderTag(_CACHECTIME,
            "CACHECTIME");

        public const int _CACHEPKGPATH = 1137;

        public static readonly RPMHeaderTag CACHEPKGPATH = new RPMHeaderTag(
            _CACHEPKGPATH, "CACHEPKGPATH");

        public const int _CACHEPKGSIZE = 1138;

        public static readonly RPMHeaderTag CACHEPKGSIZE = new RPMHeaderTag(
            _CACHEPKGSIZE, "CACHEPKGSIZE");

        public const int _CACHEPKGMTIME = 1139;

        public static readonly RPMHeaderTag CACHEPKGMTIME = new RPMHeaderTag(
            _CACHEPKGMTIME, "CACHEPKGMTIME");

        public const int _FILECOLORS = 1140;

        public static readonly RPMHeaderTag FILECOLORS = new RPMHeaderTag(_FILECOLORS,
            "FILECOLORS");

        public const int _FILECLASS = 1141;

        public static readonly RPMHeaderTag FILECLASS = new RPMHeaderTag(_FILECLASS,
            "FILECLASS");

        public const int _CLASSDICT = 1142;

        public static readonly RPMHeaderTag CLASSDICT = new RPMHeaderTag(_CLASSDICT,
            "CLASSDICT");

        public const int _FILEDEPENDSX = 1143;

        public static readonly RPMHeaderTag FILEDEPENDSX = new RPMHeaderTag(
            _FILEDEPENDSX, "FILEDEPENDSX");

        public const int _FILEDEPENDSN = 1144;

        public static readonly RPMHeaderTag FILEDEPENDSN = new RPMHeaderTag(
            _FILEDEPENDSN, "FILEDEPENDSN");

        public const int _DEPENDSDICT = 1145;

        public static readonly RPMHeaderTag DEPENDSDICT = new RPMHeaderTag(
            _DEPENDSDICT, "DEPENDSDICT");

        public const int _SOURCEPKGID = 1146;

        public static readonly RPMHeaderTag SOURCEPKGID = new RPMHeaderTag(
            _SOURCEPKGID, "SOURCEPKGID");

        public const int _FILECONTEXTS = 1147;

        public static readonly RPMHeaderTag FILECONTEXTS = new RPMHeaderTag(
            _FILECONTEXTS, "FILECONTEXTS");

        public const int _FSCONTEXTS = 1148;

        public static readonly RPMHeaderTag FSCONTEXTS = new RPMHeaderTag(_FSCONTEXTS,
            "FSCONTEXTS");

        public const int _RECONTEXTS = 1149;

        public static readonly RPMHeaderTag RECONTEXTS = new RPMHeaderTag(_RECONTEXTS,
            "RECONTEXTS");

        public const int _POLICIES = 1150;

        public static readonly RPMHeaderTag POLICIES = new RPMHeaderTag(_POLICIES,
            "POLICIES");

        public const int _PRETRANS = 1151;

        public static readonly RPMHeaderTag PRETRANS = new RPMHeaderTag(_PRETRANS,
            "PRETRANS");

        public const int _POSTTRANS = 1152;

        public static readonly RPMHeaderTag POSTTRANS = new RPMHeaderTag(_POSTTRANS,
            "POSTTRANS");

        public const int _PRETRANSPROG = 1153;

        public static readonly RPMHeaderTag PRETRANSPROG = new RPMHeaderTag(
            _PRETRANSPROG, "PRETRANSPROG");

        public const int _POSTTRANSPROG = 1154;

        public static readonly RPMHeaderTag POSTTRANSPROG = new RPMHeaderTag(
            _POSTTRANSPROG, "POSTTRANSPROG");

        public const int _DISTTAG = 1155;

        public static readonly RPMHeaderTag DISTTAG = new RPMHeaderTag(_DISTTAG,
            "DISTTAG");

        public const int _SUGGESTSNAME = 1156;

        public static readonly RPMHeaderTag SUGGESTSNAME = new RPMHeaderTag(
            _SUGGESTSNAME, "SUGGESTSNAME");

        public static readonly RPMHeaderTag SUGGESTS = new RPMHeaderTag(_SUGGESTSNAME,
            "SUGGESTS");

        public const int _SUGGESTSVERSION = 1157;

        public static readonly RPMHeaderTag SUGGESTSVERSION = new RPMHeaderTag(
            _SUGGESTSVERSION, "SUGGESTSVERSION");

        public const int _SUGGESTSFLAGS = 1158;

        public static readonly RPMHeaderTag SUGGESTSFLAGS = new RPMHeaderTag(
            _SUGGESTSFLAGS, "SUGGESTSFLAGS");

        public const int _ENHANCESNAME = 1159;

        public static readonly RPMHeaderTag ENHANCESNAME = new RPMHeaderTag(
            _ENHANCESNAME, "ENHANCESNAME");

        public static readonly RPMHeaderTag ENHANCES = new RPMHeaderTag(_ENHANCESNAME,
            "ENHANCES");

        public const int _ENHANCESVERSION = 1160;

        public static readonly RPMHeaderTag ENHANCESVERSION = new RPMHeaderTag(
            _ENHANCESVERSION, "ENHANCESVERSION");

        public const int _ENHANCESFLAGS = 1161;

        public static readonly RPMHeaderTag ENHANCESFLAGS = new RPMHeaderTag(
            _ENHANCESFLAGS, "ENHANCESFLAGS");

        public const int _PRIORITY = 1162;

        public static readonly RPMHeaderTag PRIORITY = new RPMHeaderTag(_PRIORITY,
            "PRIORITY");

        public const int _CVSID = 1163;

        public static readonly RPMHeaderTag CVSID = new RPMHeaderTag(_CVSID, "CVSID");

        public static readonly RPMHeaderTag SVNID = new RPMHeaderTag(_CVSID, "SVNID");

        public const int _BLINKPKGID = 1164;

        public static readonly RPMHeaderTag BLINKPKGID = new RPMHeaderTag(_BLINKPKGID,
            "BLINKPKGID");

        public const int _BLINKHDRID = 1165;

        public static readonly RPMHeaderTag BLINKHDRID = new RPMHeaderTag(_BLINKHDRID,
            "BLINKHDRID");

        public const int _BLINKNEVRA = 1166;

        public static readonly RPMHeaderTag BLINKNEVRA = new RPMHeaderTag(_BLINKNEVRA,
            "BLINKNEVRA");

        public const int _FLINKPKGID = 1167;

        public static readonly RPMHeaderTag FLINKPKGID = new RPMHeaderTag(_FLINKPKGID,
            "FLINKPKGID");

        public const int _FLINKHDRID = 1168;

        public static readonly RPMHeaderTag FLINKHDRID = new RPMHeaderTag(_FLINKHDRID,
            "FLINKHDRID");

        public const int _FLINKNEVRA = 1169;

        public static readonly RPMHeaderTag FLINKNEVRA = new RPMHeaderTag(_FLINKNEVRA,
            "FLINKNEVRA");

        public const int _PACKAGEORIGIN = 1170;

        public static readonly RPMHeaderTag PACKAGEORIGIN = new RPMHeaderTag(
            _PACKAGEORIGIN, "PACKAGEORIGIN");

        public const int _TRIGGERPREIN = 1171;

        public static readonly RPMHeaderTag TRIGGERPREIN = new RPMHeaderTag(
            _TRIGGERPREIN, "TRIGGERPREIN");

        public const int _BUILDSUGGESTS = 1172;

        public static readonly RPMHeaderTag BUILDSUGGESTS = new RPMHeaderTag(
            _BUILDSUGGESTS, "BUILDSUGGESTS");

        public const int _BUILDENHANCES = 1173;

        public static readonly RPMHeaderTag BUILDENHANCES = new RPMHeaderTag(
            _BUILDENHANCES, "BUILDENHANCES");

        public const int _SCRIPTSTATES = 1174;

        public static readonly RPMHeaderTag SCRIPTSTATES = new RPMHeaderTag(
            _SCRIPTSTATES, "SCRIPTSTATES");

        public const int _SCRIPTMETRICS = 1175;

        public static readonly RPMHeaderTag SCRIPTMETRICS = new RPMHeaderTag(
            _SCRIPTMETRICS, "SCRIPTMETRICS");

        public const int _BUILDCPUCLOCK = 1176;

        public static readonly RPMHeaderTag BUILDCPUCLOCK = new RPMHeaderTag(
            _BUILDCPUCLOCK, "BUILDCPUCLOCK");

        public const int _FILEDIGESTALGOS = 1177;

        public static readonly RPMHeaderTag FILEDIGESTALGOS = new RPMHeaderTag(
            _FILEDIGESTALGOS, "FILEDIGESTALGOS");

        public const int _VARIANTS = 1178;

        public static readonly RPMHeaderTag VARIANTS = new RPMHeaderTag(_VARIANTS,
            "VARIANTS");

        public const int _XMAJOR = 1179;

        public static readonly RPMHeaderTag XMAJOR = new RPMHeaderTag(_XMAJOR,
            "XMAJOR");

        public const int _XMINOR = 1180;

        public static readonly RPMHeaderTag XMINOR = new RPMHeaderTag(_XMINOR,
            "XMINOR");

        public const int _REPOTAG = 1181;

        public static readonly RPMHeaderTag REPOTAG = new RPMHeaderTag(_REPOTAG,
            "REPOTAG");

        public const int _KEYWORDS = 1182;

        public static readonly RPMHeaderTag KEYWORDS = new RPMHeaderTag(_KEYWORDS,
            "KEYWORDS");

        public const int _BUILDPLATFORMS = 1183;

        public static readonly RPMHeaderTag BUILDPLATFORMS = new RPMHeaderTag(
            _BUILDPLATFORMS, "BUILDPLATFORMS");

        public const int _PACKAGECOLOR = 1184;

        public static readonly RPMHeaderTag PACKAGECOLOR = new RPMHeaderTag(
            _PACKAGECOLOR, "PACKAGECOLOR");

        public const int _PACKAGEPREFCOLOR = 1185;

        public static readonly RPMHeaderTag PACKAGEPREFCOLOR = new RPMHeaderTag(
            _PACKAGEPREFCOLOR, "PACKAGEPREFCOLOR");

        public const int _XATTRSDICT = 1186;

        public static readonly RPMHeaderTag XATTRSDICT = new RPMHeaderTag(_XATTRSDICT,
            "XATTRSDICT");

        public const int _FILEXATTRSX = 1187;

        public static readonly RPMHeaderTag FILEXATTRSX = new RPMHeaderTag(
            _FILEXATTRSX, "FILEXATTRSX");

        public const int _DEPATTRSDICT = 1188;

        public static readonly RPMHeaderTag DEPATTRSDICT = new RPMHeaderTag(
            _DEPATTRSDICT, "DEPATTRSDICT");

        public const int _CONFLICTATTRSX = 1189;

        public static readonly RPMHeaderTag CONFLICTATTRSX = new RPMHeaderTag(
            _CONFLICTATTRSX, "CONFLICTATTRSX");

        public const int _OBSOLETEATTRSX = 1190;

        public static readonly RPMHeaderTag OBSOLETEATTRSX = new RPMHeaderTag(
            _OBSOLETEATTRSX, "OBSOLETEATTRSX");

        public const int _PROVIDEATTRSX = 1191;

        public static readonly RPMHeaderTag PROVIDEATTRSX = new RPMHeaderTag(
            _PROVIDEATTRSX, "PROVIDEATTRSX");

        public const int _REQUIREATTRSX = 1192;

        public static readonly RPMHeaderTag REQUIREATTRSX = new RPMHeaderTag(
            _REQUIREATTRSX, "REQUIREATTRSX");

        public const int _BUILDPROVIDES = 1193;

        public static readonly RPMHeaderTag BUILDPROVIDES = new RPMHeaderTag(
            _BUILDPROVIDES, "BUILDPROVIDES");

        public const int _BUILDOBSOLETES = 1194;

        public static readonly RPMHeaderTag BUILDOBSOLETES = new RPMHeaderTag(
            _BUILDOBSOLETES, "BUILDOBSOLETES");

        public const int _DBINSTANCE = 1195;

        public static readonly RPMHeaderTag DBINSTANCE = new RPMHeaderTag(_DBINSTANCE,
            "DBINSTANCE");

        public const int _NVRA = 1196;

        public static readonly RPMHeaderTag NVRA = new RPMHeaderTag(_NVRA, "NVRA");

        public const int _FILEPATHS = 1197;

        public static readonly RPMHeaderTag FILEPATHS = new RPMHeaderTag(_FILEPATHS,
            "FILEPATHS");

        public const int _ORIGPATHS = 1198;

        public static readonly RPMHeaderTag ORIGPATHS = new RPMHeaderTag(_ORIGPATHS,
            "ORIGPATHS");

        public const int _RPMLIBVERSION = 1199;

        public static readonly RPMHeaderTag RPMLIBVERSION = new RPMHeaderTag(
            _RPMLIBVERSION, "RPMLIBVERSION");

        public const int _RPMLIBTIMESTAMP = 1200;

        public static readonly RPMHeaderTag RPMLIBTIMESTAMP = new RPMHeaderTag(
            _RPMLIBTIMESTAMP, "RPMLIBTIMESTAMP");

        public const int _RPMLIBVENDOR = 1201;

        public static readonly RPMHeaderTag RPMLIBVENDOR = new RPMHeaderTag(
            _RPMLIBVENDOR, "RPMLIBVENDOR");

        public const int _CLASS = 1202;

        public static readonly RPMHeaderTag CLASS = new RPMHeaderTag(_CLASS, "CLASS");

        public const int _TRACK = 1203;

        public static readonly RPMHeaderTag TRACK = new RPMHeaderTag(_TRACK, "TRACK");

        public const int _TRACKPROG = 1204;

        public static readonly RPMHeaderTag TRACKPROG = new RPMHeaderTag(_TRACKPROG,
            "TRACKPROG");

        public const int _SANITYCHECK = 1205;

        public static readonly RPMHeaderTag SANITYCHECK = new RPMHeaderTag(
            _SANITYCHECK, "SANITYCHECK");

        public const int _SANITYCHECKPROG = 1206;

        public static readonly RPMHeaderTag SANITYCHECKPROG = new RPMHeaderTag(
            _SANITYCHECKPROG, "SANITYCHECKPROG");

        public const int _FILESTAT = 1207;

        public static readonly RPMHeaderTag FILESTAT = new RPMHeaderTag(_FILESTAT,
            "FILESTAT");

        public const int _STAT = 1208;

        public static readonly RPMHeaderTag STAT = new RPMHeaderTag(_STAT, "STAT");

        public const int _ORIGINTID = 1209;

        public static readonly RPMHeaderTag ORIGINTID = new RPMHeaderTag(_ORIGINTID,
            "ORIGINTID");

        public const int _ORIGINTIME = 1210;

        public static readonly RPMHeaderTag ORIGINTIME = new RPMHeaderTag(_ORIGINTIME,
            "ORIGINTIME");

        public const int _HEADERSTARTOFF = 1211;

        public static readonly RPMHeaderTag HEADERSTARTOFF = new RPMHeaderTag(
            _HEADERSTARTOFF, "HEADERSTARTOFF");

        public const int _HEADERENDOFF = 1212;

        public static readonly RPMHeaderTag HEADERENDOFF = new RPMHeaderTag(
            _HEADERENDOFF, "HEADERENDOFF");

        public const int _PACKAGETIME = 1213;

        public static readonly RPMHeaderTag PACKAGETIME = new RPMHeaderTag(
            _PACKAGETIME, "PACKAGETIME");

        public const int _PACKAGESIZE = 1214;

        public static readonly RPMHeaderTag PACKAGESIZE = new RPMHeaderTag(
            _PACKAGESIZE, "PACKAGESIZE");

        public const int _PACKAGEDIGEST = 1215;

        public static readonly RPMHeaderTag PACKAGEDIGEST = new RPMHeaderTag(
            _PACKAGEDIGEST, "PACKAGEDIGEST");

        public const int _PACKAGESTAT = 1216;

        public static readonly RPMHeaderTag PACKAGESTAT = new RPMHeaderTag(
            _PACKAGESTAT, "PACKAGESTAT");

        public const int _PACKAGEBASEURL = 1217;

        public static readonly RPMHeaderTag PACKAGEBASEURL = new RPMHeaderTag(
            _PACKAGEBASEURL, "PACKAGEBASEURL");

        // special strings (TODO find out id if available)
        public static readonly RPMHeaderTag MULTILIBS = new RPMHeaderTag(10000,
            "MULTILIBS");

        public static readonly RPMHeaderTag FSSIZES = new RPMHeaderTag(10001,
            "FSSIZES");

        public static readonly RPMHeaderTag FSNAMES = new RPMHeaderTag(10002,
            "FSNAMES");

        public static readonly RPMHeaderTag FILENAMES = new RPMHeaderTag(10003,
            "FILENAMES");

        public static readonly RPMHeaderTag TRIGGERCONDS = new RPMHeaderTag(10004,
            "TRIGGERCONDS");

        public static readonly RPMHeaderTag TRIGGERTYPE = new RPMHeaderTag(10005,
            "TRIGGERTYPE");

        // jrpm flags
        public static readonly RPMHeaderTag J_FILESIZE = new RPMHeaderTag(90000,
            "J_FILESIZE");

        public static readonly RPMHeaderTag J_ARCHIVESIZE = new RPMHeaderTag(90001,
            "J_ARCHIVESIZE");

        private EnumIf delegateJ;

        private RPMHeaderTag(int tag, String name)
        {
            delegateJ = new EnumDelegate(typeof(RPMHeaderTag), tag, name, this);
        }

        /**
             * Get a enum by id
             * 
             * @param id
             *                The id of the enum
             * @return The enum object
             */
        public static EnumIf getEnumById(long id)
        {
            return EnumDelegate.getEnumById(typeof(RPMHeaderTag), id);
        }

        /**
             * Get a enum by name
             * 
             * @param name
             *                The name of the enum
             * @return The enum object
             */
        public static EnumIf getEnumByName(String name)
        {
            EnumIf result = EnumDelegate.getEnumByName(typeof(RPMHeaderTag), name);

            return result;
        }

        /**
             * Get all defined enums of this class
             * 
             * @return An array of all defined enum objects
             */
        public static String[] getEnumNames()
        {
            return EnumDelegate.getEnumNames(typeof(RPMHeaderTag));
        }

        /**
             * Get a enum of this class by id
             * 
             * @param tag
             *                The id
             * @return The enum object
             */
        public static RPMHeaderTag getRPMHeaderTag(int tag)
        {
            return (RPMHeaderTag)getEnumById(tag);
        }

        /**
             * Check if this enum class contains a enum of a specified id
             * 
             * @param id
             *                The id of the enum
             * @return TRUE if the enum is defined in this class
             */
        public static bool containsEnumId(java.lang.Long id)
        {
            return EnumDelegate.containsEnumId(typeof(RPMHeaderTag), id);
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
            return delegateJ.getName()+"\t"+getId();
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