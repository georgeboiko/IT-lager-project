using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tooltip
{
    public partial class OutputForm : Form
    {
        int n;
        int[] parent;
        int[] pos;

        PictureBox previewBox;
        Bitmap previewImage;
        Graphics previewGraphics;
        InputForm inForm;
        List<List<Pair<int, int>>> roads;
        public int previewPos = 0;

        Button nextBtn;
        Button prevBtn;
        public void notify()
        {
            NotifyIcon not = new NotifyIcon();
            not.Icon = this.Icon;
            not.Visible = true;
            not.ShowBalloonTip(1, " ", "Путь рассчитан", ToolTipIcon.Info);
            not.BalloonTipClosed += (object sender, EventArgs e) =>
            {
                not.Visible = false;
            };
        }
        public void drawOnPreviewBmp()
        {
            this.previewBox.Controls.Clear();
            this.previewGraphics.Clear(Color.White);
            this.previewBox.Image = this.previewImage;
            double COEFF = Math.Min((double)this.previewBox.Width / (double)this.inForm.Width, (double)this.previewBox.Height / (double)this.inForm.Height);
            for (int i = 0; i < this.inForm.pointsHandler.points.Count; i++)
            {
                if (this.previewBox != null)
                {
                    Pnt tempPnt = new Pnt(this.inForm, this.inForm.pointsHandler, this.inForm.pointsHandler.points[i].Location, "copy", i+1);
                    int x = Convert.ToInt32(tempPnt.Location.X * COEFF);
                    int y = Convert.ToInt32(tempPnt.Location.Y * COEFF);
                    tempPnt.Location = new Point(x, y);
                    this.previewBox.Controls.Add(tempPnt);
                }
            }
            for (int i = 0; i < this.inForm.pointsHandler.integratedPoints.Count; i++)
            {
                Pen p = new Pen(Brushes.Black, 5);
                try
                {
                    int indx = Math.Min(i, this.roads[previewPos].Count - 1);
                    if (this.inForm.pointsHandler.integratedPoints[i].First.First == this.roads[previewPos][indx].First &&
                        this.inForm.pointsHandler.integratedPoints[i].Second.First == this.roads[previewPos][indx].Second)
                    {
                        p.Color = Color.Red;
                    }
                    else
                    {
                        p.Color = Color.Black;
                    }
                }
                catch { }
                this.previewGraphics.DrawLine(p, new Point((Convert.ToInt32((this.inForm.pointsHandler.integratedPoints[i].First.Second.X + 22)*COEFF)),
                    (Convert.ToInt32((this.inForm.pointsHandler.integratedPoints[i].First.Second.Y + 33) * COEFF))),
                    new Point((Convert.ToInt32((this.inForm.pointsHandler.integratedPoints[i].Second.Second.X + 22) * COEFF)),
                   (Convert.ToInt32((this.inForm.pointsHandler.integratedPoints[i].Second.Second.Y + 33) * COEFF))));
            }
            for (int i = 0; i < this.inForm.pointsHandler.textBoxes.Count; i++)
            {
                TextBox newTextBox = new TextBox();
                newTextBox.Location = new Point(Convert.ToInt32(this.inForm.pointsHandler.textBoxes[i].Location.X * COEFF),
                   Convert.ToInt32(this.inForm.pointsHandler.textBoxes[i].Location.Y * COEFF));
                newTextBox.Size = this.inForm.pointsHandler.textBoxes[i].Size;
                newTextBox.Text = this.inForm.pointsHandler.textBoxes[i].Text;
                this.previewBox.Controls.Add(newTextBox);
            }
            this.previewBox.Image = this.previewImage;
        }
        public OutputForm(int _n, int[] _parent, int[] _pos, InputForm f)
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.Text = "tooltip";
            this.Icon = Resources.new_icon;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Size = new Size(650, 400);

            this.roads = new List<List<Pair<int, int>>>() { };
            this.n = _n;
            this.parent = _parent;
            this.pos = _pos;
            this.inForm = f;

            Label result_label = new Label();
            result_label.Location = new Point(0, 0);
            result_label.Size = new Size(100, 400);
            result_label.Text = "Результат: \n";
            this.Controls.Add(result_label);

            notify();

            for (int x = 2; x <= n; x++)
            {
                List<int> temp = new List<int>();
                for (int i = x - 1; i != -1; i = parent[i])
                {
                    temp.Add(i + 1);
                }
                temp.Reverse();
                result_label.Text += (x + ": " + "\n");
                List<Pair<int, int>> tempL = new List<Pair<int, int>>() { };
                for (int i = 0; i < temp.Count; i++)
                {
                    if (i != temp.Count - 1)
                    {
                        result_label.Text += (temp[i] + " -> ");
                        Pair<int, int> tempPair = new Pair<int, int>(temp[i] - 1, temp[i+1] - 1);
                        tempL.Add(tempPair);
                    }
                    else
                    {
                        result_label.Text += (temp[i] + " ");
                    }
                }
                this.roads.Add(tempL);
                result_label.Text += ("\n" + pos[x - 1] + "\n");
            }

            //GRAPHICS PREVIEW
            this.previewBox = new PictureBox();
            this.previewBox.Size = new Size(450, 330);
            this.previewBox.Location = new Point(125, 5);
            this.previewImage = new Bitmap(450, 330);
            this.previewGraphics = Graphics.FromImage(this.previewImage);
            this.previewBox.Image = this.previewImage;
            this.previewGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.Controls.Add(this.previewBox);
            this.Show();
            this.drawOnPreviewBmp();

            //BUTTONS
            this.prevBtn = new Button();
            this.prevBtn.Text = "<";
            this.prevBtn.Location = new Point(270, 338);
            this.prevBtn.Click += (object sender, EventArgs e) => 
            {
                if (this.previewPos <= 0)
                {
                    this.previewPos = this.inForm.pointsHandler.points.Count - 2;
                }
                else if (this.previewPos > 0)
                {
                    this.previewPos--;
                }
                this.drawOnPreviewBmp();
            };
            this.Controls.Add(this.prevBtn);
            this.nextBtn = new Button();
            this.nextBtn.Text = ">";
            this.nextBtn.Location = new Point(370, 338);
            this.nextBtn.Click += (object sender, EventArgs e) =>
            {
                if (this.previewPos >= this.inForm.pointsHandler.points.Count - 2)
                {
                    this.previewPos = 0;
                }
                else if (this.previewPos < this.inForm.pointsHandler.points.Count - 1)
                {
                    this.previewPos++;
                }
                this.drawOnPreviewBmp();
            };
            this.Controls.Add(this.nextBtn);
        }
    }
}