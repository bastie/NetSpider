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
     *  An <code>HTMLOptionsCollection</code> is a list of nodes representing HTML 
     * option element. An individual node may be accessed by either ordinal 
     * index or the node's <code>name</code> or <code>id</code> attributes.  
     * Collections in the HTML DOM are assumed to be live meaning that they are 
     * automatically updated when the underlying document is changed. 
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     * @since DOM Level 2
     */
    public interface HTMLOptionsCollection
    {
        /**
         *  This attribute specifies the length or size of the list. 
         */
        int getLength();
        /**
         *  This attribute specifies the length or size of the list. 
         * @exception DOMException
         *    NOT_SUPPORTED_ERR: if setting the length is not allowed by the 
         *   implementation. 
         */
        void setLength(int length);
        //throws DOMException;

        /**
         *  This method retrieves a node specified by ordinal index. Nodes are 
         * numbered in tree order (depth-first traversal order). 
         * @param index The index of the node to be fetched. The index origin is 
         *   0.
         * @return The <code>Node</code> at the corresponding position upon 
         *   success. A value of <code>null</code> is returned if the index is 
         *   out of range. 
         */
        Node item(int index);

        /**
         * This method retrieves a <code>Node</code> using a name. It first 
         * searches for a <code>Node</code> with a matching <code>id</code> 
         * attribute. If it doesn't find one, it then searches for a 
         * <code>Node</code> with a matching <code>name</code> attribute, but 
         * only on those elements that are allowed a name attribute. This method 
         * is case insensitive in HTML documents and case sensitive in XHTML 
         * documents.
         * @param name The name of the <code>Node</code> to be fetched.
         * @return The <code>Node</code> with a <code>name</code> or 
         *   <code>id</code> attribute whose value corresponds to the specified 
         *   string. Upon failure (e.g., no node with this name exists), returns 
         *   <code>null</code>.
         */
        Node namedItem(String name);

    }
}