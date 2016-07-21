using Portakal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portakal
{
    public partial class Oteller : Form
    {
        public Oteller()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(Genel.connStr);
        DataSet ds = new DataSet();
        private void Oteller_Load(object sender, EventArgs e)
        {
            int id = 0;
            Rezerv r = new Rezerv();
            int kontrol = r.RezervleriKontrolEt();
            if (kontrol > 0)
            {
                id=r.RezDurumuGuncelle();
                r.idBul(id,txtoid.Text,txttid.Text);
                r.odasayisiniguncelle(txtoid,txttid);
            }



            Otel o = new Otel();
            o.SehirAd = Genel.UlkeAdiOtel;
            o.GirisTarihi = Genel.GirisTarihi;
            o.CikisTarihi = Genel.CikisTarihi;
            o.OdaTipi = Genel.OdaTipi;
            ds = o.OtelleriGetir(o);
            dgvOteller.DataSource = ds.Tables["Oteller"];
            ((DataGridViewImageColumn)dgvOteller.Columns[0]).ImageLayout = DataGridViewImageCellLayout.Stretch;
            dgvOteller.Columns[1].Visible = false;
            dgvOteller.Columns[7].Visible = false;
            dgvOteller.Columns[8].Visible = false;
            dgvOteller.Columns[9].Visible = false;
            dgvOteller.Columns[2].Width = 120;
            dgvOteller.Columns[3].Width = 90;
            dgvOteller.Columns[4].Width = 90;
            dgvOteller.Columns[5].Width = 90;
            dgvOteller.Columns[2].HeaderText = "Oteller";
            dgvOteller.Columns[3].HeaderText = "Dereceler";
            dgvOteller.Columns[4].HeaderText = "Statu";
            dgvOteller.Columns[5].HeaderText = "Oda Tipi";

            dtpBaslangic.Value = Convert.ToDateTime(Genel.GirisTarihi);
            dtpBitis.Value = Convert.ToDateTime(Genel.CikisTarihi);
            TimeSpan ts = dtpBitis.Value - dtpBaslangic.Value;
            lblGece.Text = ts.TotalDays.ToString();
            lblGun.Text = (ts.TotalDays - 1).ToString();
            cbAdet.SelectedItem = "1";
            txtFiyat.Text = "0";
        }

        private void dgvOteller_DoubleClick(object sender, EventArgs e)
        {
            Otel o = new Otel();
            txtOtelAdi.Text = dgvOteller.SelectedRows[0].Cells[2].Value.ToString();
            txtOdaTipi.Text = dgvOteller.SelectedRows[0].Cells[5].Value.ToString();
            TimeSpan ts = dtpBitis.Value - dtpBaslangic.Value;
            txtFiyat.Text = ((ts.TotalDays) * (Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[6].Value))).ToString();
            o.SonKalanOdaSayisi(Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[7].Value), lblSonKalanOda);
        }

        private void cbAdet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genel.GirisTarihi = dtpBaslangic.Value.ToString();
            Genel.CikisTarihi = dtpBitis.Value.ToString();
            TimeSpan ts = dtpBitis.Value - dtpBaslangic.Value;
            txtFiyat.Clear();
            txtFiyat.Text = ((Convert.ToInt32(cbAdet.SelectedItem)) * ((ts.TotalDays) * (Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[6].Value)))).ToString();
        }

        private void dtpBaslangic_ValueChanged(object sender, EventArgs e)
        {
            Genel.GirisTarihi = dtpBaslangic.Value.ToString();
            TimeSpan ts = dtpBitis.Value - dtpBaslangic.Value;
            lblGece.Text = ts.TotalDays.ToString();
            lblGun.Text = (ts.TotalDays - 1).ToString();
            txtFiyat.Text = ((ts.TotalDays) * (Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[6].Value))).ToString();
        }

        private void dtpBitis_ValueChanged(object sender, EventArgs e)
        {
            Genel.CikisTarihi = dtpBitis.Value.ToString();
            TimeSpan ts = dtpBitis.Value - dtpBaslangic.Value;
            lblGece.Text = ts.TotalDays.ToString();
            lblGun.Text = (ts.TotalDays - 1).ToString();
            txtFiyat.Text = ((ts.TotalDays) * (Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[6].Value))).ToString();
        }

        private void btnSatinAl_Click(object sender, EventArgs e)
        {
            Genel.OtelID = Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[1].Value);
            Genel.OtelAd = txtOtelAdi.Text;
            Genel.OdaTipiID = Convert.ToInt32(dgvOteller.SelectedRows[0].Cells[7].Value);
            Genel.OdaTipi = txtOdaTipi.Text;
            Genel.OdaAdet = cbAdet.SelectedItem.ToString();
            Genel.OdaFiyat = txtFiyat.Text;
            this.Hide();
            Rezervasyon frm = new Rezervasyon();
            frm.ShowDialog();
        }
    }
}
