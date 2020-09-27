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
using org.apache.commons.compress.compressors;

/*
 * This package is based on the work done by Keiron Liddle, Aftex Software
 * <keiron@aftexsw.com> to whom the Ant project is very grateful for his
 * great code.
 */
namespace org.apache.commons.compress.compressors.bzip2 {

    /**
     * An input stream that decompresses from the BZip2 format to be read as any other stream.
     * 
     * @NotThreadSafe
     */
    public class BZip2CompressorInputStream : CompressorInputStream{//, BZip2Constants {

        /**
         * Index of the last char in the block, so the block size == last + 1.
         */
        private int last;

        /**
         * Index in zptr[] of original string after sorting.
         */
        private int origPtr;

        /**
         * always: in the range 0 .. 9. The current block size is 100000 * this
         * number.
         */
        private int blockSize100k;

        private bool blockRandomised;

        private int bsBuff;
        private int bsLive;
        private CRC crc = new CRC();

        private int nInUse;

        private java.io.InputStream inJ;

        private int currentChar = -1;

        private const int EOF = 0;
        private const int START_BLOCK_STATE = 1;
        private const int RAND_PART_A_STATE = 2;
        private const int RAND_PART_B_STATE = 3;
        private const int RAND_PART_C_STATE = 4;
        private const int NO_RAND_PART_A_STATE = 5;
        private const int NO_RAND_PART_B_STATE = 6;
        private const int NO_RAND_PART_C_STATE = 7;

        private int currentState = START_BLOCK_STATE;

        private int storedBlockCRC, storedCombinedCRC;
        private int computedBlockCRC, computedCombinedCRC;

        // Variables used by setup* methods exclusively

        private int su_count;
        private int su_ch2;
        private int su_chPrev;
        private int su_i2;
        private int su_j2;
        private int su_rNToGo;
        private int su_rTPos;
        private int su_tPos;
        private char su_z;

        /**
         * All memory intensive stuff. This field is initialized by initBlock().
         */
        private DataI data;

        /**
         * Constructs a new BZip2CompressorInputStream which decompresses bytes read from the
         * specified stream.
         * 
         * @throws IOException
         *             if the stream content is malformed or an I/O error occurs.
         * @throws NullPointerException
         *             if <tt>in == null</tt>
         */
        public BZip2CompressorInputStream(java.io.InputStream input) : base() //throws IOException 
        {
            this.inJ = input;
            init();
        }

        /** {@inheritDoc} */
        public override int read() //throws IOException 
        {
            if (this.inJ != null) {
                return read0();
            } else {
                throw new java.io.IOException("stream closed");
            }
        }

        /*
         * (non-Javadoc)
         * 
         * @see java.io.InputStream#read(byte[], int, int)
         */
        public override int read(byte[] dest, int offs, int len)
            //throws IOException 
            {
            if (offs < 0) {
                throw new java.lang.IndexOutOfBoundsException("offs(" + offs + ") < 0.");
            }
            if (len < 0) {
                throw new java.lang.IndexOutOfBoundsException("len(" + len + ") < 0.");
            }
            if (offs + len > dest.Length) {
                throw new java.lang.IndexOutOfBoundsException("offs(" + offs + ") + len("
                                                    + len + ") > dest.length(" + dest.Length + ").");
            }
            if (this.inJ == null) {
                throw new java.io.IOException("stream closed");
            }

            int hi = offs + len;
            int destOffs = offs;
            for (int b; (destOffs < hi) && ((b = read0()) >= 0);) {
                dest[destOffs++] = (byte) b;
            }

            return (destOffs == offs) ? -1 : (destOffs - offs);
        }

        private void makeMaps() {
            bool[] inUse = this.data.inUse;
            byte[] seqToUnseq = this.data.seqToUnseq;

            int nInUseShadow = 0;

            for (int i = 0; i < 256; i++) {
                if (inUse[i])
                    seqToUnseq[nInUseShadow++] = (byte) i;
            }

            this.nInUse = nInUseShadow;
        }

