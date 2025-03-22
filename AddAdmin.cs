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
    public partial class AddAdmin : Form
    {
        DBConnect dbcon = new DBConnect();

        public AddAdmin()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (lblAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please Select Admin ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblAdminID.Text != String.Empty)
                    if (DialogResult.Yes == MessageBox.Show("Do You Want Delete ?", "Conform", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spAdminDelete", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin Information Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindAdmin();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                            lblAdminID.Visible = false;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAdminName.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Name of Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdminName.Focus();
                return;
               // txtClear();
            }
            else if (txtAdminID.Text == String.Empty)
            {
                MessageBox.Show("Please Enter the User ID of Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdminID.Focus();
                //txtClear();
                return;
            }
            else if (txtPass.Text == String.Empty)
            {
                MessageBox.Show("Please Enter the Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Focus();
               // txtClear();
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblAdmin WHERE AdminID = @AdminID", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);

                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show("Admin Name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spAddAdmin", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);
                cmd.Parameters.AddWithValue("@Password", Convert.ToInt32(txtPass.Text));
                cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Admin Data Inserted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindAdmin();
                    lblAdminID.Visible = false;
                }
                else
                {
                    MessageBox.Show("Admin Insertion Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BindSeller()
        {
            throw new NotImplementedException();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrEmpty(lblAdminID.Text) || string.IsNullOrEmpty(txtAdminName.Text) ||
                    string.IsNullOrEmpty(txtPass.Text) || string.IsNullOrEmpty(txtAdminID.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate if password is numeric
                if (!int.TryParse(txtPass.Text, out int password))
                {
                    MessageBox.Show("Password must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SqlCommand cmd = new SqlCommand("spAdminUpdate", dbcon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;

                // If AdminID is an int in the database, convert it
                cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Admin Information Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindAdmin();
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnAdd.Visible = true;
                    lblAdminID.Visible = false;
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
            finally
            {
                dbcon.CloseCon();
            }

        }

        private void AddAdmin_Load(object sender, EventArgs e)
        {
            lblAdminID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            txtAdminName.Focus();
            BindAdmin();
        }

        private void BindAdmin()
        {
            SqlCommand cmd = new SqlCommand("select AdminID As [Admin ID],Password,FullName as [Full Name] from tblAdmin", dbcon.GetCon());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.OpenCon();
        }
        private void txtClear()
        {
            txtAdminName.Clear();
            txtPass.Clear();
            txtAdminID.Clear();
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtClear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                lblAdminID.Visible = false;
                btnAdd.Visible = false; 

                lblAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtPass.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtAdminName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
