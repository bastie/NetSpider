/*
 *
 * Connection class for the textFile/tinySQL
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
 * Copyright 1996, Brian C. Jepson
 *                 (bjepson@ids.net)
 *
 * $Author: davis $
 * $Date: 2004/12/18 21:29:35 $
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

    public class TextFileConnection : TinySQLConnection
    {

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
        public TextFileConnection(String user, String u, java.sql.Driver d)
            //throws SQLException {
            : base(user, u, d)
        {
        }

        /**
         *
         * Returns a new textFile object which is cast to a tinySQL
         * object.
         *
         */
        public override TinySQL get_tinySQL()
        {
            return (TinySQL)new TextFile();
        }

    }
}