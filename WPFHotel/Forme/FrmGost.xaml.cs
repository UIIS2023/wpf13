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
    public partial class FrmGost : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        public FrmGost()
        {
            InitializeComponent();
            txtIme.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmGost(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtIme.Focus();
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

                string vratiTeretane = @"SELECT teretanaID, lokacija FROM Teretana";
                SqlDataAdapter daTeretana = new SqlDataAdapter(vratiTeretane, konekcija);
                DataTable dtTeretana = new DataTable();
                daTeretana.Fill(dtTeretana);
                cbTeretana.ItemsSource = dtTeretana.DefaultView;
                daTeretana.Dispose();
                dtTeretana.Dispose();


                string vratiRestorane = @"SELECT restoranID, tipObroka, radnoVreme FROM Restoran";
                SqlDataAdapter daRestoran = new SqlDataAdapter(vratiRestorane, konekcija);
                DataTable dtRestoran = new DataTable();
                daRestoran.Fill(dtRestoran);
                cbRestoran.ItemsSource = dtRestoran.DefaultView;
                daRestoran.Dispose();
                dtRestoran.Dispose();

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
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@JMBG", SqlDbType.VarChar).Value = txtJMBG.Text;
                cmd.Parameters.Add("@kontakt", SqlDbType.VarChar).Value = txtKontakt.Text;
                cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                cmd.Parameters.Add("@grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add("@teretanaID", SqlDbType.Int).Value = cbTeretana.SelectedValue;
                cmd.Parameters.Add("@restoranID", SqlDbType.Int).Value = cbRestoran.SelectedValue;

                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"UPDATE Gost SET ime=@ime,prezime=@prezime,JMBG=@JMBG,
                                       kontakt=@kontakt,adresa=@adresa,grad=@grad,teretanaID=@teretanaID,restoranID=@restoranID
                                       WHERE gostID=@id";

                    red = null;
                }
                else
                {
                    cmd.CommandText = @"INSERT INTO Gost(ime,prezime,JMBG,kontakt,adresa,grad,teretanaID,restoranID)
                                    VALUES (@ime,@prezime,@JMBG,@kontakt,@adresa,@grad,@teretanaID,@restoranID)";
                }

                
                cmd.ExecuteNonQuery(); //ova metoda pokrece izvrsenje nase komande gore
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
