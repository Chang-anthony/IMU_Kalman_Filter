using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;


namespace Kalman_filter
{
 
    class Kalman_filter
    {
       public Vectorxd x = new Vectorxd();// x 狀態向量 input 數值 1.....n object state
       private Matrixd P = new Matrixd(); //狀態斜方差矩陣 表示對於每個觀測到狀態輸入 會如何影響到其他的狀態輸入 object covariance matrix
       // u is the control vector, representing the controlling input into control-input model 控制向量，表示控制每個狀態input的控制分量(input)
       private Matrixd F = new Matrixd();//the state-transition model 狀態轉移矩陣  前一個狀態跟現在狀態的關係
       private Matrixd H = new Matrixd();//the observation model 觀察模型
       private Matrixd R = new Matrixd(); // the covariance of the observation noise 觀測模型本身的斜方差雜訊影響
       private Matrixd B = new Matrixd();//表式當前控制量 如何影響我們的狀態
       private Matrixd I = new Matrixd();//Identity matrix 最後大小跟狀態預測矩陣的呈現N維矩陣
       private Matrixd Q = new Matrixd();//the covariance of the process noise;表示預測模型本身所有的雜訊影響
       public bool isintial = new bool();


        public Kalman_filter()
        {
            this.isintial = false;
        }

        public void Set_Filter_params(Vectorxd xk_1, Matrixd I,Matrixd P,Matrixd H,Matrixd Q,Matrixd R)
        {
            this.x = xk_1.Copy();
            this.P = P.Copy();
            this.H = H.Copy();
            this.Q = Q.Copy();
            this.R = R.Copy();
            this.I = I.Copy();
            
        }
        public void Set_F(Matrixd F) 
        {
            this.F = F.Copy();
        }

        public void Set_B(Matrixd B)
        {
            this.B = B.Copy();
            this.isintial = true;
        }

        //xk_1 前一時刻狀態 
        public Vectorxd update(Vectorxd xk_1,Vectorxd Zk, Vectorxd u)
        {

            //Matrixd temp2 = this.H * Pk_ * this.H.Transpose();
            //Matrixd temp = (this.H * Pk_ * this.H.Transpose() + this.R).pinv();
            //預測
            Vectorxd xk_ = this.F * xk_1 + this.B * u;
            Matrixd Pk_ = this.F * this.P * this.F.Transpose() + this.Q;

            Vectorxd H = this.H.Get_row_vector(0);

            //更新 Kk k 時刻下的卡爾曼增益
            Matrixd Kk = Pk_ * this.H.Transpose() * ((this.H * Pk_ * this.H.Transpose() + this.R)).pinv();
            Vectorxd K_k = Kk.Get_col_vector(0);
            this.x = xk_ + K_k * (Zk - H * xk_);
            this.P = (this.I - Kk * this.H) * Pk_;

            Vectorxd error = this.x - xk_;

            //error.output();

            return this.x;
        }

    }
}