        private int read0() //throws IOException 
        {
            int retChar = this.currentChar;

            switch (this.currentState) {
            case EOF:
                return -1;

            case START_BLOCK_STATE:
                throw new java.lang.IllegalStateException();

            case RAND_PART_A_STATE:
                throw new java.lang.IllegalStateException();

            case RAND_PART_B_STATE:
                setupRandPartB();
                break;

            case RAND_PART_C_STATE:
                setupRandPartC();
                break;

            case NO_RAND_PART_A_STATE:
                throw new java.lang.IllegalStateException();

            case NO_RAND_PART_B_STATE:
                setupNoRandPartB();
                break;

            case NO_RAND_PART_C_STATE:
                setupNoRandPartC();
                break;

            default:
                throw new java.lang.IllegalStateException();
            }

            return retChar;
        }

        private void init() //throws IOException 
        {
            if (null == inJ) {
                throw new java.io.IOException("No InputStream");
            }
            if (inJ.available() == 0) {
                throw new java.io.IOException("Empty InputStream");
            }
            checkMagicChar('B', "first");
            checkMagicChar('Z', "second");
            checkMagicChar('h', "third");

            int blockSize = this.inJ.read();
            if ((blockSize < '1') || (blockSize > '9')) {
                throw new java.io.IOException("Stream is not BZip2 formatted: illegal "+ "blocksize " + (char) blockSize);
            }

            this.blockSize100k = blockSize - '0';

            initBlock();
            setupBlock();
        }

        private void checkMagicChar(char expected, String position)
            //throws IOException 
            {
            int magic = this.inJ.read();
            if (magic != expected) {
                throw new java.io.IOException("Stream is not BZip2 formatted: expected '"
                                      + expected + "' as " + position + " byte but got '"
                                      + (char) magic + "'");
            }
        }

        private void initBlock() //throws IOException 
        {
            char magic0 = bsGetUByte();
            char magic1 = bsGetUByte();
            char magic2 = bsGetUByte();
            char magic3 = bsGetUByte();
            char magic4 = bsGetUByte();
            char magic5 = bsGetUByte();

            if (magic0 == 0x17 && magic1 == 0x72 && magic2 == 0x45
                && magic3 == 0x38 && magic4 == 0x50 && magic5 == 0x90) {
                complete(); // end of file
            } else if (magic0 != 0x31 || // '1'
                       magic1 != 0x41 || // ')'
                       magic2 != 0x59 || // 'Y'
                       magic3 != 0x26 || // '&'
                       magic4 != 0x53 || // 'S'
                       magic5 != 0x59 // 'Y'
                       ) {
                this.currentState = EOF;
                throw new java.io.IOException("bad block header");
            } else {
                this.storedBlockCRC = bsGetInt();
                this.blockRandomised = bsR(1) == 1;

                /**
                 * Allocate data here instead in constructor, so we do not allocate
                 * it if the input file is empty.
                 */
                if (this.data == null) {
                    this.data = new DataI(this.blockSize100k);
                }

                // currBlockNo++;
                getAndMoveToFrontDecode();

                this.crc.initialiseCRC();
                this.currentState = START_BLOCK_STATE;
            }
        }

        private void endBlock() //throws IOException 
        {
            this.computedBlockCRC = this.crc.getFinalCRC();

            // A bad CRC is considered a fatal error.
            if (this.storedBlockCRC != this.computedBlockCRC) {
                // make next blocks readable without error
                // (repair feature, not yet documented, not tested)
                this.computedCombinedCRC = (this.storedCombinedCRC << 1)
                    | (java.dotnet.lang.Operator.shiftRightUnsignet (this.storedCombinedCRC, 31));
                this.computedCombinedCRC ^= this.storedBlockCRC;

                throw new java.io.IOException("BZip2 CRC error");
            }

            this.computedCombinedCRC = (this.computedCombinedCRC << 1)
                | (java.dotnet.lang.Operator.shiftRightUnsignet (this.storedCombinedCRC, 31));
            this.computedCombinedCRC ^= this.computedBlockCRC;
        }

