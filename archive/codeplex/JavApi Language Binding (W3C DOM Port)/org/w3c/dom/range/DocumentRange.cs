/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.range
{

    /**
     */
    public interface DocumentRange
    {
        /**
         * This interface can be obtained from the object implementing the 
         * <code>Document</code> interface using binding-specific casting methods.
         * @return The initial state of the range returned from this  method is such 
         *   that both of its end-points are positioned at the beginning  of the 
         *   corresponding  Document, before any content. The range returned can 
         *   only be used to select content associated with this Document, or with 
         *   DocumentFragments and Attrs for which this Document is the 
         *   <code>ownerDocument</code>.
         */
        Range createRange();
    }
}