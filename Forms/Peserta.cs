using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Rfid.Reader.Class;

namespace Rfid.Reader.Forms
{
    public partial class Peserta : Form
    {
        #region "Setup"
        private bool isAdd;
        private PesertaDto pesertaData;
        private UsbPortReader usbReader;
   
        public Peserta()
        {
            InitializeComponent();
        }
        #endregion
        #region "Form Action"
        private void Peserta_Load(object sender, EventArgs e)
        {
            
            DefaultSetting();
            LoadData();
            this.WindowState = FormWindowState.Maximized;
        }
        #endregion
        #region "Default State"
        private void ClearScreen()
        {
            txtNomorPeserta.Text = "";
            txtNamaPeserta.Text = "";
            txtTag.Text = "";
            txtBiB.Text = "";
            cboStatus.Text = "";
        }

        private void DisableText()
        {
            txtNomorPeserta.Properties.ReadOnly = true;
            txtNamaPeserta.Properties.ReadOnly = true;
            txtTag.Properties.ReadOnly = true;
            txtBiB.Properties.ReadOnly = true;
            cboStatus.Properties.ReadOnly = true;
        }

        private void EnableText()
        {
            txtNomorPeserta.Properties.ReadOnly = false;
            txtNamaPeserta.Properties.ReadOnly = false;
            txtTag.Properties.ReadOnly = false;
            txtBiB.Properties.ReadOnly = false;
            //cboStatus.Properties.ReadOnly = false;
        }

        private void DefaultSetting()
        {
            btnTambah.Enabled = true;
            btnUbah.Enabled = false;
            btnHapus.Enabled = false;
            btnSimpan.Enabled = false;
            btnBatal.Enabled = false;
            ClearScreen();
            DisableText();
        }
        #endregion
        #region "Form Control Action"
        private void btnBatal_Click(object sender, EventArgs e)
        {
            DefaultSetting();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            ClearScreen();
            EnableText();
            isAdd = true;
            btnBatal.Enabled = true;
            btnSimpan.Enabled = true;
            btnUbah.Enabled = false;
            btnTambah.Enabled = false;
            btnHapus.Enabled = false;
            txtTag.Focus();
            OpenUsbReader();
        }
         private void btnUbah_Click(object sender, EventArgs e)
        {
            EnableText();
            isAdd = false;
            btnBatal.Enabled = true;
            btnSimpan.Enabled = true;
            btnUbah.Enabled = false;
            btnTambah.Enabled = false;
            btnHapus.Enabled = false;
            txtNomorPeserta.Focus();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult message = MessageBox.Show("Apakah anda yakin ingin menghapus data ini?",
                    "Konfirmasi",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (message == DialogResult.OK)
                {
                    // Mengambil ID dari row yang dipilih pada RadGrid
                    string selectedId = grdPeserta.CurrentRow.Cells[3].Value.ToString();

                    // Pastikan ID tidak kosong
                    if (string.IsNullOrWhiteSpace(selectedId))
                    {
                        MessageBox.Show("ID tidak ditemukan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Query DELETE dengan parameter
                    string query = "DELETE FROM peserta WHERE id = @id";

                    using (MySqlConnection connection = Databases.OpenMysqlDatabase())
                    {
                        connection.Open();

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Menambahkan parameter ID ke query
                            command.Parameters.AddWithValue("@id", selectedId);

                            // Mengeksekusi query (karena DELETE, gunakan ExecuteNonQuery)
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Data tidak ditemukan!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            //splashManager.ShowWaitForm();
            //splashManager.SetWaitFormCaption("Please wait");
            //splashManager.SetWaitFormDescription("Proses simpan data pasien...");

            bool boolData = isAdd ? SaveData() : UpdateData();
            if (boolData) { DefaultSetting(); }
            //splashManager.CloseWaitForm();
        }
        #endregion
        #region "Function"
        private bool SaveData()
        {
            if (!ValidateInput())
            {
                return false;
            }

            MySqlConnection conn = Databases.OpenMysqlDatabase();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "add_peserta";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("@p_error_message", "")).MySqlDbType = MySqlDbType.VarChar;
                    cmd.Parameters["@p_error_message"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new MySqlParameter("@p_id", "")).MySqlDbType = MySqlDbType.Int32;
                    cmd.Parameters["@p_id"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new MySqlParameter("@p_nomor_peserta", MySqlDbType.VarChar, 25)).Value = txtNomorPeserta.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_nama_peserta", MySqlDbType.VarChar, 100)).Value = txtNamaPeserta.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_bib", MySqlDbType.VarChar, 5)).Value = txtBiB.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_tag", MySqlDbType.VarChar, 10)).Value = txtTag.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_status", MySqlDbType.VarChar, 50)).Value = "Belum";
                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@p_error_message"].Value.ToString().Trim() == "Y")
                    {

                        grdPeserta.Rows.Add(
                            cmd.Parameters["@p_id"].Value.ToString().Trim(),
                            txtTag.Text.Trim(),
                            txtNamaPeserta.Text.Trim(),
                            txtBiB.Text.Trim());
                            

                        pesertaData = new PesertaDto
                        {
                            id = cmd.Parameters["@p_id"].Value.ToString().Trim(),
                            nomor_peserta = txtNomorPeserta.Text.Trim(),
                            nama_peserta = txtNamaPeserta.Text.Trim(),
                            bib = txtBiB.Text.Trim(),
                            tag = txtTag.Text.Trim(),
                            status = cboStatus.Text.Trim()
                        };
                    }
                    else
                    {
                        MessageBox.Show(cmd.Parameters["@p_error_message"].Value.ToString().Trim());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            conn.Close();

            return true;
        }

        private bool UpdateData()
        {
            if (!ValidateEntry())
            {
                return false;
            }

            MySqlConnection conn = Databases.OpenMysqlDatabase();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update_peserta";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("@p_error_message", "")).MySqlDbType = MySqlDbType.VarChar;
                    cmd.Parameters["@p_error_message"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new MySqlParameter("@p_id", MySqlDbType.Int32)).Value = int.Parse( grdPeserta.CurrentRow.Cells[0].Value.ToString());
                    cmd.Parameters.Add(new MySqlParameter("@p_nomor_peserta", MySqlDbType.VarChar, 25)).Value = txtNomorPeserta.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_nama_peserta", MySqlDbType.VarChar, 100)).Value = txtNamaPeserta.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_bib", MySqlDbType.VarChar, 5)).Value = txtBiB.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_tag", MySqlDbType.VarChar, 10)).Value = txtTag.Text.Trim();
                    cmd.Parameters.Add(new MySqlParameter("@p_status", MySqlDbType.VarChar, 50)).Value = "Belum";
                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@p_error_message"].Value.ToString().Trim() == "Y")
                    {
                        grdPeserta.CurrentRow.Cells[1].Value = txtTag.Text.Trim();
                        grdPeserta.CurrentRow.Cells[2].Value = txtNamaPeserta.Text.Trim();
                        grdPeserta.CurrentRow.Cells[3].Value = txtBiB.Text.Trim();
                        

                        pesertaData = new PesertaDto
                        {
                            nomor_peserta = txtNomorPeserta.Text.Trim(),
                            nama_peserta = txtNamaPeserta.Text.Trim(),
                            bib = txtBiB.Text.Trim(),
                            tag = txtTag.Text.Trim(),
                            status = cboStatus.Text.Trim(),
                            id = cmd.Parameters["@p_id"].Value.ToString().Trim()
                        };
                    }
                    else
                    {
                        MessageBox.Show(cmd.Parameters["@p_error_message"].Value.ToString().Trim());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            conn.Close();
            return true;
        }

