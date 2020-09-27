/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.stylesheets
{

    /**
     *  The <code>StyleSheet</code> interface is the abstract base interface for 
     * any type of style sheet. It represents a single style sheet associated 
     * with a structured document. In HTML, the StyleSheet interface represents 
     * either an external style sheet, included via the HTML  LINK element, or an 
     * inline  STYLE element. In XML, this interface represents an external style 
     * sheet, included via a style sheet processing instruction. 
     * @since DOM Level 2
     */
    public interface StyleSheet
    {
        /**
         *  This specifies the style sheet language for this style sheet. The style 
         * sheet language is specified as a content type (e.g. "text/css").  The 
         * content type is often specified in the <code>ownerNode</code>.  A list 
         * of registered content types can be found at 
         * ftp://ftp.isi.edu/in-notes/iana/assignments/media-types/. Also see the 
         * type attribute definition for the <code>LINK</code> element in HTML 4.0, 
         * and the type pseudo-attribute for the XML style sheet processing 
         * instruction. 
         */
        String getType();
        /**
         *  <code>false</code> if the style sheet is applied to the document. 
         * <code>true</code> if it is not. Modifying this attribute may cause a new 
         * resolution of style for the document. For the , the medium has a higher 
         * priority than the <code>disabled</code> attribute. So, if the media 
         * doesn't apply to the current user agent, the <code>disabled</code> 
         * attribute is ignored. 
         */
        bool getDisabled();
        void setDisabled(bool disabled);
        /**
         *  The node that associates this style sheet with the document. For HTML, 
         * this may be the corresponding <code>LINK</code> or <code>STYLE</code> 
         * element. For XML, it may be the linking processing instruction. For 
         * style sheets that are included by other style sheets, the value of this 
         * attribute is <code>null</code>. 
         */
        Node getOwnerNode();
        /**
         *  For style sheet languages that support the concept of style sheet 
         * inclusion, this attribute represents the including style sheet, if one 
         * exists. If the style sheet is a top-level style sheet, or the style 
         * sheet language does not support inclusion, the value of this attribute 
         * is <code>null</code>. 
         */
        StyleSheet getParentStyleSheet();
        /**
         *  If the style sheet is a linked style sheet, the value of its attribute 
         * is its location. For inline style sheets, the value of this attribute is 
         * <code>null</code>. See the href attribute definition for the 
         * <code>LINK</code> element in HTML 4.0, and the href pseudo-attribute for 
         * the XML style sheet processing instruction. 
         */
        String getHref();
        /**
         *  The advisory title.  The title is often specified in the 
         * <code>ownerNode</code>.  See the title attribute definition for the 
         * <code>LINK</code> element in HTML 4.0, and the title pseudo-attribute 
         * for the XML style sheet processing instruction. 
         */
        String getTitle();
        /**
         *  The intended destination media for style information.  The media is 
         * often specified in the <code>ownerNode</code>. If no media has been 
         * specified, the <code>MediaList</code> will be empty. See the media 
         * attribute definition for the <code>LINK</code> element in HTML 4.0, and 
         * the media pseudo-attribute for the XML style sheet processing 
         * instruction . 
         */
        MediaList getMedia();
    }

}