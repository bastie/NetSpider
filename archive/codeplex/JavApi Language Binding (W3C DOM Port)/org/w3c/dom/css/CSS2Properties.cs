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
     *  The <code>CSS2Properties</code> interface represents a convenience 
     * mechanism for retrieving and setting properties within a 
     * <code>CSSStyleDeclaration</code>. The attributes of this interface 
     * correspond to all the properties specified in CSS2. Getting an attribute 
     * of this interface is equivalent to calling the 
     * <code>getPropertyValue</code> method of the 
     * <code>CSSStyleDeclaration</code> interface. Setting an attribute of this 
     * interface is equivalent to calling the <code>setProperty</code> method of 
     * the <code>CSSStyleDeclaration</code> interface. 
     * <p> A compliant implementation is not required to implement the 
     * <code>CSS2Properties</code> interface. If an implementation does implement 
     * this interface, the expectation is that language-specific methods can be 
     * used to cast from an instance of the <code>CSSStyleDeclaration</code> 
     * interface to the <code>CSS2Properties</code> interface. 
     * <p> If an implementation does implement this interface, it is expected to 
     * understand the specific syntax of the shorthand properties, and apply 
     * their semantics; when the <code>margin</code> property is set, for 
     * example, the <code>marginTop</code>, <code>marginRight</code>, 
     * <code>marginBottom</code> and <code>marginLeft</code> properties are 
     * actually being set by the underlying implementation. 
     * <p> When dealing with CSS "shorthand" properties, the shorthand properties 
     * should be decomposed into their component longhand properties as 
     * appropriate, and when querying for their value, the form returned should 
     * be the shortest form exactly equivalent to the declarations made in the 
     * ruleset.  However, if there is no shorthand declaration that could be 
     * added to the ruleset without changing in any way the rules already 
     * declared in the ruleset (i.e., by adding longhand rules that were 
     * previously not declared in the ruleset), then the empty string should be 
     * returned for the shorthand property. 
     * <p> For example, querying for the <code>font</code> property should not 
     * return "normal normal normal 14pt/normal Arial, sans-serif", when "14pt 
     * Arial, sans-serif" suffices (the normals are initial values, and are 
     * implied by use of the longhand property). 
     * <p> If the values for all the longhand properties that compose a particular 
     * string are the initial values, then a string consisting of all the initial 
     * values should be returned (e.g.  a <code>border-width</code> value of 
     * "medium" should be returned as such, not as ""). 
     * <p> For some shorthand properties that take missing values from other 
     * sides, such as the <code>margin</code>, <code>padding</code>, and 
     * <code>border-[width|style|color]</code> properties, the minimum number of 
     * sides possible should be used, i.e., "0px 10px" will be returned instead 
     * of "0px 10px 0px 10px". 
     * <p> If the value of a shorthand property can not be decomposed into its 
     * component longhand properties, as is the case for the <code>font</code> 
     * property with a value of "menu", querying for the values of the component 
     * longhand properties should return the empty string. 
     * @since DOM Level 2
     */
    public interface CSS2Properties
    {
        /**
         *  See the azimuth property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        String getAzimuth();
        void setAzimuth(String azimuth);//                                         ;//throws CSSException, DOMException;
        /**
         *  See the background property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
        String getBackground();
         void setBackground(String background)
                                                ;//throws CSSException, DOMException;
        /**
         *  See the background-attachment property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBackgroundAttachment();
         void setBackgroundAttachment(String backgroundAttachment)
                                                ;//throws CSSException, DOMException;
        /**
         *  See the background-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBackgroundColor();
         void setBackgroundColor(String backgroundColor)
                                                ;//throws CSSException, DOMException;
        /**
         *  See the background-image property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBackgroundImage();
         void setBackgroundImage(String backgroundImage)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the background-position property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBackgroundPosition();
         void setBackgroundPosition(String backgroundPosition)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the background-repeat property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBackgroundRepeat();
         void setBackgroundRepeat(String backgroundRepeat)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorder();
         void setBorder(String border)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-collapse property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderCollapse();
         void setBorderCollapse(String borderCollapse)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderColor();
         void setBorderColor(String borderColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-spacing property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderSpacing();
         void setBorderSpacing(String borderSpacing)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderStyle();
         void setBorderStyle(String borderStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-top property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderTop();
         void setBorderTop(String borderTop)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-right property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderRight();
         void setBorderRight(String borderRight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-bottom property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderBottom();
         void setBorderBottom(String borderBottom)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-left property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderLeft();
         void setBorderLeft(String borderLeft)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-top-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderTopColor();
         void setBorderTopColor(String borderTopColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-right-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderRightColor();
         void setBorderRightColor(String borderRightColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-bottom-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderBottomColor();
         void setBorderBottomColor(String borderBottomColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-left-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderLeftColor();
         void setBorderLeftColor(String borderLeftColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-top-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderTopStyle();
         void setBorderTopStyle(String borderTopStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-right-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderRightStyle();
         void setBorderRightStyle(String borderRightStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-bottom-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderBottomStyle();
         void setBorderBottomStyle(String borderBottomStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-left-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderLeftStyle();
         void setBorderLeftStyle(String borderLeftStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-top-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderTopWidth();
         void setBorderTopWidth(String borderTopWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-right-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderRightWidth();
         void setBorderRightWidth(String borderRightWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-bottom-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderBottomWidth();
         void setBorderBottomWidth(String borderBottomWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-left-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderLeftWidth();
         void setBorderLeftWidth(String borderLeftWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the border-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBorderWidth();
         void setBorderWidth(String borderWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the bottom property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getBottom();
         void setBottom(String bottom)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the caption-side property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCaptionSide();
         void setCaptionSide(String captionSide)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the clear property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getClear();
         void setClear(String clear)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the clip property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getClip();
         void setClip(String clip)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getColor();
         void setColor(String color)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the content property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getContent();
         void setContent(String content)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the counter-increment property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCounterIncrement();
         void setCounterIncrement(String counterIncrement)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the counter-reset property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCounterReset();
         void setCounterReset(String counterReset)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the cue property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCue();
         void setCue(String cue)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the cue-after property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCueAfter();
         void setCueAfter(String cueAfter)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the cue-before property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCueBefore();
         void setCueBefore(String cueBefore)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the cursor property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCursor();
         void setCursor(String cursor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the direction property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getDirection();
         void setDirection(String direction)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the display property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getDisplay();
         void setDisplay(String display)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the elevation property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getElevation();
         void setElevation(String elevation)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the empty-cells property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getEmptyCells();
         void setEmptyCells(String emptyCells)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the float property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getCssFloat();
         void setCssFloat(String cssFloat)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFont();
         void setFont(String font)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-family property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontFamily();
         void setFontFamily(String fontFamily)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-size property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontSize();
         void setFontSize(String fontSize)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-size-adjust property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontSizeAdjust();
         void setFontSizeAdjust(String fontSizeAdjust)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-stretch property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontStretch();
         void setFontStretch(String fontStretch)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontStyle();
         void setFontStyle(String fontStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-variant property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontVariant();
         void setFontVariant(String fontVariant)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the font-weight property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getFontWeight();
         void setFontWeight(String fontWeight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the height property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getHeight();
         void setHeight(String height)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the left property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getLeft();
         void setLeft(String left)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the letter-spacing property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getLetterSpacing();
         void setLetterSpacing(String letterSpacing)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the line-height property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getLineHeight();
         void setLineHeight(String lineHeight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the list-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getListStyle();
         void setListStyle(String listStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the list-style-image property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getListStyleImage();
         void setListStyleImage(String listStyleImage)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the list-style-position property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getListStylePosition();
         void setListStylePosition(String listStylePosition)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the list-style-type property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getListStyleType();
         void setListStyleType(String listStyleType)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the margin property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMargin();
         void setMargin(String margin)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the margin-top property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarginTop();
         void setMarginTop(String marginTop)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the margin-right property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarginRight();
         void setMarginRight(String marginRight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the margin-bottom property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarginBottom();
         void setMarginBottom(String marginBottom)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the margin-left property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarginLeft();
         void setMarginLeft(String marginLeft)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the marker-offset property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarkerOffset();
         void setMarkerOffset(String markerOffset)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the marks property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMarks();
         void setMarks(String marks)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the max-height property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMaxHeight();
         void setMaxHeight(String maxHeight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the max-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMaxWidth();
         void setMaxWidth(String maxWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the min-height property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMinHeight();
         void setMinHeight(String minHeight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the min-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getMinWidth();
         void setMinWidth(String minWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the orphans property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOrphans();
         void setOrphans(String orphans)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the outline property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOutline();
         void setOutline(String outline)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the outline-color property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOutlineColor();
         void setOutlineColor(String outlineColor)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the outline-style property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOutlineStyle();
         void setOutlineStyle(String outlineStyle)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the outline-width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOutlineWidth();
         void setOutlineWidth(String outlineWidth)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the overflow property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getOverflow();
         void setOverflow(String overflow)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the padding property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPadding();
         void setPadding(String padding)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the padding-top property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPaddingTop();
         void setPaddingTop(String paddingTop)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the padding-right property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPaddingRight();
         void setPaddingRight(String paddingRight)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the padding-bottom property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPaddingBottom();
         void setPaddingBottom(String paddingBottom)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the padding-left property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPaddingLeft();
         void setPaddingLeft(String paddingLeft)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the page property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPage();
         void setPage(String page)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the page-break-after property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPageBreakAfter();
         void setPageBreakAfter(String pageBreakAfter)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the page-break-before property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPageBreakBefore();
         void setPageBreakBefore(String pageBreakBefore)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the page-break-inside property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPageBreakInside();
         void setPageBreakInside(String pageBreakInside)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the pause property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPause();
         void setPause(String pause)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the pause-after property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPauseAfter();
         void setPauseAfter(String pauseAfter)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the pause-before property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPauseBefore();
         void setPauseBefore(String pauseBefore)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the pitch property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPitch();
         void setPitch(String pitch)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the pitch-range property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPitchRange();
         void setPitchRange(String pitchRange)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the play-during property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPlayDuring();
         void setPlayDuring(String playDuring)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the position property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getPosition();
         void setPosition(String position)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the quotes property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getQuotes();
         void setQuotes(String quotes)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the richness property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getRichness();
         void setRichness(String richness)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the right property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getRight();
         void setRight(String right)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the size property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSize();
         void setSize(String size)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the speak property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSpeak();
         void setSpeak(String speak)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the speak-header property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSpeakHeader();
         void setSpeakHeader(String speakHeader)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the speak-numeral property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSpeakNumeral();
         void setSpeakNumeral(String speakNumeral)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the speak-punctuation property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSpeakPunctuation();
         void setSpeakPunctuation(String speakPunctuation)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the speech-rate property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getSpeechRate();
         void setSpeechRate(String speechRate)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the stress property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getStress();
         void setStress(String stress)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the table-layout property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTableLayout();
         void setTableLayout(String tableLayout)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the text-align property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTextAlign();
         void setTextAlign(String textAlign)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the text-decoration property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTextDecoration();
         void setTextDecoration(String textDecoration)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the text-indent property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTextIndent();
         void setTextIndent(String textIndent)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the text-shadow property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTextShadow();
         void setTextShadow(String textShadow)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the text-transform property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTextTransform();
         void setTextTransform(String textTransform)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the top property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getTop();
         void setTop(String top)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the unicode-bidi property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getUnicodeBidi();
         void setUnicodeBidi(String unicodeBidi)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the vertical-align property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getVerticalAlign();
         void setVerticalAlign(String verticalAlign)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the visibility property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getVisibility();
         void setVisibility(String visibility)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the voice-family property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getVoiceFamily();
         void setVoiceFamily(String voiceFamily)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the volume property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getVolume();
         void setVolume(String volume)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the white-space property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getWhiteSpace();
         void setWhiteSpace(String whiteSpace)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the widows property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getWidows();
         void setWidows(String widows)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the width property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getWidth();
         void setWidth(String width)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the word-spacing property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getWordSpacing();
         void setWordSpacing(String wordSpacing)
                                               ;//throws CSSException, DOMException;
        /**
         *  See the z-index property definition in CSS2. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the new value has a syntax error  and is 
         *   unparsable.
         * @exception DOMException
         *   NO_MODIFICATION_ALLOWED_ERR: Raised if this property is readonly.
         */
         String getZIndex();
         void setZIndex(String zIndex)
                                               ;//throws CSSException, DOMException;
    }

}