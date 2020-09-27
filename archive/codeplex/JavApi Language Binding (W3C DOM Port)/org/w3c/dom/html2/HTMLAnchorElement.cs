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
     * The anchor element. See the A element definition in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLAnchorElement : HTMLElement
    {
        /**
         * A single character access key to give access to the form control. See 
         * the accesskey attribute definition in HTML 4.01.
         */
        String getAccessKey();
        /**
         * A single character access key to give access to the form control. See 
         * the accesskey attribute definition in HTML 4.01.
         */
        void setAccessKey(String accessKey);

        /**
         * The character encoding of the linked resource. See the charset 
         * attribute definition in HTML 4.01.
         */
        String getCharset();
        /**
         * The character encoding of the linked resource. See the charset 
         * attribute definition in HTML 4.01.
         */
        void setCharset(String charset);

        /**
         * Comma-separated list of lengths, defining an active region geometry. 
         * See also <code>shape</code> for the shape of the region. See the 
         * coords attribute definition in HTML 4.01.
         */
        String getCoords();
        /**
         * Comma-separated list of lengths, defining an active region geometry. 
         * See also <code>shape</code> for the shape of the region. See the 
         * coords attribute definition in HTML 4.01.
         */
        void setCoords(String coords);

        /**
         * The absolute URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the linked resource. See the href attribute 
         * definition in HTML 4.01.
         */
        String getHref();
        /**
         * The absolute URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the linked resource. See the href attribute 
         * definition in HTML 4.01.
         */
        void setHref(String href);

        /**
         * Language code of the linked resource. See the hreflang attribute 
         * definition in HTML 4.01.
         */
        String getHreflang();
        /**
         * Language code of the linked resource. See the hreflang attribute 
         * definition in HTML 4.01.
         */
        void setHreflang(String hreflang);

        /**
         * Anchor name. See the name attribute definition in HTML 4.01.
         */
        String getName();
        /**
         * Anchor name. See the name attribute definition in HTML 4.01.
         */
        void setName(String name);

        /**
         * Forward link type. See the rel attribute definition in HTML 4.01.
         */
        String getRel();
        /**
         * Forward link type. See the rel attribute definition in HTML 4.01.
         */
        void setRel(String rel);

        /**
         * Reverse link type. See the rev attribute definition in HTML 4.01.
         */
        String getRev();
        /**
         * Reverse link type. See the rev attribute definition in HTML 4.01.
         */
        void setRev(String rev);

        /**
         * The shape of the active area. The coordinates are given by 
         * <code>coords</code>. See the shape attribute definition in HTML 4.01.
         */
        String getShape();
        /**
         * The shape of the active area. The coordinates are given by 
         * <code>coords</code>. See the shape attribute definition in HTML 4.01.
         */
        void setShape(String shape);

        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in HTML 4.01.
         */
        int getTabIndex();
        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in HTML 4.01.
         */
        void setTabIndex(int tabIndex);

        /**
         * Frame to render the resource in. See the target attribute definition in 
         * HTML 4.01.
         */
        String getTarget();
        /**
         * Frame to render the resource in. See the target attribute definition in 
         * HTML 4.01.
         */
        void setTarget(String target);

        /**
         * Advisory content type. See the type attribute definition in HTML 4.01.
         */
        String getType();
        /**
         * Advisory content type. See the type attribute definition in HTML 4.01.
         */
        void setType(String type);

        /**
         * Removes keyboard focus from this element.
         */
        void blur();

        /**
         * Gives keyboard focus to this element.
         */
        void focus();

    }
}