
using System;
using java = biz.ritter.javapi;

using com.jguild.jrpm.io;
using org.apache.tools.ant; //import org.apache.tools.ant.BuildException; import org.apache.tools.ant.Task;
using org.apache.tools.ant.types; //import org.apache.tools.ant.types.FileSet;

namespace com.jguild.jrpm.tools
{


    /**
     * Ant task used to create RPM packages.
     *
     * @version $Id: RPMTask.java,v 1.6 2003/11/19 13:46:14 ymenager Exp $
     */
    public class RPMTask : Task
    {
        private java.io.File destfile;
        private java.util.ArrayList<Object> filesets = new java.util.ArrayList<Object>();

        /**
         * @param fileSet
         * @todo implement
         */
        public void addInner(FileSet fileSet)
        {
            filesets.add(fileSet);
        }

        /**
         * @see org.apache.tools.ant.Task#execute()
         */
        public void execute()
        {//throws BuildException {
            RPMFile rpmfile = new RPMFile();
        }

        public void setDestfile(java.io.File destfile)
        {
            this.destfile = destfile;
        }
    }
}