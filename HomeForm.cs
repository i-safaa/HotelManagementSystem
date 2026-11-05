using System;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }


        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            this.Hide();
            clientForm.Show();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            StaffForm staff = new StaffForm();
            this.Hide();
            staff.Show();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.Show();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            RoomForm roomForm = new RoomForm();
            this.Hide();
            roomForm.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            ReservationForm reservationForm = new ReservationForm();
            this.Hide();
            reservationForm.Show();
        }
    }
}
