using System.Collections.Generic;
using System.Windows.Forms;

namespace CommandsEditor
{
    public enum TreeItemType
    {
        EXPORTABLE_FILE, //An exportable file
        LOADED_STRING, //A loaded string (WIP for COMMANDS.PAK)
        PREVIEW_ONLY_FILE, //A read-only file (export not supported yet)
        DIRECTORY //A parent directory listing
    };
    public enum TreeItemIcon
    {
        FOLDER,
        FILE,
        STRING
    };

    public struct TreeItem
    {
        public string String_Value;
        public TreeItemType Item_Type;
    }

    class TreeUtility
    {
        private TreeView FileTree;
        private bool IsModelTree;
        public TreeUtility(TreeView tree, bool isModelTree = false)
        {
            FileTree = tree;
            IsModelTree = isModelTree;
        }

        /* Update the file tree GUI */
        public void UpdateFileTree(List<string> FilesToList, ContextMenuStrip contextMenu = null, List<string> tags = null)
        {
            FileTree.SuspendLayout();
            FileTree.BeginUpdate();
            FileTree.Nodes.Clear();
            for (int i = 0; i < FilesToList.Count; i++)
            {
                string[] FileNameParts = FilesToList[i].Split('/');
                if (FileNameParts.Length == 1) { FileNameParts = FilesToList[i].Split('\\'); }
                AddFileToTree(FileNameParts, 0, FileTree.Nodes, contextMenu, (tags == null) ? "" : tags[i]);
            }
            FileTree.Sort();
            FileTree.EndUpdate();
            FileTree.ResumeLayout();
        }

        /* Add a file to the GUI tree structure */
        private void AddFileToTree(string[] FileNameParts, int index, TreeNodeCollection LoopedNodeCollection, ContextMenuStrip contextMenu = null, string tag = "")
        {
            if (FileNameParts.Length <= index)
            {
                return;
            }

            bool should = true;
            foreach (TreeNode ThisFileNode in LoopedNodeCollection)
            {
                if (ThisFileNode.Text == FileNameParts[index])
                {
                    should = false;
                    AddFileToTree(FileNameParts, index + 1, ThisFileNode.Nodes, contextMenu, tag);
                    break;
                }
            }
            if (should)
            {
                TreeNode FileNode = new TreeNode(FileNameParts[index]);
                TreeItem ThisTag = new TreeItem();
                if (FileNameParts.Length - 1 == index)
                {
                    //Node is a file
                    if (tag == "")
                    {
                        for (int i = 0; i < FileNameParts.Length; i++)
                        {
                            ThisTag.String_Value += FileNameParts[i] + "/";
                        }
                        ThisTag.String_Value = ThisTag.String_Value.ToString().Substring(0, ThisTag.String_Value.ToString().Length - 1);
                    }
                    else 
                        ThisTag.String_Value = tag;

                    ThisTag.Item_Type = TreeItemType.EXPORTABLE_FILE;
                    FileNode.ImageIndex = (int)TreeItemIcon.FILE;
                    FileNode.SelectedImageIndex = (int)TreeItemIcon.FILE;
                    if (contextMenu != null) FileNode.ContextMenuStrip = contextMenu;
                }
                else
                {
                    //Node is a directory
                    ThisTag.Item_Type = TreeItemType.DIRECTORY;
                    FileNode.ImageIndex = (int)TreeItemIcon.FOLDER;
                    FileNode.SelectedImageIndex = (int)TreeItemIcon.FOLDER;
                    AddFileToTree(FileNameParts, index + 1, FileNode.Nodes, contextMenu, tag);
                }

                FileNode.Tag = ThisTag;
                LoopedNodeCollection.Add(FileNode);
            }
        }

        /* Select a node in the tree based on the path */
        public void SelectNode(string path)
        {
            string[] FileNameParts = path.Split('/');
            if (FileNameParts.Length == 1) { FileNameParts = path.Split('\\'); }
            FileTree.SelectedNode = null;

            TreeNodeCollection nodeCollection = FileTree.Nodes;
            for (int x = 0; x < FileNameParts.Length; x++)
            {
                for (int i = 0; i < nodeCollection.Count; i++)
                {
                    if (nodeCollection[i].Text == FileNameParts[x])
                    {
                        if (x == FileNameParts.Length - 1)
                        {
                            FileTree.SelectedNode = nodeCollection[i];
                        }
                        else
                        {
                            nodeCollection = nodeCollection[i].Nodes;
                        }
                        break;
                    }
                }
            }
        }
    }
}
