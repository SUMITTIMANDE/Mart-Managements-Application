using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void fromMain_Load(object sender, EventArgs e)
        {
            if (Form1.Loginname!= null)
            {
                toolStripStatusLabel3.Text = Form1.Loginname;
            }
            if (Form1.Logintype != null && Form1.Logintype == "Seller")
            {
                masterToolStripMenuItem.Visible = false;
                productToolStripMenuItem.Visible = false;
                addUserToolStripMenuItem.Visible = false;
                //masterToolStripMenuItem.Enabled = false;
                //masterToolStripMenuItem.ForeColor = Color.Red;
                //productToolStripMenuItem.Enabled = false;
                //productToolStripMenuItem.ForeColor = Color.Red;
                //addUserToolStripMenuItem.Enabled = false;
                //addUserToolStripMenuItem.ForeColor = Color.Red;
            }
        }

        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategory fcat = new frmCategory();
            fcat.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abt = new AboutBox1();
            abt.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do You Want  Closed this Application ? ", "CLOSE", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if(dialog == DialogResult.No) 
            {
                e.Cancel= true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void sellerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewSeller frmadd = new frmAddNewSeller();
            frmadd.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAdmin frmaddAdmin = new AddAdmin();
            frmaddAdmin.ShowDialog();
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProduct frmaddProduct = new AddProduct();    
            frmaddProduct.ShowDialog();
        }
    }
} 
