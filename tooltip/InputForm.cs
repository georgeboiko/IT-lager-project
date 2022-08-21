using System.Diagnostics;

namespace tooltip
{
    public partial class InputForm : Form
    {
        public Button addPointBtn;
        public Button calcBtn;
        public Button resetBtn;
        public Button themeBtn;

        public PictureBox mainPb;
        public Bitmap mainBmp;
        public Graphics mainGr;

        public PointsHandler pointsHandler;
        public DistanceCalc distanceCalc;

        LinkLabel siteLabel;

        public void SizeCh(object sender, EventArgs e)
        {
            if (this.mainPb != null)
            {
                this.mainPb.Size = this.Size;
                this.mainBmp = new Bitmap(this.mainPb.Size.Width, this.mainPb.Size.Height);
                this.mainGr = Graphics.FromImage(this.mainBmp);
                this.mainGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                this.mainGr.Clear(Color.White);
                this.mainPb.Image = this.mainBmp;
                for (int i = 0; i < pointsHandler.integratedPoints.Count; i++)
                {
                    Pen p = new Pen(Brushes.Black, 5);
                    this.mainGr.DrawLine(p, new Point(pointsHandler.integratedPoints[i].First.Second.X+22,
                        pointsHandler.integratedPoints[i].First.Second.Y+33),
                        new Point(pointsHandler.integratedPoints[i].Second.Second.X+22,
                        pointsHandler.integratedPoints[i].Second.Second.Y+33));
                }
                this.siteLabel.Location = new Point(this.Width - 285, this.Height - 56);
            }
        }
        public InputForm()
        {
            //FORM INITIALIZATION

            InitializeComponent();

            this.Icon = Resources.new_icon;
            this.Size = new Size(800, 600);
            this.Text = "tooltip";
            this.SizeChanged += SizeCh;
            this.siteLabel = new LinkLabel();
            this.siteLabel.Text = "https://navigator-download.000webhostapp.com/";
            this.siteLabel.Size = new Size(270, 15);
            this.siteLabel.BackColor = Color.White;
            this.siteLabel.Cursor = Cursors.Hand;
            this.siteLabel.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) =>
            {
                Process.Start(new ProcessStartInfo(this.siteLabel.Text) { UseShellExecute = true });
            };

            //BUTTONS INITIALIZATION
            this.addPointBtn = new Button();
            this.addPointBtn.Size = new Size(90, 40);
            this.addPointBtn.Text = "Добавить точку";
            this.addPointBtn.Location = new Point(0, 0);
            this.addPointBtn.FlatStyle = FlatStyle.Flat;
            this.addPointBtn.FlatAppearance.BorderColor = Color.LightGray;
            this.Controls.Add(this.addPointBtn);

            this.calcBtn = new Button();
            this.calcBtn.Size = new Size(90, 40);
            this.calcBtn.Text = "Рассчитать!";
            this.calcBtn.Location = new Point(0, 45);
            this.calcBtn.FlatStyle = FlatStyle.Flat;
            this.calcBtn.FlatAppearance.BorderColor = Color.LightGray;
            this.calcBtn.Click += (object sender, EventArgs e) => 
            {
                this.distanceCalc = new DistanceCalc(this.pointsHandler.points.Count);
                this.distanceCalc.calc(this.pointsHandler.points,
                    this.pointsHandler.distances,
                    this.pointsHandler.textBoxes, this);
                OutputForm outForm = new OutputForm(this.pointsHandler.points.Count,
                    this.distanceCalc.parent,
                    this.distanceCalc.pos, this);
                outForm.Show();
            };
            this.Controls.Add(this.calcBtn);

            this.resetBtn = new Button();
            this.resetBtn.Size = new Size(90, 40);
            this.resetBtn.Text = "Сбросить";
            this.resetBtn.Location = new Point(0, 90);
            this.resetBtn.FlatStyle = FlatStyle.Flat;
            this.resetBtn.FlatAppearance.BorderColor = Color.LightGray;
            this.resetBtn.Click += (object sender, EventArgs e) =>
            {
                this.pointsHandler.clear();
                this.mainGr.Clear(Color.White);
                this.mainPb.Image = this.mainBmp;
            };
            this.Controls.Add(this.resetBtn);

            this.themeBtn = new Button();
            this.themeBtn.Size = new Size(90, 40);
            this.themeBtn.Text = "Изменить оформление";
            this.themeBtn.Location = new Point(0, 135);
            this.themeBtn.FlatStyle = FlatStyle.Flat;
            this.themeBtn.FlatAppearance.BorderColor = Color.LightGray;
            //this.Controls.Add(this.themeBtn);

            //GRAPHICS INITIALIZATION
            this.mainPb = new PictureBox();
            //this.mainPb.SizeMode = PictureBoxSizeMode.StretchImage;
            this.mainPb.Size = this.Size;
            this.Controls.Add(this.mainPb);

            this.mainBmp = new Bitmap(this.mainPb.Size.Width, this.mainPb.Size.Height);

            this.mainGr = Graphics.FromImage(this.mainBmp);
            this.mainGr.Clear(Color.White);
            this.mainPb.Image = this.mainBmp;

            this.siteLabel.Location = new Point(this.Width - 285, this.Height - 56);
            this.mainPb.Controls.Add(this.siteLabel);

            //HANDLERS INITIALIZATION
            this.pointsHandler = new PointsHandler(this, Resources.crcl);
            this.distanceCalc = new DistanceCalc(0);
        }
    }
}