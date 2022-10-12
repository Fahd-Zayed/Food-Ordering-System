using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace food_ordering
{
    public partial class deliveryForm : Form
    {
        OracleDataAdapter adapter;
        DataSet ds;
        string constr = "Data Source=orcl;User Id=SCOTT;password=tiger;";
        string cmdstr = "select * from cartss where state = 'wait' OR state = 'Going'";
        public deliveryForm()
        {
            InitializeComponent();
        }


        string t = "";
        public deliveryForm(string username)
        {
            t = username;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            adapter = new OracleDataAdapter(cmdstr, constr);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns.RemoveAt(7);
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Select a Row");

            else
            {
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                if (dataGridView1.SelectedCells[6].Value.ToString() != "Done")
                {
                    MessageBox.Show("State must be Done");
                }
                else
                {
                    txtPhoneNumber.Text = dataGridView1.SelectedCells[0].Value.ToString();
                    txtCost.Text = dataGridView1.SelectedCells[4].Value.ToString();
                    txtState.Text = dataGridView1.SelectedCells[6].Value.ToString();
                    adapter.Update(ds.Tables[0]);
                    OracleDataAdapter ad = new OracleDataAdapter("update delivery set phone_cust = :phone where username = :t", constr);
                    ad.SelectCommand.Parameters.Add("phone", txtPhoneNumber.Text);
                    ad.SelectCommand.Parameters.Add("t", t.ToString());
                    ad.SelectCommand.Connection.Open();
                    ad.SelectCommand.ExecuteNonQuery();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Select a Row");
            else
            {
                OracleCommandBuilder b = new OracleCommandBuilder(adapter);
                if (dataGridView1.SelectedCells[6].Value.ToString() != "Going")
                {
                    MessageBox.Show("State must be Going");
                }
                else
                {
                    txtPhoneNumber.Text = dataGridView1.SelectedCells[0].Value.ToString();
                    txtCost.Text = dataGridView1.SelectedCells[4].Value.ToString();
                    txtState.Text = dataGridView1.SelectedCells[6].Value.ToString();
                    adapter.Update(ds.Tables[0]);
                    OracleDataAdapter ad = new OracleDataAdapter("update delivery set phone_cust = :phone where username=:t", constr);
                    ad.SelectCommand.Parameters.Add("phone", txtPhoneNumber.Text);
                    ad.SelectCommand.Parameters.Add("t", t.ToString());
                    ad.SelectCommand.Connection.Open();
                    ad.SelectCommand.ExecuteNonQuery();

                }
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm main = new LoginForm();
            main.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void deliveryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
