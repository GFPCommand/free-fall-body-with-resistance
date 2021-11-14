using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Computing computing;
        double mass = 0, height = 0, circumference = 0, radius = 0, m_density = 0, e_density = 0, resistance = 0, viscous = 0;
        private byte _variant;

        Axis x1, y1, x2, y2;

        public Form1()
        {
            InitializeComponent();

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[0].ChartType = SeriesChartType.Line;

            radioButton1.Checked = true;

            x1 = new Axis();
            y1 = new Axis();
            x2 = new Axis();
            y2 = new Axis();

            x1.Title = "Time";
            y1.Title = "Speed";
            x2.Title = "Time";
            y2.Title = "Height";

            chart1.ChartAreas[0].AxisX = x1;
            chart1.ChartAreas[0].AxisY = y1;
            
            chart2.ChartAreas[0].AxisX = x2;
            chart2.ChartAreas[0].AxisY = y2;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text  = "";
            textBox5.Text  = "";
            textBox6.Text  = "";
            textBox13.Text = "";

            textBox8.Text = "Height: ";
            textBox9.Text = "Speed: ";

            textBox1.Text = "Mass";
            textBox2.Text = "Height";
            textBox3.Text = "Circumference";
            textBox14.Text = "Resistance";

            textBox3.Visible = true;
            textBox6.Visible = true;

            textBox11.Visible = false;
            textBox12.Visible = false;

            _variant = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            textBox8.Text = "Height: ";
            textBox9.Text = "Speed: ";

            textBox1.Text = "Mass";
            textBox2.Text = "Radius";
            textBox14.Text = "Resistance";

            textBox3.Visible  = false;
            textBox6.Visible  = false;
            textBox11.Visible = false;
            textBox12.Visible = false;

            _variant = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            textBox8.Text = "Height: ";
            textBox9.Text = "Speed: ";

            textBox1.Text = "Metal density";
            textBox2.Text = "Env. density";
            textBox3.Text = "Radius";
            textBox14.Text = "Viscous";

            textBox3.Visible = true;
            textBox6.Visible = true;

            _variant = 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (_variant)
                {
                    case 1:
                        mass = double.Parse(textBox4.Text);
                        height = double.Parse(textBox5.Text);
                        circumference = double.Parse(textBox6.Text);
                        resistance = double.Parse(textBox13.Text);
                        break;
                    case 2:
                        radius = double.Parse(textBox5.Text);
                        mass = double.Parse(textBox4.Text);
                        resistance = double.Parse(textBox13.Text);
                        break;
                    case 3:
                        m_density = double.Parse(textBox4.Text);
                        e_density = double.Parse(textBox5.Text);
                        viscous = double.Parse(textBox13.Text);
                        radius = double.Parse(textBox6.Text);
                        break;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("You entered wrong data", "Error");

                textBox4.Text  = "";
                textBox5.Text  = "";
                textBox6.Text  = "";
                textBox13.Text = "";
            }

            switch (_variant)
            {
                case 1:
                    if (mass >= 0 && height >= 0 && circumference >= 0 && resistance >= 0)
                    {
                        computing = new Computing(mass, height, circumference, resistance);
                        computing.WithoutParachute();
                        GraphOutput();
                        textBox8.Text = "Height: " + Math.Round(computing.GetHeight()[computing.GetHeight().Count - 1], 2).ToString();
                        textBox9.Text = "Speed: " + Math.Round(computing.GetSpeed()[computing.GetSpeed().Count - 1], 2).ToString();
                    }
                    break;
                case 2:
                    if (mass >= 0 && resistance >= 0)
                    {
                        double S = Math.PI * Math.Pow(radius, 2);
                        computing = new Computing(S, mass, resistance);
                        computing.WithParachute();
                        GraphOutput();
                        textBox8.Text = "Height: " + Math.Round(computing.GetHeight()[computing.GetHeight().Count - 1], 2).ToString();
                        textBox9.Text = "Speed: " + Math.Round(computing.GetSpeed()[computing.GetSpeed().Count - 1], 2).ToString();
                    }
                    break;
                case 3:
                    if (m_density >= 0 && e_density >= 0 && viscous >= 0)
                    {
                        double mass1 = (4 / 3) * Math.PI * Math.Pow(radius, 3) * (m_density - e_density);
                        double mass2 = (4 / 3) * Math.PI * Math.Pow(radius, 3) * m_density;
                        computing = new Computing(m_density, e_density, viscous, radius, mass1, mass2);
                        computing.ViscousMedium();
                        GraphOutput();
                        textBox8.Text = "Height: " + Math.Round(computing.GetHeight()[computing.GetHeight().Count - 1], 2).ToString();
                        textBox9.Text = "Speed: " + Math.Round(computing.GetSpeed()[computing.GetSpeed().Count - 1], 2).ToString();
                    }
                    break;
            }
        }

        private void GraphOutput()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            for (int i = 0; i < computing.GetHeight().Count; i++)
            {
                chart1.Series[0].Points.AddXY(Math.Round(computing.GetTime()[i]), Math.Round(computing.GetSpeed()[i], 2));
                chart2.Series[0].Points.AddXY(Math.Round(computing.GetTime()[i]), Math.Round(computing.GetHeight()[i], 2));
            }
        }
    }
}
