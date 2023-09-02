using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Resources;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Matrix;
using Kalman_filter;

namespace imu
{
    
    public partial class imu : Form
    {
        public int mode = 0;
        public static double bridge;
        public FileControl imudata;
        public Thread read;
        public double wbias = 0.01;
        public double dt = new double();
        public double end_time = new double();
        public double L1,L2,ans,clc_angle = 0;
        public double thetaz = 0;
        public double[] predict = new double[3];
        public List<double[]> imu_data_record = new List<double[]>();
        public List<double[]> kf_data_record = new List<double[]>();
        public List<double> theta_z_record = new List<double>();
        Vectorxd xk_hat = new Vectorxd();
        Kalman_filter.Kalman_filter kf = new Kalman_filter.Kalman_filter();
        Bitmap b,b2,b3;
        Graphics g,g2,g3;


        public imu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            b2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g2 = Graphics.FromImage(b2);
            b3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            g3 = Graphics.FromImage(b3);
        }

        private void RefreshComPort(object sender, EventArgs e)
        {
            toolStripComSet.DropDownItems.Clear();
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                toolStripComSet.DropDownItems.Add(portName, null, PortSelect);

                if ((spSerialPort.IsOpen) & (spSerialPort.PortName == portName))
                {
                    ToolStripMenuItem menu = (ToolStripMenuItem)toolStripComSet.DropDownItems[toolStripComSet.DropDownItems.Count - 1];
                    menu.Checked = true;
                }
            }
            toolStripComSet.DropDownItems.Add(new ToolStripSeparator());
            toolStripComSet.DropDownItems.Add("Close", null, PortClose);
        }

        ResourceManager rm = new ResourceManager(typeof(imu));
       
        private void Form1_Load(object sender, EventArgs e)
        {
            //double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
            
            //儲存設定
            //string path = @"C:\Users\anthony\Desktop\IMU\CSVdata\output.csv";
            //Dictionary<string, string> data = new Dictionary<string, string>()
            //{
            //    ["acc_x"] = a[0].ToString(),
            //    ["acc_y"] = a[1].ToString(),
            //    ["acc_z"] = a[2].ToString(),
            //    ["angle_acc_x"] = w[0].ToString(),
            //    ["angle_acc_y"] = w[1].ToString(),
            //    ["angle_acc_z"] = w[2].ToString(),
            //    ["angle_x"] = Angle[0].ToString(),
            //    ["angle_y"] = Angle[1].ToString(),
            //    ["angle_z"] = Angle[2].ToString()
            //};
            //string[] dataname = data.Keys.ToArray<string>();
            //SaveCsvname(dataname, path);
            imudata = new FileControl("‪C:\\robot\\imudata.txt");
            RefreshComPort(null, null);
            Baund = 9600;            
            SetBaudrate(Baund);
            //Set_Filter(kf);
        }
        //past
        private void Set_Filter(Kalman_filter.Kalman_filter kf)
        {
            //double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
            //double dt = TimeElapse - 0;

            //double[,] F_mat = new double[,]{{ 1, 0, 0, -dt, 0, 0},
            //                            { 0,1,0,0,-dt,0},
            //                            { 0,0,1,0,0,-dt},
            //                            { 0,0,0,1,0,0},
            //                            {0,0,0,0,1,0},
            //                            {0,0,0,0,0,1}
            //                           };
            //double[,] B_mat = new double[,]{{ dt, 0, 0, 0, 0, 0},
            //                                { 0,dt,0,0,0,0},
            //                                { 0,0,dt,0,0,0},
            //                                { 0,0,0,0,0,0},
            //                                {0,0,0,0,0,0},
            //                                {0,0,0,0,0,0}
            //                                };

            //Matrixd F = new Matrixd(6, 6, F_mat);
            //Matrixd B = new Matrixd(6, 6, B_mat);



            //double[,] Q_mat = new double[,]{{ 0.01, 0, 0, 0, 0, 0},
            //                            { 0,0.01,0,0,0,0},
            //                            { 0,0,0.01,0,0,0},
            //                            { 0,0,0,0.01,0,0},
            //                            {0,0,0,0,0.01,0},
            //                            {0,0,0,0,0,0.01}
            //                           };

            //double[,] H_mat = new double[,]{{ 1, 0, 0, 0, 0, 0},
            //                                { 0,1,0,0,0,0},
            //                                { 0,0,1,0,0,0}
            //                               };

            //double[,] R_mat = new double[,]{{ 0.01, 0, 0},
            //                                { 0,0.01,0},
            //                                { 0,0,0.01}
            //                               };

            //Matrixd P = new Matrixd(6, 6);
            //Matrixd Q = new Matrixd(6, 6, Q_mat);
            //Matrixd H = new Matrixd(3, 6, H_mat);
            //Matrixd R = new Matrixd(3, 3, R_mat);

            //Matrixd I = new Matrixd(6, 6);

            //kf.Set_Filter_params(I, P, H, Q, R);
            //kf.Set_F(F);
            //kf.Set_B(B);

            //double[] xk_mat = new double[] { 0, 0, 0, wbias, wbias, wbias };

            //Vectorxd xk = new Vectorxd(6, xk_mat);//設k-1時間為零
            //Vectorxd zk = new Vectorxd(3);//設k-1時間為零
            //Vectorxd uk = new Vectorxd(6);//設k-1時間為零

            //Vectorxd xk_hat = kf.update(xk, zk, uk);

            //xk_hat.output();
        }

        private void updtate_state(Kalman_filter.Kalman_filter kf)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            if (kf.isintial == false)
            {
                //初始化
                //end_time = (DateTime.Now - TimeStart).TotalMilliseconds / 1000d;
                //double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
                //start_time = 0;
                dt = 0.063;
                double[,] F_mat = new double[,]{{ 1, 0, 0, -dt, 0, 0},
                                        { 0,1,0,0,-dt,0},
                                        { 0,0,1,0,0,0},//z 軸-dt弄掉為什麼會更穩?
                                        { 0,0,0,1,0,0},
                                        {0,0,0,0,1,0},
                                        {0,0,0,0,0,1}
                                       };
                //double[,] F_mat = new double[,]{{ 1, 0, 0, dt, 0, 0},
                //                        { 0,1,0,0,dt,0},
                //                        { 0,0,1,0,0,dt},
                //                        { 0,0,0,1,0,0},
                //                        {0,0,0,0,1,0},
                //                        {0,0,0,0,0,1}
                //                       };
                double[,] B_mat = new double[,]{{ dt, 0, 0, 0, 0, 0},
                                            { 0,dt,0,0,0,0},
                                            { 0,0,dt,0,0,0},
                                            { 0,0,0,0,0,0},
                                            {0,0,0,0,0,0},
                                            {0,0,0,0,0,0}
                                            };

                Matrixd F = new Matrixd(6, 6, F_mat);
                Matrixd B = new Matrixd(6, 6, B_mat);



                double[,] Q_mat = new double[,]{{0.01, 0, 0, 0, 0, 0},
                                        { 0,0.01,0,0,0,0},
                                        { 0,0,0.01,0,0,0},
                                        { 0,0,0,0.002,0,0},
                                        {0,0,0,0,0.002,0},
                                        {0,0,0,0,0,0.005}
                                       };

                double[,] P_mat = new double[,]{{ 0, 0, 0, 0, 0, 0},
                                        { 0,0,0,0,0,0},
                                        { 0,0,0,0,0,0},
                                        { 0,0,0,0,0,0},
                                        {0,0,0,0,0,0},
                                        {0,0,0,0,0,0}
                                       };

                double[,] H_mat = new double[,]{{ 1, 1, 0, 1,1,1}
                                            //{ 0,1,0,0,0,0},
                                            //{ 0,0,1,0,0,0}
                                           };

                double[,] R_mat = new double[,]{{0.01}
                                            //{ 0,0.03,0},
                                            //{ 0,0,0.03}
                                           };

                Matrixd P = new Matrixd(6, 6, P_mat);
                Matrixd Q = new Matrixd(6, 6, Q_mat);
                Q = Q * dt;
                Matrixd H = new Matrixd(1, 6, H_mat);
                Matrixd R = new Matrixd(1, 1, R_mat);
                Matrixd I = new Matrixd(6, 6);

                double[] xk_mat = new double[] { 0,0,0, wbias,wbias,wbias };
                //imu_data = {a[0],a[1],a[2],w[0],w[1],w[2],angle[0],angle[1],angle[2],dt}
                double[] imu_data = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 ,dt};
                imu_data_record.Add(imu_data);
                kf_data_record.Add(xk_mat);
                xk_hat = new Vectorxd(6, xk_mat);//設k-1時間為零


                kf.Set_Filter_params(xk_hat, I, P, H, Q, R);
                kf.Set_F(F);
                kf.Set_B(B);
            }
            else if(kf.isintial == true)
            {
                double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000d;
                //start_time = dt;
                //dt = (TimeElapse - end_time);

                double[] xk_1 = kf_data_record[kf_data_record.Count - 1];
                double[] xk_ans = imu_data_record[imu_data_record.Count - 1];
                double[] zk_ = new double[] { a[0], a[1], a[2], w[0], w[1], w[2], Angle[0], Angle[1], Angle[2], dt};
                //double[] acc = new double[] { a[0], a[1], a[2] };
                //double[] angle_vel = new double[] { w[0], w[1],w[2]};

                //Vector3D acc_k_1 = new Vector3D(xk_1[0], xk_1[1], xk_1[2]);
                //Vector3D norm_acc_k_1 = acc_k_1.norm(acc_k_1);
                //Vector3D angle_acc_k_1 = new Vector3D(xk_1[3], xk_1[4], xk_1[5]);
                //Vector3D norm_angle_acc_k_1 = acc_k_1.norm(angle_acc_k_1);

                //double thetax_k1 = Math.Atan2(norm_acc_k_1.y, norm_acc_k_1.z);
                //double thetay_k1 = Math.Atan2(-norm_acc_k_1.x, Math.Sqrt(norm_acc_k_1.z * norm_acc_k_1.z + norm_acc_k_1.y * norm_acc_k_1.y));
                //double thetaz_k1 = To_rad(xk_1[2] * xk_1[9]);

                //觀測
                Vector3D acc_ = new Vector3D(zk_[0], zk_[1], zk_[2]);
                Vector3D norm_acc = acc_.norm(acc_);
                //Vector3D angle_acc = new Vector3D(zk_[3], zk_[4], zk_[5]);
                //Vector3D norm_angle_acc = acc_.norm(angle_acc);

                double R = norm_acc.Get_R();
                double thetax = -Math.Atan2(norm_acc.y, R) * 2d;
                //double thetax = Math.Atan2(norm_acc.y, norm_acc.z) * 2d;
                //double thetay = Math.Atan2(-norm_acc.x,norm_acc.z);
                double thetay = Math.Atan2(norm_acc.x, R) * 2d;
                double thetaz = To_rad(zk_[2] * dt);

                //double thetax = Math.Acos(norm_acc.x);
                //double thetay = Math.Acos(norm_acc.y);
                //double thetay = Math.Atan2(-norm_acc.x,norm_acc.z);
                //thetaz = Math.Atan2(norm_acc.z, norm_acc.y) + To_rad(angle_acc[2] * dt);
                //thetaz = To_rad(zk_[2]*dt);


                //double[] tempx = new double[] { xk_1[0], xk_1[1], xk_1[2], wbias,wbias,wbias };
                double[] tempx = new double[] { xk_1[0], xk_1[1], xk_1[2], xk_1[3], xk_1[4], xk_1[5] };
                double[] angle = new double[] { To_rad(xk_ans[6]), To_rad(xk_ans[7]), To_rad(xk_ans[8]), To_rad(xk_ans[3]), To_rad(xk_ans[4]), To_rad(xk_ans[5])};

                Vectorxd angle_ans = new Vectorxd(6, angle);//
                double[] uk_mat = new double[] { To_rad(zk_[3]), To_rad(zk_[4]), To_rad(zk_[5]), 0, 0, 0 };
                double[] zk_mat = new double[] { thetax, thetay, thetaz, To_rad(zk_[3]), To_rad(zk_[4]), To_rad(zk_[5]) };
                //double[] xk_mat = new double[] { xk_hat.value[0] * 180 / Math.PI, xk_hat.value[1] * 180 / Math.PI, xk_hat.value[2] * 180 / Math.PI, wbias, wbias, wbias };

                //真正感測器量到的xk_1數值
                Vectorxd xk_1_ = new Vectorxd(6, tempx);//設k-1時間為零
                Vectorxd zk = new Vectorxd(6, zk_mat);//設k-1時間為零
                Vectorxd uk = new Vectorxd(6,uk_mat);//設k-1時間為零

                xk_hat = kf.update(xk_1_, zk, uk);

                double x = To_deg(xk_hat.value[0]);
                double y = To_deg(xk_hat.value[1]);
                double z = To_deg(xk_hat.value[2]);

                x = deg_change(x);
                y = deg_change(y);
                z = deg_change(z);
                if (!Double.IsNaN(z))
                {
                    theta_z_record.Add(z);
                }
                predict = new double[] { x, y, z};

                pictureBox1.Invoke(new EventHandler(delegate
                {
                    pictureBox1.Refresh();
                    pictureBox2.Refresh();
                    pictureBox3.Refresh();
                }));

                //double[] imu_data = new double[] { a[0], a[1], a[2], w[0], w[1], w[2], Angle[0], Angle[1], Angle[2],dt };
                imu_data_record.Add(zk_);
                //double[] kf_data = new double[] { xk_hat.value[], xk_hat.value[4], xk_hat.value[5], xk_hat.value[3],xk_hat.value[4],xk_hat.value[5] };
                kf_data_record.Add(xk_hat.value);

                //Console.WriteLine(predict[0]);
                //Console.WriteLine(predict[1]);
                //Console.WriteLine(predict[2]);

                //Console.WriteLine(Angle[0]);
                //Console.WriteLine(Angle[1]);
                //Console.WriteLine(Angle[2]);
                //Console.WriteLine("###############");
                end_time = (DateTime.Now - TimeStart).TotalMilliseconds / 1000d;
                sw.Stop();
                
            }
            //Console.WriteLine("predict theta x",xk_hat.value[0]);
        }

        private double To_rad(double deg)
        {
            return deg * Math.PI / 180d;
        }
        private double To_deg(double rad)
        {
            return rad * 180d / Math.PI;
        }

        private double deg_change(double deg)
        {   
            if(deg >=180 || deg <= -180)
            {
                if (deg >= 180)
                    deg -= 360;
                else if (deg <= -180)
                    deg += 360;
            }
            return deg;
        }

        private void SetBaudrate(int iBaund)
        {
            toolStripMenuItem2.Checked = false;
            toolStripMenuItem3.Checked = false;
            toolStripMenuItem4.Checked = false;
            toolStripMenuItem5.Checked = false;
            toolStripMenuItem6.Checked = false;
            toolStripMenuItem7.Checked = false;
            toolStripMenuItem8.Checked = false;
            toolStripMenuItem9.Checked = false;
            toolStripMenuItem10.Checked = false;
            toolStripMenuItem11.Checked = false;
            switch (iBaund)
            {
                case 2400: toolStripMenuItem2.Checked = true; break;
                case 4800: toolStripMenuItem3.Checked = true; break;
                case 9600: toolStripMenuItem4.Checked = true; break;
                case 19200: toolStripMenuItem5.Checked = true; break;
                case 38400: toolStripMenuItem6.Checked = true; break;
                case 57600: toolStripMenuItem7.Checked = true; break;
                case 115200: toolStripMenuItem8.Checked = true; break;
                case 230400: toolStripMenuItem9.Checked = true; break;
                case 460800: toolStripMenuItem10.Checked = true; break;
                case 921600: toolStripMenuItem11.Checked = true; break;
            }
            spSerialPort.BaudRate = iBaund;
        }
        private bool bListening = false;
        private bool bClosing = false;
        private DateTime TimeStart = DateTime.Now;
        private Int32 Baund=115200;
        private void PortSelect(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            try
            {
                PortClose(null, null);
                spSerialPort.PortName = menu.Text;
                spSerialPort.BaudRate = Baund;                
                spSerialPort.Open();
                menu.Checked = true;
                bClosing = false;
                timer1.Start();
            }
            catch (Exception ex)
            {
                menu.Checked = false;
            }  

        }
        private void PortClose(object sender, EventArgs e)
        {
            for (int i = 0; i < toolStripComSet.DropDownItems.Count-2; i++)
            {
                ToolStripMenuItem tempMenu = (ToolStripMenuItem)toolStripComSet.DropDownItems[i];
                tempMenu.Checked = false;
            }
            if (spSerialPort.IsOpen)
            {
                bClosing = true;
                while (bListening) Application.DoEvents(); 
                spSerialPort.Dispose(); 
                spSerialPort.Close();
                timer1.Stop();
                
            }
        }
        public double[] a = new double[4], w = new double[4], h = new double[4], Angle = new double[4], Port = new double[4];
        double Temperature, Pressure, Altitude,  GroundVelocity, GPSYaw, GPSHeight;
        long Longitude, Latitude;



        private void DisplayRefresh()
        {

            double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
            //count_time = TimeElapse - reset_time;

            string path = @"C:\Users\anthony\Desktop\IMU\CSVdata\output.csv";

            string temp = DateTime.Now.ToLongTimeString() + "\r\n"
                            + ChipTime[0].ToString() + "-" + ChipTime[1].ToString() + "-" + ChipTime[2].ToString() + "\r\n" + ChipTime[3].ToString() + ":" + ChipTime[4].ToString() + ":" + ChipTime[5].ToString() + "." + ChipTime[6].ToString() + "\r\n"
                            + TimeElapse.ToString("f3") + "\r\n\r\n"
                            + a[0].ToString("f2") + " g\r\n"
                            + a[1].ToString("f2") + " g\r\n"
                            + a[2].ToString("f2") + " g\r\n\r\n"
                            + w[0].ToString("f2") + " °/s\r\n"
                            + w[1].ToString("f2") + " °/s\r\n"
                            + w[2].ToString("f2") + " °/s\r\n\r\n"
                            + Angle[0].ToString("f2") + " °\r\n"
                            + Angle[1].ToString("f2") + " °\r\n"
                            + Angle[2].ToString("f2") + " °\r\n\r\n"
                            + h[0].ToString("f0") + " mG\r\n"
                            + h[1].ToString("f0") + " mG\r\n"
                            + h[2].ToString("f0") + " mG\r\n\r\n"
                            + Temperature.ToString("f2") + " ℃\r\n"
                            + Pressure.ToString("f0") + " Pa\r\n"
                            + Altitude.ToString("f2") + " m\r\n\r\n"
                            + (Longitude / 10000000).ToString("f0") + "°" + ((double)(Longitude % 10000000) / 1e5).ToString("f5") + "'\r\n"
                            + (Latitude / 10000000).ToString("f0") + "°" + ((double)(Latitude % 10000000) / 1e5).ToString("f5") + "'\r\n"
                            + GPSHeight.ToString("f1") + " m\r\n"
                            + GPSYaw.ToString("f1") + " °\r\n"
                            + GroundVelocity.ToString("f3") + " km/h";
            label34.Text = temp;

            

            


            //Dictionary<string, string> data = new Dictionary<string, string>()
            //{
            //    ["acc_x"] = a[0].ToString(),
            //    ["acc_y"] = a[1].ToString(),
            //    ["acc_z"] = a[2].ToString(),
            //    ["angle_acc_x"] = w[0].ToString(),
            //    ["angle_acc_y"] = w[1].ToString(),
            //    ["angle_acc_z"] = w[2].ToString(),
            //    ["angle_x"] = Angle[0].ToString(),
            //    ["angle_y"] = Angle[1].ToString(),
            //    ["angle_z"] = Angle[2].ToString()
            //};

            //Console.WriteLine(dataname);
            //SaveCsvname(dataname ,path);
            //bool ok = System.IO.File.Exists(path);
            //Console.WriteLine(data.Count);
            //String csv = String.Join(
            //    Environment.NewLine,
            //    data.Select(d => $"{d.Value};")
            //);
            //System.IO.File.AppendAllText(@"C:\Users\anthony\Desktop\IMU\CSVdata\output.csv", csv);


            //string[] datavalue = data.Values.ToArray<string>();
            //var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", datavalue);
            //File.AppendAllText(path, newLine);

        }

        private void SaveCsvname(string[] dataname,string path)
        {
            var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", dataname);

            File.AppendAllText(path, newLine);
        }

        private void DicToExcel(Dictionary<string, string> data, string v)
        {
            throw new NotImplementedException();
        }

        private double[] LastTime = new double[10];
        short sRightPack = 0;
        short [] ChipTime = new short[7];
        private void DecodeData(byte[] byteTemp)
        {
            double[] Data = new double[4];
            double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
            
            Data[0] = BitConverter.ToInt16(byteTemp, 2);
            Data[1] = BitConverter.ToInt16(byteTemp, 4);
            Data[2] = BitConverter.ToInt16(byteTemp, 6);
            Data[3] = BitConverter.ToInt16(byteTemp, 8);
            sRightPack++;
            switch (byteTemp[1])
            {
                case 0x50:
                    //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                    ChipTime[0] = (short)(2000 + byteTemp[2]);
                    ChipTime[1] = byteTemp[3];
                    ChipTime[2] = byteTemp[4];
                    ChipTime[3] = byteTemp[5];
                    ChipTime[4] = byteTemp[6];
                    ChipTime[5] = byteTemp[7];
                    ChipTime[6] = BitConverter.ToInt16(byteTemp, 8);


                    break;
                case 0x51:
                    //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                    Temperature = Data[3] / 100.0;
                    Data[0] = Data[0] / 32768.0 * 16;
                    Data[1] = Data[1] / 32768.0 * 16;
                    Data[2] = Data[2] / 32768.0 * 16;

                    a[0] = Data[0];
                    a[1] = Data[1];
                    a[2] = Data[2];
                    a[3] = Data[3];

                    if ((TimeElapse - LastTime[1]) < 0.1) return;
                    LastTime[1] = TimeElapse;

                    break;
                case 0x52:
                    //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                    Temperature = Data[3] / 100.0;
                    Data[0] = Data[0] / 32768.0 * 2000;
                    Data[1] = Data[1] / 32768.0 * 2000;
                    Data[2] = Data[2] / 32768.0 * 2000;
                    w[0] = Data[0];
                    w[1] = Data[1];
                    w[2] = Data[2];
                    w[3] = Data[3];

                    if ((TimeElapse-LastTime[2])<0.1) return;
                    LastTime[2] = TimeElapse;
                    break;
                case 0x53:
                    //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                    Temperature = Data[3] / 100.0;
                    Data[0] = Data[0] / 32768.0 * 180;
                    Data[1] = Data[1] / 32768.0 * 180;
                    Data[2] = Data[2] / 32768.0 * 180;
                    Angle[0] = Data[0];
                    Angle[1] = Data[1];
                    Angle[2] = Data[2];
                    Angle[3] = Data[3];
                    
                    
                    if ((TimeElapse-LastTime[3])<0.1) return;
                    LastTime[3] = TimeElapse;
                    break;
                case 0x54:
                    //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                    Temperature = Data[3] / 100.0;
                    h[0] = Data[0];
                    h[1] = Data[1];
                    h[2] = Data[2];
                    h[3] = Data[3];
                    if ((TimeElapse - LastTime[4]) < 0.1) return;
                    LastTime[4] = TimeElapse;
                    break;
                case 0x55:
                    Port[0] = Data[0];
                    Port[1] = Data[1];
                    Port[2] = Data[2];
                    Port[3] = Data[3];
            
                    break;

                case 0x56:
                    Pressure = BitConverter.ToInt32(byteTemp, 2);
                    Altitude = (double)BitConverter.ToInt32(byteTemp, 6) / 100.0;

                    break;

                case 0x57:
                    Longitude = BitConverter.ToInt32(byteTemp, 2);
                    Latitude  = BitConverter.ToInt32(byteTemp, 6);

                    break;

                case 0x58:
                    GPSHeight = (double)BitConverter.ToInt16(byteTemp, 2) / 10.0;
                    GPSYaw = (double)BitConverter.ToInt16(byteTemp, 4) / 10.0;
                    GroundVelocity = BitConverter.ToInt16(byteTemp, 6)/1e3;

                    break;
                default:
                    break;
            }   
        }
        byte byteLastNo = 0;
        
        delegate void UpdateData(byte[] byteData);//声明一个委托
        byte[] RxBuffer = new byte[1000];
        UInt16 usRxLength = 0;
        public void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
            read = new Thread(readdata);
            read.Start();
            read.Join();
             
        }



        //IMU TIME
        double TimeElapse = 0;
        double reset_time = 0;
        double count_time = 0;

        bool g_start = false; //print data


        public void readdata()
        {
            byte[] byteTemp = new byte[1000];

            if (bClosing) return ;
            try
            {
                bListening = true;
                UInt16 usLength = 0;
                try
                {
                    usLength = (UInt16)spSerialPort.Read(RxBuffer, usRxLength, 700);
                }
                catch (Exception err)
                {
                    //MessageBox.Show(err.Message);
                    //return;
                }
                usRxLength += usLength;
                while (usRxLength >= 11)
                {
                    UpdateData Update = new UpdateData(DecodeData);
                    RxBuffer.CopyTo(byteTemp, 0);
                    if (!((byteTemp[0] == 0x55) & ((byteTemp[1] & 0x50) == 0x50)))
                    {
                        for (int i = 1; i < usRxLength; i++) RxBuffer[i - 1] = RxBuffer[i];
                        usRxLength--;
                        continue;
                    }
                    if (((byteTemp[0] + byteTemp[1] + byteTemp[2] + byteTemp[3] + byteTemp[4] + byteTemp[5] + byteTemp[6] + byteTemp[7] + byteTemp[8] + byteTemp[9]) & 0xff) == byteTemp[10])
                        this.Invoke(Update, byteTemp);
                    for (int i = 11; i < usRxLength; i++) RxBuffer[i - 11] = RxBuffer[i];
                    usRxLength -= 11;
                }

                Thread.Sleep(10);
            }
            finally
            {
                bListening = false;//我用完了，ui可以关闭串口了。   
            }
            Thread.Sleep(10);
            TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;
            count_time = TimeElapse - reset_time;
/*
            string temp = DateTime.Now.ToLongTimeString() + "\r\n"
                            + ChipTime[0].ToString() + "-" + ChipTime[1].ToString() + "-" + ChipTime[2].ToString() + "\r\n" + ChipTime[3].ToString() + ":" + ChipTime[4].ToString() + ":" + ChipTime[5].ToString() + "." + ChipTime[6].ToString() + "\r\n"
                            + TimeElapse.ToString("f3") + "\r\n\r\n"
                            + a[0].ToString("f2") + " g\r\n"
                            + a[1].ToString("f2") + " g\r\n"
                            + a[2].ToString("f2") + " g\r\n\r\n"
                            + w[0].ToString("f2") + " °/s\r\n"
                            + w[1].ToString("f2") + " °/s\r\n"
                            + w[2].ToString("f2") + " °/s\r\n\r\n"
                            + Angle[0].ToString("f2") + " °\r\n"
                            + Angle[1].ToString("f2") + " °\r\n"
                            + Angle[2].ToString("f2") + " °\r\n\r\n"
                            + h[0].ToString("f0") + " mG\r\n"
                            + h[1].ToString("f0") + " mG\r\n"
                            + h[2].ToString("f0") + " mG\r\n\r\n"
                            + Temperature.ToString("f2") + " ℃\r\n"
                            + Pressure.ToString("f0") + " Pa\r\n"
                            + Altitude.ToString("f2") + " m\r\n\r\n"
                            + (Longitude / 10000000).ToString("f0") + "°" + ((double)(Longitude % 10000000) / 1e5).ToString("f5") + "'\r\n"
                            + (Latitude / 10000000).ToString("f0") + "°" + ((double)(Latitude % 10000000) / 1e5).ToString("f5") + "'\r\n"
                            + GPSHeight.ToString("f1") + " m\r\n"
                            + GPSYaw.ToString("f1") + " °\r\n"
                            + GroundVelocity.ToString("f3") + " km/h";*/

            string temp = DateTime.Now.ToLongTimeString() + "\r\n"
                            + ChipTime[0].ToString() + "-" + ChipTime[1].ToString() + "-" + ChipTime[2].ToString() + "\r\n" + ChipTime[3].ToString() + ":" + ChipTime[4].ToString() + ":" + ChipTime[5].ToString() + "." + ChipTime[6].ToString() + "\r\n"
                            + TimeElapse.ToString("f3") + "\r\n\r\n"
                            + a[0].ToString("f2") + " g\r\n"
                            + a[1].ToString("f2") + " g\r\n"
                            + a[2].ToString("f2") + " g\r\n\r\n"
                            + w[0].ToString("f2") + " °/s\r\n"
                            + w[1].ToString("f2") + " °/s\r\n"
                            + w[2].ToString("f2") + " °/s\r\n\r\n"
                            + Angle[0].ToString("f2") + " °\r\n"
                            + Angle[1].ToString("f2") + " °\r\n"
                            + Angle[2].ToString("f2") + " °\r\n\r\n"
                            + "No Data" + " mG\r\n"
                            + "No Data" + " mG\r\n"
                            + "No Data" + " mG\r\n\r\n";
                            
                            //+ h[0].ToString("f0") + " mG\r\n"
                            //+ h[1].ToString("f0") + " mG\r\n"
                            //+ h[2].ToString("f0") + " mG\r\n\r\n"
                            //+ Temperature.ToString("f2") + " ℃\r\n" ;

            label34.Invoke(new EventHandler(delegate { this.label34.Text = temp; }));
           
            imudata.gyro.a[0] = a[0];
            imudata.gyro.a[1] = a[1];
            imudata.gyro.a[2] = a[2];
            imudata.gyro.w[0] = w[0];
            imudata.gyro.w[1] = w[1];
            imudata.gyro.w[2] = w[2];
            imudata.gyro.angle[0] = Angle[0];
            imudata.gyro.angle[1] = Angle[1];
            imudata.gyro.angle[2] = Angle[2];
            imudata.WriteGyro();
            imudata.WriteGyroALL();
            



        }


        public void imu_graphics()
        {
           
            
        }



        private sbyte sbSumCheck(byte[] byteData,byte byteLength)
        {
            byte byteSum=0;
            for (byte i = 0;i<byteLength-2;i++)
                byteSum += byteData[i];
            if (byteData[byteLength - 1] == byteSum) return 0;
            else return -1;
        }
        public sbyte SendMessage(Byte[] byteSend)
        {
            if (spSerialPort.IsOpen == false)
            {
              //  MessageBox.Show(rm.GetString("PortNotOpen"), "Error!");
                Status.Text="Port Not Open!";
                return -1;
            }
            try
            {
                spSerialPort.Write(byteSend, 0, byteSend.Length);
                return 0;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return -1;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PortClose(null, null);
            }
            catch { }
           
        }
 

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            Byte[] byteSend = new Byte[9];
            byteSend[0] = (byte)'A';
            byteSend[1] = (byte)'T';
            byteSend[2] = (byte)'+';
            byteSend[3] = (byte)'R';
            byteSend[4] = (byte)'O';
            byteSend[5] = (byte)'L';
            byteSend[6] = (byte)'E';
            byteSend[7] = (byte)'=';
            byteSend[8] = (byte)'M';
            if (SendMessage(byteSend) != 0) return;
            Thread.Sleep(1500);
            byteSend[8] = (byte)'S';
            if (SendMessage(byteSend) != 0) return;
            Status.Text = rm.GetString("設置完成！");
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Baund = 2400;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Baund = 4800;
            SetBaudrate(Baund);
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Baund = 9600;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Baund = 19200;
            SetBaudrate(Baund);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            L1 = Convert.ToDouble(textBox1.Text);
            clc_angle = deg_change(predict[mode]);
            textBox4.Text = clc_angle.ToString();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            L2 = Convert.ToDouble(textBox2.Text);
            clc_angle -= deg_change(predict[mode]);
            clc_angle = deg_change(clc_angle);
            textBox5.Text = clc_angle.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox6.Text = "mode:"+ comboBox1.Text;
            if (comboBox1.Text == "X")
                mode = 0;
            else if (comboBox1.Text == "Z")
                mode = 2;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ans = (L1 * L1 + L2 * L2) - (2 * L1 * L2 * Math.Cos(To_rad(clc_angle)));
            textBox3.Text = ans.ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g.Clear(Color.White);
            PointF center = new PointF(b.Width / 2, b.Height / 2);
            double z_ =  deg_change(predict[2]);
            PointF end = new PointF(b.Width / 2 +(float)(20d*Math.Cos(To_rad( Angle[2]))), b.Height / 2 - (float)(20d * Math.Sin(To_rad(Angle[2]))));
            PointF end_ = new PointF(b.Width / 2 + (float)(20d * Math.Cos(To_rad(z_))), b.Height / 2 - (float)(20d * Math.Sin(To_rad(z_))));
            g.DrawLine(new Pen(Color.FromArgb(128, 255, 0, 0)),center, end);
            g.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255)), center, end_);
            g.DrawString(Angle[2].ToString("0.00"), new Font("標楷體", 12, FontStyle.Regular), new SolidBrush(Color.FromArgb(128, 255, 0, 0)), end);
            g.DrawString(z_.ToString("0.00"), new Font("標楷體", 12, FontStyle.Regular), new SolidBrush(Color.FromArgb(128, 0, 0, 255)), end_);
            e.Graphics.DrawImage(b, 0, 0);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            g2.Clear(Color.White);
            PointF center = new PointF(b2.Width / 2, b2.Height / 2);
            double y_ = deg_change(predict[1]);
            PointF end = new PointF(b2.Width / 2 + (float)(20d * Math.Cos(To_rad(Angle[1]))), b2.Height / 2 - (float)(20d * Math.Sin(To_rad(Angle[1]))));
            PointF end_ = new PointF(b2.Width / 2 + (float)(20d * Math.Cos(To_rad(y_))), b2.Height / 2 - (float)(20d * Math.Sin(To_rad(y_))));
            g2.DrawLine(new Pen(Color.FromArgb(128, 255, 0, 0)), center, end);
            g2.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255)), center, end_);
            g2.DrawString(Angle[1].ToString("0.00"), new Font("標楷體", 12, FontStyle.Regular), new SolidBrush(Color.FromArgb(128, 255, 0, 0)), end);
            g2.DrawString(y_.ToString("0.00"), new Font("標楷體", 12, FontStyle.Regular), new SolidBrush(Color.FromArgb(128, 0, 0, 255)), end_);
            e.Graphics.DrawImage(b2, 0, 0);
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            g3.Clear(Color.White);
            PointF center = new PointF(b3.Width / 2, b3.Height / 2);
            double x_ = deg_change(predict[0]);
            PointF end = new PointF(b3.Width / 2 + (float)(20d * Math.Cos(To_rad(Angle[0]))), b3.Height / 2 - (float)(20d * Math.Sin(To_rad(Angle[0]))));
            PointF end_ = new PointF(b3.Width / 2 + (float)(20d * Math.Cos(To_rad(x_))), b3.Height / 2 - (float)(20d * Math.Sin(To_rad(x_))));
            g3.DrawLine(new Pen(Color.FromArgb(128, 255, 0, 0)), center, end);
            g3.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255)), center, end_);
            g3.DrawString(Angle[0].ToString("0.00"), new Font("標楷體", 12, FontStyle.Regular), new SolidBrush(Color.FromArgb(128, 255, 0, 0)), end);
            g3.DrawString(x_.ToString("0.00"),new Font("標楷體",12,FontStyle.Regular),new SolidBrush(Color.FromArgb(128,0,0,255)),end_);
            e.Graphics.DrawImage(b3, 0, 0);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Baund = 38400;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Baund = 57600;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Baund = 115200;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Baund = 230400;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Baund = 460800;
            SetBaudrate(Baund);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            Baund = 921600;
            SetBaudrate(Baund);
        }

        int iBaudNo = 0;
        private void 自动检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iBaudNo = 0;
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (iBaudNo)            
            {
                case 0:   
                    Baund = 2400;
                    SetBaudrate(Baund);
                    sRightPack = 0;
                    Status.Text = "嘗試2400...";
                    break;
                case 1:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 4800;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試4800...";
                    }
                    break;
                case 2:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 9600;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試9600...";
                    }
                    break;
                case 3:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 19200;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試19200...";
                    }
                    break;
                case 4:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 38400;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試38400...";
                    }
                    break;
                case 5:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 57600;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試57600...";
                    }
                    break;
                case 6:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 115200;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試115200...";
                    }
                    break;
                case 7:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 230400;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試230400...";
                    }
                    break;
                case 8:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 460800;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試460800...";
                    }
                    break;
                case 9:
                    if (sRightPack > 2) {timer2.Stop();Status.Text = "找到裝置";}
                    else
                    {
                        Baund = 921600;
                        SetBaudrate(Baund);
                        sRightPack = 0;
                        Status.Text = "嘗試921600...";
                    }
                    break;
                case 10:
                    if (sRightPack > 2) { timer2.Stop(); Status.Text = "找到裝置"; }
                    else
                    {
                        timer2.Stop();
                        Status.Text = "沒找到裝置!!";
                    }
                    break;
            }
            iBaudNo = iBaudNo + 1;            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            DisplayRefresh();
            updtate_state(kf);

        }



       



    }
}
