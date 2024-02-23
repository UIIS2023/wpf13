using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WPFHotel.Forme
{
    public partial class FrmRestoran : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        public FrmRestoran()
        {
            InitializeComponent();
            txtKuhinja.Focus();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmRestoran(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtKuhinja.Focus();
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.red = red;
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@kuhinja", SqlDbType.NVarChar).Value = txtKuhinja.Text;
                cmd.Parameters.Add("@tipObroka", SqlDbType.NVarChar).Value = txtTipObroka.Text;
                cmd.Parameters.Add("@radnoVreme", SqlDbType.NVarChar).Value = txtRadnoVreme.Text;
                cmd.Parameters.Add("@lokacija", SqlDbType.NVarChar).Value = txtLokacija.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"UPDATE Restoran SET kuhinja=@kuhinja,tipObroka=@tipObroka,radnoVreme=@radnoVreme,
                                       lokacija=@lokacija 
                                       WHERE restoranID=@id";

                    red = null;
                }
                else
                {
                    cmd.CommandText = @"INSERT INTO Restoran(kuhinja,tipObroka,radnoVreme,lokacija)
                                    VALUES (@kuhinja,@tipObroka,@radnoVreme,@lokacija)";
                }

                cmd.ExecuteNonQuery(); //ova metoda pokrece izvrsenje nase komande gore
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
