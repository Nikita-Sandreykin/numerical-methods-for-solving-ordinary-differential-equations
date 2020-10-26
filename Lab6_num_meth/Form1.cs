using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
namespace Lab6_num_meth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double f(double x, double y)
        {
            return (y * y * Math.Log(x) - y) / x;

        }
        double fa(double x)
        {
            return 1 / (Math.Log(x) + 1);
        }
        double h;
        double n;
        List<double> y_rc1, y_rc2;
        List<double> y_e1, y_e2;
        List<double> x1, x2;

        private void Button3_Click(object sender, EventArgs e)
        {
            RCtable rCtable = new RCtable();
            eTable eTable = new eTable();
            analyticTable analyticTable = new analyticTable();
            int j = 0;
            for(int i = 0; i < n+1; i++)
            {
                String[] temp = { "","", "", "", "" };
                String[] temp2 = { "", "", "", "", "" };
                String[] temp3 = { "", "", "", "", "", "", "" };
                temp3[0] = (i + 1).ToString(); temp3[1] = x1[i].ToString(); temp3[2] = fa(x1[i]).ToString(); temp3[3] = y_rc1[i].ToString(); temp3[4] = y_e1[i].ToString(); temp3[5] = Math.Abs(y_rc1[i] - fa(x1[i])).ToString(); temp3[6] = Math.Abs(y_e1[i] - fa(x1[i])).ToString();
                if (i%2 == 0)
                { 
                    temp[0] = (i+1).ToString(); temp[1] = x1[i].ToString(); temp[2] = y_rc1[i].ToString(); temp[3] = y_rc2[j].ToString(); temp[4] = Math.Abs(y_rc2[j] - y_rc1[i]).ToString();
                    temp2[0] = (i + 1).ToString(); temp2[1] = x1[i].ToString(); temp2[2] = y_e1[i].ToString(); temp2[3] = y_e2[j].ToString(); temp2[4] = Math.Abs(y_e2[j] - y_e1[i]).ToString();
                    j ++;
                }
                else
                {
                    temp[0] = (i + 1).ToString(); temp[1] = x1[i].ToString(); temp[2] = y_rc1[i].ToString();// temp[4] = Math.Abs(y_rc2[j] - y_rc1[j]).ToString();
                    temp2[0] = (i + 1).ToString(); temp2[1] = x1[i].ToString(); temp2[2] = y_e1[i].ToString();
                }
                rCtable.dataGridView1.Rows.Add(temp);
                eTable.dataGridView1.Rows.Add(temp2);
                analyticTable.dataGridView1.Rows.Add(temp3);
            }
            rCtable.Width = (rCtable.dataGridView1.RowHeadersWidth * 13) + 25;
            rCtable.Show();
            eTable.Width = rCtable.Width;
            eTable.Show();
            analyticTable.Show();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            RC4 rc = new RC4(1, 2.6, 0.1, 1, f);
            h = rc.getH(); 
            y_rc1 = rc.calculateY();
            x1 = rc.getX();
            String temp = "";
            temp += "Runge-Cutta tabble: \n"; 
            for(int i = 0; i<x1.Count; i++)
            {
                temp += (i+1) + " x: " + x1[i].ToString() + " y: " + y_rc1[i].ToString() + "\n"; 
            }
            rc.setH(2 * h);
            y_rc2 = rc.calculateY();
            x2 = rc.getX();
            for (int i = 0; i < y_rc2.Count; i++)
            {
                temp += (i+1) + " x: " + x2[i].ToString() + " y: " + y_rc2[i].ToString() + "\n";
            }
            Euler euler1 = new Euler(1, 2.6, h, 1, f);
            Euler euler2 = new Euler(1, 2.6, 2 * h, 1, f);
            y_e1 = euler1.calculateY();
            y_e2 = euler1.calculateY();
            for (int i = 0; i < x1.Count; i++)
            {
                temp += (i + 1) + " x: " + x1[i].ToString() + " y: " + y_e1[i].ToString() + "\n";
            }
            /*for (int i = 0; i < y_e2.Count; i++)
            {
                temp += (i + 1) + " x: " + x2[i].ToString() + " y: " + y_e2[i].ToString() + "\n";
            }*/
            //richTextBox1.Text = temp;
            label9.Text = h.ToString();
            label10.Text = euler1.getN().ToString();
            n = euler1.getN();
            richTextBox1.Text = rc.getLog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Graph graph = new Graph();
            GraphPane pane = graph.zedGraphControl1.GraphPane;
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            for (double x = 1; x <= 2.6; x += 0.000001)
            {
                // добавим в список точку
                list.Add(x, fa(x));
            }
            int i = 0;
            for (double x = 1; x <= 2.6; x += h)
            {
                // добавим в список точку
                list2.Add(x, y_rc1[i]);
                list3.Add(x, y_e1[i]);
                i++;
            }
            LineItem myCurve = pane.AddCurve("Analytical function", list, Color.Blue, SymbolType.None);
            LineItem myCurve2 = pane.AddCurve("Runge-Kutta function", list2, Color.Green, SymbolType.Circle);
            LineItem myCurve3 = pane.AddCurve("Euler function", list3, Color.Red, SymbolType.None);

            pane.XAxis.MajorGrid.IsVisible = true;
            pane.XAxis.MajorGrid.DashOn = 10;
            pane.XAxis.MajorGrid.DashOff = 5;
            pane.YAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.DashOn = 10;
            pane.YAxis.MajorGrid.DashOff = 5;
            pane.YAxis.MinorGrid.IsVisible = true;
            pane.YAxis.MinorGrid.DashOn = 1;
            pane.YAxis.MinorGrid.DashOff = 2;
            pane.XAxis.MinorGrid.IsVisible = true;
            pane.XAxis.MinorGrid.DashOn = 1;
            pane.XAxis.MinorGrid.DashOff = 2;
            graph.zedGraphControl1.AxisChange();
            graph.zedGraphControl1.Invalidate();
            graph.Show();
        }
    }
}
