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
     *  This interface allows the DOM user to create a CSS style sheet outside the 
     * context of a document. There is no way to associate the new CSS style  
     * sheet with a document in DOM Level 2. 
     * @since DOM   Level 2
     */
    public interface DOMImplementationCSS : DOMImplementation
    {
        /**
         * Creates a new CSS style sheet.
         * @param title  The advisory title. See also . 
         * @param media  The media associated to the new style sheet. See also . 
         * @return A new CSS style sheet.
         */
        CSSStyleSheet createCSSStyleSheet(String title,
                                                     String media);
    }

}