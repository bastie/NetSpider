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

namespace org.apache.commons.compress.compressors.bzip2 {

    /**
     * An output stream that compresses into the BZip2 format into another stream.
     *
     * <p>
     * The compression requires large amounts of memory. Thus you should call the
     * {@link #close() close()} method as soon as possible, to force
     * <tt>BZip2CompressorOutputStream</tt> to release the allocated memory.
     * </p>
     *
     * <p> You can shrink the amount of allocated memory and maybe raise
     * the compression speed by choosing a lower blocksize, which in turn
     * may cause a lower compression ratio. You can avoid unnecessary
     * memory allocation by avoiding using a blocksize which is bigger
     * than the size of the input.  </p>
     *
     * <p> You can compute the memory usage for compressing by the
     * following formula: </p>
     *
     * <pre>
     * &lt;code&gt;400k + (9 * blocksize)&lt;/code&gt;.
     * </pre>
     *
     * <p> To get the memory required for decompression by {@link
     * BZip2CompressorInputStream} use </p>
     *
     * <pre>
     * &lt;code&gt;65k + (5 * blocksize)&lt;/code&gt;.
     * </pre>
     *
     * <table width="100%" border="1">
     * <colgroup> <col width="33%" /> <col width="33%" /> <col width="33%" />
     * </colgroup>
     * <tr>
     * <th colspan="3">Memory usage by blocksize</th>
     * </tr>
     * <tr>
     * <th align="right">Blocksize</th> <th align="right">Compression<br>
     * memory usage</th> <th align="right">Decompression<br>
     * memory usage</th>
     * </tr>
     * <tr>
     * <td align="right">100k</td>
     * <td align="right">1300k</td>
     * <td align="right">565k</td>
     * </tr>
     * <tr>
     * <td align="right">200k</td>
     * <td align="right">2200k</td>
     * <td align="right">1065k</td>
     * </tr>
     * <tr>
     * <td align="right">300k</td>
     * <td align="right">3100k</td>
     * <td align="right">1565k</td>
     * </tr>
     * <tr>
     * <td align="right">400k</td>
     * <td align="right">4000k</td>
     * <td align="right">2065k</td>
     * </tr>
     * <tr>
     * <td align="right">500k</td>
     * <td align="right">4900k</td>
     * <td align="right">2565k</td>
     * </tr>
     * <tr>
     * <td align="right">600k</td>
     * <td align="right">5800k</td>
     * <td align="right">3065k</td>
     * </tr>
     * <tr>
     * <td align="right">700k</td>
     * <td align="right">6700k</td>
     * <td align="right">3565k</td>
     * </tr>
     * <tr>
     * <td align="right">800k</td>
     * <td align="right">7600k</td>
     * <td align="right">4065k</td>
     * </tr>
     * <tr>
     * <td align="right">900k</td>
     * <td align="right">8500k</td>
     * <td align="right">4565k</td>
     * </tr>
     * </table>
     *
     * <p>
     * For decompression <tt>BZip2CompressorInputStream</tt> allocates less memory if the
     * bzipped input is smaller than one block.
     * </p>
     *
     * <p>
     * Instances of this class are not threadsafe.
     * </p>
     *
     * <p>
     * TODO: Update to BZip2 1.0.1
     * </p>
     * @NotThreadSafe
     */
    public class BZip2CompressorOutputStream : CompressorOutputStream//, BZip2Constants
    {

        /**
         * The minimum supported blocksize <tt> == 1</tt>.
         */
        public static readonly int MIN_BLOCKSIZE = 1;

        /**
         * The maximum supported blocksize <tt> == 9</tt>.
         */
        public static readonly int MAX_BLOCKSIZE = 9;

        private static readonly int SETMASK = (1 << 21);
        private static readonly int CLEARMASK = (~SETMASK);
        private static readonly int GREATER_ICOST = 15;
        private static readonly int LESSER_ICOST = 0;
        private static readonly int SMALL_THRESH = 20;
        private static readonly int DEPTH_THRESH = 10;
        private static readonly int WORK_FACTOR = 30;

        /*
         * <p> If you are ever unlucky/improbable enough to get a stack
         * overflow whilst sorting, increase the following constant and
         * try again. In practice I have never seen the stack go above 27
         * elems, so the following limit seems very generous.  </p>
         */
        protected internal static readonly int QSORT_STACK_SIZE = 1000;

        /**
         * Knuth's increments seem to work better than Incerpi-Sedgewick here.
         * Possibly because the number of elems to sort is usually small, typically
         * &lt;= 20.
         */
        private static readonly int[] INCS = { 1, 4, 13, 40, 121, 364, 1093, 3280,
                                            9841, 29524, 88573, 265720, 797161,
                                            2391484 };

        private static void hbMakeCodeLengths(byte[] len, int[] freq,
                                              Data dat, int alphaSize,
                                              int maxLen)
        {
            /*
             * Nodes and heap entries run from 1. Entry 0 for both the heap and
             * nodes is a sentinel.
             */
            int[] heap = Data.heap;
            int[] weight = Data.weight;
            int[] parent = Data.parent;

            for (int i = alphaSize; --i >= 0; )
            {
                weight[i + 1] = (freq[i] == 0 ? 1 : freq[i]) << 8;
            }

            for (bool tooLong = true; tooLong; )
            {
                tooLong = false;

                int nNodes = alphaSize;
                int nHeap = 0;
                heap[0] = 0;
                weight[0] = 0;
                parent[0] = -2;

                for (int i = 1; i <= alphaSize; i++)
                {
                    parent[i] = -1;
                    nHeap++;
                    heap[nHeap] = i;

                    int zz = nHeap;
                    int tmp = heap[zz];
                    while (weight[tmp] < weight[heap[zz >> 1]])
                    {
                        heap[zz] = heap[zz >> 1];
                        zz >>= 1;
                    }
                    heap[zz] = tmp;
                }

                while (nHeap > 1)
                {
                    int n1 = heap[1];
                    heap[1] = heap[nHeap];
                    nHeap--;

                    int yy = 0;
                    int zz = 1;
                    int tmp = heap[1];

                    while (true)
                    {
                        yy = zz << 1;

                        if (yy > nHeap)
                        {
                            break;
                        }

                        if ((yy < nHeap)
                            && (weight[heap[yy + 1]] < weight[heap[yy]]))
                        {
                            yy++;
                        }

                        if (weight[tmp] < weight[heap[yy]])
                        {
                            break;
                        }

                        heap[zz] = heap[yy];
                        zz = yy;
                    }

                    heap[zz] = tmp;

                    int n2 = heap[1];
                    heap[1] = heap[nHeap];
                    nHeap--;

                    yy = 0;
                    zz = 1;
                    tmp = heap[1];

                    while (true)
                    {
                        yy = zz << 1;

                        if (yy > nHeap)
                        {
                            break;
                        }

                        if ((yy < nHeap)
                            && (weight[heap[yy + 1]] < weight[heap[yy]]))
                        {
                            yy++;
                        }

                        if (weight[tmp] < weight[heap[yy]])
                        {
                            break;
                        }

                        heap[zz] = heap[yy];
                        zz = yy;
                    }

                    heap[zz] = tmp;
                    nNodes++;
                    parent[n1] = parent[n2] = nNodes;

                    int weight_n1 = weight[n1];
                    int weight_n2 = weight[n2];

                    weight[nNodes] = (int) ((weight_n1 & 0xffffff00)+ (weight_n2 & 0xffffff00))
                        | (1 + (((weight_n1 & 0x000000ff)
                                 > (weight_n2 & 0x000000ff))
                                ? (weight_n1 & 0x000000ff)
                                : (weight_n2 & 0x000000ff)));

                    parent[nNodes] = -1;
                    nHeap++;
                    heap[nHeap] = nNodes;

                    tmp = 0;
                    zz = nHeap;
                    tmp = heap[zz];
                    int weight_tmp = weight[tmp];
                    while (weight_tmp < weight[heap[zz >> 1]])
                    {
                        heap[zz] = heap[zz >> 1];
                        zz >>= 1;
                    }
                    heap[zz] = tmp;

                }

                for (int i = 1; i <= alphaSize; i++)
                {
                    int j = 0;
                    int k = i;

                    for (int parent_k; (parent_k = parent[k]) >= 0; )
                    {
                        k = parent_k;
                        j++;
                    }

                    len[i - 1] = (byte)j;
                    if (j > maxLen)
                    {
                        tooLong = true;
                    }
                }

                if (tooLong)
                {
                    for (int i = 1; i < alphaSize; i++)
                    {
                        int j = weight[i] >> 8;
                        j = 1 + (j >> 1);
                        weight[i] = j << 8;
                    }
                }
            }
        }

