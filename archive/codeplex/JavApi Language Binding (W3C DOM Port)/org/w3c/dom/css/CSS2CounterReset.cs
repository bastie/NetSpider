/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.css
{

    /**
     * The <code>CSS2CounterReset</code> interface represents a simple value for 
     * the counter-reset CSS Level 2 property.
     * @since DOM Level   2
     */
    public interface CSS2CounterReset : CSSValue
    {
        /**
         * The element name.
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the specified identifier has a syntax error and 
         *   is unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this identifier is readonly.
         */
        String getIdentifier();
        void setIdentifier(String identifier)
                                                   ;//throws CSSException, DOMException;
        /**
         * The reset (default value is 0).
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this identifier is readonly.
         */
        short getReset();
        void setReset(short reset)
                                                   ;//throws DOMException;
    }

}