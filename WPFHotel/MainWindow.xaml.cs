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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFHotel.Forme;


namespace WPFHotel
{
    public partial class MainWindow : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private string ucitanaTabela;
        private bool azuriraj;
        private DataRowView red;

        #region Select upiti

        private static string teretaneSelect = @"SELECT teretanaID AS ID, radnoVreme AS 'Radno Vreme', lokacija FROM Teretana";

        private static string sobeGostiSelect = @"SELECT sobaGostID AS ID,ime + ' ' + prezime AS Gost, brojSobe AS 'Broj Sobe',dostupnost,tipSobe AS 'Tip Sobe' FROM Soba_Gost
                                                JOIN Gost on Soba_Gost.gostID = Gost.gostID
                                                JOIN Soba on Soba_Gost.sobaID = Soba.sobaID";

        private static string sobeSelect = @"SELECT sobaID AS ID, brojSobe AS 'Broj Sobe', kapacitetSobe AS 'Kapacitet Sobe',
                                            dostupnost, kvadratura, tipSobe AS 'Tip Sobe', tip, cena, ime + ' ' + prezime AS Radnik, usluga FROM Soba
                                            JOIN Aranzman on Soba.aranzmanID = Aranzman.aranzmanID
                                            JOIN Radnik on Soba.radnikID = Radnik.radnikID";

        private static string rezervacijeSelect = @"SELECT rezervacijaID as ID, ime as 'Ime', prezime as 'Prezime', grad as 'Grad', brojSobe,
                                                    dostupnost, statusRezervacije as Status, brojGostiju, datum as Datum,
	                                                cena as 'Cena rezervacije' FROM Rezervacija 
                                                    JOIN Soba on Rezervacija.sobaID = Soba.sobaID
                                                    JOIN Gost on Rezervacija.gostID = Gost.gostID";

        private static string aranzmaniSelect = @"SELECT aranzmanID AS ID, tip AS Tip, cena FROM Aranzman";

        private static string restoraniSelect = @"SELECT restoranID AS ID, kuhinja AS Kuhinja, tipObroka AS 'Tip Obroka',
                                                radnoVreme AS 'Radno Vreme', lokacija FROM Restoran";

        private static string radniciSelect = @"SELECT radnikID AS ID, ime, prezime, JMBG, pozicija, kontakt, usluga, grad FROM Radnik";

        private static string gostiSelect = @"SELECT gostID AS ID, ime, prezime, JMBG, kontakt, adresa, grad, Teretana.lokacija AS 'Teretana Lokacija',
                                            kuhinja, tipObroka AS 'Tip Obroka', Restoran.lokacija as 'Restoran Lokacija' FROM Gost 
                                            JOIN Teretana on Gost.teretanaID = Teretana.teretanaID
                                            JOIN Restoran on Gost.restoranID = Restoran.restoranID";
        #endregion

        #region Select sa uslovom

        private static string selectUslovTeretane = @"SELECT * FROM Teretana WHERE teretanaID=";

        private static string selectUslovSobeGosti = @"SELECT * FROM Soba_Gost WHERE sobaGostID=";

        private static string selectUslovSobe = @"SELECT * FROM Soba WHERE sobaID=";

        private static string selectUslovRezervacije = @"SELECT * FROM Rezervacija WHERE rezervacijaID=";

        private static string selectUslovAranzmani = @"SELECT * FROM Aranzman WHERE aranzmanID=";

        private static string selectUslovRestorani = @"SELECT * FROM Restoran WHERE restoranID=";

        private static string selectUslovRadnici = @"SELECT * FROM Radnik WHERE radnikID=";

        private static string selectUslovGosti = @"SELECT * FROM Gost WHERE gostID=";

        #endregion

        #region Delete naredbe

        private static string teretaneDelete = @"DELETE FROM Teretana WHERE teretanaID=";

        private static string sobeGostiDelete = @"DELETE FROM Soba_Gost WHERE sobaGostID=";

        private static string sobeDelete = @"DELETE FROM Soba WHERE sobaID=";

        private static string rezervacijeDelete = @"DELETE FROM Rezervacija WHERE rezervacijaID=";

        private static string aranzmaniDelete = @"DELETE FROM Aranzman WHERE aranzmanID=";

        private static string restoraniDelete = @"DELETE FROM Restoran WHERE restoranID=";

        private static string radniciDelete = @"DELETE FROM Radnik WHERE radnikID=";

        private static string gostiDelete = @"DELETE FROM Gost WHERE gostID=";



