using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 摄像头监控录像
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        cVideo video;
        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            video = new cVideo(pictureBox1.Handle, 0, 0, pictureBox1.Width, pictureBox1.Height);
            video.StartVideo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnStar.Enabled = false;
            btnStop.Enabled = true;
            String ste = DateTime.Now.ToString("yyyyMMddHHmmss");
            video.StarKinescope(@"E:\监控" + ste + ".avi");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnStar.Enabled = true;
            btnStop.Enabled = false;
            video.StopKinescope();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}