        /**
         * Index of the last char in the block, so the block size == last + 1.
         */
        private int last;

        /**
         * Index in fmap[] of original string after sorting.
         */
        private int origPtr;

        /**
         * Always: in the range 0 .. 9. The current block size is 100000 * this
         * number.
         */
        private readonly int blockSize100k;

        private bool blockRandomised;

        private int bsBuff;
        private int bsLive;
        private readonly CRC crc = new CRC();

        private int nInUse;

        private int nMTF;

        /*
         * Used when sorting. If too many long comparisons happen, we stop sorting,
         * randomise the block slightly, and try again.
         */
        private int workDone;
        private int workLimit;
        private bool firstAttempt;

        private int currentChar = -1;
        private int runLength = 0;

        private int blockCRC;
        private int combinedCRC;
        private int allowableBlockSize;

        /**
         * All memory intensive stuff.
         */
        private Data data;

        private java.io.OutputStream outJ;

        /**
         * Chooses a blocksize based on the given length of the data to compress.
         *
         * @return The blocksize, between {@link #MIN_BLOCKSIZE} and
         *         {@link #MAX_BLOCKSIZE} both inclusive. For a negative
         *         <tt>inputLength</tt> this method returns <tt>MAX_BLOCKSIZE</tt>
         *         always.
         *
         * @param inputLength
         *            The length of the data which will be compressed by
         *            <tt>CBZip2OutputStream</tt>.
         */
        public static int chooseBlockSize(long inputLength)
        {
            return (inputLength > 0) ? (int)java.lang.Math
                .min((inputLength / 132000) + 1, 9) : MAX_BLOCKSIZE;
        }

        /**
         * Constructs a new <tt>CBZip2OutputStream</tt> with a blocksize of 900k.
         *
         * @param out 
         *            the destination stream.
         *
         * @throws IOException
         *             if an I/O error occurs in the specified stream.
         * @throws NullPointerException
         *             if <code>out == null</code>.
         */
        public BZip2CompressorOutputStream(java.io.OutputStream outJ)
            : this(outJ, MAX_BLOCKSIZE)
        //throws IOException 
        {
        }

        /**
         * Constructs a new <tt>CBZip2OutputStream</tt> with specified blocksize.
         *
         * @param out
         *            the destination stream.
         * @param blockSize
         *            the blockSize as 100k units.
         *
         * @throws IOException
         *             if an I/O error occurs in the specified stream.
         * @throws IllegalArgumentException
         *             if <code>(blockSize < 1) || (blockSize > 9)</code>.
         * @throws NullPointerException
         *             if <code>out == null</code>.
         *
         * @see #MIN_BLOCKSIZE
         * @see #MAX_BLOCKSIZE
         */
        public BZip2CompressorOutputStream(java.io.OutputStream outJ,
                                           int blockSize)
            : base()
        //throws IOException 
        {
            if (blockSize < 1)
            {
                throw new java.lang.IllegalArgumentException("blockSize(" + blockSize
                                                   + ") < 1");
            }
            if (blockSize > 9)
            {
                throw new java.lang.IllegalArgumentException("blockSize(" + blockSize
                                                   + ") > 9");
            }

            this.blockSize100k = blockSize;
            this.outJ = outJ;
            init();
        }

        /** {@inheritDoc} */
        public override void write(int b) //throws IOException 
        {
            if (this.outJ != null)
            {
                write0(b);
            }
            else
            {
                throw new java.io.IOException("closed");
            }
        }

        private void writeRun() //throws IOException 
        {
            int lastShadow = this.last;

            if (lastShadow < this.allowableBlockSize)
            {
                int currentCharShadow = this.currentChar;
                Data dataShadow = this.data;
                Data.inUse[currentCharShadow] = true;
                byte ch = (byte)currentCharShadow;

                int runLengthShadow = this.runLength;
                this.crc.updateCRC(currentCharShadow, runLengthShadow);

                switch (runLengthShadow)
                {
                    case 1:
                        Data.block[lastShadow + 2] = ch;
                        this.last = lastShadow + 1;
                        break;

                    case 2:
                        Data.block[lastShadow + 2] = ch;
                        Data.block[lastShadow + 3] = ch;
                        this.last = lastShadow + 2;
                        break;

                    case 3:
                        {
                            byte[] block = Data.block;
                            block[lastShadow + 2] = ch;
                            block[lastShadow + 3] = ch;
                            block[lastShadow + 4] = ch;
                            this.last = lastShadow + 3;
                        }
                        break;

                    default:
                        {
                            runLengthShadow -= 4;
                            Data.inUse[runLengthShadow] = true;
                            byte[] block = Data.block;
                            block[lastShadow + 2] = ch;
                            block[lastShadow + 3] = ch;
                            block[lastShadow + 4] = ch;
                            block[lastShadow + 5] = ch;
                            block[lastShadow + 6] = (byte)runLengthShadow;
                            this.last = lastShadow + 5;
                        }
                        break;

                }
            }
            else
            {
                endBlock();
                initBlock();
                writeRun();
            }
        }

        /**
         * Overriden to close the stream.
         */
        public override void finalize() //throws Throwable 
        {
            finish();
            base.finalize();
        }


        public void finish() //throws IOException 
        {
            if (outJ != null)
            {
                try
                {
                    if (this.runLength > 0)
                    {
                        writeRun();
                    }
                    this.currentChar = -1;
                    endBlock();
                    endCompression();
                }
                finally
                {
                    this.outJ = null;
                    this.data = null;
                }
            }
        }

        public override void close() //throws IOException 
        {
            if (outJ != null)
            {
                java.io.OutputStream outShadow = this.outJ;
                finish();
                outShadow.close();
            }
        }

        public override void flush() //throws IOException 
        {
            java.io.OutputStream outShadow = this.outJ;
            if (outShadow != null)
            {
                outShadow.flush();
            }
        }

        /**
         * Writes magic bytes like BZ on the first position of the stream
         * and bytes indiciating the file-format, which is 
         * huffmanised, followed by a digit indicating blockSize100k.
         * @throws IOException if the magic bytes could not been written
         */
        private void init() //throws IOException 
        {
            bsPutUByte('B');
            bsPutUByte('Z');

            this.data = new Data(this.blockSize100k);

            // huffmanised magic bytes
            bsPutUByte('h');
            bsPutUByte('0' + this.blockSize100k);

            this.combinedCRC = 0;
            initBlock();
        }

