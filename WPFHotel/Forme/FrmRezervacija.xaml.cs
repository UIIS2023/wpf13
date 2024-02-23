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
    public partial class FrmRezervacija : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        public FrmRezervacija()
        {
            InitializeComponent();
            txtBrojGostiju.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmRezervacija(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtBrojGostiju.Focus();
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.red = red;
            PopuniPadajuceListe();
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiGoste = @"SELECT gostID, ime + ' ' + prezime AS Gost FROM Gost";
                SqlDataAdapter daGost = new SqlDataAdapter(vratiGoste, konekcija);
                DataTable dtGost = new DataTable();
                daGost.Fill(dtGost);
                cbGost.ItemsSource = dtGost.DefaultView;
                daGost.Dispose();
                dtGost.Dispose();


                string vratiSobe = @"SELECT sobaID, brojSobe AS Info, kapacitetSobe FROM Soba";
                SqlDataAdapter daSoba = new SqlDataAdapter(vratiSobe, konekcija);
                DataTable dtSoba = new DataTable();
                daSoba.Fill(dtSoba);
                cbSoba.ItemsSource = dtSoba.DefaultView;
                daSoba.Dispose();
                dtSoba.Dispose();

            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@statusRezervacije", SqlDbType.VarChar).Value = txtStatusRezervacije.Text;
                cmd.Parameters.Add("@brojGostiju", SqlDbType.Int).Value = txtBrojGostiju.Text;
                cmd.Parameters.Add("@datum", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@cena", SqlDbType.Int).Value = txtCena.Text;
                cmd.Parameters.Add("@sobaID", SqlDbType.Int).Value = cbSoba.SelectedValue;
                cmd.Parameters.Add("@gostID", SqlDbType.Int).Value = cbGost.SelectedValue;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"UPDATE Rezervacija SET statusRezervacije=@statusRezervacije,brojGostiju=@brojGostiju,datum=@datum,
                                       cena=@cena,sobaID=@sobaID,gostID=@gostID
                                       WHERE rezervacijaID=@id";

                    red = null;
                }
                else
                {
                    cmd.CommandText = @"INSERT INTO Rezervacija(statusRezervacije,brojGostiju,datum,cena,sobaID,gostID)
                                        VALUES (@statusRezervacije,@brojGostiju,@datum,@cena,@sobaID,@gostID)";
                }
                
                cmd.ExecuteNonQuery(); //ova metoda pokrece izvrsenje nase komande gore
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Doslo je do greske prilikom konverzija podataka", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
