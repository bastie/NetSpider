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
using java = biz.ritter.javapi;

namespace vampire.extension
{
///<summary>
/// Entension class for enums
///</summary>
namespace biz.ritter.javapi.lang.SystemJ.Logger {
        public static class LevelExtension {
        
	      public static String getName(this java.lang.SystemJ.Logger.Level self) {
	        switch (self){
	          case java.lang.SystemJ.Logger.Level.OFF : return "OFF";
	          case java.lang.SystemJ.Logger.Level.ERROR : return "ERROR";
	          case java.lang.SystemJ.Logger.Level.WARNING : return "WARNING";
	          case java.lang.SystemJ.Logger.Level.INFO : return "INFO";
	          case java.lang.SystemJ.Logger.Level.DEBUG : return "DEBUG";
	          case java.lang.SystemJ.Logger.Level.TRACE : return "TRACE";
	          case java.lang.SystemJ.Logger.Level.ALL : return "ALL";
	          default : throw new java.lang.UnknownError("Unknown enum error.");
	        } 
	      }
	      public static int getSeverity(this java.lang.SystemJ.Logger.Level self){
	        switch (self){
	          case java.lang.SystemJ.Logger.Level.OFF : return (int)java.lang.SystemJ.Logger.Level.OFF;
	          case java.lang.SystemJ.Logger.Level.ERROR : return (int)java.lang.SystemJ.Logger.Level.ERROR;
	          case java.lang.SystemJ.Logger.Level.WARNING : return (int)java.lang.SystemJ.Logger.Level.WARNING;
	          case java.lang.SystemJ.Logger.Level.INFO : return (int)java.lang.SystemJ.Logger.Level.INFO;
	          case java.lang.SystemJ.Logger.Level.DEBUG : return (int)java.lang.SystemJ.Logger.Level.DEBUG;
	          case java.lang.SystemJ.Logger.Level.TRACE : return (int)java.lang.SystemJ.Logger.Level.TRACE;
	          case java.lang.SystemJ.Logger.Level.ALL : return (int)java.lang.SystemJ.Logger.Level.ALL;
	          default : return 0;
	        } 
	      }
	        
	      public static java.lang.SystemJ.Logger.Level valueOf (this java.lang.SystemJ.Logger.Level self, String name) {
	        if (null == name) throw new java.lang.NullPointerException();
	        switch (name){
	          case "OFF" : return java.lang.SystemJ.Logger.Level.OFF;
	          case "ERROR" : return java.lang.SystemJ.Logger.Level.ERROR;
	          case "WARNING" : return java.lang.SystemJ.Logger.Level.WARNING;
	          case "INFO" : return java.lang.SystemJ.Logger.Level.INFO;
	          case "DEBUG" : return java.lang.SystemJ.Logger.Level.DEBUG;
	          case "TRACE" : return java.lang.SystemJ.Logger.Level.TRACE;
	          case "ALL" : return java.lang.SystemJ.Logger.Level.ALL;
	          default : throw new java.lang.IllegalArgumentException("No enum value called "+name+" found.");
	        } 
          }
        
	      public static java.lang.SystemJ.Logger.Level[] values (this java.lang.SystemJ.Logger.Level self, String name) {
	        return new java.lang.SystemJ.Logger.Level[] {
	          java.lang.SystemJ.Logger.Level.OFF,
              java.lang.SystemJ.Logger.Level.ERROR,
	          java.lang.SystemJ.Logger.Level.WARNING,
	          java.lang.SystemJ.Logger.Level.INFO,
	          java.lang.SystemJ.Logger.Level.DEBUG,
	          java.lang.SystemJ.Logger.Level.TRACE,
	          java.lang.SystemJ.Logger.Level.ALL};
          }
        }
      
    }
}