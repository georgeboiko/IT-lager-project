using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tooltip
{
    public class DistanceCalc
    {
        public int[] parent;
        int n;
        public List<List<int>> matrix;
        public int[] pos;
        InputForm inForm;
        public void calc(List<Pnt> points, List<between2Points> distances, List<TextBox> distance_values, InputForm _inForm)
        {
            try
            {
                this.inForm = _inForm;
                const int INF = 1000000000;
                int n = points.Count, min = 0, index_min = 0;
                Array.Resize(ref parent, n);
                for (int i = 0; i < parent.Length; i++)
                {
                    parent[i] = -1;
                }

                between2Points tempb;
                if (n > 2)
                {
                    for (int i = 0; i < n; i++)
                    {
                        try
                        {
                            distances[i] = new between2Points(inForm.pointsHandler.integratedPoints[i].First.First, inForm.pointsHandler.integratedPoints[i].Second.First, Convert.ToInt32(distance_values[i].Text));
                        }
                        catch { }
                    }

                    for (int i = 0; i < matrix.Count; i++)
                    {
                        try
                        {
                            matrix[distances[i].index1][distances[i].index2] = distances[i].distance;
                        }
                        catch { }
                        try
                        {
                            matrix[distances[i].index2][distances[i].index1] = distances[i].distance;
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        distances[0] = new between2Points(distances[0].index1, distances[0].index2, Convert.ToInt32(distance_values[0].Text));
                        matrix[distances[0].index1][distances[0].index2] = distances[0].distance;
                        matrix[distances[0].index2][distances[0].index1] = distances[0].distance;
                    }
                    catch { }
                }

                pos = new int[n];
                for (int i = 0; i < n; i++)
                {
                    pos[i] = INF;
                }
                bool[] visited = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    visited[i] = false;
                }
                pos[0] = 0;
                for (int i = 0; i < n - 1; i++)
                {
                    min = INF;
                    for (int j = 0; j < n - 1; j++)
                    {
                        if (!visited[j] && pos[j] < min)
                        {
                            min = pos[j];
                            index_min = j;
                        }
                    }
                    visited[index_min] = true;
                    for (int j = 0; j < n; j++)
                    {
                        if ((!visited[j]) && (matrix[index_min][j] > 0) && (pos[index_min] != INF)
                         && (pos[index_min] + matrix[index_min][j] < pos[j]))
                        {
                            pos[j] = pos[index_min] + matrix[index_min][j];
                            parent[j] = index_min;
                        }
                    }
                }
            }
            catch { }
        }
        public DistanceCalc(int n)
        {
            this.matrix = new List<List<int>>() { };
            this.parent = new int[n];
            for (int i = 0; i < n; i++)
            {
                List<int> temp = new List<int>() { };
                for (int j = 0; j < n; j++)
                {
                    temp.Add(0);
                }
                this.matrix.Add(temp);
            }
        }
    }
}