        private bool LoadData()
        {
            MySqlConnection mySqlConnection = Databases.OpenMysqlDatabase();
            mySqlConnection.Open();

            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandText = "select id,tag,nama_peserta,bib from peserta " +
                        "order by id asc";

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);

                    DataSet dataSet = null;
                    dataSet = new DataSet();

                    mySqlDataAdapter.Fill(dataSet, "peserta");
                    if (dataSet.Tables["peserta"].Rows.Count > 0)
                    {
                        grdPeserta.DataSource = dataSet.Tables["peserta"];
                        grdPeserta.Columns[0].Width = 75;
                        grdPeserta.Columns[0].HeaderText = "ID";
                        grdPeserta.Columns[1].Width = 120;
                        grdPeserta.Columns[1].HeaderText = "TAG";
                        grdPeserta.Columns[2].Width = 350;
                        grdPeserta.Columns[2].HeaderText = "NAMA PESERTA";
                        grdPeserta.Columns[3].Width = 120;
                        grdPeserta.Columns[3].HeaderText = "BIB";
                        
                        grdPeserta.Focus();
                    }
                    else
                    {
                        dataSet = null;
                        grdPeserta.DataSource = null;  // Optionally clear the grid
                        MessageBox.Show("Data peserta tidak ditemukan", "Informasi");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            mySqlConnection.Close();
            return true;
        }
        private bool ValidasiText(string strFieldName, string strValue)
        {
            // Query menggunakan parameter untuk mencegah SQL Injection
            string query = "SELECT 1 FROM peserta WHERE " + strFieldName + "= @strValue LIMIT 1";

            try
            {
                // Menggunakan 'using' untuk memastikan koneksi ditutup secara otomatis
                using (MySqlConnection connection = Databases.OpenMysqlDatabase())
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Menambahkan parameter dengan nilai yang telah di-trim
                        command.Parameters.AddWithValue("@strValue", strValue.Trim());

                        // Mengeksekusi reader dan memeriksa apakah ada baris yang ditemukan
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log atau tangani kesalahan sesuai kebutuhan
                Console.WriteLine($"Error ValidasiNoRM: {ex.Message}");
                return false;
            }
        }

