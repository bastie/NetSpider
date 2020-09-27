/*
 * tsColumn.java - Column Object for tinySQL.
 * 
 * Copyright 1996, Brian C. Jepson
 *                 (bjepson@ids.net)
 * $Author: davis $
 * $Date: 2004/12/18 21:25:35 $
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
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    /*
     * Object to hold column metadata and value
     * Example for a column_def entity:
     * phone  CHAR(30)  DEFAULT '-'  NOT NULL
     *
     * @author Thomas Morgner <mgs@sherito.org> type is now integer 
     * and contains one of the java.sql.Types Values
     */
    public class TsColumn : java.lang.Cloneable
    {
        public String name = null;      // the column's name
        public String alias = null;      // the column's definition
        public String longName = null;      // the column's long name ( > 11 chars)
        public java.util.Vector<Object> contextList;    // the columns context (SELECT,ORDER,etc.)
        public int type = -1;      // the column's type
        // dBase types:
        // 'C' Char (max 254 bytes)
        // 'N' '-.0123456789' (max 19 bytes)
        // 'L' 'YyNnTtFf?' (1 byte)
        // 'M' 10 digit .DBT block number
        // 'D' 8 digit YMD
        public int size = 0;         // the column's size
        public int decimalPlaces = 0;   // decimal places in numeric column
        public String defaultVal = null;// not yet supported
        public int position = 0;        // internal use
        public String tableName = ""; // the table which "owns" the column
        public TinySQLTable columnTable = null;
        public String newLine = java.lang.SystemJ.getProperty("line.separator");
        String functionName = (String)null;  // Function name
        String functionArgString = (String)null;  // Function arguments
        java.util.Vector<Object> functionArgs = (java.util.Vector<Object>)null; // Function arguments as columns
        internal bool notNull = false;
        bool valueSet = false;
        String stringValue = (String)null;
        int intValue = java.lang.Integer.MIN_VALUE;
        float floatValue = java.lang.Float.MIN_VALUE;
        java.text.SimpleDateFormat fmtyyyyMMdd = new java.text.SimpleDateFormat("yyyy-MM-dd");
        java.util.Calendar today = java.util.Calendar.getInstance();
        internal bool isConstant = false;
        bool groupedColumn = false;
        /*
         * The constructor creates a column object using recursion if this is a 
         * function.
         */
        internal TsColumn(String s) //throws tinySQLException
            :
               this(s, (java.util.Hashtable<Object, Object>)null, "SELECT")
        {
        }
        internal TsColumn(String s, java.util.Hashtable<Object, Object> tableDefs, String inputContext)
        //throws tinySQLException
        {
            FieldTokenizer ft, ftArgs;
            int j, numericType, nameLength, dotAt, argIndex;
            String upperName, checkName, nextArg;
            TinySQLTable jtbl;
            TsColumn tcol;
            java.util.Vector<Object> t;
            java.util.Enumeration<Object> col_keys;
            name = s;
            longName = name;
            nameLength = name.length();
            contextList = new java.util.Vector<Object>();
            contextList.addElement(inputContext);
            ft = new FieldTokenizer(name, '(', false);
            if (ft.countFields() == 2)
            {
                /*
                 *       This is a function rather than a simple column or constant
                 */
                functionName = ft.getField(0).toUpperCase();
                if (functionName.equals("COUNT"))
                {
                    type = java.sql.Types.INTEGER;
                    size = 10;
                    intValue = java.lang.Integer.MIN_VALUE;
                    groupedColumn = true;
                }
                else if (functionName.equals("SUM"))
                {
                    type = java.sql.Types.FLOAT;
                    size = 10;
                    groupedColumn = true;
                }
                else if (functionName.equals("TO_DATE"))
                {
                    type = java.sql.Types.DATE;
                    size = 10;
                }
                else if (functionName.equals("CONCAT") |
                          functionName.equals("UPPER") |
                          functionName.equals("SUBSTR") |
                          functionName.equals("TRIM"))
                {
                    type = java.sql.Types.CHAR;
                }
                functionArgString = ft.getField(1);
                ftArgs = new FieldTokenizer(functionArgString, ',', false);
                functionArgs = new java.util.Vector<Object>();
                argIndex = 0;
                while (ftArgs.hasMoreFields())
                {
                    nextArg = ftArgs.nextField();
                    tcol = new TsColumn(nextArg, tableDefs, inputContext);
                    if (tcol.isGroupedColumn()) groupedColumn = true;
                    /*
                     *          MAX and MIN functions can be either FLOAT or CHAR types
                     *          depending upon the type of the argument.
                     */
                    if (functionName.equals("MAX") | functionName.equals("MIN"))
                    {
                        if (argIndex > 0)
                            throw new TinySQLException("Function can only have 1 argument");
                        groupedColumn = true;
                        type = tcol.type;
                        size = tcol.size;
                    }
                    else if (functionName.equals("CONCAT"))
                    {
                        type = java.sql.Types.CHAR;
                        size += tcol.size;
                    }
                    else if (functionName.equals("UPPER"))
                    {
                        type = java.sql.Types.CHAR;
                        size = tcol.size;
                    }
                    else if (functionName.equals("TO_DATE"))
                    {
                        type = java.sql.Types.DATE;
                        size = 10;
                    }
                    else if (functionName.equals("TRIM"))
                    {
                        type = java.sql.Types.CHAR;
                        size = tcol.size;
                    }
                    else if (functionName.equals("SUBSTR"))
                    {
                        type = java.sql.Types.CHAR;
                        if (argIndex == 0 & tcol.type != java.sql.Types.CHAR)
                        {
                            throw new TinySQLException("SUBSTR first argument must be character");
                        }
                        else if (argIndex == 1)
                        {
                            if (tcol.type != java.sql.Types.INTEGER | tcol.intValue < 1)
                                throw new TinySQLException("SUBSTR second argument "
                                + tcol.getString() + " must be integer > 0");
                        }
                        else if (argIndex == 2)
                        {
                            if (tcol.type != java.sql.Types.INTEGER | tcol.intValue < 1)
                                throw new TinySQLException("SUBSTR third argument "
                                + tcol.getString() + " must be integer > 0");
                            size = tcol.intValue;
                        }
                    }
                    argIndex++;
                    functionArgs.addElement(tcol);
                }
            }
            else
            {
                /*
                 *       Check for SYSDATE
                 */
                if (name.toUpperCase().equals("SYSDATE"))
                {
                    isConstant = true;
                    type = java.sql.Types.DATE;
                    size = 10;
                    notNull = true;
                    valueSet = true;
                    //Basties mote: not really SimpleDateFormat needed... - need yyyy-MM-dd
                    // stringValue = fmtyyyyMMdd.format(today.getTime());
                    stringValue = System.DateTime.Today.ToString("yyyy-MM-dd");
                    /*
                     *          Check for a quoted string
                     */
                }
                else if (UtilString.isQuotedString(name))
                {
                    isConstant = true;
                    type = java.sql.Types.CHAR;
                    stringValue = UtilString.removeQuotes(name);
                    if (stringValue != (String)null)
                    {
                        size = stringValue.length();
                        notNull = true;
                        valueSet = true;
                    }
                }
                else
                {
                    /*
                     *          Check for a numeric constant
                     */
                    numericType = UtilString.getValueType(name);
                    if (numericType == java.sql.Types.INTEGER)
                    {
                        intValue = java.lang.Integer.valueOf(name).intValue();
                        size = 10;
                        type = numericType;
                        isConstant = true;
                        notNull = true;
                        valueSet = true;
                    }
                    else if (numericType == java.sql.Types.FLOAT)
                    {
                        floatValue = java.lang.Float.valueOf(name).floatValue();
                        size = 10;
                        type = numericType;
                        isConstant = true;
                        notNull = true;
                        valueSet = true;
                    }
                    else
                    {
                        /*
                         *             This should be a column name. 
                         */
                        columnTable = (TinySQLTable)null;
                        upperName = name.toUpperCase();
                        if (TinySQLGlobals.DEBUG)
                            java.lang.SystemJ.outJ.println("Trying to find table for " + upperName);
                        dotAt = upperName.indexOf(".");
                        if (dotAt > -1)
                        {
                            tableName = upperName.substring(0, dotAt);
                            if (tableDefs != (java.util.Hashtable<Object, Object>)null &
                                 tableName.indexOf("->") < 0)
                            {
                                t = (java.util.Vector<Object>)tableDefs.get("TABLE_SELECT_ORDER");
                                tableName = UtilString.findTableAlias(tableName, t);
                            }
                            upperName = upperName.substring(dotAt + 1);
                            /*
                             *                Check to see if this column name has a short equivalent.
                             */
                            if (upperName.length() > 11)
                            {
                                longName = name;
                                upperName = TinySQLGlobals.getShortName(upperName);
                            }
                            columnTable = (TinySQLTable)tableDefs.get(tableName);
                        }
                        else if (tableDefs != (java.util.Hashtable<Object, Object>)null)
                        {
                            /*
                             *                Check to see if this column name has a short equivalent.
                             */
                            if (upperName.length() > 11)
                            {
                                longName = name;
                                upperName = TinySQLGlobals.getShortName(upperName);
                            }
                            /*
                             *                Use an enumeration to go through all of the tables to find
                             *                this column.
                             */
                            t = (java.util.Vector<Object>)tableDefs.get("TABLE_SELECT_ORDER");
                            for (j = 0; j < t.size(); j++)
                            {
                                tableName = (String)t.elementAt(j);
                                jtbl = (TinySQLTable)tableDefs.get(tableName);
                                col_keys = jtbl.column_info.keys();
                                /*
                                 *                   Check all columns.
                                 */
                                while (col_keys.hasMoreElements())
                                {
                                    checkName = (String)col_keys.nextElement();
                                    if (checkName.equals(upperName))
                                    {
                                        upperName = checkName;
                                        columnTable = jtbl;
                                        break;
                                    }
                                }
                                if (columnTable != (TinySQLTable)null) break;
                            }
                        }
                        else
                        {
                            if (TinySQLGlobals.DEBUG)
                                java.lang.SystemJ.outJ.println("No table definitions.");
                        }
                        if (columnTable != (TinySQLTable)null)
                        {
                            name = columnTable.table + "->" + columnTable.tableAlias
                               + "." + upperName;
                            type = columnTable.ColType(upperName);
                            size = columnTable.ColSize(upperName);
                            decimalPlaces = columnTable.ColDec(upperName);
                            tableName = columnTable.table + "->" + columnTable.tableAlias;
                        }
                    }
                }
            }
        }
        /*
         * This function sets the column to a null value if the column belongs
         * to the input table, or the column is a function which has an
         * argument which belongs to the input table and whose value is null
         * if any argument is null.
         */
        public bool clear()
        {
            return clear((String)null);
        }
        public bool clear(String inputTableName)
        {
            int i;
            TsColumn argColumn;
            bool argClear;
            if (functionName == (String)null)
            {
                if (!isConstant)
                {
                    if (inputTableName == (String)null)
                    {
                        notNull = false;
                        valueSet = false;
                    }
                    else if (tableName == (String)null)
                    {
                        notNull = false;
                        valueSet = false;
                    }
                    else if (tableName.equals(inputTableName))
                    {
                        notNull = false;
                        valueSet = false;
                    }
                }
            }
            else
            {
                for (i = 0; i < functionArgs.size(); i++)
                {
                    argColumn = (TsColumn)functionArgs.elementAt(i);
                    argClear = argColumn.clear(inputTableName);
                    if (argClear & Utils.clearFunction(functionName))
                    {
                        notNull = false;
                        valueSet = false;
                    }
                }
            }
            return isNull();
        }
        /*
         * This method updates the value of the column.  In the case of a function
         * only the argument values are updated, not the function as a whole. Functions
         * must be done using updateFunctions because of the requirement 
         * to evaluate summary functions only once per row.
         */
        public void update(String inputColumnName, String inputColumnValue)
        //throws tinySQLException
        {
            int i;
            TsColumn argColumn;
            if (isConstant | inputColumnName == (String)null) return;
            if (inputColumnName.trim().length() == 0) return;
            if (functionName == (String)null)
            {
                /*
                 *       Only update the * column once per row.
                 */
                if (name.equals("*") & valueSet) return;
                if (inputColumnName.equals(name) | name.equals("*"))
                {
                    if (TinySQLGlobals.DEBUG)
                        java.lang.SystemJ.outJ.println("Set " + contextToString()
                        + " column " + name + " = " + inputColumnValue.trim());
                    /*
                     *          If this is a simple column value, reset to null before
                     *          trying to interpret the inputColumnValue.
                     */
                    valueSet = true;
                    notNull = false;
                    stringValue = (String)null;
                    intValue = java.lang.Integer.MIN_VALUE;
                    floatValue = java.lang.Float.MIN_VALUE;
                    /*
                     *          Empty string will be interpreted as nulls
                     */
                    if (inputColumnValue == (String)null) return;
                    if (inputColumnValue.trim().length() == 0) return;
                    notNull = true;
                    if (type == java.sql.Types.CHAR | type == java.sql.Types.DATE | type == -1)
                    {
                        stringValue = inputColumnValue;
                    }
                    else if (type == java.sql.Types.INTEGER & notNull)
                    {
                        try
                        {
                            intValue = java.lang.Integer.parseInt(inputColumnValue.trim());
                        }
                        catch (Exception )
                        {
                            throw new TinySQLException(inputColumnValue + " is not an integer.");
                        }
                    }
                    else if (type == java.sql.Types.FLOAT & notNull)
                    {
                        try
                        {
                            floatValue = java.lang.Float.valueOf(inputColumnValue.trim()).floatValue();
                        }
                        catch (Exception )
                        {
                            throw new TinySQLException(inputColumnValue + " is not a Float.");
                        }
                    }
                }
            }
            else
            {
                /*
                 *       Update the function arguments.
                 */
                for (i = 0; i < functionArgs.size(); i++)
                {
                    argColumn = (TsColumn)functionArgs.elementAt(i);
                    argColumn.update(inputColumnName, inputColumnValue);
                }
            }
        }
        /*
         * This method evaluates the value of functions.  This step must be kept
         * separate from the update of individual columns to prevent evaluation
         * of summary functions such as COUNT and SUM more than once, or when 
         * the row being processed will ultimately fail a where clause condition.
         */
        public void updateFunctions()
        //throws tinySQLException
        {
            int i, startAt, charCount, day, monthAt, month, year;
            TsColumn argColumn;
            java.lang.StringBuffer concatBuffer;
            FieldTokenizer ft;
            String[] ftFields;
            String months = "-JAN-FEB-MAR-APR-MAY-JUN-JUL-AUG-SEP-OCT-NOV-DEC-",
            monthName, dayField, monthField, yearField;
            if (isConstant) return;
            if (functionName == (String)null) return;
            if (functionName.equals("CONCAT"))
            {
                concatBuffer = new java.lang.StringBuffer();
                for (i = 0; i < functionArgs.size(); i++)
                {
                    argColumn = (TsColumn)functionArgs.elementAt(i);
                    argColumn.updateFunctions();
                    if (argColumn.isValueSet()) valueSet = true;
                    if (argColumn.notNull)
                    {
                        concatBuffer.append(argColumn.getString());
                        notNull = true;
                    }
                }
                stringValue = concatBuffer.toString();
            }
            else if (functionName.equals("UPPER"))
            {
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                if (argColumn.notNull)
                {
                    stringValue = argColumn.getString().toUpperCase();
                    notNull = true;
                }
            }
            else if (functionName.equals("TRIM"))
            {
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                if (argColumn.notNull)
                {
                    stringValue = argColumn.getString().trim();
                    notNull = true;
                }
            }
            else if (functionName.equals("SUBSTR"))
            {
                if (functionArgs.size() != 3)
                    throw new TinySQLException("Wrong number of arguments for SUBSTR");
                argColumn = (TsColumn)functionArgs.elementAt(1);
                startAt = argColumn.intValue;
                argColumn = (TsColumn)functionArgs.elementAt(2);
                charCount = argColumn.intValue;
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                if (argColumn.notNull)
                {
                    stringValue = argColumn.stringValue;
                    if (startAt < stringValue.length() - 1 & charCount > 0)
                    {
                        stringValue = stringValue.substring(startAt - 1, startAt + charCount - 1);
                        notNull = true;
                    }
                    else
                    {
                        stringValue = (String)null;
                    }
                }
            }
            else if (functionName.equals("COUNT"))
            {
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                /*
                 *       The COUNT function always returns a not null value
                 */
                notNull = true;
                valueSet = true;
                if (intValue == java.lang.Integer.MIN_VALUE)
                {
                    intValue = 0;
                }
                else
                {
                    intValue = intValue + 1;
                }
            }
            else if (functionName.equals("TO_DATE"))
            {
                /*
                 *       Validate the TO_DATE argument
                 */
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                type = java.sql.Types.DATE;
                size = 10;
                if (argColumn.notNull)
                {
                    stringValue = argColumn.getString().trim();
                    ft = new FieldTokenizer(stringValue, '-', false);
                    ftFields = ft.getFields();
                    if (ftFields.Length < 3)
                    {
                        throw new TinySQLException(stringValue + " is not a date with "
                        + "format DD-MON-YY!");
                    }
                    else
                    {
                        try
                        {
                            day = java.lang.Integer.parseInt(ftFields[0]);
                            if (day < 1 | day > 31)
                                throw new TinySQLException(stringValue + " day not "
                                + "between 1 and 31.");
                            monthName = ftFields[1].toUpperCase();
                            monthAt = months.indexOf("-" + monthName + "-");
                            if (monthAt == -1)
                                throw new TinySQLException(stringValue + " month not "
                                + "recognized.");
                            month = (monthAt + 4) / 4;
                            year = java.lang.Integer.parseInt(ftFields[2]);
                            if (year < 0 | year > 2100)
                                throw new TinySQLException(stringValue + " year not "
                                + "recognized.");
                            /*
                             *                Assume that years < 50 are in the 21st century, otherwise 
                             *                the 20th.
                             */
                            if (year < 50)
                            {
                                year = 2000 + year;
                            }
                            else if (year < 100)
                            {
                                year = 1900 + year;
                            }
                            dayField = java.lang.Integer.toString(day);
                            if (dayField.length() < 2) dayField = "0" + dayField;
                            monthField = java.lang.Integer.toString(month);
                            if (monthField.length() < 2) monthField = "0" + monthField;
                            yearField = java.lang.Integer.toString(year);
                            stringValue = yearField + "-" + monthField + "-" + dayField;
                        }
                        catch (Exception dayEx)
                        {
                            throw new TinySQLException(stringValue + " exception "
                            + dayEx.getMessage());
                        }
                    }
                    notNull = true;
                }
            }
            else if (functionName.equals("SUM"))
            {
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                if (argColumn.type == java.sql.Types.CHAR | argColumn.type == java.sql.Types.DATE)
                    throw new TinySQLException(argColumn.name + " is not numeric!");
                if (argColumn.notNull)
                {
                    notNull = true;
                    if (floatValue == java.lang.Float.MIN_VALUE)
                    {
                        floatValue = (float)0.0;
                    }
                    else
                    {
                        if (argColumn.type == java.sql.Types.INTEGER)
                            floatValue += new java.lang.Integer(argColumn.intValue).floatValue();
                        else
                            floatValue += argColumn.floatValue;
                    }
                }
            }
            else if (functionName.equals("MAX") | functionName.equals("MIN"))
            {
                argColumn = (TsColumn)functionArgs.elementAt(0);
                argColumn.updateFunctions();
                if (argColumn.isValueSet()) valueSet = true;
                if (argColumn.notNull)
                {
                    notNull = true;
                    if (argColumn.type == java.sql.Types.CHAR | argColumn.type == java.sql.Types.DATE)
                    {
                        if (stringValue == null)
                        {
                            stringValue = argColumn.stringValue;
                        }
                        else
                        {
                            /* 
                             *                Update the max and min based upon string comparisions.
                             */
                            if (functionName.equals("MAX") &
                               (argColumn.stringValue.compareTo(stringValue) > 0))
                            {
                                stringValue = argColumn.stringValue;
                            }
                            else if (functionName.equals("MIN") &
                             (argColumn.stringValue.compareTo(stringValue) < 0))
                            {
                                stringValue = argColumn.stringValue;
                            }
                        }
                    }
                    else if (argColumn.type == java.sql.Types.INTEGER)
                    {
                        /*
                         *             Update max and min based upon numeric values.
                         */
                        if (intValue == java.lang.Integer.MIN_VALUE)
                        {
                            intValue = argColumn.intValue;
                        }
                        else
                        {
                            if (functionName.equals("MIN") &
                               argColumn.intValue < intValue)
                                intValue = argColumn.intValue;
                            else if (functionName.equals("MAX") &
                               argColumn.intValue > intValue)
                                intValue = argColumn.intValue;
                        }
                    }
                    else if (argColumn.type == java.sql.Types.FLOAT)
                    {
                        if (floatValue == java.lang.Float.MIN_VALUE)
                        {
                            floatValue = argColumn.floatValue;
                        }
                        else
                        {
                            if (functionName.equals("MIN") &
                               argColumn.floatValue < floatValue)
                                floatValue = argColumn.floatValue;
                            else if (functionName.equals("MAX") &
                               argColumn.floatValue > floatValue)
                                floatValue = argColumn.floatValue;
                        }
                    }
                }
            }
        }
        public bool isGroupedColumn()
        {
            return groupedColumn;
        }
        public bool isValueSet()
        {
            return valueSet;
        }
        public bool isNotNull()
        {
            return notNull;
        }
        public bool isNull()
        {
            return !notNull;
        }
        /*
         * The following function compares this column to the input using
         * a "like" comparison using % as the wildcard.
         */
        public bool like(TsColumn inputColumn) //throws tinySQLException
        {
            FieldTokenizer ft;
            String nextField, firstField, lastField;
            bool like;
            int foundAt;
            if (!Utils.isCharColumn(type) | !Utils.isCharColumn(inputColumn.type))
                throw new TinySQLException("Column " + name + " or "
                + inputColumn.name + " is not character.");
            ft = new FieldTokenizer(inputColumn.stringValue, '%', true);
            like = true;
            foundAt = 0;
            firstField = (String)null;
            lastField = (String)null;
            while (ft.hasMoreFields())
            {
                nextField = ft.nextField();
                lastField = nextField;
                /*
                 *       If the first matching field is not the wildcare character
                 *       then the test field must start with this string.
                 */
                if (firstField == (String)null)
                {
                    firstField = nextField;
                    if (!firstField.equals("%") & !stringValue.startsWith(firstField))
                    {
                        like = false;
                        break;
                    }
                }
                if (!nextField.equals("%"))
                {
                    if (stringValue.indexOf(nextField, foundAt) < 0)
                    {
                        like = false;
                        break;
                    }
                    foundAt = stringValue.indexOf(nextField, foundAt) + 1;
                }
            }
            if (!lastField.equals("%") & !stringValue.endsWith(lastField))
                like = false;
            if (TinySQLGlobals.DEBUG)
                java.lang.SystemJ.outJ.println("Is " + getString() + " like " +
             inputColumn.getString() + " ? " + like);
            return like;
        }
        public Object clone() //throws CloneNotSupportedException
        {
            return base.MemberwiseClone();
        }
        public int compareTo(Object inputObj) //throws tinySQLException
        {
            String thisYMD, inputYMD;
            TsColumn inputColumn;
            int inputType, returnValue;
            double thisValue, inputValue;
            inputColumn = (TsColumn)inputObj;
            inputType = inputColumn.type;
            thisValue = java.lang.Double.MIN_VALUE;
            inputValue = java.lang.Double.MIN_VALUE;
            returnValue = 0;
            if (Utils.isCharColumn(type))
            {
                /*
                 *       Compare character java.sql.Types.
                 */
                if (!Utils.isCharColumn(inputType))
                {
                    throw new TinySQLException("Type mismatch between "
                    + getString() + " and " + inputColumn.getString());
                }
                else if (stringValue == (String)null |
                        inputColumn.stringValue == (String)null)
                {
                    throw new TinySQLException("One of the values is NULL");
                }
                else
                {
                    returnValue = stringValue.compareTo(inputColumn.stringValue);
                }
            }
            else if (Utils.isDateColumn(type))
            {
                /*
                 *       Compare date java.sql.Types.
                 */
                if (!Utils.isDateColumn(inputType))
                {
                    throw new TinySQLException("Type mismatch between "
                    + getString() + " and " + inputColumn.getString());
                }
                else if (stringValue == (String)null |
                        inputColumn.stringValue == (String)null)
                {
                    throw new TinySQLException("One of the values is NULL");
                }
                else
                {
                    inputYMD = UtilString.toStandardDate(inputColumn.stringValue);
                    thisYMD = UtilString.toStandardDate(stringValue);
                    returnValue = thisYMD.compareTo(inputYMD);
                }
            }
            else if (Utils.isNumberColumn(type))
            {
                if (type == java.sql.Types.INTEGER) thisValue = (double)intValue;
                else if (type == java.sql.Types.FLOAT) thisValue = (double)floatValue;
                if (inputType == java.sql.Types.INTEGER)
                    inputValue = (double)inputColumn.intValue;
                else if (inputType == java.sql.Types.FLOAT)
                    inputValue = (double)inputColumn.floatValue;
                if (thisValue > inputValue) returnValue = 1;
                else if (thisValue < inputValue) returnValue = -1;
            }
            else
            {
                java.lang.SystemJ.outJ.println("Cannot sort unknown type");
            }
            if (TinySQLGlobals.DEBUG)
                java.lang.SystemJ.outJ.println("Comparing " + getString() + " to " +
                inputColumn.getString() + " gave " + returnValue);
            return returnValue;
        }
        public void addContext(String inputContext)
        {
            if (inputContext != (String)null)
            {
                contextList.addElement(inputContext);
            }
        }
        /*
         * This method checks to see if the column has the specified context.
         */
        public String contextToString()
        {
            java.lang.StringBuffer outputBuffer = new java.lang.StringBuffer();
            int i;
            for (i = 0; i < contextList.size(); i++)
            {
                if (i > 0) outputBuffer.append(",");
                outputBuffer.append((String)contextList.elementAt(i));
            }
            return outputBuffer.toString();
        }
        /*
         * This method returns the list of contexts as a string
         */
        public bool getContext(String inputContext)
        {
            String nextContext;
            int i;
            for (i = 0; i < contextList.size(); i++)
            {
                nextContext = (String)contextList.elementAt(i);
                if (nextContext == (String)null) continue;
                if (nextContext.equals(inputContext)) return true;
            }
            return false;
        }
        /*
         * This method returns the value of the column as a string
         */
        public String getString()
        {
            if (!notNull) return "null";
            if (type == java.sql.Types.CHAR | type == java.sql.Types.DATE | type == -1)
            {
                return stringValue;
            }
            else if (type == java.sql.Types.INTEGER)
            {
                if (intValue == java.lang.Integer.MIN_VALUE) return (String)null;
                return java.lang.Integer.toString(intValue);
            }
            else if (type == java.sql.Types.FLOAT)
            {
                if (floatValue == java.lang.Float.MIN_VALUE) return (String)null;
                return java.lang.Float.toString(floatValue);
            }
            return (String)null;
        }
        public String toString()
        {
            int i;
            java.lang.StringBuffer outputBuffer = new java.lang.StringBuffer();
            if (functionName == (String)null)
            {
                outputBuffer.append("-----------------------------------" + newLine
                + "Column Name: " + name + newLine
                + "Table: " + tableName + newLine
                + "IsNotNull: " + notNull + newLine
                + "valueSet: " + valueSet + newLine
                + "IsConstant: " + isConstant + newLine
                + "Type: " + type + newLine
                + "Size: " + size + newLine
                + "Context: " + contextToString() + newLine
                + "Value: " + getString());
            }
            else
            {
                outputBuffer.append("Function: " + functionName + newLine
                + "IsNotNull: " + notNull + newLine
                + "Type: " + type + newLine
                + "Size: " + size + newLine
                + "Value: " + getString());
                for (i = 0; i < functionArgs.size(); i++)
                {
                    outputBuffer.append(newLine + "Argument " + i + " follows" + newLine
                    + ((TsColumn)functionArgs.elementAt(i)).toString() + newLine);
                }
            }
            return outputBuffer.toString();
        }
    }
}
