using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using UHFReader.Readers;

namespace Rfid.Reader
{
    public partial class Reader : Form
    {
        public Reader()
        {
            InitializeComponent();
        }

        //private void Reader_Load(object sender, EventArgs e)
        //{
        //    var ip = new IPEndPoint(IPAddress.Parse("192.168.1.190"), 6000);
        //    var res = new NetReader(ip);

        //    var list = res.Inventory_G2(0, 0, 0);
        //    foreach (var epc in list)
        //    {
        //        Console.WriteLine(ByteArrayToString(epc));
        //    }
        //}
        //public static string ByteArrayToString(byte[] b)
        //{
        //    return BitConverter.ToString(b).Replace("-", "");
        //}
    }
}
