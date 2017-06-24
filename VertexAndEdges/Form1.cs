using System;
using System.Collections;
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
        private static int maxSize = 256;
        private Vertex[] vertices = new Vertex[maxSize];
        private int lastIndex = 0;
        const string mainColor = "#000000";

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
            drawVertexAndLabel((lastIndex+1).ToString(), mouseX, mouseY, null);
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
            startComboBox.Items.Clear();
            endComboBox.Items.Clear();
            for (int x=0; x<=lastIndex;x++)
            {
                fromVertexComboBox.Items.Add(x + 1);
                toVertexComboBox.Items.Add(x + 1);
                startComboBox.Items.Add(x + 1);
                endComboBox.Items.Add(x + 1);
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
                    vertices[toVertex - 1].mouseY,
                    null
                );
            }
        }

        public void drawVertexAndLabel(string label, int posX, int posY, string customColor)
        {
            string vertexColor = mainColor;
            if (customColor != null)
            {
                vertexColor = customColor;
            }
            //draw the circle vertex
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Brush blackPen = new SolidBrush(ColorTranslator.FromHtml(vertexColor));
            Font myFont = new Font("Arial", 12);
            g.FillEllipse(blackPen, posX - 20, posY - 20, 40, 40);
            g.DrawString(label, myFont, Brushes.White, new Point(posX - 9, posY - 8));
        }

        public void drawEdgeLineAndRelabel(string label1, string label2, int posX1, int posY1, int posX2, int posY2, string customColor)
        {
            string lineColor = mainColor;
            if (customColor != null)
            {
                lineColor = customColor;
            }
            //then draw the line
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Pen blackPen = new Pen(ColorTranslator.FromHtml(lineColor), 3);
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
            Brush whitePen = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
            g.FillRectangle(whitePen, 0, 0, mainPicBox.Width, mainPicBox.Height);
            //then lets draw the vertices and edges
            for (int x = 0; x < lastIndex; x++)
            {
                //MessageBox.Show("Drawing vertex " + vertices[x].indexNumber.ToString());
                drawVertexAndLabel((vertices[x].indexNumber+1).ToString(), vertices[x].mouseX, vertices[x].mouseY, null);
                foreach (int neighbor in vertices[x].neighbors)
                {
                    drawEdgeLineAndRelabel(
                        (vertices[x].indexNumber+1).ToString(),
                        (vertices[neighbor].indexNumber+1).ToString(),
                        vertices[x].mouseX,
                        vertices[x].mouseY,
                        vertices[neighbor].mouseX,
                        vertices[neighbor].mouseY,
                        null
                    );
                }
            }
        }

        private void dFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int startVertex = 1, endVertex = 1;
            if (!Int32.TryParse(startComboBox.Text, out startVertex) || !Int32.TryParse(endComboBox.Text, out endVertex))
            {
                MessageBox.Show("Invalid vertex selected.");
                return;
            }
            startVertex--;
            endVertex--;
            //MessageBox.Show("Travelling from "+startVertex+" to "+endVertex);

            string tempLogs = "";
            Stack tempVertices = new Stack();
            List<int> finalPath = new List<int>();
            List<int> visitedPaths = new List<int>();
            tempVertices.Push(startVertex); //push first vertex
            visitedPaths.Add(startVertex); //assume start vertex has been visited
            tempLogs += "PUSH\t" + (startVertex + 1) + "\n";
            bool isDone = true;
            int currentVertex;
            while(isDone)
            {
                currentVertex = Int32.Parse(tempVertices.Pop().ToString());
                tempLogs += "POP\t" + (currentVertex + 1) + "\n";
                finalPath.Add(currentVertex);
                if (currentVertex == endVertex)
                {
                    isDone = false;
                    break;
                }
                vertices[currentVertex].neighbors.Sort(); //lets sort before pushing the neighbors to the stack
                foreach (int vertex in vertices[currentVertex].neighbors)
                {
                    if (!checkIfListContains(visitedPaths, vertex))
                    {
                        tempVertices.Push(vertex);
                        visitedPaths.Add(vertex);
                        tempLogs += "PUSH\t" + (vertex + 1) + "\n";
                    }
                }
                if (tempVertices.Count == 0) //check if the stack is empty
                {
                    isDone = false;
                }
            }
            //check if the endVertex is in the finalPath's list
            if (checkIfListContains(finalPath, endVertex))
            {
                //check if showing of logs is enabled
                if (showLog.Checked)
                {
                    tempLogs += "\nPath: " + printVertexList(finalPath);
                    MessageBox.Show(tempLogs);
                }
                else
                {
                    MessageBox.Show("Path: " + printVertexList(finalPath));
                }
                int speed = 0;
                if (animate.Checked)
                {
                    speed = 500;
                }
                traverseFinalPath(finalPath, speed, "#33CC33");
                System.Threading.Thread.Sleep(2000);
                traverseFinalPath(finalPath, 0, mainColor); //reset graph
            }
            
        }

        private void bFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int startVertex = 1, endVertex = 1;
            if (!Int32.TryParse(startComboBox.Text, out startVertex) || !Int32.TryParse(endComboBox.Text, out endVertex))
            {
                MessageBox.Show("Invalid vertex selected.");
                return;
            }
            startVertex--;
            endVertex--;
            //MessageBox.Show("Travelling from "+startVertex+" to "+endVertex);

            string tempLogs = "";
            Queue tempVertices = new Queue();
            List<int> finalPath = new List<int>();
            List<int> visitedPaths = new List<int>();
            tempVertices.Enqueue(startVertex); //push first vertex
            visitedPaths.Add(startVertex); //assume start vertex has been visited
            tempLogs += "PUSH\t" + (startVertex + 1) + "\n";
            bool isDone = true;
            int currentVertex;
            while (isDone)
            {
                currentVertex = Int32.Parse(tempVertices.Dequeue().ToString());
                tempLogs += "POP\t" + (currentVertex + 1) + "\n";
                finalPath.Add(currentVertex);
                if (currentVertex == endVertex)
                {
                    isDone = false;
                    break;
                }
                vertices[currentVertex].neighbors.Sort(); //lets sort before pushing the neighbors to the stack
                foreach (int vertex in vertices[currentVertex].neighbors)
                {
                    if (!checkIfListContains(visitedPaths, vertex))
                    {
                        tempVertices.Enqueue(vertex);
                        visitedPaths.Add(vertex);
                        tempLogs += "PUSH\t" + (vertex + 1) + "\n";
                    }
                }
                if (tempVertices.Count == 0) //check if the stack is empty
                {
                    isDone = false;
                }
            }
            //check if the endVertex is in the finalPath's list
            if (checkIfListContains(finalPath, endVertex))
            {
                //check if showing of logs is enabled
                if (showLog.Checked)
                {
                    tempLogs += "\nPath: " + printVertexList(finalPath);
                    MessageBox.Show(tempLogs);
                }
                else
                {
                    MessageBox.Show("Path: " + printVertexList(finalPath));
                }
                int speed = 0;
                if (animate.Checked)
                {
                    speed = 1000;
                }
                traverseFinalPath(finalPath, speed, "#33CC33");
                System.Threading.Thread.Sleep(2000);
                traverseFinalPath(finalPath, 0, mainColor); //reset graph
            }
        }

        public void traverseFinalPath(List<int> tempList, int speed, string customColor)
        {
            //MessageBox.Show("Path Received: " + printVertexList(tempList));
            for (int x=0; x<tempList.Count; x++)
            {
                //MessageBox.Show("At index " + vertices[tempList[x]].indexNumber + " then added with 1 so " + (vertices[tempList[x]].indexNumber + 1).ToString());
                drawVertexAndLabel(
                    (vertices[tempList[x]].indexNumber + 1).ToString(),
                    vertices[tempList[x]].mouseX,
                    vertices[tempList[x]].mouseY,
                    customColor
                );
                System.Threading.Thread.Sleep(speed);
                //MessageBox.Show(vertices[tempList[x]].indexNumber+" Drawing from " + (vertices[tempList[x]].indexNumber+1) + " to " + (vertices[tempList[x]].indexNumber + 2));
                //MessageBox.Show("At x="+x+" and vertex "+ (vertices[tempList[x]].indexNumber + 1));
                if (x<tempList.Count-1)
                {
                    //only draw an animated edge if the two vertices are neighbors
                    if (checkIfListContains(vertices[tempList[x]].neighbors, vertices[tempList[x + 1]].indexNumber))
                    {
                        drawEdgeLineAndRelabel(
                            (vertices[tempList[x]].indexNumber + 1).ToString(),
                            (vertices[tempList[x + 1]].indexNumber + 1).ToString(),
                            vertices[tempList[x]].mouseX,
                            vertices[tempList[x]].mouseY,
                            vertices[tempList[x + 1]].mouseX,
                            vertices[tempList[x + 1]].mouseY,
                            customColor
                        );
                    }
                    else
                    {
                        int y = x+1; //we are adding +1 here because the next vertex is not it's neighbor
                        while (y >= 0)
                        {
                            y--;
                            if (checkIfListContains(vertices[tempList[x+1]].neighbors, (vertices[tempList[y]].indexNumber)))
                            {
                                drawEdgeLineAndRelabel(
                                    (vertices[tempList[x+1]].indexNumber + 1).ToString(),
                                    (vertices[tempList[y]].indexNumber + 1).ToString(),
                                    vertices[tempList[x+1]].mouseX,
                                    vertices[tempList[x+1]].mouseY,
                                    vertices[tempList[y]].mouseX,
                                    vertices[tempList[y]].mouseY,
                                    customColor
                                );
                                //MessageBox.Show("Backtrack connecting " + (vertices[tempList[x+1]].indexNumber+1) + " and " + (vertices[tempList[y]].indexNumber+1) + " at x="+x+" and y="+y);
                                break;
                            }
                        }
                    }
                }                
                System.Threading.Thread.Sleep(speed);
            }
        }

        public string printVertexList(List<int> tempList)
        {
            string tempString = "";
            foreach (int x in tempList)
            {
                tempString += (x+1) + " ";
            }
            return tempString;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lastIndex = 0;
            Bitmap bmp = new Bitmap(mainPicBox.Width, mainPicBox.Height);
            Graphics g = mainPicBox.CreateGraphics();
            Brush whitePen = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
            g.FillRectangle(whitePen, 0, 0, mainPicBox.Width, mainPicBox.Height);
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
