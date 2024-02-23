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
    public partial class FrmSoba : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        public FrmSoba()
        {
            InitializeComponent();
            txtBrojSobe.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmSoba(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtBrojSobe.Focus();
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

                string vratiAranzmane = @"SELECT aranzmanID, Tip FROM Aranzman";
                SqlDataAdapter daAranzman = new SqlDataAdapter(vratiAranzmane, konekcija);
                DataTable dtAranzman = new DataTable();
                daAranzman.Fill(dtAranzman);
                cbAranzman.ItemsSource = dtAranzman.DefaultView;
                daAranzman.Dispose();
                dtAranzman.Dispose();


                string vratiRadnike = @"SELECT radnikID, ime + ' ' + prezime AS Radnik FROM Radnik";
                SqlDataAdapter daRadnik = new SqlDataAdapter(vratiRadnike, konekcija);
                DataTable dtRadnik = new DataTable();
                daRadnik.Fill(dtRadnik);
                cbRadnik.ItemsSource = dtRadnik.DefaultView;
                daRadnik.Dispose();
                dtRadnik.Dispose();

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

                cmd.Parameters.Add("@brojSobe", SqlDbType.Int).Value = txtBrojSobe.Text;
                cmd.Parameters.Add("@kapacitetSobe", SqlDbType.Int).Value = txtKapacitetSobe.Text;
                cmd.Parameters.Add("@dostupnost", SqlDbType.Bit).Value = Convert.ToInt32(cbxDostupnost.IsChecked);
                cmd.Parameters.Add("@kvadratura", SqlDbType.Int).Value = txtKvadratura.Text;
                cmd.Parameters.Add("@tipSobe", SqlDbType.NVarChar).Value = txtTipSobe.Text;
                cmd.Parameters.Add("@aranzmanID", SqlDbType.Int).Value = cbAranzman.SelectedValue;
                cmd.Parameters.Add("@radnikID", SqlDbType.Int).Value = cbRadnik.SelectedValue;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"UPDATE Soba SET brojSobe=@brojSobe,kapacitetSobe=@kapacitetSobe,dostupnost=@dostupnost,
                                       kvadratura=@kvadratura,tipSobe=@tipSobe,aranzmanID=@aranzmanID,radnikID=@radnikID
                                       WHERE sobaID=@id";

                    red = null;
                }
                else
                {
                    cmd.CommandText = @"INSERT INTO Soba(brojSobe,kapacitetSobe,dostupnost,kvadratura,tipSobe,aranzmanID,radnikID)
                                    VALUES (@brojSobe,@kapacitetSobe,@dostupnost,@kvadratura,@tipSobe,@aranzmanID,@radnikID)";
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
