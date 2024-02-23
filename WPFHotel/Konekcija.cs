using System;
using System.Collections.Generic;
using System.Data.SqlClient;  //namespace koji je .NET Data Provider za SQL Server i koji nam omogucava da pristupamao podacima iz baza podataka
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHotel
{
    public class Konekcija
    {
        //predstavlja konekciju sa SQL Server bazom
        public SqlConnection KreirajKonekciju()
        {
            //pruza jednostavan nacin za kreiranje i upravljanje sadrzajem konekcionog stringa
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-09SAJI2\SQLEXPRESS", //naziv lokalnog servera Vašeg računara
                InitialCatalog = "Hotel", //Baza na lokalnom serveru
                IntegratedSecurity = true //koristice se trenutni windows kredencijali za autentifikaciju, u slucaju da je false potrebno bi bilo u okviru konekcionog stringa navesti User ID i password
            };
            string con = ccnSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}
