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
    public partial class frmCategory : Form
    {
        DBConnect dbcon = new DBConnect();
        public frmCategory()
        {
            InitializeComponent();
        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    btnUpdate.Visible = true;
        //    btnDelete.Visible = true;
        //    btnAddCat.Visible = false;
        //    string varCatID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
        //    txtCategeryName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        //    rtbCatDisc.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        //}

        private void frmCategory_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            LblCatID.Visible = false;
            BindCategory();
        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if(txtCategeryName.Text== String.Empty)
        //    {
        //        MessageBox.Show("Please Enter Categery Name.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //        txtCategeryName.Focus();
        //        return;
        //    }
        //    else if (rtbCatDisc.Text == String.Empty)
        //    {
        //        MessageBox.Show("Please Enter the Product Discripation","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //        rtbCatDisc.Focus();
        //        return;
        //    }
        //    else
        //    {
        //        SqlCommand cmd = new SqlCommand("select @CategoryName from tblCategory where @CategoryName=@CategoryName", dbcon.GetCon());
        //        cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
        //        dbcon.OpenCon();
        //        var result = cmd.ExecuteScalar();
        //        if (result != null)
        //        {
        //            MessageBox.Show ("Category is alredy exist ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            txtClear();
        //        }
        //        else
        //        {

        //            cmd = new SqlCommand("spCatInsert", dbcon.GetCon());
        //           // cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(LblCatID.Text));
        //            cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
        //            cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDisc.Text);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            int i = cmd.ExecuteNonQuery();
        //            if (i >0)
        //            {
        //                MessageBox.Show("Category Inserte Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                txtClear();
        //                BindCategory();
        //            }
        //        }
        //        dbcon.CloseCon();
        //    }
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCategeryName.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Category Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategeryName.Focus();
                return;
            }
            else if (rtbCatDisc.Text == String.Empty)
            {
                MessageBox.Show("Please Enter the Product Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbCatDisc.Focus();
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblCategory WHERE CategoryName = @CategoryName", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);

                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show("Category already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spCatInsert", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
                cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDisc.Text);
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Category Inserted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindCategory();
                    LblCatID.Visible = false;
                }
                else
                {
                    MessageBox.Show("Insertion Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtCategeryName.Clear();
            rtbCatDisc.Clear();
        }
        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("select CatID As CategoryID,CategoryName,CategoryDesc as CategoryDescription from tblCategory ", dbcon.GetCon());
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
            LblCatID.Visible= false; 
            btnAddCat.Visible = false;

            LblCatID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtCategeryName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rtbCatDisc.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

        }

        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (LblCatID.Text == String.Empty)
        //        {
        //            MessageBox.Show("Please Select Your CategeryID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            txtCategeryName.Focus();
        //            return;
        //        }
        //        if (txtCategeryName.Text == String.Empty)
        //        {
        //            MessageBox.Show("Please Enter Categery Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            txtCategeryName.Focus();
        //            return;
        //        }
        //        else if (rtbCatDisc.Text == String.Empty)
        //        {
        //            MessageBox.Show("Please Enter the Product Discripation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            rtbCatDisc.Focus();
        //            return;
        //        }
        //        else
        //        {
        //            SqlCommand cmd = new SqlCommand("select @CategoryName from tblCategory where @CategoryName=@CategoryName", dbcon.GetCon());
        //            cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
        //            dbcon.OpenCon();
        //            var result = cmd.ExecuteScalar();
        //            if (result != null)
        //            {
        //                MessageBox.Show(String.Format("CategoryName {0} alredy exist"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                txtClear();
        //            }
        //            else
        //            {

        //                cmd = new SqlCommand("spCatUpdate", dbcon.GetCon());
        //                cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(LblCatID.Text));
        //                cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
        //                cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDisc.Text);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                int i = cmd.ExecuteNonQuery();
        //                if (i > 0)
        //                {
        //                    MessageBox.Show("Category Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    txtClear();
        //                    BindCategory();
        //                    btnUpdate.Visible = false;
        //                    btnDelete.Visible = false;
        //                    btnAddCat.Visible = true;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Update Fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //            dbcon.CloseCon();
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        //    }
        //}

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (LblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please Select Your Category ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCategeryName.Focus();
                    return;
                }
                if (txtCategeryName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Category Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCategeryName.Focus();
                    return;
                }
                else if (rtbCatDisc.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter the Product Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtbCatDisc.Focus();
                    return;
                }

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblCategory WHERE CategoryName = @CategoryName AND CatID <> @CatID", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
                cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(LblCatID.Text));

                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show($"CategoryName '{txtCategeryName.Text}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spCatUpdate", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(LblCatID.Text));
                cmd.Parameters.AddWithValue("@CategoryName", txtCategeryName.Text);
                cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDisc.Text);
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Category Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindCategory();
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnAddCat.Visible = true;
                    LblCatID.Visible= false ; // Clear selected ID
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
                if (LblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please Select CategoryID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (LblCatID.Text != String.Empty)
                    if(DialogResult.Yes==MessageBox.Show("Do You Want Delete ?", "Conform", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                        SqlCommand cmd = new SqlCommand("spCatDelete", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(LblCatID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindCategory();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAddCat.Visible = true;
                            LblCatID.Visible = false;
                            btnClear.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Delete failed..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }

                    }
            } catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtClear();
            btnAddCat.Visible= true;
            btnUpdate.Visible= false;
            btnDelete.Visible= false;

        }
    }
}
