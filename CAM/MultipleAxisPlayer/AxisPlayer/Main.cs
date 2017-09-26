using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AxisPlayer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Player> lstPlayer = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                Player p = new Player();
                lstPlayer.Add(p);
            }

            MessageBox.Show("Show");

            foreach (Player p in lstPlayer)
            {
                p.Show();
            }

            MessageBox.Show("Play");

            foreach (Player p in lstPlayer)
            {
                p.Play();
            }

        }
    }
}
