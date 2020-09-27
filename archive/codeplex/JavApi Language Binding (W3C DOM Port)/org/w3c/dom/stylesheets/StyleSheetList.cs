/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.stylesheets
{

    /**
     * The <code>StyleSheetList</code> interface provides the  abstraction of an 
     * ordered collection of style sheets. 
     * @since DOM Level 2
     */
    public interface StyleSheetList
    {
        /**
         *  The number of <code>StyleSheet</code> in the list. The range of valid 
         * child stylesheet indices is <code>0</code> to <code>length-1</code> 
         * inclusive. 
         */
        int getLength();
        /**
         *  Used to retrieve a style sheet by ordinal index. 
         * @param index Index into the collection
         * @return The style sheet at the <code>index</code> position in the 
         *   <code>StyleSheetList</code>, or <code>null</code> if  that is not a 
         *   valid index. 
         */
        StyleSheet item(int index);
    }
}