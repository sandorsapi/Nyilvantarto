using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdatReteg;

namespace Nyilvantarto
{
    public partial class Csaladok : Form
    {
        AdatReteg.AdatReteg ar = new AdatReteg.AdatReteg();
        public Csaladok()
        {           
            InitializeComponent();
            csaladNevek();
            
        }

        //Csalad Form bezárása
        private void btnCsaladBezar_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Családnevek betöltése a ListBox-ba
        public void csaladNevek()
        {           
            ar.dbMegnyit();
            DataTable aNevek = new DataTable();
            aNevek = ar.csaladNevek();
            if (aNevek.Rows.Count!=0)
            {
                for (int i = 0; i < aNevek.Rows.Count; i++)
                {
                    string nevek = aNevek.Rows[i]["vezeteknev"].ToString();                    
                    lbCsaladnev.Items.Add(nevek);
                }
            }
            ar.dbBezar();
        } 

        //A listából kiválasztás
        private void lbCsaladnev_SelectedIndexChanged(object sender, EventArgs e)
        {
            Csaladok cs = new Csaladok();
            string csaladnev = null;
            try
            {
                csaladnev = lbCsaladnev.SelectedItem.ToString();
                lblCsaladnev.Text = csaladnev;
                adatLista();
                cs.Refresh();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Még nincs kiválasztva családnév!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Adatok betöltése
        public void adatLista()
        {           
            ar.dbMegnyit();
            DataTable aAdatok = new DataTable();
            aAdatok = ar.cegertek(lblCsaladnev.Text);
            if (aAdatok.Rows.Count !=0)
            {
                lblCegnev.Text = aAdatok.Rows[0]["szervezetnev"].ToString();
                lblAdoszam.Text = aAdatok.Rows[0]["adoszam"].ToString();
                lblCegertek.Text = aAdatok.Rows[0]["cegertek"].ToString();
                string szazalekjel = " %".ToString();
                string stArany = aAdatok.Rows[0]["tulajdonresz"].ToString();
                lblTulajdonosSzam.Text = aAdatok.Rows[0]["nev_db"].ToString();
                double ertek = (Convert.ToDouble(aAdatok.Rows[0]["cegertek"].ToString()) * Convert.ToDouble(stArany))/100;
                lblArany.Text = stArany + szazalekjel;
                string osszeg =" Ft".ToString();
                lblAranyOsszeg.Text = Convert.ToString(ertek)+ osszeg;
            }
            ar.dbBezar();
        }
    }
}