        private void complete() //throws IOException 
        {
            this.storedCombinedCRC = bsGetInt();
            this.currentState = EOF;
            this.data = null;

            if (this.storedCombinedCRC != this.computedCombinedCRC) {
                throw new java.io.IOException("BZip2 CRC error");
            }
        }

        public override void close() //throws IOException 
        {
            java.io.InputStream inShadow = this.inJ;
            if (inShadow != null) {
                try {
                    if (inShadow != java.lang.SystemJ.inJ) {
                        inShadow.close();
                    }
                } finally {
                    this.data = null;
                    this.inJ = null;
                }
            }
        }

        private int bsR(int n) //throws IOException 
        {
            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            if (bsLiveShadow < n) {
                java.io.InputStream inShadow = this.inJ;
                do {
                    int thech = inShadow.read();

                    if (thech < 0) {
                        throw new java.io.IOException("unexpected end of stream");
                    }

                    bsBuffShadow = (bsBuffShadow << 8) | thech;
                    bsLiveShadow += 8;
                } while (bsLiveShadow < n);

                this.bsBuff = bsBuffShadow;
            }

            this.bsLive = bsLiveShadow - n;
            return (bsBuffShadow >> (bsLiveShadow - n)) & ((1 << n) - 1);
        }

        private bool bsGetBit() //throws IOException 
        {
            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            if (bsLiveShadow < 1) {
                int thech = this.inJ.read();

                if (thech < 0) {
                    throw new java.io.IOException("unexpected end of stream");
                }

                bsBuffShadow = (bsBuffShadow << 8) | thech;
                bsLiveShadow += 8;
                this.bsBuff = bsBuffShadow;
            }

            this.bsLive = bsLiveShadow - 1;
            return ((bsBuffShadow >> (bsLiveShadow - 1)) & 1) != 0;
        }

        private char bsGetUByte() //throws IOException 
        {
            return (char) bsR(8);
        }

        private int bsGetInt() //throws IOException 
        {
            return (((((bsR(8) << 8) | bsR(8)) << 8) | bsR(8)) << 8) | bsR(8);
        }

        /**
         * Called by createHuffmanDecodingTables() exclusively.
         */
        private static void hbCreateDecodeTables(int[] limit,
                                                 int[] baseJ, int[] perm, char[] length,
                                                 int minLen, int maxLen, int alphaSize) {
            for (int i = minLen, pp = 0; i <= maxLen; i++) {
                for (int j = 0; j < alphaSize; j++) {
                    if (length[j] == i) {
                        perm[pp++] = j;
                    }
                }
            }

            for (int i = BZip2Constants.MAX_CODE_LEN; --i > 0; )
            {
                baseJ[i] = 0;
                limit[i] = 0;
            }

            for (int i = 0; i < alphaSize; i++) {
                baseJ[length[i] + 1]++;
            }

            for (int i = 1, b = baseJ[0]; i < BZip2Constants.MAX_CODE_LEN; i++)
            {
                b += baseJ[i];
                baseJ[i] = b;
            }

            for (int i = minLen, vec = 0, b = baseJ[i]; i <= maxLen; i++) {
                int nb = baseJ[i + 1];
                vec += nb - b;
                b = nb;
                limit[i] = vec - 1;
                vec <<= 1;
            }

            for (int i = minLen + 1; i <= maxLen; i++) {
                baseJ[i] = ((limit[i - 1] + 1) << 1) - baseJ[i];
            }
        }

