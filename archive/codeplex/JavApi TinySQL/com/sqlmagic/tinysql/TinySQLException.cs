/*
 *
 * tinySQLException.java
 * An Exception that is thrown when a problem has occurred in tinySQL
 *
 * Copyright 1996, Brian C. Jepson
 *                 (bjepson@ids.net)
 * $Author: davis $
 * $Date: 2004/12/18 21:29:20 $
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
     * @author Thomas Morgner <mgs@sherito.org> tinySQLException is now a subclass of
     * SQLException and can be directly passed to the caller.
     */
    public class TinySQLException : java.sql.SQLException
    {

        /**
         *
         * Constructs a new tinySQLException
         * @param message the exception's message
         *
         */
        public TinySQLException(String message) :
            base(message)
        {
        }

        /**
         *
         * Constructs a new tinySQLException with no message.
         *
         */
        public TinySQLException()
        {
        }

    }
}