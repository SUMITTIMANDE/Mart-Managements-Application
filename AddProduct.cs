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
    public partial class AddProduct : Form
    {
        DBConnect dbcon = new DBConnect();

        public AddProduct()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;
                }
                else if (txtPrice.Text == String.Empty && Convert.ToInt32(txtPrice.Text) >= 0)
                {
                    MessageBox.Show("Please Enter the Price and Pricde Must Be Gretter Then Zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQty.Text == String.Empty && Convert.ToInt32(txtQty.Text) >= 0)
                {
                    MessageBox.Show("Please Enter the Quantity and it Must Be Gretter Then Zero .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else if (cmbCategory.Text == String.Empty)
                {
                    MessageBox.Show("Please Select the Category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }

                SqlCommand cmd = new SqlCommand("spCheckCategoryDuplicate", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcon.CloseCon();

                if (count > 0)
                {
                    MessageBox.Show("Product Name is already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                    return;
                }

                cmd = new SqlCommand("spInsertProduct", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                cmd.Parameters.AddWithValue("@ProdCatID",cmbCategory.SelectedValue );
                cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));
                cmd.CommandType = CommandType.StoredProcedure;

                dbcon.OpenCon();
                int i = cmd.ExecuteNonQuery();
                dbcon.CloseCon();

                if (i > 0)
                {
                    MessageBox.Show("Product Data Inserted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtClear();
                    BindProductList();
                    lblProudID.Visible = false;
                }
                else
                {
                    MessageBox.Show("Product Insertion Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

      

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            lblProudID.Visible = false;
            BindProductList();
            BindCategory();
        }

        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbcon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DataSource= dt;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CatID";
            dbcon.CloseCon();
        }

        private void BindProductList()
        {
            SqlCommand cmd = new SqlCommand("select * from tblProduct ", dbcon.GetCon());
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
            lblProudID.Visible = false;
            btnAdd.Visible = false;

            lblProudID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtProdName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            cmbCategory.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtQty.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtClear();
        
        }
        private void txtClear()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            //cmbCategory.Items.Clear();
        }
    }
}
