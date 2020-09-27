/*
 *  Copyright 2003 jRPM Team
 * 
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
 */
using System;
using java = biz.ritter.javapi;
using javax = biz.ritter.javapix;

using com.jguild.jrpm.io;
using com.jguild.jrpm.io.datatype;

namespace com.jguild.jrpm.ui{

/**
 * A browser to search for rpm files in a given directory. The rpms are schown
 * in a tree. The tree display the in rpm defined groups. So every rpm that
 * point to the same group are sorted into the same node in the tree. This
 * browser should demonstrate the functionality of jRPM.
 *
 * @author kuss
 */
public class RPMBrowser : JFrame , ActionListener {

    private InfoPanel infoPanel = new InfoPanel(new String[]{"name",
            "version", "release", "summary", "description", "vendor", "url",
            "copyright", "distribution", "disturl", "filenames"});

    private RPMGroupTreeModel treeModel = new RPMGroupTreeModel();

    /**
     * Creates a new RPMBrowser object.
     */
    public RPMBrowser() {
        setTitle("RPM Browser");
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        setSize(new Dimension(600, 400));
        setJMenuBar(createMenuBar());

        Container content = getContentPane();
        JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT,
                new JScrollPane(createGroupTree()), new JScrollPane(
                createRPMInfoPane()));
        splitPane.setDividerLocation(200);
        content.add(splitPane);

        //Center on screen
        setLocationRelativeTo(null);

        addWindowListener(new IAC_WindowAdapter() );
    }

    sealed class IAC_WindowAdapter : java.awt.WindowAdapter {
        private readonly RPMBrowser root;
        internal IAC_WindowAdapter(RPMBrowser instance) {
            this.root= instance;
        }
            public void windowClosing(WindowEvent e) {
                this.root.quit();
            }
    }

    /*
     * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
     */
    public void actionPerformed(ActionEvent e) {
        if (e.getActionCommand().equals("Exit")) {
            quit();
        } else if (e.getActionCommand().equals("ChooseDir")) {
            chooseBaseDir();
        }
    }

    /**
     * Main method to start the RPMBrowser
     *
     * @param args The start arguments
     */
    public static void main(String[] args) {
        Locale.setDefault(Locale.ENGLISH);
        RPMBrowser frame = new RPMBrowser();
        frame.setVisible(true);
    }

    private List getRPMFiles(File dir, MyMonitor monitor) {
        List rpms = new ArrayList();
        File[] list = dir.listFiles(new FileFilter() {

            public bool accept(File f) {
                return f.isDirectory()
                        || f.getName().toLowerCase().endsWith(".rpm");
            }
        });

        monitor.setMaximum(monitor.getMaximum() + list.Length);

        for (int pos = 0; pos < list.Length; pos++) {
            if (list[pos].isDirectory()) {
                rpms.addAll(getRPMFiles(list[pos], monitor));
            } else {
                rpms.add(list[pos]);
            }

            monitor.incProgress();
        }

        Thread.yield();

        return rpms;
    }

    private void chooseBaseDir() {
        JFileChooser dirChooser = new JFileChooser();
        dirChooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);

