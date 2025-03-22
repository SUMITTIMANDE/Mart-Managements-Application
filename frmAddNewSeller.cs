using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class frmAddNewSeller : Form
    {
        DBConnect dbcon = new DBConnect();

        public frmAddNewSeller()
        {
            InitializeComponent();
        }

        private void frmAddNewSeller_Load(object sender, EventArgs e)
        {
            lblSellerID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            BindSeller();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSellerName.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Seller Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSellerName.Focus();
                return;
            }
            else if (txtPass.Text == String.Empty)
            {
                MessageBox.Show("Please Enter the Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAge.Focus();
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblSeller WHERE SellerName = @SellerName", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);

                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show("Seller Name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spSellerInsert", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                cmd.Parameters.AddWithValue("@SellerPass", Convert.ToInt32(txtPass.Text));
                cmd.Parameters.AddWithValue("@SellerAge" ,txtAge.Text);
                cmd.Parameters.AddWithValue("@SellerPhone" ,txtPhone.Text);
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Seller Data Inserted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindSeller();
                    lblSellerID.Visible = false;
                }
                else
                {
                    MessageBox.Show("Seller Insertion Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbcon.CloseCon(); // Ensure connection is always closed
            }


        }

        private void txtClear()
        {
            txtSellerName.Clear();
            txtPass.Clear();
            txtPhone.Clear();
            txtAge.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please Select Seller ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   // txtSellerName.Focus();
                    return;
                }
                if (txtSellerName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Seller Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;
                }
                else if (txtPass.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Focus();
                    return;
                }

                SqlCommand cmd = new SqlCommand("SELECT SellerName FROM tblSeller WHERE SellerName = @SellerName" , dbcon.GetCon());
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                //cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(lblSellerID .Text));

                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show($"Seller is '{txtSellerName.Text}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spSellerUpdate", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(lblSellerID.Text));
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                cmd.Parameters.AddWithValue("@SellerAge",Convert.ToInt32( txtAge.Text));
                cmd.Parameters.AddWithValue("@SellerPhone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@SellerPass",txtPass.Text);
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Seller Information Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindSeller();
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnAdd.Visible = true;
                    lblSellerID.Visible = false; // Clear selected ID
                }
                else
                {
                    MessageBox.Show("Update Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please Select Seller ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblSellerID.Text != String.Empty)
                    if (DialogResult.Yes == MessageBox.Show("Do You Want Delete ?", "Conform", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spSellerDelete", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(lblSellerID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Information Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindSeller();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                            lblSellerID.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Delete failed..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }

                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindSeller()
        {
            SqlCommand cmd = new SqlCommand("select * from tblSeller ", dbcon.GetCon());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.OpenCon();

        }



      

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblSellerID.Visible = false;
            btnAdd.Visible = false;

            lblSellerID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtSellerName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtAge.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPass.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
