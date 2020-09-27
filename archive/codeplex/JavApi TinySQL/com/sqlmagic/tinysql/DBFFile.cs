/*
 *
 * dbfFile - an extension of tinySQL for dbf file access
 *
 * Copyright 1996 John Wiley & Sons, Inc.
 * See the COPYING file for redistribution details.
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:27:51 $
 * $Revision: 1.1 $
 *
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    /**
    dBase read/write access <br>
    @author Brian Jepson <bjepson@home.com>
    @author Marcel Ruff <ruff@swand.lake.de> Added write access to dBase and JDK 2 support
    @author Thomas Morgner <mgs@sherito.org> Changed ColumnName to 11 bytes and strip name
     after first occurence of 0x00.
     Types are now handled as java.sql.Types, not as character flag
    */
    public class DBFFile : TinySQL
    {

        public static String dataDir;
        public static bool debug = false;
        private java.util.Vector<Object> tableList = new java.util.Vector<Object>();
        static DBFFile()
        {

            try
            {
                dataDir = java.lang.SystemJ.getProperty("user.home") + java.io.File.separator + ".tinySQL";
            }
            catch (Exception )
            {
                java.lang.SystemJ.err.println("tinySQL: unable to get user.home property, " +
                                     "reverting to current working directory.");
                dataDir = "." + java.io.File.separator + ".tinySQL";
            }

        }

        /**
         *
         * Constructs a new dbfFile object
         *
         */
        public DBFFile()
            : base()
        {

            if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Set datadir=" + dataDir);

        }

        /**
         *
         * Constructs a new dbfFile object
         *
         * @param d directory with which to override the default data directory
         *
         */
        public DBFFile(String d)
            : base()
        {

            dataDir = d; // d is usually extracted from the connection URL
            if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Set datadir=" + dataDir);

        }


        /**
         *
         * Creates a table given the name and a vector of
         * column definition (tsColumn) arrays.
         *
         * @param tableName the name of the table
         * @param v a Vector containing arrays of column definitions.
         * @see tinySQL#CreateTable
         *
         */
        void setDataDir(String d)
        {
            /*
             *   Method to set datadir - this is a crude way to allow support for
             *   multiple tinySQL connections
             */
            dataDir = d;
        }
        internal override void CreateTable(String tableName, java.util.Vector<Object> v)
        {//    throws IOException, tinySQLException {

            //---------------------------------------------------
            // determin meta data ....
            int numCols = v.size();
            int recordLength = 1;        // 1 byte for the flag field
            for (int i = 0; i < numCols; i++)
            {
                TsColumn coldef = ((TsColumn)v.elementAt(i));
                recordLength += coldef.size;
            }

            //---------------------------------------------------
            // create the new dBase file ...
            DBFHeader dbfHeader = new DBFHeader(numCols, recordLength);
            java.io.RandomAccessFile ftbl = dbfHeader.create(dataDir, tableName);

            //---------------------------------------------------
            // write out the rest of the columns' definition.
            for (int i = 0; i < v.size(); i++)
            {
                TsColumn coldef = ((TsColumn)v.elementAt(i));
                Utils.log("CREATING COL=" + coldef.name);
                writeColdef(ftbl, coldef);
            }

            ftbl.write((byte)0x0d); // header section ends with CR (carriage return)

            ftbl.close();
        }


        /**
         * Creates new Columns in tableName, given a vector of
         * column definition (tsColumn) arrays.<br>
         * It is necessary to copy the whole file to do this task.
         *
         * ALTER TABLE table [ * ] ADD [ COLUMN ] column type
         *
         * @param tableName the name of the table
         * @param v a Vector containing arrays of column definitions.
         * @see tinySQL#AlterTableAddCol
         */
        internal override void AlterTableAddCol(String tableName, java.util.Vector<Object> v)
        {//throws IOException, tinySQLException {

            // rename the file ...
            String fullpath = dataDir + java.io.File.separator + tableName + DBFFileTable.dbfExtension;
            String tmppath = dataDir + java.io.File.separator + tableName + "_tmp_tmp" + DBFFileTable.dbfExtension;
            if (Utils.renameFile(fullpath, tmppath) == false)
                throw new TinySQLException("ALTER TABLE ADD COL error in renaming " + fullpath);

            try
            {
                // open the old file ...
                java.io.RandomAccessFile ftbl_tmp = new java.io.RandomAccessFile(tmppath, "r");

                // read the first 32 bytes ...
                DBFHeader dbfHeader_tmp = new DBFHeader(ftbl_tmp);

                // read the column info ...
                java.util.Vector<Object> coldef_list = new java.util.Vector<Object>(dbfHeader_tmp.numFields + v.size());
                int locn = 0; // offset of the current column
                for (int i = 1; i <= dbfHeader_tmp.numFields; i++)
                {
                    TsColumn coldef = readColdef(ftbl_tmp, tableName, i, locn);
                    locn += coldef.size; // increment locn by the length of this field.
                    coldef_list.addElement(coldef);
                }

                // add the new column definitions to the existing ...
                for (int jj = 0; jj < v.size(); jj++)
                    coldef_list.addElement(v.elementAt(jj));

                // create the new table ...
                CreateTable(tableName, coldef_list);

                // copy the data from old to new

                // opening new created dBase file ...
                java.io.RandomAccessFile ftbl = new java.io.RandomAccessFile(fullpath, "rw");
                ftbl.seek(ftbl.length()); // go to end of file

                int numRec = 0;
                for (int iRec = 1; iRec <= dbfHeader_tmp.numRecords; iRec++)
                {

                    String str = GetRecord(ftbl_tmp, dbfHeader_tmp, iRec);

                    // Utils.log("Copy of record#" + iRec + " str='" + str + "' ...");

                    if (str == null) continue; // record was marked as deleted, ignore it

                    ftbl.write(str.getBytes(Utils.encode));     // write original record
                    numRec++;

                    for (int iCol = 0; iCol < v.size(); iCol++) // write added columns
                    {
                        TsColumn coldef = (TsColumn)v.elementAt(iCol);

                        // enforce the correct column length
                        String value = Utils.forceToSize(coldef.defaultVal, coldef.size, " ");

                        // transform to byte and write to file
                        byte[] b = value.getBytes(Utils.encode);
                        ftbl.write(b);
                    }
                }

                ftbl_tmp.close();

                DBFHeader.writeNumRecords(ftbl, numRec);
                ftbl.close();

                Utils.delFile(tmppath);

            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }
        }



        /**
         * Retrieve a record (=row)
         * @param dbfHeader dBase meta info
         * @param recordNumber starts with 1
         * @return the String with the complete record
         *         or null if the record is marked as deleted
         * @see tinySQLTable#GetCol
         */
        public String GetRecord(java.io.RandomAccessFile ff, DBFHeader dbfHeader, int recordNumber) //throws tinySQLException
        {
            if (recordNumber < 1)
                throw new TinySQLException("Internal error - current record number < 1");

            try
            {
                // seek the starting offset of the current record,
                // as indicated by recordNumber
                ff.seek(dbfHeader.headerLength + (recordNumber - 1) * dbfHeader.recordLength);

                // fully read a byte array out to the length of
                // the record.
                byte[] b = new byte[dbfHeader.recordLength];
                ff.readFully(b);

                // make it into a String
                String record = new java.lang.StringJ(b, Utils.encode);

                // remove deleted records
                if (DBFFileTable.isDeleted(record))
                    return null;

                return record;

            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }
        }


        /**
         *
         * Deletes Columns from tableName, given a vector of
         * column definition (tsColumn) arrays.<br>
         *
         * ALTER TABLE table DROP [ COLUMN ] column { RESTRICT | CASCADE }
         *
         * @param tableName the name of the table
         * @param v a Vector containing arrays of column definitions.
         * @see tinySQL#AlterTableDropCol
         *
         */
        internal override void AlterTableDropCol(String tableName, java.util.Vector<Object> v)
        {//throws IOException, tinySQLException {

            // rename the file ...
            String fullpath = dataDir + java.io.File.separator + tableName + DBFFileTable.dbfExtension;
            String tmppath = dataDir + java.io.File.separator + tableName + "-tmp" + DBFFileTable.dbfExtension;
            if (Utils.renameFile(fullpath, tmppath) == false)
                throw new TinySQLException("ALTER TABLE DROP COL error in renaming " + fullpath);

            try
            {
                // open the old file ...
                java.io.RandomAccessFile ftbl_tmp = new java.io.RandomAccessFile(tmppath, "r");

                // read the first 32 bytes ...
                DBFHeader dbfHeader_tmp = new DBFHeader(ftbl_tmp);

                // read the column info ...
                java.util.Vector<Object> coldef_list = new java.util.Vector<Object>(dbfHeader_tmp.numFields - v.size());
                int locn = 0; // offset of the current column

              nextCol: for (int i = 1; i <= dbfHeader_tmp.numFields; i++)
                {

                    TsColumn coldef = readColdef(ftbl_tmp, tableName, i, locn);

                    // remove the DROP columns from the existing cols ...
                    for (int jj = 0; jj < v.size(); jj++)
                    {
                        String colName = (String)v.elementAt(jj);
                        if (coldef.name.equals(colName))
                        {
                            Utils.log("Dropping " + colName);
                            goto nextCol;
                        }
                    }

                    locn += coldef.size; // increment locn by the length of this field.
                    // Utils.log("Recycling " + coldef.name);
                    coldef_list.addElement(coldef);
                }

                // create the new table ...
                CreateTable(tableName, coldef_list);

                // copy the data from old to new

                // opening new created dBase file ...
                java.io.RandomAccessFile ftbl = new java.io.RandomAccessFile(fullpath, "rw");
                ftbl.seek(ftbl.length()); // go to end of file

                int numRec = 0;
                for (int iRec = 1; iRec <= dbfHeader_tmp.numRecords; iRec++)
                {

                    if (DBFFileTable.isDeleted(ftbl_tmp, dbfHeader_tmp, iRec) == true) continue;

                    numRec++;

                    ftbl.write(DBFFileTable.RECORD_IS_NOT_DELETED);  // write flag

                    // Read the whole column into the table's cache
                    String column = DBFFileTable._GetCol(ftbl_tmp, dbfHeader_tmp, iRec);

                    for (int iCol = 0; iCol < coldef_list.size(); iCol++) // write columns
                    {
                        TsColumn coldef = (TsColumn)coldef_list.elementAt(iCol);

                        // Extract column values from cache
                        String value = DBFFileTable.getColumn(coldef, column);
                        java.lang.SystemJ.outJ.println("From cache column value" + value);

                        value = Utils.forceToSize(value, coldef.size, " "); // enforce the correct column length

                        byte[] b = value.getBytes(Utils.encode);            // transform to byte and write to file
                        ftbl.write(b);
                    }
                }

                ftbl_tmp.close();

                // remove temp file
                java.io.File f = new java.io.File(tmppath);
                if (f.exists())
                    f.delete();

                DBFHeader.writeNumRecords(ftbl, numRec);
                ftbl.close();

            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }
        }


        /*
         * Rename columns
         *
         * ALTER TABLE table RENAME war TO peace
         */
        internal override void AlterTableRenameCol(String tableName, String oldColname, String newColname)
        //throws tinySQLException
        {
            String fullpath = dataDir + java.io.File.separator + tableName + DBFFileTable.dbfExtension;
            try
            {
                java.io.RandomAccessFile ftbl = new java.io.RandomAccessFile(fullpath, "rw");

                DBFHeader dbfHeader = new DBFHeader(ftbl); // read the first 32 bytes ...

                int locn = 0; // offset of the current column
                for (int iCol = 1; iCol <= dbfHeader.numFields; iCol++)
                {
                    TsColumn coldef = readColdef(ftbl, tableName, iCol, locn);
                    if (coldef.name.equals(oldColname))
                    {
                        Utils.log("Replacing column name '" + oldColname + "' with '" + newColname + "'");
                        ftbl.seek((iCol - 1) * 32 + 32);
                        ftbl.write(Utils.forceToSize(newColname,
                              DBFFileTable.FIELD_TYPE_INDEX - DBFFileTable.FIELD_NAME_INDEX,
                              (byte)0));
                        ftbl.close();
                        return;
                    }
                }
                ftbl.close();
                throw new TinySQLException("Renaming of column name '" + oldColname + "' to '" + newColname + "' failed, no column '" + oldColname + "' found");
            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }

        }

        /**
         *
         * Return a tinySQLTable object, given a table name.
         *
         * @param tableName
         * @see tinySQL#getTable
         *
         */
        internal override TinySQLTable getTable(String tableName) //throws tinySQLException 
        {
            int i, tableIndex;
            TinySQLTable nextTable;
            tableIndex = java.lang.Integer.MIN_VALUE;
            if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Trying to create table"
          + " object for " + tableName);
            for (i = 0; i < tableList.size(); i++)
            {
                nextTable = (TinySQLTable)tableList.elementAt(i);
                if (nextTable.table.equals(tableName))
                {
                    if (nextTable.isOpen())
                    {
                        if (TinySQLGlobals.DEBUG)
                            java.lang.SystemJ.outJ.println("Found in cache " + nextTable.toString());
                        return nextTable;
                    }
                    tableIndex = i;
                    break;
                }
            }
            if (tableIndex == java.lang.Integer.MIN_VALUE)
            {
                tableList.addElement(new DBFFileTable(dataDir, tableName));
                nextTable = (TinySQLTable)tableList.lastElement();
                if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Add to cache "
              + nextTable.toString());
                return (TinySQLTable)tableList.lastElement();
            }
            else
            {
                tableList.setElementAt(new DBFFileTable(dataDir, tableName), tableIndex);
                nextTable = (TinySQLTable)tableList.elementAt(tableIndex);
                if (TinySQLGlobals.DEBUG) java.lang.SystemJ.outJ.println("Update in cache "
              + nextTable.toString());
                return (TinySQLTable)tableList.elementAt(tableIndex);
            }
        }

        /**
         *
         * The DBF File class provides read-only access to DBF
         * files, so this baby should throw an exception.
         *
         * @param fname table name
         * @see tinySQL#DropTable
         *
         */
        internal override void DropTable(String fname)
        {//throws tinySQLException {
            DBFHeader.dropTable(dataDir, fname);
        }
        /**
        Reading a column definition from file<br>
        @param ff file handle (correctly positioned)
        @param iCol index starts with 1
        @param locn offset to the current column
        @return struct with column info
        */
        internal static TsColumn readColdef(java.io.RandomAccessFile ff, String tableName, int iCol, int locn) //throws tinySQLException
        {
            try
            {
                // seek the position of the field definition data.
                // This information appears after the first 32 byte
                // table information, and lives in 32 byte chunks.
                //
                ff.seek((iCol - 1) * 32 + 32);

                // get the column name into a byte array
                //
                byte[] b = new byte[11];
                ff.readFully(b);

                // convert the byte array to a String
                // Seek first 0x00 occurence and strip array after that
                //
                // some C-implementations do not set the remaining bytes
                // after the name to 0x00, so we have to correct this.
                //bool clear = false;
                int i = 0;
                while ((i < 11) && (b[i] != 0))
                {
                    i++;
                }
                while (i < 11)
                {
                    b[i] = 0;
                    i++;
                }
                String colName = (new java.lang.StringJ(b, Utils.encode)).trim();
                // read in the column type which follows the 11 byte column name
                //
                byte[] c = new byte[1];
                c[0] = ff.readByte();
                String ftyp = new java.lang.StringJ(c, Utils.encode);

                // skip four bytes
                //
                ff.skipBytes(4);

                // get field length and precision which are in the two bytes following
                // the column type.
                //
                short flen = Utils.fixByte(ff.readByte()); // 16
                short fdec = Utils.fixByte(ff.readByte()); // 17
                if (ftyp.equals("N") & fdec == 0) ftyp = "I";

                // bytes 18 - 31 are reserved

                // create a new tsColumn object and assign it the
                // attributes of the current field
                //
                if (TinySQLGlobals.DEBUG)
                    java.lang.SystemJ.outJ.println("Try and create tsColumn for " + colName);
                TsColumn column = new TsColumn(colName);
                /*
                 *    The column type is now given as java.sql.Types constant
                 */
                column.type = typeToSQLType(ftyp);
                column.size = flen;
                column.decimalPlaces = fdec;
                column.position = locn + 1;  // set the field position to the current
                column.tableName = tableName;
                return column;

            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }
        }

        /**
        Writing a column definition to file<br>
        NOTE: the file pointer (seek()) must be at the correct position
        @param ff file handle (correctly positioned)
        @param coldef struct with column info
        */
        void writeColdef(java.io.RandomAccessFile ff, TsColumn coldef) //throws tinySQLException
        {
            // Utils.log("Writing Field Def: coldef.name=" + coldef.name + ", coldef.type=" + coldef.type + ", cildef.size=" + coldef.size);

            try
            {
                ff.write(Utils.forceToSize(coldef.name,
                          DBFFileTable.FIELD_TYPE_INDEX - DBFFileTable.FIELD_NAME_INDEX,
                          (byte)0));

                // Convert the Java.SQL.Type back to a DBase Type and write it
                String type = null;
                if (coldef.type == java.sql.Types.CHAR || coldef.type == java.sql.Types.VARCHAR || coldef.type == java.sql.Types.LONGVARCHAR)
                    type = "C";
                else
                    if (coldef.type == java.sql.Types.NUMERIC || coldef.type == java.sql.Types.INTEGER ||
                        coldef.type == java.sql.Types.TINYINT || coldef.type == java.sql.Types.SMALLINT ||
                        coldef.type == java.sql.Types.BIGINT || coldef.type == java.sql.Types.FLOAT ||
                        coldef.type == java.sql.Types.DOUBLE || coldef.type == java.sql.Types.REAL)
                        type = "N";
                    else
                        if (coldef.type == java.sql.Types.BIT)
                            type = "L";
                        else
                            if (coldef.type == java.sql.Types.DATE)
                                type = "D";
                            else
                                type = "M";

                ff.write(Utils.forceToSize(type,
                          1,
                          (byte)0));

                ff.write(Utils.forceToSize(null,
                          4,
                          (byte)0));             // imu field (in memory use) 12-15

                ff.write(coldef.size);           // one byte

                ff.write(coldef.decimalPlaces);  // one byte

                ff.write(Utils.forceToSize(null,
                          DBFHeader.BULK_SIZE - DBFFileTable.FIELD_RESERVED_INDEX,
                          (byte)0));
            }
            catch (Exception e)
            {
                throw new TinySQLException(e.getMessage());
            }
        }


        /**
        'C' Char (max 254 bytes)
        'N' '-.0123456789' (max 19 bytes)
        'L' 'YyNnTtFf?' (1 byte)
        'M' 10 digit .DBT block number
        'D' 8 digit YYYYMMDD
        *
        * Uses java.sql.Types as key
        */
        internal static String typeToLiteral(int type)
        {
            if (type == java.sql.Types.CHAR) return "CHAR";
            if (type == java.sql.Types.VARCHAR) return "VARCHAR";
            if (type == java.sql.Types.FLOAT) return "FLOAT";
            if (type == java.sql.Types.NUMERIC) return "NUMERIC";
            if (type == java.sql.Types.INTEGER) return "INT";
            if (type == java.sql.Types.BIT) return "BIT";
            if (type == java.sql.Types.BINARY) return "BINARY";
            if (type == java.sql.Types.DATE) return "DATE";
            return "CHAR"; // fallback
        }


        /**
        'C' Char (max 254 bytes)
        'N' '-.0123456789' (max 19 bytes)
        'L' 'YyNnTtFf?' (1 byte)
        'M' 10 digit .DBT block number
        'D' 8 digit YYYYMMDD
        */
        static int typeToSQLType(String type)
        {
            if (type.equals("C")) return java.sql.Types.CHAR;
            if (type.equals("N")) return java.sql.Types.FLOAT;
            if (type.equals("I")) return java.sql.Types.INTEGER;
            if (type.equals("L")) return java.sql.Types.CHAR;
            if (type.equals("M")) return java.sql.Types.INTEGER;
            if (type.equals("D")) return java.sql.Types.DATE;
            return java.sql.Types.CHAR; // fallback
        }
    }
}