        private void initBlock()
        {
            // blockNo++;
            this.crc.initialiseCRC();
            this.last = -1;
            // ch = 0;

            bool[] inUse = Data.inUse;
            for (int i = 256; --i >= 0; )
            {
                inUse[i] = false;
            }

            /* 20 is just a paranoia constant */
            this.allowableBlockSize = (this.blockSize100k * BZip2Constants.BASEBLOCKSIZE) - 20;
        }

        private void endBlock() //throws IOException 
        {
            this.blockCRC = this.crc.getFinalCRC();
            this.combinedCRC = (this.combinedCRC << 1) | (java.dotnet.lang.Operator.shiftRightUnsignet(this.combinedCRC, 31));
            this.combinedCRC ^= this.blockCRC;

            // empty block at end of file
            if (this.last == -1)
            {
                return;
            }

            /* sort the block and establish posn of original string */
            blockSort();

            /*
             * A 6-byte block header, the value chosen arbitrarily as 0x314159265359
             * :-). A 32 bit value does not really give a strong enough guarantee
             * that the value will not appear by chance in the compressed
             * datastream. Worst-case probability of this event, for a 900k block,
             * is about 2.0e-3 for 32 bits, 1.0e-5 for 40 bits and 4.0e-8 for 48
             * bits. For a compressed file of size 100Gb -- about 100000 blocks --
             * only a 48-bit marker will do. NB: normal compression/ decompression
             * donot rely on these statistical properties. They are only important
             * when trying to recover blocks from damaged files.
             */
            bsPutUByte(0x31);
            bsPutUByte(0x41);
            bsPutUByte(0x59);
            bsPutUByte(0x26);
            bsPutUByte(0x53);
            bsPutUByte(0x59);

            /* Now the block's CRC, so it is in a known place. */
            bsPutInt(this.blockCRC);

            /* Now a single bit indicating randomisation. */
            if (this.blockRandomised)
            {
                bsW(1, 1);
            }
            else
            {
                bsW(1, 0);
            }

            /* Finally, block's contents proper. */
            moveToFrontCodeAndSend();
        }

        private void endCompression() //throws IOException 
        {
            /*
             * Now another magic 48-bit number, 0x177245385090, to indicate the end
             * of the last block. (sqrt(pi), if you want to know. I did want to use
             * e, but it contains too much repetition -- 27 18 28 18 28 46 -- for me
             * to feel statistically comfortable. Call me paranoid.)
             */
            bsPutUByte(0x17);
            bsPutUByte(0x72);
            bsPutUByte(0x45);
            bsPutUByte(0x38);
            bsPutUByte(0x50);
            bsPutUByte(0x90);

            bsPutInt(this.combinedCRC);
            bsFinishedWithStream();
        }

        /**
         * Returns the blocksize parameter specified at construction time.
         */
        public int getBlockSize()
        {
            return this.blockSize100k;
        }

        public override void write(byte[] buf, int offs, int len)
        //throws IOException 
        {
            if (offs < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("offs(" + offs + ") < 0.");
            }
            if (len < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("len(" + len + ") < 0.");
            }
            if (offs + len > buf.Length)
            {
                throw new java.lang.IndexOutOfBoundsException("offs(" + offs + ") + len("
                                                    + len + ") > buf.length("
                                                    + buf.Length + ").");
            }
            if (this.outJ == null)
            {
                throw new java.io.IOException("stream closed");
            }

            for (int hi = offs + len; offs < hi; )
            {
                write0(buf[offs++]);
            }
        }

        private void write0(int b) //throws IOException 
        {
            if (this.currentChar != -1)
            {
                b &= 0xff;
                if (this.currentChar == b)
                {
                    if (++this.runLength > 254)
                    {
                        writeRun();
                        this.currentChar = -1;
                        this.runLength = 0;
                    }
                    // else nothing to do
                }
                else
                {
                    writeRun();
                    this.runLength = 1;
                    this.currentChar = b;
                }
            }
            else
            {
                this.currentChar = b & 0xff;
                this.runLength++;
            }
        }

        private static void hbAssignCodes(int[] code, byte[] length,
                                          int minLen, int maxLen,
                                          int alphaSize)
        {
            int vec = 0;
            for (int n = minLen; n <= maxLen; n++)
            {
                for (int i = 0; i < alphaSize; i++)
                {
                    if ((length[i] & 0xff) == n)
                    {
                        code[i] = vec;
                        vec++;
                    }
                }
                vec <<= 1;
            }
        }

        private void bsFinishedWithStream() //throws IOException 
        {
            while (this.bsLive > 0)
            {
                int ch = this.bsBuff >> 24;
                this.outJ.write(ch); // write 8-bit
                this.bsBuff <<= 8;
                this.bsLive -= 8;
            }
        }

        private void bsW(int n, int v) //throws IOException 
        {
            java.io.OutputStream outShadow = this.outJ;
            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            while (bsLiveShadow >= 8)
            {
                outShadow.write(bsBuffShadow >> 24); // write 8-bit
                bsBuffShadow <<= 8;
                bsLiveShadow -= 8;
            }

            this.bsBuff = bsBuffShadow | (v << (32 - bsLiveShadow - n));
            this.bsLive = bsLiveShadow + n;
        }

        private void bsPutUByte(int c) //throws IOException 
        {
            bsW(8, c);
        }

        private void bsPutInt(int u) //throws IOException 
        {
            bsW(8, (u >> 24) & 0xff);
            bsW(8, (u >> 16) & 0xff);
            bsW(8, (u >> 8) & 0xff);
            bsW(8, u & 0xff);
        }

        private void sendMTFValues() //throws IOException 
        {
            byte[,] len = Data.sendMTFValues_len;
            int alphaSize = this.nInUse + 2;

            for (int t = BZip2Constants.N_GROUPS; --t >= 0; )
            {
                byte[] len_t = java.util.Arrays<byte>.getIndexArray(len,t);
                for (int v = alphaSize; --v >= 0; )
                {
                    len_t[v] = (byte)GREATER_ICOST;
                }
            }

            /* Decide how many coding tables to use */
            // assert (this.nMTF > 0) : this.nMTF;
            int nGroups = (this.nMTF < 200) ? 2 : (this.nMTF < 600) ? 3
                : (this.nMTF < 1200) ? 4 : (this.nMTF < 2400) ? 5 : 6;

            /* Generate an initial set of coding tables */
            sendMTFValues0(nGroups, alphaSize);

            /*
             * Iterate up to N_ITERS times to improve the tables.
             */
            int nSelectors = sendMTFValues1(nGroups, alphaSize);

            /* Compute MTF values for the selectors. */
            sendMTFValues2(nGroups, nSelectors);

            /* Assign actual codes for the tables. */
            sendMTFValues3(nGroups, alphaSize);

            /* Transmit the mapping table. */
            sendMTFValues4();

            /* Now the selectors. */
            sendMTFValues5(nGroups, nSelectors);

            /* Now the coding tables. */
            sendMTFValues6(nGroups, alphaSize);

            /* And finally, the block data proper */
            sendMTFValues7(nSelectors);
        }

