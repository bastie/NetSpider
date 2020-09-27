/*
 * Copyright (c) 2003 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de
 * Recherche en Informatique et en Automatique, Keio University). All
 * Rights Reserved. This program is distributed under the W3C's Software
 * Intellectual Property License. This program is distributed in the
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE.
 * See W3C License http://www.w3.org/Consortium/Legal/ for more details.
 */
using System;

namespace org.w3c.dom.html2
{

    /**
     * The select element allows the selection of an option. The contained options 
     * can be directly accessed through the select element as a collection. See 
     * the SELECT element definition in HTML 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
     */
    public interface HTMLSelectElement : HTMLElement
    {
        /**
         * The type of this form control. This is the string "select-multiple" 
         * when the multiple attribute is <code>true</code> and the string 
         * "select-one" when <code>false</code>.
         */
        String getType();

        /**
         * The ordinal index of the selected option, starting from 0. The value -1 
         * is returned if no element is selected. If multiple options are 
         * selected, the index of the first selected option is returned. 
         */
        int getSelectedIndex();
        /**
         * The ordinal index of the selected option, starting from 0. The value -1 
         * is returned if no element is selected. If multiple options are 
         * selected, the index of the first selected option is returned. 
         */
        void setSelectedIndex(int selectedIndex);

        /**
         *  The current form control value (i.e. the value of the currently 
         * selected option), if multiple options are selected this is the value 
         * of the first selected option. 
         */
        String getValue();
        /**
         *  The current form control value (i.e. the value of the currently 
         * selected option), if multiple options are selected this is the value 
         * of the first selected option. 
         */
        void setValue(String value);

        /**
         *  The number of options in this <code>SELECT</code>. 
         * @version DOM Level 2
         */
        int getLength();
        /**
         *  The number of options in this <code>SELECT</code>. 
         * @exception DOMException
         *    NOT_SUPPORTED_ERR: if setting the length is not allowed by the 
         *   implementation. 
         * @version DOM Level 2
         */
        void setLength(int length);
        //throws DOMException;

        /**
         * Returns the <code>FORM</code> element containing this control. Returns 
         * <code>null</code> if this control is not within the context of a 
         * form. 
         */
        HTMLFormElement getForm();

        /**
         * The collection of <code>OPTION</code> elements contained by this 
         * element. 
         * @version DOM Level 2
         */
        HTMLOptionsCollection getOptions();

        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in HTML 4.01.
         */
        bool getDisabled();
        /**
         * The control is unavailable in this context. See the disabled attribute 
         * definition in HTML 4.01.
         */
        void setDisabled(bool disabled);

        /**
         * If true, multiple <code>OPTION</code> elements may be selected in this 
         * <code>SELECT</code>. See the multiple attribute definition in HTML 
         * 4.01.
         */
        bool getMultiple();
        /**
         * If true, multiple <code>OPTION</code> elements may be selected in this 
         * <code>SELECT</code>. See the multiple attribute definition in HTML 
         * 4.01.
         */
        void setMultiple(bool multiple);

        /**
         * Form control or object name when submitted with a form. See the name 
         * attribute definition in HTML 4.01.
         */
        String getName();
        /**
         * Form control or object name when submitted with a form. See the name 
         * attribute definition in HTML 4.01.
         */
        void setName(String name);

        /**
         * Number of visible rows. See the size attribute definition in HTML 4.01.
         */
        int getSize();
        /**
         * Number of visible rows. See the size attribute definition in HTML 4.01.
         */
        void setSize(int size);

        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in HTML 4.01.
         */
        int getTabIndex();
        /**
         * Index that represents the element's position in the tabbing order. See 
         * the tabindex attribute definition in HTML 4.01.
         */
        void setTabIndex(int tabIndex);

        /**
         * Add a new element to the collection of <code>OPTION</code> elements for 
         * this <code>SELECT</code>. This method is the equivalent of the 
         * <code>appendChild</code> method of the <code>Node</code> interface if 
         * the <code>before</code> parameter is <code>null</code>. It is 
         * equivalent to the <code>insertBefore</code> method on the parent of 
         * <code>before</code> in all other cases. This method may have no 
         * effect if the new element is not an <code>OPTION</code> or an 
         * <code>OPTGROUP</code>.
         * @param element The element to add.
         * @param before The element to insert before, or <code>null</code> for 
         *   the tail of the list.
         * @exception DOMException
         *   NOT_FOUND_ERR: Raised if <code>before</code> is not a descendant of 
         *   the <code>SELECT</code> element. 
         */
        void add(HTMLElement element,
                        HTMLElement before);
        //throws DOMException;

        /**
         * Remove an element from the collection of <code>OPTION</code> elements 
         * for this <code>SELECT</code>. Does nothing if no element has the 
         * given index.
         * @param index The index of the item to remove, starting from 0.
         */
        void remove(int index);

        /**
         * Removes keyboard focus from this element.
         */
        void blur();

        /**
         * Gives keyboard focus to this element.
         */
        void focus();

    }
}