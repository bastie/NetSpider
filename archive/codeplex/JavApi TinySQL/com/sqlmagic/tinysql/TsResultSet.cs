/*
 * tsResultSet.java - Result Set object for tinySQL.
 * 
 * Copyright 1996, Brian C. Jepson
 *                 (bjepson@ids.net)
 * $Author: davis $
 * $Date: 2004/12/18 21:26:18 $
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
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{

    /*
     *
     * tsResultSet - object to hold query results
     *
     * @author Thomas Morgner <mgs@sherito.org> Changed tsResultSet to support java.sql.Types,
     * a custom fetchsize, and storing a state for unfinished queries.
     * I also marked all members as private and use access-Functions to set or query
     * this ResultSets properties.
     */
    public class TsResultSet
    {

        private java.util.Vector<Object> rows;// = new Vector();    // all the rows
        private java.util.Vector<Object> rsColumns;// = new Vector(); // all the tsColumn objects
        private java.util.Vector<Object> selectColumns;// = new Vector(); // all the selected columns
        private java.util.Vector<Object> orderByColumns;// = new Vector(); // all ORDER BY columns
        private java.util.Vector<Object> tables; // SQL-Query information
        private TinySQLWhere whereC;

        private int fetchsize;
        private int windowStart;
        private int level;
        private TinySQL dbengine;
        private java.util.Hashtable<Object, Object> sTables;
        private String orderType;
        private bool distinct;
        private int type;
        private bool eof;
        private bool groupedColumns = false;
        public String newLine = java.lang.SystemJ.getProperty("line.separator");
        /*
         * The constructor with no arguments is provided for the Metadata
         * ResulSets.
         */
        public TsResultSet()
        :
            this((TinySQLWhere)null, (TinySQL)null){
        }
        public TsResultSet(TinySQLWhere w, TinySQL dbeng)
        {
            dbengine = dbeng;
            windowStart = 0;
            whereC = w;
            rows = new java.util.Vector<Object>();
            rsColumns = new java.util.Vector<Object>();
            selectColumns = new java.util.Vector<Object>();
            orderByColumns = new java.util.Vector<Object>();
            tables = new java.util.Vector<Object>();
        }
        /*
         * This method sets the initial state of the ResultSet, including adding
         * any grouping or ordering information.  If the ResultSet contains
         * summary functions, then a single row is added to the ResultSet initially.
         * If no rows are found that match any specified where clauses, this initial
         * row will be returned.
         */
        public void setState(int pstate, java.util.Hashtable<Object, Object> ptables,
           String inputOrderType, bool inputDistinct)
        //throws tinySQLException
        {
            int i;
            TsRow record = new TsRow();
            TsColumn initializeColumn;
            sTables = ptables;
            orderType = inputOrderType;
            distinct = inputDistinct;
            level = pstate;
            if (groupedColumns)
            {
                /*
                 *       Initialize the ResultSet with any not null summary functions
                 *       such as COUNT = 0 
                 */
                for (i = 0; i < rsColumns.size(); i++)
                {
                    initializeColumn = (TsColumn)rsColumns.elementAt(i);
                    /*
                     *          Evaluate all functions before adding the
                     *          column to the output record.
                     */
                    initializeColumn.updateFunctions();
                    if (initializeColumn.isNotNull())
                        record.put(initializeColumn.name, initializeColumn.getString());
                }
                addRow(record);
            }
        }
        public void setType(int type)
        {
            if ((type == java.sql.ResultSetConstants.TYPE_FORWARD_ONLY) ||
              (type == java.sql.ResultSetConstants.TYPE_SCROLL_SENSITIVE) ||
              (type == java.sql.ResultSetConstants.TYPE_SCROLL_INSENSITIVE))

                this.type = type;
        }
        public int getType()
        {
            return type;
        }
        public void setFetchSize(int i)
        {
            fetchsize = i;
        }
        public int getFetchSize()
        {
            return fetchsize;
        }
        public void addColumn(TsColumn col)
        {
            int i;
            bool addTable;
            TinySQLTable checkTable;
            rsColumns.addElement(col);
            if (col.getContext("SELECT"))
                selectColumns.addElement(col);
            if (col.getContext("ORDER"))
                orderByColumns.addElement(col);
            if (col.isGroupedColumn()) groupedColumns = true;
            /*
             *    Add the table that this column belongs to if required
             */
            addTable = true;
            if (col.columnTable != (TinySQLTable)null)
            {
                for (i = 0; i < tables.size(); i++)
                {
                    checkTable = (TinySQLTable)tables.elementAt(i);
                    if (checkTable.table.equals(col.columnTable.table))
                    {
                        addTable = false;
                        break;
                    }
                }
                if (addTable)
                {
                    tables.addElement(col.columnTable);
                }
            }
        }
        public bool isGrouped()
        {
            return groupedColumns;
        }
        public bool getMoreResults(int newPos, int fetchsize)
        {
            this.fetchsize = fetchsize;
            if (dbengine != null)
            {
                try
                {
                    if (type != java.sql.ResultSetConstants.TYPE_SCROLL_INSENSITIVE)
                    {
                        rows.removeAllElements();
                        windowStart = newPos;
                    }
                    dbengine.contSelectStatement(this);
                    if (level != 0)
                    {
                        eof = false;
                        return eof;
                    }
                }
                catch (TinySQLException )
                {
                }
            }
            eof = true;
            return eof;
        }
        /*
         * The following method adds a row to the ResultSet.  If sortRows is true then
         * the row will be added to the appriate location in the ResultSet (which 
         * defaults to the sort order of the columns being fetched).  If sort os false
         * the row is just appended.
         */
        public bool addRow(TsRow row)
        {
            return addRow(row, true);
        }
        public bool addRow(TsRow row, bool sortRows)
        {
            int i;
            bool sortUp = true;
            TsRow sortRow;
            if (!sortRows)
            {
                rows.addElement(row);
                return true;
            }
            if (orderType != (String)null)
                if (orderType.startsWith("DESC")) sortUp = false;
            /*
             *    Pass the list of ORDER BY columns to the new row to enable
             *    compareTo method.
             */
            row.setOrderBy(orderByColumns);
            if (rows.size() > 0)
            {
                /*
                 *       Insert or append the row depending upon the ORDER BY
                 *       conditions and a comparison of the new and the existing row.
                 */
                if (sortUp)
                {
                    for (i = rows.size() - 1; i > -1; i--)
                    {
                        sortRow = (TsRow)rows.elementAt(i);
                        if (row.compareTo(sortRow) < 0) continue;
                        if (row.compareTo(sortRow) == 0 & distinct) return true;
                        if (i == rows.size() - 1)
                            rows.addElement(row);
                        else
                            rows.insertElementAt(row, i + 1);
                        return true;
                    }
                }
                else
                {
                    for (i = rows.size() - 1; i > -1; i--)
                    {
                        sortRow = (TsRow)rows.elementAt(i);
                        if (row.compareTo(sortRow) > 0) continue;
                        if (row.compareTo(sortRow) == 0 & distinct) return true;
                        if (i == rows.size() - 1)
                            rows.addElement(row);
                        else
                            rows.insertElementAt(row, i + 1);
                        return true;
                    }
                }
                rows.insertElementAt(row, 0);
                return true;
            }
            rows.addElement(row);
            if ((fetchsize > 0) && (rows.size() >= fetchsize)) return false;
            return true;
        }
        /*
         * The following methods update a particular row in the result set.
         */
        public void updateRow(TsRow inputRow)
        {
            updateRow(inputRow, rows.size() - 1);
        }
        public void updateRow(TsRow inputRow, int rowIndex)
        {
            rows.setElementAt(inputRow, rowIndex);
        }
        public java.util.Vector<Object> getTables()
        {
            return tables;
        }
        public int getLevel()
        {
            return level;
        }
        public void setLevel(int l)
        {
            level = l;
        }
        public java.util.Hashtable<Object, Object> getTableState()
        {
            return sTables;
        }
        public TinySQLWhere getWhereClause()
        {
            return whereC;
        }
        /*
         * Returns the number of SELECT columns in the result set 
         */
        public int getColumnCount()
        {
            return selectColumns.size();
        }
        /*
         * Returns the number of columns in the result set including ORDER BY,
         * GROUP BY columns.
         */
        public int numcols()
        {
            return rsColumns.size();
        }
        /*
         * Update all the columns in the ResultSet.
         */
        public void updateColumns(String inputColumnName, String inputColumnValue)
        //throws tinySQLException
        {
            int i;
            TsColumn tcol;
            for (i = 0; i < rsColumns.size(); i++)
            {
                tcol = (TsColumn)rsColumns.elementAt(i);
                tcol.update(inputColumnName, inputColumnValue);
            }
        }
        /*
         * Returns the number of rows in the result set.
         */
        public int size()
        {
            return rows.size();
        }
        /*
         * Returns the tsRow at a given row offset (starts with zero).
         *
         * @param i the row offset/index
         */
        public TsRow rowAt(int row)
        {
            int i;
            if (row >= (windowStart + rows.size()))
            {
                getMoreResults(row, fetchsize);
            }
            i = row - windowStart;
            if (i < rows.size())
            {
                return (TsRow)rows.elementAt(i);
            }
            return null;
        }
        /*
         * Returns the tsColumn at a given column offset (starts with zero).
         * The second argument is true if all columns (as opposed to just SELECT
         * columns ) are to be returned.
         */
        public TsColumn columnAtIndex(int i)
        {
            return columnAtIndex(i, false);
        }
        public TsColumn columnAtIndex(int i, bool allColumns)
        {
            if (allColumns) return (TsColumn)rsColumns.elementAt(i);
            else return (TsColumn)selectColumns.elementAt(i);

        }
        /* 
         * Debugging method to dump out the result set
         */
        public String toString()
        {
            int i;
            TsColumn tcol;
            java.lang.StringBuffer outputBuffer;
            /*
             *    Display columns
             */
            outputBuffer = new java.lang.StringBuffer(newLine + "Columns in ResultSet"
            + newLine);
            for (i = 0; i < rsColumns.size(); i++)
            {
                tcol = (TsColumn)rsColumns.elementAt(i);
                outputBuffer.append(tcol.toString());
            }
            outputBuffer.append(newLine + "Rows in tsResultSet" + newLine);
            for (i = 0; i < size(); i++)
            {
                TsRow row = rowAt(i);
                outputBuffer.append(row.toString() + newLine);
            }
            return outputBuffer.toString();
        }
    }
}