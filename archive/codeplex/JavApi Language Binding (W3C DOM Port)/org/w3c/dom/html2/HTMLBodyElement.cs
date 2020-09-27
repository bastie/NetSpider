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
     * The HTML document body. This element is always present in the DOM API, even 
     * if the tags are not present in the source document. See the BODY element 
     * definition in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLBodyElement : HTMLElement
    {
        /**
         * Color of active links (after mouse-button down, but before mouse-button 
         * up). See the alink attribute definition in HTML 4.01. This attribute 
         * is deprecated in HTML 4.01.
         */
        String getALink();
        /**
         * Color of active links (after mouse-button down, but before mouse-button 
         * up). See the alink attribute definition in HTML 4.01. This attribute 
         * is deprecated in HTML 4.01.
         */
        void setALink(String aLink);

        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the background texture tile image. See the background attribute 
         * definition in HTML 4.01. This attribute is deprecated in HTML 4.01.
         */
        String getBackground();
        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the background texture tile image. See the background attribute 
         * definition in HTML 4.01. This attribute is deprecated in HTML 4.01.
         */
        void setBackground(String background);

        /**
         * Document background color. See the bgcolor attribute definition in HTML 
         * 4.01. This attribute is deprecated in HTML 4.01.
         */
        String getBgColor();
        /**
         * Document background color. See the bgcolor attribute definition in HTML 
         * 4.01. This attribute is deprecated in HTML 4.01.
         */
        void setBgColor(String bgColor);

        /**
         * Color of links that are not active and unvisited. See the link 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01.
         */
        String getLink();
        /**
         * Color of links that are not active and unvisited. See the link 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01.
         */
        void setLink(String link);

        /**
         * Document text color. See the text attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        String getText();
        /**
         * Document text color. See the text attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        void setText(String text);

        /**
         * Color of links that have been visited by the user. See the vlink 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01.
         */
        String getVLink();
        /**
         * Color of links that have been visited by the user. See the vlink 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01.
         */
        void setVLink(String vLink);

    }
}