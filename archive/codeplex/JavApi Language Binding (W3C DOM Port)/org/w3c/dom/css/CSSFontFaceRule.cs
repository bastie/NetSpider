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
     *  The <code>CSSFontFaceRule</code> interface represents a @font-face  rule 
     * in a CSS style sheet. The <code>@font-face</code> rule  is used to hold a 
     * set of font descriptions. 
     * @since DOM Level 2
     */
    public interface CSSFontFaceRule : CSSRule
    {
        /**
         *  The declaration-block of this rule. 
         */
        CSSStyleDeclaration getStyle();
    }

}