/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.css
{

    /**
     *  The <code>CSSRuleList</code> interface provides the  abstraction of an 
     * ordered collection of CSS rules. 
     * @since DOM Level 2
     */
    public interface CSSRuleList
    {
        /**
         *  The number of <code>CSSRule</code>s in the list. The range of valid 
         * child rule indices is <code>0</code> to <code>length-1</code> inclusive. 
         */
        int getLength();
        /**
         *  Used to retrieve a CSS rule by ordinal index. The order in this 
         * collection represents the order of the rules in the CSS style sheet. 
         * @param index Index into the collection
         * @return The style rule at the <code>index</code> position in the 
         *   <code>CSSRuleList</code>, or <code>null</code> if  that is not a valid 
         *   index. 
         */
        CSSRule item(int index);
    }

}