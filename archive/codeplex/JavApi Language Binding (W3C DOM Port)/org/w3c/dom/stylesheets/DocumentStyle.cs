/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.stylesheets
{

    /**
     *  The <code>DocumentStyle</code> interface provides a mechanism by which the 
     * style sheets embedded a document can be retrieved. The expectation is that 
     * an instance of the <code>DocumentStyle</code> interface can be obtained by 
     * using binding-specific casting methods on an instance of the 
     * <code>Document</code> interface. 
     * @since DOM Level 2
     */
    public interface DocumentStyle
    {
        /**
         *  A list containing all the style sheets explicitly linked into or 
         * embedded in a document.  For HTML documents, this includes external 
         * style sheets, included via the HTML  LINK element, and inline  STYLE 
         * elements. In XML, this includes external style sheets, included via 
         * style sheet processing instructions. 
         */
        StyleSheetList getStyleSheets();
    }
}