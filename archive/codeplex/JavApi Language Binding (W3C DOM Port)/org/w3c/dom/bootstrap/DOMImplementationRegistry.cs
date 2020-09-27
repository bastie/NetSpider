/*
 * Copyright (c) 2004 World Wide Web Consortium,
 *
 * (Massachusetts Institute of Technology, European Research Consortium for
 * Informatics and Mathematics, Keio University). All Rights Reserved. This
 * work is distributed under the W3C(r) Software License [1] in the hope that
 * it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 * [1] http://www.w3.org/Consortium/Legal/2002/copyright-software-20021231
 */
using System;
using java = biz.ritter.javapi;
using org.w3c.dom;

namespace org.w3c.dom.bootstrap
{

    /**
     * A factory that enables applications to obtain instances of
     * <code>DOMImplementation</code>.
     *
     * <p>
     * Example:
     * </p>
     *
     * <pre class='example'>
     *  // get an instance of the DOMImplementation registry
     *  DOMImplementationRegistry registry =
     *       DOMImplementationRegistry.newInstance();
     *  // get a DOM implementation the Level 3 XML module
     *  DOMImplementation domImpl =
     *       registry.getDOMImplementation("XML 3.0");
     * </pre>
     *
     * <p>
     * This provides an application with an implementation-independent starting
     * point. DOM implementations may modify this class to meet new security
     * standards or to provide *additional* fallbacks for the list of
     * DOMImplementationSources.
     * </p>
     *
     * @see DOMImplementation
     * @see DOMImplementationSource
     * @since DOM Level 3
     */
    public sealed class DOMImplementationRegistry
    {
        /**
         * The system property to specify the
         * DOMImplementationSource class names.
         */
        public static readonly String PROPERTY =
        "org.w3c.dom.DOMImplementationSourceList";

        /**
         * Default columns per line.
         */
        private static readonly int DEFAULT_LINE_LENGTH = 80;

        /**
         * The list of DOMImplementationSources.
         */
        private java.util.Vector<Object> sources;

        /**
         * Private constructor.
         * @param srcs Vector List of DOMImplementationSources
         */
        private DOMImplementationRegistry(java.util.Vector<Object> srcs)
        {
            sources = srcs;
        }

        /**
         * Obtain a new instance of a <code>DOMImplementationRegistry</code>.
         *

         * The <code>DOMImplementationRegistry</code> is initialized by the
         * application or the implementation, depending on the context, by
         * first checking the value of the Java system property
         * <code>org.w3c.dom.DOMImplementationSourceList</code> and
         * the the service provider whose contents are at
         * "<code>META_INF/services/org.w3c.dom.DOMImplementationSourceList</code>"
         * The value of this property is a white-space separated list of
         * names of availables classes implementing the
         * <code>DOMImplementationSource</code> interface. Each class listed
         * in the class name list is instantiated and any exceptions
         * encountered are thrown to the application.
         *
         * @return an initialized instance of DOMImplementationRegistry
         * @throws ClassNotFoundException
         *     If any specified class can not be found
         * @throws InstantiationException
         *     If any specified class is an interface or abstract class
         * @throws IllegalAccessException
         *     If the default constructor of a specified class is not accessible
         * @throws ClassCastException
         *     If any specified class does not implement
         * <code>DOMImplementationSource</code>
         */
        public static DOMImplementationRegistry newInstance()
        {
            //throws     ClassNotFoundException,     InstantiationException,     IllegalAccessException,     ClassCastException {
            java.util.Vector<Object> sources = new java.util.Vector<Object>();

            java.lang.ClassLoader classLoader = getClassLoader();
            // fetch system property:
            String p = getSystemProperty(PROPERTY);

            //
            // if property is not specified then use contents of
            // META_INF/org.w3c.dom.DOMImplementationSourceList from classpath
            if (p == null)
            {
                p = getServiceValue(classLoader);
            }
            if (p == null)
            {
                //
                // DOM Implementations can modify here to add *additional* fallback
                // mechanisms to access a list of default DOMImplementationSources.

            }
            if (p != null)
            {
                java.util.StringTokenizer st = new java.util.StringTokenizer(p);
                while (st.hasMoreTokens())
                {
                    String sourceName = st.nextToken();
                    // Use context class loader, falling back to Class.forName
                    // if and only if this fails...
                    java.lang.Class sourceClass = null;
                    if (classLoader != null)
                    {
                        sourceClass = classLoader.loadClass(sourceName);
                    }
                    else
                    {
                        sourceClass = java.lang.Class.forName(sourceName);
                    }
                    DOMImplementationSource source =
                        (DOMImplementationSource)sourceClass.newInstance();
                    sources.addElement(source);
                }
            }
            return new DOMImplementationRegistry(sources);
        }

