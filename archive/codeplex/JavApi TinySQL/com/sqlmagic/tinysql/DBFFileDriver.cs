/*
 *
 * dbfFile/tinySQL JDBC driver
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
 * $Date: 2004/12/18 21:32:10 $
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
    */
    public class DBFFileDriver : TinySQLDriver
    {

        /*
         *
         * Instantiate a new dbfFileDriver(), registering it with
         * the JDBC DriverManager.
         *
         */
        static DBFFileDriver()
        {
            try
            {
                java.sql.DriverManager.registerDriver(new DBFFileDriver());
            }
            catch (Exception e)
            {
                java.lang.SystemJ.outJ.println(e.ToString());
                //e.printStackTrace();
            }
        }

        /**
         *
         * Constructs a new dbfFileDriver
         *
         */
        public DBFFileDriver() :
            base()
        {
        }

        /**
         *
         * returns a new dbfFileConnection object, which is cast
         * to a tinySQLConnection object.
         *
         * @exception SQLException when an error occurs
         * @param user the username - currently unused
         * @param url the url to the data source
         * @param d the Driver object.
         *
         */
        public override TinySQLConnection getConnection
                       (String user, String url, java.sql.Driver d)
        {//throws SQLException {
            if (url != (String)null)
            {
                if (url.length() > 13) TinySQLGlobals.readLongNames(url.substring(13));
            }
            return (TinySQLConnection)new DBFFileConnection(user, url, d);
        }

        /**
         *
         * Check to see if the URL is a dbfFile URL. It should start
         * with jdbc:dbfFile in order to qualify.
         *
         * @param url The URL of the database.
         * @return True if this driver can connect to the given URL.
         *
         */
        public override bool acceptsURL(String url)
        {//throws SQLException {

            // make sure the length is at least twelve
            // before bothering with the substring
            // comparison.
            //
            if (url.length() < 12)
            {
                return false;
            }

            // if everything after the jdbc: part is
            // dbfFile, then return true.
            //
            return url.substring(5, 12).equals("dbfFile");

        }

    }
}