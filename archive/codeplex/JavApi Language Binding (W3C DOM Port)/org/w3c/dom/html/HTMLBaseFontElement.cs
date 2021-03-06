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
     * Base font. See the BASEFONT element definition in HTML 4.0. This element is 
     * deprecated in HTML 4.0.
     */
    public interface HTMLBaseFontElement : HTMLElement
    {
        /**
         * Font color. See the color attribute definition in HTML 4.0. This 
         * attribute is deprecated in HTML 4.0.
         */
        String getColor();
        void setColor(String color);
        /**
         * Font face identifier. See the face attribute definition in HTML 4.0. This 
         * attribute is deprecated in HTML 4.0.
         */
        String getFace();
        void setFace(String face);
        /**
         * Font size. See the size attribute definition in HTML 4.0. This attribute 
         * is deprecated in HTML 4.0.
         */
        String getSize();
        void setSize(String size);
    }

}