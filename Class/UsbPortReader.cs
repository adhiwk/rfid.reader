using System;
using System.IO.Ports;
using Newtonsoft.Json;


namespace Rfid.Reader.Class
{
    class UsbPortReader
    {
        public event EventHandler<string> DataReceivedEvent;
        private SerialPort _serialPort;

        // Konstruktor untuk menginisialisasi port serial
        public UsbPortReader(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += SerialPort_DataReceived; // Event handler untuk data yang diterima
        }

        // Membuka koneksi ke port USB
        public void OpenConnection()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    Console.WriteLine("Port USB terbuka.");
                }
                else
                {
                    Console.WriteLine("Port USB sudah terbuka.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        // Menutup koneksi ke port USB
        public void CloseConnection()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine("Port USB ditutup.");
            }
            else
            {
                Console.WriteLine("Port USB sudah ditutup.");
            }
        }

        // Event handler untuk data yang diterima
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadLine(); // Membaca data dari port
            //Console.WriteLine(data);

            if (data.StartsWith("Post: {"))
            {
                // Proses data lebih lanjut di sini
                string jsonResult = ProcessValidData(data);
                DataReceivedEvent?.Invoke(this, jsonResult);
            }
        }

        public static string ProcessValidData(string data)
        {
            try
            {
                string json = data.Substring(6);
                var jsonData = JsonConvert.DeserializeObject<ReaderDto>(json);

                var result = new
                {
                    waktu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    tag = jsonData.TagNo
                };

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                var errorResult = new
                {
                    error = "Gagal memproses data",
                    message = ex.Message
                };

                return JsonConvert.SerializeObject(errorResult);
            }
        }
    }
}
