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
     *  The <code>CSSRule</code> interface is the abstract base interface for any 
     * type of CSS statement. This includes both rule sets and at-rules. An 
     * implementation is expected to preserve all rules specified in a CSS style 
     * sheet, even if it is not recognized. Unrecognized rules are represented 
     * using the <code>CSSUnknownRule</code> interface. 
     * @since DOM Level 2
     */
    public interface CSSRule
    {

        /**
         *  The type of the rule, as defined above. The expectation is that 
         * binding-specific casting methods can be used to cast down from an 
         * instance of the <code>CSSRule</code> interface to the specific derived 
         * interface implied by the <code>type</code>. 
         */
        short getType();
        /**
         *  The parsable textual representation of the rule. This reflects the 
         * current state of the rule and not its initial value. 
         * @exception CSSException
         *   SYNTAX_ERR: Raised if the specified CSS string value has a syntax 
         *   error and is unparsable.
         *   <br>INVALID_MODIFICATION_ERR: Raised if the specified CSS string value 
         *   represents a different type of rule than the current one.
         * @exception DOMException
         *   HIERARCHY_REQUEST_ERR: Raised if the rule cannot be inserted at this 
         *   point in the style sheet.
         *   <br>NO_MODIFICATION_ALLOWED_ERR: Raised if the rule is readonly.
         */
        String getCssText();
        void setCssText(String cssText)
                                      ;//throws CSSException, DOMException;
        /**
         *  The style sheet that contains this rule. 
         */
        CSSStyleSheet getParentStyleSheet();
        /**
         *  If this rule is contained inside another rule (e.g. a style rule inside 
         * an @media block), this is the containing rule. If this rule is not 
         * nested inside any other rules, this returns <code>null</code>. 
         */
        CSSRule getParentRule();
    }

    public sealed class CSSRuleConstants
    {
        // RuleType
        public const short UNKNOWN_RULE = 0;
        public const short STYLE_RULE = 1;
        public const short CHARSET_RULE = 2;
        public const short IMPORT_RULE = 3;
        public const short MEDIA_RULE = 4;
        public const short FONT_FACE_RULE = 5;
        public const short PAGE_RULE = 6;
    }
}