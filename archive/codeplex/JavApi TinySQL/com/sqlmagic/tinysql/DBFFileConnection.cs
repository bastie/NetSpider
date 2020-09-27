/*
 *
 * Connection class for the dbfFile/tinySQL
 * JDBC driver
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
 * Copyright 1996 John Wiley & Sons, Inc. 
 * See the COPYING file for redistribution details.
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:30:05 $
 * $Revision: 1.1 $
 */
using System;
using java = biz.ritter.javapi;

namespace com.sqlmagic.tinysql
{


    /**
    dBase read/write access <br> 
    @author Brian Jepson <bjepson@home.com>
    @author Marcel Ruff <ruff@swand.lake.de> Added write access to dBase and JDK 2 support
    */
    public class DBFFileConnection : TinySQLConnection
    {

        private DBFFileDatabaseMetaData myMetaData = null;

        /**
         *
         * Constructs a new JDBC Connection object.
         *
         * @exception SQLException in case of an error
         * @param user the user name - not currently used
         * @param u the url to the data source
         * @param d the Driver object
         *
         */
        public DBFFileConnection(String user, String u, java.sql.Driver d)
            ://throws SQLException {
       base(user, u, d)
        {
        }

        /**
         *
         * Returns a new dbfFile object which is cast to a tinySQL
         * object.
         *
         */
        public override TinySQL get_tinySQL()
        {

            // if there's a data directory, it will
            // be everything after the jdbc:dbfFile:
            //
            if (url.length() > 13)
            {
                String dataDir = url.substring(13);
                return (TinySQL)new DBFFile(dataDir);
            }

            // if there was no data directory specified in the
            // url, then just use the default constructor
            //
            return (TinySQL)new DBFFile();

        }

        /**
         * This method retrieves DatabaseMetaData
         * @see java.sql.Connection#getMetData
         * @exception SQLException
         * @return a DatabaseMetaData object (conforming to JDK 2)
         *
         */
        public override java.sql.DatabaseMetaData getMetaData()
        {//throws SQLException {
            if (myMetaData == null)
                myMetaData = new DBFFileDatabaseMetaData(this);
            return (java.sql.DatabaseMetaData)myMetaData;
        }
    }
}