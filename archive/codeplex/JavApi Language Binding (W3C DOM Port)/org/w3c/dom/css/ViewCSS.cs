/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;
using org.w3c.dom.views;

namespace org.w3c.dom.css
{

    /**
     *  This interface represents a CSS view. The <code>getComputedStyle</code> 
     * method provides a read only access to the computed values of an element. 
     */
    public interface ViewCSS : AbstractView
    {
        /**
         *  This method is used to the computed style sheet as it defines in the . 
         * @param elt  The element. 
         * @param pseudoElt  The pseudo element or <code>null</code> if any. 
         * @return  The computed style. The <code>CSSStyleDeclaration</code> is read 
         *   only. 
         */
        CSSStyleDeclaration getComputedStyle(Element elt,
                                                   String pseudoElt);
    }

}