        /**
         * Return the first implementation that has the desired
         * features, or <code>null</code> if none is found.
         *
         * @param features
         *            A string that specifies which features are required. This is
         *            a space separated list in which each feature is specified by
         *            its name optionally followed by a space and a version number.
         *            This is something like: "XML 1.0 Traversal +Events 2.0"
         * @return An implementation that has the desired features,
         *         or <code>null</code> if none found.
         */
        public DOMImplementation getDOMImplementation(String features)
        {
            int size = sources.size();
            //String name = null; Basties Note: unused...
            for (int i = 0; i < size; i++)
            {
                DOMImplementationSource source =
                (DOMImplementationSource)sources.elementAt(i);
                DOMImplementation impl = source.getDOMImplementation(features);
                if (impl != null)
                {
                    return impl;
                }
            }
            return null;
        }

        /**
         * Return a list of implementations that support the
         * desired features.
         *
         * @param features
         *            A string that specifies which features are required. This is
         *            a space separated list in which each feature is specified by
         *            its name optionally followed by a space and a version number.
         *            This is something like: "XML 1.0 Traversal +Events 2.0"
         * @return A list of DOMImplementations that support the desired features.
         */
        public DOMImplementationList getDOMImplementationList(String features)
        {
            java.util.Vector<Object> implementations = new java.util.Vector<Object>();
            int size = sources.size();
            for (int i = 0; i < size; i++)
            {
                DOMImplementationSource source =
                (DOMImplementationSource)sources.elementAt(i);
                DOMImplementationList impls =
                source.getDOMImplementationList(features);
                for (int j = 0; j < impls.getLength(); j++)
                {
                    DOMImplementation impl = impls.item(j);
                    implementations.addElement(impl);
                }
            }
            return new IAC_DOMImplementationList(implementations);
        }
        sealed class IAC_DOMImplementationList : DOMImplementationList
        {
            java.util.Vector<Object> implementations = new java.util.Vector<Object>();
            public IAC_DOMImplementationList(java.util.Vector<Object> implList)
            {
                this.implementations = implList;
            }
            public DOMImplementation item(int index)
            {
                if (index >= 0 && index < implementations.size())
                {
                    try
                    {
                        return (DOMImplementation)
                        implementations.elementAt(index);
                    }
                    catch (java.lang.ArrayIndexOutOfBoundsException)
                    {
                        return null;
                    }
                }
                return null;
            }

            public int getLength()
            {
                return implementations.size();
            }

        }

        /**
         * Register an implementation.
         *
         * @param s The source to be registered, may not be <code>null</code>
         */
        public void addSource(DOMImplementationSource s)
        {
            if (s == null)
            {
                throw new java.lang.NullPointerException();
            }
            if (!sources.contains(s))
            {
                sources.addElement(s);
            }
        }

        /**
         *
         * Gets a class loader.
         *
         * @return A class loader, possibly <code>null</code>
         */
        private static java.lang.ClassLoader getClassLoader()
        {
            try
            {
                java.lang.ClassLoader contextClassLoader = getContextClassLoader();

                if (contextClassLoader != null)
                {
                    return contextClassLoader;
                }
            }
            catch (Exception)
            {
                // Assume that the DOM application is in a JRE 1.1, use the
                // current ClassLoader
                return typeof(DOMImplementationRegistry).getClass().getClassLoader();
            }
            return typeof(DOMImplementationRegistry).getClass().getClassLoader();
        }

