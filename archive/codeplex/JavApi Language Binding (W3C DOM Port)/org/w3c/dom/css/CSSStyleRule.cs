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
     *  The <code>CSSStyleRule</code> interface represents a single rule set in a 
     * CSS style sheet. 
     * @since DOM Level 2
     */
    public interface CSSStyleRule : CSSRule
    {
        /**
         *  The textual representation of the selector for the rule set. The 
         * implementation may have stripped out insignificant whitespace while 
         * parsing the selector. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the specified CSS string value has a syntax 
         *   error  and is unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this rule is readonly.
         */
        String getSelectorText();
        void setSelectorText(String selectorText)
                                     ;//throws CSSException, DOMException;
        /**
         *  The declaration-block of this rule set. 
         */
        CSSStyleDeclaration getStyle();
    }

}