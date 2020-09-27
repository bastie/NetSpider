/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
namespace org.w3c.dom.css
{

    /**
     * The <code>CSSValueList</code> interface provides the abstraction of an 
     * ordered collection of CSS values.
     * <p> Some properties allow an empty list into their syntax. In that case, 
     * these properties take the <code>none</code> identifier. So, an empty list  
     * means that the propertie has the value <code>none</code>. 
     * @since DOM Level 2
     */
    public interface CSSValueList : CSSValue
    {
        /**
         * The number of <code>CSSValue</code>s in the list. The range of valid 
         * values indices is <code>0</code> to <code>length-1</code> inclusive.
         * <br> For 
         */
        int getLength();
        /**
         * Used to retrieve a CSS rule by ordinal index. The order in this 
         * collection represents the order of the values in the CSS style property.
         * @param index Index into the collection.
         * @return The style rule at the <code>index</code> position in the 
         *   <code>CSSValueList</code>, or <code>null</code> if that is not valid 
         *   index.
         */
        CSSValue item(int index);
    }

}