/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 **/
using System;
using java = biz.ritter.javapi;
using com.jguild.jrpm.io;
using com.jguild.jrpm.io.datatype;

namespace com.jguild.jrpm.tools
{

    public class Info
    {
        private static double kbs = 0;

        private static long start_base = 0;

        /**
             * @param args
             * @throws IOException
             */
        public static void main(String[] args)
        {
            java.io.File dir = new java.io.File(args[0]);

            start_base = java.lang.SystemJ.currentTimeMillis();

            scanFiles(dir.listFiles());
            print("DONE ================================== ", 45, false);
            print(getTime(java.lang.SystemJ.currentTimeMillis() - start_base), 10, true);
        }

        /**
             * @param files
             */
        private static void scanFiles(java.io.File[] files)
        {
            for (int pos = 0; pos < files.Length; pos++)
            {
                if (files[pos].isDirectory())
                {
                    scanFiles(files[pos].listFiles());
                    continue;
                }
                if (!files[pos].getName().endsWith(".rpm"))
                    continue;

                long start = java.lang.SystemJ.currentTimeMillis();
                try
                {

                    print(files[pos].getName(), 40, false);
                    RPMFile rpm = new RPMFile(files[pos]);
                    rpm.parse();

                    print(" ", 1, false);
                    print("OK!", 5, false);
                    print(" ", 1, false);
                    long time = java.lang.SystemJ.currentTimeMillis() - start;
                    print(getTime(time++), 8, true);
                    print(" ", 1, false);
                    print(rpm.getTag("PAYLOADCOMPRESSOR"), 6, true);
                    print(" ", 1, false);
                    print(rpm.getTag("VENDOR"), 20, false);
                    print(" ", 1, false);

                    double kb = (files[pos].length() / 1024d);
                    kbs += kb;
                    print(java.lang.Math.round(kb / (time / 1000d)) + " KB/s", 20, false);
                    print(java.lang.Math.round(kbs
                        / ((java.lang.SystemJ.currentTimeMillis() - start_base) / 1000d))
                        + " KB/s", 20, false);
                    java.lang.SystemJ.outJ.println();
                }
                catch (java.lang.Exception e)
                {
                    print(" ", 1, false);
                    print("FAILED!", 10, false);
                    java.lang.SystemJ.outJ.println();
                    e.printStackTrace(java.lang.SystemJ.outJ);
                }
            }
        }

        /**
             * @param tag
             * @param i
             * @param b
             */
        private static void print(DataTypeIf tag, int size, bool right)
        {
            String str = "";
            if (tag != null)
            {
                str = tag.toString();
            }
            print(str, size, right);
        }

        /**
             * @param l
             * @return
             */
        private static String getTime(long time)
        {
            String einheit = "ms";
            if (time > 2000)
            {
                time /= 1000;
                einheit = "s";

                if (time > 120)
                {
                    time /= 60;
                    einheit = "m";
                }
            }
            return time + " " + einheit;
        }

        /**
             * @param string
             * @param i
             */
        private static void print(String stringJ, int size, bool right)
        {
            int length = stringJ.length();
            if (length > size)
            {
                stringJ = stringJ.substring(0, size);
            }
            if (right)
            {
                while (length++ < size)
                {
                    java.lang.SystemJ.outJ.print(" ");
                }
            }
            java.lang.SystemJ.outJ.print(stringJ);

            if (!right)
            {
                while (length++ < size)
                {
                    java.lang.SystemJ.outJ.print(" ");
                }
            }
        }
    }
}