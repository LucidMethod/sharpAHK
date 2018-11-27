using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class AHKExpressions
    {

        // === TREEVIEW ===

        /// <summary>Expand All Nodes in TreeView</summary>
        /// <param name="TV"> </param>
        public static void Expand(this TreeView TV)
        {
            // expand search results in tree
            if (TV.InvokeRequired)
            {
                TV.BeginInvoke((MethodInvoker)delegate () { TV.ExpandAll(); });
            }
            else
            {
                TV.ExpandAll();
            }
        }

        /// <summary>
        /// Expand TreeNodes Below NodeLevel 
        /// </summary>
        /// <param name="TV">TreeView Control</param>
        /// <param name="NodeLevel">Level of Nodes to Expand</param>
        public static void Expand_Level(this TreeView TV, int NodeLevel = 0)
        {
            List<TreeNode> results = NodeList(TV, false); // return list of nodes (option for checked only)

            foreach (TreeNode node in results)
            {
                if (node.Level <= NodeLevel)
                {
                    node.Expand();
                }
                if (node.Level > NodeLevel)
                {
                    node.Collapse();
                }
            }
        }


        #region === TreeView: Return From Tree ===

        // Return Data From TreeView

        /// <summary>Returns list of nodes in TreeView (Option to Return Checked Only) - option to return nodes on certain level</summary>
        /// <param name="TV"> </param>
        /// <param name="CheckedOnly"> </param>
        /// <param name="NodeLevel"> </param>
        public static List<TreeNode> NodeList(this TreeView TV, bool CheckedOnly = false, int NodeLevel = -1)
        {
            ////Ex: 

            //List<TreeNode> results = tv.NodeList(treeView1, false); // return list of nodes (option for checked only)

            //foreach (TreeNode node in results)
            //{
            //    ahk.MsgBox(node.Text); 
            //}


            List<TreeNode> result = new List<TreeNode>();


            // create list of nodes to loop through
            List<TreeNode> nodeList = new List<TreeNode>();  // create list of all nodes in treeview to check
            foreach (TreeNode node in TV.Nodes) { nodeList.Add(node); }


            foreach (TreeNode node in nodeList)
            {

                if (NodeLevel != -1)
                {
                    if (node.Level == NodeLevel)
                    {
                        if (CheckedOnly)
                        {
                            if (node.Checked) { result.Add(node); }
                        }

                        if (!CheckedOnly) { result.Add(node); }
                    }
                    continue;
                }



                // only return values that are checked
                if (CheckedOnly)
                {
                    if (node.Checked)
                    {
                        result.Add(node);
                        List<TreeNode> kids = Nodes_Children(TV, node, CheckedOnly);
                        foreach (TreeNode kid in kids)
                        {
                            result.Add(kid);
                        }
                    }

                    if (!node.Checked)
                    {
                        List<TreeNode> kids = Nodes_Children(TV, node, CheckedOnly);
                        foreach (TreeNode kid in kids)
                        {
                            result.Add(kid);
                        }
                    }

                }

                // return all entries, checked + unchecked
                if (!CheckedOnly)
                {
                    //MessageBox.Show(node.Text);
                    result.Add(node);
                    List<TreeNode> kids = Nodes_Children(TV, node, CheckedOnly);
                    foreach (TreeNode kid in kids)
                    {
                        result.Add(kid);
                    }
                }

            }


            return result;
        }


        /// <summary>Recurse through treeview nodes, return list of child nodes</summary>
        /// <param name="TV"> </param>
        /// <param name="treeNode"> </param>
        /// <param name="CheckedOnly"> </param>
        public static List<TreeNode> Nodes_Children(this TreeView TV, TreeNode treeNode, bool CheckedOnly = false)
        {
            List<TreeNode> kids = new List<TreeNode>();

            if (treeNode == null) { return null; }  //nothing to do if null value passed while user is clicking


            // update control text (from any thread) -- [ works in dll ]
            List<TreeNode> nodeList = new List<TreeNode>();  // create list of all nodes in treeview to check
            foreach (TreeNode tnz in treeNode.Nodes) { nodeList.Add(tnz); }


            // Print each child node recursively.
            foreach (TreeNode tn in nodeList)
            {
                // only return values that are checked
                if (CheckedOnly)
                {
                    if (tn.Checked)
                    {
                        kids.Add(tn);
                        List<TreeNode> subkids = Nodes_Children(TV, tn, CheckedOnly);
                        foreach (TreeNode kid in subkids)
                        {
                            kids.Add(kid);
                        }
                    }
                    if (!tn.Checked)
                    {
                        List<TreeNode> subkids = Nodes_Children(TV, tn, CheckedOnly);
                        foreach (TreeNode kid in subkids)
                        {
                            kids.Add(kid);
                        }
                    }

                }

                // return all entries, checked + unchecked
                if (!CheckedOnly)
                {
                    // Print the node.
                    //MessageBox.Show(tn.Text);
                    kids.Add(tn);
                    List<TreeNode> subkids = Nodes_Children(TV, tn, CheckedOnly);
                    foreach (TreeNode kid in subkids)
                    {
                        kids.Add(kid);
                    }

                }
            }

            return kids;
        }




        #endregion




    }
}
