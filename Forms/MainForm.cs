using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfid.Reader.Class;

namespace Rfid.Reader.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        readonly Peserta frmPeserta = new Peserta();
        readonly Dashboard frmDashboard = new Dashboard();
        readonly UHFRfidReader frmUHFReader = new UHFRfidReader();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FlatForm.SetBevel(this, false);
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnPeserta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPeserta.MdiParent = this;
            frmPeserta.Show();
            frmPeserta.BringToFront();
        }

        private void btnButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDashboard.MdiParent = this;
            frmDashboard.Show();
            frmDashboard.BringToFront();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void btnSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUHFReader.MdiParent = this;
            frmUHFReader.Show();
            frmUHFReader.BringToFront();
        }
    }
}
