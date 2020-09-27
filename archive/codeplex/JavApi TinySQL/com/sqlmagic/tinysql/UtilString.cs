/*
 * This class provides string manipulation methods.
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:23:31 $
 * $Revision: 1.1 $
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
 * Revision History:
 *
 * Written by Davis Swan in February, 2004.
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    public class UtilString
    {
        /*
         * Is this a quoted string?
         */
        public static bool isQuotedString(String inputString)
        {
            String trimString;
            int trimLength;
            if (inputString == (String)null) return false;
            trimString = inputString.trim();
            trimLength = trimString.length();
            if (trimString.length() == 0) return false;
            if ((trimString.charAt(0) == '\'' &
                   trimString.charAt(trimLength - 1) == '\'') |
                 (trimString.charAt(0) == '"' &
                   trimString.charAt(trimLength - 1) == '"'))
            {
                return true;
            }
            return false;
        }
        /*
         * Remove enclosing quotes from a string.
         */
        public static String removeQuotes(String inputString)
        {
            String trimString;
            int trimLength;
            if (inputString == (String)null) return inputString;
            trimString = inputString.trim();
            trimLength = trimString.length();
            if (trimString.length() == 0) return inputString;
            if ((trimString.charAt(0) == '\'' &
                   trimString.charAt(trimLength - 1) == '\'') |
                 (trimString.charAt(0) == '"' &
                   trimString.charAt(trimLength - 1) == '"'))
            {
                return trimString.substring(1, trimString.length() - 1);
            }
            return inputString;
        }
        /*
         * Convert a string to a double or return a default value.
         */
        public static double doubleValue(String inputString)
        {
            return doubleValue(inputString, java.lang.Double.MIN_VALUE);
        }
        public static double doubleValue(String inputString, double defaultValue)
        {
            try
            {
                return java.lang.Double.parseDouble(inputString);
            }
            catch (Exception )
            {
                return defaultValue;
            }
        }
        /*
         * Convert a date string in the format YYYYMMDD to the standard
         * date output YYYY-MM-DD
         */
        public static String toStandardDate(String inputDateString)
        //throws tinySQLException
        {
            String dateString, stdDateString;
            if (inputDateString == (String)null)
                throw new TinySQLException("Cannot format NULL date");
            if (inputDateString.length() < 8)
                throw new TinySQLException("Date " + inputDateString
                + " not in YYYYMMDD format");
            /*
             *    Convert the input to YYYYMMDD - this is required because 
             *    versions of tinySQL before 2.26d incorrectly stored dates in several
             *    different character string representations.
             */
            dateString = dateValue(inputDateString);
            stdDateString = dateString.substring(0, 4) + "-"
            + dateString.substring(4, 6) + "-"
            + dateString.substring(6, 8);
            return stdDateString;
        }
        /*
         * Convert a date string in the format DD-MON-YY, DD-MON-YYYY, or YYYYMMDD
         * to the output YYYYMMDD after checking the validity of all subfields. 
         * A tinySQLException is thrown if there are problems.
         */
        public static String dateValue(String inputString) //throws tinySQLException
        {
            String months = "-JAN-FEB-MAR-APR-MAY-JUN-JUL-AUG-SEP-OCT-NOV-DEC-";
            int[] daysInMonth = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            String dateString, dayField, monthName, monthField, yearField;
            String[] ftFields;
            FieldTokenizer ft;
            int year, month, day, monthAt;
            dateString = inputString.toUpperCase().trim();
            if (dateString.length() < 8)
                throw new TinySQLException(dateString + " is less than 8 characters.");
            /*
             *    Check for YYYY-MM-DD format - convert to YYYYMMDD if found.
             */
            if (dateString.length() == 10 & dateString.charAt(4) == '-' &
                 dateString.charAt(7) == '-')
            {
                dateString = dateString.substring(0, 4) + dateString.substring(5, 7)
                + dateString.substring(8, 10);
            }
            /*
             *    First check for an 8 character field properly formatted.
             */
            if (dateString.length() == 8 & isInteger(dateString))
            {
                try
                {
                    year = java.lang.Integer.parseInt(dateString.substring(0, 4));
                    if (year < 0 | year > 2100)
                        throw new TinySQLException(dateString + " year not "
                        + "recognized.");
                    month = java.lang.Integer.parseInt(dateString.substring(4, 6));
                    if (month < 1 | month > 12)
                        throw new TinySQLException(dateString + " month not "
                        + "recognized.");
                    day = java.lang.Integer.parseInt(dateString.substring(6, 8));
                    if (day < 1 | day > daysInMonth[month - 1])
                        throw new TinySQLException(dateString + " day not "
                        + "recognized.");
                    return dateString;
                }
                catch (Exception dateEx)
                {
                    throw new TinySQLException(dateEx.getMessage());
                }
            }
            /*
             *    Check for dd-MON-YY formats - strip off TO_DATE if it exists.
             */
            if (dateString.startsWith("TO_DATE"))
            {
                dateString = dateString.substring(8, dateString.length() - 1);
                dateString = removeQuotes(dateString);
            }
            ft = new FieldTokenizer(dateString, '-', false);
            ftFields = ft.getFields();
            if (ftFields.Length < 3)
            {
                throw new TinySQLException(dateString + " is not a date with "
                + "format DD-MON-YY!");
            }
            else
            {
                try
                {
                    day = java.lang.Integer.parseInt(ftFields[0]);
                    monthName = ftFields[1];
                    monthAt = months.indexOf("-" + monthName + "-");
                    if (monthAt == -1)
                        throw new TinySQLException(dateString + " month not "
                        + "recognized.");
                    month = (monthAt + 4) / 4;
                    if (day < 1 | day > daysInMonth[month - 1])
                        throw new TinySQLException(dateString + " day not "
                        + "between 1 and " + daysInMonth[month - 1]);
                    year = java.lang.Integer.parseInt(ftFields[2]);
                    if (year < 0 | year > 2100)
                        throw new TinySQLException(dateString + " year not "
                        + "recognized.");
                    /*
                     *          Assume that years < 50 are in the 21st century, otherwise 
                     *          the 20th.
                     */
                    if (year < 50) year = 2000 + year;
                    else year = 1900 + year;
                    dayField = java.lang.Integer.toString(day);
                    if (dayField.length() < 2) dayField = "0" + dayField;
                    monthField = java.lang.Integer.toString(month);
                    if (monthField.length() < 2) monthField = "0" + monthField;
                    yearField = java.lang.Integer.toString(year);
                    return yearField + monthField + dayField;
                }
                catch (Exception dayEx)
                {
                    throw new TinySQLException(dateString + " exception "
                    + dayEx.getMessage());
                }
            }
        }
        /*
         * The following method replaces all occurrences of oldString with newString
         * in the inputString.  This function can be replaced with the native
         * String method replaceAll in JDK 1.4 and above but is provide to support
         * earlier versions of the JRE.
         */
        public static String replaceAll(String inputString, String oldString,
           String newString)
        {
            java.lang.StringBuffer outputString = new java.lang.StringBuffer(100);
            int startIndex = 0, nextIndex;
            while (inputString.substring(startIndex).indexOf(oldString) > -1)
            {
                nextIndex = startIndex + inputString.substring(startIndex).indexOf(oldString);
                if (nextIndex > startIndex)
                {
                    outputString.append(inputString.substring(startIndex, nextIndex));
                }
                outputString.append(newString);
                startIndex = nextIndex + oldString.length();
            }
            if (startIndex <= inputString.length() - 1)
            {
                outputString.append(inputString.substring(startIndex));
            }
            return outputString.toString();
        }
        /*
         * Check to see if the input string is an integer.
         */
        public static bool isInteger(String inputString)
        {
            int testInt;
            try
            {
                testInt = java.lang.Integer.parseInt(inputString);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        /*
         * Convert a string to an int or return a default value.
         */
        public static int intValue(String inputString, int defaultValue)
        {
            try
            {
                return java.lang.Integer.parseInt(inputString);
            }
            catch (Exception )
            {
                return defaultValue;
            }
        }
        /*
         * Convert a date in the format MM/DD/YYYY to YYYYMMDD
         */
        public static String toYMD(String inputDate)
        {
            String day, month;
            FieldTokenizer ft;
            String[] ftFields;
            ft = new FieldTokenizer(inputDate, '/', false);
            ftFields = ft.getFields();
            if (ftFields.Length == 1)
            {
                return inputDate;
            }
            else if (ftFields.Length == 3)
            {
                month = ftFields[0];
                if (month.length() == 1) month = "0" + month;
                day = ftFields[1];
                if (day.length() == 1) day = "0" + day;
                return ftFields[2] + month + day;
            }
            return inputDate;
        }
        /*
         * This method formats an action Hashtable for display.
         */
        public static String actionToString(java.util.Hashtable<Object, Object> displayAction)
        {
            java.lang.StringBuffer displayBuffer = new java.lang.StringBuffer();
            String displayType, tableName;
            TinySQLWhere displayWhere;
            TsColumn createColumn, displayColumn;
            bool groupBy = false, orderBy = false;
            int i;
            java.util.Vector<Object> displayTables, displayColumns, columnDefs, displayValues;
            java.util.Hashtable<Object, Object> tables;
            displayType = (String)displayAction.get("TYPE");
            displayBuffer.append(displayType + " ");
            displayWhere = null;
            displayColumns = null;
            if (displayType.equals("SELECT"))
            {
                tables = (java.util.Hashtable<Object, Object>)displayAction.get("TABLES");
                displayTables = (java.util.Vector<Object>)tables.get("TABLE_SELECT_ORDER");
                displayColumns = (java.util.Vector<Object>)displayAction.get("COLUMNS");
                displayWhere = (TinySQLWhere)displayAction.get("WHERE");
                for (i = 0; i < displayColumns.size(); i++)
                {
                    displayColumn = (TsColumn)displayColumns.elementAt(i);
                    if (displayColumn.getContext("GROUP"))
                    {
                        groupBy = true;
                        continue;
                    }
                    else if (displayColumn.getContext("ORDER"))
                    {
                        orderBy = true;
                        continue;
                    }
                    if (i > 0) displayBuffer.append(",");
                    displayBuffer.append((String)displayColumn.name);
                }
                displayBuffer.append(" FROM ");
                for (i = 0; i < displayTables.size(); i++)
                {
                    if (i > 0) displayBuffer.append(",");
                    displayBuffer.append((String)displayTables.elementAt(i));
                }
            }
            else if (displayType.equals("DROP_TABLE"))
            {
                tableName = (String)displayAction.get("TABLE");
                displayBuffer.append(tableName);
            }
            else if (displayType.equals("CREATE_TABLE"))
            {
                tableName = (String)displayAction.get("TABLE");
                displayBuffer.append(tableName + " (");
                columnDefs = (java.util.Vector<Object>)displayAction.get("COLUMN_DEF");
                for (i = 0; i < columnDefs.size(); i++)
                {
                    if (i > 0) displayBuffer.append(",");
                    createColumn = (TsColumn)columnDefs.elementAt(i);
                    displayBuffer.append(createColumn.name + " " + createColumn.type
                    + "( " + createColumn.size + "," + createColumn.decimalPlaces
                    + ")");
                }
                displayBuffer.append(")");
            }
            else if (displayType.equals("INSERT"))
            {
                tableName = (String)displayAction.get("TABLE");
                displayBuffer.append("INTO " + tableName + "(");
                displayColumns = (java.util.Vector<Object>)displayAction.get("COLUMNS");
                for (i = 0; i < displayColumns.size(); i++)
                {
                    if (i > 0) displayBuffer.append(",");
                    displayBuffer.append((String)displayColumns.elementAt(i));
                }
                displayBuffer.append(") VALUES (");
                displayValues = (java.util.Vector<Object>)displayAction.get("VALUES");
                for (i = 0; i < displayValues.size(); i++)
                {
                    if (i > 0) displayBuffer.append(",");
                    displayBuffer.append((String)displayValues.elementAt(i));
                }
                displayBuffer.append(")");
            }
            else if (displayType.equals("UPDATE"))
            {
                tableName = (String)displayAction.get("TABLE");
                displayBuffer.append(tableName + " SET ");
                displayColumns = (java.util.Vector<Object>)displayAction.get("COLUMNS");
                displayValues = (java.util.Vector<Object>)displayAction.get("VALUES");
                displayWhere = (TinySQLWhere)displayAction.get("WHERE");
                for (i = 0; i < displayColumns.size(); i++)
                {
                    if (i > 0) displayBuffer.append(",");
                    displayBuffer.append((String)displayColumns.elementAt(i)
                    + "=" + (String)displayValues.elementAt(i));
                }
            }
            else if (displayType.equals("DELETE"))
            {
                tableName = (String)displayAction.get("TABLE");
                displayBuffer.append(" FROM " + tableName);
                displayWhere = (TinySQLWhere)displayAction.get("WHERE");
            }
            if (displayWhere != (TinySQLWhere)null)
            {
                displayBuffer.append(displayWhere.toString());
            }
            if (groupBy)
            {
                displayBuffer.append(" GROUP BY ");
                for (i = 0; i < displayColumns.size(); i++)
                {
                    displayColumn = (TsColumn)displayColumns.elementAt(i);
                    if (!displayColumn.getContext("GROUP")) continue;
                    if (!displayBuffer.toString().endsWith(" GROUP BY "))
                        displayBuffer.append(",");
                    displayBuffer.append(displayColumn.name);
                }
            }
            if (orderBy)
            {
                displayBuffer.append(" ORDER BY ");
                for (i = 0; i < displayColumns.size(); i++)
                {
                    displayColumn = (TsColumn)displayColumns.elementAt(i);
                    if (!displayColumn.getContext("ORDER")) continue;
                    if (!displayBuffer.toString().endsWith(" ORDER BY "))
                        displayBuffer.append(",");
                    displayBuffer.append(displayColumn.name);
                }
            }
            return displayBuffer.toString();
        }
        /*
         * Find the input table alias in the list provided and return the table name.
         */
        public static String findTableForAlias(String inputAlias, java.util.Vector<Object> tableList)
        //throws tinySQLException
        {
            int aliasAt;
            String tableAndAlias;
            tableAndAlias = findTableAlias(inputAlias, tableList);
            aliasAt = tableAndAlias.indexOf("->");
            return tableAndAlias.substring(0, aliasAt);
        }
        /*
         * Find the input table alias in the list provided and return the table name
         * and alias in the form tableName=tableAlias.
         */
        public static String findTableAlias(String inputAlias, java.util.Vector<Object> tableList)
        //throws tinySQLException
        {
            int i, aliasAt;
            String tableAndAlias, tableName, tableAlias;
            for (i = 0; i < tableList.size(); i++)
            {
                tableAndAlias = (String)tableList.elementAt(i);
                aliasAt = tableAndAlias.indexOf("->");
                tableName = tableAndAlias.substring(0, aliasAt);
                tableAlias = tableAndAlias.substring(aliasAt + 2);
                if (inputAlias.equals(tableAlias))
                {
                    return tableAndAlias;
                }
            }
            throw new TinySQLException("Unable to identify table alias "
            + inputAlias);
        }
        /*
         * Determine a type for the input string. 
         */
        public static int getValueType(String inputValue)
        {
            double doubleValue;
            long intValue;
            if (inputValue.startsWith("\"") |
                 inputValue.startsWith("'"))
                return java.sql.Types.CHAR;
            try
            {
                /*
                 *       If the parse methods don't generate an exception this is a 
                 *       valid number
                 */
                if (inputValue.trim().indexOf(".") > -1)
                {
                    doubleValue = java.lang.Double.parseDouble(inputValue.trim());
                    return java.sql.Types.FLOAT;
                }
                else
                {
                    intValue = java.lang.Long.parseLong(inputValue.trim());
                    return java.sql.Types.INTEGER;
                }
            }
            catch (Exception )
            {
                return java.sql.Types.CHAR;
            }
        }
    }
}