        private void recvDecodingTables() //throws IOException 
        {
            DataI dataShadow = this.data;
            bool[] inUse = dataShadow.inUse;
            byte[] pos = dataShadow.recvDecodingTables_pos;
            byte[] selector = dataShadow.selector;
            byte[] selectorMtf = dataShadow.selectorMtf;

            int inUse16 = 0;

            /* Receive the mapping table */
            for (int i = 0; i < 16; i++) {
                if (bsGetBit()) {
                    inUse16 |= 1 << i;
                }
            }

            for (int i = 256; --i >= 0;) {
                inUse[i] = false;
            }

            for (int i = 0; i < 16; i++) {
                if ((inUse16 & (1 << i)) != 0) {
                    int i16 = i << 4;
                    for (int j = 0; j < 16; j++) {
                        if (bsGetBit()) {
                            inUse[i16 + j] = true;
                        }
                    }
                }
            }

            makeMaps();
            int alphaSize = this.nInUse + 2;

            /* Now the selectors */
            int nGroups = bsR(3);
            int nSelectors = bsR(15);

            for (int i = 0; i < nSelectors; i++) {
                int j = 0;
                while (bsGetBit()) {
                    j++;
                }
                selectorMtf[i] = (byte) j;
            }

            /* Undo the MTF values for the selectors. */
            for (int v = nGroups; --v >= 0;) {
                pos[v] = (byte) v;
            }

            for (int i = 0; i < nSelectors; i++) {
                int v = selectorMtf[i] & 0xff;
                byte tmp = pos[v];
                while (v > 0) {
                    // nearly all times v is zero, 4 in most other cases
                    pos[v] = pos[v - 1];
                    v--;
                }
                pos[0] = tmp;
                selector[i] = tmp;
            }

            char[,] len = dataShadow.temp_charArray2d;

            /* Now the coding tables */
            for (int t = 0; t < nGroups; t++) {
                int curr = bsR(5);
                char[] len_t = java.util.Arrays<char>.getIndexArray(len,t);// len[t];
                for (int i = 0; i < alphaSize; i++) {
                    while (bsGetBit()) {
                        curr += bsGetBit() ? -1 : 1;
                    }
                    len_t[i] = (char) curr;
                }
            }

            // finally create the Huffman tables
            createHuffmanDecodingTables(alphaSize, nGroups);
        }

        /**
         * Called by recvDecodingTables() exclusively.
         */
        private void createHuffmanDecodingTables(int alphaSize,
                                                 int nGroups) {
            DataI dataShadow = this.data;
            char[,] len = dataShadow.temp_charArray2d;
            int[] minLens = dataShadow.minLens;
            int[,] limit = dataShadow.limit;
            int[,] baseJ = dataShadow.baseJ;
            int[,] perm = dataShadow.perm;

            for (int t = 0; t < nGroups; t++) {
                int minLen = 32;
                int maxLen = 0;
                char[] len_t = java.util.Arrays<char>.getIndexArray(len,t);//len[t];
                for (int i = alphaSize; --i >= 0;) {
                    char lent = len_t[i];
                    if (lent > maxLen) {
                        maxLen = lent;
                    }
                    if (lent < minLen) {
                        minLen = lent;
                    }
                }
                hbCreateDecodeTables(
                    java.util.Arrays<int>.getIndexArray(limit,t),//limit[t], 
                    java.util.Arrays<int>.getIndexArray(baseJ,t),//baseJ[t], 
                    java.util.Arrays<int>.getIndexArray(perm,t),//perm[t], 
                    java.util.Arrays<char>.getIndexArray(len,t),//len[t], 
                    minLen,
                    maxLen, 
                    alphaSize);
                minLens[t] = minLen;
            }
        }

