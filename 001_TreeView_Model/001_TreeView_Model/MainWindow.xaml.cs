using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _001_TreeView_Model
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Node> nodes = new List<Node>()
            {
                new Node { ID = 1, Name = "中国", bClick=true },
                new Node { ID = 2, Name = "北京市",  bClick=true, ParentID = 1 },
                new Node { ID = 3, Name = "吉林省",  bClick=true, ParentID = 1 },
                new Node { ID = 4, Name = "上海市",   bClick=true, ParentID = 1 },
                new Node { ID = 5, Name = "海淀区",  bClick=true, ParentID = 2 },
                new Node { ID = 6, Name = "朝阳区",  bClick=true, ParentID = 2 },
                new Node { ID = 7, Name = "大兴区", bClick=true, ParentID = 2 },
                new Node { ID = 8, Name = "白山市",  bClick=true, ParentID = 3 },
                new Node { ID = 9, Name = "长春市",  bClick=true, ParentID = 3 },
                new Node { ID = 10, Name = "抚松县", bClick=true, ParentID = 8 },
                new Node { ID = 11, Name = "靖宇县",  bClick=true, ParentID = 8 }
            };

            List<Node> outputList = Bind(nodes);

            tvTreeView.ItemsSource = outputList;

        }


        List<Node> Bind(List<Node> nodes)
        {
            List<Node> outputList = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ParentID == -1)
                {
                    outputList.Add(nodes[i]);
                }
                else
                {
                    FindDownward(nodes, nodes[i].ParentID).Nodes.Add(nodes[i]);
                }
            }
            return outputList;
        }
        /// <summary>
        /// 递归向下查找
        /// </summary>
        Node FindDownward(List<Node> nodes, int id)
        {
            if (nodes == null) return null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ID == id)
                {
                    return nodes[i];
                }
                Node node = FindDownward(nodes[i].Nodes, id);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            TreeViewItem Item = (TreeViewItem)VisualUpwardSearch<TreeViewItem>(cb);

            Node node = Item.DataContext as Node;

            MessageBox.Show(string.Format("{0} is Click({1})", node.Name, node.bClick));
        }

        DependencyObject VisualUpwardSearch<T>(DependencyObject source)  //搜寻可视话树的父节点
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);
            return source;
        }

        string treeStr = "";
        void printVisualTree(int depth, DependencyObject obj)
        {
            treeStr += new string(' ', depth) + obj + "\r\n"; //打印空格，方便查看

            //递归打印视觉树
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                printVisualTree(depth + 1, VisualTreeHelper.GetChild(obj, i));
            }
        }

        private void tvTreeViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;

            item.Focusable = true;
            item.Focus();
            item.Focusable = false;
            e.Handled = true;

            MessageBox.Show("TreeViewItem MouseLeftButtonDown");
        }
        private void Contract_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem root = (TreeViewItem)tvTreeView.ItemContainerGenerator.ContainerFromIndex(0);
            if (root != null)
            {
                CollapseTreeviewItems(root);
            }
        }


        private void CollapseTreeviewItems(TreeViewItem Item)
        {
            Item.IsExpanded = false;

            foreach (var item in Item.Items)
            {
                DependencyObject dObject = tvTreeView.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = false;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems(((TreeViewItem)dObject));
                    }
                }
            }
        }
    }

    public class Node
    {
        public Node()
        {
            this.Nodes = new List<Node>();
            this.ParentID = -1;
            this.Image = "D:/Code/C#/001_TreeView_Model/001_TreeView_Model/link.png";
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool bClick { get; set; }

        public int ParentID { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
