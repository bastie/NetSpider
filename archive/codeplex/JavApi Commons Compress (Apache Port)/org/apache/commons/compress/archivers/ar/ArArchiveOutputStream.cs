/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using java = biz.ritter.javapi;
using org.apache.commons.compress.archivers;
using org.apache.commons.compress.utils;

namespace org.apache.commons.compress.archivers.ar {


    /**
     * Implements the "ar" archive format as an output stream.
     * 
     * @NotThreadSafe
     */
    public class ArArchiveOutputStream : ArchiveOutputStream {

        private readonly java.io.OutputStream outJ;
        private long archiveOffset = 0;
        private long entryOffset = 0;
        private ArArchiveEntry prevEntry;
        private bool haveUnclosedEntry = false;
    
        /** indicates if this archive is finished */
        private bool finished = false;

        public ArArchiveOutputStream( java.io.OutputStream pOut ) {
            this.outJ = pOut;
        }

        private long writeArchiveHeader() //throws IOException 
        {
            byte [] header = ArchiveUtils.toAsciiBytes(ArArchiveEntry.HEADER);
            outJ.write(header);
            return header.Length;
        }

        /** {@inheritDoc} */
        public override void closeArchiveEntry()// throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            if (prevEntry == null || !haveUnclosedEntry){
                throw new java.io.IOException("No current entry to close");
            }
            if ((entryOffset % 2) != 0) {
                outJ.write('\n'); // Pad byte
                archiveOffset++;
            }
            haveUnclosedEntry = false;
        }

        /** {@inheritDoc} */
        public override void putArchiveEntry( ArchiveEntry pEntry ) //throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
        
            ArArchiveEntry pArEntry = (ArArchiveEntry)pEntry;
            if (prevEntry == null) {
                archiveOffset += writeArchiveHeader();
            } else {
                if (prevEntry.getLength() != entryOffset) {
                    throw new java.io.IOException("length does not match entry (" + prevEntry.getLength() + " != " + entryOffset);
                }

                if (haveUnclosedEntry) {
                    closeArchiveEntry();
                }
            }

            prevEntry = pArEntry;

            archiveOffset += writeEntryHeader(pArEntry);

            entryOffset = 0;
            haveUnclosedEntry = true;
        }

        private long fill( long pOffset, long pNewOffset, char pFill ) //throws IOException 
        { 
            long diff = pNewOffset - pOffset;

            if (diff > 0) {
                for (int i = 0; i < diff; i++) {
                    write(pFill);
                }
            }

            return pNewOffset;
        }

        private long write( String data ) //throws IOException 
        {
            byte[] bytes = data.getBytes("ASCII");
            write(bytes);
            return bytes.Length;
        }

        private long writeEntryHeader( ArArchiveEntry pEntry ) //throws IOException 
        {

            long offset = 0;

            String n = pEntry.getName();
            if (n.length() > 16) {
                throw new java.io.IOException("filename too long, > 16 chars: "+n);
            }
            offset += write(n);

            offset = fill(offset, 16, ' ');
            String m = "" + (pEntry.getLastModified());
            if (m.length() > 12) {
                throw new java.io.IOException("modified too long");
            }
            offset += write(m);

            offset = fill(offset, 28, ' ');
            String u = "" + pEntry.getUserId();
            if (u.length() > 6) {
                throw new java.io.IOException("userid too long");
            }
            offset += write(u);

            offset = fill(offset, 34, ' ');
            String g = "" + pEntry.getGroupId();
            if (g.length() > 6) {
                throw new java.io.IOException("groupid too long");
            }
            offset += write(g);

            offset = fill(offset, 40, ' ');
            String fm = java.lang.Integer.toString(pEntry.getMode(), 8).ToString();
            if (fm.length() > 8) {
                throw new java.io.IOException("filemode too long");
            }
            offset += write(fm);

            offset = fill(offset, 48, ' ');
            String s = "" + pEntry.getLength();
            if (s.length() > 10) {
                throw new java.io.IOException("size too long");
            }
            offset += write(s);

            offset = fill(offset, 58, ' ');

            offset += write(ArArchiveEntry.TRAILER);

            return offset;
        }

        public override void write(byte[] b, int off, int len) //throws IOException 
        {
            outJ.write(b, off, len);
            count(len);
            entryOffset += len;
        }

        /**
         * Calls finish if necessary, and then closes the OutputStream
         */
        public override void close() //throws IOException 
        {
            if(!finished) {
                finish();
            }
            outJ.close();
            prevEntry = null;
        }

        /** {@inheritDoc} */
        public override ArchiveEntry createArchiveEntry(java.io.File inputFile, String entryName)
                //throws IOException 
        {
            if(finished) {
                throw new java.io.IOException("Stream has already been finished");
            }
            return new ArArchiveEntry(inputFile, entryName);
        }

        /** {@inheritDoc} */
        public override void finish() //throws IOException 
        {
            if(haveUnclosedEntry) {
                throw new java.io.IOException("This archive contains unclosed entries.");
            } else if(finished) {
                throw new java.io.IOException("This archive has already been finished");
            }
            finished = true;
        }
    }
}