/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.range
{

    /**
     */
    public interface Range
    {
        /**
         * Node within which the range begins 
         */
        Node getStartContainer();
        /**
         * Offset within the starting node of the range. 
         */
        int getStartOffset();
        /**
         * Node within which the range ends 
         */
        Node getEndContainer();
        /**
         * Offset within the ending node of the range. 
         */
        int getEndOffset();
        /**
         * TRUE if the range is collapsed 
         */
        bool getIsCollapsed();
        /**
         * The common ancestor container of the range's two end-points.
         */
        Node getCommonAncestorContainer();
        /**
         * Sets the attributes describing the start of therange. 
         * @param refNode The <code>refNode</code> value. Thisparameter must be 
         *   different from <code>null</code>.
         * @param offset The <code>startOffset</code> value. 
         * @exception RangeException
         *   NULL_NODE_ERR: Raised if <code>refNode</code> is <code>null</code>.
         *   <br>INVALID_NODE_TYPE_ERR: Raised if<code>refNode</code> or an 
         *   ancestor of <code>refNode</code> is anAttr, Entity, Notation, or 
         *   DocumentType node.If an offset is out-of-bounds, shouldit just be 
         *   fixed up or should an exception be raised.
         */
        void setStart(Node refNode,
                                           int offset)
                                           ;//throws RangeException;
        /**
         * Sets the attributes describing the end of a range.
         * @param refNode The <code>refNode</code> value. Thisparameter must be 
         *   different from <code>null</code>.
         * @param offset The <code>endOffset</code> value. 
         * @exception RangeException
         *   NULL_NODE_ERR: Raised if <code>refNode</code> is <code>null</code>.
         *   <br>INVALID_NODE_TYPE_ERR: Raised if<code>refNode</code> or an 
         *   ancestor of <code>refNode</code> is anAttr, Entity, Notation, or 
         *   DocumentType node.
         */
        void setEnd(Node refNode,
                                         int offset)
                                         ;//throws RangeException;
        /**
         * Sets the start position to be before a node
         * @param refNode Range starts before <code>refNode</code> 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if an ancestorof <code>refNode</code> is 
         *   an Attr, Entity,Notation, or DocumentType node or if 
         *   <code>refNode</code> is a Document,DocumentFragment, Attr, Entity, or 
         *   Notation node.
         */
        void setStartBefore(Node refNode)
                                                 ;//throws RangeException;
        /**
         * Sets the start position to be after a node
         * @param refNode Range starts after <code>refNode</code> 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if an ancestorof <code>refNode</code> is 
         *   an Attr, Entity,Notation, or DocumentType node or if 
         *   <code>refNode</code> is a Document,DocumentFragment, Attr, Entity, or 
         *   Notation node.
         */
        void setStartAfter(Node refNode)
                                                ;//throws RangeException;
        /**
         * Sets the end position to be before a node. 
         * @param refNode Range ends before <code>refNode</code> 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if an ancestorof <code>refNode</code> is 
         *   an Attr, Entity,Notation, or DocumentType node or if 
         *   <code>refNode</code> is a Document,DocumentFragment, Attr, Entity, or 
         *   Notation node.
         */
        void setEndBefore(Node refNode)
                                               ;//throws RangeException;
        /**
         * Sets the end of a range to be after a node 
         * @param refNode Range ends after <code>refNode</code>. 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if an ancestorof <code>refNode</code> is 
         *   an Attr, Entity,Notation or DocumentType node or if 
         *   <code>refNode</code> is a Document,DocumentFragment, Attr, Entity, or 
         *   Notation node.
         */
        void setEndAfter(Node refNode)
                                              ;//throws RangeException;
        /**
         * Collapse a range onto one of its end-points 
         * @param toStart If TRUE, collapses the Range onto its start;if FALSE, 
         *   collapses it onto its end. 
         */
        void collapse(bool toStart);
        /**
         * Select a node and its contents 
         * @param refNode The node to select. 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if an ancestorof <code>refNode</code> is 
         *   an Attr, Entity,Notation or DocumentType node or if 
         *   <code>refNode</code> is a Document,DocumentFragment, Attr, Entity, or 
         *   Notation node.
         */
        void selectNode(Node refNode)
                                             ;//throws RangeException;
        /**
         * Select the contents within a node 
         * @param refNode Node to select from 
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if<code>refNode</code> or an ancestor of 
         *   <code>refNode</code> is anAttr, Entity, Notation or DocumentType node.
         */
        void selectNodeContents(Node refNode)
                                                     ;//throws RangeException;


        /**
         * Compare the end-points of two ranges in a document.
         * @param how 
         * @param sourceRange 
         * @return  -1, 0 or 1 depending on whether the correspondingend-point of 
         *   the Range is before, equal to, or after thecorresponding end-point of 
         *   <code>sourceRange</code>. 
         * @exception DOMException
         *   WRONG_DOCUMENT_ERR: Raised if the two Rangesare not in the same 
         *   document or document fragment.
         */
        short compareEndPoints(int how,
                                                   Range sourceRange)
                                                   ;//throws DOMException;
        /**
         * Removes the contents of a range from the containingdocument or document 
         * fragment without returning a reference to theremoved content.  
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if anyportion of the content of 
         *   the range is read-only or anyof the nodes that contain any of the 
         *   content of the range areread-only.
         */
        void deleteContents()
                                                 ;//throws DOMException;
        /**
         * Moves the contents of a range from the containingdocument or document 
         * fragment to a new DocumentFragment. 
         * @return A DocumentFragment containing the extractedcontents. 
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if anyportion of the content of 
         *   the range is read-only or anyof the nodes which contain any of the 
         *   content of the range are read-only.
         *   <br>HIERARCHY_REQUEST_ERR: Raised if aDocumentType node would be 
         *   extracted into the newDocumentFragment.
         */
        DocumentFragment extractContents()
                                                  ;//throws DOMException;
        /**
         * Duplicates the contents of a range 
         * @return A DocumentFragment containing contents equivalentto those of this 
         *   range. 
         * @exception DOMException
         *   HIERARCHY_REQUEST_ERR: Raised if aDocumentType node would be extracted 
         *   into the newDocumentFragment.
         */
        DocumentFragment cloneContents()
                                                ;//throws DOMException;
        /**
         * Inserts a node into the document or document fragmentat the start of the 
         * range. 
         * @param newNode The node to insert at the start of therange 
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if an ancestor container of the 
         *   start of the range is read-only.
         *   <br>WRONG_DOCUMENT_ERR: Raised if<code>newNode</code> and the container
         *    of the start of the Range were not created from the same document.
         *   <br>HIERARCHY_REQUEST_ERR: Raised if the container of the start of the 
         *   Range is of a type that does not allow children ofthe type of 
         *   <code>newNode</code> or if <code>newNode</code> is an ancestor of the
         *   container.
         * @exception RangeException
         *   INVALID_NODE_TYPE_ERR: Raised if<code>node</code> is an Attr, Entity, 
         *   Notation,DocumentFragment, or Document node.
         */
        void insertNode(Node newNode)
                                             ;//throws DOMException, RangeException;
        /**
         * Reparents the contents of the range to the given nodeand inserts the node 
         * at the position of the start of therange. 
         * @param newParent The node to surround the contents with. 
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if an ancestor container of either 
         *   end-point of the range is read-only.
         *   <br>WRONG_DOCUMENT_ERR: Raised if<code>newParent</code> and the 
         *   container of the start of the Range were not created from the same 
         *   document.
         *   <br>HIERARCHY_REQUEST_ERR: Raised if the container of the start of the 
         *   Range is of a type that does not allow children ofthe type of 
         *   <code>newParent</code> or if <code>newParent</code> is an ancestor of 
         *   thecontaineror if <code>node</code> would end up with a child node of 
         *   a type not allowedby the type of <code>node</code>.
         * @exception RangeException
         *   BAD_ENDPOINTS_ERR: Raised if the range partially selects a non-text 
         *   node.
         *   <br>INVALID_NODE_TYPE_ERR: Raised if<code>node</code> is an Attr, 
         *   Entity, DocumentType, Notation,Document, or DocumentFragment node.
         */
        void surroundContents(Node newParent)
                                                   ;//throws DOMException, RangeException;
        /**
         * Produces a new range whose end-points are equal tothe end-points of the 
         * range. 
         * @return The duplicated range. 
         */
        Range cloneRange();
        /**
         * Returns the contents of a range as a string. 
         * @return The contents of the range.
         */
        String toString();
    }
    public sealed class RangeConstants
    {
        public const int StartToStart = 1;
        public const int StartToEnd = 2;
        public const int EndToEnd = 3;
        public const int EndToStart = 4;
    }
}