        private void getAndMoveToFrontDecode() //throws IOException 
        {
            this.origPtr = bsR(24);
            recvDecodingTables();

            java.io.InputStream inShadow = this.inJ;
            DataI dataShadow = this.data;
            byte[] ll8 = dataShadow.ll8;
            int[] unzftab = dataShadow.unzftab;
            byte[] selector = dataShadow.selector;
            byte[] seqToUnseq = dataShadow.seqToUnseq;
            char[] yy = dataShadow.getAndMoveToFrontDecode_yy;
            int[] minLens = dataShadow.minLens;
            int[,] limit = dataShadow.limit;
            int[,] baseJ = dataShadow.baseJ;
            int[,] perm = dataShadow.perm;
            int limitLast = this.blockSize100k * 100000;

            /*
             * Setting up the unzftab entries here is not strictly necessary, but it
             * does save having to do it later in a separate pass, and so saves a
             * block's worth of cache misses.
             */
            for (int i = 256; --i >= 0;) {
                yy[i] = (char) i;
                unzftab[i] = 0;
            }

            int groupNo = 0;
            int groupPos = BZip2Constants.G_SIZE - 1;
            int eob = this.nInUse + 1;
            int nextSym = getAndMoveToFrontDecode0(0);
            int bsBuffShadow = this.bsBuff;
            int bsLiveShadow = this.bsLive;
            int lastShadow = -1;
            int zt = selector[groupNo] & 0xff;
            int[] base_zt = java.util.Arrays<int>.getIndexArray(baseJ,zt);// baseJ[zt];
            int[] limit_zt = java.util.Arrays<int>.getIndexArray(limit,zt);// limit[zt];
            int[] perm_zt = java.util.Arrays<int>.getIndexArray(perm,zt);// perm[zt];
            int minLens_zt = minLens[zt];

            while (nextSym != eob) {
                if ((nextSym == BZip2Constants.RUNA) || (nextSym == BZip2Constants.RUNB))
                {
                    int s = -1;

                    for (int n = 1; true; n <<= 1) {
                        if (nextSym == BZip2Constants.RUNA)
                        {
                            s += n;
                        }
                        else if (nextSym == BZip2Constants.RUNB)
                        {
                            s += n << 1;
                        } else {
                            break;
                        }
                        if (groupNo == 18000 && groupPos == 0)
                        {
                            Console.Beep();
                        }
                        if (groupPos == 0) {
                            groupPos = BZip2Constants.G_SIZE - 1;
                            zt = selector[++groupNo] & 0xff;
                            base_zt = java.util.Arrays<int>.getIndexArray(baseJ,zt);// baseJ[zt];
                            limit_zt = java.util.Arrays<int>.getIndexArray(limit,zt);// limit[zt];
                            perm_zt = java.util.Arrays<int>.getIndexArray(perm,zt);// perm[zt];
                            minLens_zt = minLens[zt];
                        } else {
                            groupPos--;
                        }

                        int zn = minLens_zt;

                        // Inlined:
                        // int zvec = bsR(zn);
                        while (bsLiveShadow < zn) {
                            int thech = inShadow.read();
                            if (thech >= 0) {
                                bsBuffShadow = (bsBuffShadow << 8) | thech;
                                bsLiveShadow += 8;
                                continue;
                            } else {
                                throw new java.io.IOException("unexpected end of stream");
                            }
                        }
                        int zvec = (bsBuffShadow >> (bsLiveShadow - zn))
                            & ((1 << zn) - 1);
                        bsLiveShadow -= zn;

                        while (zvec > limit_zt[zn]) {
                            zn++;
                            while (bsLiveShadow < 1) {
                                int thech = inShadow.read();
                                if (thech >= 0) {
                                    bsBuffShadow = (bsBuffShadow << 8) | thech;
                                    bsLiveShadow += 8;
                                    continue;
                                } else {
                                    throw new java.io.IOException(
                                                          "unexpected end of stream");
                                }
                            }
                            bsLiveShadow--;
                            zvec = (zvec << 1)
                                | ((bsBuffShadow >> bsLiveShadow) & 1);
                        }
                        nextSym = perm_zt[zvec - base_zt[zn]];
                    }

                    byte ch = seqToUnseq[yy[0]];
                    unzftab[ch & 0xff] += s + 1;

                    while (s-- >= 0) {
                        ll8[++lastShadow] = ch;
                    }

                    if (lastShadow >= limitLast) {
                        throw new java.io.IOException("block overrun");
                    }
                } else {
                    if (++lastShadow >= limitLast) {
                        throw new java.io.IOException("block overrun");
                    }

                    char tmp = yy[nextSym - 1];
                    unzftab[seqToUnseq[tmp] & 0xff]++;
                    ll8[lastShadow] = seqToUnseq[tmp];

                    /*
                     * This loop is hammered during decompression, hence avoid
                     * native method call overhead of System.arraycopy for very
                     * small ranges to copy.
                     */
                    if (nextSym <= 16) {
                        for (int j = nextSym - 1; j > 0;) {
                            yy[j] = yy[--j];
                        }
                    } else {
                        java.lang.SystemJ.arraycopy(yy, 0, yy, 1, nextSym - 1);
                    }

                    yy[0] = tmp;

                    if (groupPos == 0) {
                        groupPos = BZip2Constants.G_SIZE - 1;
                        zt = selector[++groupNo] & 0xff;
                        base_zt = java.util.Arrays<int>.getIndexArray(baseJ,zt);// baseJ[zt];
                        limit_zt = java.util.Arrays<int>.getIndexArray(limit,zt);//limit[zt];
                        perm_zt = java.util.Arrays<int>.getIndexArray(perm,zt);//perm[zt];
                        minLens_zt = minLens[zt];
                    } else {
                        groupPos--;
                    }

                    int zn = minLens_zt;

                    // Inlined:
                    // int zvec = bsR(zn);
                    while (bsLiveShadow < zn) {
                        int thech = inShadow.read();
                        if (thech >= 0) {
                            bsBuffShadow = (bsBuffShadow << 8) | thech;
                            bsLiveShadow += 8;
                            continue;
                        } else {
                            throw new java.io.IOException("unexpected end of stream");
                        }
                    }
                    int zvec = (bsBuffShadow >> (bsLiveShadow - zn))
                        & ((1 << zn) - 1);
                    bsLiveShadow -= zn;

                    while (zvec > limit_zt[zn]) {
                        zn++;
                        while (bsLiveShadow < 1) {
                            int thech = inShadow.read();
                            if (thech >= 0) {
                                bsBuffShadow = (bsBuffShadow << 8) | thech;
                                bsLiveShadow += 8;
                                continue;
                            } else {
                                throw new java.io.IOException("unexpected end of stream");
                            }
                        }
                        bsLiveShadow--;
                        zvec = (zvec << 1) | ((bsBuffShadow >> bsLiveShadow) & 1);
                    }
                    nextSym = perm_zt[zvec - base_zt[zn]];
                }
            }

            this.last = lastShadow;
            this.bsLive = bsLiveShadow;
            this.bsBuff = bsBuffShadow;
        }

