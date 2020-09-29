/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  Copyright © 2020 Sebastian Ritter
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.text
{
    ///
    /// Dummy implementation to build project 
    /// TODO: implement it!
    ///
    public class MessageFormat : java.text.Format
    {
        public override java.lang.StringBuffer format(Object obj, java.lang.StringBuffer buffer, FieldPosition field){
            throw new java.lang.UnsupportedOperationException ("Not yet implemented");
        }
        
        public override Object parseObject(String s, ParsePosition position){
            throw new java.lang.UnsupportedOperationException ("Not yet implemented");
        }
        
        ///
        /// Dummy method for classes 
        /// - java.util.logging.Formatter
        /// - java.util.logging.SimpleFormatter
        ///
        public static String format(String pattern, Object[] paramsJ) {
            throw new java.lang.UnsupportedOperationException ("Not yet implemented");
        }
    }

}