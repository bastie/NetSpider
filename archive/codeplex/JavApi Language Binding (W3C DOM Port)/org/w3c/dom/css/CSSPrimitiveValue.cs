/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.css
{

    /**
     *  The <code>CSSPrimitiveValue</code> interface represents a single CSS value
     * . This interface may be used to determine the value of a specific style 
     * property currently set in a block or to set a specific style properties  
     * explicitly within the block. An instance of this interface can be obtained 
     * from the <code>getPropertyCSSValue</code> method of the  
     * <code>CSSStyleDeclaration</code> interface. 
     * @since DOM Level 2
     */
    public interface CSSPrimitiveValue : CSSValue
    {
        /**
         * The type of the value as defined by the constants specified above.
         */
        short getPrimitiveType();
        /**
         *  A method to set the float value with a specified unit. If the property 
         * attached with this value can not accept the specified unit or the float 
         * value, the value will be unchanged and a <code>DOMException</code> will 
         * be raised. 
         * @param unitType  A unit code as defined above. The unit code can only be 
         *   a float unit type (e.g. <code>NUMBER</code>, <code>PERCENTAGE</code>, 
         *   <code>CSS_EMS</code>, <code>CSS_EXS</code>, <code>CSS_PX</code>, 
         *   <code>CSS_PX</code>, <code>CSS_CM</code>, <code>CSS_MM</code>, 
         *   <code>CSS_IN</code>,  <code>CSS_PT</code>, <code>CSS_PC</code>, 
         *   <code>CSS_DEG</code>, <code>CSS_RAD</code>, <code>CSS_GRAD</code>, 
         *   <code>CSS_MS</code>, <code>CSS_S</code>, <code>CSS_HZ</code>, 
         *   <code>CSS_KHZ</code>, <code>CSS_DIMENSION</code>). 
         * @param floatValue  The new float value. 
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the attached property doesn't support 
         *   the float value or the unit type.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        void setFloatValue(short unitType,
                                               float floatValue)
                                              ;//throws DOMException;
        /**
         *  This method is used to get a float value in a specified unit. If this 
         * CSS value doesn't contain a float value or can't be converted into the 
         * specified unit, a <code>DOMException</code> is raised. 
         * @param unitType  A unit code to get the float value.  The unit code can 
         *   only be a float unit type (e.g. <code>CSS_NUMBER</code>, 
         *   <code>CSS_PERCENTAGE</code>, <code>CSS_EMS</code>, <code>CSS_EXS</code>
         *   , <code>CSS_PX</code>, <code>CSS_PX</code>, <code>CSS_CM</code>, 
         *   <code>CSS_MM</code>, <code>CSS_IN</code>,  <code>CSS_PT</code>, 
         *   <code>CSS_PC</code>, <code>CSS_DEG</code>, <code>CSS_RAD</code>, 
         *   <code>CSS_GRAD</code>, <code>CSS_MS</code>, <code>CSS_S</code>, 
         *   <code>CSS_HZ</code>, <code>CSS_KHZ</code>, <code>CSS_DIMENSION</code>).
         *    
         * @return  The float value in the specified unit. 
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the CSS value doesn't contain a float 
         *   value or if the float value can't be converted into the specified unit.
         *    
         */
        float getFloatValue(short unitType)
                                              ;//throws DOMException;
        /**
         *  A method to set the string value with a specified unit. If the property  
         * attached to this value can't accept the specified unit or the string 
         * value, the value will be unchanged and a <code>DOMException</code> will  
         * be raised. 
         * @param stringType  A string code as defined above. The string code can 
         *   only be a string unit type (e.g. <code>CSS_URI</code>, 
         *   <code>CSS_IDENT</code>, <code>CSS_INHERIT</code> and 
         *   <code>CSS_ATTR</code>). 
         * @param stringValue  The new string value. If the <code>stringType</code> 
         *   is equal to <code>CSS_INHERIT</code>, the <code>stringValue</code> 
         *   should be <code>inherit</code>. 
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the CSS value doesn't contain a string 
         *   value or if the string value can't be converted into the specified 
         *   unit.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        void setStringValue(short stringType,
                                                String stringValue)
                                               ;//throws DOMException;
        /**
         *  This method is used to get the string value in a specified unit. If the 
         * CSS value doesn't contain a string value, a <code>DOMException</code> is 
         * raised. 
         * @return  The string value in the current unit. The current 
         *   <code>valueType</code> can only be a string unit type (e.g. 
         *   <code>CSS_URI</code>, <code>CSS_IDENT</code> and <code>CSS_ATTR</code>
         *   ). 
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the CSS value doesn't contain a string 
         *   value. 
         */
        String getStringValue()
                                               ;//throws DOMException;
        /**
         *  This method is used to get the Counter value. If this CSS value doesn't 
         * contain a counter value, a <code>DOMException</code> is raised. 
         * Modification to the corresponding style property can be achieved using 
         * the <code>Counter</code> interface. 
         * @return The Counter value.
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the CSS value doesn't contain a Counter 
         *   value. 
         */
        Counter getCounterValue()
                                                ;//throws DOMException;
        /**
         *  This method is used to get the Rect value. If this CSS value doesn't 
         * contain a rect value, a <code>DOMException</code> is raised. 
         * Modification to the corresponding style property can be achieved using 
         * the <code>Rect</code> interface. 
         * @return The Rect value.
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the CSS value doesn't contain a Rect 
         *   value. 
         */
        Rect getRectValue()
                                             ;//throws DOMException;
        /**
         *  This method is used to get the RGB color. If this CSS value doesn't 
         * contain a RGB color value, a <code>DOMException</code> is raised. 
         * Modification to the corresponding style property can be achieved using 
         * the <code>RGBColor</code> interface. 
         * @return the RGB color value.
         * @exception DOMException
         *    INVALID_ACCESS_ERR: Raises if the attached property can't return a 
         *   RGB color value. 
         */
        RGBColor getRGBColorValue()
                                                 ;//throws DOMException;
    }

    public sealed class CSSPrimitiveValueConstants
    {
        // UnitTypes
        public const short CSS_UNKNOWN = 0;
        public const short CSS_NUMBER = 1;
        public const short CSS_PERCENTAGE = 2;
        public const short CSS_EMS = 3;
        public const short CSS_EXS = 4;
        public const short CSS_PX = 5;
        public const short CSS_CM = 6;
        public const short CSS_MM = 9;
        public const short CSS_IN = 10;
        public const short CSS_PT = 11;
        public const short CSS_PC = 12;
        public const short CSS_DEG = 13;
        public const short CSS_RAD = 14;
        public const short CSS_GRAD = 15;
        public const short CSS_MS = 16;
        public const short CSS_S = 17;
        public const short CSS_HZ = 18;
        public const short CSS_KHZ = 19;
        public const short CSS_DIMENSION = 20;
        public const short CSS_STRING = 21;
        public const short CSS_URI = 22;
        public const short CSS_IDENT = 23;
        public const short CSS_ATTR = 24;
        public const short CSS_COUNTER = 25;
        public const short CSS_RECT = 26;
        public const short CSS_RGBCOLOR = 27;


    }
}