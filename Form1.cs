using System;
using System.Windows.Forms;
using ReaderB;
using System.Text;
using System.Threading;


namespace Rfid.Reader
{
    public partial class UHFRfidReader : Form
    {
        private int frmcomportindex = -1;  // Menyimpan indeks port
        private bool isReading = false;     // Menandakan apakah kita sedang dalam proses pembacaan tag

        public UHFRfidReader()
        {
            InitializeComponent();
        }

        // Fungsi untuk membuka port dan menginisialisasi pembaca
        private void OpenReader()
        {
            int port;
            string IPAddr;
            byte fComAdr = Convert.ToByte("FF", 16); // Alamat default pembaca
            //byte fBaud = Convert.ToByte(3);
            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Invalid port number", "Error");
                return;
            }

            IPAddr = txtIpAddress.Text;

            //int openresult = StaticClassReaderB.OpenComPort(port, ref fComAdr, fBaud, ref frmcomportindex);

            // Membuka koneksi ke pembaca UHF
            int openresult = StaticClassReaderB.OpenNetPort(port, IPAddr, ref fComAdr, ref frmcomportindex);

            if (frmcomportindex == -1)
            {
                MessageBox.Show("Reader port not opened!", "Error");
                return;
            }

            if (openresult == 0)
            {
                MessageBox.Show("Open port success", "Informasi");
            }
            else
            {
                MessageBox.Show("Gagal membuka port");
                //MessageBox.Show($"TCP/IP error: {openresult:X}", "Error");
                StaticClassReaderB.CloseNetPort(frmcomportindex);
            }
        }

        private void StartReadingTags()
        {
            if (!isReading)
            {
                isReading = true;
                GetWorkModeParameter();
                new Thread(Inventory).Start();  // Mulai thread terpisah untuk polling
            }
        }

        private void Epc()
        {
                int CardNum = 0;
                int Totallen = 0;
                byte[] EPC = new byte[5000];
                string temps;
                byte fComAdr = 0xFF; // Default address reader

                while (isReading)
                {
                    this.Invoke(new Action(() => { txtReceive.AppendText("Reading RFID...\n"); }));

                    // Inventory tanpa parameter TIDFlag
                    int fCmdRet = StaticClassReaderB.Inventory_G2(ref fComAdr, 0x00, 0x00, 0x00, EPC, ref Totallen, ref CardNum, frmcomportindex);

                    if (fCmdRet == 0x00) // Berhasil membaca tag
                    {
                        if (CardNum > 0)
                        {
                            byte[] epcData = new byte[Totallen];
                            Array.Copy(EPC, epcData, Totallen);
                            temps = ByteArrayToHexString(epcData);

                            this.Invoke(new Action(() =>
                            {
                                txtReceive.AppendText($"EPC: {temps}\n");
                            }));

                            // Setelah membaca EPC, coba baca TID menggunakan ReadData
                           
                        }
                        else
                        {
                            this.Invoke(new Action(() => { txtReceive.AppendText("No tags found.\n"); }));
                        }
                    }
                    else
                    {
                        this.Invoke(new Action(() => { txtReceive.AppendText($"Error reading tags! ErrorCode: {fCmdRet:X}\n"); }));
                    }

                    Thread.Sleep(500);
                }
            }

        private void Inventory()
        {
            int CardNum = 0;
            int Totallen = 0;
            byte[] EPC = new byte[5000];
            string temps;
            byte AdrTID = 0x02; // Biasanya alamat TID dimulai dari 0x02
            byte LenTID = 0x06; // 6 byte TID (default)
            byte TIDFlag = 0x01; // 1 untuk membaca TID, 0 untuk tidak

            byte fComAdr = 0xFF; // Alamat default reader

            while (isReading)
            {
                this.Invoke(new Action(() => { txtReceive.AppendText("Reading RFID...\n"); }));

                // Panggil fungsi Inventory_G2 untuk membaca tag
                int fCmdRet = StaticClassReaderB.Inventory_G2(ref fComAdr, AdrTID, LenTID, TIDFlag, EPC, ref Totallen, ref CardNum, frmcomportindex);

                if (fCmdRet == 0x00) // 0x00 berarti sukses membaca tag
                {
                    if (CardNum > 0) // Jika ada tag yang terbaca
                    {
                        byte[] tidData = new byte[Totallen];
                        Array.Copy(EPC, tidData, Totallen);
                        temps = ByteArrayToHexString(tidData);

                        this.Invoke(new Action(() =>
                        {
                            txtReceive.AppendText($"TID: {temps}\n");
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() => { txtReceive.AppendText("No tags found.\n"); }));
                    }
                }
                else
                {
                    this.Invoke(new Action(() => { txtReceive.AppendText($"Error reading tags! ErrorCode: {fCmdRet:X}\n"); }));
                }

                Thread.Sleep(500);
            }
        }

         private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenReader();
        }

        // Event handler untuk mulai membaca tag otomatis saat tombol 'Start' diklik
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartReadingTags();
        }

        private void GetWorkModeParameter()
        {
            byte fComAdr = 0xFF; // Alamat komunikasi reader
            byte[] Parameter = new byte[2]; // Array untuk menyimpan parameter hasil pembacaan

            int result = StaticClassReaderB.GetWorkModeParameter(ref fComAdr, Parameter, frmcomportindex);

            if (result == 0x00) // Jika sukses
            {
                string modeDescription = "";

                switch (Parameter[0]) // Byte pertama menentukan mode kerja
                {
                    case 0x00:
                        modeDescription = "Answer Mode (Manual Trigger)";
                        break;
                    case 0x01:
                        modeDescription = "Auto Mode (Continuous Reading)";
                        break;
                    case 0x02:
                        modeDescription = "Trigger Mode (External Trigger)";
                        break;
                    case 0x03:
                        modeDescription = "Low Power Mode (Sleep)";
                        break;
                    default:
                        modeDescription = "Unknown Mode";
                        break;
                }

                txtReceive.AppendText($"Work Mode: {modeDescription}, Parameter[1]: {Parameter[1]:X2}\n");
            }
            else
            {
                txtReceive.AppendText($"Failed to read work mode parameter! ErrorCode: {result:X}\n");
            }
        }



        private void ReadTagsTid()
        {
            int fCmdRet;
            byte[] ScanModeData = new byte[40960];
            int ValidDatalength = 16;
            while (isReading)
            {
                this.Invoke(new Action(() => { txtReceive.AppendText("Reading RFID...\n"); }));

                fCmdRet = StaticClassReaderB.ReadActiveModeData(ScanModeData, ref ValidDatalength, frmcomportindex);

                if (fCmdRet == 0)
                {
                    string temp = ByteArrayToHexString(ScanModeData);
                    this.Invoke(new Action(() => { txtReceive.AppendText($"TID: {temp}\n"); }));
                }
                else
                {
                    this.Invoke(new Action(() => { txtReceive.AppendText($"Error reading active mode data! ErrorCode: {fCmdRet:X}\n"); }));
                }

                Thread.Sleep(500);
            }  
        }

        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        // Fungsi untuk berhenti membaca tag otomatis
        private void StopReadingTags()
        {
            isReading = false;
            StaticClassReaderB.CloseNetPort(frmcomportindex);
        }

        // Event handler untuk berhenti membaca tag saat tombol 'Stop' diklik
        
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopReadingTags();
        }

        private void frmUsbDataReader_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }

}