        #endregion

        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(sobeSelect);
        }

        private void UcitajPodatke(string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataGridCentralni != null)
                {
                    dataGridCentralni.ItemsSource = dataTable.DefaultView;
                }

                ucitanaTabela = selectUpit;
                dataAdapter.Dispose();
                dataTable.Dispose();

            }
            catch (SqlException)
            {
                MessageBox.Show("Nastala je greska pri ucitavanju tabele", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(teretaneSelect))
            {
                prozor = new FrmTeretana();
                prozor.ShowDialog();
                UcitajPodatke(teretaneSelect);

            }
            else if (ucitanaTabela.Equals(sobeGostiSelect))
            {
                prozor = new FrmSobaGost();
                prozor.ShowDialog();
                UcitajPodatke(sobeGostiSelect);
            }
            else if (ucitanaTabela.Equals(sobeSelect))
            {
                prozor = new FrmSoba();
                prozor.ShowDialog();
                UcitajPodatke(sobeSelect);
            }
            else if (ucitanaTabela.Equals(rezervacijeSelect))
            {
                prozor = new FrmRezervacija();
                prozor.ShowDialog();
                UcitajPodatke(rezervacijeSelect);
            }
            else if (ucitanaTabela.Equals(aranzmaniSelect))
            {
                prozor = new FrmAranzman();
                prozor.ShowDialog();
                UcitajPodatke(aranzmaniSelect);
            }
            else if (ucitanaTabela.Equals(restoraniSelect))
            {
                prozor = new FrmRestoran();
                prozor.ShowDialog();
                UcitajPodatke(restoraniSelect);
            }
            else if (ucitanaTabela.Equals(radniciSelect))
            {
                prozor = new FrmRadnik();
                prozor.ShowDialog();
                UcitajPodatke(radniciSelect);
            }
            else if (ucitanaTabela.Equals(gostiSelect))
            {
                prozor = new FrmGost();
                prozor.ShowDialog();
                UcitajPodatke(gostiSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(teretaneSelect))
            {
                PopuniFormu(selectUslovTeretane);
                UcitajPodatke(teretaneSelect);

            }
            else if (ucitanaTabela.Equals(sobeGostiSelect))
            {
                PopuniFormu(selectUslovSobeGosti);
                UcitajPodatke(sobeGostiSelect);
            }
            else if (ucitanaTabela.Equals(sobeSelect))
            {
                PopuniFormu(selectUslovSobe);
                UcitajPodatke(sobeSelect);
            }
            else if (ucitanaTabela.Equals(rezervacijeSelect))
            {
                PopuniFormu(selectUslovRezervacije);
                UcitajPodatke(rezervacijeSelect);
            }
            else if (ucitanaTabela.Equals(aranzmaniSelect))
            {
                PopuniFormu(selectUslovAranzmani);
                UcitajPodatke(aranzmaniSelect);
            }
            else if (ucitanaTabela.Equals(restoraniSelect))
            {
                PopuniFormu(selectUslovRestorani);
                UcitajPodatke(restoraniSelect);
            }
            else if (ucitanaTabela.Equals(radniciSelect))
            {
                PopuniFormu(selectUslovRadnici);
                UcitajPodatke(radniciSelect);
            }
            else if (ucitanaTabela.Equals(gostiSelect))
            {
                PopuniFormu(selectUslovGosti);
                UcitajPodatke(gostiSelect);
            }
        }

        private void PopuniFormu(object selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                red = (DataRowView)dataGridCentralni.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(teretaneSelect))
                    {
                        FrmTeretana prozorTeretana = new FrmTeretana(azuriraj, red);
                        prozorTeretana.txtLokacija.Text = citac["lokacija"].ToString();
                        prozorTeretana.txtRadnoVreme.Text = citac["radnoVreme"].ToString();
                        prozorTeretana.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(sobeGostiSelect))
                    {
                        FrmSobaGost prozorSobaGost = new FrmSobaGost(azuriraj, red);
                        prozorSobaGost.cbGost.SelectedValue = citac["gostID"].ToString();
                        prozorSobaGost.cbSoba.SelectedValue = citac["sobaID"].ToString();
                        prozorSobaGost.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(rezervacijeSelect))
                    {
                        FrmRezervacija prozorRezervazija = new FrmRezervacija(azuriraj, red);
                        prozorRezervazija.txtBrojGostiju.Text = citac["brojGostiju"].ToString();
                        prozorRezervazija.txtCena.Text = citac["cena"].ToString();
                        prozorRezervazija.txtStatusRezervacije.Text = citac["statusRezervacije"].ToString();
                        prozorRezervazija.dpDatum.SelectedDate = (DateTime)citac["datum"];
                        prozorRezervazija.cbGost.SelectedValue = citac["gostID"].ToString();
                        prozorRezervazija.cbSoba.SelectedValue = citac["sobaID"].ToString();
                        prozorRezervazija.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(sobeSelect))
                    {
                        FrmSoba prozorSoba = new FrmSoba(azuriraj, red);
                        prozorSoba.txtBrojSobe.Text = citac["brojSobe"].ToString();
                        prozorSoba.txtKapacitetSobe.Text = citac["kapacitetSobe"].ToString();
                        prozorSoba.txtKvadratura.Text = citac["kvadratura"].ToString();
                        prozorSoba.txtTipSobe.Text = citac["tipSobe"].ToString();
                        prozorSoba.cbAranzman.SelectedValue = citac["aranzmanID"].ToString();
                        prozorSoba.cbRadnik.SelectedValue = citac["radnikID"].ToString();
                        prozorSoba.cbxDostupnost.IsChecked = (bool)citac["dostupnost"];
                        prozorSoba.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(aranzmaniSelect))
                    {
                        FrmAranzman prozorAranzman = new FrmAranzman(azuriraj, red);
                        prozorAranzman.txtTip.Text = citac["tip"].ToString();
                        prozorAranzman.txtCena.Text = citac["cena"].ToString();;
                        prozorAranzman.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(restoraniSelect))
                    {
                        FrmRestoran prozorRestoran = new FrmRestoran(azuriraj, red);
                        prozorRestoran.txtKuhinja.Text = citac["kuhinja"].ToString();
                        prozorRestoran.txtLokacija.Text = citac["tipObroka"].ToString();
                        prozorRestoran.txtRadnoVreme.Text = citac["radnoVreme"].ToString();
                        prozorRestoran.txtTipObroka.Text = citac["lokacija"].ToString();
                        prozorRestoran.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(radniciSelect))
                    {
                        FrmRadnik prozorRadnik = new FrmRadnik(azuriraj, red);
                        prozorRadnik.txtGrad.Text = citac["grad"].ToString();
                        prozorRadnik.txtIme.Text = citac["ime"].ToString();
                        prozorRadnik.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorRadnik.txtKontakt.Text = citac["kontakt"].ToString();
                        prozorRadnik.txtPozicija.Text = citac["pozicija"].ToString();
                        prozorRadnik.txtPrezime.Text = citac["prezime"].ToString();
                        prozorRadnik.txtUsluga.Text = citac["usluga"].ToString();
                        prozorRadnik.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(gostiSelect))
                    {
                        FrmGost prozorGost = new FrmGost(azuriraj, red);
                        prozorGost.txtAdresa.Text = citac["adresa"].ToString();
                        prozorGost.txtGrad.Text = citac["grad"].ToString();
                        prozorGost.txtIme.Text = citac["ime"].ToString();
                        prozorGost.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorGost.txtKontakt.Text = citac["kontakt"].ToString();
                        prozorGost.txtPrezime.Text = citac["prezime"].ToString();
                        prozorGost.cbRestoran.SelectedValue = citac["restoranID"].ToString();
                        prozorGost.cbTeretana.SelectedValue = citac["teretanaID"].ToString();
                        prozorGost.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(teretaneSelect))
            {
                ObrisiZapis(teretaneDelete);
                UcitajPodatke(teretaneSelect);

            }
            else if (ucitanaTabela.Equals(sobeGostiSelect))
            {
                ObrisiZapis(sobeGostiDelete);
                UcitajPodatke(sobeGostiSelect);
            }
            else if (ucitanaTabela.Equals(sobeSelect))
            {
                ObrisiZapis(sobeDelete);
                UcitajPodatke(sobeSelect);
            }
            else if (ucitanaTabela.Equals(rezervacijeSelect))
            {
                ObrisiZapis(rezervacijeDelete);
                UcitajPodatke(rezervacijeSelect);
            }
            else if (ucitanaTabela.Equals(aranzmaniSelect))
            {
                ObrisiZapis(aranzmaniDelete);
                UcitajPodatke(aranzmaniSelect);
            }
            else if (ucitanaTabela.Equals(restoraniSelect))
            {
                ObrisiZapis(restoraniDelete);
                UcitajPodatke(restoraniSelect);
            }
            else if (ucitanaTabela.Equals(radniciSelect))
            {
                ObrisiZapis(radniciDelete);
                UcitajPodatke(radniciSelect);
            }
            else if (ucitanaTabela.Equals(gostiSelect))
            {
                ObrisiZapis(gostiDelete);
                UcitajPodatke(gostiSelect);
            }
        }

        private void ObrisiZapis(string deleteUslov)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)dataGridCentralni.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUslov + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red za brisanje!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnAranzmani_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(aranzmaniSelect);
        }

        private void btnGosti_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(gostiSelect);

        }

        private void btnRadnici_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(radniciSelect);

        }

        private void btnRestorani_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(restoraniSelect);

        }

        private void btnRezervacije_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(rezervacijeSelect);

        }

        private void btnSobe_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(sobeSelect);

        }

        private void btnSobeGosti_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(sobeGostiSelect);

        }

        private void btnTeretane_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(teretaneSelect);
            
        }
    }
}
