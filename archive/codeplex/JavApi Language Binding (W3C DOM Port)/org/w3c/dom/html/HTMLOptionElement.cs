/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;

namespace org.w3c.dom.html
{

    /**
     * A selectable choice. See the OPTION element definition in HTML 4.0.
     */
    public interface HTMLOptionElement : HTMLElement
    {
        /**
         * Returns the <code>FORM</code> element containing this control. Returns 
         * <code>null</code> if this control is not within the context of a form. 
         */
        HTMLFormElement getForm();
        /**
         * Represents the value of the HTML selected attribute. The value of this 
         * attribute does not change if the state of the corresponding form 
         * control, in an interactive user agent, changes. Changing 
         * <code>defaultSelected</code>, however, resets the state of the form 
         * control. See the selected attribute definition in HTML 4.0.
         */
        bool getDefaultSelected();
        void setDefaultSelected(bool defaultSelected);
        /**
         * The text contained within the option element. 
         */
        String getText();
        /**
         * The index of this <code>OPTION</code> in its parent <code>SELECT</code>. 
         */
        int getIndex();
        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in HTML 4.0.
         */
        bool getDisabled();
        void setDisabled(bool disabled);
        /**
         * Option label for use in hierarchical menus. See the label attribute 
         * definition in HTML 4.0.
         */
        String getLabel();
        void setLabel(String label);
        /**
         * Represents the current state of the corresponding form control, in an 
         * interactive user agent. Changing this attribute changes the state of the 
         * form control, but does not change the value of the HTML selected 
         * attribute of the element.
         */
        bool getSelected();
        void setSelected(bool selected);
        /**
         * The current form control value. See the value attribute definition in 
         * HTML 4.0.
         */
        String getValue();
        void setValue(String value);
    }

}