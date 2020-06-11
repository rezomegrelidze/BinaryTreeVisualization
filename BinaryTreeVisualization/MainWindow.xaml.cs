using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BinaryTreeVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BinaryTree tree;
        private double initialWidthDelta = 400;
        private double initialHeightDelta = 100;

        public MainWindow()
        {
            InitializeComponent();

            ConstructSampleTree();

            DrawBinaryTree();
        }

        private void DrawBinaryTree()
        {
            double X = treeCanvas.Width / 2.0; // initial x-coord at canvas center
            double Y = 30;                     // initial y-coord slightly below top point
            DrawBinaryTree(tree.Root,X,Y,initialWidthDelta,initialHeightDelta);
        }

        void DrawBinaryTree(BinaryTree.Node node,double x,double y,double widthDelta,double heightDelta)
        {


            int nodeWidth = 5;
            int nodeHeight = 5;

            if(node == null) return;

            Ellipse ellipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Blue),
                Width = nodeWidth,
                Height = nodeHeight
            };
            ellipse.SetValue(Canvas.LeftProperty,x);
            ellipse.SetValue(Canvas.TopProperty,y);

            if (node.Left != null) // draw left leg
            {
                DrawBinaryTree(node.Left, x - widthDelta  , y + heightDelta,widthDelta * 0.45,heightDelta * 0.85);

                Line line = new Line()
                {
                    X1 = x + ellipse.Width / 2.0,
                    Y1 = y + ellipse.Height / 2.0,
                    X2 = x - widthDelta  + ellipse.Width / 2.0,
                    Y2 = y + heightDelta  + ellipse.Height / 2.0,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Black)
                };

                treeCanvas.Children.Add(line);
            }


            if (node.Right != null) // draw right leg
            {
                DrawBinaryTree(node.Right, x + widthDelta , y + heightDelta,widthDelta * 0.45,heightDelta * 0.85);

                Line line2 = new Line()
                {
                    X1 = x + ellipse.Width / 2.0,
                    Y1 = y + ellipse.Height / 2.0,
                    X2 = x + widthDelta + ellipse.Width / 2.0,
                    Y2 = y + heightDelta + ellipse.Height / 2.0,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Black)
                };

                treeCanvas.Children.Add(line2);
            }

            ellipse.SetValue(Panel.ZIndexProperty,1);
            treeCanvas.Children.Add(ellipse);
        }

        private void ConstructSampleTree()
        {
            tree = new BinaryTree();
            Random rand = new Random();
            for (int i = 0; i < 500; i++)
            {
                tree.Insert(rand.Next(),i);
            }
        }
    }

    public class BinaryTree : IEnumerable<int>
    {
        public class Node
        {
            public Node(int key, int value) => (Key, Value) = (key, value);

            public int Value { get; set; }
            public int Key { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        private Node root;

        public int Get(int searchKey)
        {
            int GetImpl(int key, Node x)
            {
                if (x.Key == key) return key;
                else if (key < x.Key) return GetImpl(key, x.Left);
                else return GetImpl(key, x.Right);
            }

            return GetImpl(searchKey, root);
        }

        public void Insert(int key, int value)
        {
            root = Insert(key, value, root);
        }

        private Node Insert(int key, int value, Node x)
        {
            if (x == null)
            {
                x = new Node(key, value);
            }
            else if (x.Key == key)
            {
                x.Value = value;
            }
            else if (key < x.Key)
            {
                x.Left = Insert(key, value, x.Left);
            }
            else x.Right = Insert(key, value, x.Right);

            return x;
        }

        public Node Root => root;

        public IEnumerator<int> GetEnumerator()
        {
            return Traverse(root).GetEnumerator();
        }

        private IEnumerable<int> Traverse(Node node)
        {
            if(node == null) yield break;
            foreach (var value in Traverse(node.Left)) yield return value;
            yield return node.Value;
            foreach (var value in Traverse(node.Right)) yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
