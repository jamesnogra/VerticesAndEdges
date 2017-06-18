using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VertexAndEdges
{

    public partial class VertexAndEdges : Form
    {
        private Vertex[] vertices = new Vertex[256];
        private int lastIndex = 0;
        private string mainColor = "#000000";

        public VertexAndEdges()
        {
            InitializeComponent();
        }

        private void VertexAndEdges_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mainPicBox_Click(object sender, EventArgs e)
        {
            int mouseX, mouseY;
            MouseEventArgs me = (MouseEventArgs)e;
            mouseX = me.X;
            mouseY = me.Y;
            //MessageBox.Show("X is " + this.mouseX + " and Y is " + this.mouseY);
            drawVertexAndLabel((lastIndex+1).ToString(), mouseX, mouseY);
            updateFromAndToVertexComboBox();
            saveToVertex(lastIndex, mouseX, mouseY);
        }

        private void saveToVertex(int indexNumber, int mouseX, int mouseY)
        {
            vertices[lastIndex] = new Vertex(indexNumber, mouseX, mouseY);
            lastIndex++;
        }

        private void updateFromAndToVertexComboBox()
        {
            fromVertexComboBox.Items.Clear();
            toVertexComboBox.Items.Clear();
            for (int x=0; x<=lastIndex;x++)
            {
                fromVertexComboBox.Items.Add(x + 1);
                toVertexComboBox.Items.Add(x + 1);
            }
        }

        private void connectVertexButton_Click(object sender, EventArgs e)
        {
            int fromVertex=1, toVertex=1;
            if (!Int32.TryParse(fromVertexComboBox.Text, out fromVertex) || !Int32.TryParse(toVertexComboBox.Text, out toVertex))
            {
                MessageBox.Show("Invalid vertex selected.");
                return;
            }
            //MessageBox.Show("Connect from " + vertices[fromVertex - 1].indexNumber + " to " + vertices[toVertex - 1].indexNumber);

            //connect the vertices in the variable
            if (updateNeighbors(fromVertex, toVertex))
            {
                drawEdgeLineAndRelabel(
                    (vertices[fromVertex - 1].indexNumber + 1).ToString(),
                    (vertices[toVertex - 1].indexNumber + 1).ToString(),
                    vertices[fromVertex - 1].mouseX,
                    vertices[fromVertex - 1].mouseY,
                    vertices[toVertex - 1].mouseX,
                    vertices[toVertex - 1].mouseY
                );
            }
        }

        public void drawVertexAndLabel(string label, int posX, int posY)
        {
            //draw the circle vertex
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Brush blackPen = new SolidBrush(ColorTranslator.FromHtml(mainColor));
            Font myFont = new Font("Arial", 12);
            g.FillEllipse(blackPen, posX - 20, posY - 20, 40, 40);
            g.DrawString(label, myFont, Brushes.White, new Point(posX - 9, posY - 8));
        }

        public void drawEdgeLineAndRelabel(string label1, string label2, int posX1, int posY1, int posX2, int posY2)
        {
            //then draw the line
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Pen blackPen = new Pen(ColorTranslator.FromHtml(mainColor), 3);
            g.DrawLine(blackPen, posX1, posY1, posX2, posY2);

            //draw the label again because the line might have overlapped it
            Font myFont = new Font("Arial", 12);
            g.DrawString(label1, myFont, Brushes.White, new Point(posX1 - 9, posY1 - 8));
            g.DrawString(label2, myFont, Brushes.White, new Point(posX2 - 9, posY2 - 8));
        }

        private bool updateNeighbors(int fromVertex, int toVertex)
        {
            try
            {
                //iterate first through the List if they are already neighbors
                if (!checkIfListContains(vertices[fromVertex - 1].neighbors, toVertex - 1))
                {
                    vertices[fromVertex - 1].neighbors.Add(toVertex - 1);
                    vertices[toVertex - 1].neighbors.Add(fromVertex - 1);
                }
                else
                {
                    MessageBox.Show("That edge from " + fromVertex + " to " + toVertex + " already exists.");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("That edge from " + fromVertex + " to " + toVertex + " does not exist.");
                return false;
            }
            return true;
        }

        private bool checkIfListContains(List<int> tempList, int key)
        {
            foreach (int x in tempList)
            {
                if (x == key)
                {
                    return true;
                }
            }
            return false;
        }

        private void showAdjacencyMatrixButton_Click(object sender, EventArgs e)
        {
            string tempText = "\t";
            for (int z = 0; z < lastIndex; z++)
            {
                tempText += (vertices[z].indexNumber + 1) + "\t";
            }
            tempText = tempText.Remove(tempText.Length-1);
            for (int x = 0; x < lastIndex; x++)
            {
                tempText += "\n\n"+(x+1);
                for (int y = 0; y < lastIndex; y++)
                {
                    if (x == y)
                    {
                        tempText += "\t0";
                    }
                    else
                    {
                        if (checkIfListContains(vertices[x].neighbors, y))
                        {
                            tempText += "\t1";
                        }
                        else if (checkIfListContains(vertices[y].neighbors, x))
                        {
                            tempText += "\t1";
                        }
                        else
                        {
                            tempText += "\t0";
                        }
                    }
                }
            }
            MessageBox.Show(tempText, "Adjacency Matrix");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempText = "";
            for (int x=0; x < lastIndex; x++)
            {
                tempText += vertices[x].indexNumber + " ";
                tempText += vertices[x].mouseX + " " + vertices[x].mouseY + " ";
                foreach (int neighbor in vertices[x].neighbors)
                {
                    tempText += neighbor + " ";
                }
                tempText = tempText.Remove(tempText.Length - 1);
                tempText += "\n";
            }
            tempText = tempText.Remove(tempText.Length - 1);
            //MessageBox.Show(tempText);

            //then save to text file
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "TempGraph.txt";
            saveFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFile.FileName, tempText);
            }
        }

        private void openGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readText = "";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filePath = openFileDialog1.FileName;
                try
                {
                    readText = File.ReadAllText(filePath);
                    insertAllVerticesAndEdges(readText);
                }
                catch (IOException) { }
            }
            //MessageBox.Show(readText);
        }

        private void insertAllVerticesAndEdges(string tempString)
        {
            lastIndex = 0; //reset the index before reinserting all vertices and edges
            string[] verticesLine = tempString.Split('\n');
            for (int x = 0; x < verticesLine.Length; x++)
            {
                string[] allElements = verticesLine[x].Split(' ');
                try
                {
                    vertices[x] = new Vertex(Int32.Parse(allElements[0]), Int32.Parse(allElements[1]), Int32.Parse(allElements[2]));
                    //reinsert the neighbors of this vertex
                    for (int y = 3; y < allElements.Length; y++)
                    {
                        vertices[x].neighbors.Add(Int32.Parse(allElements[y]));
                    }
                    updateFromAndToVertexComboBox();
                    lastIndex++;
                }
                catch (Exception)
                {
                    MessageBox.Show("Something is wrong with that file.");
                }
                
            }

            //clear the picturebox
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Brush blackPen = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
            g.FillRectangle(blackPen, 0, 0, mainPicBox.Width, mainPicBox.Height);
            //then lets draw the vertices and edges
            for (int x = 0; x < lastIndex; x++)
            {
                //MessageBox.Show("Drawing vertex " + vertices[x].indexNumber.ToString());
                drawVertexAndLabel((vertices[x].indexNumber+1).ToString(), vertices[x].mouseX, vertices[x].mouseY);
                foreach (int neighbor in vertices[x].neighbors)
                {
                    drawEdgeLineAndRelabel(
                        (vertices[x].indexNumber+1).ToString(),
                        (vertices[neighbor].indexNumber+1).ToString(),
                        vertices[x].mouseX,
                        vertices[x].mouseY,
                        vertices[neighbor].mouseX,
                        vertices[neighbor].mouseY
                    );
                }
            }
        }
    }

    public class Vertex
    {
        public int indexNumber;
        public int mouseX;
        public int mouseY;
        public List<int> neighbors;

        public Vertex(int indexNumber, int mouseX, int mouseY)
        {
            this.indexNumber = indexNumber;
            this.mouseX = mouseX;
            this.mouseY = mouseY;
            neighbors = new List<int>();
        }

        public override string ToString()
        {
            string tempString = "Index: "+this.indexNumber;
            tempString += "\nPos: "+this.mouseX+","+this.mouseY;
            tempString += "\nNeighbors: ";
            foreach (int a in this.neighbors)
            {
                tempString += a.ToString() + " ";
            }
            return tempString;
        }
    }
}
