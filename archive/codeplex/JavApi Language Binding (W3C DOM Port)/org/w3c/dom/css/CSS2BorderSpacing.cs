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
     * The <code>CSS2BorderSpacing</code> interface represents the border-spacing 
     * CSS Level 2 property.
     * @since DOM Level 2
     */
    public interface CSS2BorderSpacing : CSSValue
    {
        /**
         *  The A code defining the type of the value as defined in 
         * <code>CSSValue</code>. It would be one of <code>CSS_EMS</code>, 
         * <code>CSS_EXS</code>, <code>CSS_PX</code>, <code>CSS_CM</code>, 
         * <code>CSS_MM</code>, <code>CSS_IN</code>, <code>CSS_PT</code> or 
         * <code>CSS_PC</code>. 
         */
        short getHorizontalType();
        /**
         *  The A code defining the type of the value as defined in 
         * <code>CSSValue</code>. It would be one of <code>CSS_EMS</code>, 
         * <code>CSS_EXS</code>, <code>CSS_PX</code>, <code>CSS_CM</code>, 
         * <code>CSS_MM</code>, <code>CSS_IN</code>, <code>CSS_PT</code>, 
         * <code>CSS_PC</code> or <code>CSS_INHERIT</code>. 
         */
        short getVerticalType();
        /**
         * This method is used to get the float value in a specified unit if the 
         * <code>horizontalSpacing</code> represents a length. If the float doesn't 
         * contain a float value or can't be converted into the specified unit, a 
         * <code>DOMException</code> is raised.
         * @param hType The horizontal unit.
         * @return The float value.
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raises if the property doesn't contain a float  or 
         *   the value can't be converted.
         */
        float getHorizontalSpacing(float hType)
                                                     ;//throws DOMException;
        /**
         * This method is used to get the float value in a specified unit if the 
         * <code>verticalSpacing</code> represents a length. If the float doesn't 
         * contain a float value or can't be converted into the specified unit, a 
         * <code>DOMException</code> is raised. The value is <code>0</code> if only 
         * the horizontal value has been specified.
         * @param vType The vertical unit.
         * @return The float value.
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raises if the property doesn't contain a float  or 
         *   the value can't be converted.
         */
        float getVerticalSpacing(float vType)
                                                   ;//throws DOMException;
        /**
         *  This method is used to set the horizontal spacing with a specified unit. 
         * If the vertical value is a length, it sets the vertical spacing to 
         * <code>0</code>. 
         * @param hType The horizontal unit.
         * @param value  The new value. 
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raises if the specified unit is not a length.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raises if this property is readonly.
         */
        void setHorizontalSpacing(short hType,
                                                      float value)
                                                     ;//throws DOMException;
        /**
         *  This method is used to set the vertical spacing with a specified unit. 
         * If the horizontal value is not a length, it sets the vertical spacing to 
         * <code>0</code>. 
         * @param vType The vertical unit.
         * @param value  The new value. 
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raises if the specified unit is not a length or a 
         *   percentage.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raises if this property is readonly.
         */
        void setVerticalSpacing(short vType,
                                                    float value)
                                                   ;//throws DOMException;
    }

}