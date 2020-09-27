/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;
using org.w3c.dom.stylesheets;

namespace org.w3c.dom.css
{

    /**
     *  The <code>CSSImportRule</code> interface represents a @import rule within 
     * a CSS style sheet. The <code>@import</code> rule is used to import style 
     * rules from other style sheets. 
     * @since DOM Level 2
     */
    public interface CSSImportRule : CSSRule
    {
        /**
         *  The location of the style sheet to be imported. The attribute will not 
         * contain the <code>"url(...)"</code> specifier around the URI. 
         */
        String getHref();
        /**
         *  A list of media types for which this style sheet may be used. 
         */
        MediaList getMedia();
        /**
         * The style sheet referred to by this rule, if it has been loaded. The 
         * value of this attribute is <code>null</code> if the style sheet has not 
         * yet been loaded or if it will not be loaded (e.g. if the style sheet is 
         * for a media type not supported by the user agent). 
         */
        CSSStyleSheet getStyleSheet();
    }

}