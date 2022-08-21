using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tooltip
{
    public class Pnt : Panel
    {
        PictureBox PntImage;
        TextBox PntName;
        bool isDown = false;
        int num;
        string src;
/*        public Pnt(Pnt _p)
        {
            this.PntImage = ICloneable.Clone(_p.PntImage);
            this.PntName = _p.PntName;
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(44, 66);
            this.Location = _p.Location;
            this.Controls.Add(this.PntImage);
            this.Controls.Add(this.PntName);
        }*/

        public Pnt(InputForm form, PointsHandler pntHndlr, Point _location, string _src, int _num)
        {
            this.src = _src;
            this.num = _num;
            this.PntImage = new PictureBox();
            this.PntImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.PntImage.Size = new Size(41, 41);
            this.PntImage.Location = new Point(1, 25);
            this.PntImage.Image = pntHndlr.pntImage;
            this.PntImage.Enabled = false;

            this.PntName = new TextBox();
            this.PntName.Width = 42;
            this.PntName.Location = new Point(1, 1);
            if (src == "new")
            {
                this.PntName.Text = Convert.ToString(pntHndlr.pointNum);
            }
            else if (src == "copy")
            {
                this.PntName.Text = Convert.ToString(num);
            }

            //this = new Panel();
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(44, 66);
            this.Location = _location;
            this.Controls.Add(this.PntImage);
            this.Controls.Add(this.PntName);
            this.MouseDown += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDown = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (pntHndlr.clicked[0] == -1)
                    {
                        pntHndlr.clicked[0] = pntHndlr.points.IndexOf(sender as Pnt);
                    }
                    else if (pntHndlr.clicked[0] != -1 && pntHndlr.clicked[1] == -1)
                    {
                        pntHndlr.clicked[1] = pntHndlr.points.IndexOf(sender as Pnt);
                    }
                    if (pntHndlr.clicked[0] != -1 && pntHndlr.clicked[1] != -1)
                    {
                        Pair<Pair<int, Point>, Pair<int, Point>> tempPair = new Pair<Pair<int, Point>, Pair<int, Point>>(new Pair<int, Point>(pntHndlr.clicked[0], pntHndlr.points[pntHndlr.clicked[0]].Location), new Pair<int, Point>(pntHndlr.clicked[1], pntHndlr.points[pntHndlr.clicked[1]].Location));
                        pntHndlr.integratedPoints.Add(tempPair);
                        pntHndlr.integratePoints(pntHndlr, form);
                    }
                }
            };
            this.MouseUp += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDown = false;
                }
            };
            this.MouseMove += (object sender, MouseEventArgs e) =>
            {
                Control c = sender as Control;
                if (isDown && e.Button == MouseButtons.Left)
                {
                    c.Location = form.PointToClient(Control.MousePosition);
                }
            };
            if (src == "new")
            {
                pntHndlr.points.Add(this);
            }
        }
    }

    public class PointsHandler
    {
        public List<Pnt> points = new List<Pnt>() { };
        public List<TextBox> textBoxes = new List<TextBox>() { };
        public List<between2Points> distances = new List<between2Points>() { };
        public List<Pair<Pair<int, Point>, Pair<int, Point>>> integratedPoints = new List<Pair<Pair<int, Point>, Pair<int, Point>>>() { };
        InputForm form;
        public Image pntImage;
        public int[] clicked = new int[2] { -1, -1 };

        public int pointNum = 1;
        public void clear()
        {
            for (int i = 0; i < this.points.Count; i++)
            {
                this.points[i].Visible = false;
            }
            for (int i = 0; i < this.textBoxes.Count; i++)
            {
                this.textBoxes[i].Visible = false;
            }
            this.points.Clear();
            this.textBoxes.Clear();
            this.distances.Clear();
            this.integratedPoints.Clear();
            this.pointNum = 1;
            this.clicked[0] = -1;
            this.clicked[1] = -1;
        }
        public void integratePoints(PointsHandler pntHndlr, InputForm form)
        {
            Pen p = new Pen(Brushes.Black, 5);
            Point p1 = new Point(pntHndlr.points[pntHndlr.clicked[0]].Left + 22, pntHndlr.points[pntHndlr.clicked[0]].Top + 33);
            Point p2 = new Point(pntHndlr.points[pntHndlr.clicked[1]].Left + 22, pntHndlr.points[pntHndlr.clicked[1]].Top + 33);
            if (p1.X > p2.X)
            {
                Point tempp = p1;
                p1 = p2;
                p2 = tempp;
            }

            form.mainGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            form.mainGr.DrawLine(p, p1, p2);
            form.mainPb.Image = form.mainBmp;

            TextBox point_s = new TextBox();
            point_s.Width = 42;
            point_s.Location = new Point(Math.Abs(points[clicked[0]].Left + points[clicked[1]].Left) / 2, Math.Abs(points[clicked[0]].Top + points[clicked[1]].Top) / 2);
            form.mainPb.Controls.Add(point_s);

            between2Points tempb = new between2Points(clicked[0], clicked[1]);
            distances.Add(tempb);
            this.textBoxes.Add(point_s);

            this.clicked[0] = -1;
            this.clicked[1] = -1;
            
        }
        public PointsHandler(InputForm _form, Image _pntImage)
        {
            this.form = _form;
            this.pntImage = _pntImage;
            this.form.addPointBtn.Click += (object sender, EventArgs e) => {
                Pnt tempPnt = new Pnt(form, this, new Point(200, 200), "new", -1);
                form.mainPb.Controls.Add(this.points[this.points.Count - 1]);
                pointNum++;
            };
            this.form.mainPb.DoubleClick += (object sender, EventArgs e) => {
                Pnt tempPnt = new Pnt(form, this, new Point(Cursor.Position.X - form.Location.X, Cursor.Position.Y - form.Location.Y), "new", -1);
                form.mainPb.Controls.Add(this.points[this.points.Count - 1]);
                pointNum++;
            };
        }
    }
}