using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Vector3D
    {

        public double x, y, z;

        public Vector3D()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D Copy()
        {
            Vector3D copy = new Vector3D(this.x, this.y, this.z);

            return copy;
        }


        public Vector3D Copy(Vector3D src)
        {
            Vector3D copy = new Vector3D(src.x, src.y, src.z);

            return copy;
        }

        public double Dot(Vector3D a, Vector3D b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public Vector3D Cross(Vector3D a, Vector3D b)
        {
            Vector3D c = new Vector3D();
            c.x = a.y * b.z - a.z * b.y;
            c.y = -(a.x * b.z - a.z * b.x);
            c.z = a.x * b.y - a.y * b.x;

            return c;
        }

        public Vector3D norm(Vector3D a)
        {
            double len = Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
            Vector3D norm = new Vector3D(a.x / len, a.y / len, a.z / len);

            return norm;
        }

        public double Get_R(Vector3D a)
        {
            double R = Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            return R;
        }

        public double Get_R()
        {
            double R = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);

            return R;
        }

        public Vector3D  Power(Vector3D a,int power)
        {
            Vector3D output = new Vector3D(Math.Pow(a.x,power), Math.Pow(a.y, power), Math.Pow(a.z, power));
            return output;
        }
    }


    class Vectorxd
    {
        private int  size;
        public double[] value;

        public Vectorxd()
        {

        }
        public Vectorxd(int size)
        {
            this.size = size;
            this.value = new double[size];

            for (int i = 0; i < size; i++)
            {
                this.value[i] = 0;
            }
        }

        public Vectorxd(int size,double[] value)
        {
            this.size = size;
            this.value = new double[size];

            for (int i = 0; i < size; i++)
            {
                this.value[i] = value[i];
            }
        }

        public Vectorxd Copy()
        {
            Vectorxd copy = new Vectorxd(this.size);

            for (int i = 0; i < this.size; i++)
            {
                copy.value[i] = this.value[i];
            }

            return copy;
        }


        public Vectorxd Copy(Vectorxd src)
        {
            Vectorxd copy = new Vectorxd(src.size);

            for (int i = 0; i < src.size; i++)
            {
                copy.value[i] = src.value[i];
            }

            return copy;
        }


        public void output()
        {
            for (int i = 0; i < this.size; i++)
            {
                Console.WriteLine(this.value[i]);
            }
        }
        public int Size()
        {
            return this.size;
        }
        public Vectorxd Resize(int size)
        {
            Vectorxd copy = new Vectorxd(size);

            return copy;
        }

        public static Vectorxd operator+(Vectorxd a,Vectorxd b)
        {
            Vectorxd c = new Vectorxd(a.size);

            for (int i = 0; i < a.size; i++)
            {
                c.value[i] = a.value[i] + b.value[i];
            }
            return c;
        }

        public static Vectorxd operator-(Vectorxd a, Vectorxd b)
        {
            Vectorxd c = new Vectorxd(a.size);

            for (int i = 0; i < a.size; i++)
            {
                c.value[i] = a.value[i] - b.value[i];
            }

            return c;
        }


        //乘法
        public static Vectorxd operator*(Vectorxd a, Vectorxd b)
        {
            Vectorxd c = new Vectorxd(a.size);

            for (int i = 0; i < a.size; i++)
            {
                //for (int j = 0; j < b.size; j++)
                //{
                    c.value[i] = a.value[i] * b.value[i];
                //}   
            }

            return c;
        }

        //
        public static Vectorxd operator /(Vectorxd a, Vectorxd b)
        {
            Vectorxd c = new Vectorxd(a.size);

            for (int i = 0; i < a.size; i++)
            {
                c.value[i] = a.value[i] / b.value[i];
            }
            return c;
        }
    }

    class Matrixd
    {
        private const string V = "can't do this operator because row or col can match each other";
        private int row, col;
        public double[,] value;

        public Matrixd()
        {

        }
        public Matrixd(int row ,int col)
        {
            this.row = row;
            this.col = col;
            this.value = new double[row,col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    this.value[i, j] = 0;
                }
            }
        }

        public Matrixd(int row, int col,double[,] val)
        {
            this.row = row;
            this.col = col;
            this.value = new double[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    this.value[i,j] = val[i,j];
                }
            }
        }

        public Vectorxd Get_row_vector(int row)
        {
            Vectorxd output = new Vectorxd(this.col);
            for (int i = 0; i < this.col; i++)
            {
                output.value[i] = this.value[row, i];
            }
            return output;
        }
        public Vectorxd Get_col_vector(int col)
        {
            Vectorxd output = new Vectorxd(this.row);
            for (int i = 0; i < this.row; i++)
            {
                output.value[i] = this.value[i, col];
            }
            return output;
        }


        public Vectorxd Get_row_vector(Matrixd a,int row)
        {
            Vectorxd output = new Vectorxd(a.col);
            for(int i = 0; i < a.col; i++)
            {
                output.value[i] = a.value[row, i];
            }
            return output;
        }
        public Vectorxd Get_col_vector(Matrixd a, int col)
        {
            Vectorxd output = new Vectorxd(a.row);
            for (int i = 0; i < a.row; i++)
            {
                output.value[i] = a.value[i, col];
            }
            return output;
        }

        public Matrixd Copy()
        {
            Matrixd copy = new Matrixd(this.row,this.col);
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    copy.value[i, j] = this.value[i, j];
                }
            }
            return copy;
        }

        public Matrixd Copy(Matrixd src)
        {
            Matrixd copy = new Matrixd(src.row,src.col);
            for (int i = 0; i < this.row; i++)
            { 
                for (int j = 0; j < this.col; j++)
                {
                    copy.value[i, j] = src.value[i, j];
                }
            }
            return copy;
        }

        public  Matrixd Transpose()
        {
            Matrixd Transpose = new Matrixd(this.col, this.row);

            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    Transpose.value[j,i] = this.value[i,j];
                }
            }
            return Transpose;
        }

        //單位矩陣 必為方陣
        public  Matrixd Identity(int row)
        {
            Matrixd I = new Matrixd(row, row);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (i == j)
                        I.value[i,j] = 1;
                }
            }
            return I;
        }

        //伴隨矩陣
        //reference::https://blog.csdn.net/fengbingchun/article/details/72518661
        public Matrixd adj()
        {
            Matrixd adj = new Matrixd(this.row, this.row);

            if (this.row * this.col == 1)
            {
                return this;
            }

            int N = this.row;
            for (int y = 0; y < N; ++y)
                {
                    int[] m_cols = new int[N];
                    for (int i = 0; i < N; ++i)
                    {
                        if (i != y) m_cols[i] = i;
                    }

                    for (int x = 0; x < N; ++x)
                    {
                        int[] m_rows = new int[N];
                        for (int i = 0; i < N; ++i)
                        {
                            if (i != x) m_rows[i] = i;
                        }

                        Matrixd m = new Matrixd(N - 1, N - 1);
                        //    for (int i = 0; i < N - 1; ++i)
                        //{
                        //    m.value[i,] = new double[N - 1];
                        //}
                    for (int j = 0; j < N - 1; ++j)
                        {
                            for (int i = 0; i < N - 1; ++i)
                            {
                                m.value[j,i] = this.value[m_rows[j],m_cols[i]];
                            }
                        }
                        int sign = (int)Math.Pow(-1, x + y);
                        adj.value[y,x] = sign * m.det(m);
                    }
                }
            return adj;
        }


        //計算行列式的值
        //referance::https://rumenz.com/examples/cpp/cplusplus-compute-the-determinant-of-a-matrix.html
        public double det(Matrixd A)
        {
            double det = new double();
            if (A.row == 1)
            {
                det = A.value[0,0];
                return det;
            }
            Matrixd M = new Matrixd(A.row - 1, A.row - 1);
            if (A.row == 2)
            {
                det = A.value[0,0] * A.value[1,1] - A.value[0,1] * A.value[1,0];
                return det;
            }
            else
            {
                for (int x = 0; x < A.row; x++)
                {
                    int subi = 0;
                    for (int i = 1; i < A.row; i++)
                    {
                        int subj = 0;
                        for (int j = 0; j < A.row; j++)
                        {
                            if (j == x)
                                continue;
                            M.value[subi,subj] = A.value[i,j];
                            subj++;
                        }
                        subi++;

                    }
                    det = det + (Math.Pow(-1, x) * A.value[0,x] * this.det(M));
                }
            }
            return det;
        }

        public Matrixd inv()
        {
            if (this.row * this.col == 1)
            {
                Matrixd inv = new Matrixd(this.row, this.col);
                double det = this.det(this);
                inv.value[0, 0] = this.value[0, 0] / det;
                return inv;
            }
            if (this.isInvterable() && (this.row * this.col ) != 1 )
            {
                //A(-1) = A*/det(A)
                Matrixd inv = new Matrixd(this.row, this.col);
                double det = this.det(this);
                Matrixd adj = this.adj();

                for (int i = 0; i < this.row; i++)
                {
                    for (int j = 0; j < this.row; j++)
                    {
                        inv.value[i,j] = adj.value[i,j] / det;
                    }
                }
                return inv;
            }
            else
            {
                Matrixd inv = new Matrixd(this.row, this.col);
                Console.WriteLine("this matrix can't inv");
                return inv;
            }      
        }


        public Matrixd pinv()
        {
            if (!this.isSqaure())
            {
                // m(row) > n(col) 右逆
                if (this.row < this.col)
                {
                    Matrixd Transpose = this.Transpose();
                    Matrixd A_AT = this * Transpose;
                    Matrixd inv = A_AT.inv();
                    Matrixd pinv = Transpose * inv;
                    return pinv;
                }
                else // m(row) < n(col) 左逆
                {
                    Matrixd Transpose = this.Transpose();
                    Matrixd A_AT = Transpose * this;
                    Matrixd inv = A_AT.inv();
                    Matrixd pinv = inv * Transpose;
                    return pinv;
                }
            }
            else
            {
                //Matrixd Transpose = this.Transpose();
                //Matrixd A_AT = Transpose * this;
                //Matrixd inv = A_AT.inv();
                //Matrixd pinv = inv * Transpose;
                Matrixd pinv = this.inv();
                return pinv;
            }
        }

        public bool isInvterable()
        {
            if (this.det(this) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public  bool isSqaure()
        {
            if (this.row == this.col)
                return true;
            else
                return false;
        }

        public void output_val()
        {
            for (int i = 0; i < this.row; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < this.col; j++)
                    Console.WriteLine(this.value[i,j]);
            }
        }

        public static Matrixd operator*(Matrixd A, Matrixd B)
        {
            Matrixd output = new Matrixd(A.row, B.col);

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < B.col; j++)
                {
                    for (int k = 0; k < A.col; k++)
                    {
                        output.value[i,j] += A.value[i,k] * B.value[k,j];
                    }
                }
            }

            return output; 
        }


        public static Vectorxd operator *(Matrixd A, Vectorxd B)
        {
            Vectorxd output = new Vectorxd();
            if (A.row < B.Size())
            {
               output = output.Resize(B.Size());
            }
            else
            {
                output = output.Resize(A.row);
            }
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < B.Size(); j++)
                {
                    output.value[i] += A.value[i, j] * B.value[j];
                }
            }

            return output;
        }
        public static Matrixd operator *(Matrixd A, double x)
        {
            Matrixd output = new Matrixd(A.row, A.col);

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.col; j++)
                {
                    output.value[i,j] = A.value[i, j] * x;
                }
            }

            return output;
        }

        public static Matrixd operator-(Matrixd A, Matrixd B)
        {
            Matrixd output = new Matrixd(A.row, A.col);

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.col; j++)
                {
                    output.value[i,j] = A.value[i,j] - B.value[i,j];
                }
            }

            return output;
        }

        public static Matrixd operator+(Matrixd A, Matrixd B)
        {
            Matrixd output = new Matrixd(A.row, A.col);

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.col; j++)
                {
                    output.value[i,j] = A.value[i,j] + B.value[i,j];
                }
            }

            return output;
        }

    }

}