        private void sendMTFValues0(int nGroups, int alphaSize)
        {
            byte[,] len =Data.sendMTFValues_len;
            int[] mtfFreq = Data.mtfFreq;

            int remF = this.nMTF;
            int gs = 0;

            for (int nPart = nGroups; nPart > 0; nPart--)
            {
                int tFreq = remF / nPart;
                int ge = gs - 1;
                int aFreq = 0;

                for (int a = alphaSize - 1; (aFreq < tFreq) && (ge < a); )
                {
                    aFreq += mtfFreq[++ge];
                }

                if ((ge > gs) && (nPart != nGroups) && (nPart != 1)
                    && (((nGroups - nPart) & 1) != 0))
                {
                    aFreq -= mtfFreq[ge--];
                }

                byte[] len_np = java.util.Arrays<byte>.getIndexArray(len,nPart - 1);
                for (int v = alphaSize; --v >= 0; )
                {
                    if ((v >= gs) && (v <= ge))
                    {
                        len_np[v] = (byte)LESSER_ICOST;
                    }
                    else
                    {
                        len_np[v] = (byte)GREATER_ICOST;
                    }
                }

                gs = ge + 1;
                remF -= aFreq;
            }
        }

        private int sendMTFValues1(int nGroups, int alphaSize)
        {
            Data dataShadow = this.data;
            int[,] rfreq = Data.sendMTFValues_rfreq;
            int[] fave = Data.sendMTFValues_fave;
            short[] cost = Data.sendMTFValues_cost;
            char[] sfmap = Data.sfmap;
            byte[] selector = Data.selector;
            byte[,] len = Data.sendMTFValues_len;
            byte[] len_0 = java.util.Arrays<byte>.getIndexArray(len,0);
            byte[] len_1 = java.util.Arrays<byte>.getIndexArray(len,1);
            byte[] len_2 = java.util.Arrays<byte>.getIndexArray(len,2);
            byte[] len_3 = java.util.Arrays<byte>.getIndexArray(len,3);
            byte[] len_4 = java.util.Arrays<byte>.getIndexArray(len,4);
            byte[] len_5 = java.util.Arrays<byte>.getIndexArray(len,5);
            int nMTFShadow = this.nMTF;

            int nSelectors = 0;

            for (int iter = 0; iter < BZip2Constants.N_ITERS; iter++)
            {
                for (int t = nGroups; --t >= 0; )
                {
                    fave[t] = 0;
                    int[] rfreqt = java.util.Arrays<int>.getIndexArray(rfreq,t);
                    for (int i = alphaSize; --i >= 0; )
                    {
                        rfreqt[i] = 0;
                    }
                }

                nSelectors = 0;

                for (int gs = 0; gs < this.nMTF; )
                {
                    /* Set group start & end marks. */

                    /*
                     * Calculate the cost of this group as coded by each of the
                     * coding tables.
                     */

                    int ge = java.lang.Math.min(gs + BZip2Constants.G_SIZE - 1, nMTFShadow - 1);

                    if (nGroups == BZip2Constants.N_GROUPS)
                    {
                        // unrolled version of the else-block

                        short cost0 = 0;
                        short cost1 = 0;
                        short cost2 = 0;
                        short cost3 = 0;
                        short cost4 = 0;
                        short cost5 = 0;

                        for (int i = gs; i <= ge; i++)
                        {
                            int icv = sfmap[i];
                            cost0 += (short)(len_0[icv] & 0xff);
                            cost1 += (short)(len_1[icv] & 0xff);
                            cost2 += (short)(len_2[icv] & 0xff);
                            cost3 += (short)(len_3[icv] & 0xff);
                            cost4 += (short)(len_4[icv] & 0xff);
                            cost5 += (short)(len_5[icv] & 0xff);
                        }

                        cost[0] = cost0;
                        cost[1] = cost1;
                        cost[2] = cost2;
                        cost[3] = cost3;
                        cost[4] = cost4;
                        cost[5] = cost5;

                    }
                    else
                    {
                        for (int t = nGroups; --t >= 0; )
                        {
                            cost[t] = 0;
                        }

                        for (int i = gs; i <= ge; i++)
                        {
                            int icv = sfmap[i];
                            for (int t = nGroups; --t >= 0; )
                            {
                                cost[t] += (short)(len[t,icv] & 0xff);
                            }
                        }
                    }

                    /*
                     * Find the coding table which is best for this group, and
                     * record its identity in the selector table.
                     */
                    int bt = -1;
                    for (int t = nGroups, bc = 999999999; --t >= 0; )
                    {
                        int cost_t = cost[t];
                        if (cost_t < bc)
                        {
                            bc = cost_t;
                            bt = t;
                        }
                    }

                    fave[bt]++;
                    selector[nSelectors] = (byte)bt;
                    nSelectors++;

                    /*
                     * Increment the symbol frequencies for the selected table.
                     */
                    int[] rfreq_bt = java.util.Arrays<int>.getIndexArray(rfreq,bt);
                    for (int i = gs; i <= ge; i++)
                    {
                        rfreq_bt[sfmap[i]]++;
                    }

                    gs = ge + 1;
                }

                /*
                 * Recompute the tables based on the accumulated frequencies.
                 */
                for (int t = 0; t < nGroups; t++)
                {
                    hbMakeCodeLengths(java.util.Arrays<byte>.getIndexArray(len,t), java.util.Arrays<int>.getIndexArray(rfreq,t), this.data, alphaSize, 20);
                }
            }

            return nSelectors;
        }

        private void sendMTFValues2(int nGroups, int nSelectors)
        {
            // assert (nGroups < 8) : nGroups;

            Data dataShadow = this.data;
            byte[] pos = Data.sendMTFValues2_pos;

            for (int i = nGroups; --i >= 0; )
            {
                pos[i] = (byte)i;
            }

            for (int i = 0; i < nSelectors; i++)
            {
                byte ll_i = Data.selector[i];
                byte tmp = pos[0];
                int j = 0;

                while (ll_i != tmp)
                {
                    j++;
                    byte tmp2 = tmp;
                    tmp = pos[j];
                    pos[j] = tmp2;
                }

                pos[0] = tmp;
                Data.selectorMtf[i] = (byte)j;
            }
        }

        private void sendMTFValues3(int nGroups, int alphaSize)
        {
            int[,] code = Data.sendMTFValues_code;
            byte[,] len = Data.sendMTFValues_len;

            for (int t = 0; t < nGroups; t++)
            {
                int minLen = 32;
                int maxLen = 0;
                byte[] len_t = java.util.Arrays<byte>.getIndexArray(len,t);
                for (int i = alphaSize; --i >= 0; )
                {
                    int l = len_t[i] & 0xff;
                    if (l > maxLen)
                    {
                        maxLen = l;
                    }
                    if (l < minLen)
                    {
                        minLen = l;
                    }
                }

                // assert (maxLen <= 20) : maxLen;
                // assert (minLen >= 1) : minLen;

                hbAssignCodes(java.util.Arrays<int>.getIndexArray(code,t), java.util.Arrays<byte>.getIndexArray(len,t), minLen, maxLen, alphaSize);
            }
        }

        private void sendMTFValues4() //throws IOException 
        {
            bool[] inUse = Data.inUse;
            bool[] inUse16 = Data.sentMTFValues4_inUse16;

            for (int i = 16; --i >= 0; )
            {
                inUse16[i] = false;
                int i16 = i * 16;
                for (int j = 16; --j >= 0; )
                {
                    if (inUse[i16 + j])
                    {
                        inUse16[i] = true;
                    }
                }
            }

            for (int i = 0; i < 16; i++)
            {
                bsW(1, inUse16[i] ? 1 : 0);
            }

            java.io.OutputStream outShadow = this.outJ;
            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            for (int i = 0; i < 16; i++)
            {
                if (inUse16[i])
                {
                    int i16 = i * 16;
                    for (int j = 0; j < 16; j++)
                    {
                        // inlined: bsW(1, inUse[i16 + j] ? 1 : 0);
                        while (bsLiveShadow >= 8)
                        {
                            outShadow.write(bsBuffShadow >> 24); // write 8-bit
                            bsBuffShadow <<= 8;
                            bsLiveShadow -= 8;
                        }
                        if (inUse[i16 + j])
                        {
                            bsBuffShadow |= 1 << (32 - bsLiveShadow - 1);
                        }
                        bsLiveShadow++;
                    }
                }
            }

            this.bsBuff = bsBuffShadow;
            this.bsLive = bsLiveShadow;
        }

