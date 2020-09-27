/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;
using java = biz.ritter.javapi;

namespace org.w3c.dom.range
{

    /**
     * The Range object needs additional exception codes to thosein DOM Level 1. 
     * These codes will need to be consolidated withother exception codes added 
     * to DOM Level 2.
     */
    public abstract class RangeException : java.lang.RuntimeException
    {
        public RangeException(short code, String message)
            : base(message)
        {
            this.code = code;
        }
        public short code;
        // RangeExceptionCode
        public const short BAD_ENDPOINTS_ERR = 201;
        public const short INVALID_NODE_TYPE_ERR = 202;
        public const short NULL_NODE_ERR = 203;

    }
}