        private int getAndMoveToFrontDecode0(int groupNo) //throws IOException 
        {
            java.io.InputStream inShadow = this.inJ;
            DataI dataShadow = this.data;
            int zt = dataShadow.selector[groupNo] & 0xff;
            int[] limit_zt = java.util.Arrays<int>.getIndexArray(dataShadow.limit,zt); ;// dataShadow.limit[zt];
            int zn = dataShadow.minLens[zt];
            int zvec = bsR(zn);
            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            while (zvec > limit_zt[zn]) {
                zn++;
                while (bsLiveShadow < 1) {
                    int thech = inShadow.read();

                    if (thech >= 0) {
                        bsBuffShadow = (bsBuffShadow << 8) | thech;
                        bsLiveShadow += 8;
                        continue;
                    } else {
                        throw new java.io.IOException("unexpected end of stream");
                    }
                }
                bsLiveShadow--;
                zvec = (zvec << 1) | ((bsBuffShadow >> bsLiveShadow) & 1);
            }

            this.bsLive = bsLiveShadow;
            this.bsBuff = bsBuffShadow;

            return dataShadow.perm[zt,zvec - dataShadow.baseJ[zt,zn]];
        }

        private void setupBlock() //throws IOException 
        {
            if (this.data == null) {
                return;
            }

            int[] cftab = this.data.cftab;
            int[] tt = this.data.initTT(this.last + 1);
            byte[] ll8 = this.data.ll8;
            cftab[0] = 0;
            java.lang.SystemJ.arraycopy(this.data.unzftab, 0, cftab, 1, 256);

            for (int i = 1, c = cftab[0]; i <= 256; i++) {
                c += cftab[i];
                cftab[i] = c;
            }

            for (int i = 0, lastShadow = this.last; i <= lastShadow; i++) {
                tt[cftab[ll8[i] & 0xff]++] = i;
            }

            if ((this.origPtr < 0) || (this.origPtr >= tt.Length)) {
                throw new java.io.IOException("stream corrupted");
            }

            this.su_tPos = tt[this.origPtr];
            this.su_count = 0;
            this.su_i2 = 0;
            this.su_ch2 = 256; /* not a char and not EOF */

            if (this.blockRandomised) {
                this.su_rNToGo = 0;
                this.su_rTPos = 0;
                setupRandPartA();
            } else {
                setupNoRandPartA();
            }
        }

