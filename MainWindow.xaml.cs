using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace RegistrSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = Registration; Trusted_connection=True";
        public string str = "INSERT INTO Reg(Names, Mail, Nikname, Pass) ";
        string str2;
        string sqlExpression = "SELECT * FROM Reg";
        public int kol=0;
        public MainWindow()
        {
            InitializeComponent();
        }
        async void Regi()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                str2 = str + "VALUES('" + NamesH.Text + "', '" + EmailH.Text + "', '" + UsernameH.Text + "', '" + PassH.Password.ToString() + "')";
                MessageBox.Show(str2);
                //открываем подклчение
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(str2, connection);

                int num = await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Добавлено объектов {num}");
            }
            Console.Read();
            str2 = "";
        }
        async void Proverka()
        {
            if (NamesH.Text == "" || EmailH.Text == "" || UsernameH.Text == "" || PassH.Password.ToString() == "")
            {

                MessageBox.Show("Поля не могут быть пустыми");
                return;
            }
            kol = 0;
            using (SqlConnection connection = new SqlConnection(connect))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        string s1 = reader.GetName(3);
                        string s2 = reader.GetName(2);

                        while (await reader.ReadAsync())
                        {
                            object nik = reader.GetValue(3);
                            object pass = reader.GetValue(2);
                            if (UsernameH.Text==nik.ToString())
                            {
                                kol++;
                                MessageBox.Show("Этот ник занят. Попробуйте другой.");
                            }
                            if (EmailH.Text == pass.ToString())
                            {
                                kol++;
                                MessageBox.Show("Этот Mail занят. Попробуйте другой.");
                            }

                        }
                        if(kol>0)
                        {
                            return;
                        }
                        else if(kol==0)
                        {
                            Regi();
                        }
                        
                        await reader.CloseAsync();
                    }

                }
                catch (Exception e) { Console.WriteLine(e.Message); };

            }
        }
        private void Butt_Click(object sender, RoutedEventArgs e)
        {


            for (int i = 0; i < NamesH.Text.Length; i++)
            {
                if (NamesH.Text[i] == ' ')
                {
                    MessageBox.Show("No Space in Names");
                    NamesH.Text = "";
                    break;
                }
            }
            for (int i = 0; i < EmailH.Text.Length; i++)
            {
                if (EmailH.Text[i] == ' ')
                {
                    MessageBox.Show("No Space in Email");
                    EmailH.Text = "";
                    break;
                }
            }

            for (int i = 0; i < UsernameH.Text.Length; i++)
            {
                if (UsernameH.Text[i] == ' ')
                {
                    MessageBox.Show("No Space in Nikname");
                    UsernameH.Text = "";
                    break;
                }
            }
            for (int i = 0; i < PassH.Password.ToString().Length; i++)
            {
                if (PassH.Password[i] == ' ')
                {
                    MessageBox.Show("No Space in Password");
                    PassH.Password = "";
                    break;
                }
            }
            Proverka();
            
            
        }
    }
}
