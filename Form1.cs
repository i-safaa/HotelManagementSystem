using System;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int StartPoint = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartPoint++;
            bunifuCircleProgressbar1.Value = StartPoint;

            if (bunifuCircleProgressbar1.Value == 50)
            {
                StartPoint = 0;
                bunifuCircleProgressbar1.Value = 0;
                timer1.Stop();

                LoginForm loginForm = new LoginForm();
                this.Hide();
                loginForm.Show();
            }
        }
    }
}
