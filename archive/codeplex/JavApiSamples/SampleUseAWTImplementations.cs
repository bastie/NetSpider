using System;
using java = biz.ritter.javapi;

namespace javapi.sample
{
    class SampleUseAWTImplementations
    {
        [STAThread]
        static void Main()
        {
            bool forms = true;
            if (forms)
            {
                // Step 0: Show system configuration
                java.lang.SystemJ.outJ.println("awt.toolkit=" + java.lang.SystemJ.getProperty("awt.toolkit"));
                java.lang.SystemJ.outJ.println("java.awt.headless=" + java.lang.SystemJ.getProperty("java.awt.headless"));

                // Step 1: 
                java.awt.Frame formsFrame = new java.awt.Frame("Hello AWT over Windows Forms!");
                formsFrame.setSize(800, 600);
                formsFrame.setVisible(true);
            }
            else
            {
                // Step 2: Redirect to WPF
                java.util.Properties prop = java.lang.SystemJ.getProperties();
                prop.put("awt.toolkit", "biz.ritter.awt.wpf.WPFToolkit");

                // Step 3: Show system configuration
                java.lang.SystemJ.outJ.println("awt.toolkit=" + java.lang.SystemJ.getProperty("awt.toolkit"));
                java.lang.SystemJ.outJ.println("java.awt.headless=" + java.lang.SystemJ.getProperty("java.awt.headless"));

                // Step 4: we use Windows Presentation Framework
                java.awt.Frame wpfFrame = new java.awt.Frame("Hello AWT over Windows Presentation Framework");
                wpfFrame.setSize(400, 300);
                wpfFrame.setVisible(true);
            }
        }
    }
}
