/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.css
{

    /**
     * The <code>CSS2TextShadow</code> interface represents a simple value for the 
     * text-shadow CSS Level 2 property.
     * @since DOM Level 2
     */
    public interface CSS2TextShadow
    {
        /**
         * Specified the color of the text shadow. The CSS Value can contain an 
         * empty string if no color has been specified.
         */
         CSSValue getColor();
        /**
         * The horizontal position of the text shadow. <code>0</code> if no length 
         * has been specified.
         */
         CSSValue getHorizontal();
        /**
         * The vertical position of the text shadow. <code>0</code> if no length has 
         * been specified.
         */
         CSSValue getVertical();
        /**
         * The blur radius of the text shadow. <code>0</code> if no length has been 
         * specified.
         */
         CSSValue getBlur();
    }

}