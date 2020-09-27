/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.html
{

    /**
     * The document title. See the TITLE element definition in HTML 4.0.
     */
    public interface HTMLTitleElement : HTMLElement
    {
        /**
         * The specified title as a string. 
         */
        String getText();
        void setText(String text);
    }

}