/*
 * Java program to execute SQL commands using the tinySQL JDBC driver.
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:26:02 $
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
 * Written by Davis Swan in November, 2003.
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    public class tinySQLCmd
    {
        static java.util.Vector<Object> tableList;
        static String dbVersion;
        static java.io.FileWriter spoolFileWriter = (java.io.FileWriter)null;
        static String newLine = java.lang.SystemJ.getProperty("line.separator");
        public static void main(String[] args) //throws IOException,SQLException
        {
            java.sql.DatabaseMetaData dbMeta;
            java.sql.ResultSetMetaData meta;
            java.sql.ResultSet display_rs, typesRS;
            java.io.BufferedReader stdin, loadFileReader;
            java.io.BufferedReader startReader = (java.io.BufferedReader)null;
            String[] fields;
            java.sql.Connection con;
            java.sql.Statement stmt;
            FieldTokenizer ft;
            java.sql.PreparedStatement pstmt = (java.sql.PreparedStatement)null;
            int i, rsColCount, endAt, colWidth, colScale, colPrecision, typeCount,
            colType, parameterIndex, b1, b2, parameterInt, startAt, columnIndex, valueIndex;
            String fName, tableName = null, inputString, cmdString, colTypeName, dbType,
            parameterString, loadString, fieldString, readString;
            java.lang.StringBuffer lineOut, prepareBuffer, valuesBuffer, inputBuffer;
            bool echo = false;
            stdin = new java.io.BufferedReader(new java.io.InputStreamReader(java.lang.SystemJ.inJ));
            try
            {
                /*
                 *       Register the JDBC driver for dBase
                 */
                java.lang.Class.forName("com.sqlmagic.tinysql.dbfFileDriver");
            }
            catch (java.lang.ClassNotFoundException e)
            {
                java.lang.SystemJ.err.println(
                     "JDBC Driver could not be registered!!\n");
                if (TinySQLGlobals.DEBUG) e.printStackTrace();
            }
            fName = ".";
            if (args.Length > 0) fName = args[0];
            /* 
             *    Establish a connection to dBase
             */
            con = dbConnect(fName);
            if (con == (java.sql.Connection)null)
            {
                fName = ".";
                con = dbConnect(fName);
            }
            dbMeta = con.getMetaData();
            dbType = dbMeta.getDatabaseProductName();
            dbVersion = dbMeta.getDatabaseProductVersion();
            java.lang.SystemJ.outJ.println("===================================================");
            java.lang.SystemJ.outJ.println(dbType + " Command line interface version "
            + dbVersion + " Java version released March 15, 2007");
            java.lang.SystemJ.outJ.println("Type HELP to get information on available commands.");
            cmdString = "NULL";
            stmt = con.createStatement();
            inputString = (String)null;
            if (args.Length > 1) inputString = args[1].trim();
            while (!cmdString.toUpperCase().equals("EXIT"))
            {
                try
                {
                    if (startReader != (java.io.BufferedReader)null)
                    {
                        /*
                         *             Command START files can contain comments and can have
                         *             commands broken over several lines.  However, they 
                         *             cannot have partial commands on a line.
                         */
                        inputBuffer = new java.lang.StringBuffer();
                        inputString = (String)null;
                        while ((readString = startReader.readLine()) != null)
                        {
                            if (readString.startsWith("--") |
                                 readString.startsWith("#")) continue;
                            inputBuffer.append(readString + " ");
                            /*
                             *                A field tokenizer must be used to avoid problems with
                             *                semi-colons inside quoted strings.
                             */
                            ft = new FieldTokenizer(inputBuffer.toString(), ';', true);
                            if (ft.countFields() > 1)
                            {
                                inputString = inputBuffer.toString();
                                break;
                            }
                        }
                        if (inputString == (String)null)
                        {
                            startReader = (java.io.BufferedReader)null;
                            continue;
                        }
                    }
                    else if (args.Length == 0)
                    {
                        java.lang.SystemJ.outJ.print("tinySQL>");
                        inputString = stdin.readLine().trim();
                    }
                    if (inputString == (String)null) break;
                    if (inputString.toUpperCase().startsWith("EXIT") |
                        inputString.toUpperCase().startsWith("QUIT")) break;
                    startAt = 0;
                    while (startAt < inputString.length() - 1)
                    {
                        endAt = inputString.indexOf(";", startAt);
                        if (endAt == -1)
                            endAt = inputString.length();
                        cmdString = inputString.substring(startAt, endAt);
                        if (echo) java.lang.SystemJ.outJ.println(cmdString);
                        startAt = endAt + 1;
                        if (cmdString.toUpperCase().startsWith("SELECT"))
                        {
                            display_rs = stmt.executeQuery(cmdString);
                            if (display_rs == (java.sql.ResultSet)null)
                            {
                                java.lang.SystemJ.outJ.println("Null ResultSet returned from query");
                                continue;
                            }
                            meta = display_rs.getMetaData();
                            /*
                             *                The actual number of columns retrieved has to be checked
                             */
                            rsColCount = meta.getColumnCount();
                            lineOut = new java.lang.StringBuffer(100);
                            int[] columnWidths = new int[rsColCount];
                            int[] columnScales = new int[rsColCount];
                            int[] columnPrecisions = new int[rsColCount];
                            int[] columnTypes = new int[rsColCount];
                            String[] columnNames = new String[rsColCount];
                            for (i = 0; i < rsColCount; i++)
                            {
                                columnNames[i] = meta.getColumnName(i + 1);
                                columnWidths[i] = meta.getColumnDisplaySize(i + 1);
                                columnTypes[i] = meta.getColumnType(i + 1);
                                columnScales[i] = meta.getScale(i + 1);
                                columnPrecisions[i] = meta.getPrecision(i + 1);
                                if (columnNames[i].length() > columnWidths[i])
                                    columnWidths[i] = columnNames[i].length();
                                lineOut.append(padString(columnNames[i], columnWidths[i]) + " ");
                            }
                            if (TinySQLGlobals.DEBUG)
                                java.lang.SystemJ.outJ.println(lineOut.toString());
                            displayResults(display_rs);
                        }
                        else if (cmdString.toUpperCase().startsWith("CONNECT"))
                        {
                            con = dbConnect(cmdString.substring(8, cmdString.length()));
                        }
                        else if (cmdString.toUpperCase().startsWith("HELP"))
                        {
                            helpMsg(cmdString);
                        }
                        else if (cmdString.toUpperCase().startsWith("DESCRIBE"))
                        {
                            dbMeta = con.getMetaData();
                            tableName = cmdString.toUpperCase().substring(9);
                            display_rs = dbMeta.getColumns(null, null, tableName, null);
                            java.lang.SystemJ.outJ.println("\nColumns for table " + tableName + "\n"
                            + "Name                            Type");
                            while (display_rs.next())
                            {
                                lineOut = new java.lang.StringBuffer(100);
                                lineOut.append(padString(display_rs.getString(4), 32));
                                colTypeName = display_rs.getString(6);
                                colType = display_rs.getInt(5);
                                colWidth = display_rs.getInt(7);
                                colScale = display_rs.getInt(9);
                                colPrecision = display_rs.getInt(10);
                                if (colTypeName.equals("CHAR"))
                                {
                                    colTypeName = colTypeName + "("
                                    + java.lang.Integer.toString(colWidth) + ")";
                                }
                                else if (colTypeName.equals("FLOAT"))
                                {
                                    colTypeName += "(" + java.lang.Integer.toString(colPrecision)
                                    + "," + java.lang.Integer.toString(colScale) + ")";
                                }
                                lineOut.append(padString(colTypeName, 20) + padString(colType, 12));
                                java.lang.SystemJ.outJ.println(lineOut.toString());
                            }
                        }
                        else if (cmdString.toUpperCase().equals("SHOW TABLES"))
                        {
                            for (i = 0; i < tableList.size(); i++)
                                java.lang.SystemJ.outJ.println((String)tableList.elementAt(i));
                        }
                        else if (cmdString.toUpperCase().equals("SHOW TYPES"))
                        {
                            typesRS = dbMeta.getTypeInfo();
                            typeCount = displayResults(typesRS);
                        }
                        else if (cmdString.toUpperCase().startsWith("SET "))
                        {
                            /*
                             *                Support for SET DEBUG ON/OFF and SET ECHO ON/OFF
                             */
                            ft = new FieldTokenizer(cmdString.toUpperCase(), ' ', false);
                            fields = ft.getFields();
                            if (fields[1].equals("ECHO"))
                            {
                                if (fields[2].equals("ON")) echo = true;
                                else echo = false;
                            }
                            else if (fields[1].equals("DEBUG"))
                            {
                                if (fields[2].equals("ON")) TinySQLGlobals.DEBUG = true;
                                else TinySQLGlobals.DEBUG = false;
                            }
                            else if (fields[1].equals("PARSER_DEBUG"))
                            {
                                if (fields[2].equals("ON"))
                                    TinySQLGlobals.PARSER_DEBUG = true;
                                else TinySQLGlobals.PARSER_DEBUG = false;
                            }
                            else if (fields[1].equals("WHERE_DEBUG"))
                            {
                                if (fields[2].equals("ON"))
                                    TinySQLGlobals.WHERE_DEBUG = true;
                                else TinySQLGlobals.WHERE_DEBUG = false;
                            }
                            else if (fields[1].equals("EX_DEBUG"))
                            {
                                if (fields[2].equals("ON"))
                                    TinySQLGlobals.EX_DEBUG = true;
                                else TinySQLGlobals.EX_DEBUG = false;
                            }
                        }
                        else if (cmdString.toUpperCase().startsWith("SPOOL "))
                        {
                            /*
                             *                Spool output to a file.
                             */
                            ft = new FieldTokenizer(cmdString, ' ', false);
                            fName = ft.getField(1);
                            if (fName.equals("OFF"))
                            {
                                try
                                {
                                    spoolFileWriter.close();
                                }
                                catch (Exception spoolEx)
                                {
                                    java.lang.SystemJ.outJ.println("Unable to close spool file "
                                    + spoolEx.getMessage() + newLine);
                                }
                            }
                            else
                            {
                                try
                                {
                                    spoolFileWriter = new java.io.FileWriter(fName);
                                    if (spoolFileWriter != (java.io.FileWriter)null)
                                        java.lang.SystemJ.outJ.println("Output spooled to " + fName);
                                }
                                catch (Exception spoolEx)
                                {
                                    java.lang.SystemJ.outJ.println("Unable to spool to file "
                                    + spoolEx.getMessage() + newLine);
                                }
                            }
                        }
                        else if (cmdString.toUpperCase().startsWith("START "))
                        {
                            ft = new FieldTokenizer(cmdString, ' ', false);
                            fName = ft.getField(1);
                            if (!fName.toUpperCase().endsWith(".SQL")) fName += ".SQL";
                            try
                            {
                                startReader = new java.io.BufferedReader(new java.io.FileReader(fName));
                            }
                            catch (Exception)
                            {
                                startReader = (java.io.BufferedReader)null;
                                throw new TinySQLException("No such file: " + fName);
                            }
                        }
                        else if (cmdString.toUpperCase().startsWith("LOAD"))
                        {
                            ft = new FieldTokenizer(cmdString, ' ', false);
                            fName = ft.getField(1);
                            tableName = ft.getField(3);
                            display_rs = stmt.executeQuery("SELECT * FROM " + tableName);
                            meta = display_rs.getMetaData();
                            rsColCount = meta.getColumnCount();
                            /*
                             *                Set up the PreparedStatement for the inserts
                             */
                            prepareBuffer = new java.lang.StringBuffer("INSERT INTO " + tableName);
                            valuesBuffer = new java.lang.StringBuffer(" VALUES");
                            for (i = 0; i < rsColCount; i++)
                            {
                                if (i == 0)
                                {
                                    prepareBuffer.append(" (");
                                    valuesBuffer.append(" (");
                                }
                                else
                                {
                                    prepareBuffer.append(",");
                                    valuesBuffer.append(",");
                                }
                                prepareBuffer.append(meta.getColumnName(i + 1));
                                valuesBuffer.append("?");
                            }
                            prepareBuffer.append(")" + valuesBuffer.toString() + ")");
                            try
                            {
                                pstmt = con.prepareStatement(prepareBuffer.toString());
                                loadFileReader = new java.io.BufferedReader(new java.io.FileReader(fName));
                                while ((loadString = loadFileReader.readLine()) != null)
                                {
                                    if (loadString.toUpperCase().equals("ENDOFDATA"))
                                        break;
                                    columnIndex = 0;
                                    valueIndex = 0;
                                    ft = new FieldTokenizer(loadString, '|', true);
                                    while (ft.hasMoreFields())
                                    {
                                        fieldString = ft.nextField();
                                        if (fieldString.equals("|"))
                                        {
                                            columnIndex++;
                                            if (columnIndex > valueIndex)
                                            {
                                                pstmt.setString(valueIndex + 1, (String)null);
                                                valueIndex++;
                                            }
                                        }
                                        else if (columnIndex < rsColCount)
                                        {
                                            pstmt.setString(valueIndex + 1, fieldString);
                                            valueIndex++;
                                        }
                                    }
                                    pstmt.executeUpdate();
                                }
                                pstmt.close();
                            }
                            catch (Exception loadEx)
                            {
                                java.lang.SystemJ.outJ.println(loadEx.getMessage());
                            }
                        }
                        else if (cmdString.toUpperCase().startsWith("SETSTRING") |
                                  cmdString.toUpperCase().startsWith("SETINT"))
                        {
                            b1 = cmdString.indexOf(" ");
                            b2 = cmdString.lastIndexOf(" ");
                            if (b2 > b1 & b1 > 0)
                            {
                                parameterIndex = java.lang.Integer.parseInt(cmdString.substring(b1 + 1, b2));
                                parameterString = cmdString.substring(b2 + 1);
                                if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Set parameter["
                               + parameterIndex + "]=" + parameterString);
                                if (cmdString.toUpperCase().startsWith("SETINT"))
                                {
                                    parameterInt = java.lang.Integer.parseInt(parameterString);
                                    pstmt.setInt(parameterIndex, parameterInt);
                                }
                                else
                                {
                                    pstmt.setString(parameterIndex, parameterString);
                                }
                                if (parameterIndex == 2)
                                    pstmt.executeUpdate();
                            }
                        }
                        else
                        {
                            if (cmdString.indexOf("?") > -1)
                            {
                                pstmt = con.prepareStatement(cmdString);
                            }
                            else
                            {
                                try
                                {
                                    stmt.executeUpdate(cmdString);
                                    java.lang.SystemJ.outJ.println("DONE\n");
                                }
                                catch (java.lang.Exception upex)
                                {
                                    java.lang.SystemJ.outJ.println(upex.getMessage());
                                    if (TinySQLGlobals.DEBUG) upex.printStackTrace();
                                }
                            }
                        }
                    }
                    if (args.Length > 1) cmdString = "EXIT";
                }
                catch (java.sql.SQLException te)
                {
                    java.lang.SystemJ.outJ.println(te.getMessage());
                    if (TinySQLGlobals.DEBUG) te.printStackTrace(java.lang.SystemJ.outJ);
                    inputString = (String)null;
                }
                catch (Exception e)
                {
                    java.lang.SystemJ.outJ.println(e.getMessage());
                    cmdString = "EXIT";
                    break;
                }
            }
            try
            {
                if (spoolFileWriter != (java.io.FileWriter)null) spoolFileWriter.close();
            }
            catch (Exception spoolEx)
            {
                java.lang.SystemJ.outJ.println("Unable to close spool file "
                + spoolEx.getMessage() + newLine);
            }
        }

        private static void helpMsg(String inputCmd)
        {
            String upperCmd;
            upperCmd = inputCmd.toUpperCase().trim();
            if (upperCmd.equals("HELP"))
            {
                java.lang.SystemJ.outJ.println("The following help topics are available:\n"
                + "=============================================================\n"
                + "HELP NEW - list of new features in tinySQL " + dbVersion + "\n"
                + "HELP COMMANDS - help for the non-SQL commands\n"
                + "HELP LIMITATIONS - limitations of tinySQL " + dbVersion + "\n"
                + "HELP ABOUT - short description of tinySQL.\n");
            }
            else if (upperCmd.equals("HELP COMMANDS"))
            {
                java.lang.SystemJ.outJ.println("The following non-SQL commands are supported:\n"
                + "=============================================================\n"
                + "SHOW TABLES - lists the tinySQL tables (DBF files) in the current "
                + "directory\n"
                + "SHOW TYPES - lists column types supported by tinySQL.\n"
                + "DESCRIBE table_name - describes the columns in table table_name.\n"
                + "CONNECT directory - connects to a different directory;\n"
                + "   Examples:  CONNECT C:\\TEMP in Windows\n"
                + "              CONNECT /home/mydir/temp in Linux/Unix\n"
                + "SET DEBUG ON/OFF - turns general debugging on or off\n"
                + "SET PARSER_DEBUG ON/OFF - turns parser debugging on or off\n"
                + "SET WHERE_DEBUG ON/OFF - turns where clause debugging on or off\n"
                + "SET EX_DEBUG ON/OFF - turns exception stack printing on or off\n"
                + "SET ECHO ON/OFF - will echo input commands\n"
                + "START <filename> - executes commands in the text file\n"
                + "SPOOL <filename> - spools output of commands to the file\n"
                + "EXIT - leave the tinySQL command line interface.\n");
            }
            else if (upperCmd.equals("HELP LIMITATIONS"))
            {
                java.lang.SystemJ.outJ.println("tinySQL " + dbVersion
                + " does NOT support the following:\n"
                + "=============================================================\n"
                + "Subqueries: eg SELECT COL1 from TABLE1 where COL2 in (SELECT ...\n"
                + "IN specification within a WHERE clause.\n"
                + "GROUP BY clause in SELECT statments.\n"
                + "AS in CREATE statements; eg CREATE TABLE TAB2 AS SELECT ...\n"
                + "UPDATE statements including JOINS.\n\n"
                + "If you run into others let us know by visiting\n"
                + "http://sourceforge.net/projects/tinysql\n");
            }
            else if (upperCmd.equals("HELP NEW"))
            {
                java.lang.SystemJ.outJ.println("New features in tinySQL releases.\n"
                + "=============================================================\n"
                + "Java version 2.26h released March 15, 2007\n"
                + "Corrected problems with date comparisions, added support for \n"
                + "the TO_DATE function, corrected problems with DELETE.\n"
                + "Added support for IS NULL and IS NOT NULL, added ability\n"
                + "to spool output to a file.\n"
                + "---------------------------------------------------------------\n"
                + "Version 2.10 released October 22, 2006\n"
                + "Added support for long column names and fixed bugs in \n"
                + "ALTER TABLE commands.\n"
                + "---------------------------------------------------------------\n"
                + "Version 2.02 released July 20, 2005\n"
                + "Fixed more bugs with the COUNT(*) and the like comparison.\n"
                + "---------------------------------------------------------------\n"
                + "Version 2.01 released April 20, 2005\n"
                + "Fixed several bugs with the COUNT(*) and other summary functions\n"
                + "Fixed ORDER BY using columns that were not SELECTed.\n"
                + "Added support for DISTINCT keyword and TRIM function.\n"
                + "Added default sorting by selected column.\n"
                + "Significant reorganization of code to allow the use of functions\n"
                + "in WHERE clauses (now stores tsColumn objects in tinySQLWhere).\n"
                + "---------------------------------------------------------------\n"
                + "Version 2.0 released Dec. 20, 2004\n"
                + "The package name was changed to com.sqlmagic.tinysql.\n"
                + "Support for table aliases in JOINS: see example below\n"
                + "  SELECT A.COL1,B.COL2 FROM TABLE1 A,TABLE2 B WHERE A.COL3=B.COL3\n"
                + "COUNT,MAX,MIN,SUM aggregate functions.\n"
                + "CONCAT,UPPER,SUBSTR in-line functions for strings.\n"
                + "SYSDATE - current date.\n"
                + "START script_file.sql - executes SQL commands in file.\n"
                + "Support for selection of constants: see example below:\n"
                + "  SELECT 'Full Name: ',first_name,last_name from person\n"
                + "All comparisions work properly: < > = != LIKE \n");
            }
            else if (upperCmd.equals("HELP ABOUT"))
            {
                java.lang.SystemJ.outJ.println(
                  "=============================================================\n"
                + "tinySQL was originally written by Brian Jepson\n"
                + "as part of the research he did while writing the book \n"
                + "Java Database Programming (John Wiley, 1996).  The database was\n"
                + "enhanced by Andreas Kraft, Thomas Morgner, Edson Alves Pereira,\n"
                + "and Marcel Ruff between 1997 and 2002.\n"
                + "The current version " + dbVersion
                + " was developed by Davis Swan starting in 2004.\n\n"
                + "tinySQL is free software; you can redistribute it and/or\n"
                + "modify it under the terms of the GNU Lesser General Public\n"
                + "License as published by the Free Software Foundation; either\n"
                + "version 2.1 of the License, or (at your option) any later version.\n"
                + "This library is distributed in the hope that it will be useful,\n"
                + "but WITHOUT ANY WARRANTY; without even the implied warranty of\n"
                + "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU\n"
                + "Lesser General Public License for more details at\n"
                + "http://www.gnu.org/licenses/lgpl.html");
            }
            else
            {
                java.lang.SystemJ.outJ.println("Unknown help command.\n");
            }
        }
        private static String padString(int inputint, int padLength)
        {
            return padString(java.lang.Integer.toString(inputint), padLength);
        }
        private static String padString(String inputString, int padLength)
        {
            java.lang.StringBuffer outputBuffer;
            String blanks = "                                        ";
            if (inputString != (String)null)
                outputBuffer = new java.lang.StringBuffer(inputString);
            else
                outputBuffer = new java.lang.StringBuffer(blanks);
            while (outputBuffer.length() < padLength)
                outputBuffer.append(blanks);
            return outputBuffer.toString().substring(0, padLength);
        }
        private static java.sql.Connection dbConnect(String tinySQLDir)// throws SQLException
        {
            java.sql.Connection con = null;
            java.sql.DatabaseMetaData dbMeta;
            java.io.File conPath;
            java.io.File[] fileList;
            String tableName;
            java.sql.ResultSet tables_rs;
            conPath = new java.io.File(tinySQLDir);
            fileList = conPath.listFiles();
            if (fileList == null)
            {
                java.lang.SystemJ.outJ.println(tinySQLDir + " is not a valid directory.");
                return (java.sql.Connection)null;
            }
            else
            {
                java.lang.SystemJ.outJ.println("Connecting to " + conPath.getAbsolutePath());
                con = java.sql.DriverManager.getConnection("jdbc:dbfFile:" + conPath, "", "");
            }
            dbMeta = con.getMetaData();
            tables_rs = dbMeta.getTables(null, null, null, null);
            tableList = new java.util.Vector<Object>();
            while (tables_rs.next())
            {
                tableName = tables_rs.getString("TABLE_NAME");
                tableList.addElement(tableName);
            }
            if (tableList.size() == 0)
                java.lang.SystemJ.outJ.println("There are no tinySQL tables in this directory.");
            else
                java.lang.SystemJ.outJ.println("There are " + tableList.size() + " tinySQL tables"
                + " in this directory.");
            return con;
        }
        /**
        Formatted output to stdout
        @return number of tuples
        */
        static int displayResults(java.sql.ResultSet rs)
        {//throws java.sql.SQLException
            {
                if (rs == null)
                {
                    java.lang.SystemJ.err.println("ERROR in displayResult(): No data in ResultSet");
                    return 0;
                }
                java.sql.Date testDate;
                int numCols = 0, nameLength;
                java.sql.ResultSetMetaData meta = rs.getMetaData();
                int cols = meta.getColumnCount();
                int[] width = new int[cols];
                String dashes = "=============================================";
                /*
                 *    Display column headers
                 */
                bool first = true;
                java.lang.StringBuffer head = new java.lang.StringBuffer();
                java.lang.StringBuffer line = new java.lang.StringBuffer();
                /*
                 *    Fetch each row
                 */
                while (rs.next())
                {
                    /*
                     *       Get the column, and see if it matches our expectations
                     */
                    String text = "";
                    for (int ii = 0; ii < cols; ii++)
                    {
                        String value = rs.getString(ii + 1);
                        if (first)
                        {
                            width[ii] = meta.getColumnDisplaySize(ii + 1);
                            if (TinySQLGlobals.DEBUG &
                                 meta.getColumnType(ii + 1) == java.sql.Types.DATE)
                            {
                                testDate = rs.getDate(ii + 1);
                                java.lang.SystemJ.outJ.println("Value " + value + ", Date "
                                + testDate.toString());
                            }
                            nameLength = meta.getColumnName(ii + 1).length();
                            if (nameLength > width[ii]) width[ii] = nameLength;
                            head.append(padString(meta.getColumnName(ii + 1), width[ii]));
                            head.append(" ");
                            line.append(padString(dashes + dashes, width[ii]));
                            line.append(" ");
                        }
                        text += padString(value, width[ii]);
                        text += " ";   // the gap between the columns
                    }
                    try
                    {
                        if (first)
                        {
                            if (spoolFileWriter != (java.io.FileWriter)null)
                            {
                                spoolFileWriter.write(head.toString() + newLine);
                                spoolFileWriter.write(head.toString() + newLine);
                            }
                            else
                            {
                                java.lang.SystemJ.outJ.println(head.toString());
                                java.lang.SystemJ.outJ.println(line.toString());
                            }
                            first = false;
                        }
                        if (spoolFileWriter != (java.io.FileWriter)null)
                            spoolFileWriter.write(text + newLine);
                        else
                            java.lang.SystemJ.outJ.println(text);
                        numCols++;
                    }
                    catch (Exception writeEx)
                    {
                        java.lang.SystemJ.outJ.println("Exception writing to spool file "
                        + writeEx.getMessage());
                    }
                }
                return numCols;
            }
        }
    }
}