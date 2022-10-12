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
    public partial class customerForm : Form
    {

        OracleConnection con = new OracleConnection("Data Source=orcl;User Id=SCOTT;password=tiger;");

        OracleCommand cmd = new OracleCommand();
        public customerForm()
        {
            InitializeComponent();
        }
        Int32 ID;
        public customerForm(Int32 id)
        {
            ID = id;
            InitializeComponent();
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            string constr = "Data Source=orcl;User Id=SCOTT;password=tiger;";
            OracleDataAdapter cmdstr = new OracleDataAdapter("select * from menuss ", constr);
            DataSet dt = new DataSet();
            cmdstr.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            
            OracleDataAdapter d = new OracleDataAdapter("delete from cartss where phone_number=:phone ", con);
            d.SelectCommand.Parameters.Add("phone", textBox12.Text);
            d.SelectCommand.ExecuteNonQuery();

            cmd.CommandText = "insert into cartss (phone_number,meal1,meal2,meal3,address,cost1,state,custid) values(:phone_number,:meal1,:meal2,:meal3,:address,:cost,'wait',:custID) ";
            cmd.Parameters.Add("phone_number", textBox12.Text);
            cmd.Parameters.Add("meal1", textBox2.Text);//meal1
            cmd.Parameters.Add("meal2", textBox5.Text);
            cmd.Parameters.Add("meal3", textBox3.Text);
            cmd.Parameters.Add("address", textBox1.Text);//meal1
            cmd.Parameters.Add("cost", textBox10.Text);
            cmd.Parameters.Add("custID", ID);


            int r = cmd.ExecuteNonQuery();
            if (r != -1)
            {
                MessageBox.Show("added");
            }
        }
        int total = 0;


        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //       curreent =                                       meal 1
            if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == dataGridView1.Rows[0].Cells[1].Value.ToString())
            {
                total += Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value.ToString());
                textBox2.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox4.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox9.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();


            }
            //       curreent =                                       meal 2
            if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == dataGridView1.Rows[1].Cells[1].Value.ToString())
            {
                total += Convert.ToInt32(dataGridView1.Rows[1].Cells[2].Value.ToString());
                textBox5.Text = dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox8.Text = dataGridView1.Rows[1].Cells[0].Value.ToString();
                textBox7.Text = dataGridView1.Rows[1].Cells[2].Value.ToString();

            }
            //       curreent=                                       meal 3
            if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == dataGridView1.Rows[2].Cells[1].Value.ToString())
            {
                total += Convert.ToInt32(dataGridView1.Rows[2].Cells[2].Value.ToString());
                textBox3.Text = dataGridView1.Rows[2].Cells[1].Value.ToString();
                textBox11.Text = dataGridView1.Rows[2].Cells[0].Value.ToString();
                textBox6.Text = dataGridView1.Rows[2].Cells[2].Value.ToString();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            textBox10.Text = total.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm main = new LoginForm();
            main.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