        if (dirChooser.showOpenDialog(this) == JFileChooser.APPROVE_OPTION) {
            createRPMTree(dirChooser.getSelectedFile());
        }
    }

    private JTree createGroupTree() {
        JTree tree = new JTree(treeModel);
        ToolTipManager.sharedInstance().registerComponent(tree);
        tree.setCellRenderer(new MyRenderer());
        tree.addTreeSelectionListener(new TreeSelectionListener() {

            public void valueChanged(TreeSelectionEvent e) {
                JTree tree = (JTree) e.getSource();
                Object node = tree.getLastSelectedPathComponent();

                if (node instanceof RPMNode) {
                    infoPanel.displayRPM(((RPMNode) node).getRPM());
                }
            }
        });

        return tree;
    }

    private JMenuBar createMenuBar() {
        JMenuBar menuBar;
        JMenu menu;
        JMenuItem menuItem;

        menuBar = new JMenuBar();

        menu = new JMenu("File");
        menu.setMnemonic(KeyEvent.VK_F);
        menuBar.add(menu);

        menuItem = new JMenuItem("Choose dir ...", KeyEvent.VK_C);
        menuItem.setActionCommand("ChooseDir");
        menuItem.addActionListener(this);
        menu.add(menuItem);
        menu.addSeparator();

        menuItem = new JMenuItem("Exit", KeyEvent.VK_E);
        menuItem.setActionCommand("Exit");
        menuItem.addActionListener(this);
        menu.add(menuItem);

        return menuBar;
    }

    private JPanel createRPMInfoPane() {
        return infoPanel;
    }

    /**
     * @param file
     */
    private void createRPMTree( File f) {
        setEnabled(false);

        Thread thread = new Thread() {

            public void run() {
                MyMonitor monitor = new MyMonitor(RPMBrowser.this, "Progress",
                        "<html>Searching rpm files in '" + f.getPath()
                                + "'<br>and all subdirectories ...<br></html>",
                        1);
                List rpms = getRPMFiles(f, monitor);
                treeModel.setRoot(new GroupNode(f.getPath()));
                monitor.setVisible(false);

                monitor.setMaximum(rpms.size());
                monitor.setProgress(0);
                monitor
                        .setMessage("<html>Reading RPM informations ...<br></html>");
                monitor.pack();
                monitor.setVisible(true);

                for (int pos = 0; pos < rpms.size(); pos++) {
                    monitor.incProgress();

                    try {
                        RPMFile rpm = new RPMFile((File) rpms.get(pos));
                        rpm.parse();
                        GroupNode node = treeModel.putGroup(rpm.getTag("group")
                                .toString());

                        if (node.addRPMNode(new RPMNode(rpm, (File) rpms
                                .get(pos)))) {
                            treeModel.fireTreeStructureChanged(node);
                        }
                    } catch (IOException ) {
                        //Ignore this
                    }
                }

                monitor.setVisible(false);
                RPMBrowser.this.setEnabled(true);
            }
        };

        thread.start();
    }

    private void quit() {
        if (quitConfirmed()) {
            System.exit(0);
        }
    }

    private bool quitConfirmed() {
        String s1 = "Quit";
        String s2 = "Cancel";
        Object[] options = {s1, s2};
        int n = JOptionPane.showOptionDialog(this,
                "Do you really want to quit?", "Quit Confirmation",
                JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE, null,
                options, s1);

        if (n == JOptionPane.YES_OPTION) {
            return true;
        } else {
            return false;
        }
    }

    class MyRenderer : DefaultTreeCellRenderer {

        public MyRenderer() {
        }

        public Component getTreeCellRendererComponent(JTree tree, Object value,
                                                      bool sel, bool expanded, bool leaf, int row,
                                                      bool hasFocus) {
            super.getTreeCellRendererComponent(tree, value, sel, expanded,
                    leaf, row, hasFocus);

            if (value instanceof RPMNode) {
                RPMNode node = (RPMNode) value;
                File[] file = node.getPath();
                java.lang.StringBuffer buf = new java.lang.StringBuffer();
                buf.append("<html><B>Locations:</B><BR>");

                for (int pos = 0; pos < file.length; pos++) {
                    buf.append(file[pos]);

                    if (pos < (file.length - 1)) {
                        buf.append("<BR>");
                    }
                }

                buf.append("<html>");
                setToolTipText(buf.toString());
            } else {
                setToolTipText(null);
            }

            return this;
        }
    }

    private class GroupNode {

        private Comparator comp = new Comparator() {

            public int compare(Object o1, Object o2) {
                if ((o1 == null) && (o2 == null)) {
                    return 0;
                }

                if (o1 == null) {
                    return -1;
                }

                if (o2 == null) {
                    return 1;
                }

                return o1.toString().compareTo(o2.toString());
            }
        };

        private Set groups = new TreeSet(comp);

        private Set rpms = new TreeSet(comp);

        private String name;

        public GroupNode(String name) {
            this.name = name;
        }

        public Object getChild(int count) {
            Object ret = null;

            if (count < groups.size()) {
                ret = groups.toArray()[count];
            } else {
                ret = rpms.toArray()[count - groups.size()];
            }

            return ret;
        }

        public int getChildCount() {
            return rpms.size() + groups.size();
        }

        public bool isLeaf() {
            return (rpms.size() == 0) && (groups.size() == 0);
        }

        public bool addGroupNode(GroupNode node) {
            if (groups.contains(node)) {
                return false;
            }

            groups.add(node);

            return true;
        }

        public bool addRPMNode(RPMNode node) {
            if (rpms.contains(node)) {
                ArrayList rpmList = new ArrayList(rpms);
                RPMNode rpmnode = (RPMNode) rpmList.get(rpmList.indexOf(node));
                File[] paths = node.getPath();

                for (int pos = 0; pos < paths.length; pos++) {
                    rpmnode.addPath(paths[pos]);
                }

                return false;
            }

            rpms.add(node);

            return true;
        }

        public override bool Equals(Object obj) {
            return name.equals(obj.toString());
        }

        public override int GetHashCode() {
            return name.hashCode();
        }

        /**
         * @param child
         * @return
         */
        public int indexOf(Object child) {
            int ret = -1;

            if ((ret = (new ArrayList(groups)).indexOf(child)) < 0) {
                ret = (new ArrayList(rpms)).indexOf(child);

                if (ret >= 0) {
                    ret += groups.size();
                }
            }

            return ret;
        }

        public String toString() {
            return name;
        }
    }

    private class InfoPanel : JPanel : ActionListener {

        private DefaultComboBoxModel model = new DefaultComboBoxModel();

        private Hashtable tagLabels = new Hashtable();

        private JComboBox langBox = new JComboBox();

        private RPMFile rpmFile = null;

        public InfoPanel(String[] tags) {
            langBox.setModel(model);

            GridBagLayout gridLayout = new GridBagLayout();
            GridBagConstraints c = new GridBagConstraints();
            c.fill = GridBagConstraints.VERTICAL;
            c.anchor = GridBagConstraints.NORTHWEST;

            setLayout(gridLayout);

            langBox.setEditable(false);
            langBox.addActionListener(this);
            c.gridy = 0;

            JLabel langLabel = new JLabel("Language : ");
            gridLayout.setConstraints(langLabel, c);
            add(langLabel);
            c.gridx = 1;
            gridLayout.setConstraints(langBox, c);
            add(langBox);
            c.gridx = 0;
            c.gridwidth = 2;
            c.fill = GridBagConstraints.HORIZONTAL;
            c.weightx = 1.0;

            for (int pos = 0; pos < tags.length; pos++) {
                c.gridy = pos + 1;

                TagLabel tagLabel = new TagLabel(tags[pos]);
                gridLayout.setConstraints(tagLabel, c);
                add(tagLabel);
                tagLabels.put(tags[pos], tagLabel);
            }

            c.weightx = 0;
            c.weighty = 1.0;

            JPanel fill = new JPanel();
            gridLayout.setConstraints(fill, c);
            add(fill);
        }

        /*
         * @see java.awt.event.ActionListener#actionPerformed(java.awt.event.ActionEvent)
         */
        public void actionPerformed(ActionEvent e) {
            if (rpmFile != null) {
                String locale = (String) langBox.getSelectedItem();

                if (locale != null) {
                    rpmFile.setLocale(locale);
                }

                updateView();
            }
        }

        public void displayRPM(RPMFile rpm) {
            rpmFile = rpm;

            langBox.setSelectedIndex(-1);
            model.removeAllElements();

            String[] langs = rpmFile.getLocales();

            for (int pos = 0; pos < langs.length; pos++) {
                model.addElement(langs[pos]);
            }

            updateView();
        }

        private void updateView() {
            Enumeration keys = tagLabels.keys();

            while (keys.hasMoreElements()) {
                String tagName = (String) keys.nextElement();
                TagLabel label = (TagLabel) tagLabels.get(tagName);
                DataTypeIf tagValue = rpmFile.getTag(tagName);

                if (tagValue == null) {
                    label.setValue("(none)");
                } else {
                    label.setValue(tagValue.toString());
                }
            }
        }
    }

    private class MyMonitor : JDialog {

        private JLabel label;

        private JProgressBar bar;

        public MyMonitor(JFrame parentComponent, String title, String message,
                         int max) {
            super(parentComponent);
            setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
            setTitle(title);

            Container content = getContentPane();
            content.setLayout(new BorderLayout());
            bar = new JProgressBar(0, max);
            content.add(bar, BorderLayout.CENTER);
            label = new JLabel(message);
            content.add(label, BorderLayout.NORTH);
            pack();
            setLocationRelativeTo(parentComponent);
            setVisible(true);
        }

        public void setMaximum(int max) {
            bar.setMaximum(max);
        }

        public int getMaximum() {
            return bar.getMaximum();
        }

        public void setMessage(String message) {
            label.setText(message);
        }

        public void setProgress(int p) {
            bar.setValue(p);
        }

        public void incProgress() {
            bar.setValue(bar.getValue() + 1);
        }
    }

    private class RPMGroupTreeModel : TreeModel {

        private GroupNode root = null;

        private Hashtable groups = new Hashtable();

        private Vector treeModelListeners = new Vector();

        public Object getChild(Object parent, int index) {
            Object ret = null;

            if (parent instanceof GroupNode) {
                ret = ((GroupNode) parent).getChild(index);
            }

            return ret;
        }

        public int getChildCount(Object parent) {
            int count = 0;

            if (parent instanceof GroupNode) {
                count = ((GroupNode) parent).getChildCount();
            }

            return count;
        }

        public int getIndexOfChild(Object parent, Object child) {
            int ret = -1;

            if (parent instanceof GroupNode) {
                ret = ((GroupNode) parent).indexOf(child);
            }

            return ret;
        }

        public bool isLeaf(Object node) {
            bool isLeaf = true;

            if (node is GroupNode) {
                isLeaf = ((GroupNode) node).isLeaf();
            }

            return isLeaf;
        }

        public void setRoot(GroupNode root) {
            GroupNode oldRoot = this.root;
            this.root = root;
            groups = new Hashtable();

            if (oldRoot != null) {
                fireTreeStructureChanged(oldRoot);
            }
        }

        public Object getRoot() {
            return root;
        }

        public void addTreeModelListener(TreeModelListener l) {
            treeModelListeners.addElement(l);
        }

        public void fireTreeStructureChanged(Object obj) {
            int len = treeModelListeners.size();
            TreeModelEvent e = new TreeModelEvent(this, new Object[]{obj});

            for (int i = 0; i < len; i++) {
                ((TreeModelListener) treeModelListeners.elementAt(i))
                        .treeStructureChanged(e);
            }
        }

        public GroupNode putGroup(String path) {
            GroupNode group = null;
            StringTokenizer pathToken = new StringTokenizer(path, "/");

            //try to get existing group
            GroupNode parent = root;
            String tmp = "";
            String groupName = null;

            while (pathToken.hasMoreTokens()) {
                groupName = pathToken.nextToken();
                tmp += groupName;

                if (!groups.containsKey(tmp)) {
                    group = new GroupNode(groupName);
                    groups.put(tmp, group);
                } else {
                    group = (GroupNode) groups.get(tmp);
                }

                if (parent.addGroupNode(group)) {
                    fireTreeStructureChanged(parent);
                }

                parent = group;
                tmp += "/";
            }

            return group;
        }

        public void removeTreeModelListener(TreeModelListener l) {
            treeModelListeners.removeElement(l);
        }

        public void valueForPathChanged(TreePath path, Object newValue) {
            throw new UnsupportedOperationException();
        }
    }

    private class RPMNode {

        private RPMFile rpm;

        private Vector paths = new Vector();

        public RPMNode(RPMFile rpm, File path) {
            this.rpm = rpm;
            paths.add(path);
        }

        public File[] getPath() {
            return (File[]) paths.toArray(new File[0]);
        }

        public RPMFile getRPM() {
            return rpm;
        }

        public void addPath(File f) {
            if (!paths.contains(f)) {
                paths.add(f);
            }
        }

        public bool equals(Object obj) {
            return toString().equals(obj.toString());
        }

        public int hashCode() {
            return toString().hashCode();
        }

        public String toString() {
            return rpm.getTag("name").toString() + "-"
                    + rpm.getTag("version").toString() + "-"
                    + rpm.getTag("release").toString();
        }
    }

    private class TagLabel : JPanel {

        private JLabel label = new JLabel();

        private JTextArea value = new JTextArea();

        public TagLabel(String tagName) {
            setLayout(new BoxLayout(this, BoxLayout.Y_AXIS));
            label.setText(tagName);
            label.setAlignmentX(LEFT_ALIGNMENT);
            value.setAlignmentX(LEFT_ALIGNMENT);
            add(label);
            add(value);
            setAlignmentX(LEFT_ALIGNMENT);
        }

        public void setValue(String valueStr) {
            value.setText(valueStr);
        }
    }
}
