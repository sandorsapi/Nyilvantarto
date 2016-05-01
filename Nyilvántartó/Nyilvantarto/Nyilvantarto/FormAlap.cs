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
    
    public partial class FormAlap : Form
    {
        AdatReteg.AdatReteg ar = new AdatReteg.AdatReteg();
        private int record = 0;
        private int lapozoIndex = 0;
        private string tipus = null;
        private string szemelyCimTipus = null;
        private string szervezetCimTipus = null;
        private int cimLapozoIndex = 0;
        private int cimRecord = 0;
        private int szemelyLapozoIndex = 0;
        private int szemelyRecord = 0;
        private int szemelyCimLapozoIndex = 0;
        private int szemelyCimRecord = 0;
        private string tulajdonresz = "100";
        private int szervezet_id;
        public FormAlap()
        {
            InitializeComponent();
            szervBetolt();
            szervezetCimBetolt();
            cegListaBetolt();
            szemelyBetolt();
            szemelyCimBetolt(); 
        }

        //Program bezárása
        private void btnBezar_Click(object sender, EventArgs e)
        {
            Close();
        }
  
        //Új adatok bevitele
        private void btnCégUj_Click(object sender, EventArgs e)
        {
            //A mezők kitöltésének ellenőrzése          
            if (tbCegnev.Text.Length == 0 || tbAdoTorzs.Text.Length == 0 || tbAdoJel.Text.Length == 0 || tbAdoJelzet.Text.Length == 0 || tbCegertek.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Az adószám helyességének ellenőrzése
                if (tbAdoTorzs.Text.Length > 8 || tbAdoTorzs.Text.Length < 8 || tbAdoJel.Text.Length > 1 || tbAdoJel.Text.Length < 1 || tbAdoJelzet.Text.Length > 2 || tbAdoJelzet.Text.Length < 2)
                {
                    MessageBox.Show("Az adószámot hibásan adta meg!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {                   
                        string adoszam = tbAdoTorzs.Text + "-" + tbAdoJel.Text + "-" + tbAdoJelzet.Text;
                        //Ha a bevinni akart adószám már létezik
                        if (adoszamEllenor(adoszam)==false)
                        {
                            MessageBox.Show("Az adószám már szerepel az adatbázisban!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {                        
                        try
                        {
                            ar.dbMegnyit();
                            ar.cegadatUj(tbCegnev.Text, adoszam, Convert.ToInt32(tbCegertek.Text));
                            MessageBox.Show("A felvitel sikeres");
                            ar.dbBezar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A felvitel sikertelen!" + ex);
                        }
                        finally
                        {
                            ar.dbBezar();
                        }
                        tbCegnev.Text = null;
                        tbAdoTorzs.Text = null;
                        tbAdoJel.Text = null;
                        tbAdoJelzet.Text = null;
                        tbCegertek.Text = null;
                        szervBetolt();
                        cegListaBetolt();
                    }
                }
            }
        }

        //Adatok betöltése a controlokba
        public void szervBetolt()
        {           
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szervezetAllTabla();
            if (aSzerv.Rows.Count != 0)
            {
                record = aSzerv.Rows.Count;
                lblRecord.Text = lapozoIndex.ToString();
                tbCegnev.Text = aSzerv.Rows[lapozoIndex]["szervezetnev"].ToString();
                string[] adotorzs = aSzerv.Rows[lapozoIndex]["adoszam"].ToString().Split(new Char[] { '-' });
                tbAdoTorzs.Text = adotorzs[0].ToString();
                tbAdoJel.Text = adotorzs[1].ToString();
                tbAdoJelzet.Text = adotorzs[2].ToString();
                tbCegertek.Text = aSzerv.Rows[lapozoIndex]["cegertek"].ToString();
            }
            ar.dbBezar();
        }

        //Cég címeinek betöltése
        public void szervezetCimBetolt()
        {            
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szervezetAllTabla();
            string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
            aSzerv = ar.szervezetCimAllTabla(Convert.ToInt32(sor));
            if (aSzerv.Rows.Count != 0)
            {
                cimRecord = aSzerv.Rows.Count;
                lblCimRecord.Text = cimLapozoIndex.ToString();
                cbCegCimTipus.Text = aSzerv.Rows[cimLapozoIndex]["tipus"].ToString();
                tbIranyitoszam.Text = aSzerv.Rows[cimLapozoIndex]["iranyitoszam"].ToString();
                tbTelepules.Text = aSzerv.Rows[cimLapozoIndex]["telepules"].ToString();
                tbUtca.Text = aSzerv.Rows[cimLapozoIndex]["utca"].ToString();
                tbHazszam.Text = aSzerv.Rows[cimLapozoIndex]["hazszam"].ToString();
            }
            ar.dbBezar();
        }

        //Recordban mozgás le
        private void btnCegBal_Click(object sender, EventArgs e)
        {
            lapozoIndex--;
            if (lapozoIndex <= 0)
            {
                lapozoIndex = 0;
            }
            lblRecord.Text = lapozoIndex.ToString();
            cbCegCimTipus.SelectedText = null;
            cbCegCimTipus.Text = null;
            tbIranyitoszam.Text = null;
            tbTelepules.Text = null;
            tbUtca.Text = null;
            tbHazszam.Text = null;
            cimLapozoIndex = 0;
            szervBetolt();
            szervezetCimBetolt();
        }

        //Recordban mozgás fel
        private void btnCegJobb_Click(object sender, EventArgs e)
        {
            lapozoIndex++;
            if (lapozoIndex >= record)
            {
                lapozoIndex = record - 1;
            }
            lblRecord.Text = lapozoIndex.ToString();
            cbCegCimTipus.SelectedText = null;
            cbCegCimTipus.Text = null;
            tbIranyitoszam.Text = null;
            tbTelepules.Text = null;
            tbUtca.Text = null;
            tbHazszam.Text = null;
            cimLapozoIndex = 0;
            szervBetolt();
            szervezetCimBetolt();
        }

        //Módosítás végrehajtása
        private void btnCegModosit_Click(object sender, EventArgs e)
        {           
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szervezetAllTabla();
            string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
            //A mezők kitöltésének ellenőrzése
            if (tbCegnev.Text.Length == 0 || tbAdoJelzet.Text.Length == 0 || tbAdoJel.Text.Length == 0 || tbAdoJelzet.Text.Length == 0 || tbCegertek.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Az adószám helyességének ellenőrzése
                if (tbAdoTorzs.Text.Length > 8 || tbAdoTorzs.Text.Length < 8 || tbAdoJel.Text.Length > 1 || tbAdoJel.Text.Length < 1 || tbAdoJelzet.Text.Length > 2 || tbAdoJelzet.Text.Length < 2)
                {
                    MessageBox.Show("Az adószámot hibásan adta meg!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string adoszam = tbAdoTorzs.Text + "-" + tbAdoJel.Text + "-" + tbAdoJelzet.Text;
                    //Ha a bevinni akart adószám már létezik
                    if (adoszamEllenor(adoszam) == false)
                    {
                        MessageBox.Show("Az adószám már szerepel az adatbázisban!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        try
                        {
                            ar.szervezetModosit(tbCegnev.Text, adoszam, Convert.ToInt32(tbCegertek.Text), Convert.ToInt32(sor));
                            MessageBox.Show("A módosítás sikeres");
                            ar.dbBezar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A Módosítás sikertelen!" + ex, "SQL hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            ar.dbBezar();
                        }
                        tbCegnev.Text = null;
                        tbAdoTorzs.Text = null;
                        tbAdoJel.Text = null;
                        tbAdoJelzet.Text = null;
                        tbCegertek.Text = null;
                        lapozoIndex = 0;
                        szervBetolt();
                        cegListaBetolt();
                    }
                }
            }
        }

        //Törlés végrehajtása
        private void btnCegTorles_Click(object sender, EventArgs e)
        {           
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szervezetAllTabla();
            string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
            try
            {
                ar.deleteSzervezet(sor);
                MessageBox.Show("Töröltem a sort!");
                ar.dbBezar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A törlést nem tudom végrehajtani!" + ex, "SQL hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ar.dbBezar();
            }
            tbCegnev.Text = null;
            tbAdoTorzs.Text = null;
            tbAdoJel.Text = null;
            tbAdoJelzet.Text = null;
            tbCegertek.Text = null;
            lapozoIndex = 0;
            szervBetolt();
            szervezetCimBetolt();
            cegListaBetolt();
        }

        //Új cim felvitele
        private void btnCimUj_Click(object sender, EventArgs e)
        {
            //A mezők kitöltésének ellenőrzése
            if (tbIranyitoszam.Text.Length == 0 || tbTelepules.Text.Length == 0 || tbUtca.Text.Length == 0 || tbHazszam.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {                
                ar.dbMegnyit();
                DataTable aSzerv = new DataTable();
                aSzerv = ar.szervezetAllTabla();
                string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
                try
                {
                    ar.cegCimAdatUj(tipus, tbIranyitoszam.Text, tbTelepules.Text, tbUtca.Text, tbHazszam.Text, Convert.ToInt32(sor));
                    MessageBox.Show("A felvitel sikeres");
                    ar.dbBezar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A felvitel sikertelen!" + ex);
                }
                finally
                {
                    ar.dbBezar();
                }
                cbCegCimTipus.SelectedText = null;
                cbCegCimTipus.Text = null;
                tbIranyitoszam.Text = null;
                tbTelepules.Text = null;
                tbUtca.Text = null;
                tbHazszam.Text = null;
                cimLapozoIndex = 0;
                szervezetCimBetolt();
            }
        }

        //Szervezet címének módosítása
        private void btnCimModosit_Click(object sender, EventArgs e)
        {
            //A mezők kitöltésének ellenőrzése
            if (tbIranyitoszam.Text.Length == 0 || tbTelepules.Text.Length == 0 || tbUtca.Text.Length == 0 || tbHazszam.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {                
                ar.dbMegnyit();
                DataTable aSzerv = new DataTable();
                aSzerv = ar.szervezetAllTabla();
                string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
                aSzerv = ar.szervezetCimAllTabla(Convert.ToInt32(sor));
                string cimsor = aSzerv.Rows[cimLapozoIndex]["szerv_cimID"].ToString();
                szervezetCimTipus = cbCegCimTipus.Text;
                try
                {
                    ar.szervezetCimModosit(szervezetCimTipus, tbIranyitoszam.Text, tbTelepules.Text, tbUtca.Text, tbHazszam.Text, Convert.ToInt32(sor), Convert.ToInt32(cimsor));
                    MessageBox.Show("A módosítás sikeres");
                    ar.dbBezar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A módosítás sikertelen!" + ex);
                }
                finally
                {
                    ar.dbBezar();
                }
                cbCegCimTipus.SelectedText = null;
                cbCegCimTipus.Text = null;
                tbTelepules.Text = null;
                tbIranyitoszam.Text = null;
                tbUtca.Text = null;
                tbHazszam.Text = null;
                cimLapozoIndex = 0;
                szervezetCimBetolt();
            }
        }   

        private void cbCegCimTipus_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipus = cbCegCimTipus.SelectedItem.ToString();
        }

        //Cim recordban mozgás le
        private void btnCimBal_Click_1(object sender, EventArgs e)
        {
            cimLapozoIndex--;
            if (cimLapozoIndex <= 0)
            {
                cimLapozoIndex = 0;
            }
            lblCimRecord.Text = cimLapozoIndex.ToString();
            szervezetCimBetolt();
        }

        //Cim recordban mozgás fel
        private void btnCimJobb_Click_1(object sender, EventArgs e)
        {
            cimLapozoIndex++;
            if (cimLapozoIndex >= cimRecord)
            {
                cimLapozoIndex = cimRecord - 1;
            }
            lblCimRecord.Text = cimLapozoIndex.ToString();
            szervezetCimBetolt();
        }

        //Szervezet Cim sor törlés
        private void btnCimTorles_Click_1(object sender, EventArgs e)
        {           
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szervezetAllTabla();
            string sor = aSzerv.Rows[lapozoIndex]["szervID"].ToString();
            aSzerv = ar.szervezetCimAllTabla(Convert.ToInt32(sor));
            sor = aSzerv.Rows[cimLapozoIndex]["szerv_cimID"].ToString();
            try
            {
                ar.deleteSzervezetCim(sor);
                MessageBox.Show("Töröltem a sort!");
                ar.dbBezar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A törlést nem tudom végrehajtani!" + ex, "SQL hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ar.dbBezar();
            }
            cbCegCimTipus.SelectedText = null;
            cbCegCimTipus.Text = null;
            tbIranyitoszam.Text = null;
            tbTelepules.Text = null;
            tbUtca.Text = null;
            tbHazszam.Text = null;
            cimLapozoIndex = 0;
            szervezetCimBetolt();
        }

        //Cégek betőltése
        public void cegListaBetolt()
        {
            cbTulajdonok.Items.Clear();            
            DataTable aCegek = new DataTable();
            ar.dbMegnyit();
            aCegek = ar.szervezetAllTabla();
            for (int i = 0; i < aCegek.Rows.Count; i++)
            {               
                cbTulajdonok.Items.Add(aCegek.Rows[i]["szervezetnev"].ToString());
            }
            ar.dbBezar();
        }

        //Új személy felvitele
        private void btnSzemelyUj_Click(object sender, EventArgs e)
        {                            
            //A mezők kitöltésének ellenőrzése           
            if (tbVezeteknev.Text.Length==0 || tbUtonev.Text.Length==0 || tbSzulDatum.Text.Length==0 ||
                tbSzulHely.Text.Length==0 || tbAnyNeve.Text.Length==0 || tbAdoazonosito.Text.Length==0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (tbAdoazonosito.Text.Length > 10 || tbAdoazonosito.Text.Length < 10)
                {
                    MessageBox.Show("Az adóazonosító 10 jegyű szám!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Ha az adóazonosító már létezik
                    if (adoazonositoEllenor(tbAdoazonosito.Text.ToString()) == false)
                    {
                        MessageBox.Show("Az adóazonosító már szerepel az adatbázisban!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {                        
                        try
                        {
                            ar.dbMegnyit();                           
                            ar.szemelyAdatUj(tbVezeteknev.Text, tbUtonev.Text, tbSzulDatum.Text, tbSzulHely.Text, tbAnyNeve.Text, tbAdoazonosito.Text, tulajdonresz, szervezet_id);
                            //Tulajdonrészek kiszámítása és módosítása
                            tulajdonResz(tbVezeteknev.Text, tbAnyNeve.Text, szervezet_id);
                            MessageBox.Show("A felvitel sikeres");
                            ar.dbBezar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A felvitel sikertelen!" + ex);
                        }
                        finally
                        {
                            ar.dbBezar();
                        }

                        cbTulajdonok.SelectedText = null;
                        cbTulajdonok.Text = null;
                        tbVezeteknev.Text = null;
                        tbUtonev.Text = null;
                        tbSzulDatum.Text = null;
                        tbSzulHely.Text = null;
                        tbAnyNeve.Text = null;
                        tbAdoazonosito.Text = null;
                        szemelyLapozoIndex = 0;
                        szemelyBetolt();
                    }
                }
            }
        }

        //Személyek módosítása
        private void btnSzemelyModosit_Click(object sender, EventArgs e)
        {            
            ar.dbMegnyit();
            DataTable szemelyTabla = ar.szemelyAllTabla();
            tulajdonresz = szemelyTabla.Rows[szemelyLapozoIndex]["tulajdonresz"].ToString();
            //A mezők kitöltésének ellenőrzése
            if (tbVezeteknev.Text.Length == 0 || tbUtonev.Text.Length == 0 || tbSzulDatum.Text.Length == 0 ||
                tbSzulHely.Text.Length == 0 || tbAnyNeve.Text.Length == 0 || tbAdoazonosito.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (tbAdoazonosito.Text.Length > 10 || tbAdoazonosito.Text.Length < 10)
                {
                    MessageBox.Show("Az adóazonosító 10 jegyű szám!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                        try
                        {
                            ar.szemelyAdatModosit(tbVezeteknev.Text, tbUtonev.Text, tbSzulDatum.Text, tbSzulHely.Text, tbAnyNeve.Text, tbAdoazonosito.Text, tulajdonresz, szervezet_id, Convert.ToInt32(szemelyTabla.Rows[szemelyLapozoIndex]["szemelyID"]));                          
                            //Tulajdonrészek kiszámítása és módosítása
                            tulajdonResz(tbVezeteknev.Text, tbAnyNeve.Text, szervezet_id);
                            MessageBox.Show("A módosítás sikeres");
                            ar.dbBezar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A módosítás sikertelen!" + ex);
                        }
                        finally
                        {
                            ar.dbBezar();
                        }
                        cbTulajdonok.SelectedText = null;
                        cbTulajdonok.Text = null;
                        tbVezeteknev.Text = null;
                        tbUtonev.Text = null;
                        tbSzulDatum.Text = null;
                        tbSzulHely.Text = null;
                        tbAnyNeve.Text = null;
                        tbAdoazonosito.Text = null;
                        szemelyLapozoIndex = 0;
                        szemelyBetolt();               
                }
            }
        }

        //Személyek betőltése
        public void szemelyBetolt()
        {            
            ar.dbMegnyit();
            DataTable aSzemelyek = new DataTable();           
            aSzemelyek = ar.szemelyAllTabla();            
            if (aSzemelyek.Rows.Count != 0)
            {               
                szemelyRecord = aSzemelyek.Rows.Count;
                lblSzemelyRecord.Text = szemelyLapozoIndex.ToString();               
                DataTable szervezet = ar.szervezetNevTabla(Convert.ToInt32(aSzemelyek.Rows[szemelyLapozoIndex]["szervezet"].ToString()));
                lblTulajdon.Text = szervezet.Rows[0]["szervezetnev"].ToString();
                tbVezeteknev.Text = aSzemelyek.Rows[szemelyLapozoIndex]["vezeteknev"].ToString();
                tbUtonev.Text = aSzemelyek.Rows[szemelyLapozoIndex]["utonev"].ToString();
                tbSzulDatum.Text = aSzemelyek.Rows[szemelyLapozoIndex]["szul_datum"].ToString();
                tbSzulHely.Text = aSzemelyek.Rows[szemelyLapozoIndex]["szul_hely"].ToString();
                tbAnyNeve.Text = aSzemelyek.Rows[szemelyLapozoIndex]["anyja_neve"].ToString();
                tbAdoazonosito.Text = aSzemelyek.Rows[szemelyLapozoIndex]["adoazonosito"].ToString();
                tbTulajdonresz.Text = aSzemelyek.Rows[szemelyLapozoIndex]["tulajdonresz"].ToString();
            }
            ar.dbBezar();
        }

        //Szemelyek törlése
        private void btnSzemelyTorol_Click(object sender, EventArgs e)
        {            
            ar.dbMegnyit();
            DataTable aSzemely = new DataTable();
            aSzemely = ar.szemelyAllTabla();
            string sor = aSzemely.Rows[szemelyLapozoIndex]["szemelyID"].ToString();
            string nev = aSzemely.Rows[szemelyLapozoIndex]["vezeteknev"].ToString();
            string anyjneve = aSzemely.Rows[szemelyLapozoIndex]["anyja_neve"].ToString();
            string szervezet = aSzemely.Rows[szemelyLapozoIndex]["szervezet"].ToString();
            try
            {               
                ar.deleteSzemely(sor);
                //Tulajdonrészek kiszámítása és módosítása
                tulajdonResz(nev, anyjneve, Convert.ToInt32(szervezet));
                MessageBox.Show("Töröltem a sort!");
                ar.dbBezar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A törlést nem tudom végrehajtani!" + ex, "SQL hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ar.dbBezar();
            }
            cbTulajdonok.SelectedText = null;
            cbTulajdonok.Text = null;
            tbVezeteknev.Text = null;
            tbUtonev.Text = null;
            tbSzulDatum.Text = null;
            tbSzulHely.Text = null;
            tbAnyNeve.Text = null;
            tbAdoazonosito.Text = null;
            szemelyLapozoIndex = 0;
            szemelyBetolt();
            szemelyCimBetolt();
        }

        //Szemely recordok mozgatása le
        private void btnSzemelyBal_Click(object sender, EventArgs e)
        {
            szemelyLapozoIndex--;
            if (szemelyLapozoIndex <= 0)
            {
                szemelyLapozoIndex = 0;
            }
            lblSzemelyRecord.Text = szemelyLapozoIndex.ToString();
            cbSzemelyCimTipus.SelectedText = null;
            cbSzemelyCimTipus.Text = null;
            tbSzemelyTelepules.Text = null;
            tbSzemelyIranyitoszam.Text = null;
            tbSzemelyUtca.Text = null;
            tbSzemelyHazszam.Text = null;
            szemelyCimLapozoIndex = 0;
            szemelyBetolt();
            szemelyCimBetolt();           
        }

        //Szemely recordok mozgatása fel
        private void btnSzemelyJobb_Click(object sender, EventArgs e)
        {
            szemelyLapozoIndex++;
            if (szemelyLapozoIndex >= szemelyRecord)
            {
                szemelyLapozoIndex = szemelyRecord - 1;
            }
            lblSzemelyRecord.Text = szemelyLapozoIndex.ToString();
            cbSzemelyCimTipus.SelectedText = null;
            cbSzemelyCimTipus.Text = null;
            tbSzemelyTelepules.Text = null;
            tbSzemelyIranyitoszam.Text = null;
            tbSzemelyUtca.Text = null;
            tbSzemelyHazszam.Text = null;
            szemelyCimLapozoIndex = 0;
            szemelyBetolt();
            szemelyCimBetolt();          
        }

        //Tulajdonolni akart szervezet kiválasztása
        private void cbTulajdonok_SelectedIndexChanged(object sender, EventArgs e)
        {           
            ar.dbMegnyit();
            DataTable cegNevTabla = new DataTable();
            cegNevTabla = ar.szervezetNevIDTabla(cbTulajdonok.SelectedItem.ToString());
            szervezet_id = Convert.ToInt32(cegNevTabla.Rows[0]["szervID"].ToString());           
        }

        //Személyek új címadatainak felvitele
        private void btnSzemelyCimUj_Click(object sender, EventArgs e)
        {
            //A mezők kitöltésének ellenőrzése
            if (tbSzemelyTelepules.Text.Length == 0 || tbSzemelyIranyitoszam.Text.Length == 0 || tbSzemelyUtca.Text.Length == 0 || tbSzemelyHazszam.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {               
                ar.dbMegnyit();
                DataTable aSzemely = new DataTable();
                aSzemely = ar.szemelyAllTabla();
                string sor = aSzemely.Rows[szemelyLapozoIndex]["szemelyID"].ToString();
                try
                {
                    ar.szemelyCimUj(szervezetCimTipus, tbSzemelyIranyitoszam.Text, tbSzemelyTelepules.Text, tbSzemelyUtca.Text, tbSzemelyHazszam.Text, Convert.ToInt32(sor));
                    MessageBox.Show("A felvitel sikeres");
                    ar.dbBezar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A felvitel sikertelen!" + ex);
                }
                finally
                {
                    ar.dbBezar();
                }
                cbSzemelyCimTipus.SelectedText = null;
                cbSzemelyCimTipus.Text = null;
                tbSzemelyTelepules.Text = null;
                tbSzemelyIranyitoszam.Text = null;
                tbSzemelyUtca.Text = null;
                tbSzemelyUtca.Text = null;
                szemelyCimBetolt();
            }
        }

        //Személyek címének módosítása
        private void btnSzemelyCimModosit_Click(object sender, EventArgs e)
        {
            //A mezők kitöltésének ellenőrzése
            if (tbIranyitoszam.Text.Length == 0 || tbTelepules.Text.Length == 0 || tbUtca.Text.Length == 0 || tbHazszam.Text.Length == 0)
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {                
                ar.dbMegnyit();
                DataTable aSzemely = new DataTable();
                aSzemely = ar.szemelyAllTabla();
                string sor = aSzemely.Rows[szemelyLapozoIndex]["szemelyID"].ToString();
                aSzemely = ar.szemelyCimAllTabla(Convert.ToInt32(sor));
                string cimsor = aSzemely.Rows[szemelyCimLapozoIndex]["sz_cimID"].ToString();
                szervezetCimTipus = cbSzemelyCimTipus.Text;
                try
                {
                    ar.szemelyCimAdatModosit(szervezetCimTipus, tbSzemelyIranyitoszam.Text, tbSzemelyTelepules.Text, tbSzemelyUtca.Text, tbSzemelyHazszam.Text, Convert.ToInt32(sor), Convert.ToInt32(cimsor));
                    MessageBox.Show("A módosítás sikeres");
                    ar.dbBezar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A módosítás sikertelen!" + ex);
                }
                finally
                {
                    ar.dbBezar();
                }
                cbSzemelyCimTipus.SelectedText = null;
                cbSzemelyCimTipus.Text = null;
                tbSzemelyTelepules.Text = null;
                tbSzemelyIranyitoszam.Text = null;
                tbSzemelyUtca.Text = null;
                tbSzemelyUtca.Text = null;
                szemelyCimLapozoIndex = 0;
                szemelyCimBetolt();
            }
        }

        //Szemelyek címeinek törlése
        private void btnSzemelyCimTorol_Click(object sender, EventArgs e)
        {            
            ar.dbMegnyit();
            DataTable aSzerv = new DataTable();
            aSzerv = ar.szemelyAllTabla();
            string sor = aSzerv.Rows[szemelyLapozoIndex]["szemelyID"].ToString();
            aSzerv = ar.szemelyCimAllTabla(Convert.ToInt32(sor));
            sor = aSzerv.Rows[szemelyCimLapozoIndex]["sz_cimID"].ToString();
            try
            {
                ar.deleteSzemelyCim(sor);
                MessageBox.Show("Töröltem a sort!");
                ar.dbBezar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A törlést nem tudom végrehajtani!" + ex, "SQL hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ar.dbBezar();
            }
            cbSzemelyCimTipus.SelectedText = null;
            cbSzemelyCimTipus.Text = null;
            tbSzemelyTelepules.Text = null;
            tbSzemelyIranyitoszam.Text = null;
            tbSzemelyUtca.Text = null;
            tbSzemelyUtca.Text = null;
            szemelyCimLapozoIndex = 0;
            szemelyCimBetolt();
        }

        //Személyek címeinek betöltése
        public void szemelyCimBetolt()
        {            
            ar.dbMegnyit();
            DataTable aSzemely = new DataTable();
            aSzemely = ar.szemelyAllTabla();
            string sor = aSzemely.Rows[szemelyLapozoIndex]["szemelyID"].ToString();
            aSzemely = ar.szemelyCimAllTabla(Convert.ToInt32(sor));
            if (aSzemely.Rows.Count != 0)
            {
                szemelyCimRecord = aSzemely.Rows.Count;
                lblSzemelyCimRecord.Text = szemelyCimLapozoIndex.ToString();
                cbSzemelyCimTipus.Text = aSzemely.Rows[szemelyCimLapozoIndex]["tipus"].ToString();
                tbSzemelyIranyitoszam.Text = aSzemely.Rows[szemelyCimLapozoIndex]["iranyitoszam"].ToString();
                tbSzemelyTelepules.Text = aSzemely.Rows[szemelyCimLapozoIndex]["telepules"].ToString();
                tbSzemelyUtca.Text = aSzemely.Rows[szemelyCimLapozoIndex]["utca"].ToString();
                tbSzemelyHazszam.Text = aSzemely.Rows[szemelyCimLapozoIndex]["hazszam"].ToString();
            }
            ar.dbBezar();
        }

        //Személyek címének recordjai le
        private void btnSzemelyCimBal_Click(object sender, EventArgs e)
        {
            szemelyCimLapozoIndex--;
            if (szemelyCimLapozoIndex <= 0)
            {
                szemelyCimLapozoIndex = 0;
            }
            lblSzemelyCimRecord.Text = szemelyCimLapozoIndex.ToString();
            cbSzemelyCimTipus.SelectedText = null;
            cbSzemelyCimTipus.Text = null;
            tbSzemelyTelepules.Text = null;
            tbSzemelyIranyitoszam.Text = null;
            tbSzemelyUtca.Text = null;
            tbSzemelyUtca.Text = null;          
            szemelyCimBetolt();
        }

        //Személyek címének recordjai fel
        private void btnSzemelyCimJobb_Click(object sender, EventArgs e)
        {
            szemelyCimLapozoIndex++;
            if (szemelyCimLapozoIndex >= szemelyCimRecord)
            {
                szemelyCimLapozoIndex = szemelyCimRecord - 1;
            }
            lblSzemelyCimRecord.Text = szemelyCimLapozoIndex.ToString();
            cbSzemelyCimTipus.SelectedText = null;
            cbSzemelyCimTipus.Text = null;
            tbSzemelyTelepules.Text = null;
            tbSzemelyIranyitoszam.Text = null;
            tbSzemelyUtca.Text = null;
            tbSzemelyUtca.Text = null;            
            szemelyCimBetolt();
        }

        private void cbSzemelyCimTipus_SelectedIndexChanged(object sender, EventArgs e)
        {
            szervezetCimTipus = cbSzemelyCimTipus.SelectedItem.ToString();
        }

        private void btnCegKereso_Click(object sender, EventArgs e)
        {           
            ar.dbMegnyit();
            DataTable keresTabla = new DataTable();
            keresTabla = ar.Kereses(tbCegnevKeres.Text.ToString());
            if (keresTabla.Rows.Count != 0)
            {
                for (int i = 0; i < keresTabla.Rows.Count; i++)
                {                   
                    lblKeresettCegNev.Text = keresTabla.Rows[i]["szervezetnev"].ToString();
                    lblKeresCegAdoszam.Text = keresTabla.Rows[i]["adoszam"].ToString();
                    lblKeresCegertek.Text = keresTabla.Rows[i]["cegertek"].ToString()+" " + "Ft";
                }
            }
            else
            {
                MessageBox.Show("Ninc találat!");
            }           
        }

        //Az adószám első nyolc tagjának ellenőrzése
        public Boolean adoszamEllenor(string adoszam)
        {            
            string[] tbTorzs = adoszam.Split('-');           
            ar.dbMegnyit();
            DataTable adoTabla = ar.adoszam();
            ar.dbBezar();
            if (adoTabla.Rows.Count != 0)
            {
                for (int i = 0; i < adoTabla.Rows.Count; i++)
                {
                    string[] torzs = (adoTabla.Rows[i]["adoszam"].ToString().Split('-'));
                    
                    if (tbTorzs[0] == torzs[0])
                    {
                        return false;
                    }                    
                }
            }        
            return true;          
        }

        //Az adóazonosító ellenőrzése
        public Boolean adoazonositoEllenor(string adoazonosito)
        {           
            ar.dbMegnyit();
            DataTable adoTabla = ar.adoazonosito();
            ar.dbBezar();
            if (adoTabla.Rows.Count != 0)
            {
                for (int i = 0; i < adoTabla.Rows.Count; i++)
                {
                    string azonosito = adoTabla.Rows[i]["adoazonosito"].ToString();
                    if (adoazonosito == azonosito)
                    {
                        return false;
                    }
                }
            }        
            return true;
        }

        //Tulajdonrészek kiszámítása és recordok módosítása
        public void tulajdonResz(string csaladnev, string anyaneve, int cegazonosito)
        {        
            ar.dbMegnyit();
            DataTable aSzemely = new DataTable();
            aSzemely = ar.csaladokTabla(csaladnev, anyaneve, cegazonosito);
            int sor = aSzemely.Rows.Count;
            double szazalek = 100;
            if (sor <= 1)
            {             
                szazalek = 100;
                tulajdonresz = szazalek.ToString();
            }
            else
            {
                tulajdonresz = (Convert.ToString(szazalek / sor));
                if (tulajdonresz.Length >10)
                {
                     tulajdonresz= tulajdonresz.Substring(0, 10);
                }           
            }
            if (sor != 0)
            {
                try
                {
                    for (int i = 0; i < sor; i++)
                    {
                        string vezeteknev = aSzemely.Rows[i]["vezeteknev"].ToString();
                        string utonev = aSzemely.Rows[i]["utonev"].ToString();
                        string szulDatum = aSzemely.Rows[i]["szul_datum"].ToString();
                        string szulHely = aSzemely.Rows[i]["szul_hely"].ToString();
                        string anyjaNeve = aSzemely.Rows[i]["anyja_neve"].ToString();
                        string adoazonosito = aSzemely.Rows[i]["adoazonosito"].ToString();                        
                        int nev_id = Convert.ToInt32(aSzemely.Rows[i]["szemelyID"].ToString());
                        int szervezet = Convert.ToInt32(aSzemely.Rows[i]["szervezet"].ToString());
                        ar.szemelyAdatModosit(vezeteknev, utonev, szulDatum, szulHely, anyjaNeve, adoazonosito, tulajdonresz, szervezet, nev_id);
                    }
                    ar.szemelyAdatModosit(tbVezeteknev.Text, tbUtonev.Text, tbSzulDatum.Text, tbSzulHely.Text, tbAnyNeve.Text, tbAdoazonosito.Text, tulajdonresz, szervezet_id, cegazonosito);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A módosítás sikertelen!" + ex, "Sql hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ar.dbBezar();
        }

        //Menü esemény
        private void családokcégértékAlapjánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Csaladok csaladok = new Csaladok();
            csaladok.Show();
        }    
    }
}
