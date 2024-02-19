using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;

namespace Mail2
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            try //olusabilecek hatalar icin
            {
                if (txtAdSoyad.Text == "" || txtPosta.Text == "" || txtKonu.Text == "" || txtMesaj.Text == "") //textbox kontolu
                {
                    lblMesaj.Visible = true;
                    lblMesaj.Text = "Lütfen boş yerleri doldurunuz!";
                    lblMesaj.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff4081");
                }
                else
                {                  
                    string adSoyad = txtAdSoyad.Text;
                    string epostaAdresi = txtPosta.Text; //girilen bilginin mail adresi olup olmadiginin kontrolu front kisimda yapilacak
                    string konu = txtKonu.Text;
                    string mesaj = txtMesaj.Text;
                    string ipAdresi = ""; //ip adresi sunucudan gelecek

                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                        ipAdresi = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                        ipAdresi = HttpContext.Current.Request.UserHostAddress;

                    string mesajIcerik = "";
                    mesajIcerik += "<b>İletişim Formundan Gelen Mesaj</b> / " + DateTime.Now.ToString() + " / " + ipAdresi + "<br/>"; //kisilestirilebilir
                    mesajIcerik += "<b>Adı Soyadı: </b>" + adSoyad + "<br/>";
                    mesajIcerik += "<b>E-posta: </b>" + epostaAdresi + "<br/>";
                    mesajIcerik += "<b>Konu: </b>" + konu + "<br/>";
                    mesajIcerik += "<b>İleti: </b>" + mesaj;

                    MailMessage yeniMesaj = new MailMessage();
                    yeniMesaj.IsBodyHtml = true;
                    yeniMesaj.To.Add("xxxxxxxxxx"); //kullanicinin gonderecegi mesajin gelecegi mail adresi.
                    yeniMesaj.From = new MailAddress("xxxxxxxxxx", "Gönderen", System.Text.Encoding.UTF8); //kullanicinin mesaj gondermek icin kullanacagi mail adresi.
                    yeniMesaj.Subject = "Yeni Mesaj: " + adSoyad + " - " + konu;
                    yeniMesaj.Body = mesajIcerik;

                    SmtpClient gonder = new SmtpClient();
                    gonder.Credentials = new NetworkCredential("xxxxxxxxxx", "********"); //kullanicinin mesaj gondermek icin kullanacagi mail adresinin bilgileri.
                    gonder.Port = 587; //port bilgisi
                    gonder.Host = "outlook.office365.com"; //smtp adresi
                    gonder.EnableSsl = true;
                    gonder.DeliveryMethod = SmtpDeliveryMethod.Network;

                    gonder.Send(yeniMesaj);
                    lblMesaj.Visible = true;
                    lblMesaj.Text = "Mesajınız başarılı bir şekilde gönderildi!";
                    lblMesaj.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a186");
                }
            }
            catch //hata mesaji
            {
                lblMesaj.Visible = true;
                lblMesaj.Text = "Mesajınız gönderilirken bir hata oluştu, lütfen daha sonra tekrar deneyiniz.";
                lblMesaj.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff4081");
            }
            finally //hata olmamasi durumu
            {
                txtAdSoyad.Text = "";
                txtPosta.Text = "";
                txtKonu.Text = "";
                txtMesaj.Text = "";
            }
        }
    }
}