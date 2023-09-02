using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace imu
{
    //  Path    C:\\robot\\bridge.txt

    //  Format   
    //      ---------------------------------------------------------------------------------------------------------
    //                  Column        1           2           3           4           5           6           7     |
    //                                                                                                              |
    //         Row      Object  |   Center  |    Top    |   Bottom  |   Left    |   Right   |  Distance |    Find   |
    //          1       Ball    |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |     d     |     0     |
    //          2       Obstade |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |     d     |     0     |
    //          3       Door    |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |     d     |     0     |
    //          4       Camera  |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |   (x,y)   |     d     |     0     |
    //      ---------------------------------------------------------------------------------------------------------

    public class FileControl
    {
        public string Path;
        public double[] gyrodata = new double[9];
        public int[] value = new int[48];
        public string data = "";
        public int[] output;

        public FileControl(string setPath)
        {
            Path = setPath;
            data = "";
            for (int i = 0; i < 48; i++)
            {
                value[i] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                gyrodata[i] = 0;
            }
        }
        public FileControl()
        {
            Path = "C:\\robot\\bridge.txt";
            data = "";
            for (int i = 0; i < 48; i++)
            {
                value[i] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                gyrodata[i] = 0;
            }
        }
        public class Point
        {
            public int X;
            public int Y;
            public Point()
            {
                X = 0;
                Y = 0;
            }

        }
        public class Coordinate
        {
            public string Name;
            public Point Center = new Point();
            public Point Top = new Point();
            public Point Bottom = new Point();
            public Point Left = new Point();
            public Point Right = new Point();
            public double Distance;
            public int Find;
            public Coordinate(string style)
            {
                Name = style;
                Find = 0;
                Distance = 0;
            }
        }

        public class Sensor
        {
            public double[] a;
            public double[] w;
            public double[] angle;
            public Sensor()
            {
                a = new double[3];
                w = new double[3];
                angle = new double[3];
            }

        }
        public Coordinate Ball = new Coordinate("Ball");
        public Coordinate Obstade = new Coordinate("Obstade");
        public Coordinate Door = new Coordinate("Door");
        public Coordinate Camera = new Coordinate("Camera");
        public Sensor gyro = new Sensor();
        public void updataFromRead()
        {
            Ball.Center.X = value[0];
            Ball.Center.Y = value[1];
            Ball.Top.X = value[2];
            Ball.Top.Y = value[3];
            Ball.Bottom.X = value[4];
            Ball.Bottom.Y = value[5];
            Ball.Left.X = value[6];
            Ball.Left.Y = value[7];
            Ball.Right.X = value[8];
            Ball.Right.Y = value[9];
            Ball.Distance = value[10];
            Ball.Find = value[11];

            Obstade.Center.X = value[12];
            Obstade.Center.Y = value[13];
            Obstade.Top.X = value[14];
            Obstade.Top.Y = value[15];
            Obstade.Bottom.X = value[16];
            Obstade.Bottom.Y = value[17];
            Obstade.Left.X = value[18];
            Obstade.Left.Y = value[19];
            Obstade.Right.X = value[20];
            Obstade.Right.Y = value[21];
            Obstade.Distance = value[22];
            Obstade.Find = value[23];

            Door.Center.X = value[24];
            Door.Center.Y = value[25];
            Door.Top.X = value[26];
            Door.Top.Y = value[27];
            Door.Bottom.X = value[28];
            Door.Bottom.Y = value[29];
            Door.Left.X = value[30];
            Door.Left.Y = value[31];
            Door.Right.X = value[32];
            Door.Right.Y = value[33];
            Door.Distance = value[34];
            Door.Find = value[35];

            Camera.Center.X = value[36];
            Camera.Center.Y = value[37];
            Camera.Top.X = value[38];
            Camera.Top.Y = value[39];
            Camera.Bottom.X = value[40];
            Camera.Bottom.Y = value[41];
            Camera.Left.X = value[42];
            Camera.Left.Y = value[43];
            Camera.Right.X = value[44];
            Camera.Right.Y = value[45];
            Camera.Distance = value[46];
            Camera.Find = value[47];
        }
        public void updataToWrite()
        {
            value[0]      = Ball.Center.X       ;  
            value[1]      = Ball.Center.Y       ;  
            value[2]      = Ball.Top.X          ;  
            value[3]      = Ball.Top.Y          ;  
            value[4]      = Ball.Bottom.X       ;  
            value[5]      = Ball.Bottom.Y       ;  
            value[6]      = Ball.Left.X         ;  
            value[7]      = Ball.Left.Y         ;  
            value[8]      = Ball.Right.X        ;  
            value[9]      = Ball.Right.Y        ;  
            value[10]       = Convert.ToInt16( Ball.Distance  )      ;
            value[11]       = Ball.Find            ;

            value[12]       = Obstade.Center.X     ;
            value[13]       = Obstade.Center.Y     ;
            value[14]       = Obstade.Top.X        ;
            value[15]       = Obstade.Top.Y        ;
            value[16]       = Obstade.Bottom.X     ;
            value[17]       = Obstade.Bottom.Y     ;
            value[18]       = Obstade.Left.X       ;
            value[19]       = Obstade.Left.Y       ;
            value[20]       = Obstade.Right.X      ;
            value[21]       = Obstade.Right.Y      ;
            value[22]       = Convert.ToInt16(Obstade.Distance)     ;
            value[23]       = Obstade.Find         ;

            value[24]       = Door.Center.X        ;
            value[25]       = Door.Center.Y        ;
            value[26]       = Door.Top.X           ;
            value[27]       = Door.Top.Y           ;
            value[28]       = Door.Bottom.X        ;
            value[29]       = Door.Bottom.Y        ;
            value[30]       = Door.Left.X          ;
            value[31]       = Door.Left.Y          ;
            value[32]       = Door.Right.X         ;
            value[33]       = Door.Right.Y         ;
            value[34]       =Convert.ToInt16( Door.Distance  )      ;
            value[35]       = Door.Find            ;

            value[36]       = Camera.Center.X      ;
            value[37]       = Camera.Center.Y      ;
            value[38]       = Camera.Top.X         ;
            value[39]       = Camera.Top.Y         ;
            value[40]       = Camera.Bottom.X      ;
            value[41]       = Camera.Bottom.Y      ;
            value[42]       = Camera.Left.X        ;
            value[43]       = Camera.Left.Y        ;
            value[44]       = Camera.Right.X       ;
            value[45]       = Camera.Right.Y       ;
            value[46]       = Convert.ToInt16(Camera.Distance)      ;
            value[47]       = Camera.Find          ;

            data = "";
            for (int Row = 0; Row < 4; Row++)
            {
                for (int Column = 0; Column < 12; Column++)
                {
                    if (Column == 0)
                    {
                        data += value[Column + Row * 12];
                    }
                    else
                    {
                        data += " " + value[Column + Row * 12];
                    }
                }
                data += "\n";
            }

        }
        public void Read()
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '(', ')', '\t', '\n' };
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String line;
            int Row = 0;
            while ((line = sr.ReadLine()) != null)
            {
                String[] Words = line.Split(delimiterChars);
                int Column = 0;
                foreach (String s in Words)
                {
                    value[Column + Row * 12] = Convert.ToInt16(s);
                    Column++;
                }
                Row++;
            }
            updataFromRead();
            sr.Close();
        }

        public void ReadGyro()
        {
            char[] delimiterChars = { ' ', ',', ':', '(', ')', '\t', '\n' };
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String line;
            
            while ((line = sr.ReadLine()) != null)
            {
                String[] Words = line.Split(delimiterChars);
                int Column = 0;
                foreach (String s in Words)
                {
                    gyrodata[Column] = Convert.ToDouble(s);
                    Column++;
                }
                
            }
            gyro.a[0] = gyrodata[0];
            gyro.a[1] = gyrodata[1];
            gyro.a[2] = gyrodata[2];
            gyro.w[0] = gyrodata[3];
            gyro.w[1] = gyrodata[4];
            gyro.w[2] = gyrodata[5];
            gyro.angle[0] = gyrodata[6];
            gyro.angle[1] = gyrodata[7];
            gyro.angle[2] = gyrodata[8];
            sr.Close();
        }
        public void Read(string path)
        {
            char[] delimiterChars = { ' ', ',', ':', '(', ')', '\t', '\n' };
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String line;
            int Row = 0;
            while ((line = sr.ReadLine()) != null)
            {
                String[] Words = line.Split(delimiterChars);
                int Column = 0;
                foreach (String s in Words)
                {
                    value[Column + Row * 12] = Convert.ToInt16(s);
                    Column++;
                }
                Row++;
            }
            updataFromRead();
            sr.Close();
            fs.Close();
        }
        public void Read(string path,ref int Data)
        {
            char[] delimiterChars = { ' ', ',', ':', '(', ')', '\t', '\n' };
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String line;
            int Row = 0;
            while ((line = sr.ReadLine()) != null)
            {
                String[] Words = line.Split(delimiterChars);
                int Column = 0;
                foreach (String s in Words)
                {
                    value[Column + Row * 12] = Convert.ToInt16(Convert.ToDouble(s));
                    Column++;
                }
                Row++;
            }
            Data = value[0];
            sr.Close();
            fs.Close();
        }
        public int[] Read(string path,bool test)
        {
            char[] delimiterChars = { ' ', ',', ':', '(', ')', '\t', '\n' };
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String line;
            int a = 0;
            while ((line = sr.ReadLine()) != null)
            {
                String[] Words = line.Split(delimiterChars);
               
                foreach (String s in Words)
                {
                    if (s=="True" || s=="true")
                    {
                        value[a] = 1;
                    }
                    else if (s == "False" || s == "false")
                    {
                        value[a] = 0;
                    }
                    else
                    {
                        value[a] = Convert.ToInt16(s);
                    }
                    
                    a++;
                }
                
            }
            output = new int[value[1]+2];
            for (int i = 0; i < (value[1]+2); i++)
            {
                output[i] = value[i];
            }
            sr.Close();
            fs.Close();
            return output;
        }
        public void WriteGyroALL()
        {
            double[] information = new double[9];
            string temp = "";
            Path = @"C:\Users\anthony\Desktop\IMU\imudata.txt";
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            information[0] = gyro.a[0];
            information[1] = gyro.a[1];
            information[2] = gyro.a[2];
            information[3] = gyro.w[0];
            information[4] = gyro.w[1];
            information[5] = gyro.w[2];
            information[6] = gyro.angle[0];
            information[7] = gyro.angle[1];
            information[8] = gyro.angle[2];
            /*
            for (int i = 0; i < 9; i++)
            {
                if (i!=8)
                {
                    temp += information[i] + " ";
                }
                else
                {
                    temp += information[i] + "\n";
                }
                
            }
            */
            temp = "" + information[6];
            temp += " " + information[7];
            temp += " " + information[8];
            //開始寫入
            sw.Write(temp);
            //清空緩衝區
            sw.Flush();
            //關閉流
            sw.Close();
            fs.Close();
        }
        public void WriteGyro()
        {
            double[] information=new double[9];
            string temp="" ;
            Path= @"C:\Users\anthony\Desktop\IMU\imudata.txt";
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            information[0] = gyro.a[0];
            information[1] = gyro.a[1];
            information[2] = gyro.a[2];
            information[3] = gyro.w[0];
            information[4] = gyro.w[1];
            information[5] = gyro.w[2];
            information[6] = gyro.angle[0];
            information[7] = gyro.angle[1];
            information[8] = gyro.angle[2];
            /*
            for (int i = 0; i < 9; i++)
            {
                if (i!=8)
                {
                    temp += information[i] + " ";
                }
                else
                {
                    temp += information[i] + "\n";
                }
                
            }
            */
            temp = ""+information[8];
            //開始寫入
            sw.Write(temp);
            //清空緩衝區
            sw.Flush();
            //關閉流
            sw.Close();
            fs.Close();
        }
        
        public void Write(string path)
        {
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            updataToWrite();
            //開始寫入
            sw.Write(data);
            //清空緩衝區
            sw.Flush();
            //關閉流
            sw.Close();
            fs.Close();
        }
        public void Write(string path, string data)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            //開始寫入
            sw.Write(data);
            //清空緩衝區
            sw.Flush();
            //關閉流
            sw.Close();
            fs.Close();
        }

    }
}
