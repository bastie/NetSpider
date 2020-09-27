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
     * Script statements. See the SCRIPT element definition in HTML 4.0.
     */
    public interface HTMLScriptElement : HTMLElement
    {
        /**
         * The script content of the element. 
         */
        String getText();
        void setText(String text);
        /**
         * Reserved for future use. 
         */
        String getHtmlFor();
        void setHtmlFor(String htmlFor);
        /**
         * Reserved for future use. 
         */
        String getEvent();
        void setEvent(String e);
        /**
         * The character encoding of the linked resource. See the charset attribute 
         * definition in HTML 4.0.
         */
        String getCharset();
        void setCharset(String charset);
        /**
         * Indicates that the user agent can defer processing of the script.  See 
         * the defer attribute definition in HTML 4.0.
         */
        bool getDefer();
        void setDefer(bool defer);
        /**
         * URI designating an external script. See the src attribute definition in 
         * HTML 4.0.
         */
        String getSrc();
        void setSrc(String src);
        /**
         * The content type of the script language. See the type attribute definition
         *  in HTML 4.0.
         */
        String getType();
        void setType(String type);
    }

}