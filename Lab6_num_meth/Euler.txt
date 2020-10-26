using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_num_meth
{
    class Euler
    {
        private List<double> y = new List<double>();
        private List<double> x = new List<double>();
        private double a, b;
        func f;
        private double h;
        private int n;
        public Euler(double a, double b, double h0, double y0, func f)
        {
            this.a = a;
            this.b = b;
            this.h = h0;
            y.Add(y0);
            x.Add(a);
            n = (int)((b - a) / h);
            this.f = f;
            setX();
        }
        private void setX()
        {
            for (int i = 1; i < n + 1; i++)
            {
                x.Add(x[i - 1] + h);
            }
        }
        public List<double> getX()
        {
            return x;
        }
        public int getN()
        {
            return n;
        }
        public List<double> calculateY()
        {
            for (int i = 1; i < x.Count; i++)
            {
                y.Add(y[i - 1] + h*f(x[i-1], y[i-1]));
            }
            return y;
        }
    }
}