        private void sendMTFValues5(int nGroups, int nSelectors)
        //throws IOException 
        {
            bsW(3, nGroups);
            bsW(15, nSelectors);

            java.io.OutputStream outShadow = this.outJ;
            byte[] selectorMtf = Data.selectorMtf;

            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            for (int i = 0; i < nSelectors; i++)
            {
                for (int j = 0, hj = selectorMtf[i] & 0xff; j < hj; j++)
                {
                    // inlined: bsW(1, 1);
                    while (bsLiveShadow >= 8)
                    {
                        outShadow.write(bsBuffShadow >> 24);
                        bsBuffShadow <<= 8;
                        bsLiveShadow -= 8;
                    }
                    bsBuffShadow |= 1 << (32 - bsLiveShadow - 1);
                    bsLiveShadow++;
                }

                // inlined: bsW(1, 0);
                while (bsLiveShadow >= 8)
                {
                    outShadow.write(bsBuffShadow >> 24);
                    bsBuffShadow <<= 8;
                    bsLiveShadow -= 8;
                }
                // bsBuffShadow |= 0 << (32 - bsLiveShadow - 1);
                bsLiveShadow++;
            }

            this.bsBuff = bsBuffShadow;
            this.bsLive = bsLiveShadow;
        }

        private void sendMTFValues6(int nGroups, int alphaSize)
        //throws IOException 
        {
            byte[,] len = Data.sendMTFValues_len;
            java.io.OutputStream outShadow = this.outJ;

            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            for (int t = 0; t < nGroups; t++)
            {
                byte[] len_t = java.util.Arrays<byte>.getIndexArray(len,t);
                int curr = len_t[0] & 0xff;

                // inlined: bsW(5, curr);
                while (bsLiveShadow >= 8)
                {
                    outShadow.write(bsBuffShadow >> 24); // write 8-bit
                    bsBuffShadow <<= 8;
                    bsLiveShadow -= 8;
                }
                bsBuffShadow |= curr << (32 - bsLiveShadow - 5);
                bsLiveShadow += 5;

                for (int i = 0; i < alphaSize; i++)
                {
                    int lti = len_t[i] & 0xff;
                    while (curr < lti)
                    {
                        // inlined: bsW(2, 2);
                        while (bsLiveShadow >= 8)
                        {
                            outShadow.write(bsBuffShadow >> 24); // write 8-bit
                            bsBuffShadow <<= 8;
                            bsLiveShadow -= 8;
                        }
                        bsBuffShadow |= 2 << (32 - bsLiveShadow - 2);
                        bsLiveShadow += 2;

                        curr++; /* 10 */
                    }

                    while (curr > lti)
                    {
                        // inlined: bsW(2, 3);
                        while (bsLiveShadow >= 8)
                        {
                            outShadow.write(bsBuffShadow >> 24); // write 8-bit
                            bsBuffShadow <<= 8;
                            bsLiveShadow -= 8;
                        }
                        bsBuffShadow |= 3 << (32 - bsLiveShadow - 2);
                        bsLiveShadow += 2;

                        curr--; /* 11 */
                    }

                    // inlined: bsW(1, 0);
                    while (bsLiveShadow >= 8)
                    {
                        outShadow.write(bsBuffShadow >> 24); // write 8-bit
                        bsBuffShadow <<= 8;
                        bsLiveShadow -= 8;
                    }
                    // bsBuffShadow |= 0 << (32 - bsLiveShadow - 1);
                    bsLiveShadow++;
                }
            }

            this.bsBuff = bsBuffShadow;
            this.bsLive = bsLiveShadow;
        }

        private void sendMTFValues7(int nSelectors) //throws IOException 
        {
            Data dataShadow = this.data;
            byte[,] len = Data.sendMTFValues_len;
            int[,] code = Data.sendMTFValues_code;
            java.io.OutputStream outShadow = this.outJ;
            byte[] selector = Data.selector;
            char[] sfmap = Data.sfmap;
            int nMTFShadow = this.nMTF;

            int selCtr = 0;

            int bsLiveShadow = this.bsLive;
            int bsBuffShadow = this.bsBuff;

            for (int gs = 0; gs < nMTFShadow; )
            {
                int ge = java.lang.Math.min(gs + BZip2Constants.G_SIZE - 1, nMTFShadow - 1);
                int selector_selCtr = selector[selCtr] & 0xff;
                int[] code_selCtr = java.util.Arrays<int>.getIndexArray(code,selector_selCtr);
                byte[] len_selCtr = java.util.Arrays<byte>.getIndexArray(len,selector_selCtr);

                while (gs <= ge)
                {
                    int sfmap_i = sfmap[gs];

                    //
                    // inlined: bsW(len_selCtr[sfmap_i] & 0xff,
                    // code_selCtr[sfmap_i]);
                    //
                    while (bsLiveShadow >= 8)
                    {
                        outShadow.write(bsBuffShadow >> 24);
                        bsBuffShadow <<= 8;
                        bsLiveShadow -= 8;
                    }
                    int n = len_selCtr[sfmap_i] & 0xFF;
                    bsBuffShadow |= code_selCtr[sfmap_i] << (32 - bsLiveShadow - n);
                    bsLiveShadow += n;

                    gs++;
                }

                gs = ge + 1;
                selCtr++;
            }

            this.bsBuff = bsBuffShadow;
            this.bsLive = bsLiveShadow;
        }

        private void moveToFrontCodeAndSend() //throws IOException 
        {
            bsW(24, this.origPtr);
            generateMTFValues();
            sendMTFValues();
        }

