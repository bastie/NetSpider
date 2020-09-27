/*
 * Copyright (c) 1999 World Wide Web Consortium,
 * (Massachusetts Institute of Technology, Institut National de Recherche
 *  en Informatique et en Automatique, Keio University).
 * All Rights Reserved. http://www.w3.org/Consortium/Legal/
 */

namespace org.w3c.dom.stylesheets
{

    /**
     *  The <code>LinkStyle</code> interface provides a mechanism by which a style 
     * sheet can be retrieved from the node responsible for linking it into a 
     * document. An instance of the <code>LinkStyle</code> interface can be 
     * obtained using binding-specific casting methods on an instance of a 
     * linking node (<code>HTMLLinkElement</code>, <code>HTMLStyleElement</code> 
     * or <code>ProcessingInstruction</code> in DOM Level 2). 
     */
    public interface LinkStyle
    {
        /**
         *  The style sheet. 
         */
        StyleSheet getSheet();
    }
}
