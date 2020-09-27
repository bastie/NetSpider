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
     *  The <code>Counter</code> interface is used to represent any counter or 
     * counters function value. This interface reflects the values in  the 
     * underlying style property. Hence, modifications made through this  
     * interface modify the style property. 
     * @since DOM Level 2
     */
    public interface Counter
    {
        /**
         *  This attribute is used for the identifier of the counter. 
         */
        String getIdentifier();
        /**
         *  This attribute is used for the style of the list. 
         */
        String getListStyle();
        /**
         *  This attribute is used for the separator of nested counters. 
         */
        String getSeparator();
    }

}