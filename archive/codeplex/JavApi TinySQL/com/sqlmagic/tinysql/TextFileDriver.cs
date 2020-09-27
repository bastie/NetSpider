/*
 *
 * textFile/tinySQL JDBC driver
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
 * $Date: 2004/12/18 21:27:36 $
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

    public class TextFileDriver : TinySQLDriver
    {

        /*
         *
         * Instantiate a new textFileDriver(), registering it with
         * the JDBC DriverManager.
         *
         */
        static TextFileDriver()
        {
            try
            {
                java.sql.DriverManager.registerDriver(new TextFileDriver());
            }
            catch (Exception e)
            {
                java.lang.SystemJ.err.println(e.toString());//      e.printStackTrace();
            }
        }

        /**
         *
         * Constructs a new textFileDriver
         *
         */
        public TextFileDriver() :
            base()
        {
        }

        /**
         *
         * returns a new textFileConnection object, which is cast
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

            return (TinySQLConnection)new TextFileConnection(user, url, d);
        }

    }
}