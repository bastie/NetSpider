/*
 * Utils.java
 * tinySQL, some helper methods
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:27:20 $
 * $Revision: 1.1 $
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{


    /**
    Some helper methods for tinySQL
    @author Brian Jepson <bjepson@home.com>
    @author Marcel Ruff <ruff@swand.lake.de> Added write access to dBase and JDK 2 support
    */
    public class Utils
    {
        // JLex (the lexical analyzer) doesn´t support unicode
        // so we must play around with the different code pages:
        // "Cp437"  = 7 bit MS-DOS, US-ASCII
        // "Cp850"  = 8 bit MS-DOS, Multilingual Latin 1,   &auml; = 0x83 = 131
        // "Cp1252" = 8 bit Windows Multilingual,           &auml; = 0xe4 = 228
        // In future the code page should be passed at connection time using the URL
        internal const String encode = "Cp1252"; // dBase encoding

        /**
         * Converts a long to a little-endian four-byte array
         *
         */
        public static byte[] intToLittleEndian(int val)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[i] = (byte)(val % 256);
                val = val / 256;
            }
            return b;
        }


        /**
         * Converts a long to a little-endian two-byte array
         *
         */
        public static byte[] shortToLittleEndian(short val)
        {
            byte[] b = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                b[i] = (byte)(val % 256);
                val = (short)(val / 256);
            }
            return b;
        }


        /**
         *
         * Converts a little-endian four-byte array to a long,
         * represented as a double, since long is signed.
         *
         * I don't know why Java doesn't supply this. It could
         * be that it's there somewhere, but I looked and couldn't
         * find it.
         *
         */
        public static double vax_to_long(byte[] b)
        {

            //existing code that has been commented out
            //return fixByte(b[0]) + ( fixByte(b[1]) * 256) + 
            //( fixByte(b[2]) * (256^2)) + ( fixByte(b[3]) * (256^3));

            // Fix courtesy Preetha Suri <Preetha.Suri@sisl.co.in>
            //
            long lngTmp = (long)(0x0ffL & b[0])
                          | ((0x0ffL & (long)b[1]) << 8)
                          | ((0x0ffL & (long)b[2]) << 16)
                          | ((0x0ffL & (long)b[3]) << 24);


            return ((double)lngTmp);

        }


        /**
         *
         * Converts a little-endian four-byte array to a short,
         * represented as an int, since short is signed.
         *
         * I don't know why Java doesn't supply this. It could
         * be that it's there somewhere, but I looked and couldn't
         * find it.
         *
         */
        public static int vax_to_short(byte[] b)
        {
            return (int)(fixByte(b[0]) + (fixByte(b[1]) * 256));
        }


        /*
         *
         * bytes are signed; let's fix them...
         *
         */
        public static short fixByte(byte b)
        {

            if (b < 0)
            {
                return (short)(b + 256);
            }
            return b;
        }

        /**
        Cut or padd the string to the given size
        @param a string
        @param size the wanted length
        @param padChar char to use for padding (must be of length()==1!)
        @return the string with correct lenght, padded with pad if necessary
        */
        public static String forceToSize(String str, int size, String padChar)
        {
            if (str != null && str.length() == size)
                return str;

            java.lang.StringBuffer tmp;
            if (str == null)
                tmp = new java.lang.StringBuffer(size);
            else
                tmp = new java.lang.StringBuffer(str);

            if (tmp.length() > size)
            {
                return tmp.toString().substring(0, size);  // do cutting
            }
            else
            {
                // or add some padding to the end of the string
                java.lang.StringBuffer pad = new java.lang.StringBuffer(size);
                int numBlanks = size - tmp.length();
                for (int p = 0; p < numBlanks; p++)
                {
                    pad.append(padChar);
                }
                return tmp.append(pad).toString();
            }
        }


        /**
        Cut or padd the string to the given size
        @param a string
        @param size the wanted length
        @param padByte char to use for padding
        @return the string with correct lenght, padded with pad if necessary
        */
        public static byte[] forceToSize(String str, int size, byte padByte) //throws java.io.UnsupportedEncodingException
        {
            if (str != null && str.length() == size)
                return str.getBytes(encode);

            byte[] result = new byte[size];

            if (str == null)
            {
                for (int ii = 0; ii < size; ii++) result[ii] = padByte;
                return result;
            }

            if (str.length() > size)
                return str.substring(0, size).getBytes(encode);  // do cutting

            // do padding
            byte[] tmp = str.getBytes(encode);
            for (int jj = 0; jj < tmp.Length; jj++)
                result[jj] = tmp[jj];
            for (int kk = tmp.Length; kk < size; kk++)
                result[kk] = padByte;
            return result;
        }


        /*
         * Delete a file in the data directory
         */
        public static void delFile(String fname) //throws NullPointerException, IOException
        {
            java.io.File f = new java.io.File(fname);

            // only delete a file that exists
            //
            if (f.exists())
            {
                // try the delete. If it fails, complain
                //
                if (!f.delete())
                {
                    throw new java.io.IOException("Could not delete: " + f.getAbsolutePath() + ".");
                }
            }
        }
        public static void delFile(String dataDir, String fname)
        {//throws NullPointerException, IOException {

            java.io.File f = new java.io.File(dataDir + java.io.File.separator + fname);

            // only delete a file that exists
            //
            if (f.exists())
            {
                // try the delete. If it fails, complain
                //
                if (!f.delete())
                {
                    throw new java.io.IOException("Could not delete file: " +
                                           dataDir + "/" + fname + ".");
                }
            }
        }


        /**
        rename a file
        @return true if succeeded
        */
        public static bool renameFile(String oldName, String newName)
        {
            java.io.File f_old = new java.io.File(oldName);
            java.io.File f_new = new java.io.File(newName);
            bool ret = f_old.renameTo(f_new);
            return ret;
        }


        /**
        Strip the path and suffix of a file name
        @param file   "/usr/local/dbase/test.DBF"
        @return "test"
        */
        public static String stripPathAndExtension(String file)
        {
            String sep = java.io.File.separator;
            int begin = file.lastIndexOf(sep);
            if (begin < 0) begin = 0;
            else begin++;
            int end = file.lastIndexOf(".");
            if (end < 0) end = file.length();
            String str = file.substring(begin, end);
            return str;
        }


        /*
         * Scan the given directory for files containing the substrMatch<br> 
         * Small case extensions '.dbf' are recognized and returned as '.DBF'
         */
        public static java.util.Vector<Object> getAllFiles(String path, String suffix)
        {
            java.util.Vector<Object> vec = null;
            String[] fileNameList;
            java.io.File currentDir, f;
            String fileName, upperSuffix;
            int i;
            upperSuffix = suffix.toUpperCase();
            currentDir = new java.io.File(path);
            fileNameList = currentDir.list();
            if (fileNameList == null)
            {
                java.lang.SystemJ.outJ.println("*** null for " + path);
            }
            else
            {
                vec = new java.util.Vector<Object>(fileNameList.Length);
                for (i = 0; i < fileNameList.Length; i++)
                {
                    f = new java.io.File(fileNameList[i]);
                    if (!f.isDirectory())
                    {
                        fileName = f.getPath().toString().toUpperCase();
                        //           lastModified = new java.util.Date(f.lastModified());
                        if (upperSuffix == null | fileName.endsWith(upperSuffix))
                        {
                            vec.addElement(f);
                        }
                    }
                }
            }
            return vec;
        }
        public static bool isDateColumn(int columnType)
        {
            if (columnType == java.sql.Types.DATE | columnType == java.sql.Types.TIMESTAMP)
                return true;
            else return false;
        }
        public static bool isCharColumn(int columnType)
        {
            if (columnType == java.sql.Types.CHAR | columnType == java.sql.Types.VARCHAR |
                columnType == java.sql.Types.LONGVARCHAR) return true;
            else return false;
        }
        public static bool isNumberColumn(int columnType)
        {
            if (columnType == java.sql.Types.NUMERIC | columnType == java.sql.Types.INTEGER |
                columnType == java.sql.Types.TINYINT | columnType == java.sql.Types.SMALLINT |
                columnType == java.sql.Types.BIGINT | columnType == java.sql.Types.FLOAT |
                columnType == java.sql.Types.DOUBLE | columnType == java.sql.Types.REAL) return true;
            else return false;
        }
        public static bool isFunctionName(String inputName)
        {
            int i;
            String[] functionNames = {"COUNT","SUM","MIN","MAX","UPPER","TRIM",
        "SUBSTR","CONCAT","TO_DATE"};
            for (i = 0; i < functionNames.Length; i++)
                if (inputName.equalsIgnoreCase(functionNames[i])) return true;
            return false;
        }
        public static bool endsWithFunctionName(String inputName)
        {
            int i;
            String upperName;
            String[] functionNames = {"COUNT","SUM","MIN","MAX","UPPER","TRIM",
        "SUBSTR","CONCAT","TO_DATE"};
            upperName = inputName.toUpperCase();
            for (i = 0; i < functionNames.Length; i++)
                if (upperName.endsWith(functionNames[i])) return true;
            return false;
        }
        /*
         * This function indicates which functions should be set to null if any
         * of its arguments are null.
         */
        public static bool clearFunction(String inputName)
        {
            int i;
            String[] functionNames = { "UPPER", "TRIM", "SUBSTR" };
            for (i = 0; i < functionNames.Length; i++)
                if (inputName.equalsIgnoreCase(functionNames[i])) return true;
            return false;
        }
        /*
         * Move the input table to the top of the selection list.
         */
        public static void setPriority(java.util.Vector<Object> inputList, String inputTable)
        {
            String tableName;
            int i;
            if (inputList == null) return;
            for (i = 0; i < inputList.size(); i++)
            {
                tableName = (String)inputList.elementAt(i);
                if (tableName.equals(inputTable))
                {
                    if (i > 0)
                    {
                        inputList.removeElementAt(i);
                        inputList.insertElementAt(tableName, 0);
                    }
                    break;
                }
            }
        }

        /**
        For debugging/tracing
        Switch the debug mode on/off:
        */
        internal static readonly bool debug = false;
        internal static void log(String id, String str)
        {
            if (debug) log(id + ": " + str);
        }
        internal static void log(String str)
        {
            if (debug) java.lang.SystemJ.outJ.println(str);
        }

    }
}