/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */
using System;
using org.w3c.dom.stylesheets;

namespace org.w3c.dom.css
{

    /**
     * This interface represents a document with a CSS view.
     * <p> The <code>getOverrideStyle</code> method provides a mechanism through 
     * which a DOM author could effect immediate change to the style of an 
     * element without modifying the explicitly linked stylesheets of a document 
     * or the inline style of elements in the stylesheets.  This style sheet 
     * comes after the author style sheet in the cascade algorithm and is called 
     * override style sheet. The override style sheet takes over author style 
     * sheet. An "!important" declaration still takes precedence over a normal 
     * declaration. Both override and user style sheets may contain "!important" 
     * declarations, and user "!important" rules takes precedence over override 
     * "!important" rules. Both author and override style sheets may contain 
     * "!important" declarations, and override "!important" rules takes 
     * precedence over author "!important" rules. 
     */
    public interface DocumentCSS : DocumentStyle
    {
        /**
         *  This method is used to retrieve the override style sheet. 
         * @param elt  The element to be modified. 
         * @param pseudoElt  The pseudo element or <code>null</code> if any. 
         * @return  The override style declaration. 
         */
        CSSStyleDeclaration getOverrideStyle(Element elt,
                                                   String pseudoElt);
    }

}