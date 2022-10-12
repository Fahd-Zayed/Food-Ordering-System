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
    public partial class LoginForm : Form
    {
        string ordb = "Data Source=orcl;User Id=SCOTT;password=tiger;";
        OracleConnection conn;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
        }

     
        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '*';
        }


        private void button3_Click_1(object sender, EventArgs e)
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            
            if (txtName.Text == "")
                MessageBox.Show("User Name is required");
            else if (txtPass.Text == "")
                MessageBox.Show("Password is required");
            else if (custReg.Checked == false && delivReg.Checked == false)
                MessageBox.Show("You must choose account type");
            

            else
            {
                if (custReg.Checked)
                {
                    //multible row 
                    //check from table customer
                    //if the email is already exist then display msg please log in instead of sign up
                    cmd.CommandText = "FINDEMAIL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", txtName.Text);
                    cmd.Parameters.Add("emails", OracleDbType.RefCursor, ParameterDirection.Output);

                    OracleDataReader dr = cmd.ExecuteReader();// carry multible rows
                    bool flag = false;

                    while (dr.Read())
                    {
                        if (txtName.Text == dr[0].ToString())//check by user name only
                        {
                            flag = true;//if found on data base
                            MessageBox.Show("Email already exists ,Try to log in");
                        }
                    }

                    if (flag == false)//will register succsesfully
                    {
                        OracleDataAdapter maxCust = new OracleDataAdapter("select max (customer_id) from customerss",conn);
                        int id = Convert.ToInt32(maxCust.SelectCommand.ExecuteScalar());
                        id++;

                        OracleCommand c = new OracleCommand();
                        c.Connection = conn;
                        c.CommandText = "insert into customerss values (:id,:username,:password)";
                        c.Parameters.Add("id", id.ToString());
                        c.Parameters.Add("username", txtName.Text);
                        c.Parameters.Add("password", txtPass.Text);
                        
                        int re = c.ExecuteNonQuery();
                        if (re != -1)
                            MessageBox.Show("New Custmoer is Added");
                    }

                }
                
                else if (delivReg.Checked)
                {

                    cmd.CommandText = "FINDEMAIL2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", txtName.Text);
                    cmd.Parameters.Add("password", txtPass.Text);
                    cmd.Parameters.Add("emails", OracleDbType.RefCursor, ParameterDirection.Output);

                    OracleDataReader dr = cmd.ExecuteReader();
                    bool flag = false;

                    while (dr.Read())
                    {
                        if (txtName.Text == dr[0].ToString() && txtPass.Text == dr[1].ToString())
                        {
                            flag = true;
                            MessageBox.Show("Email already exists ,Try to log in");
                            break;

                        }

                    }
                    if (flag == false)
                    {
                        OracleDataAdapter maxCust = new OracleDataAdapter("select max(customer_id) from customerss", conn);
                        int id = Convert.ToInt32(maxCust.SelectCommand.ExecuteScalar());
                        id++;
                        OracleCommand c = new OracleCommand();
                        c.Connection = conn;
                        c.CommandText = "insert into delivery values (:id,'',:username,:password)";
                        c.Parameters.Add("id", id.ToString());
                        c.Parameters.Add("username", txtName.Text);
                        c.Parameters.Add("password", txtPass.Text);
                        int re = c.ExecuteNonQuery();
                        if (re != -1)
                            MessageBox.Show("New Delivery Pesron is Added");

                    }



                }
                

            }
            
        }

        private void button2_Click(object sender, EventArgs e) //login
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            if (textBox2.Text == "")
                MessageBox.Show("UserName is required");
            else if (textBox4.Text == "")
                MessageBox.Show("Password is required");
            else if (custLog.Checked == false && delivLog.Checked == false)
                MessageBox.Show("You must choose account type");
            
            
            else
            {
                // 1 user mawgod 0 yb2a msh mwgod
                int found = 0;
                if (custLog.Checked)
                {
                    cmd.CommandText = "CHECK_CUST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("P_user_Name1", textBox2.Text.ToString());
                    cmd.Parameters.Add("P_password1", textBox4.Text.ToString());
                    cmd.Parameters.Add("FOUND_CUST", OracleDbType.Int32, ParameterDirection.Output);
                    cmd.ExecuteNonQuery();
                    found = Convert.ToInt32(cmd.Parameters["FOUND_CUST"].Value.ToString());
                }
                else if (delivLog.Checked)
                {
                    cmd.CommandText = "CHECK_DEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("P_user_Name2", textBox2.Text.ToString());
                    cmd.Parameters.Add("P_password2", textBox4.Text.ToString());
                    cmd.Parameters.Add("FOUND_DEL", OracleDbType.Int32, ParameterDirection.Output);
                    cmd.ExecuteNonQuery();
                    found = Convert.ToInt32(cmd.Parameters["FOUND_DEL"].Value.ToString());
                }
                if (found > 0)
                {
                    if (custLog.Checked)
                    {
                        OracleDataAdapter d = new OracleDataAdapter("Select customer_id from customerss where user_name = :name", conn);
                        d.SelectCommand.Parameters.Add("name", textBox2.Text);
                        int id = Convert.ToInt32( d.SelectCommand.ExecuteScalar());//return cell
                      
                        customerForm f = new customerForm(id);
                        this.Hide();
                        f.Show();

                    }
                    if (delivLog.Checked)
                    {
                        deliveryForm f2 = new deliveryForm(textBox2.Text.ToString());
                        this.Hide();
                        f2.Show();
                    }
                }
                else
                    MessageBox.Show("Wrong username or Password,Try again");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Form4 f4 = new Form4();
            //this.Hide();
            //f4.Show();
        }

    }
}
