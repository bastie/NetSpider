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
     * Embedded image. See the IMG element definition in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLImageElement : HTMLElement
    {
        /**
         * The name of the element (for backwards compatibility). 
         */
        String getName();
        /**
         * The name of the element (for backwards compatibility). 
         */
        void setName(String name);

        /**
         * Aligns this object (vertically or horizontally) with respect to its 
         * surrounding text. See the align attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        String getAlign();
        /**
         * Aligns this object (vertically or horizontally) with respect to its 
         * surrounding text. See the align attribute definition in HTML 4.01. 
         * This attribute is deprecated in HTML 4.01.
         */
        void setAlign(String align);

        /**
         * Alternate text for user agents not rendering the normal content of this 
         * element. See the alt attribute definition in HTML 4.01.
         */
        String getAlt();
        /**
         * Alternate text for user agents not rendering the normal content of this 
         * element. See the alt attribute definition in HTML 4.01.
         */
        void setAlt(String alt);

        /**
         * Width of border around image. See the  border attribute definition in 
         * HTML 4.01. This attribute is deprecated in HTML 4.01. Note that the 
         * type of this attribute was <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>]
         * .
         */
        String getBorder();
        /**
         * Width of border around image. See the  border attribute definition in 
         * HTML 4.01. This attribute is deprecated in HTML 4.01. Note that the 
         * type of this attribute was <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>]
         * .
         */
        void setBorder(String border);

        /**
         * Height of the image in pixels. See the height attribute definition in 
         * HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        int getHeight();
        /**
         * Height of the image in pixels. See the height attribute definition in 
         * HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        void setHeight(int height);

        /**
         * Horizontal space to the left and right of this image in pixels. See the 
         * hspace attribute definition in HTML 4.01. This attribute is 
         * deprecated in HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        int getHspace();
        /**
         * Horizontal space to the left and right of this image in pixels. See the 
         * hspace attribute definition in HTML 4.01. This attribute is 
         * deprecated in HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        void setHspace(int hspace);

        /**
         * Use server-side image map. See the ismap attribute definition in HTML 
         * 4.01.
         */
        bool getIsMap();
        /**
         * Use server-side image map. See the ismap attribute definition in HTML 
         * 4.01.
         */
        void setIsMap(bool isMap);

        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] designating a long description of this image or frame. See the 
         * longdesc attribute definition in HTML 4.01.
         */
        String getLongDesc();
        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] designating a long description of this image or frame. See the 
         * longdesc attribute definition in HTML 4.01.
         */
        void setLongDesc(String longDesc);

        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] designating the source of this image. See the src attribute 
         * definition in HTML 4.01.
         */
        String getSrc();
        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] designating the source of this image. See the src attribute 
         * definition in HTML 4.01.
         */
        void setSrc(String src);

        /**
         * Use client-side image map. See the usemap attribute definition in HTML 
         * 4.01.
         */
        String getUseMap();
        /**
         * Use client-side image map. See the usemap attribute definition in HTML 
         * 4.01.
         */
        void setUseMap(String useMap);

        /**
         * Vertical space above and below this image in pixels. See the vspace 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01. Note that the type of this attribute was "DOMString" in 
         * DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        int getVspace();
        /**
         * Vertical space above and below this image in pixels. See the vspace 
         * attribute definition in HTML 4.01. This attribute is deprecated in 
         * HTML 4.01. Note that the type of this attribute was "DOMString" in 
         * DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        void setVspace(int vspace);

        /**
         * The width of the image in pixels. See the width attribute definition in 
         * HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        int getWidth();
        /**
         * The width of the image in pixels. See the width attribute definition in 
         * HTML 4.01. Note that the type of this attribute was 
         * <code>DOMString</code> in DOM Level 1 HTML [<a href='http://www.w3.org/TR/1998/REC-DOM-Level-1-19981001'>DOM Level 1</a>].
         * @version DOM Level 2
         */
        void setWidth(int width);

    }
}