using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_num_meth
{
    delegate double func(double x, double y);
    class RC4
    {
        private List<double> y;
        private List<double> x;
        private List<double> k = new List<double>(4);
        private List<double> y1_nexts = new List<double>(2);
        private List<double> y2_nexts = new List<double>(2);
        private double a, b;
        string log = "Вычисление шага (начальный шаг 0.1): \n";
        func f;
        private double h;
        private int n;
        public RC4(double a, double b, double h0, double y0, func f)
        {
            for(int i = 0; i < 4; i++) { k.Add(0); }
            h = h0;
            x = new List<double>();
            y = new List<double>();
            this.a = a; this.b = b;
            x.Add(a);
            y.Add(y0);
            this.f = f;
            calibrateh();
            setX();
        }
        public void setH(double h)
        {
            double xtemp = x[0];
            double ytemp = y[0];
            y = new List<double>();
            x = new List<double>();
            y.Add(ytemp);
            x.Add(xtemp);
            n = (int)((b-a) / h) ;
            this.h = h;
            setX();
        }
        public double getH()
        {
            return h;
        }
        private double checkEps()
        {
            double h_temp = h;
            List<double> y1_nexts = new List<double>();
            List<double> y2_nexts = new List<double>();
            calculateK(x[0], y[0]);
            y1_nexts.Add(y[0] + (k[0] + 2 * k[1] + 2 * k[2] + k[3]) / 6);
            calculateK(x[0] + h, y1_nexts[0]);
            y1_nexts.Add(y1_nexts[0] + (k[0] + 2 * k[1] + 2 * k[2] + k[3]) / 6);

            h *= 2;

            calculateK(x[0], y[0]);
            y2_nexts.Add(y[0] + (k[0] + 2 * k[1] + 2 * k[2] + k[3]) / 6);
            h = h_temp;
            return Math.Abs(y2_nexts[0] - y1_nexts[1]);
        }
        private void calibrateh()
        {
            double e = checkEps();
            while(e < 0.0001)
            {
                log += e.ToString() + " < " + "0.0001 => увеличиваем шаг в 2 раза \n";
                h *= 2;
                log += h.ToString() + " - новый шаг \n";
                e = checkEps();
            }
            log += e.ToString() + " > " + "0.0001 =>" + " вычисляем количество шагов: \n";
            n = (int)((b - a) / h);
            log += n.ToString() + "\n";
            if(n%2 == 1)
            {
                log += "n - нечетно, увеличиваем n на 1 \n";
                n++;
            }
            h = (b - a) / n;
        }
        private void setX()
        {
            for (int i = 1; i < n + 1; i++)
            {
                x.Add(x[i - 1] + h);
            }
        }
        private void calculateK(double xi, double yi)
        {
            k[0] = h * f(xi, yi);
            k[1] = h * f(xi + h / 2, yi + k[0] / 2);
            k[2] = h * f(xi + h / 2, yi + k[1] / 2);
            k[3] = h * f(xi + h, yi + k[2]);
        }
        public string getLog()
        {
            return log;
        }
        public List<double> getX()
        {
            return x;
        }
        public List<double> calculateY()
        {
            for(int i = 1; i < x.Count; i++)
            {
                calculateK(x[i - 1], y[i - 1]);
                y.Add(y[i - 1] + (k[0] + 2 * k[1] + 2 * k[2] + k[3]) / 6);
            }
            return y;
        }
    }
}
