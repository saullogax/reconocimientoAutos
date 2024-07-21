using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Threading;

namespace Detectar_Carros
{
    public partial class Form1 : Form
    {


        //SE DECLARAN VARIABLES DE CAPTURA
        private VideoCapture _capture;
        private Thread _captureThread;
        List<Image> imgList = new List<Image>();
        int i = 0;
        int total = 0;

        private CascadeClassifier _CascadeClassifier;
        public Form1()
        {
            InitializeComponent();
            _CascadeClassifier = new CascadeClassifier(Application.StartupPath + "/cars2.xml");
        }

        private void DisplayWebCam()
        {
            while (_capture.IsOpened)
            {
                Mat frame = _capture.QueryFrame();
                //CvInvoke.Resize(frame, frame, imageBox1.Size);
                imageBox1.Image = frame;

                using (var ImageFrame = frame.ToImage<Bgr, Byte>())
                {
                    if (ImageFrame != null)
                    {
                        var grayFrame = ImageFrame.Convert<Gray, Byte>();

                        var cars = _CascadeClassifier.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty);

                        foreach (var car in cars)
                        {
                            //Detecta Rostros decimal la imagen
                            ImageFrame.Draw(car, new Bgr(Color.Red), 3);
                        }

                    }
                    imageBox1.Image = ImageFrame;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                //SE CARGARA EL VIDEO
                _capture = new VideoCapture(open.FileName);
                _captureThread = new Thread(DisplayWebCam);
                _captureThread.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}