        private void setupRandPartA() //throws IOException 
        {
            if (this.su_i2 <= this.last) {
                this.su_chPrev = this.su_ch2;
                int su_ch2Shadow = this.data.ll8[this.su_tPos] & 0xff;
                this.su_tPos = this.data.tt[this.su_tPos];
                if (this.su_rNToGo == 0) {
                    this.su_rNToGo = Rand.rNums(this.su_rTPos) - 1;
                    if (++this.su_rTPos == 512) {
                        this.su_rTPos = 0;
                    }
                } else {
                    this.su_rNToGo--;
                }
                this.su_ch2 = su_ch2Shadow ^= (this.su_rNToGo == 1) ? 1 : 0;
                this.su_i2++;
                this.currentChar = su_ch2Shadow;
                this.currentState = RAND_PART_B_STATE;
                this.crc.updateCRC(su_ch2Shadow);
            } else {
                endBlock();
                initBlock();
                setupBlock();
            }
        }

        private void setupNoRandPartA() //throws IOException 
        {
            if (this.su_i2 <= this.last) {
                this.su_chPrev = this.su_ch2;
                int su_ch2Shadow = this.data.ll8[this.su_tPos] & 0xff;
                this.su_ch2 = su_ch2Shadow;
                this.su_tPos = this.data.tt[this.su_tPos];
                this.su_i2++;
                this.currentChar = su_ch2Shadow;
                this.currentState = NO_RAND_PART_B_STATE;
                this.crc.updateCRC(su_ch2Shadow);
            } else {
                this.currentState = NO_RAND_PART_A_STATE;
                endBlock();
                initBlock();
                setupBlock();
            }
        }

        private void setupRandPartB() //throws IOException 
        {
            if (this.su_ch2 != this.su_chPrev) {
                this.currentState = RAND_PART_A_STATE;
                this.su_count = 1;
                setupRandPartA();
            } else if (++this.su_count >= 4) {
                this.su_z = (char) (this.data.ll8[this.su_tPos] & 0xff);
                this.su_tPos = this.data.tt[this.su_tPos];
                if (this.su_rNToGo == 0) {
                    this.su_rNToGo = Rand.rNums(this.su_rTPos) - 1;
                    if (++this.su_rTPos == 512) {
                        this.su_rTPos = 0;
                    }
                } else {
                    this.su_rNToGo--;
                }
                this.su_j2 = 0;
                this.currentState = RAND_PART_C_STATE;
                if (this.su_rNToGo == 1) {
                    this.su_z ^= (char)1;
                }
                setupRandPartC();
            } else {
                this.currentState = RAND_PART_A_STATE;
                setupRandPartA();
            }
        }

        private void setupRandPartC() //throws IOException 
        {
            if (this.su_j2 < this.su_z) {
                this.currentChar = this.su_ch2;
                this.crc.updateCRC(this.su_ch2);
                this.su_j2++;
            } else {
                this.currentState = RAND_PART_A_STATE;
                this.su_i2++;
                this.su_count = 0;
                setupRandPartA();
            }
        }

