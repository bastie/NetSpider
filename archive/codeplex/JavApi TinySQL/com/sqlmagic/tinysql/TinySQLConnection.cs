/*
 * tinySQLConnection - a Connection object for the tinySQL JDBC Driver.
 * 
 * Note that since the tinySQL class is abstract, this class needs to
 * be abstract, as well. It's only in such manifestations of tinySQL
 * as textFile that the tinySQLConnection can reach its true potential.
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
 * $Date: 2004/12/18 21:28:32 $
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

    /**
     * @author Thomas Morgner <mgs@sherito.org> executetinySQL is now called with a statement
     * containing the SQL-Query String.
     */
    public abstract class TinySQLConnection : java.sql.Connection
    {

        /**
         *
         * The tinySQL object
         *
         */
        protected TinySQL tsql = null;

        /**
         *
         * The JDBC driver 
         *
         */
        protected java.sql.Driver driver;

        /**
         *
         * The URL to the datasource
         *
         */
        protected internal String url;

        /**
         *
         * The user name - currently unused
         *
         */
        protected String user;

        /**
         *
         * the catalog - it's not used by tinySQL
         *
         */
        protected String catalog;

        /**
         *
         * Transaction isolation level - it's not used by tinySQL
         *
         */
        protected int isolation;

        static bool debug = false;
        /**
         * 
         * Constructs a new JDBC Connection for a tinySQL database
         *
         * @exception SQLException in case of an error
         * @param user the user name - currently unused
         * @param u the URL used to connect to the datasource
         * @param d the Driver that instantiated this connection
         *
         */
        public TinySQLConnection(String user, String u, java.sql.Driver d)
        {//throws SQLException {

            this.url = u;
            this.user = user;
            this.driver = d;

            // call get_tinySQL() to return a new tinySQL object.
            // get_tinySQL() is an abstract method which allows
            // subclasses of tinySQL, such as textFile, to be used
            // as JDBC datasources
            //
            tsql = get_tinySQL();

        }

        /**
         *
         * Create and return a tinySQLStatement.
         * @see java.sql.Connection#createStatement
         * @exception SQLException thrown in case of error
         *
         */
        public java.sql.Statement createStatement()
        {//throws SQLException {
            return (java.sql.Statement)new TinySQLStatement(this);
        }

        /**
         *
         * Create and return a PreparedStatement. tinySQL doesn't support
         * these, so it always throws an exception.
         *
         * @see java.sql.Connection#prepareStatement
         * @param sql the SQL Statement
         * @exception SQLException gets thrown if you even look at this method
         *
         */
        public java.sql.PreparedStatement prepareStatement(String sql)
        {//throws SQLException {
            return (java.sql.PreparedStatement)new TinySQLPreparedStatement(this, sql);
        }

        /**
         *
         * Create and return a CallableStatement. tinySQL does not support
         * stored procs, so this automatically throws an exception.
         *
         * @see java.sql.Connection#prepareCall
         * @param sql the SQL Statement
         * @exception SQLException gets thrown always
         *
         */
        public java.sql.CallableStatement prepareCall(String sql)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support stored procedures.");
        }

        /**
         *
         * Converts escaped SQL to tinySQL syntax. This is not supported yet,
         * but some level of it will be meaningful, when tinySQL begins to
         * support scalar functions. For now, it just returns the original SQL.
         * 
         * @see java.sql.Connection#nativeSQL
         * @param sql the SQL statement
         * @return just what you gave it
         *
         */
        public String nativeSQL(String sql)
        {//throws SQLException {
            return sql;
        }

        /**
         *
         * Sets autocommit mode - tinySQL has no support for transactions,
         * so this does nothing.
         * @see java.sql.Connection#setAutoCommit
         * @param b this does nothing
         *
         */
        public void setAutoCommit(bool b)
        {//throws SQLException {
        }

        /**
         *
         * Commits a transaction. Since all SQL statements are implicitly
         * committed, it's save to preserve the illusion, and when this
         * method is invoked, it does not throw an exception.
         * @see java.sql.Connection#commit
         *
         */
        public void commit()
        {//throws SQLException {
        }

        /**
         * 
         * Rolls back a transaction. tinySQL does not support transactions,
         * so this throws an exception.
         * @see java.sql.Connection#rollback
         * @exception SQLException gets thrown automatically
         *
         */
        public void rollback()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support rollbacks.");
        }

        /**
         *
         * Close a Connection object. Does nothing, really.
         * @see java.sql.Connection#close
         * @exception SQLException is never thrown
         *
         */
        public void close()
        {//throws SQLException {
        }

        /**
         *
         * Returns the status of the Connection.
         * @see java.sql.Connection#isClosed
         * @exception SQLException is never thrown
         * @return true if the connection is closed, false otherwise
         *
         */
        public bool isClosed()
        {//throws SQLException {
            return (tsql == null);
        }

        internal TinySQL getTinySqlHandle()
        {
            return tsql;
        }

        /**
         *
         * This method would like to retrieve some DatabaseMetaData, but it
         * is presently only supported for dBase access
         * @see java.sql.Connection#getMetData
         * @exception SQLException is never thrown
         * @return a DatabaseMetaData object - someday
         *
         */
        public virtual java.sql.DatabaseMetaData getMetaData()
        {//throws SQLException {
            java.lang.SystemJ.outJ.println("******con.getMetaData NOT IMPLEMENTED******");
            return null;
        }

        /**
         * Puts the database in read-only mode... not! This throws an
         * exception whenever it is called. tinySQL does not support
         * a read-only mode, and it might be dangerous to let a program
         * think it's in that mode.
         * @see java.sql.Connection#setReadOnly
         * @param b meaningless
         */
        public void setReadOnly(bool b)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not have a read-only mode.");
        }

        /**
         *
         * Returns true if the database is in read-only mode. It always
         * returns false.
         * @see java.sql.Connection#isReadOnly
         * @return the false will be with you... always
         *
         */
        public bool isReadOnly()
        {//throws SQLException {
            return false;
        }

        /**
         *
         * Sets the current catalog within the database. This is not 
         * supported by tinySQL, but we'll set the catalog String anyway.
         * @see java.sql.Connection#setCatalog
         * @param str the catalog
         *
         */
        public void setCatalog(String str)
        {//throws SQLException {
            catalog = str;
        }

        /**
         *
         * Returns the current catalog. This has no significance in tinySQL
         * @see java.sql.Connection#getCatalog
         * @return the catalog name
         *
         */
        public String getCatalog()
        {//throws SQLException {
            return catalog;
        }

        /**
         *
         * Sets the transaction isolation level, which has no meaning in tinySQL.
         * We'll set the isolation level value anyhow, just to keep it happy.
         * @see java.sql.Connection#setTransactionIsolation
         * @param x the isolation level
         *
         */
        public void setTransactionIsolation(int x)
        {//throws SQLException {
            isolation = x;
        }

        /**
         *
         * Returns the isolation level. This is not significant for tinySQL
         * @see java.sql.Connection#getTransactionIsolation
         * @return the transaction isolation level
         *
         */
        public int getTransactionIsolation()
        {//throws SQLException {
            return isolation;
        }

        /**
         *
         * Disables autoclosing of connections and result sets. This is 
         * not supported by tinySQL.
         * @see java.sql.Connection#disableAutoClose
         *
         */
        public void disableAutoClose()
        {//throws SQLException {
        }

        /**
         *
         * Returns a chain of warnings for the current connection; this
         * is not supported by tinySQL.
         * @see java.sql.Connection#getWarnings
         * @return the chain of warnings for this connection
         *
         */
        public java.sql.SQLWarning getWarnings()
        {//throws SQLException {
            return null;
        }

        /**
         *
         * Clears the non-existant warning chain.
         * @see java.sql.Connection#clearWarnings
         *
         */
        public void clearWarnings()
        {//throws SQLException {
        }

        /**
         *
         * Execute a tinySQL Statement
         * @param sql the statement to be executed
         * @return tsResultSet containing the results of the SQL statement
         *
         */
        public TsResultSet executetinySQL(TinySQLStatement sql) //throws SQLException 
        {
            TsResultSet result;

            // try to execute the SQL
            //
            try
            {
                result = tsql.sqlexec(sql);
            }
            catch (TinySQLException e)
            {
                if (debug) e.printStackTrace();
                throw new java.sql.SQLException("Exception: " + e.getMessage());
            }
            return result;
        }
        public TsResultSet executetinySQL(TinySQLPreparedStatement psql) //throws SQLException 
        {
            TsResultSet result;

            // try to execute the SQL
            //
            try
            {
                result = tsql.sqlexec(psql);
            }
            catch (TinySQLException e)
            {
                if (debug) e.printStackTrace();
                throw new java.sql.SQLException("Exception: " + e.getMessage());
            }
            return result;
        }

        /**
         *
         * Execute a tinySQL Statement
         * @param sql the statement to be executed
         * @return either the row count for INSERT, UPDATE or DELETE or 0 for SQL statements that return nothing
         *
         */
        public int executetinyUpdate(TinySQLStatement sql)
        {//throws SQLException {

            // the result set
            //
            TsResultSet result;

            // try to execute the SQL
            //
            try
            {
                result = tsql.sqlexec(sql);
            }
            catch (TinySQLException e)
            {
                if (debug) e.printStackTrace();
                throw new java.sql.SQLException("Exception: " + e.getMessage());
            }
            return 0;
        }
        public int executetinyUpdate(TinySQLPreparedStatement psql)
        {//throws SQLException {

            // the result set
            //
            TsResultSet result;

            // try to execute the SQL
            //
            try
            {
                result = tsql.sqlexec(psql);
            }
            catch (TinySQLException e)
            {
                if (debug) e.printStackTrace();
                throw new java.sql.SQLException("Exception: " + e.getMessage());
            }
            return 0;
        }

        public bool getAutoCommit()
        {
            return true;
        }

        public void setAutoClose(bool l)
        {
        }

        public bool getAutoClose()
        {
            return false;
        }


        /**
         *
         * creates a new tinySQL object and returns it. Well, not really,
         * since tinySQL is an abstract class. When you subclass tinySQLConnection,
         * you will need to include this method, and return some subclass
         * of tinySQL.
         *
         */
        public abstract TinySQL get_tinySQL();

        //--------------------------JDBC 2.0-----------------------------

        /**
         * JDBC 2.0
         *
             * Creates a <code>Statement</code> object that will generate
             * <code>ResultSet</code> objects with the given type and concurrency.
         * This method is the same as the <code>createStatement</code> method
             * above, but it allows the default result set
         * type and result set concurrency type to be overridden.
         *
         * @param resultSetType a result set type; see ResultSet.TYPE_XXX
         * @param resultSetConcurrency a concurrency type; see ResultSet.CONCUR_XXX
         * @return a new Statement object 
         * @exception SQLException if a database access error occurs
         */
        public java.sql.Statement createStatement(int resultSetType, int resultSetConcurrency)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support createStatement with concurrency.");
        }
        public java.sql.Statement createStatement(int resultSetType, int resultSetConcurrency, int resultSetHoldablity)
        {
            throw new java.sql.SQLException("tinySQL does not support createStatement with concurrency.");
        }

        /**
         * JDBC 2.0
         *
             * Creates a <code>PreparedStatement</code> object that will generate
             * <code>ResultSet</code> objects with the given type and concurrency.
         * This method is the same as the <code>prepareStatement</code> method
             * above, but it allows the default result set
         * type and result set concurrency type to be overridden.
         *
         * @param resultSetType a result set type; see ResultSet.TYPE_XXX
         * @param resultSetConcurrency a concurrency type; see ResultSet.CONCUR_XXX
         * @return a new PreparedStatement object containing the
         * pre-compiled SQL statement 
         * @exception SQLException if a database access error occurs
         */
        public java.sql.PreparedStatement prepareStatement(String sql, int resultSetType,
                                           int resultSetConcurrency)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support preparedStatement with concurrency.");
        }

        /**
         * JDBC 2.0
         *
             * Creates a <code>CallableStatement</code> object that will generate
             * <code>ResultSet</code> objects with the given type and concurrency.
         * This method is the same as the <code>prepareCall</code> method
             * above, but it allows the default result set
         * type and result set concurrency type to be overridden.
         *
         * @param resultSetType a result set type; see ResultSet.TYPE_XXX
         * @param resultSetConcurrency a concurrency type; see ResultSet.CONCUR_XXX
         * @return a new CallableStatement object containing the
         * pre-compiled SQL statement 
         * @exception SQLException if a database access error occurs
         */
        public java.sql.CallableStatement prepareCall(String sql, int resultSetType,
                                     int resultSetConcurrency)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support prepareCall with concurrency.");
        }

        /**
         * JDBC 2.0
         *
         * Gets the type map object associated with this connection.
         * Unless the application has added an entry to the type map,
             * the map returned will be empty.
             *
             * @return the <code>java.util.Map</code> object associated 
             *         with this <code>Connection</code> object
         */
        public java.util.Map<String, java.lang.Class> getTypeMap()
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support getTypeMap.");
        }

        /**
         * JDBC 2.0
         *
         * Installs the given type map as the type map for
         * this connection.  The type map will be used for the
             * custom mapping of SQL structured types and distinct types.
             *
             * @param the <code>java.util.Map</code> object to install
             *        as the replacement for this <code>Connection</code>
             *        object's default type map
         */
        public void setTypeMap(java.util.Map<Object, Object> map)
        {//throws SQLException {
            throw new java.sql.SQLException("tinySQL does not support setTypeMap.");
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
        public java.sql.Struct createStruct(String typeName, Object[] attributes)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.Array createArrayOf(String typeName, Object[] elements)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        private java.util.Properties clientInfo = new java.util.Properties();
        public java.util.Properties getClientInfo(){
            return this.clientInfo;
        }
        public String getClientInfo(String name)
        {
            if (isClosed()) throw new TinySQLException();
            return null == this.clientInfo.get(name) ? null : 0 == this.clientInfo.get(name).length() ? null : this.clientInfo.get(name);
        }
        public void setClientInfo(String name, String value)
        {
            if (value == null && name != null)
            {
                this.clientInfo.remove(name);
            }
            else
            {
                if ("ApplicationName".equals(name))
                {
                    this.clientInfo.put(name, value);
                }
                else if ("ClientUser".equals(name))
                {
                    this.clientInfo.put(name, value);
                }
                else if ("ClientHostname".equals(name))
                {
                    this.clientInfo.put(name, value);
                }

                else
                {
                    throw new java.sql.SQLClientInfoException();
                }
            }
        }
        public void setClientInfo(java.util.Properties props)
        {
            java.util.HashMap<String, java.sql.ClientInfoStatus> errors = new java.util.HashMap<String,java.sql.ClientInfoStatus>();
            java.util.Enumeration<String> names = props.keys();
            String name = null;
            while (names.hasMoreElements()) {
                try
                {
                    name = names.nextElement();
                    this.setClientInfo(name, props.get(name));
                }
                catch (java.sql.SQLClientInfoException) {
                    errors.put(name, java.sql.ClientInfoStatus.REASON_UNKNOWN);
                }
            }
            if (0 != errors.size())
            {
                throw new java.sql.SQLClientInfoException(errors);
            }
        }
        public bool isValid(int timeout)
        {
            return !this.isClosed();
        }
        public java.sql.SQLXML createSQLXML()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.NClob createNClob()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.Blob createBlob()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.Clob createClob()
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void setTypeMap(java.util.Map<String, java.lang.Class> typeMap)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.Savepoint setSavepoint()
        {
            return this.setSavepoint(null);
        }
        public java.sql.Savepoint setSavepoint(String name)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }

        private int holdability = java.sql.ResultSetConstants.HOLD_CURSORS_OVER_COMMIT;
        public void setHoldability(int value)
        {
            if (java.sql.ResultSetConstants.HOLD_CURSORS_OVER_COMMIT != value) throw new java.sql.SQLFeatureNotSupportedException();
            else
            {
                this.holdability = value;
            }
        }
        public void rollback(java.sql.Savepoint savepoint)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public void releaseSavepoint(java.sql.Savepoint savepoint)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.PreparedStatement prepareStatement(String sql, String[] columnName)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.PreparedStatement prepareStatement(String sql, int resultSetType, int resultSetConcurrency, int resultSetHoldability)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.PreparedStatement prepareStatement(String sql, int[] columnIndex)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.PreparedStatement prepareStatement(String sql, int autoGeneratedKeys)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public java.sql.CallableStatement prepareCall(String sql, int resultSetType, int resultSetConcurrency, int resultSetHoldability)
        {
            throw new java.sql.SQLFeatureNotSupportedException();
        }
        public int getHoldability()
        {
            return this.holdability;
        }
    }
}