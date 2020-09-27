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
     * Create a horizontal rule. See the HR element definition in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLHRElement : HTMLElement
    {
        /**
         * Align the rule on the page. See the align attribute definition in HTML 
         * 4.01. This attribute is deprecated in HTML 4.01.
         */
        String getAlign();
        /**
         * Align the rule on the page. See the align attribute definition in HTML 
         * 4.01. This attribute is deprecated in HTML 4.01.
         */
        void setAlign(String align);

        /**
         * Indicates to the user agent that there should be no shading in the 
         * rendering of this element. See the noshade attribute definition in 
         * HTML 4.01. This attribute is deprecated in HTML 4.01.
         */
        bool getNoShade();
        /**
         * Indicates to the user agent that there should be no shading in the 
         * rendering of this element. See the noshade attribute definition in 
         * HTML 4.01. This attribute is deprecated in HTML 4.01.
         */
        void setNoShade(bool noShade);

        /**
         * The height of the rule. See the size attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        String getSize();
        /**
         * The height of the rule. See the size attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        void setSize(String size);

        /**
         * The width of the rule. See the width attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        String getWidth();
        /**
         * The width of the rule. See the width attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        void setWidth(String width);

    }
}