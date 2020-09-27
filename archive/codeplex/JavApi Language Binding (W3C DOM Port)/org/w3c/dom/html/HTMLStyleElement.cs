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
     *  Style information. See the STYLE element definition in HTML 4.0, the  
     * module and the <code>HTMLStyleElementStyle</code> interface in the  module.
     *  
     */
    public interface HTMLStyleElement : HTMLElement
    {
        /**
         * Enables/disables the style sheet. 
         */
        bool getDisabled();
        void setDisabled(bool disabled);
        /**
         * Designed for use with one or more target media. See the media attribute 
         * definition in HTML 4.0.
         */
        String getMedia();
        void setMedia(String media);
        /**
         * The style sheet language (Internet media type). See the type attribute 
         * definition in HTML 4.0.
         */
        String getType();
        void setType(String type);
    }

}