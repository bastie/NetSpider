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
     * The <code>CSS2Azimuth</code> interface represents the azimuth CSS Level 2 
     * property.
     * @since DOM Level 2
     */
    public interface CSS2Azimuth : CSSValue
    {
        /**
         * A code defining the type of the value as defined in <code>CSSValue</code>
         * . It would be one of <code>CSS_DEG</code>, <code>CSS_RAD</code>, 
         * <code>CSS_GRAD</code> or <code>CSS_IDENT</code>.
         */
        short getAzimuthType();
        /**
         * If <code>azimuthType</code> is <code>CSS_IDENT</code>, 
         * <code>identifier</code> contains one of left-side, far-left, left, 
         * center-left, center, center-right, right, far-right, right-side, 
         * leftwards, rightwards. The empty string if none is set.
         */
        String getIdentifier();
        /**
         * <code>behind</code> indicates whether the behind identifier has been set.
         */
        bool getBehind();
        /**
         * A method to set the angle value with a specified unit. This method will 
         * unset any previously set identifiers values.
         * @param uType The unitType could only be one of <code>CSS_DEG</code>, 
         *   <code>CSS_RAD</code> or <code>CSS_GRAD</code>).
         * @param fValue The new float value of the angle.
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raised if the unit type is invalid.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        void setAngleValue(short uType,
                                               float fValue)
                                              ;//throws DOMException;
        /**
         * Used to retrieved the float value of the azimuth property.
         * @param uType The unit type can be only an angle unit type (
         *   <code>CSS_DEG</code>, <code>CSS_RAD</code> or <code>CSS_GRAD</code>).
         * @return The float value.
         * @exception DOMException
         *   INVALID_ACCESS_ERR: Raised if the unit type is invalid.
         */
        float getAngleValue(short uType)
                                              ;//throws DOMException;
        /**
         * Setting the identifier for the azimuth property will unset any previously 
         * set angle value. The value of <code>azimuthType</code> is set to 
         * <code>CSS_IDENT</code>
         * @param ident The new identifier. If the identifier is "leftwards" or 
         *   "rightward", the behind attribute is ignored.
         * @param b The new value for behind.
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the specified <code>identifier</code> has a 
         *   syntax error and is unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        void setIdentifier(String ident,
                                               bool b)
                                              ;//throws CSSException, DOMException;
    }

}