        private void setupNoRandPartB() //throws IOException 
        {
            if (this.su_ch2 != this.su_chPrev) {
                this.su_count = 1;
                setupNoRandPartA();
            } else if (++this.su_count >= 4) {
                this.su_z = (char) (this.data.ll8[this.su_tPos] & 0xff);
                this.su_tPos = this.data.tt[this.su_tPos];
                this.su_j2 = 0;
                setupNoRandPartC();
            } else {
                setupNoRandPartA();
            }
        }

        private void setupNoRandPartC() //throws IOException 
        {
            if (this.su_j2 < this.su_z) {
                int su_ch2Shadow = this.su_ch2;
                this.currentChar = su_ch2Shadow;
                this.crc.updateCRC(su_ch2Shadow);
                this.su_j2++;
                this.currentState = NO_RAND_PART_C_STATE;
            } else {
                this.su_i2++;
                this.su_count = 0;
                setupNoRandPartA();
            }
        }

        /**
         * Checks if the signature matches what is expected for a bzip2 file.
         * 
         * @param signature
         *            the bytes to check
         * @param length
         *            the number of bytes to check
         * @return true, if this stream is a bzip2 compressed stream, false otherwise
         * 
         * @since Apache Commons Compress 1.1
         */
        public static bool matches(byte[] signature, int length) {

            if (length < 3) {
                return false;
            }
        
            if (signature[0] != 'B') {
                return false;
            }

            if (signature[1] != 'Z') {
                return false;
            }

            if (signature[2] != 'h') {
                return false;
            }
        
            return true;
        }
    }
#region Data
    internal sealed class DataI {

        // (with blockSize 900k)
        internal readonly bool[] inUse = new bool[256]; // 256 byte

        internal readonly byte[] seqToUnseq = new byte[256]; // 256 byte
        internal readonly byte[] selector = new byte[BZip2Constants.MAX_SELECTORS]; // 18002 byte
        internal readonly byte[] selectorMtf = new byte[BZip2Constants.MAX_SELECTORS]; // 18002 byte

        /**
            * Freq table collected to save a pass over the data during
            * decompression.
            */
        internal readonly int[] unzftab = new int[256]; // 1024 byte

        internal readonly int[,] limit = new int[BZip2Constants.N_GROUPS,BZip2Constants.MAX_ALPHA_SIZE]; // 6192 byte
        internal readonly int[,] baseJ = new int[BZip2Constants.N_GROUPS,BZip2Constants.MAX_ALPHA_SIZE]; // 6192 byte
        internal readonly int[,] perm = new int[BZip2Constants.N_GROUPS,BZip2Constants.MAX_ALPHA_SIZE]; // 6192 byte
        internal readonly int[] minLens = new int[BZip2Constants.N_GROUPS]; // 24 byte

        internal readonly int[] cftab = new int[257]; // 1028 byte
        internal readonly char[] getAndMoveToFrontDecode_yy = new char[256]; // 512 byte
        internal readonly char[,] temp_charArray2d = new char[BZip2Constants.N_GROUPS,BZip2Constants.MAX_ALPHA_SIZE]; // 3096
        // byte
        internal readonly byte[] recvDecodingTables_pos = new byte[BZip2Constants.N_GROUPS]; // 6 byte
        // ---------------
        // 60798 byte

        internal int[] tt; // 3600000 byte
        internal byte[] ll8; // 900000 byte

        // ---------------
        // 4560782 byte
        // ===============

        internal DataI(int blockSize100k) {
            this.ll8 = new byte[blockSize100k * BZip2Constants.BASEBLOCKSIZE];
        }

        /**
            * Initializes the {@link #tt} array.
            * 
            * This method is called when the required length of the array is known.
            * I don't initialize it at construction time to avoid unneccessary
            * memory allocation when compressing small files.
            */
        internal int[] initTT(int length) {
            int[] ttShadow = this.tt;

            // tt.length should always be >= length, but theoretically
            // it can happen, if the compressor mixed small and large
            // blocks. Normally only the last block will be smaller
            // than others.
            if ((ttShadow == null) || (ttShadow.Length < length)) {
                this.tt = ttShadow = new int[length];
            }

            return ttShadow;
        }

    }
#endregion
}