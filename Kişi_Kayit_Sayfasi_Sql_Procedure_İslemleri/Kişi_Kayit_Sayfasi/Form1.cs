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

namespace Kisi_Kayit_Sql_Procedure
{
    public partial class Form1 : Form
    {
        MySqlConnection Connect = new MySqlConnection();
        DataSet ds = new DataSet();
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            listBox6.Visible = false;
            comboJob.Text = "Meslek";
            comboGender.Text = "Cinsiyet";
            comboGender.Items.Add("ERKEK");
            comboGender.Items.Add("KADIN");         
            ComboJobList();
            PersonList();


        }

        #region ListBoxes selected index
        private void Indexer(int a)
        {
            listBox1.SelectedIndex = a;
            listBox2.SelectedIndex = a;
            listBox3.SelectedIndex = a;
            listBox4.SelectedIndex = a;
            listBox5.SelectedIndex = a;
            listBox6.SelectedIndex = a;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox1.SelectedIndex;
            Indexer(a);
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox2.SelectedIndex;
            Indexer(a);
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox3.SelectedIndex;
            Indexer(a);
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox4.SelectedIndex;
            Indexer(a);
        }
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox5.SelectedIndex;
            Indexer(a);
        }
        #endregion

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            txtName.Text = listBox2.Text;
            txtSurname.Text = listBox3.Text;
            comboJob.Text = listBox4.Text;
            comboGender.Text = listBox5.Text;
        }


        private void ComboJobList()
        {
            SqlCommand cmd = new SqlCommand("exec spJobList", Connect.Connection());
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                comboJob.Items.Add(read.GetString(0));
            }
            Connect.Connection().Close();
        }

        private int ComboJobIdFinder()
        {
            int index = 0;
            SqlCommand command1 = new SqlCommand("exec spFindJobId @p1", Connect.Connection());
            command1.Parameters.AddWithValue("@p1", comboJob.Text);
            SqlDataReader read = command1.ExecuteReader();
            while (read.Read())
            {
                index = int.Parse(read[0].ToString());
            }
            Connect.Connection().Close();
            return index;
        }

        private void PersonList()
        {
            Connect.Connection().Close();
            foreach (Control item in groupBox3.Controls)
            {
                if (item is ListBox)
                {
                    ListBox listBox = (ListBox)item;
                    listBox.Items.Clear();
                }
            }
            SqlCommand cmd = new SqlCommand("exec spPersonList", Connect.Connection());
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                listBox1.Items.Add(string.Format("{0}. ", listBox1.Items.Count + 1));
                listBox6.Items.Add(read[0]);
                listBox2.Items.Add(read[1]);
                listBox3.Items.Add(read[2]);
                listBox4.Items.Add(read[3]);
                listBox5.Items.Add(read[4]);

            }
        }
        private void ListBoxRemoveLastItem()
        {
            foreach (Control items in groupBox3.Controls)
            {
                if (items is ListBox)
                {
                    ListBox listBox = (ListBox)items;
                    listBox.Items.RemoveAt(listBox.Items.Count - 1);
                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/ilker-%C5%9Fenel-112363221/");
        }
        private void btnClean_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            comboJob.Text = "Meslek";
            comboGender.Text = "Cinsiyet";
        }




        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                int index = comboJob.Items.IndexOf(comboJob.Text);
                int index2 = comboGender.Items.IndexOf(comboGender.Text);


                if (txtName.Text != string.Empty && txtSurname.Text != string.Empty)
                {



                    if (index > -1 && index2 > -1)
                    {


                        SqlCommand command = new SqlCommand("exec spPersonAdd @name,@surname,@jobid,@gender",
                        Connect.Connection());
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@surname", txtSurname.Text);
                        command.Parameters.AddWithValue("@jobid", ComboJobIdFinder());
                        command.Parameters.AddWithValue("@gender", comboGender.Text);
                        command.ExecuteNonQuery();
                        Connect.Connection().Close();
                        MessageBox.Show("kişi kaydı eklendi");
                        PersonList();

                    }
                    else
                    {
                        MessageBox.Show("lütfen bir meslek veya cinsiyet seçimi yapınız");
                    }



                }
                else
                {
                    MessageBox.Show("LÜTFEN BÜTÜN ALANLARI DOLDURUNUZ", "!!! UYARI !!!");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("bir hata oluştu");
            }





        }





        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count == 0)
                {
                    MessageBox.Show("HİÇ KAYIT BULUNAMADI", "!!!UYARI!!!");
                }
                else
                {
                    if (listBox1.Text != "")
                    {
                        int a = listBox6.SelectedIndex;
                        a = int.Parse(listBox6.Items[a].ToString());
                        SqlCommand command = new SqlCommand("exec spDelete @Id", Connect.Connection());
                        command.Parameters.AddWithValue("@Id", a);
                        command.ExecuteNonQuery();
                        MessageBox.Show("kişi kaydı silindi");
                        PersonList();
                    }
                    else
                    {
                        MessageBox.Show("LÜTFEN ÖNCE SEÇİM YAPINIZ", "!!!Uyarı!!!");
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("bir hata oluştu");
            }


        }





        private void btnEdit_Click_1(object sender, EventArgs e)
        {

            try
            {
                int index = comboJob.Items.IndexOf(comboJob.Text);
                int index2 = comboGender.Items.IndexOf(comboGender.Text);
                if (listBox6.SelectedIndex > -1)
                {
                    if (txtName.Text != string.Empty && txtSurname.Text != string.Empty)
                    {
                        if (index > -1 && index2 > -1)
                        {
                            //listBox4.Items.IndexOf();
                            SqlCommand command = new SqlCommand("exec spPersonUpdate @Id,@name,@surname,@jobid,@gender",
                                Connect.Connection());
                            command.Parameters.AddWithValue("@Id", int.Parse(listBox6.Text));
                            command.Parameters.AddWithValue("@name", txtName.Text);
                            command.Parameters.AddWithValue("@surname", txtSurname.Text);
                            command.Parameters.AddWithValue("@jobid", ComboJobIdFinder());
                            command.Parameters.AddWithValue("@gender", comboGender.Text);
                            command.ExecuteNonQuery();
                            Connect.Connection().Close();
                            MessageBox.Show("kişi kaydı güncellendi");
                            PersonList();

                        }
                        else
                        {
                            MessageBox.Show("lütfen bir meslek veya cinsiyet seçimi yapınız");
                        }
                    }
                    else
                    {
                        MessageBox.Show("LÜTFEN BÜTÜN ALANLARI DOLDURUNUZ", "!!! UYARI !!!");
                    }
                }
                else
                {
                    MessageBox.Show("lütfen güncellenecek kullanıcıyı seçin");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("bir hata oluştu");
            }

        }

       
    }
}
