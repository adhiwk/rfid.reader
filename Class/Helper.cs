using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Rfid.Reader.Class
{
    public class Helper
    {
        //public static string HitungUmur(DateTime dTglLahir)
        //{
        //    DateTime dtNow = DateTime.Now;
        //    DateTime dob = dTglLahir;
        //    //TimeSpan today = dtNow.Subtract(dob);
        //    int years, months, days;
        //    months = 12 * (dtNow.Year - dob.Year) + (dtNow.Month - dob.Month);

        //    if (dtNow.Day < dob.Day)
        //    {
        //        months -= 1;
        //        days = DateTime.DaysInMonth(dob.Year, dob.Month) - dob.Day + dtNow.Day;
        //    }
        //    else
        //    {
        //        days = dtNow.Day - dob.Day;
        //    }
        //    years = (int)Math.Floor(months / 12.0);
        //    months -= years * 12;
        //    return $"{years} thn {months} bln {days} hari";
        //}
        public static bool ValidateField(string fieldValue, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                MessageBox.Show(errorMessage, "Error");
                return false;
            }
            return true;
        }
        public static DateTime GetDateTime()
        {
            DateTime dtTglJam = DateTime.Now;
            try
            {
                using (var mySqlConnection = Databases.OpenMysqlDatabase())
                {
                    mySqlConnection.Open();
                    using (var mySqlCommand = new MySqlCommand("SELECT NOW() as TGL_JAM", mySqlConnection))
                    {
                        var result = mySqlCommand.ExecuteScalar();
                        if (result != null)
                        {
                            dtTglJam = DateTime.ParseExact(result.ToString(), "yyyy-MM-dd HH:mm:ss", null);
                        }
                    }
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dtTglJam;
        }

        public static DateTime GetDateTimeUtc()
        {
            DateTime dtTglJam = DateTime.UtcNow; // Default ke UTC Now

            try
            {
                using (var mySqlConnection = Databases.OpenMysqlDatabase())
                {
                    mySqlConnection.Open();

                    using (var mySqlCommand = new MySqlCommand("SELECT NOW() as TGL_JAM", mySqlConnection))
                    {
                        var result = mySqlCommand.ExecuteScalar(); // Ambil hasil pertama dari query
                        if (result != null)
                        {
                            dtTglJam = DateTime.Parse(result.ToString()).ToUniversalTime(); // Parsing dan konversi ke UTC
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Menangani error
            }

            return dtTglJam; // Kembalikan waktu dalam UTC
        }


        public static TimeSpan GetTime()
        {
            TimeSpan jam = TimeSpan.Zero;
            MySqlConnection mySqlConnection = Databases.OpenMysqlDatabase();

            try
            {
                mySqlConnection.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandText = "SELECT CURRENT_TIME() as JAM";

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (mySqlDataReader.Read())
                        {
                            // Mengambil waktu sebagai TimeSpan
                            jam = TimeSpan.Parse(mySqlDataReader["JAM"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            mySqlConnection.Close();
            return jam;
        }

        public static (int umur, string status_umur) StatusUmur(DateTime tanggalLahir, DateTime tanggalSekarang)
        {
            // Hitung perbedaan antara tanggal lahir dan tanggal sekarang
            TimeSpan selisih = tanggalSekarang - tanggalLahir;

            if (selisih.Days >= 365)
            {
                int umurTahun = selisih.Days / 365;
                return (umurTahun, "Th");
            }
            else if (selisih.Days >= 30)
            {
                int umurBulan = selisih.Days / 30;
                return (umurBulan, "Bl");
            }
            else
            {
                return (selisih.Days, "Hr");
            }
        }

        public static string TrimTo50Characters(string input)
        {
            if (input.Length > 50)
            {
                return input.Substring(0, 50);
            }
            return input;
        }

        public static string DateToString(DevExpress.XtraEditors.DateEdit dtControl)
        {
            return dtControl.DateTime.ToString("yyyy-MM-dd");
        }

        //refactor hitung umur
        public static string HitungUmur(DateTime tglLahir)
        {
            DateTime today = DateTime.Today;
            int years = CalculateYears(today, tglLahir);
            int months = CalculateMonths(today, tglLahir);
            int days = CalculateDays(today, tglLahir);

            return $"{years} thn {months} bln {days} hari";
        }

        private static int CalculateYears(DateTime today, DateTime tglLahir)
        {
            int years = today.Year - tglLahir.Year;
            if (today < tglLahir.AddYears(years))
            {
                years--;
            }
            return years;
        }

        private static int CalculateMonths(DateTime today, DateTime tglLahir)
        {
            int months = today.Month - tglLahir.Month;
            if (today.Day < tglLahir.Day)
            {
                months--;
            }
            if (months < 0)
            {
                months += 12;
            }
            return months;
        }

        private static int CalculateDays(DateTime today, DateTime tglLahir)
        {
            int days;
            if (today.Day >= tglLahir.Day)
            {
                days = today.Day - tglLahir.Day;
            }
            else
            {
                var lastMonth = today.AddMonths(-1);
                days = (DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month) - tglLahir.Day) + today.Day;
            }
            return days;
        }

        public static void GridRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Belum")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#A8DF8E");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Sudah")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = Color.LightGray;
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Batal")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#FFBFBF");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Berkas Diterima")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#BEADFA");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Dirujuk")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#96B6C5");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Meninggal")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#EEE0C9");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Dirawat")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#6E85B7");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["STATUS"].Value == "Pulang Paksa")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#FF87B2");
                //e.RowElement.ForeColor = Color.Black;
            }
            else
            {
                e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            }
        }

        public static void GridPasienRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if ((string)e.RowElement.RowInfo.Cells["JENIS"].Value == "UMU")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#BFECFF");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["JENIS"].Value == "ASR")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#CDC1FF");
                //e.RowElement.ForeColor = Color.Black;
            }
            else if ((string)e.RowElement.RowInfo.Cells["JENIS"].Value == "BPJ")
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = ColorTranslator.FromHtml("#FFF6E3");
                //e.RowElement.ForeColor = Color.Black;
            }
        }
    }
}
