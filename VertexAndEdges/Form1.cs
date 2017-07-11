using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

            //draw the length of the edge
            myFont = new Font("Arial", 10);
            g.DrawString(
                calculateEdgeLength(posX1, posY1, posX2, posY2).ToString(),
                myFont,
                Brushes.Red,
                new Point(calculateMiddlePoint(posX1, posX2), calculateMiddlePoint(posY1, posY2))
            );
        }

        private int calculateEdgeLength(int posX1, int posY1, int posX2, int posY2)
        {
            int tempRes;
            tempRes = (int)(Math.Sqrt(Math.Pow((posX2 - posX1), 2) + Math.Pow((posY2 - posY1), 2)));
            return tempRes;
        }

        private int calculateMiddlePoint(int a, int b)
        {
            if (a < b)
            {
                return ((b - a) / 2) + a;
            }
            return (a - b) / 2 + b;
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
                    //MessageBox.Show("That edge from " + fromVertex + " to " + toVertex + " already exists.");
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
            else
            {
                MessageBox.Show("No path found.");
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
                    speed = 500;
                }
                traverseFinalPath(finalPath, speed, "#33CC33");
                System.Threading.Thread.Sleep(2000);
                traverseFinalPath(finalPath, 0, mainColor); //reset graph
            }
            else
            {
                MessageBox.Show("No path found.");
            }
        }

        private void greedyBFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int startVertex = 1, endVertex = 1;
            if (!Int32.TryParse(startComboBox.Text, out startVertex) || !Int32.TryParse(endComboBox.Text, out endVertex))
            {
                MessageBox.Show("Invalid vertex selected.");
                return;
            }
            startVertex--;
            endVertex--;

            List<int> finalPath = new List<int>();
            List<int> vertexPushed = new List<int>();
            bool isDone = false;
            int atVertex;
            string tempLogs = "";

            //push the start vertex
            vertexPushed.Add(startVertex);
            while (!isDone)
            {
                //remove the last item in the list of vertexPushed because it's the shortest
                atVertex = vertexPushed.ElementAt(vertexPushed.Count - 1);
                tempLogs += "\nRemoved and added to FinalPath vertex " + (atVertex + 1);
                if (!checkIfListContains(finalPath, atVertex))
                {
                    finalPath.Add(atVertex);
                }
                vertexPushed.Remove(vertexPushed.Last());
                //MessageBox.Show("REMOVE " + (atVertex + 1) + "\n\n"+printVertexList(vertexPushed) + "\n\nRemoving at Index: " + (vertexPushed.Count - 1));

                //check if the removed vertex is the goal vertex
                if (atVertex == endVertex)
                {
                    isDone = true;
                    //MessageBox.Show("Final Path is: " + printVertexList(finalPath));
                    break;
                }

                //add the neighbors of this vertex to vertexPushed
                foreach (int vertex in vertices[atVertex].neighbors)
                {
                    if (!checkIfListContains(vertexPushed, vertex) && !checkIfListContains(finalPath,vertex))
                    {
                        //MessageBox.Show("ADDING: " + (vertex+1));
                        tempLogs += "\nAdding vertex " + (vertex + 1);
                        vertexPushed.Add(vertex);
                    }
                }
                
                //rearrange the list of vertices pushed according to the distance from goal
                //the first element should have the longest direct distance to goal
                //while the last element will have the shortest direct distance to goal
                vertexPushed = sortListOfVerticesToGoal(vertexPushed, endVertex);
                tempLogs += "\nSorting vertices.";
                //MessageBox.Show("After: " + printVertexList(vertexPushed));

                if (vertexPushed.Count == 0)
                {
                    isDone = true;
                }
            }

            //let's check if we found the goal
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
            else
            {
                MessageBox.Show("No path found.");
            }

        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int startVertex = 1, endVertex = 1;
            if (!Int32.TryParse(startComboBox.Text, out startVertex) || !Int32.TryParse(endComboBox.Text, out endVertex))
            {
                MessageBox.Show("Invalid vertex selected.");
                return;
            }
            startVertex--;
            endVertex--;

            List<AStarVertex> aStar = new List<AStarVertex>();
            List<AStarVertex> tempAStar = new List<AStarVertex>();
            List<int> finalPath = new List<int>();
            AStarVertex tempVertex;
            int tempTotalCost;
            bool isDone = false;

            aStar.Add(new AStarVertex(startVertex, -1, 0)); //add the start vertex

            while (!isDone)
            {
                tempVertex = new AStarVertex(aStar.ElementAt(aStar.Count-1));
                tempAStar.Add(tempVertex);
                finalPath.Add(tempVertex.indexNumber);
                aStar.Remove(aStar.Last()); //remove the vertex with the lowest cost
                //MessageBox.Show("Removing: " + (tempVertex.indexNumber+1));
                
                if (tempVertex.indexNumber == endVertex) //check if current vertex is the goal
                {
                    isDone = true;
                    break;
                }

                vertices[tempVertex.indexNumber].neighbors.Sort();
                foreach (int vertex in vertices[tempVertex.indexNumber].neighbors)
                {
                    //MessageBox.Show("Adding: " + (vertex+1));
                    tempTotalCost = calculateEdgeLength(
                        vertices[vertex].mouseX,
                        vertices[vertex].mouseY,
                        vertices[tempVertex.indexNumber].mouseX, 
                        vertices[tempVertex.indexNumber].mouseY
                    );
                    aStar.Add(new AStarVertex(vertex, tempVertex.indexNumber, tempTotalCost + tempVertex.totalCost));
                }

                //sort the list of vertices after adding
                //starting from the longest to shortest (last element has the lowest goal cost)
                aStar = sortListAStarVertex(aStar, endVertex);
                //MessageBox.Show("Sorting");
            }
            //MessageBox.Show(printAStarVertexList(tempAStar));

            //clean the final path for detours
            /*finalPath = finalPath.Distinct().ToList<int>();
            int at = finalPath.Count - 1;
            while (at > 0)
            {
                if (!checkIfListContains(vertices[finalPath[at]].neighbors, vertices[finalPath[at-1]].indexNumber))
                {
                    //MessageBox.Show("Removed detour at vertex " + (vertices[finalPath[at - 1]].indexNumber + 1));
                    finalPath.RemoveAt(at - 1);
                }
                at--;
            }
            finalPath = removeLongDetoursAStar(finalPath);*/
            //get the clean path using parent
            List<int> superFinalPath = new List<int>();
            int at = tempAStar.Count - 1;
            //add the indexNumber and parent of the last element of tempAStar
            superFinalPath.Add(tempAStar[at].indexNumber);
            while (at >= 0)
            {
                //MessageBox.Show("Adding vertex " + (tempAStar[at].indexNumber+1) + " and parent " + (tempAStar[at].parentVertex+1) + " at index " + at);
                if (tempAStar[at].parentVertex != -1)
                {
                    superFinalPath.Add(tempAStar[at].parentVertex);
                }
                if (at == 0)
                {
                    break;
                }
                for (int y=0; y<tempAStar.Count; y++)
                {
                    if (tempAStar[y].indexNumber == tempAStar[at].parentVertex)
                    {
                        //MessageBox.Show("Changing x from " + at + " to " + y);
                        at = y;
                        y = tempAStar.Count; //end y loop
                    }
                }
            }
            superFinalPath.Reverse();
            finalPath = superFinalPath;


            //check if showing of logs is enabled
            if (showLog.Checked)
            {
                MessageBox.Show(printAStarVertexList(tempAStar));
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

        public List<int> removeLongDetoursAStar(List<int> theList)
        {
            List<int> tempList = theList;

            for (int x=0; x<tempList.Count-1; x++)
            {
                for (int y=x+1; y<tempList.Count; y++)
                {
                    //MessageBox.Show("Is " + (vertices[tempList[y]].indexNumber + 1) + " a neighbor of " + (vertices[tempList[x]].indexNumber + 1));
                    if (checkIfListContains(vertices[tempList[x]].neighbors, vertices[tempList[y]].indexNumber))
                    {
                        //MessageBox.Show("Deleting in between " + x + " and " + y);
                        //then delete the vertices in between
                        for (int z=(x+1); z<y; z++)
                        {
                            tempList.RemoveAt(z);
                            //MessageBox.Show("Removed detour at " + vertices[tempList[z]]);
                        }
                    }
                }
            }

            return tempList;
        }

        public List<AStarVertex> sortListAStarVertex(List<AStarVertex> theList, int goalVertex)
        {
            List<AStarVertex> tempList = theList;
            AStarVertex tempVertex;
            int length1, length2;

            for (int x=0; x < tempList.Count; x++)
            {
                for (int y=0; y<tempList.Count; y++)
                {
                    //aside from the total path cost, we will also add the
                    //heuristic current vertex to goal length to the f(n)
                    length1 = tempList[x].totalCost + calculateEdgeLength(
                        vertices[tempList[x].indexNumber].mouseX,
                        vertices[tempList[x].indexNumber].mouseY,
                        vertices[goalVertex].mouseX,
                        vertices[goalVertex].mouseY
                    );
                    length2 = tempList[y].totalCost + calculateEdgeLength(
                        vertices[tempList[y].indexNumber].mouseX,
                        vertices[tempList[y].indexNumber].mouseY,
                        vertices[goalVertex].mouseX,
                        vertices[goalVertex].mouseY
                    );
                    if (length2 < length1)
                    {
                        tempVertex = tempList[y];
                        tempList[y] = tempList[x];
                        tempList[x] = tempVertex;
                    }
                }
            }

            return tempList;
        }

        public List<int> sortListOfVerticesToGoal(List<int> theList, int goalVertex)
        {
            List<int> sortedList = theList;
            int length1, length2;
            int tempVertex;
            for (int x=0; x<sortedList.Count; x++)
            {
                for (int y=0; y<sortedList.Count; y++)
                {
                    length1 = calculateEdgeLength(
                        vertices[sortedList[x]].mouseX,
                        vertices[sortedList[x]].mouseY,
                        vertices[goalVertex].mouseX,
                        vertices[goalVertex].mouseY
                    );
                    length2 = calculateEdgeLength(
                        vertices[sortedList[y]].mouseX,
                        vertices[sortedList[y]].mouseY,
                        vertices[goalVertex].mouseX,
                        vertices[goalVertex].mouseY
                    );
                    if (length2 < length1)
                    {
                        tempVertex = sortedList[y];
                        sortedList[y] = theList[x];
                        sortedList[x] = tempVertex;
                    }
                }
            }
            return sortedList;
        }

        private void geneticAlgorithmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int MaxGenerations = 25;
            //complete the graph first
            int fromVertex, toVertex;
            MessageBox.Show("This will make the graph complete.");
            for (int x=1; x<=lastIndex; x++)
            {
                for (int y=1; y<=lastIndex; y++)
                {
                    if (x != y)
                    {
                        fromVertex = x;
                        toVertex = y;
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
                }
            }

            //randomize the first four paths for generation 1
            List<VerticesGeneticAlgorithm> listOfFamily = new List<VerticesGeneticAlgorithm>();
            VerticesGeneticAlgorithm tempRand;
            int[] tempPath = randomizeListForPath();
            int[] emptyParent = new int[lastIndex];
            tempRand = new VerticesGeneticAlgorithm(emptyParent, emptyParent, 0, calculateTotalLength(tempPath), tempPath);
            listOfFamily.Add(tempRand);
            tempPath = randomizeListForPath();
            tempRand = new VerticesGeneticAlgorithm(emptyParent, emptyParent, 0, calculateTotalLength(tempPath), tempPath);
            listOfFamily.Add(tempRand);
            tempPath = randomizeListForPath();
            tempRand = new VerticesGeneticAlgorithm(emptyParent, emptyParent, 0, calculateTotalLength(tempPath), tempPath);
            listOfFamily.Add(tempRand);
            tempPath = randomizeListForPath();
            tempRand = new VerticesGeneticAlgorithm(emptyParent, emptyParent, 0, calculateTotalLength(tempPath), tempPath);
            listOfFamily.Add(tempRand);

            //sort the listOfFamily so that the first elements have the least cost
            listOfFamily = sortAllFamily(listOfFamily);

            int upToPosCopy; //from what position will we crossover
            int[] temp1, temp2, temp3, parent1, parent2, parent3;
            int tempPathToMove;
            for (int x=1; x<MaxGenerations; x++)
            {
                //copy values of first three least cost
                temp1 = listOfFamily[0].path;
                parent1 = temp1;
                temp2 = listOfFamily[1].path;
                parent2 = temp2;
                temp3 = listOfFamily[2].path;
                parent3 = temp3;
                //crossover of 1st and 2nd least cost
                upToPosCopy = StaticRandom.Instance.Next(0, lastIndex/2); //we only crossover from pos 0 to max at the middle
                for (int y=0; y<upToPosCopy; y++)
                {
                    tempPathToMove = temp1[y];
                    temp1[y] = temp2[y];
                    temp2[y] = tempPathToMove;
                }
                tempRand = new VerticesGeneticAlgorithm(parent1, parent2, x, calculateTotalLength(temp1), temp1);
                listOfFamily.Add(tempRand);
                tempRand = new VerticesGeneticAlgorithm(parent1, parent2, x, calculateTotalLength(temp2), temp2);
                listOfFamily.Add(tempRand);
                //crossover of 1st and 3rd least cost
                upToPosCopy = StaticRandom.Instance.Next(0, lastIndex / 2); //we only crossover from pos 0 to max at the middle
                for (int y = 0; y < upToPosCopy; y++)
                {
                    tempPathToMove = temp1[y];
                    temp1[y] = temp3[y];
                    temp3[y] = tempPathToMove;
                }
                tempRand = new VerticesGeneticAlgorithm(parent1, parent3, x, calculateTotalLength(temp3), temp3);
                listOfFamily.Add(tempRand);
                //sort the listOfFamily so that the first elements have the least cost
                listOfFamily = sortAllFamily(listOfFamily);
            }

            string ts = "";
            foreach(VerticesGeneticAlgorithm a in listOfFamily)
            {
                ts += a.ToString();
            }
            Prompt.ShowDialog(ts, "List of Family");
        }

        public List<VerticesGeneticAlgorithm> sortAllFamily(List<VerticesGeneticAlgorithm> tempFamily)
        {
            List<VerticesGeneticAlgorithm> resultSortedFamily = new List<VerticesGeneticAlgorithm>();
            VerticesGeneticAlgorithm tempForTransfer;

            for (int x=0; x<tempFamily.Count; x++)
            {
                for (int y=x; y<tempFamily.Count; y++)
                {
                    if (tempFamily[x].totalCost > tempFamily[y].totalCost)
                    {
                        tempForTransfer = tempFamily[x];
                        tempFamily[x] = tempFamily[y];
                        tempFamily[y] = tempForTransfer;
                    }
                }
            }

            return tempFamily;
        }

        public int calculateTotalLength(int[] tempPath)
        {
            int totalCost = 0;

            for (int x=0; x<tempPath.Length-1; x++)
            {
                totalCost += calculateEdgeLength(vertices[tempPath[x]].mouseX, vertices[tempPath[x]].mouseY, vertices[tempPath[x+1]].mouseX, vertices[tempPath[x+1]].mouseY);
            }

            return totalCost;
        }

        public int[] randomizeListForPath()
        {
            int[] tempList = new int[lastIndex];
            for (int x=0; x<tempList.Length; x++)
            {
                tempList[x] = -1;
            }
            int whereToPut;
            for (int x=0; x<lastIndex; x++)
            {
                whereToPut = StaticRandom.Instance.Next(0, lastIndex);
                while(tempList[whereToPut] != -1)
                {
                    whereToPut = StaticRandom.Instance.Next(0, lastIndex);
                }
                //MessageBox.Show("Put " + x + " to position " + whereToPut);
                tempList[whereToPut] = x;
            }
            return tempList;
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

        public string printAStarVertexList(List<AStarVertex> tempList)
        {
            string tempString = "";
            foreach (AStarVertex tempVertex in tempList)
            {
                tempString += "Vertex: " + (tempVertex.indexNumber + 1) + "\tParent: " + (tempVertex.parentVertex + 1) + "\tPath Length: " + tempVertex.totalCost + "\n";
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
        public int distanceToGoal;
        public List<int> neighbors;

        public Vertex(int indexNumber, int mouseX, int mouseY)
        {
            this.indexNumber = indexNumber;
            this.mouseX = mouseX;
            this.mouseY = mouseY;
            this.distanceToGoal = int.MaxValue;
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

    public class AStarVertex
    {
        public int indexNumber;
        public int totalCost;
        public int parentVertex;

        public AStarVertex(int indexNumber, int parentVertex, int totalCost)
        {
            this.indexNumber = indexNumber;
            this.parentVertex = parentVertex;
            this.totalCost = totalCost;
        }

        public AStarVertex(AStarVertex vertex)
        {
            this.indexNumber = vertex.indexNumber;
            this.totalCost = vertex.totalCost;
            this.parentVertex = vertex.parentVertex;
        }
    }

    public class VerticesGeneticAlgorithm
    {
        public int[] parent1;
        public int[] parent2;
        public int generation;
        public int totalCost;
        public int[] path;
        
        public VerticesGeneticAlgorithm(int[] parent1, int[] parent2, int generation, int totalCost, int[] path)
        {
            this.parent1 = parent1;
            this.parent2 = parent2;
            this.generation = generation;
            this.totalCost = totalCost;
            this.path = path;
        }

        public override string ToString()
        {
            string tempString = "Generation: " + this.generation;
            tempString += "\tParent: ";
            for (int a = 0; a < parent1.Length; a++)
            {
                tempString += parent1[a] + " ";
            }
            tempString += " and ";
            for (int a = 0; a < parent2.Length; a++)
            {
                tempString += parent2[a] + " ";
            }
            tempString += "\tTotal Cost: " + this.totalCost;
            tempString += "\tPath: ";
            for (int a=0; a<path.Length; a++)
            {
                tempString += path[a] + " ";
            }
            return tempString + "\n";
        }
    }

    //from https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
    public static class StaticRandom
    {
        private static int seed;
        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }
    }

    //taken from https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 700,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            //Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 450 };
            TextBox textBox1 = new TextBox();
            textBox1.Multiline = true;
            textBox1.Text = text.Replace("\n", "\r\n");
            textBox1.Height = prompt.Height-20;
            textBox1.Width = prompt.Width-10;
            textBox1.ScrollBars = ScrollBars.Vertical;
            prompt.Controls.Add(textBox1);
            //prompt.Controls.Add(textLabel);
            return prompt.ShowDialog() == DialogResult.OK ? "" : "";
        }
    }
}