        private bool ValidateInput()
        {
            //if (ValidasiText("nomor_peserta",txtNomorPeserta.Text.Trim()))
            //{
            //    MessageBox.Show("Nomor Peserta Sudah Tersimpan", "Informasi");
            //    return false;
            //}

            if (ValidasiText("tag", txtTag.Text.Trim()))
            {
                MessageBox.Show("Tag RFID Peserta Sudah Tersimpan", "Informasi");
                return false;
            }
            return ValidateEntry();
        }

        private bool ValidateEntry()
        {

            var validations = new (Control control, string errorMessage)[]
            {
                //(txtNomorPeserta, "Nomor Peserta tidak boleh kosong"),
                (txtNamaPeserta, "Nama Peserta tidak boleh kosong"),
                (txtBiB, "BiB tidak boleh kosong"),
                (txtTag, "Rfid Tag tidak boleh kosong"),
                //(cboStatus, "Status Peserta tidak boleh kosong"),
            };

            foreach (var (control, errorMessage) in validations)
            {
                if (!Helper.ValidateField(control.Text.Trim(), errorMessage))
                {
                    control.Focus();
                    return false;
                }
            }
            return true;

        }
        #endregion

        //private  void btnRfid_Click(object sender, EventArgs e)
        //{
        //    splashManager.ShowWaitForm();
        //    splashManager.SetWaitFormCaption("Please wait");
        //    splashManager.SetWaitFormDescription("Proses simpan data pasien...");


        //    if (btnRfid.Text.Trim() == "Open Reader")
        //    {
        //        usbReader = new UsbPortReader(Properties.Settings.Default.DevicePort.Trim(),
        //            int.Parse(Properties.Settings.Default.DeviceBaudRate.Trim()));

        //        // Subscribing event untuk menangkap data dari USB Reader
        //        usbReader.DataReceivedEvent += UsbReader_DataReceived;
        //        usbReader.OpenConnection();
        //        btnRfid.Text = "Close Reader";
        //    }
        //    else
        //    {
        //        CloseUsbReader(); // Panggil fungsi yang dibuat
        //        btnRfid.Text = "Open Reader";
        //    }

        //    splashManager.CloseWaitForm();
        //}
        private void CloseUsbReader()
        {
            if (usbReader != null)
            {
                usbReader.CloseConnection();
                usbReader = null; // Menghindari pemanggilan ulang pada objek yang sudah tertutup
            }
        }

        private void OpenUsbReader()
        {
            
                usbReader = new UsbPortReader(Properties.Settings.Default.DevicePort.Trim(),
                    int.Parse(Properties.Settings.Default.DeviceBaudRate.Trim()));

                // Subscribing event untuk menangkap data dari USB Reader
                usbReader.DataReceivedEvent += UsbReader_DataReceived;
                usbReader.OpenConnection();
            
        }
        private async void UsbReader_DataReceived(object sender, string data)
        {
            var tag = await Task.Run(() => JsonConvert.DeserializeObject<TagDto>(data));

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => txtTag.Text = tag.tag));
            }
            else
            {
                txtTag.Text = tag.tag;
            }
        }

        private void txtTag_EditValueChanged(object sender, EventArgs e)
        {
            if (txtTag.Text != "")
            {
                CloseUsbReader();
            }
        }

        private void grdPeserta_Click(object sender, EventArgs e)
        {
            try
            {
                txtTag.Text = grdPeserta.CurrentRow.Cells[1].Value.ToString();
                txtNamaPeserta.Text = grdPeserta.CurrentRow.Cells[2].Value.ToString();
                txtBiB.Text = grdPeserta.CurrentRow.Cells[3].Value.ToString();
                btnHapus.Enabled = true;
                btnUbah.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Informasi");
            }  
        }

        private void btnCari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MySqlConnection mySqlConnection = Databases.OpenMysqlDatabase();
                mySqlConnection.Open();

                try
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand())
                    {
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.CommandText = "select id,tag,nama_peserta,bib " +
                            "from peserta " +
                            "where nama_peserta like '%" + 
                            btnCari.Text.Trim() + 
                            "%' order by nama_peserta asc";

                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);

                        DataSet dataSet = null;
                        dataSet = new DataSet();

                        mySqlDataAdapter.Fill(dataSet, "peserta");
                        if (dataSet.Tables["peserta"].Rows.Count > 0)
                        {
                            grdPeserta.DataSource = dataSet.Tables["peserta"];
                            grdPeserta.Columns[0].Width = 75;
                            grdPeserta.Columns[0].HeaderText = "ID";
                            grdPeserta.Columns[1].Width = 120;
                            grdPeserta.Columns[1].HeaderText = "TAG";
                            grdPeserta.Columns[2].Width = 350;
                            grdPeserta.Columns[2].HeaderText = "NAMA PESERTA";
                            grdPeserta.Columns[3].Width = 120;
                            grdPeserta.Columns[3].HeaderText = "BIB";
                           
                            grdPeserta.Focus();
                        }
                        else
                        {
                            dataSet = null;
                            grdPeserta.DataSource = null;  // Optionally clear the gridw
                            MessageBox.Show("Data peserta tidak ditemukan", "Informasi");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                mySqlConnection.Close();
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