        /**
         * This is the most hammered method of this class.
         *
         * <p>
         * This is the version using unrolled loops. Normally I never use such ones
         * in Java code. The unrolling has shown a noticable performance improvement
         * on JRE 1.4.2 (Linux i586 / HotSpot Client). Of course it depends on the
         * JIT compiler of the vm.
         * </p>
         */
        private bool mainSimpleSort(Data dataShadow, int lo, int hi, int d) {
            int bigN = hi - lo + 1;
            if (bigN < 2) {
                return this.firstAttempt && (this.workDone > this.workLimit);
            }

            int hp = 0;
            while (INCS[hp] < bigN) {
                hp++;
            }

            int[] fmap = Data.fmap;
            char[] quadrant = Data.quadrant;
            byte[] block = Data.block;
            int lastShadow = this.last;
            int lastPlus1 = lastShadow + 1;
            bool firstAttemptShadow = this.firstAttempt;
            int workLimitShadow = this.workLimit;
            int workDoneShadow = this.workDone;
            // Following block contains unrolled code which could be shortened by
            // coding it in additional loops.

            HP: while (--hp >= 0) {
                int h = INCS[hp];
                int mj = lo + h - 1;

                for (int i = lo + h; i <= hi;) {
                    // copy
                    for (int k = 3; (i <= hi) && (--k >= 0); i++) {
                        int v = fmap[i];
                        int vd = v + d;
                        int j = i;

                        // for (int a;
                        // (j > mj) && mainGtU((a = fmap[j - h]) + d, vd,
                        // block, quadrant, lastShadow);
                        // j -= h) {
                        // fmap[j] = a;
                        // }
                        //
                        // unrolled version:

                        // start inline mainGTU
                        bool onceRunned = false;
                        int a = 0;

                        HAMMER: while (true) {
                            if (onceRunned) {
                                fmap[j] = a;
                                if ((j -= h) <= mj) {
                                    goto HAMMER;
                                }
                            } else {
                                onceRunned = true;
                            }

                            a = fmap[j - h];
                            int i1 = a + d;
                            int i2 = vd;

                            // following could be done in a loop, but
                            // unrolled it for performance:
                            if (block[i1 + 1] == block[i2 + 1]) {
                                if (block[i1 + 2] == block[i2 + 2]) {
                                    if (block[i1 + 3] == block[i2 + 3]) {
                                        if (block[i1 + 4] == block[i2 + 4]) {
                                            if (block[i1 + 5] == block[i2 + 5]) {
                                                if (block[(i1 += 6)] == block[(i2 += 6)]) {
                                                    int x = lastShadow;
                                                    X: while (x > 0) {
                                                        x -= 4;

                                                        if (block[i1 + 1] == block[i2 + 1]) {
                                                            if (quadrant[i1] == quadrant[i2]) {
                                                                if (block[i1 + 2] == block[i2 + 2]) {
                                                                    if (quadrant[i1 + 1] == quadrant[i2 + 1]) {
                                                                        if (block[i1 + 3] == block[i2 + 3]) {
                                                                            if (quadrant[i1 + 2] == quadrant[i2 + 2]) {
                                                                                if (block[i1 + 4] == block[i2 + 4]) {
                                                                                    if (quadrant[i1 + 3] == quadrant[i2 + 3]) {
                                                                                        if ((i1 += 4) >= lastPlus1) {
                                                                                            i1 -= lastPlus1;
                                                                                        }
                                                                                        if ((i2 += 4) >= lastPlus1) {
                                                                                            i2 -= lastPlus1;
                                                                                        }
                                                                                        workDoneShadow++;
                                                                                        goto X; //continue X; // Basties note: BUHHHHH
                                                                                    } else if ((quadrant[i1 + 3] > quadrant[i2 + 3])) {
                                                                                        goto HAMMER;
                                                                                    } else {
                                                                                        goto HAMMER;
                                                                                    }
                                                                                } else if ((block[i1 + 4] & 0xff) > (block[i2 + 4] & 0xff)) {
                                                                                    goto HAMMER;
                                                                                } else {
                                                                                    goto HAMMER;
                                                                                }
                                                                            } else if ((quadrant[i1 + 2] > quadrant[i2 + 2])) {
                                                                                goto HAMMER;
                                                                            } else {
                                                                                goto HAMMER;
                                                                            }
                                                                        } else if ((block[i1 + 3] & 0xff) > (block[i2 + 3] & 0xff)) {
                                                                            goto HAMMER;
                                                                        } else {
                                                                            goto HAMMER;
                                                                        }
                                                                    } else if ((quadrant[i1 + 1] > quadrant[i2 + 1])) {
                                                                        goto HAMMER;
                                                                    } else {
                                                                        goto HAMMER;
                                                                    }
                                                                } else if ((block[i1 + 2] & 0xff) > (block[i2 + 2] & 0xff)) {
                                                                    goto HAMMER;
                                                                } else {
                                                                    goto HAMMER;
                                                                }
                                                            } else if ((quadrant[i1] > quadrant[i2])) {
                                                                goto HAMMER;
                                                            } else {
                                                                goto HAMMER;
                                                            }
                                                        } else if ((block[i1 + 1] & 0xff) > (block[i2 + 1] & 0xff)) {
                                                            goto HAMMER;
                                                        } else {
                                                            goto HAMMER;
                                                        }

                                                    }
                                                    goto HAMMER;
                                                } // while x > 0
                                                else {
                                                    if ((block[i1] & 0xff) > (block[i2] & 0xff)) {
                                                        goto HAMMER;
                                                    } else {
                                                        goto HAMMER;
                                                    }
                                                }
                                            } else if ((block[i1 + 5] & 0xff) > (block[i2 + 5] & 0xff)) {
                                                goto HAMMER;
                                            } else {
                                                goto HAMMER;
                                            }
                                        } else if ((block[i1 + 4] & 0xff) > (block[i2 + 4] & 0xff)) {
                                            goto HAMMER;
                                        } else {
                                            goto HAMMER;
                                        }
                                    } else if ((block[i1 + 3] & 0xff) > (block[i2 + 3] & 0xff)) {
                                        goto HAMMER;
                                    } else {
                                        goto HAMMER;
                                    }
                                } else if ((block[i1 + 2] & 0xff) > (block[i2 + 2] & 0xff)) {
                                    goto HAMMER;
                                } else {
                                    goto HAMMER;
                                }
                            } else if ((block[i1 + 1] & 0xff) > (block[i2 + 1] & 0xff)) {
                                goto HAMMER;
                            } else {
                                goto HAMMER;
                            }

                        } // HAMMER
                        // end inline mainGTU

                        fmap[j] = v;
                    }

                    if (firstAttemptShadow && (i <= hi)
                        && (workDoneShadow > workLimitShadow)) {
                        goto HP;
                    }
                }
            }

            this.workDone = workDoneShadow;
            return firstAttemptShadow && (workDoneShadow > workLimitShadow);
        }

        private static void vswap(int[] fmap, int p1, int p2, int n)
        {
            n += p1;
            while (p1 < n)
            {
                int t = fmap[p1];
                fmap[p1++] = fmap[p2];
                fmap[p2++] = t;
            }
        }

        private static byte med3(byte a, byte b, byte c)
        {
            return (a < b) ? (b < c ? b : a < c ? c : a) : (b > c ? b : a > c ? c
                                                            : a);
        }

        private void blockSort()
        {
            this.workLimit = WORK_FACTOR * this.last;
            this.workDone = 0;
            this.blockRandomised = false;
            this.firstAttempt = true;
            mainSort();

            if (this.firstAttempt && (this.workDone > this.workLimit))
            {
                randomiseBlock();
                this.workLimit = this.workDone = 0;
                this.firstAttempt = false;
                mainSort();
            }

            int[] fmap = Data.fmap;
            this.origPtr = -1;
            for (int i = 0, lastShadow = this.last; i <= lastShadow; i++)
            {
                if (fmap[i] == 0)
                {
                    this.origPtr = i;
                    break;
                }
            }

            // assert (this.origPtr != -1) : this.origPtr;
        }

