using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Rfid.Reader.Class;

namespace Rfid.Reader.Forms
{
    public partial class Dashboard : Form
    {
        private bool btnToggle;
        private Timer timer;
        private bool isTimerRunning = false;

        private Stopwatch stopwatch = new Stopwatch();
        private TimeSpan waktuTotal;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnStart.ImageOptions.Image = Image.FromFile(Application.StartupPath + "\\Icon\\play-button.png");
            DefaultSetting();
            lblCountdown.Text = "00:00:00.000";
        }
        private void DefaultSetting()
        {
            //btnStart.Enabled = true;
            btnStart.ImageOptions.Image = Image.FromFile(Application.StartupPath + "\\Icon\\play-button.png");
            btnStop.Enabled = false;
            btnReset.Enabled = false;
            btnSave.Enabled = false;
            btnToggle = true;
            lblCountdown.ForeColor = Color.Black;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnToggle)
            {
                if (!isTimerRunning) // Cek jika timer belum berjalan
                {
                    isTimerRunning = true;
                    btnStart.ImageOptions.Image = Image.FromFile(Application.StartupPath + "\\Icon\\play-button-on.png");
                    btnStop.Enabled = true;
                    btnSave.Enabled = true;

                    stopwatch.Reset(); // Reset stopwatch
                    waktuTotal = TimeSpan.FromMinutes(2); // Hitung mundur dari 2 menit
                    stopwatch.Start(); // Mulai stopwatch

                    // Timer untuk update UI tiap detik
                    timer = new Timer { Interval = 100 }; // Gunakan interval 100ms agar lebih presisi
                    timer.Tick += TimerTick;
                    timer.Start();
                    lblCountdown.ForeColor = Color.Green;
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            // Hitung waktu yang telah berjalan menggunakan Stopwatch
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Kurangi dari waktu total (waktuTotal - elapsedTime)
            TimeSpan remainingTime = waktuTotal - elapsedTime;

            if (remainingTime.TotalMilliseconds <= 0)
            {
                // Waktu habis, berhenti stopwatch dan timer
                timer.Stop();
                stopwatch.Stop();
                lblCountdown.Text = "00:00:00.000";
                DefaultSetting();
                btnReset.Enabled = true;
                btnSave.Enabled = true;
                MessageBox.Show("Pertandingan selesai", "Informasi");
                isTimerRunning = false;
            }
            else
            {
                // Ubah warna menjadi merah jika waktu tersisa <= 1 menit
                if (remainingTime.TotalSeconds <= 60)
                {
                    lblCountdown.ForeColor = Color.Red;
                }
                else
                {
                    lblCountdown.ForeColor = Color.Green; // Warna default jika lebih dari 1 menit
                }

                // Tampilkan waktu dalam format hh:mm:ss.fff
                lblCountdown.Text = remainingTime.ToString(@"hh\:mm\:ss\.fff");
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            DefaultSetting();
            btnReset.Enabled = true;
            btnToggle = true;
            stopwatch.Stop();
            timer.Stop();
            isTimerRunning = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DefaultSetting();
            lblCountdown.Text = "00:00:00.000";
        }

        private void txtBatch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string query = "SELECT nama_peserta, bib, tag FROM peserta " +
                               "WHERE status = 'Belum' ORDER BY id ASC LIMIT " + txtBatch.Text;
                int no = 1;
                try
                {
                    // Menggunakan 'using' untuk memastikan koneksi ditutup secara otomatis
                    using (MySqlConnection connection = Databases.OpenMysqlDatabase())
                    {
                        connection.Open();

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                           
                            // Mengeksekusi reader dan memeriksa apakah ada baris yang ditemukan
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    // Clear grid sebelum menambah data baru
                                    grdPeserta.Rows.Clear();

                                    // Iterasi melalui setiap baris yang ditemukan
                                    while (reader.Read())
                                    {
                                        // Menambahkan data ke DataGridView (misalnya, kolom nama_peserta, bib, dan tag)
                                        grdPeserta.Rows.Add(no++, 
                                            reader["nama_peserta"].ToString(), 
                                            reader["bib"].ToString(), 
                                            reader["tag"].ToString());
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Tidak ada data ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log atau tangani kesalahan sesuai kebutuhan
                    Console.WriteLine($"Error ValidasiNoRM: {ex.Message}");
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void Dashboard_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
