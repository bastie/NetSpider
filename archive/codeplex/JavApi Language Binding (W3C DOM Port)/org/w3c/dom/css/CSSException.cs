/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;
using java = biz.ritter.javapi;

namespace org.w3c.dom.css
{

    /**
     *  This exception is raised when a specific CSS operation is impossible to 
     * perform. 
     */
    public abstract class CSSException : java.lang.RuntimeException
    {
        public CSSException(short code, String message)
            : base(message)
        {
            this.code = code;
        }
        short code;
        // CSSExceptionCode
        public const short SYNTAX_ERR = 0;
        public const short INVALID_MODIFICATION_ERR = 1;

    }

}