        /**
         * This method attempts to return the first line of the resource
         * META_INF/services/org.w3c.dom.DOMImplementationSourceList
         * from the provided ClassLoader.
         *
         * @param classLoader classLoader, may not be <code>null</code>.
         * @return first line of resource, or <code>null</code>
         */
        private static String getServiceValue(java.lang.ClassLoader classLoader)
        {
            String serviceId = "META-INF/services/" + PROPERTY;
            // try to find services in CLASSPATH
            try
            {
                java.io.InputStream input = getResourceAsStream(classLoader, serviceId);

                if (input != null)
                {
                    java.io.BufferedReader rd;
                    try
                    {
                        rd =
                        new java.io.BufferedReader(new java.io.InputStreamReader(input, "UTF-8"),
                                   DEFAULT_LINE_LENGTH);
                    }
                    catch (java.io.UnsupportedEncodingException)
                    {
                        rd =
                        new java.io.BufferedReader(new java.io.InputStreamReader(input),
                                   DEFAULT_LINE_LENGTH);
                    }
                    String serviceValue = rd.readLine();
                    rd.close();
                    if (serviceValue != null && serviceValue.length() > 0)
                    {
                        return serviceValue;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        /*  Basties note: we do not need JRE 1.1 method
         * A simple JRE (Java Runtime Environment) 1.1 test
         *
         * @return <code>true</code> if JRE 1.1 
         *
        private static bool isJRE11() {
        try {
            Class c = Class.forName("java.security.AccessController");
            // java.security.AccessController existed since 1.2 so, if no
            // exception was thrown, the DOM application is running in a JRE
            // 1.2 or higher
            return false;
        } catch (Exception ex) {
            // ignore 
        }
        return true;
        }*/

        /**
         * This method returns the ContextClassLoader or <code>null</code> if
         * running in a JRE 1.1
         *
         * @return The Context Classloader
         */
        private static java.lang.ClassLoader getContextClassLoader()
        {
            /*
        return isJRE11()
            ? null
            : (ClassLoader)
              AccessController.doPrivileged(new PrivilegedAction() {
                public Object run() {
                ClassLoader classLoader = null;
                try {
                    classLoader =
                    Thread.currentThread().getContextClassLoader();
                } catch (SecurityException ex) {
                }
                return classLoader;
                }
            });*/
            return typeof(DOMImplementationRegistry).getClass().getClassLoader();
        }

        /**
         * This method returns the system property indicated by the specified name
         * after checking access control privileges. For a JRE 1.1, this check is
         * not done.
         * 	 
         * @param name the name of the system property	 
         * @return the system property
         */
        private static String getSystemProperty(String name)
        {
            return java.lang.SystemJ.getProperty(name);/*
    return isJRE11()
        ? (String) System.getProperty(name)
        : (String) AccessController.doPrivileged(new PrivilegedAction() {
            public Object run() {
            return System.getProperty(name);
            }
        });*/
        }

        /**
         * This method returns an Inputstream for the reading resource
         * META_INF/services/org.w3c.dom.DOMImplementationSourceList after checking
         * access control privileges. For a JRE 1.1, this check is not done.
         *
         * @param classLoader classLoader	 
         * @param name the resource 	 
         * @return an Inputstream for the resource specified
         */
        private static java.io.InputStream getResourceAsStream(java.lang.ClassLoader classLoader,
                              String name)
        {
            java.io.InputStream input = null == classLoader ? java.lang.ClassLoader.getSystemResourceAsStream(name) : classLoader.getResourceAsStream(name);
            return input;
            /*
        if (isJRE11()) {
            InputStream ris;
            if (classLoader == null) {
            ris = ClassLoader.getSystemResourceAsStream(name);
            } else {
            ris = classLoader.getResourceAsStream(name);
            }    
            return ris;
        } else {
            return (InputStream)
            AccessController.doPrivileged(new PrivilegedAction() {
                public Object run() {
                    InputStream ris;
                    if (classLoader == null) {
                    ris =
                        ClassLoader.getSystemResourceAsStream(name);
                    } else {
                    ris = classLoader.getResourceAsStream(name);
                    }
                    return ris;
                }
                });*/
        }
    }
}