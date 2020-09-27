/*
 * Copyright (c) 2003 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de
 * Recherche en Informatique et en Automatique, Keio University). All
 * Rights Reserved. This program is distributed under the W3C's Software
 * Intellectual Property License. This program is distributed in the
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE.
 * See W3C License http://www.w3.org/Consortium/Legal/ for more details.
 */
using System;

namespace org.w3c.dom.html2
{

    /**
     * Base font. See the BASEFONT element definition in HTML 4.01. This element 
     * is deprecated in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLBaseFontElement : HTMLElement
    {
        /**
         * Font color. See the color attribute definition in HTML 4.01. This 
         * attribute is deprecated in HTML 4.01.
         */
        String getColor();
        /**
         * Font color. See the color attribute definition in HTML 4.01. This 
         * attribute is deprecated in HTML 4.01.
         */
        void setColor(String color);

        /**
         * Font face identifier. See the face attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        String getFace();
        /**
         * Font face identifier. See the face attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        void setFace(String face);

        /**
         * Computed font size. See the size attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         * @version DOM Level 2
         */
        int getSize();
        /**
         * Computed font size. See the size attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         * @version DOM Level 2
         */
        void setSize(int size);

    }
}