/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.css
{

    /**
     *  The <code>RGBColor</code> interface is used to represent any RGB color 
     * value. This interface reflects the values in the underlying style 
     * property. Hence, modifications made through this interface modify the 
     * style property. 
     * @since DOM Level 2
     */
    public interface RGBColor
    {
        /**
         *  This attribute is used for the red value of the RGB color. 
         */
        CSSPrimitiveValue getRed();
        /**
         *  This attribute is used for the green value of the RGB color. 
         */
        CSSPrimitiveValue getGreen();
        /**
         *  This attribute is used for the blue value of the RGB color. 
         */
        CSSPrimitiveValue getBlue();
    }

}