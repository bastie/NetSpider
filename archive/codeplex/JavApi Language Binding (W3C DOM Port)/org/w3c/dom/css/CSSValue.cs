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
     *  The <code>CSSValue</code> interface represents a simple or a complex value.
     *  
     * @since DOM Level 2
     */
    public interface CSSValue
    {
        // UnitTypes

        /**
         *  A string representation of the current value. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the specified CSS string value has a syntax 
         *   error and is unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this value is readonly.
         */
        String getCssText();
        void setCssText(String cssText);//                                 throws CSSException, DOMException;
        /**
         * A code defining the type of the value as defined above.
         */
        short getValueType();
    }

    public sealed class CSSValueConstants
    {
        public const short CSS_INHERIT = 0;
        public const short CSS_PRIMITIVE_VALUE = 1;
        public const short CSS_VALUE_LIST = 2;
        public const short CSS_CUSTOM = 3;

    }
}