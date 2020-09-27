/*
 * tinySQLGlobals
 * 
 * $Author: $
 * $Date:  $
 * $Revision:  $
 *
 * Static class to hold global values.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 *
 * Revision History;
 *
 * Written by Davis Swan in October, 2006.
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    public class TinySQLGlobals
    {
        internal static String dataDir = (String)null;
        internal static java.util.Vector<Object> longColumnNames;
        internal static String fileSep = java.lang.SystemJ.getProperty("file.separator");
        internal static String newLine = java.lang.SystemJ.getProperty("line.separator");
        internal static java.util.Hashtable<Object, Object> DB_INDEX = new java.util.Hashtable<Object, Object>();
        internal static String VERSION = "2.261h";
        internal static bool DEBUG = false;
        internal static bool PARSER_DEBUG = false;
        internal static bool WHERE_DEBUG = false;
        internal static bool EX_DEBUG = false;
        internal static int longNamesInFileCount;
        internal static bool debug = false;
        public static void readLongNames(String inputDataDir)
        {
            String fullPath, longNameRecord;
            String[] fields;
            FieldTokenizer ft;
            java.io.File longColumnNameFile;
            dataDir = inputDataDir;
            java.io.BufferedReader longNameReader = (java.io.BufferedReader)null;
            fullPath = dataDir + fileSep + "TINYSQL_LONG_COLUMN_NAMES.dat";
            longColumnNames = new java.util.Vector<Object>();
            longColumnNameFile = new java.io.File(fullPath);
            if (longColumnNameFile.exists())
            {
                try
                {
                    longNameReader = new java.io.BufferedReader(new java.io.FileReader(fullPath));
                    while ((longNameRecord = longNameReader.readLine()) != null)
                    {
                        ft = new FieldTokenizer(longNameRecord, '|', false);
                        fields = ft.getFields();
                        longColumnNames.addElement(fields[0]);
                        longColumnNames.addElement(fields[1]);
                    }
                    longNameReader.close();
                    longNamesInFileCount = longColumnNames.size() / 2;
                    if (debug)
                        java.lang.SystemJ.outJ.println("Long Names read: " + longNamesInFileCount);
                }
                catch (Exception readEx)
                {
                    java.lang.SystemJ.outJ.println("Reader exception " + readEx.getMessage());
                    longNamesInFileCount = 0;
                }
            }
        }
        /*
         * Method to add a long column name to the global Vector.  Note that
         * the entries are keyed by the short column name so that there is always
         * one and only one short name for any long name.
         */
        public static String addLongName(String inputColumnName)
        {
            String shortColumnName, countString;
            countString = "0000" + java.lang.Integer.toString(longColumnNames.size() / 2);
            shortColumnName = "COL" + countString.substring(countString.length() - 5);
            if (debug)
                java.lang.SystemJ.outJ.println("Add " + shortColumnName + "|" + inputColumnName);
            longColumnNames.addElement(shortColumnName);
            longColumnNames.addElement(inputColumnName);
            return shortColumnName;
        }
        /*
         * This method checks for the existence of a short column name for the
         * input name.  If one does not exist it is created.
         */
        public static String getShortName(String inputColumnName)
        {
            String shortColumnName = (String)null, longColumnName;
            int i;
            if (inputColumnName.length() < 12) return inputColumnName;
            for (i = 0; i < longColumnNames.size(); i += 2)
            {
                longColumnName = (String)longColumnNames.elementAt(i + 1);
                if (longColumnName.equalsIgnoreCase(inputColumnName))
                {
                    shortColumnName = (String)longColumnNames.elementAt(i);
                    if (debug) java.lang.SystemJ.outJ.println("Return " + shortColumnName);
                    return shortColumnName;
                }
            }
            if (shortColumnName == (String)null)
            {
                /*
                 *       A short name has not been set up for this long name yet. 
                 */
                if (debug)
                    java.lang.SystemJ.outJ.println("Generate short name for " + inputColumnName);
                return addLongName(inputColumnName);
            }
            return inputColumnName;
        }
        /*
         * Get the long column name for the input short name.  
         */
        public static String getLongName(String inputColumnName)
        {
            String longColumnName, shortColumnName;
            int i;
            for (i = 0; i < longColumnNames.size(); i += 2)
            {
                shortColumnName = (String)longColumnNames.elementAt(i);
                if (shortColumnName.equalsIgnoreCase(inputColumnName))
                {
                    longColumnName = (String)longColumnNames.elementAt(i + 1);
                    if (debug) java.lang.SystemJ.outJ.println("Return " + longColumnName);
                    return longColumnName;
                }
            }
            return inputColumnName;
        }
        public static void writeLongNames()
        {
            java.io.FileWriter longNameWriter = (java.io.FileWriter)null;
            String fullPath, longColumnName, shortColumnName;
            int i;
            if (longColumnNames.size() > longNamesInFileCount * 2)
            {
                /*
                 *       The file needs to be updated.
                 */
                fullPath = dataDir + fileSep + "TINYSQL_LONG_COLUMN_NAMES.dat";
                try
                {
                    longNameWriter = new java.io.FileWriter(fullPath);
                    if (longNameWriter != (java.io.FileWriter)null)
                    {
                        for (i = 0; i < longColumnNames.size(); i += 2)
                        {
                            shortColumnName = (String)longColumnNames.elementAt(i);
                            longColumnName = (String)longColumnNames.elementAt(i + 1);
                            longNameWriter.write(shortColumnName + "|" + longColumnName
                            + newLine);
                        }
                        longNameWriter.close();
                        longNamesInFileCount = longColumnNames.size() / 2;
                    }
                    else
                    {
                        java.lang.SystemJ.outJ.println("Unable to update long column names.");
                    }
                }
                catch (Exception writeEx)
                {
                    java.lang.SystemJ.outJ.println("Write exception " + writeEx.getMessage());
                }
            }
        }
    }
}