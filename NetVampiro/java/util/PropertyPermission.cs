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

namespace biz.ritter.javapi.util
{
    
    public sealed class PropertyPermission : java.security.BasicPermission {
    
      public PropertyPermission (String name, String actions) : base (name, actions){
        //if (null == name) throw new java.lang.NullPointerException ("PropertyPermission with null as name not allowed");
        if (null == actions) throw new java.lang.IllegalArgumentException ("PropertyPermission need actions");
        // read,write or write,read or read,write,read,write,write or write or read but not read,write,
        
        actions = actions.trim();
        {/* jdk compatibility hint */
          if (actions.StartsWith(",") || actions.EndsWith(","))
            throw new java.lang.IllegalArgumentException ("Unsupported property action");
        }
        
        bool isReadActionWanted = false;
        bool isWriteActionWanted = false;
        String [] action = actions.Split(",");
        Array.Sort (action,StringComparer.InvariantCulture); // read before write
        foreach (String wantedAction in action) {
          switch (wantedAction) {
          case "read" : isReadActionWanted = true; break;
          case "write": isWriteActionWanted = true; break;
          default:
            throw new java.lang.IllegalArgumentException ("Unsupported property action");
          }
        }
        
        if (isReadActionWanted && isWriteActionWanted) {
          actions = "read,write";
        }
        else if (isReadActionWanted) {
          actions = "read";
        }
        else /* no read && no write throws Exception above */{
          actions ="write";
        }
      }
      
      
      private readonly String actions;
      
      public override String getActions(){
            return ""; //$NON-NLS-1$
      }
      
    } 
}