        /**
         * Method "mainQSort3", file "blocksort.c", BZip2 1.0.2
         */
        private void mainQSort3(Data dataShadow, int loSt, int hiSt, int dSt)
        {
            int[] stack_ll = Data.stack_ll;
            int[] stack_hh = Data.stack_hh;
            int[] stack_dd = Data.stack_dd;
            int[] fmap = Data.fmap;
            byte[] block = Data.block;

            stack_ll[0] = loSt;
            stack_hh[0] = hiSt;
            stack_dd[0] = dSt;

            for (int sp = 1; --sp >= 0; )
            {
                int lo = stack_ll[sp];
                int hi = stack_hh[sp];
                int d = stack_dd[sp];

                if ((hi - lo < SMALL_THRESH) || (d > DEPTH_THRESH))
                {
                    if (mainSimpleSort(dataShadow, lo, hi, d))
                    {
                        return;
                    }
                }
                else
                {
                    int d1 = d + 1;
                    int med = med3(block[fmap[lo] + d1],
                                   block[fmap[hi] + d1],
                                   block[fmap[java.dotnet.lang.Operator.shiftRightUnsignet((lo + hi), 1)] + d1]) & 0xff;

                    int unLo = lo;
                    int unHi = hi;
                    int ltLo = lo;
                    int gtHi = hi;

                    while (true)
                    {
                        while (unLo <= unHi)
                        {
                            int n = (block[fmap[unLo] + d1] & 0xff)
                                - med;
                            if (n == 0)
                            {
                                int temp = fmap[unLo];
                                fmap[unLo++] = fmap[ltLo];
                                fmap[ltLo++] = temp;
                            }
                            else if (n < 0)
                            {
                                unLo++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (unLo <= unHi)
                        {
                            int n = (block[fmap[unHi] + d1] & 0xff)
                                - med;
                            if (n == 0)
                            {
                                int temp = fmap[unHi];
                                fmap[unHi--] = fmap[gtHi];
                                fmap[gtHi--] = temp;
                            }
                            else if (n > 0)
                            {
                                unHi--;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (unLo <= unHi)
                        {
                            int temp = fmap[unLo];
                            fmap[unLo++] = fmap[unHi];
                            fmap[unHi--] = temp;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (gtHi < ltLo)
                    {
                        stack_ll[sp] = lo;
                        stack_hh[sp] = hi;
                        stack_dd[sp] = d1;
                        sp++;
                    }
                    else
                    {
                        int n = ((ltLo - lo) < (unLo - ltLo)) ? (ltLo - lo)
                            : (unLo - ltLo);
                        vswap(fmap, lo, unLo - n, n);
                        int m = ((hi - gtHi) < (gtHi - unHi)) ? (hi - gtHi)
                            : (gtHi - unHi);
                        vswap(fmap, unLo, hi - m + 1, m);

                        n = lo + unLo - ltLo - 1;
                        m = hi - (gtHi - unHi) + 1;

                        stack_ll[sp] = lo;
                        stack_hh[sp] = n;
                        stack_dd[sp] = d;
                        sp++;

                        stack_ll[sp] = n + 1;
                        stack_hh[sp] = m - 1;
                        stack_dd[sp] = d1;
                        sp++;

                        stack_ll[sp] = m;
                        stack_hh[sp] = hi;
                        stack_dd[sp] = d;
                        sp++;
                    }
                }
            }
        }

        private void mainSort()
        {
            Data dataShadow = this.data;
            int[] runningOrder = Data.mainSort_runningOrder;
            int[] copy = Data.mainSort_copy;
            bool[] bigDone = Data.mainSort_bigDone;
            int[] ftab = Data.ftab;
            byte[] block = Data.block;
            int[] fmap = Data.fmap;
            char[] quadrant = Data.quadrant;
            int lastShadow = this.last;
            int workLimitShadow = this.workLimit;
            bool firstAttemptShadow = this.firstAttempt;

            // Set up the 2-byte frequency table
            for (int i = 65537; --i >= 0; )
            {
                ftab[i] = 0;
            }

            /*
             * In the various block-sized structures, live data runs from 0 to
             * last+NUM_OVERSHOOT_BYTES inclusive. First, set up the overshoot area
             * for block.
             */
            for (int i = 0; i < BZip2Constants.NUM_OVERSHOOT_BYTES; i++)
            {
                block[lastShadow + i + 2] = block[(i % (lastShadow + 1)) + 1];
            }
            for (int i = lastShadow + BZip2Constants.NUM_OVERSHOOT_BYTES + 1; --i >= 0; )
            {
                quadrant[i] = (char)0;
            }
            block[0] = block[lastShadow + 1];

            // Complete the initial radix sort:

            int c1 = block[0] & 0xff;
            for (int i = 0; i <= lastShadow; i++)
            {
                int c2 = block[i + 1] & 0xff;
                ftab[(c1 << 8) + c2]++;
                c1 = c2;
            }

            for (int i = 1; i <= 65536; i++)
                ftab[i] += ftab[i - 1];

            c1 = block[1] & 0xff;
            for (int i = 0; i < lastShadow; i++)
            {
                int c2 = block[i + 2] & 0xff;
                fmap[--ftab[(c1 << 8) + c2]] = i;
                c1 = c2;
            }

            fmap[--ftab[((block[lastShadow + 1] & 0xff) << 8) + (block[1] & 0xff)]] = lastShadow;

            /*
             * Now ftab contains the first loc of every small bucket. Calculate the
             * running order, from smallest to largest big bucket.
             */
            for (int i = 256; --i >= 0; )
            {
                bigDone[i] = false;
                runningOrder[i] = i;
            }

            for (int h = 364; h != 1; )
            {
                h /= 3;
                for (int i = h; i <= 255; i++)
                {
                    int vv = runningOrder[i];
                    int a = ftab[(vv + 1) << 8] - ftab[vv << 8];
                    int b = h - 1;
                    int j = i;
                    for (int ro = runningOrder[j - h]; (ftab[(ro + 1) << 8] - ftab[ro << 8]) > a; ro = runningOrder[j
                                                                                                                    - h])
                    {
                        runningOrder[j] = ro;
                        j -= h;
                        if (j <= b)
                        {
                            break;
                        }
                    }
                    runningOrder[j] = vv;
                }
            }

            /*
             * The main sorting loop.
             */
            for (int i = 0; i <= 255; i++)
            {
                /*
                 * Process big buckets, starting with the least full.
                 */
                int ss = runningOrder[i];

                // Step 1:
                /*
                 * Complete the big bucket [ss] by quicksorting any unsorted small
                 * buckets [ss, j]. Hopefully previous pointer-scanning phases have
                 * already completed many of the small buckets [ss, j], so we don't
                 * have to sort them at all.
                 */
                for (int j = 0; j <= 255; j++)
                {
                    int sb = (ss << 8) + j;
                    int ftab_sb = ftab[sb];
                    if ((ftab_sb & SETMASK) != SETMASK)
                    {
                        int lo = ftab_sb & CLEARMASK;
                        int hi = (ftab[sb + 1] & CLEARMASK) - 1;
                        if (hi > lo)
                        {
                            mainQSort3(dataShadow, lo, hi, 2);
                            if (firstAttemptShadow
                                && (this.workDone > workLimitShadow))
                            {
                                return;
                            }
                        }
                        ftab[sb] = ftab_sb | SETMASK;
                    }
                }

                // Step 2:
                // Now scan this big bucket so as to synthesise the
                // sorted order for small buckets [t, ss] for all t != ss.

                for (int j = 0; j <= 255; j++)
                {
                    copy[j] = ftab[(j << 8) + ss] & CLEARMASK;
                }

                for (int j = ftab[ss << 8] & CLEARMASK, hj = (ftab[(ss + 1) << 8] & CLEARMASK); j < hj; j++)
                {
                    int fmap_j = fmap[j];
                    c1 = block[fmap_j] & 0xff;
                    if (!bigDone[c1])
                    {
                        fmap[copy[c1]] = (fmap_j == 0) ? lastShadow : (fmap_j - 1);
                        copy[c1]++;
                    }
                }

                for (int j = 256; --j >= 0; )
                    ftab[(j << 8) + ss] |= SETMASK;

                // Step 3:
                /*
                 * The ss big bucket is now done. Record this fact, and update the
                 * quadrant descriptors. Remember to update quadrants in the
                 * overshoot area too, if necessary. The "if (i < 255)" test merely
                 * skips this updating for the last bucket processed, since updating
                 * for the last bucket is pointless.
                 */
                bigDone[ss] = true;

                if (i < 255)
                {
                    int bbStart = ftab[ss << 8] & CLEARMASK;
                    int bbSize = (ftab[(ss + 1) << 8] & CLEARMASK) - bbStart;
                    int shifts = 0;

                    while ((bbSize >> shifts) > 65534)
                    {
                        shifts++;
                    }

                    for (int j = 0; j < bbSize; j++)
                    {
                        int a2update = fmap[bbStart + j];
                        char qVal = (char)(j >> shifts);
                        quadrant[a2update] = qVal;
                        if (a2update < BZip2Constants.NUM_OVERSHOOT_BYTES)
                        {
                            quadrant[a2update + lastShadow + 1] = qVal;
                        }
                    }
                }

            }
        }

        private void randomiseBlock()
        {
            bool[] inUse = Data.inUse;
            byte[] block = Data.block;
            int lastShadow = this.last;

            for (int i = 256; --i >= 0; )
                inUse[i] = false;

            int rNToGo = 0;
            int rTPos = 0;
            for (int i = 0, j = 1; i <= lastShadow; i = j, j++)
            {
                if (rNToGo == 0)
                {
                    rNToGo = (char)Rand.rNums(rTPos);
                    if (++rTPos == 512)
                    {
                        rTPos = 0;
                    }
                }

                rNToGo--;
                block[j] ^= (byte) ((rNToGo == 1) ? 1 : 0);

                // handle 16 bit signed numbers
                inUse[block[j] & 0xff] = true;
            }

            this.blockRandomised = true;
        }

        private void generateMTFValues()
        {
            int lastShadow = this.last;
            Data dataShadow = this.data;
            bool[] inUse = Data.inUse;
            byte[] block = Data.block;
            int[] fmap = Data.fmap;
            char[] sfmap = Data.sfmap;
            int[] mtfFreq = Data.mtfFreq;
            byte[] unseqToSeq = Data.unseqToSeq;
            byte[] yy = Data.generateMTFValues_yy;

            // make maps
            int nInUseShadow = 0;
            for (int i = 0; i < 256; i++)
            {
                if (inUse[i])
                {
                    unseqToSeq[i] = (byte)nInUseShadow;
                    nInUseShadow++;
                }
            }
            this.nInUse = nInUseShadow;

            int eob = nInUseShadow + 1;

            for (int i = eob; i >= 0; i--)
            {
                mtfFreq[i] = 0;
            }

            for (int i = nInUseShadow; --i >= 0; )
            {
                yy[i] = (byte)i;
            }

            int wr = 0;
            int zPend = 0;

            for (int i = 0; i <= lastShadow; i++)
            {
                byte ll_i = unseqToSeq[block[fmap[i]] & 0xff];
                byte tmp = yy[0];
                int j = 0;

                while (ll_i != tmp)
                {
                    j++;
                    byte tmp2 = tmp;
                    tmp = yy[j];
                    yy[j] = tmp2;
                }
                yy[0] = tmp;

                if (j == 0)
                {
                    zPend++;
                }
                else
                {
                    if (zPend > 0)
                    {
                        zPend--;
                        while (true)
                        {
                            if ((zPend & 1) == 0)
                            {
                                sfmap[wr] = (char) BZip2Constants.RUNA;
                                wr++;
                                mtfFreq[BZip2Constants.RUNA]++;
                            }
                            else
                            {
                                sfmap[wr] = (char) BZip2Constants.RUNB;
                                wr++;
                                mtfFreq[BZip2Constants.RUNB]++;
                            }

                            if (zPend >= 2)
                            {
                                zPend = (zPend - 2) >> 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        zPend = 0;
                    }
                    sfmap[wr] = (char)(j + 1);
                    wr++;
                    mtfFreq[j + 1]++;
                }
            }

            if (zPend > 0)
            {
                zPend--;
                while (true)
                {
                    if ((zPend & 1) == 0)
                    {
                        sfmap[wr] = (char) BZip2Constants.RUNA;
                        wr++;
                        mtfFreq[BZip2Constants.RUNA]++;
                    }
                    else
                    {
                        sfmap[wr] = (char) BZip2Constants.RUNB;
                        wr++;
                        mtfFreq[BZip2Constants.RUNB]++;
                    }

                    if (zPend >= 2)
                    {
                        zPend = (zPend - 2) >> 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            sfmap[wr] = (char)eob;
            mtfFreq[eob]++;
            this.nMTF = wr + 1;
        }

        #region Data
        internal sealed class Data
        {

            // with blockSize 900k
            public static readonly bool[] inUse = new bool[256]; // 256 byte
            public static readonly byte[] unseqToSeq = new byte[256]; // 256 byte
            public static readonly int[] mtfFreq = new int[BZip2Constants.MAX_ALPHA_SIZE]; // 1032 byte
            public static readonly byte[] selector = new byte[BZip2Constants.MAX_SELECTORS]; // 18002 byte
            public static readonly byte[] selectorMtf = new byte[BZip2Constants.MAX_SELECTORS]; // 18002 byte

            public static readonly byte[] generateMTFValues_yy = new byte[256]; // 256 byte
            public static readonly byte[,] sendMTFValues_len = new byte[BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE]; // 1548
            // byte
            public static readonly int[,] sendMTFValues_rfreq = new int[BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE]; // 6192
            // byte
            public static readonly int[] sendMTFValues_fave = new int[BZip2Constants.N_GROUPS]; // 24 byte
            public static readonly short[] sendMTFValues_cost = new short[BZip2Constants.N_GROUPS]; // 12 byte
            public static readonly int[,] sendMTFValues_code = new int[BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE]; // 6192
            // byte
            public static readonly byte[] sendMTFValues2_pos = new byte[BZip2Constants.N_GROUPS]; // 6 byte
            public static readonly bool[] sentMTFValues4_inUse16 = new bool[16]; // 16 byte

            public static readonly int[] stack_ll = new int[BZip2CompressorOutputStream.QSORT_STACK_SIZE]; // 4000 byte
            public static readonly int[] stack_hh = new int[BZip2CompressorOutputStream.QSORT_STACK_SIZE]; // 4000 byte
            public static readonly int[] stack_dd = new int[BZip2CompressorOutputStream.QSORT_STACK_SIZE]; // 4000 byte

            public static readonly int[] mainSort_runningOrder = new int[256]; // 1024 byte
            public static readonly int[] mainSort_copy = new int[256]; // 1024 byte
            public static readonly bool[] mainSort_bigDone = new bool[256]; // 256 byte

            public static readonly int[] heap = new int[BZip2Constants.MAX_ALPHA_SIZE + 2]; // 1040 byte
            public static readonly int[] weight = new int[BZip2Constants.MAX_ALPHA_SIZE * 2]; // 2064 byte
            public static readonly int[] parent = new int[BZip2Constants.MAX_ALPHA_SIZE * 2]; // 2064 byte

            public static readonly int[] ftab = new int[65537]; // 262148 byte
            // ------------
            // 333408 byte

            public static byte[] block; // 900021 byte
            public static int[] fmap; // 3600000 byte
            public static char[] sfmap; // 3600000 byte
            // ------------
            // 8433529 byte
            // ============

            /**
             * Array instance identical to sfmap, both are used only
             * temporarily and indepently, so we do not need to allocate
             * additional memory.
             */
            public static char[] quadrant;

            internal Data(int blockSize100k)
            {
                int n = blockSize100k * BZip2Constants.BASEBLOCKSIZE;
                block = new byte[(n + 1 + BZip2Constants.NUM_OVERSHOOT_BYTES)];
                fmap = new int[n];
                sfmap = new char[2 * n];
                quadrant = sfmap;
            }

        }
        #endregion
    }
}