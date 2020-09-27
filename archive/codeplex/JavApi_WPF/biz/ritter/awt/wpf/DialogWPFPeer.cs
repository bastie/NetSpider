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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.awt.wpf
{
    public class DialogWPFPeer : System.Windows.Window, java.awt.peer.DialogPeer
    {
        /// <summary>
        /// Set the title of frame
        /// </summary>
        /// <param name="newTitle">title</param>
        public virtual void setTitle(String newTitle)
        {
            this.Title = newTitle;
        }
        /// <summary>
        /// Display or hides this frame and contained child GUI element.
        /// </summary>
        /// <param name="showMe">true if to display, false if to hide</param>
        public virtual void setVisible(bool showMe)
        {
            this.Visibility = showMe ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        public void show()
        {
            this.setVisible(true);
        }
        public int getHeight()
        {
            return (int)this.Height;
        }
        public int getWidth()
        {
            return (int)this.Width;
        }
        public int getX()
        {
            return (int)this.Left;
        }
        public int getY()
        {
            return (int)this.Top;
        }
    }
}
