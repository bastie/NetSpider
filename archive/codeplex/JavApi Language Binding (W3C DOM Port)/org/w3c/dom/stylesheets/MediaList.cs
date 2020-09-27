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
     *  The <code>MediaList</code> interface provides the abstraction of an 
     * ordered collection of media, without defining or constraining how this 
     * collection is implemented. An empty list is the same as a list that 
     * contain the medium <code>"all"</code>. 
     * @since DOM Level 2
     */
    public interface MediaList
    {
        /**
         *  The parsable textual representation of the media list. This is a 
         * comma-separated list of media. 
         * @exception DOMException
         *   SYNTAX_ERR: Raised if the specified CSS string value has a syntax 
         *   error and is unparsable.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raised if this media list is readonly.
         */
        String getCssText();
        void setCssText(String cssText);//                                 throws DOMException;
        /**
         *  The number of media in the list. The range of valid media is 
         * <code>0</code> to <code>length-1</code> inclusive. 
         */
        int getLength();
        /**
         *  Returns the <code>index</code>th in the list. If <code>index</code> is 
         * greater than or equal to the number of media in the list, this returns 
         * <code>null</code>. 
         * @param index  Index into the collection. 
         * @return  The medium at the <code>index</code>th position in the 
         *   <code>MediaList</code>, or <code>null</code> if that is not a valid 
         *   index. 
         */
        String item(int index);
        /**
         *  Deletes the medium indicated by <code>oldMedium</code> from the list. 
         * @param oldMedium The medium to delete in the media list.
         * @exception DOMException
         *    NO_MODIFICATION_ALLOWED_ERR: Raised if this list is readonly. 
         *   <br> NOT_FOUND_ERR: Raised if <code>oldMedium</code> is not in the 
         *   list. 
         */
        void delete(String oldMedium);//                                   throws DOMException;
        /**
         *  Adds the medium <code>newMedium</code> to the end of the list. It the 
         * <code>newMedium</code> is already used, it is first removed. 
         * @param newMedium The new medium to add.
         * @exception DOMException
         *    NO_MODIFICATION_ALLOWED_ERR: Raised if this list is readonly. 
         */
        void append(String newMedium);//                                   throws DOMException;
    }
}