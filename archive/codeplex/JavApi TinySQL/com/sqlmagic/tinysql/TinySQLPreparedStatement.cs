/*
 * PreparedStatement object for the tinySQL driver
 *
 * A lot of this code is based on or directly taken from
 * George Reese's (borg@imaginary.com) mSQL driver.
 *
 * So, it's probably safe to say:
 *
 * Portions of this code Copyright (c) 1996 George Reese
 *
 * The rest of it:
 *
 * Copyright 1996, Brian C. Jepson
 *                 (bjepson@ids.net)
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:31:53 $
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

    /**
     * @author Thomas Morgner <mgs@sherito.org> statementString contains the last
     * used SQL-Query. Support for set/getFetchSize, ResultSets are created with a
     * reference to the creating statement
     */
    public class TinySQLPreparedStatement : java.sql.PreparedStatement
    {

        

        /**
         * Holds the original prepare stement including ? placeholders for 
         * values that will be replaced later.
         */
        private String statementString;
        /**
         * Holds the list of substitution values to be used in the prepared
         * statement.
         */
        private java.util.Vector<Object> substitute = (java.util.Vector<Object>)null;
        /**
         * Holds the list of table file objects so that they can be closed
         * when all the updates have been completed.
         */
        private java.util.Vector<Object> tableList = new java.util.Vector<Object>();
        /**
         * Holds the error message for invalid substitution index.
         */
        private String invalidIndex = (String)null;
        /**
         * Holds the last used queryString. execute() has to be synchronized,
         * to guarantee thread-safety
         */
        /**
         *
         * A connection object to execute queries and... stuff
         *
         */
        private TinySQLConnection connection;

        /**
         *
         * A result set returned from this query 
         *
         */
        private TinySQLResultSet result;
        /**
         *
         * A set of actions returned by tinySQLParser (see tinySQL.java)
         *
         */
        private java.util.Vector<Object> actions = (java.util.Vector<Object>)null;

        /**
         *
         * The max field size for tinySQL
         * This can be pretty big, before things start to break.
         *
         */
        private int max_field_size = 0;

        /**
         *
         * The max rows supported by tinySQL
         * I can't think of any limits, right now, but I'm sure some
         * will crop up...
         *
         */
        private int max_rows = 65536;

        /**
         *
         * The number of seconds the driver will allow for a SQL statement to
         * execute before giving up.  The default is to wait forever (0).
         *
         */
        private int timeout = 0;

        /**
         * How many rows to fetch in a single run. Default is now 4096 rows.
         */
        private int fetchsize = 4096;
        /**
         * Debug flag
         */
        private static bool debug = false;
        /**
         *
         * Constructs a new tinySQLStatement object.
         * @param conn the tinySQLConnection object
         *
         */
        public TinySQLPreparedStatement(TinySQLConnection conn, String inputString)
        {

            int nextQuestionMark, startAt;
            connection = conn;
            startAt = 0;
            statementString = inputString;
            while ((nextQuestionMark = statementString.indexOf("?", startAt)) > -1)
            {
                if (substitute == (java.util.Vector<Object>)null) substitute = new java.util.Vector<Object>();
                substitute.addElement("");
                startAt = nextQuestionMark + 1;
            }
            invalidIndex = " is not in the range 1 to "
            + java.lang.Integer.toString(substitute.size());
            if (debug) java.lang.SystemJ.outJ.println("Prepare statement has " + substitute.size()
          + " parameters.");

            this.setPoolable(true); //Basties note: see Java documentation java.sql.SQLStatement.isPoolable
        }

        /**
         *
         * Execute an SQL statement and return a result set.
         * @see java.sql.PreparedStatement#executeQuery
         * @exception SQLException raised for any errors
         * @param sql the SQL statement string
         * @return the result set from the query
         *
         */
        public java.sql.ResultSet executeQuery()
        {//throws SQLException {
            lock (this)
            {
                // tinySQL only supports one result set at a time, so
                // don't let them get another one, just in case it's
                // hanging out.
                //
                result = null;

                // create a new tinySQLResultSet with the tsResultSet
                // returned from connection.executetinySQL()
                //
                if (debug) java.lang.SystemJ.outJ.println("executeQuery conn is " + connection.toString());
                return new TinySQLResultSet(connection.executetinySQL(this), this);
            }
        }
        public java.sql.ResultSet executeQuery(String sql)
        {//  throws SQLException {
            lock (this)
            {
                // tinySQL only supports one result set at a time, so
                // don't let them get another one, just in case it's
                // hanging out.
                //
                result = null;
                statementString = sql;

                // create a new tinySQLResultSet with the tsResultSet
                // returned from connection.executetinySQL()
                //
                if (debug) java.lang.SystemJ.outJ.println("executeQuery conn is " + connection.toString());
                return new TinySQLResultSet(connection.executetinySQL(this), this);
            }
        }

        /**
         * 
         * Execute an update, insert, delete, create table, etc. This can
         * be anything that doesn't return rows.
         * @see java.sql.PreparedStatement#executeUpdate
         * @exception java.sql.SQLException thrown when an error occurs executing
         * the SQL
         * @return either the row count for INSERT, UPDATE or DELETE or 0 for SQL statements that return nothing
         */
        public int executeUpdate(String sql)
        {//throws SQLException {
            lock (this)
            {
                statementString = sql;
                return connection.executetinyUpdate(this);
            }
        }
        public int executeUpdate()
        {//throws SQLException {
            lock (this)
            {
                return connection.executetinyUpdate(this);
            }
        }

        /**
         * 
         * Executes some SQL and returns true or false, depending on
         * the success. The result set is stored in result, and can
         * be retrieved with getResultSet();
         * @see java.sql.PreparedStatement#execute
         * @exception SQLException raised for any errors
         * @param sql the SQL to be executed
         * @return true if there is a result set available
         */
        public bool execute()
        {//throws SQLException {

            // a result set object
            //
            TsResultSet r;

            // execute the query 
            //
            r = connection.executetinySQL(this);

            // check for a null result set. If it wasn't null,
            // use it to create a tinySQLResultSet, and return whether or
            // not it is null (not null returns true).
            //
            if (r == null)
            {
                result = null;
            }
            else
            {
                result = new TinySQLResultSet(r, this);
            }
            return (result != null);

        }
        public bool execute(String sql)
        {//throws SQLException {

            // a result set object
            //
            TsResultSet r;
            statementString = sql;

            // execute the query 
            //
            r = connection.executetinySQL(this);

            // check for a null result set. If it wasn't null,
            // use it to create a tinySQLResultSet, and return whether or
            // not it is null (not null returns true).
            //
            if (r == null)
            {
                result = null;
            }
            else
            {
                result = new TinySQLResultSet(r, this);
            }
            return (result != null);

        }

        /**
         * Returns the current query-String 
         */
        public String getSQLString()
        {
            return statementString;
        }

        /**
         * 
         * Close any result sets. This is not used by tinySQL.
         * @see java.sql.PreparedStatement#close
         *
         */
        public void close() //throws SQLException
        {
            int i;
            TinySQLTable nextTable;
            for (i = 0; i < tableList.size(); i++)
            {
                nextTable = (TinySQLTable)tableList.elementAt(i);
                if (debug) java.lang.SystemJ.outJ.println("Closing " + nextTable.table);
                nextTable.close();
            }
        }

        /**
         * 
         * Returns the last result set
         * @see java.sql.PreparedStatement#getResultSet
         * @return null if no result set is available, otherwise a result set
         *
         */
        public java.sql.ResultSet getResultSet()
        {//throws SQLException {

            java.sql.ResultSet r;

            r = result;    // save the existing result set
            result = null; // null out the existing result set
            return r;      // return the previously extant result set
        }

        /**
         * 
         * Return the row count of the last operation. tinySQL does not support
         * this, so it returns -1
         * @see java.sql.PreparedStatement#getUpdateCount
         * @return -1
         */
        public int getUpdateCount()
        {//throws SQLException {
            return -1;
        }

        /**
         *
         * This returns true if there are any pending result sets. This
         * should only be true after invoking execute() 
         * @see java.sql.PreparedStatement#getMoreResults
         * @return true if rows are to be gotten
         *
         */
        public bool getMoreResults()
        {//throws SQLException {

            return (result != null);

        }

        /**
         *
         * Get the maximum field size to return in a result set.
         * @see java.sql.PreparedStatement#getMaxFieldSize
         * @return the value of max field size
         *
         */
        public int getMaxFieldSize()
        {//throws SQLException {
            return max_field_size;
        }

        /**
         *
         * set the max field size.
         * @see java.sql.PreparedStatement#setMaxFieldSize
         * @param max the maximum field size
         *
         */
        public void setMaxFieldSize(int max)
        {//throws SQLException {
            max_field_size = max;
        }

        /**
         * 
         * Get the maximum row count that can be returned by a result set.
         * @see java.sql.PreparedStatement#getMaxRows
         * @return the maximum rows 
         *
         */
        public int getMaxRows()
        {//throws SQLException {
            return max_rows;
        }

        /**
         *
         * Get the maximum row count that can be returned by a result set.
         * @see java.sql.PreparedStatement.setMaxRows
         * @param max the max rows
         *
         */
        public void setMaxRows(int max)
        {//throws SQLException {
            max_rows = max;
        }

        /**
         *
         * If escape scanning is on (the default) the driver will do
         * escape substitution before sending the SQL to the database.
         * @see java.sql.PreparedStatement#setEscapeProcessing
         * @param enable this does nothing right now
         *
         */
        public void setEscapeProcessing(bool enable)
        {//throws SQLException {
            throw new java.sql.SQLException("The tinySQL Driver doesn't " +
                                   "support escape processing.");
        }

        /**
         *
         * Discover the query timeout.
         * @see java.sql.PreparedStatement#getQueryTimeout
         * @see setQueryTimeout
         * @return the timeout value for this statement
         *
         */
        public int getQueryTimeout()
        {//throws SQLException {
            return timeout;
        }

        /**
         *
         * Set the query timeout.
         * @see java.sql.PreparedStatement#setQueryTimeout
         * @see getQueryTimeout
         * @param x the new query timeout value
         *
         */
        public void setQueryTimeout(int x)
        {//throws SQLException {
            timeout = x;
        }

        /**
         *
         * This can be used by another thread to cancel a statement. This
         * doesn't matter for tinySQL, as far as I can tell.
         * @see java.sql.PreparedStatement#cancel
         *
         */
        public void cancel()
        {
        }

        /**
         *
         * Get the warning chain associated with this Statement
         * @see java.sql.PreparedStatement#getWarnings
         * @return the chain of warnings
         *
         */
        public java.sql.SQLWarning getWarnings()
        {//throws SQLException {
            return null;
        }

        /**
         *
         * Clear the warning chain associated with this Statement
         * @see java.sql.PreparedStatement#clearWarnings
         *
         */
        public void clearWarnings()
        {//throws SQLException {
        }

        /**
         * 
         * Sets the cursor name for this connection. Presently unsupported.
         *
         */
        public void setCursorName(String unused)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support cursors.");
        }

        //--------------------------JDBC 2.0-----------------------------


        /**
         * JDBC 2.0
         *
         * Gives the driver a hint as to the direction in which
             * the rows in a result set
         * will be processed. The hint applies only to result sets created 
         * using this Statement object.  The default value is 
         * ResultSet.FETCH_FORWARD.
         * <p>Note that this method sets the default fetch direction for 
             * result sets generated by this <code>Statement</code> object.
             * Each result set has its own methods for getting and setting
             * its own fetch direction.
         * @param direction the initial direction for processing rows
         * @exception SQLException if a database access error occurs
             * or the given direction
         * is not one of ResultSet.FETCH_FORWARD, ResultSet.FETCH_REVERSE, or
         * ResultSet.FETCH_UNKNOWN
         */
        public void setFetchDirection(int direction)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support setFetchDirection.");
        }

        /**
         * JDBC 2.0
         *
         * Retrieves the direction for fetching rows from
             * database tables that is the default for result sets
             * generated from this <code>Statement</code> object.
             * If this <code>Statement</code> object has not set
             * a fetch direction by calling the method <code>setFetchDirection</code>,
             * the return value is implementation-specific.
         *
         * @return the default fetch direction for result sets generated
             *          from this <code>Statement</code> object
         * @exception SQLException if a database access error occurs
         */
        public int getFetchDirection()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support getFetchDirection.");
        }

        /**
         * JDBC 2.0
         *
         * Gives the JDBC driver a hint as to the number of rows that should 
         * be fetched from the database when more rows are needed.  The number 
         * of rows specified affects only result sets created using this 
         * statement. If the value specified is zero, then the hint is ignored.
         * The default value is zero.
         *
         * @param rows the number of rows to fetch
         * @exception SQLException if a database access error occurs, or the
         * condition 0 <= rows <= this.getMaxRows() is not satisfied.
         */
        public void setFetchSize(int rows)
        {//throws SQLException {
            if ((rows <= 0) || (rows >= this.getMaxRows()))
                throw new java.sql.SQLException("Condition 0 <= rows <= this.getMaxRows() is not satisfied");

            fetchsize = rows;
        }

        /**
         * JDBC 2.0
         *
         * Retrieves the number of result set rows that is the default 
             * fetch size for result sets
             * generated from this <code>Statement</code> object.
             * If this <code>Statement</code> object has not set
             * a fetch size by calling the method <code>setFetchSize</code>,
             * the return value is implementation-specific.
         * @return the default fetch size for result sets generated
             *          from this <code>Statement</code> object
         * @exception SQLException if a database access error occurs
         */
        public int getFetchSize()
        {//throws SQLException {
            return fetchsize;
        }

        /**
         * JDBC 2.0
         *
         * Retrieves the result set concurrency.
         */
        public int getResultSetConcurrency()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support ResultSet concurrency.");
        }

        /**
         * JDBC 2.0
         *
         * Determine the result set type.
         */
        public int getResultSetType()
        {// throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support getResultSetType.");
        }

        /**
         * JDBC 2.0
         *
         * Adds a SQL command to the current batch of commmands for the statement.
         * This method is optional.
         *
         * @param sql typically this is a static SQL INSERT or UPDATE statement
         * @exception SQLException if a database access error occurs, or the
         * driver does not support batch statements
         */
        public void addBatch()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support addBatch.");
        }
        public void addBatch(String sql)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support addBatch.");
        }

        /**
         * JDBC 2.0
         *
         * Makes the set of commands in the current batch empty.
         * This method is optional.
         *
         * @exception SQLException if a database access error occurs or the
         * driver does not support batch statements
         */
        public void clearBatch()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support clearBatch.");
        }

        /**
         * JDBC 2.0
         * 
         * Submits a batch of commands to the database for execution.
         * This method is optional.
         *
         * @return an array of update counts containing one element for each
         * command in the batch.  The array is ordered according 
         * to the order in which commands were inserted into the batch.
         * @exception SQLException if a database access error occurs or the
         * driver does not support batch statements
         */
        public int[] executeBatch()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support executeBatch.");
        }

        /**
         * JDBC 2.0
         * 
         * Returns the <code>Connection</code> object
             * that produced this <code>Statement</code> object.
             * @return the connection that produced this statement
         * @exception SQLException if a database access error occurs
         */
        public java.sql.Connection getConnection() // throws SQLException
        {
            return connection;
        }
        /*
         *  Set methods for the prepared statement.
         */
        public void setBoolean(int parameterIndex, bool inputValue)
        //throws SQLException
        {
            if (inputValue) setString(parameterIndex, "TRUE");
            else setString(parameterIndex, "FALSE");
        }
        public void setInt(int parameterIndex, int inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Integer.toString(inputValue));
        }
        public void setDouble(int parameterIndex, double inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Double.toString(inputValue));
        }
        public void setBigDecimal(int parameterIndex, java.math.BigDecimal inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setDate(int parameterIndex, java.sql.Date inputValue,
           java.util.Calendar inputCalendar) //throws SQLException
        {
            String dateString;
            /*
             *     Convert string to YYYYMMDD format that dBase needs.
             */
            if (inputValue == (java.sql.Date)null)
            {
                setString(parameterIndex, (String)null);
            }
            else if (inputValue.toString().trim().length() < 8)
            {
                setString(parameterIndex, (String)null);
            }
            else
            {
                dateString = inputValue.toString().trim();
                /*
                 *        Convert date string to the standard YYYYMMDD format
                 */
                dateString = UtilString.dateValue(dateString);
                setString(parameterIndex, dateString);
            }
        }
        public void setDate(int parameterIndex, java.sql.Date inputValue)
        //throws SQLException
        {
            String dateString;
            /*
             *     Convert string to YYYYMMDD format that dBase needs.
             */
            dateString = UtilString.dateValue(inputValue.toString());
            setString(parameterIndex, dateString);
        }
        public void setTime(int parameterIndex, java.sql.Time inputValue,
           java.util.Calendar inputCalendar) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setTime(int parameterIndex, java.sql.Time inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setTimestamp(int parameterIndex, java.sql.Timestamp inputValue,
           java.util.Calendar inputCalendar) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setTimestamp(int parameterIndex, java.sql.Timestamp inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setAsciiStream(int parameterIndex,
           java.io.InputStream inputValue, int streamLength) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setUnicodeStream(int parameterIndex,
           java.io.InputStream inputValue, int streamLength) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setBinaryStream(int parameterIndex,
           java.io.InputStream inputValue, int streamLength) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setCharacterStream(int parameterIndex,
           java.io.Reader inputValue, int streamLength) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setRef(int parameterIndex, java.sql.Ref inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setBlob(int parameterIndex, java.sql.Blob inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setArray(int parameterIndex, java.sql.Array inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setClob(int parameterIndex, java.sql.Clob inputValue)
        //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setByte(int parameterIndex, byte inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Byte.toString(inputValue));
        }
        public void setBytes(int parameterIndex, byte[] inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Byte.toString(inputValue[0]));
        }
        public void setShort(int parameterIndex, short inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Short.toString(inputValue));
        }
        public void setFloat(int parameterIndex, float inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Float.toString(inputValue));
        }
        public void setLong(int parameterIndex, long inputValue)
        //throws SQLException
        {
            setString(parameterIndex, java.lang.Long.toString(inputValue));
        }
        public void setObject(int parameterIndex, Object inputValue)
        //throws SQLException
        {
            setObject(parameterIndex, inputValue, 0, 0);
        }
        public void setObject(int parameterIndex, Object inputValue,
           int targetSQLType) //throws SQLException
        {
            setObject(parameterIndex, inputValue, targetSQLType, 0);
        }
        public void setObject(int parameterIndex, Object inputValue,
           int targetSQLType, int scale) //throws SQLException
        {
            setString(parameterIndex, inputValue.toString());
        }
        public void setNull(int parameterIndex, int sqlType)
        //throws SQLException
        {
            setNull(parameterIndex, sqlType, (String)null);
        }
        public void setNull(int parameterIndex, int sqlType, String sqlTypeName)
        //throws SQLException
        {
            if (parameterIndex > substitute.size())
                throw new java.sql.SQLException("Parameter index " + parameterIndex
                + invalidIndex);
            substitute.setElementAt((String)null, parameterIndex - 1);
        }
        public void setString(int parameterIndex, String setString)
        //throws SQLException
        {
            if (parameterIndex > substitute.size())
                throw new java.sql.SQLException("Parameter index " + parameterIndex
                + invalidIndex);
            substitute.setElementAt(setString, parameterIndex - 1);
        }
        public void clearParameters() //throws SQLException
        {
            substitute.removeAllElements();
        }
        /*
         *  Update the actions based upon the contents of the substitute Vector.
         *  Only INSERT and UPDATE commands are supported at this time.
         */
        public void updateActions(java.util.Vector<Object> inputActions) //throws SQLException
        {
            java.util.Vector<Object> values, originalValues;
            java.util.Hashtable<Object, Object> action;
            String actionType, valueString;
            int i, j, subCount;
            if (actions == null)
                actions = inputActions;
            if (actions == null) return;
            for (i = 0; i < actions.size(); i++)
            {
                action = (java.util.Hashtable<Object, Object>)actions.elementAt(i);
                actionType = (String)action.get("TYPE");
                if (actionType.equals("INSERT") | actionType.equals("UPDATE"))
                {
                    /*
                     *           Look for the original values (with the ? for parameters).
                     */
                    originalValues = (java.util.Vector<Object>)action.get("ORIGINAL_VALUES");
                    values = (java.util.Vector<Object>)action.get("VALUES");
                    if (originalValues == (java.util.Vector<Object>)null)
                    {
                        originalValues = (java.util.Vector<Object>)values.clone();
                        action.put("ORIGINAL_VALUES", originalValues);
                    }
                    subCount = 0;
                    for (j = 0; j < originalValues.size(); j++)
                    {
                        valueString = (String)originalValues.elementAt(j);
                        if (valueString.equals("?"))
                        {
                            if (subCount > substitute.size() - 1)
                                throw new java.sql.SQLException("Substitution index " + subCount
                                + " not between 0 and "
                                + java.lang.Integer.toString(substitute.size() - 1));
                            values.setElementAt(substitute.elementAt(subCount), j);
                            subCount++;
                        }
                    }
                }
            }
        }
        public void addTable(TinySQLTable inputTable)
        {
            int i;
            TinySQLTable nextTable;
            for (i = 0; i < tableList.size(); i++)
            {
                nextTable = (TinySQLTable)tableList.elementAt(i);
                if (nextTable.table.equals(inputTable.table)) return;
            }
            tableList.addElement(inputTable);
        }


        public java.util.Vector<Object> getActions()
        {
            return actions;
        }
        public java.sql.ResultSetMetaData getMetaData()
        {
            return null;
        }

        //Basties note: not implemented methods following...
        public bool isWrapperFor(java.lang.Class clazz)
        {
            return false;
        }
        public T unwrap<T>(java.lang.Class iface)
        {
            throw new TinySQLException();
        }
        // Basties note: Here comes missing methods...
        public int getResultSetHoldability()
        {
            if (isClosed()) throw new java.sql.SQLException();
            return java.sql.ResultSetConstants.CLOSE_CURSORS_AT_COMMIT;
        }
        protected bool closing = false;
        public bool isClosed()
        {
            return this.closing;
        }
        private bool poolable = false;
        public void setPoolable(bool b)
        {
            if (this.isClosed()) throw new TinySQLException("Statemant is closed.");
            this.poolable = b;
        }
        public bool isPoolable()
        {
            return this.poolable;
        }
        public bool getMoreResults(int i)
        {
            throw new java.lang.UnsupportedOperationException("Not yet implemented");
        }
        public java.sql.ResultSet getGeneratedKeys()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public int executeUpdate(String sql, int autoGeneratedKeys)
        {
            switch (autoGeneratedKeys)
            {
                case java.sql.StatementConstants.RETURN_GENERATED_KEYS:
                    throw new java.sql.SQLFeatureNotSupportedException();
                default:
                    throw new java.lang.UnsupportedOperationException();
            }
        }
        public int executeUpdate(String sql, int[] columnIndexes)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public int executeUpdate(String sql, String[] columnNames)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public bool execute(String sql, int autoGeneratedKeys)
        {
            if (autoGeneratedKeys != java.sql.StatementConstants.RETURN_GENERATED_KEYS &&
                autoGeneratedKeys != java.sql.StatementConstants.NO_GENERATED_KEYS)
            {
                throw new java.sql.SQLException();
            }
            switch (autoGeneratedKeys)
            {
                case java.sql.StatementConstants.RETURN_GENERATED_KEYS:
                    throw new java.sql.SQLFeatureNotSupportedException();
                default:
                    throw new java.lang.UnsupportedOperationException();
            }
        }
        public bool execute(String sql, int[] columnIndexes)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public bool execute(String sql, String[] columnNames)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNClob(int column, java.io.Reader reader)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNClob(int column, java.sql.NClob nclob)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNClob(int column, java.io.Reader reader, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setClob(int column, java.io.Reader reader)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setClob(int column, java.io.Reader reader, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setBlob(int column, java.io.Reader reader)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setBlob(int column, java.io.InputStream inputStream, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNCharacterStream(int column, java.io.Reader reader)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNCharacterStream(int column, java.io.Reader reader, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setCharacterStream(int column, java.io.Reader reader)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setCharacterStream(int column, java.io.Reader reader, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setBlob(int column, java.io.InputStream inputStream)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setCharacterStream(int column, java.io.InputStream inputStream)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setCharacterStream(int column, java.io.InputStream inputStream, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setBinaryStream(int column, java.io.InputStream inputStream)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setBinaryStream(int column, java.io.InputStream inputStream, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setAsciiStream(int column, java.io.InputStream inputStream)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setAsciiStream(int column, java.io.InputStream inputStream, long length)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setSQLXML(int column, java.sql.SQLXML sqlxml)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setNString(int column, String value)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setRowId(int column, java.sql.RowId rowid)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setURL(int column, java.net.URL url)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.ParameterMetaData getParameterMetaData()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
    }
}