using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Hyrule_Warriors.Properties;
using System.Diagnostics;
using System.Net;

namespace Hyrule_Warriors
{
    public partial class Form1 : Form
    {

        private const uint CodeHandlerStart = 0x01133000;

        private const uint CodeHandlerEnd = 0x01134300;

        private const uint CodeHandlerEnabled = 0x10014CFC;

        private TCPGecko Gecko;

        private bool connected;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Settings.Default.IpAddress;
        }

        #region Cheats

        private enum Cheat
        {
            Magic = 1,
            Special = 2,
            KO = 3,
            MKO = 4,
            SKO = 5,
            Damage = 6,
            Rupee = 7,
            Termina = 8,
            MagicP2 = 9,
            SpecialP2 = 10,
            infMaterials = 11,
        }

        #endregion

        #region Connection to TCP Gecko

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Gecko = new TCPGecko(textBox1.Text, 7331);

                connected = Gecko.Connect();

                if (connected)
                {

                    Settings.Default.IpAddress = textBox1.Text;
                    Settings.Default.Save();


                    MessageBox.Show("Successfully connected!");
                    button1.Enabled = false;
                    button2.Enabled = true;
                    tabControl1.Enabled = true;
                    
                }
            }
            catch (ETCPGeckoException ex)
            {
                connected = false;

                MessageBox.Show(ex.Message);
            }
            catch (System.Net.Sockets.SocketException)
            {
                connected = false;

                MessageBox.Show("You've entered a wrong IP!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gecko.Disconnect();
            button1.Enabled = true;
            button2.Enabled = false;
            tabControl1.Enabled = false;
        }

        #endregion

        #region Add Cheats

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            var selected = new List<Cheat>();

            if (infMaterial.Checked == true)
            {
                selected.Add(Cheat.infMaterials);
            }

            if (Magic.Checked == true)
            {
                selected.Add(Cheat.Magic);
            }

            if (MagicP2.Checked == true)
            {
                selected.Add(Cheat.MagicP2);
            }

            if (Damage.Checked == true)
            {
                selected.Add(Cheat.Damage);
            }

            if (Special.Checked == true)
            {
                selected.Add(Cheat.Special);
            }

            if (SpecialP2.Checked == true)
            {
                selected.Add(Cheat.SpecialP2);
            }

            if (KO.Checked == true)
            {
                selected.Add(Cheat.KO);
            }

            if (MKO.Checked == true)
            {
                selected.Add(Cheat.MKO);
            }

            if (SKO.Checked == true)
            {
                selected.Add(Cheat.SKO);
            }

            if (Rupee.Checked == true)
            {
                selected.Add(Cheat.Rupee);
            }

            if (Termina.Checked == true)
            {
                selected.Add(Cheat.Termina);
            }

            SetCheats(selected);
        }

        #endregion

        #region Send added Cheats

        private void SetCheats(ICollection<Cheat> cheats)
        {

            this.Gecko.poke32(CodeHandlerEnabled, 0x00000000);
            var clear = CodeHandlerStart;
            while (clear <= CodeHandlerEnd)
            {
                this.Gecko.poke32(clear, 0x0);
                clear += 0x4;
            }

            var codes = new List<uint>();

            if (cheats.Contains(Cheat.infMaterials))
            {
                codes.Add(0x0201004E); codes.Add(0x3562A874);
                codes.Add(0x000003E7); codes.Add(0x00000002);
                codes.Add(0x00000000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.Magic))
            {
                codes.Add(0x00020000); codes.Add(0x1071390C);
                codes.Add(0x10100000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.MagicP2))
            {
                codes.Add(0x00020000); codes.Add(0x10713B34);
                codes.Add(0x10100000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.Damage))
            {
                codes.Add(0x00020000); codes.Add(0x1071387C);
                codes.Add(0x00000000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.Special))
            {
                codes.Add(0x00020000); codes.Add(0x107138FC);
                codes.Add(0x03030000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.SpecialP2))
            {
                codes.Add(0x00020000); codes.Add(0x10713B24);
                codes.Add(0x03030000); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.KO))
            {
                codes.Add(0x00020000); codes.Add(0x10713880);
                codes.Add(0x0000238E); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.MKO))
            {
                codes.Add(0x00020000); codes.Add(0x1071388C);
                codes.Add(0x0000238E); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.SKO))
            {
                codes.Add(0x00020000); codes.Add(0x10713894);
                codes.Add(0x0000238E); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.Rupee))
            {
                codes.Add(0x00020000); codes.Add(0x35616C94);
                codes.Add(0x0098967F); codes.Add(0x00000000);
            }

            if (cheats.Contains(Cheat.Termina))
            {
                codes.Add(0x00020000); codes.Add(0x35637404);
                codes.Add(0x48000000); codes.Add(0x00000000);
            }

            var address = CodeHandlerStart;

            foreach (var code in codes)
            {
                this.Gecko.poke32(address, code);
                address += 0x4;
            }

            this.Gecko.poke32(CodeHandlerEnabled, 0x00000001);
        }

        #endregion

        #region Codes

        private void button3enable(object sender, EventArgs e)
        {
            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e) // Material Icons
        {
            Gecko.poke32(0x3562A974, 0xFFFFFFFF);
            Gecko.poke32(0x3562A978, 0xFFFFFFFF);
            Gecko.poke32(0x3562A97C, 0xFF1F0000);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x35616D18, 0x0003FFFF);
            Gecko.poke32(0x35616D24, 0x000001F0);
            Gecko.poke32(0x35616D40, 0x0003FFFF);
            Gecko.poke32(0x35616D4C, 0x000001F0);
            Gecko.poke32(0x35616D68, 0x0003FFFF);
            Gecko.poke32(0x35616D74, 0x000001F0);
            Gecko.poke32(0x35616D90, 0x0003FFFF);
            Gecko.poke32(0x35616D9C, 0x000001F0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x3562AD34, 0x06060606);
            Gecko.poke32(0x3562AD38, 0x06060606);
            Gecko.poke32(0x3562AD3C, 0x06060606);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x3562FD8C, 0x06060606);
            Gecko.poke32(0x3562FD90, 0x06060606);
            Gecko.poke32(0x3562FD94, 0x06060606);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x356325B8, 0x06000000);
            Gecko.poke32(0x356325BC, 0x00060600);
            Gecko.poke32(0x356325C4, 0x06060606);
            Gecko.poke32(0x356325C8, 0x06060606);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x35634DE4, 0x06060000);
            Gecko.poke32(0x35634DE8, 0x00000006);
            Gecko.poke32(0x35634DF8, 0x06060606);
            Gecko.poke32(0x35634DFC, 0x06060606);
        }

        private void Termina_CheckedChanged(object sender, EventArgs e)
        {
            if (Termina.Checked)
                button3.Enabled = true;

            else
                button3.Enabled = true;
        }

        private void bossbattle_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This requires the 'Boss Pack' DLC\nDo you have it installed?", "DLC Request", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Unlocked all Challenges");
                Gecko.poke32(0x35616D30, 0xFFFFFFFF);
                Gecko.poke32(0x35616D34, 0x0000FFFF);
            }

            else if (dialogResult == DialogResult.No)
            {
                // nothing
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x3563EC98, 0xFFFFFFFF);
            Gecko.poke32(0x3563EC9C, 0xFFFFFFFF);
            Gecko.poke32(0x3563ECA0, 0xFFFFFFFF);
            Gecko.poke32(0x3563ECA4, 0xFFFFFFFF);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x3562AA64, 0x000003E8);
            Gecko.poke32(0x3562AA74, 0x000003E8);
            Gecko.poke32(0x3562AA84, 0x0000000A);
            Gecko.poke32(0x3562AA94, 0x000001F4);
            Gecko.poke32(0x3562AAA4, 0x00000063);
            Gecko.poke32(0x3562AA2C, 0x000186A0);
            Gecko.poke32(0x3562AA5C, 0x000F4240);
            Gecko.poke32(0x3562AA6C, 0x000001F4);
            Gecko.poke32(0x3562AA7C, 0x00000064);
            Gecko.poke32(0x3562AA8C, 0x00000064);
            Gecko.poke32(0x3562AA9C, 0x19267230);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Gecko.poke32(0x3562AA14, 0x0000FFFF);
            Gecko.poke32(0x3562AA18, 0x3FFFF000);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x387740FF, Convert.ToByte(numericUpDown1.Value));
            Gecko.poke08(0x388311FF, Convert.ToByte(numericUpDown1.Value));
        }

        public void adventure(uint address, string mode)
        {
            switch (mode)
            {
                case "Link":
                    Gecko.poke08(address, 0x00);
                    break;

                case "Zelda":
                    Gecko.poke08(address, 0x01);
                    break;

                case "Shiek":
                    Gecko.poke08(address, 0x02);
                    break;

                case "Impa":
                    Gecko.poke08(address, 0x03);
                    break;

                case "Ganondorf":
                    Gecko.poke08(address, 0x04);
                    break;

                case "Darunia":
                    Gecko.poke08(address, 0x05);
                    break;

                case "Ruto":
                    Gecko.poke08(address, 0x06);
                    break;

                case "Agitha":
                    Gecko.poke08(address, 0x07);
                    break;

                case "Midna":
                    Gecko.poke08(address, 0x08);
                    break;

                case "Fi":
                    Gecko.poke08(address, 0x09);
                    break;

                case "Ghirahim":
                    Gecko.poke08(address, 0x0A);
                    break;

                case "Zant":
                    Gecko.poke08(address, 0x0B);
                    break;

                case "Lana":
                    Gecko.poke08(address, 0x0D);
                    break;

                case "Cia":
                    Gecko.poke08(address, 0x2C);
                    break;

                case "Volga":
                    Gecko.poke08(address, 0x2D);
                    break;

                case "Wizzro":
                    Gecko.poke08(address, 0x2E);
                    break;

                case "Twilight Midna":
                    Gecko.poke08(address, 0x2F);
                    break;

                case "Young Link":
                    Gecko.poke08(address, 0x30);
                    break;

                case "Tingle":
                    Gecko.poke08(address, 0x31);
                    break;

                case "Linkle":
                    Gecko.poke08(address, 0x34);
                    break;

                case "Skull Kid":
                    Gecko.poke08(address, 0x35);
                    break;

                case "Toon-Link":
                    Gecko.poke08(address, 0x36);
                    break;

                case "Tetra":
                    Gecko.poke08(address, 0x37);
                    break;

                case "King Daphnes":
                    Gecko.poke08(address, 0x38);
                    break;

                case "Medli":
                    Gecko.poke08(address, 0x39);
                    break;

                case "Marin":
                    Gecko.poke08(address, 0x3A);
                    break;

                case "Toon-Zelda":
                    Gecko.poke08(address, 0x3B);
                    break;

                case "Ravio":
                    Gecko.poke08(address, 0x3C);
                    break;

                case "Yuga":
                    Gecko.poke08(address, 0x3D);
                    break;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            adventure(0x3868D163, comboBox1.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MessageBox.Show("------Adventure Mode------\nIf you are in the Character select screen,\nchoose a Character from the Dropdownlist.\nPress 'Set' and then don't select any Character in Hyrule Warriors!\nJust press 'A' after hitting 'Set' and then\nthe Weapons of your chosen Character should appear.\nMake sure that you only use Characters that you own,\nbecause you can't start the Round without a Weapon!\n\n------Legend/Free Mode------\nIn the Character Select screen, choose any Character from the Dropdownlist,\nthen press 'Set' and then press 'A' until the Round starts.\n");
        }

        public void legend(uint address, string mode)
        {
            switch (mode)
            {
                case "Link":
                    Gecko.poke08(address, 0x00);
                    break;

                case "Zelda":
                    Gecko.poke08(address, 0x01);
                    break;

                case "Shiek":
                    Gecko.poke08(address, 0x02);
                    break;

                case "Impa":
                    Gecko.poke08(address, 0x03);
                    break;

                case "Ganondorf":
                    Gecko.poke08(address, 0x04);
                    break;

                case "Darunia":
                    Gecko.poke08(address, 0x05);
                    break;

                case "Ruto":
                    Gecko.poke08(address, 0x06);
                    break;

                case "Agitha":
                    Gecko.poke08(address, 0x07);
                    break;

                case "Midna":
                    Gecko.poke08(address, 0x08);
                    break;

                case "Fi":
                    Gecko.poke08(address, 0x09);
                    break;

                case "Ghirahim":
                    Gecko.poke08(address, 0x0A);
                    break;

                case "Zant":
                    Gecko.poke08(address, 0x0B);
                    break;

                case "Lana":
                    Gecko.poke08(address, 0x0D);
                    break;

                case "Cia":
                    Gecko.poke08(address, 0x2C);
                    break;

                case "Volga":
                    Gecko.poke08(address, 0x2D);
                    break;

                case "Wizzro":
                    Gecko.poke08(address, 0x2E);
                    break;

                case "Twilight Midna":
                    Gecko.poke08(address, 0x2F);
                    break;

                case "Young Link":
                    Gecko.poke08(address, 0x30);
                    break;

                case "Tingle":
                    Gecko.poke08(address, 0x31);
                    break;

                case "Linkle":
                    Gecko.poke08(address, 0x34);
                    break;

                case "Skull Kid":
                    Gecko.poke08(address, 0x35);
                    break;

                case "Toon-Link":
                    Gecko.poke08(address, 0x36);
                    break;

                case "Tetra":
                    Gecko.poke08(address, 0x37);
                    break;

                case "King Daphnes":
                    Gecko.poke08(address, 0x38);
                    break;

                case "Medli":
                    Gecko.poke08(address, 0x39);
                    break;

                case "Marin":
                    Gecko.poke08(address, 0x3A);
                    break;

                case "Toon-Zelda":
                    Gecko.poke08(address, 0x3B);
                    break;

                case "Ravio":
                    Gecko.poke08(address, 0x3C);
                    break;

                case "Yuga":
                    Gecko.poke08(address, 0x3D);
                    break;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            legend(0x35617217, comboBox2.Text);
        }

        private void button17_Click(object sender, EventArgs e) // complete Map
        {
            DialogResult dialogResult = MessageBox.Show("This will unlock all Squares in evey Map, are you sure you want to activate it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                Gecko.poke32(0x3562AB34, 0x00000003);
                Gecko.poke32(0x3562AB38, 0x00000003);
                Gecko.poke32(0x3562AB3C, 0x00000003);
                Gecko.poke32(0x3562AB40, 0x00000003);
                Gecko.poke32(0x3562AB44, 0x00000003);
                Gecko.poke32(0x3562AB48, 0x00000003);
                Gecko.poke32(0x3562AB4C, 0x00000003);
                Gecko.poke32(0x3562AB50, 0x00000003);
                Gecko.poke32(0x3562AB54, 0x00000003);
                Gecko.poke32(0x3562AB58, 0x00000003);
                Gecko.poke32(0x3562AB5C, 0x00000003);
                Gecko.poke32(0x3562AB60, 0x00000003);
                Gecko.poke32(0x3562AB64, 0x00000003);
                Gecko.poke32(0x3562AB68, 0x00000003);
                Gecko.poke32(0x3562AB6C, 0x00000003);
                Gecko.poke32(0x3562AB70, 0x00000003);
                Gecko.poke32(0x3562AB74, 0x00000003);
                Gecko.poke32(0x3562AB78, 0x00000003);
                Gecko.poke32(0x3562AB7C, 0x00000003);
                Gecko.poke32(0x3562AB80, 0x00000003);
                Gecko.poke32(0x3562AB84, 0x00000003);
                Gecko.poke32(0x3562AB88, 0x00000003);
                Gecko.poke32(0x3562AB8C, 0x00000003);
                Gecko.poke32(0x3562AB90, 0x00000003);
                Gecko.poke32(0x3562AB94, 0x00000003);
                Gecko.poke32(0x3562AB98, 0x00000003);
                Gecko.poke32(0x3562AB9C, 0x00000003);
                Gecko.poke32(0x3562ABA0, 0x00000003);
                Gecko.poke32(0x3562ABA4, 0x00000003);
                Gecko.poke32(0x3562ABA8, 0x00000003);
                Gecko.poke32(0x3562ABAC, 0x00000003);
                Gecko.poke32(0x3562ABB0, 0x00000003);
                Gecko.poke32(0x3562ABB4, 0x00000003);
                Gecko.poke32(0x3562ABB8, 0x00000003);
                Gecko.poke32(0x3562ABBC, 0x00000003);
                Gecko.poke32(0x3562ABC0, 0x00000003);
                Gecko.poke32(0x3562ABC4, 0x00000003);
                Gecko.poke32(0x3562ABC8, 0x00000003);
                Gecko.poke32(0x3562ABCC, 0x00000003);
                Gecko.poke32(0x3562ABD0, 0x00000003);
                Gecko.poke32(0x3562ABD4, 0x00000003);
                Gecko.poke32(0x3562ABD8, 0x00000003);
                Gecko.poke32(0x3562ABDC, 0x00000003);
                Gecko.poke32(0x3562ABE0, 0x00000003);
                Gecko.poke32(0x3562ABE4, 0x00000003);
                Gecko.poke32(0x3562ABE8, 0x00000003);
                Gecko.poke32(0x3562ABEC, 0x00000003);
                Gecko.poke32(0x3562ABF0, 0x00000003);
                Gecko.poke32(0x3562ABF4, 0x00000003);
                Gecko.poke32(0x3562ABF8, 0x00000003);
                Gecko.poke32(0x3562ABFC, 0x00000003);
                Gecko.poke32(0x3562AC00, 0x00000003);
                Gecko.poke32(0x3562AC04, 0x00000003);
                Gecko.poke32(0x3562AC08, 0x00000003);
                Gecko.poke32(0x3562AC0C, 0x00000003);
                Gecko.poke32(0x3562AC10, 0x00000003);
                Gecko.poke32(0x3562AC14, 0x00000003);
                Gecko.poke32(0x3562AC18, 0x00000003);
                Gecko.poke32(0x3562AC1C, 0x00000003);
                Gecko.poke32(0x3562AC20, 0x00000003);
                Gecko.poke32(0x3562AC24, 0x00000003);
                Gecko.poke32(0x3562AC28, 0x00000003);
                Gecko.poke32(0x3562AC2C, 0x00000003);
                Gecko.poke32(0x3562AC30, 0x00000003);
                Gecko.poke32(0x3562AC34, 0x00000003);
                Gecko.poke32(0x3562AC38, 0x00000003);
                Gecko.poke32(0x3562AC3C, 0x00000003);
                Gecko.poke32(0x3562AC40, 0x00000003);
                Gecko.poke32(0x3562AC44, 0x00000003);
                Gecko.poke32(0x3562AC48, 0x00000003);
                Gecko.poke32(0x3562AC4C, 0x00000003);
                Gecko.poke32(0x3562AC50, 0x00000003);
                Gecko.poke32(0x3562AC54, 0x00000003);
                Gecko.poke32(0x3562AC58, 0x00000003);
                Gecko.poke32(0x3562AC5C, 0x00000003);
                Gecko.poke32(0x3562AC60, 0x00000003);
                Gecko.poke32(0x3562AC64, 0x00000003);
                Gecko.poke32(0x3562AC68, 0x00000003);
                Gecko.poke32(0x3562AC6C, 0x00000003);
                Gecko.poke32(0x3562AC70, 0x00000003);
                Gecko.poke32(0x3562AC74, 0x00000003);
                Gecko.poke32(0x3562AC78, 0x00000003);
                Gecko.poke32(0x3562AC7C, 0x00000003);
                Gecko.poke32(0x3562AC80, 0x00000003);
                Gecko.poke32(0x3562AC84, 0x00000003);
                Gecko.poke32(0x3562AC88, 0x00000003);
                Gecko.poke32(0x3562AC8C, 0x00000003);
                Gecko.poke32(0x3562AC90, 0x00000003);
                Gecko.poke32(0x3562AC94, 0x00000003);
                Gecko.poke32(0x3562AC98, 0x00000003);
                Gecko.poke32(0x3562AC9C, 0x00000003);
                Gecko.poke32(0x3562ACA0, 0x00000003);
                Gecko.poke32(0x3562ACA4, 0x00000003);
                Gecko.poke32(0x3562ACA8, 0x00000003);
                Gecko.poke32(0x3562ACAC, 0x00000003);
                Gecko.poke32(0x3562ACB0, 0x00000003);
                Gecko.poke32(0x3562ACB4, 0x00000003);
                Gecko.poke32(0x3562ACB8, 0x00000003);
                Gecko.poke32(0x3562ACBC, 0x00000003);
                Gecko.poke32(0x3562ACC0, 0x00000003);
                Gecko.poke32(0x3562ACC4, 0x00000003);
                Gecko.poke32(0x3562ACC8, 0x00000003);
                Gecko.poke32(0x3562ACCC, 0x00000003);
                Gecko.poke32(0x3562ACD0, 0x00000003);
                Gecko.poke32(0x3562ACD4, 0x00000003);
                Gecko.poke32(0x3562ACD8, 0x00000003);
                Gecko.poke32(0x3562ACDC, 0x00000003);
                Gecko.poke32(0x3562ACE0, 0x00000003);
                Gecko.poke32(0x3562ACE4, 0x00000003);
                Gecko.poke32(0x3562ACE8, 0x00000003);
                Gecko.poke32(0x3562ACEC, 0x00000003);
                Gecko.poke32(0x3562ACF0, 0x00000003);
                Gecko.poke32(0x3562ACF4, 0x00000003);
                Gecko.poke32(0x3562ACF8, 0x00000003);
                Gecko.poke32(0x3562ACFC, 0x00000003);
                Gecko.poke32(0x3562AD00, 0x00000003);
                Gecko.poke32(0x3562AD04, 0x00000003);
                Gecko.poke32(0x3562AD08, 0x00000003);
                Gecko.poke32(0x3562AD0C, 0x00000003);
                Gecko.poke32(0x3562AD10, 0x00000002);
                Gecko.poke32(0x3562AD14, 0x00000003);
                Gecko.poke32(0x3562AD18, 0x00000003);
                Gecko.poke32(0x3562AD1C, 0x00000003);
                Gecko.poke32(0x3562AD20, 0x00000003);
                Gecko.poke32(0x3562AD24, 0x00000003);
                Gecko.poke32(0x3562AD28, 0x00000003);
                Gecko.poke32(0x3562AD2C, 0x00000003);
                Gecko.poke32(0x3562AD30, 0x00000003);
                Gecko.poke32(0x3562D440, 0x00000003);
                Gecko.poke32(0x3562D47C, 0x00000003);
                Gecko.poke32(0x3562D480, 0x00000003);
                Gecko.poke32(0x3562D484, 0x00000003);
                Gecko.poke32(0x3562D4B8, 0x00000003);
                Gecko.poke32(0x3562D4BC, 0x00000003);
                Gecko.poke32(0x3562D4C0, 0x00000003);
                Gecko.poke32(0x3562D4C4, 0x00000003);
                Gecko.poke32(0x3562D4C8, 0x00000003);
                Gecko.poke32(0x3562D4FC, 0x00000003);
                Gecko.poke32(0x3562D500, 0x00000003);
                Gecko.poke32(0x3562D504, 0x00000003);
                Gecko.poke32(0x3562D540, 0x00000003);
                Gecko.poke32(0x3562FB8C, 0x00000003);
                Gecko.poke32(0x3562FB90, 0x00000003);
                Gecko.poke32(0x3562FB94, 0x00000003);
                Gecko.poke32(0x3562FB98, 0x00000003);
                Gecko.poke32(0x3562FB9C, 0x00000003);
                Gecko.poke32(0x3562FBA0, 0x00000003);
                Gecko.poke32(0x3562FBA4, 0x00000003);
                Gecko.poke32(0x3562FBA8, 0x00000003);
                Gecko.poke32(0x3562FBAC, 0x00000003);
                Gecko.poke32(0x3562FBB0, 0x00000003);
                Gecko.poke32(0x3562FBB4, 0x00000003);
                Gecko.poke32(0x3562FBB8, 0x00000003);
                Gecko.poke32(0x3562FBBC, 0x00000003);
                Gecko.poke32(0x3562FBC0, 0x00000003);
                Gecko.poke32(0x3562FBC4, 0x00000003);
                Gecko.poke32(0x3562FBC8, 0x00000003);
                Gecko.poke32(0x3562FBCC, 0x00000003);
                Gecko.poke32(0x3562FBD0, 0x00000003);
                Gecko.poke32(0x3562FBD4, 0x00000003);
                Gecko.poke32(0x3562FBD8, 0x00000003);
                Gecko.poke32(0x3562FBDC, 0x00000003);
                Gecko.poke32(0x3562FBE0, 0x00000003);
                Gecko.poke32(0x3562FBE4, 0x00000003);
                Gecko.poke32(0x3562FBE8, 0x00000003);
                Gecko.poke32(0x3562FBEC, 0x00000003);
                Gecko.poke32(0x3562FBF0, 0x00000003);
                Gecko.poke32(0x3562FBF4, 0x00000003);
                Gecko.poke32(0x3562FBF8, 0x00000003);
                Gecko.poke32(0x3562FBFC, 0x00000003);
                Gecko.poke32(0x3562FC00, 0x00000003);
                Gecko.poke32(0x3562FC04, 0x00000003);
                Gecko.poke32(0x3562FC08, 0x00000003);
                Gecko.poke32(0x3562FC0C, 0x00000003);
                Gecko.poke32(0x3562FC10, 0x00000003);
                Gecko.poke32(0x3562FC14, 0x00000003);
                Gecko.poke32(0x3562FC18, 0x00000003);
                Gecko.poke32(0x3562FC1C, 0x00000003);
                Gecko.poke32(0x3562FC20, 0x00000003);
                Gecko.poke32(0x3562FC24, 0x00000003);
                Gecko.poke32(0x3562FC28, 0x00000003);
                Gecko.poke32(0x3562FC2C, 0x00000003);
                Gecko.poke32(0x3562FC30, 0x00000003);
                Gecko.poke32(0x3562FC34, 0x00000003);
                Gecko.poke32(0x3562FC38, 0x00000003);
                Gecko.poke32(0x3562FC3C, 0x00000003);
                Gecko.poke32(0x3562FC40, 0x00000003);
                Gecko.poke32(0x3562FC44, 0x00000003);
                Gecko.poke32(0x3562FC48, 0x00000003);
                Gecko.poke32(0x3562FC4C, 0x00000003);
                Gecko.poke32(0x3562FC50, 0x00000003);
                Gecko.poke32(0x3562FC54, 0x00000003);
                Gecko.poke32(0x3562FC58, 0x00000003);
                Gecko.poke32(0x3562FC5C, 0x00000003);
                Gecko.poke32(0x3562FC60, 0x00000003);
                Gecko.poke32(0x3562FC64, 0x00000003);
                Gecko.poke32(0x3562FC68, 0x00000003);
                Gecko.poke32(0x3562FC6C, 0x00000003);
                Gecko.poke32(0x3562FC70, 0x00000003);
                Gecko.poke32(0x3562FC74, 0x00000003);
                Gecko.poke32(0x3562FC78, 0x00000003);
                Gecko.poke32(0x3562FC7C, 0x00000003);
                Gecko.poke32(0x3562FC80, 0x00000003);
                Gecko.poke32(0x3562FC84, 0x00000003);
                Gecko.poke32(0x3562FC88, 0x00000003);
                Gecko.poke32(0x3562FC8C, 0x00000003);
                Gecko.poke32(0x3562FC90, 0x00000003);
                Gecko.poke32(0x3562FC94, 0x00000003);
                Gecko.poke32(0x3562FC98, 0x00000003);
                Gecko.poke32(0x3562FC9C, 0x00000003);
                Gecko.poke32(0x3562FCA0, 0x00000003);
                Gecko.poke32(0x3562FCA4, 0x00000003);
                Gecko.poke32(0x3562FCA8, 0x00000003);
                Gecko.poke32(0x3562FCAC, 0x00000003);
                Gecko.poke32(0x3562FCB0, 0x00000003);
                Gecko.poke32(0x3562FCB4, 0x00000003);
                Gecko.poke32(0x3562FCB8, 0x00000003);
                Gecko.poke32(0x3562FCBC, 0x00000003);
                Gecko.poke32(0x3562FCC0, 0x00000003);
                Gecko.poke32(0x3562FCC4, 0x00000003);
                Gecko.poke32(0x3562FCC8, 0x00000003);
                Gecko.poke32(0x3562FCCC, 0x00000003);
                Gecko.poke32(0x3562FCD0, 0x00000003);
                Gecko.poke32(0x3562FCD4, 0x00000003);
                Gecko.poke32(0x3562FCD8, 0x00000003);
                Gecko.poke32(0x3562FCDC, 0x00000003);
                Gecko.poke32(0x3562FCE0, 0x00000003);
                Gecko.poke32(0x3562FCE4, 0x00000003);
                Gecko.poke32(0x3562FCE8, 0x00000003);
                Gecko.poke32(0x3562FCEC, 0x00000003);
                Gecko.poke32(0x3562FCF0, 0x00000003);
                Gecko.poke32(0x3562FCF4, 0x00000003);
                Gecko.poke32(0x3562FCF8, 0x00000003);
                Gecko.poke32(0x3562FCFC, 0x00000003);
                Gecko.poke32(0x3562FD00, 0x00000003);
                Gecko.poke32(0x3562FD04, 0x00000003);
                Gecko.poke32(0x3562FD08, 0x00000003);
                Gecko.poke32(0x3562FD0C, 0x00000003);
                Gecko.poke32(0x3562FD10, 0x00000003);
                Gecko.poke32(0x3562FD14, 0x00000003);
                Gecko.poke32(0x3562FD18, 0x00000003);
                Gecko.poke32(0x3562FD1C, 0x00000003);
                Gecko.poke32(0x3562FD20, 0x00000003);
                Gecko.poke32(0x3562FD24, 0x00000003);
                Gecko.poke32(0x3562FD28, 0x00000003);
                Gecko.poke32(0x3562FD2C, 0x00000003);
                Gecko.poke32(0x3562FD30, 0x00000003);
                Gecko.poke32(0x3562FD34, 0x00000003);
                Gecko.poke32(0x3562FD38, 0x00000003);
                Gecko.poke32(0x3562FD3C, 0x00000003);
                Gecko.poke32(0x3562FD40, 0x00000003);
                Gecko.poke32(0x3562FD44, 0x00000003);
                Gecko.poke32(0x3562FD48, 0x00000003);
                Gecko.poke32(0x3562FD4C, 0x00000003);
                Gecko.poke32(0x3562FD50, 0x00000003);
                Gecko.poke32(0x3562FD54, 0x00000003);
                Gecko.poke32(0x3562FD58, 0x00000003);
                Gecko.poke32(0x3562FD5C, 0x00000003);
                Gecko.poke32(0x3562FD60, 0x00000003);
                Gecko.poke32(0x3562FD64, 0x00000003);
                Gecko.poke32(0x3562FD68, 0x00000003);
                Gecko.poke32(0x3562FD6C, 0x00000003);
                Gecko.poke32(0x3562FD70, 0x00000003);
                Gecko.poke32(0x3562FD74, 0x00000003);
                Gecko.poke32(0x3562FD78, 0x00000003);
                Gecko.poke32(0x3562FD7C, 0x00000003);
                Gecko.poke32(0x3562FD80, 0x00000003);
                Gecko.poke32(0x3562FD84, 0x00000003);
                Gecko.poke32(0x3562FD88, 0x00000003);
                Gecko.poke32(0x356323C4, 0x00000003);
                Gecko.poke32(0x356323C8, 0x00000003);
                Gecko.poke32(0x356323CC, 0x00000003);
                Gecko.poke32(0x356323D0, 0x00000003);
                Gecko.poke32(0x356323D4, 0x00000003);
                Gecko.poke32(0x356323D8, 0x00000003);
                Gecko.poke32(0x356323DC, 0x00000003);
                Gecko.poke32(0x356323E0, 0x00000003);
                Gecko.poke32(0x356323E4, 0x00000003);
                Gecko.poke32(0x356323E8, 0x00000003);
                Gecko.poke32(0x356323EC, 0x00000003);
                Gecko.poke32(0x356323F0, 0x00000003);
                Gecko.poke32(0x356323F4, 0x00000003);
                Gecko.poke32(0x356323F8, 0x00000003);
                Gecko.poke32(0x356323FC, 0x00000003);
                Gecko.poke32(0x35632400, 0x00000003);
                Gecko.poke32(0x35632404, 0x00000003);
                Gecko.poke32(0x35632408, 0x00000003);
                Gecko.poke32(0x3563240C, 0x00000003);
                Gecko.poke32(0x35632410, 0x00000003);
                Gecko.poke32(0x35632414, 0x00000003);
                Gecko.poke32(0x35632418, 0x00000003);
                Gecko.poke32(0x3563241C, 0x00000003);
                Gecko.poke32(0x35632420, 0x00000003);
                Gecko.poke32(0x35632424, 0x00000003);
                Gecko.poke32(0x35632428, 0x00000003);
                Gecko.poke32(0x3563242C, 0x00000003);
                Gecko.poke32(0x35632430, 0x00000003);
                Gecko.poke32(0x35632434, 0x00000003);
                Gecko.poke32(0x35632438, 0x00000003);
                Gecko.poke32(0x3563243C, 0x00000003);
                Gecko.poke32(0x35632440, 0x00000003);
                Gecko.poke32(0x35632444, 0x00000003);
                Gecko.poke32(0x35632448, 0x00000003);
                Gecko.poke32(0x3563244C, 0x00000003);
                Gecko.poke32(0x35632450, 0x00000003);
                Gecko.poke32(0x35632454, 0x00000003);
                Gecko.poke32(0x35632458, 0x00000003);
                Gecko.poke32(0x3563245C, 0x00000003);
                Gecko.poke32(0x35632460, 0x00000003);
                Gecko.poke32(0x35632464, 0x00000003);
                Gecko.poke32(0x35632468, 0x00000003);
                Gecko.poke32(0x3563246C, 0x00000003);
                Gecko.poke32(0x35632470, 0x00000003);
                Gecko.poke32(0x35632474, 0x00000003);
                Gecko.poke32(0x35632478, 0x00000003);
                Gecko.poke32(0x3563247C, 0x00000003);
                Gecko.poke32(0x35632480, 0x00000003);
                Gecko.poke32(0x35632484, 0x00000003);
                Gecko.poke32(0x35632488, 0x00000003);
                Gecko.poke32(0x3563248C, 0x00000003);
                Gecko.poke32(0x35632490, 0x00000003);
                Gecko.poke32(0x35632494, 0x00000003);
                Gecko.poke32(0x35632498, 0x00000003);
                Gecko.poke32(0x3563249C, 0x00000003);
                Gecko.poke32(0x356324A0, 0x00000003);
                Gecko.poke32(0x356324A4, 0x00000003);
                Gecko.poke32(0x356324A8, 0x00000003);
                Gecko.poke32(0x356324AC, 0x00000003);
                Gecko.poke32(0x356324B0, 0x00000003);
                Gecko.poke32(0x356324B4, 0x00000003);
                Gecko.poke32(0x356324B8, 0x00000003);
                Gecko.poke32(0x356324BC, 0x00000003);
                Gecko.poke32(0x356324C0, 0x00000003);
                Gecko.poke32(0x356324C4, 0x00000003);
                Gecko.poke32(0x356324C8, 0x00000003);
                Gecko.poke32(0x356324CC, 0x00000003);
                Gecko.poke32(0x356324D0, 0x00000003);
                Gecko.poke32(0x356324D4, 0x00000003);
                Gecko.poke32(0x356324D8, 0x00000003);
                Gecko.poke32(0x356324DC, 0x00000003);
                Gecko.poke32(0x356324E0, 0x00000003);
                Gecko.poke32(0x356324E4, 0x00000003);
                Gecko.poke32(0x356324E8, 0x00000003);
                Gecko.poke32(0x356324EC, 0x00000003);
                Gecko.poke32(0x356324F0, 0x00000003);
                Gecko.poke32(0x356324F4, 0x00000003);
                Gecko.poke32(0x356324F8, 0x00000003);
                Gecko.poke32(0x356324FC, 0x00000003);
                Gecko.poke32(0x35632500, 0x00000003);
                Gecko.poke32(0x35632504, 0x00000003);
                Gecko.poke32(0x35632508, 0x00000003);
                Gecko.poke32(0x3563250C, 0x00000003);
                Gecko.poke32(0x35632510, 0x00000003);
                Gecko.poke32(0x35632514, 0x00000003);
                Gecko.poke32(0x35632518, 0x00000003);
                Gecko.poke32(0x3563251C, 0x00000003);
                Gecko.poke32(0x35632520, 0x00000003);
                Gecko.poke32(0x35632524, 0x00000003);
                Gecko.poke32(0x35632528, 0x00000003);
                Gecko.poke32(0x3563252C, 0x00000003);
                Gecko.poke32(0x35632530, 0x00000003);
                Gecko.poke32(0x35632534, 0x00000003);
                Gecko.poke32(0x35632538, 0x00000003);
                Gecko.poke32(0x3563253C, 0x00000003);
                Gecko.poke32(0x35632540, 0x00000003);
                Gecko.poke32(0x35632544, 0x00000003);
                Gecko.poke32(0x35632548, 0x00000003);
                Gecko.poke32(0x3563254C, 0x00000003);
                Gecko.poke32(0x35632550, 0x00000003);
                Gecko.poke32(0x35632554, 0x00000003);
                Gecko.poke32(0x35632558, 0x00000003);
                Gecko.poke32(0x3563255C, 0x00000003);
                Gecko.poke32(0x35632560, 0x00000003);
                Gecko.poke32(0x35632564, 0x00000003);
                Gecko.poke32(0x35632568, 0x00000003);
                Gecko.poke32(0x3563256C, 0x00000003);
                Gecko.poke32(0x35632570, 0x00000003);
                Gecko.poke32(0x35632574, 0x00000003);
                Gecko.poke32(0x35632578, 0x00000003);
                Gecko.poke32(0x3563257C, 0x00000003);
                Gecko.poke32(0x35632580, 0x00000003);
                Gecko.poke32(0x35632584, 0x00000003);
                Gecko.poke32(0x35632588, 0x00000003);
                Gecko.poke32(0x3563258C, 0x00000003);
                Gecko.poke32(0x35632590, 0x00000003);
                Gecko.poke32(0x35632594, 0x00000003);
                Gecko.poke32(0x35632598, 0x00000003);
                Gecko.poke32(0x3563259C, 0x00000003);
                Gecko.poke32(0x356325A0, 0x00000003);
                Gecko.poke32(0x35634BE4, 0x003C0003);
                Gecko.poke32(0x35634BE8, 0x003C0003);
                Gecko.poke32(0x35634BEC, 0x003C0003);
                Gecko.poke32(0x35634BF0, 0x003C0003);
                Gecko.poke32(0x35634BF4, 0x003C0003);
                Gecko.poke32(0x35634BF8, 0x003C0003);
                Gecko.poke32(0x35634BFC, 0x003C0003);
                Gecko.poke32(0x35634C00, 0x003C0003);
                Gecko.poke32(0x35634C04, 0x003C0003);
                Gecko.poke32(0x35634C08, 0x003C0003);
                Gecko.poke32(0x35634C0C, 0x003C0003);
                Gecko.poke32(0x35634C10, 0x003C0003);
                Gecko.poke32(0x35634C14, 0x003C0003);
                Gecko.poke32(0x35634C18, 0x003C0003);
                Gecko.poke32(0x35634C1C, 0x003C0003);
                Gecko.poke32(0x35634C20, 0x003C0003);
                Gecko.poke32(0x35634C24, 0x003C0003);
                Gecko.poke32(0x35634C28, 0x003C0003);
                Gecko.poke32(0x35634C2C, 0x003C0003);
                Gecko.poke32(0x35634C30, 0x003C0003);
                Gecko.poke32(0x35634C34, 0x003C0003);
                Gecko.poke32(0x35634C38, 0x003C0003);
                Gecko.poke32(0x35634C3C, 0x003C0003);
                Gecko.poke32(0x35634C40, 0x003C0003);
                Gecko.poke32(0x35634C44, 0x003C0003);
                Gecko.poke32(0x35634C48, 0x003C0003);
                Gecko.poke32(0x35634C4C, 0x003C0003);
                Gecko.poke32(0x35634C50, 0x003C0003);
                Gecko.poke32(0x35634C54, 0x003C0003);
                Gecko.poke32(0x35634C58, 0x003C0003);
                Gecko.poke32(0x35634C5C, 0x003C0003);
                Gecko.poke32(0x35634C60, 0x003C0003);
                Gecko.poke32(0x35634C64, 0x003C0003);
                Gecko.poke32(0x35634C68, 0x003C0003);
                Gecko.poke32(0x35634C6C, 0x003C0003);
                Gecko.poke32(0x35634C70, 0x003C0003);
                Gecko.poke32(0x35634C74, 0x003C0003);
                Gecko.poke32(0x35634C78, 0x003C0003);
                Gecko.poke32(0x35634C7C, 0x003C0003);
                Gecko.poke32(0x35634C80, 0x003C0003);
                Gecko.poke32(0x35634C84, 0x003C0003);
                Gecko.poke32(0x35634C88, 0x003C0003);
                Gecko.poke32(0x35634C8C, 0x003C0003);
                Gecko.poke32(0x35634C90, 0x003C0003);
                Gecko.poke32(0x35634C94, 0x003C0003);
                Gecko.poke32(0x35634C98, 0x003C0003);
                Gecko.poke32(0x35634C9C, 0x003C0003);
                Gecko.poke32(0x35634CA0, 0x003C0003);
                Gecko.poke32(0x35634CA4, 0x003C0003);
                Gecko.poke32(0x35634CA8, 0x003C0003);
                Gecko.poke32(0x35634CAC, 0x003C0003);
                Gecko.poke32(0x35634CB0, 0x003C0003);
                Gecko.poke32(0x35634CB4, 0x003C0003);
                Gecko.poke32(0x35634CB8, 0x003C0003);
                Gecko.poke32(0x35634CBC, 0x003C0003);
                Gecko.poke32(0x35634CC0, 0x003C0003);
                Gecko.poke32(0x35634CC4, 0x003C0003);
                Gecko.poke32(0x35634CC8, 0x003C0003);
                Gecko.poke32(0x35634CCC, 0x003C0003);
                Gecko.poke32(0x35634CD0, 0x003C0003);
                Gecko.poke32(0x35634CD4, 0x003C0003);
                Gecko.poke32(0x35634CD8, 0x003C0003);
                Gecko.poke32(0x35634CDC, 0x003C0003);
                Gecko.poke32(0x35634CE0, 0x003C0003);
                Gecko.poke32(0x35634CE4, 0x003C0003);
                Gecko.poke32(0x35634CE8, 0x003C0003);
                Gecko.poke32(0x35634CEC, 0x003C0003);
                Gecko.poke32(0x35634CF0, 0x003C0003);
                Gecko.poke32(0x35634CF4, 0x003C0003);
                Gecko.poke32(0x35634CF8, 0x003C0003);
                Gecko.poke32(0x35634CFC, 0x003C0003);
                Gecko.poke32(0x35634D00, 0x003C0003);
                Gecko.poke32(0x35634D04, 0x003C0003);
                Gecko.poke32(0x35634D08, 0x003C0003);
                Gecko.poke32(0x35634D0C, 0x003C0003);
                Gecko.poke32(0x35634D10, 0x003C0003);
                Gecko.poke32(0x35634D14, 0x003C0003);
                Gecko.poke32(0x35634D18, 0x003C0003);
                Gecko.poke32(0x35634D1C, 0x003C0003);
                Gecko.poke32(0x35634D20, 0x003C0003);
                Gecko.poke32(0x35634D24, 0x003C0003);
                Gecko.poke32(0x35634D28, 0x003C0003);
                Gecko.poke32(0x35634D2C, 0x003C0003);
                Gecko.poke32(0x35634D30, 0x003C0003);
                Gecko.poke32(0x35634D34, 0x003C0003);
                Gecko.poke32(0x35634D38, 0x003C0003);
                Gecko.poke32(0x35634D3C, 0x003C0003);
                Gecko.poke32(0x35634D40, 0x003C0003);
                Gecko.poke32(0x35634D44, 0x003C0003);
                Gecko.poke32(0x35634D48, 0x003C0003);
                Gecko.poke32(0x35634D4C, 0x003C0003);
                Gecko.poke32(0x35634D50, 0x003C0003);
                Gecko.poke32(0x35634D54, 0x003C0003);
                Gecko.poke32(0x35634D58, 0x003C0003);
                Gecko.poke32(0x35634D5C, 0x003C0003);
                Gecko.poke32(0x35634D60, 0x003C0003);
                Gecko.poke32(0x35634D64, 0x003C0003);
                Gecko.poke32(0x35634D68, 0x003C0003);
                Gecko.poke32(0x35634D6C, 0x003C0003);
                Gecko.poke32(0x35634D70, 0x003C0003);
                Gecko.poke32(0x35634D74, 0x003C0003);
                Gecko.poke32(0x35634D78, 0x003C0003);
                Gecko.poke32(0x35634D7C, 0x003C0003);
                Gecko.poke32(0x35634D80, 0x003C0003);
                Gecko.poke32(0x35634D84, 0x003C0003);
                Gecko.poke32(0x35634D88, 0x003C0003);
                Gecko.poke32(0x35634D8C, 0x003C0003);
                Gecko.poke32(0x35634D90, 0x003C0003);
                Gecko.poke32(0x35634D94, 0x003C0003);
                Gecko.poke32(0x35634D98, 0x003C0003);
                Gecko.poke32(0x35634D9C, 0x003C0003);
                Gecko.poke32(0x35634DA0, 0x003C0003);
                Gecko.poke32(0x35634DA4, 0x003C0003);
                Gecko.poke32(0x35634DA8, 0x003C0003);
                Gecko.poke32(0x35634DAC, 0x003C0003);
                Gecko.poke32(0x35634DB0, 0x003C0003);
                Gecko.poke32(0x35634DB4, 0x003C0003);
                Gecko.poke32(0x35634DB8, 0x003C0003);
                Gecko.poke32(0x35634DBC, 0x003C0003);
                Gecko.poke32(0x35634DC0, 0x003C0003);
                Gecko.poke32(0x35634DC4, 0x003C0003);
                Gecko.poke32(0x35634DC8, 0x003C0003);
                Gecko.poke32(0x35634DCC, 0x003C0003);
                Gecko.poke32(0x35634DD0, 0x003C0003);
                Gecko.poke32(0x35634DD4, 0x003C0003);
                MessageBox.Show("Unlocked all Squares");
            }

            else if (dialogResult == DialogResult.No)
            {
                // nothing
            }
        }

        #endregion

        #region WeaponType

        public void weapontype(uint address1, string mode)
        {
            switch (mode)
            {

                case "Normal weapon (blank?)":
                    Gecko.poke08(address1, 0x01);
                    break;

                case "Normal weapon (NEW)":
                    Gecko.poke08(address1, 0x02);
                    break;

                case "Normal weapon":
                    Gecko.poke08(address1, 0x03);
                    break;

                case "Master Sword (blank?)":
                    Gecko.poke08(address1, 0x09);
                    break;

                case "Master sword (NEW)":
                    Gecko.poke08(address1, 0x0A);
                    break;

                case "Master sword":
                    Gecko.poke08(address1, 0x0B);
                    break;

                case "Legendary skill (blank?)":
                    Gecko.poke08(address1, 0x11);
                    break;

                case "Legendary skill (NEW)":
                    Gecko.poke08(address1, 0x12);
                    break;

                case "Legendary skill":
                    Gecko.poke08(address1, 0x13);
                    break;
            }
        }

#endregion

        #region WeaponID

        public void weaponid(uint address1, string mode)
        {
            switch (mode)
            {
                case "00. Knight's Sword (Link - Hylian Sword Level 1)":
                    Gecko.poke32(address1, 0x00000000);
                    break;

                case "01. White Sword (Link - Hylian Sword Level 2)":
                    Gecko.poke32(address1, 0x00000001);
                    break;

                case "02. Magical Sword (Link - Hylian Sword Level 3)":
                    Gecko.poke32(address1, 0x00000002);
                    break;

                case "03. Fire Rod (Link - Magic Rod Level 1)":
                    Gecko.poke32(address1, 0x00000003);
                    break;

                case "04. Prism Rod (Link - Magic Rod Level 2)":
                    Gecko.poke32(address1, 0x00000004);
                    break;

                case "05. Magical Rod (Link - Magic Rod Level 3)":
                    Gecko.poke32(address1, 0x00000005);
                    break;

                case "06. Great Fountain Fairy (Link - Great Fairy Level 1)":
                    Gecko.poke32(address1, 0x00000006);
                    break;

                case "07. Great Forest Fairy (Link - Great Fairy Level 2)":
                    Gecko.poke32(address1, 0x00000007);
                    break;

                case "08. Great Sky Fairy (Link - Great Fairy Level 3)":
                    Gecko.poke32(address1, 0x00000008);
                    break;

                case "09. Silver Gauntlets (Link - Gauntlets Level 1)":
                    Gecko.poke32(address1, 0x00000009);
                    break;

                case "10. Golden Gauntlets (Link - Gauntlets Level 2)":
                    Gecko.poke32(address1, 0x0000000A);
                    break;

                case "11. Power Gloves (Link - Gauntlets Level 3)":
                    Gecko.poke32(address1, 0x0000000B);
                    break;

                case "12. Spirit's Tome (Lana - Book of Sorcery Level 1)":
                    Gecko.poke32(address1, 0x0000000C);
                    break;

                case "13. Sealing Tome (Lana - Book of Sorcery Level 2)":
                    Gecko.poke32(address1, 0x0000000D);
                    break;

                case "14. Sorceress Tome (Lana - Book of Sorcery Level 3)":
                    Gecko.poke32(address1, 0x0000000E);
                    break;

                case "15. Deku Spear (Lana - Spear Level 1)":
                    Gecko.poke32(address1, 0x0000000F);
                    break;

                case "16. Kokiri Spear (Lana - Spear Level 2)":
                    Gecko.poke32(address1, 0x00000010);
                    break;

                case "17. Faron Spear (Lana - Spear Level 3)":
                    Gecko.poke32(address1, 0x00000011);
                    break;

                case "18. Gate of Time (Lana - Summoning Gate Level 1)":
                    Gecko.poke32(address1, 0x00000012);
                    break;

                case "19. Guardian's Gate (Lana - Summoning Gate Level 2)":
                    Gecko.poke32(address1, 0x00000013);
                    break;

                case "20. Gate of Souls (Lana - Summoning Gate Level 3)":
                    Gecko.poke32(address1, 0x00000014);
                    break;

                case "21. Polished Rapier (Zelda - Rapier Level 1)":
                    Gecko.poke32(address1, 0x00000015);
                    break;

                case "22. Glittering Rapier (Zelda - Rapier Level 2)":
                    Gecko.poke32(address1, 0x00000016);
                    break;

                case "23. Gleaming Rapier (Zelda - Rapier Level 3)":
                    Gecko.poke32(address1, 0x00000017);
                    break;

                case "24. Wind Waker (Zelda - Baton Level 1)":
                    Gecko.poke32(address1, 0x00000018);
                    break;

                case "25. Sacred Baton (Zelda - Baton Level 2)":
                    Gecko.poke32(address1, 0x00000019);
                    break;

                case "26. Glorious Baton (Zelda - Baton Level 3)":
                    Gecko.poke32(address1, 0x0000001A);
                    break;

                case "27. Giant's Knife (Impa - Giant Blade Level 1)":
                    Gecko.poke32(address1, 0x0000001B);
                    break;

                case "28. Biggoron's Knife (Impa - Giant Blade Level 2)":
                    Gecko.poke32(address1, 0x0000001C);
                    break;

                case "29. Biggoron's Sword (Impa - Giant Blade Level 3)":
                    Gecko.poke32(address1, 0x0000001D);
                    break;

                case "30. Guardian Naginata (Impa - Naginata Level 1)":
                    Gecko.poke32(address1, 0x0000001E);
                    break;

                case "31. Scorching Naginata (Impa - Naginata Level 2)":
                    Gecko.poke32(address1, 0x0000001F);
                    break;

                case "32. Sheikah Naginata (Impa - Naginata Level 3)":
                    Gecko.poke32(address1, 0x00000020);
                    break;

                case "33. Swords of Despair (Ganondorf - Great Swords Level 1)":
                    Gecko.poke32(address1, 0x00000021);
                    break;

                case "34. Swords of Darkness (Ganondorf - Great Swords Level 2)":
                    Gecko.poke32(address1, 0x00000022);
                    break;

                case "35. Swords of Demise (Ganondorf - Great Swords Level 3)":
                    Gecko.poke32(address1, 0x00000023);
                    break;

                case "36. Goddess's Harp (Sheik - Harp Level 1)":
                    Gecko.poke32(address1, 0x00000024);
                    break;

                case "37. Typhoon Harp (Sheik - Harp Level 2)":
                    Gecko.poke32(address1, 0x00000025);
                    break;

                case "38. Triforce Harp (Sheik - Harp Level 3)":
                    Gecko.poke32(address1, 0x00000026);
                    break;

                case "39. Magic Hammer (Darunia - Hammer Level 1)":
                    Gecko.poke32(address1, 0x00000027);
                    break;

                case "40. Igneous Hammer (Darunia - Hammer Level 2)":
                    Gecko.poke32(address1, 0x00000028);
                    break;

                case "41. Megaton Hammer (Darunia - Hammer Level 3)":
                    Gecko.poke32(address1, 0x00000029);
                    break;

                case "42. Silver Scale (Ruto - Zora Scale Level 1)":
                    Gecko.poke32(address1, 0x0000002A);
                    break;

                case "43. Golden Scale (Ruto - Zora Scale Level 2)":
                    Gecko.poke32(address1, 0x0000002B);
                    break;

                case "44. Water Dragon Scale (Ruto - Zora Scale Level 3)":
                    Gecko.poke32(address1, 0x0000002C);
                    break;

                case "45. Butterfly Parasol (Agitha - Parasol Level 1)":
                    Gecko.poke32(address1, 0x0000002D);
                    break;

                case "46. Luna Parasol (Agitha - Parasol Level 2)":
                    Gecko.poke32(address1, 0x0000002E);
                    break;

                case "47. Princess Parasol (Agitha - Parasol Level 3)":
                    Gecko.poke32(address1, 0x0000002F);
                    break;

                case "48. Cursed Shackle (Midna - Shackle Level 1)":
                    Gecko.poke32(address1, 0x00000030);
                    break;

                case "49. Twilight Shackle (Midna - Shackle Level 2)":
                    Gecko.poke32(address1, 0x00000031);
                    break;

                case "50. Sol Shackle (Midna - Shackle Level 3)":
                    Gecko.poke32(address1, 0x00000032);
                    break;

                case "51. Goddess Sword (Fi - Goddess Blade Level 1)":
                    Gecko.poke32(address1, 0x00000033);
                    break;

                case "52. Goddess Longsword (Fi - Goddess Blade Level 2)":
                    Gecko.poke32(address1, 0x00000034);
                    break;

                case "53. True Goddess Blade (Fi - Goddess Blade Level 3)":
                    Gecko.poke32(address1, 0x00000035);
                    break;

                case "54. Demon Tribe Sword (Ghirahim - Demon Blade Level 1)":
                    Gecko.poke32(address1, 0x00000036);
                    break;

                case "55. Demon Longsword (Ghirahim - Demon Blade Level 2)":
                    Gecko.poke32(address1, 0x00000037);
                    break;

                case "56. True Demon Blade (Ghirahim - Demon Blade Level 3)":
                    Gecko.poke32(address1, 0x00000038);
                    break;

                case "57. Usurper's Scimitars (Zant - Scimitars Level 1)":
                    Gecko.poke32(address1, 0x00000039);
                    break;

                case "58. Shadow Scimitars (Zant - Scimitars Level 2)":
                    Gecko.poke32(address1, 0x0000003A);
                    break;

                case "59. Scimitars of Twilight (Zant - Scimitars Level 3)":
                    Gecko.poke32(address1, 0x0000003B);
                    break;

                case "60. [unknown weapon]":
                    Gecko.poke32(address1, 0x0000003C);
                    break;

                case "61. Master Sword (Link - Master Sword)":
                    Gecko.poke32(address1, 0x0000003D);
                    break;

                case "62. 8-Bit Wooden Sword (Link - Hylian Sword Level 3)":
                    Gecko.poke32(address1, 0x0000003E);
                    break;

                case "63. 8-Bit Candle (Link - Magic Rod Level 3)":
                    Gecko.poke32(address1, 0x0000003F);
                    break;

                case "64. 8-Bit Fairy (Link - Great Fairy Level 3)":
                    Gecko.poke32(address1, 0x00000040);
                    break;

                case "65. 8-Bit Power Bracelets (Link - Gauntlets Level 3)":
                    Gecko.poke32(address1, 0x00000041);
                    break;

                case "66. 8-Bit Book of Magic (Lana - Book of Sorcery Level 3)":
                    Gecko.poke32(address1, 0x00000042);
                    break;

                case "67. 8-Bit Magical Rod? (Lana - Spear Level 3)":
                    Gecko.poke32(address1, 0x00000043);
                    break;

                case "68. 8-Bit Compass (Lana - Summoning Gate Level 3)":
                    Gecko.poke32(address1, 0x00000044);
                    break;

                case "69. 8-Bit White Sword? (Zelda - Rapier Level 3)":
                    Gecko.poke32(address1, 0x00000045);
                    break;

                case "70. 8-Bit Recorder (Zelda - Baton Level 3)":
                    Gecko.poke32(address1, 0x00000046);
                    break;

                case "71. 8-Bit Boomerang? (Impa - Giant Blade Level 3)":
                    Gecko.poke32(address1, 0x00000047);
                    break;

                case "72. 8-Bit Magical Sword? (Impa - Naginata Level 3)":
                    Gecko.poke32(address1, 0x00000048);
                    break;

                case "73. 8-Bit Magical Keys (Ganondorf - Great Swords Level 3)":
                    Gecko.poke32(address1, 0x00000049);
                    break;

                case "74. 8-Bit Stepladder (Sheik - Harp Level 3)":
                    Gecko.poke32(address1, 0x0000004A);
                    break;

                case "75. 8-Bit Food (Darunia - Hammer Level 3)":
                    Gecko.poke32(address1, 0x0000004B);
                    break;

                case "76. 8-Bit Clock (Ruto - Zora Scale Level 3)":
                    Gecko.poke32(address1, 0x0000004C);
                    break;

                case "77. 8-Bit Rupee (Agitha - Parasol Level 3)":
                    Gecko.poke32(address1, 0x0000004D);
                    break;

                case "78. 8-Bit Red Ring (Midna - Shackle Level 3)":
                    Gecko.poke32(address1, 0x0000004E);
                    break;

                case "79. 8-Bit Silver Arrow (Fi - Goddess Blade Level 3)":
                    Gecko.poke32(address1, 0x0000004F);
                    break;

                case "80. 8-Bit Arrow (Ghirahim - Demon Blade Level 3)":
                    Gecko.poke32(address1, 0x00000050);
                    break;

                case "81. 8-Bit Magic Boomerangs (Zant - Scimitars Level 3)":
                    Gecko.poke32(address1, 0x00000051);
                    break;

                case "82. Scepter of Time (Cia - Scepter Level 1)":
                    Gecko.poke32(address1, 0x00000052);
                    break;

                case "83. Guardian's Scepter (Cia - Scepter Level 2)":
                    Gecko.poke32(address1, 0x00000053);
                    break;

                case "84. Scepter of Souls (Cia - Scepter Level 3)":
                    Gecko.poke32(address1, 0x00000054);
                    break;

                case "85. Dragonbone Pike (Volga - Dragon Spear Level 1)":
                    Gecko.poke32(address1, 0x00000055);
                    break;

                case "86. Stonecleaver Claw (Volga - Dragon Spear Level 2)":
                    Gecko.poke32(address1, 0x00000056);
                    break;

                case "87. Flesh-Render Fang (Volga - Dragon Spear Level 3)":
                    Gecko.poke32(address1, 0x00000057);
                    break;

                case "88. Blue Ring (Wizzro - Ring Level 1)":
                    Gecko.poke32(address1, 0x00000058);
                    break;

                case "89. Red Ring (Wizzro - Ring Level 2)":
                    Gecko.poke32(address1, 0x00000059);
                    break;

                case "90. Magical Ring (Wizzro - Ring Level 3)":
                    Gecko.poke32(address1, 0x0000005A);
                    break;

                case "91. Epona (Link - Horse Level 1)":
                    Gecko.poke32(address1, 0x0000005B);
                    break;

                case "92. Twilight Epona (Link - Horse Level 2)":
                    Gecko.poke32(address1, 0x0000005C);
                    break;

                case "93. Epona of Time (Link - Horse Level 3)":
                    Gecko.poke32(address1, 0x0000005D);
                    break;

                case "94. Mirror of Shadows (Twili Midna - Mirror Level 1)":
                    Gecko.poke32(address1, 0x0000005E);
                    break;

                case "95. Mirror of Silence (Twili Midna - Mirror Level 2)":
                    Gecko.poke32(address1, 0x0000005F);
                    break;

                case "96. Mirror of Twilight (Twili Midna - Mirror Level 3)":
                    Gecko.poke32(address1, 0x00000060);
                    break;

                case "97. Ancient Spinner (Link - Spinner Level 1)":
                    Gecko.poke32(address1, 0x00000061);
                    break;

                case "98. Enhanced Spinner (Link - Spinner Level 2)":
                    Gecko.poke32(address1, 0x00000062);
                    break;

                case "99. Triforce Spinner (Link - Spinner Level 3)":
                    Gecko.poke32(address1, 0x00000063);
                    break;

                case "100. Old Dominion Rod (Zelda - Dominion Rod Level 1)":
                    Gecko.poke32(address1, 0x00000064);
                    break;

                case "101. High Dominion Rod (Zelda - Dominion Rod Level 2)":
                    Gecko.poke32(address1, 0x00000065);
                    break;

                case "102. Royal Dominion Rod (Zelda - Dominion Rod Level 3)":
                    Gecko.poke32(address1, 0x00000066);
                    break;

                case "103. Fierce Deity Mask (Young Link - Mask Level 1)":
                    Gecko.poke32(address1, 0x00000067);
                    break;

                case "104. Furious Deity Mask (Young Link - Mask Level 2)":
                    Gecko.poke32(address1, 0x00000068);
                    break;

                case "105. Vengeful Deity Mask (Young Link - Mask Level 3)":
                    Gecko.poke32(address1, 0x00000069);
                    break;

                case "106. Rosy Balloon (Tingle - Balloon Level 1)":
                    Gecko.poke32(address1, 0x0000006A);
                    break;

                case "107. Love-Filled Balloon (Tingle - Balloon Level 2)":
                    Gecko.poke32(address1, 0x0000006B);
                    break;

                case "108. Mr. Fairy Balloon (Tingle - Balloon Level 3)":
                    Gecko.poke32(address1, 0x0000006C);
                    break;

                case "109. *** Ganon's Rage (Ganon) RESERVED, DO NOT USE ***":
                    Gecko.poke32(address1, 0x0000006D);
                    break;

                case "110. *** Cucco's Spirit (Cucco) RESERVED, DO NOT USE ***":
                    Gecko.poke32(address1, 0x0000006E);
                    break;

                case "111. Thief's Trident (Ganondorf - Trident Level 1)":
                    Gecko.poke32(address1, 0x0000006F);
                    break;

                case "112. King of Evil Trident (Ganondorf - Trident Level 2)":
                    Gecko.poke32(address1, 0x00000070);
                    break;

                case "113. Trident of Demise (Ganondorf - Trident Level 3)":
                    Gecko.poke32(address1, 0x00000071);
                    break;

                case "114. Simple Crossbows (Linkle - Crossbows Level 1)":
                    Gecko.poke32(address1, 0x00000072);
                    break;

                case "115. Hylian Crossbows (Linkle - Crossbows Level 2)":
                    Gecko.poke32(address1, 0x00000073);
                    break;

                case "116. Legend's Crossbows (Linkle - Crossbows Level 3)":
                    Gecko.poke32(address1, 0x00000074);
                    break;

                case "117. Fairy Ocarina (Skull Kid - Ocarina Level 1)":
                    Gecko.poke32(address1, 0x00000075);
                    break;

                case "118. Lunar Ocarina (Skull Kid - Ocarina Level 2)":
                    Gecko.poke32(address1, 0x00000076);
                    break;

                case "119. Majora's Ocarina (Skull Kid - Ocarina Level 3)":
                    Gecko.poke32(address1, 0x00000077);
                    break;

                case "120. Hero's Sword (Toon Link - Light Sword Level 1)":
                    Gecko.poke32(address1, 0x00000078);
                    break;

                case "121. Phantom Sword (Toon Link - Light Sword Level 2)":
                    Gecko.poke32(address1, 0x00000079);
                    break;

                case "122. Lokomo Sword (Toon Link - Light Sword Level 3)":
                    Gecko.poke32(address1, 0x0000007A);
                    break;
                
                case "123. Pirate Cutlass (Tetra - Cutlass Level 1)":
                    Gecko.poke32(address1, 0x0000007B);
                    break;

                case "124. Jeweled Cutlass (Tetra - Cutlass Level 2)":
                    Gecko.poke32(address1, 0x0000007C);
                    break;

                case "125. Regal Cutlass (Tetra - Cutlass Level 3)":
                    Gecko.poke32(address1, 0x0000007D);
                    break;

                case "126. Windfall Sail (King Daphnes - Sail Level 1)":
                    Gecko.poke32(address1, 0x0000007E);
                    break;

                case "127. Swift Sail (King Daphnes - Sail Level 2)":
                    Gecko.poke32(address1, 0x0000007F);
                    break;

                case "128. Sail of Red Lions (King Daphnes - Sail Level 3)":
                    Gecko.poke32(address1, 0x00000080);
                    break;

                case "129. Sacred Harp (Medli - Rito Harp Level 1)":
                    Gecko.poke32(address1, 0x00000081);
                    break;

                case "130. Earth God's Harp (Medli - Rito Harp Level 2)":
                    Gecko.poke32(address1, 0x00000082);
                    break;

                case "131. Din's Harp (Medli - Rito Harp Level 3)":
                    Gecko.poke32(address1, 0x00000083);
                    break;

                case "132. Sea Lily's Bell (Marin - Bell Level 1)":
                    Gecko.poke32(address1, 0x00000084);
                    break;

                case "133. Wavelet Bell (Marin - Bell Level 2)":
                    Gecko.poke32(address1, 0x00000085);
                    break;

                case "134. Awakening Bell (Marin - Bell Level 3)":
                    Gecko.poke32(address1, 0x00000086);
                    break;

                case "135. Winged Boots (Linkle - Boots Level 1)":
                    Gecko.poke32(address1, 0x00000087);
                    break;

                case "136. Roc Boots (Linkle - Boots Level 2)":
                    Gecko.poke32(address1, 0x00000088);
                    break;

                case "137. Pegasus Boots (Linkle - Boots Level 3)":
                    Gecko.poke32(address1, 0x00000089);
                    break;

                case "138. Protector Sword (Toon Zelda - Sword Level 1)":
                    Gecko.poke32(address1, 0x0000008A);
                    break;

                case "139. Warp Sword (Toon Zelda - Sword Level 2)":
                    Gecko.poke32(address1, 0x0000008B);
                    break;

                case "140. Wrecker Sword (Toon Zelda - Sword Level 3)":
                    Gecko.poke32(address1, 0x0000008C);
                    break;

                case "141. Sand Wand (Toon Link - Wand Level 1)":
                    Gecko.poke32(address1, 0x0000008D);
                    break;

                case "142. Jeweled Sand Wand (Toon Link - Wand Level 2)":
                    Gecko.poke32(address1, 0x0000008E);
                    break;

                case "143. Nice Sand Wand (Toon Link - Wand Level 3)":
                    Gecko.poke32(address1, 0x0000008F);
                    break;

                case "144. Wooden Hammer (Ravio - Rental Hammer Level 1)":
                    Gecko.poke32(address1, 0x00000090);
                    break;

                case "145. White Bunny Hammer (Ravio - Rental Hammer Level 2)":
                    Gecko.poke32(address1, 0x00000091);
                    break;

                case "146. Nice Hammer (Ravio - Rental Hammer Level 3)":
                    Gecko.poke32(address1, 0x00000092);
                    break;

                case "147. Wooden Frame (Yuga - Picture Frame Level 1)":
                    Gecko.poke32(address1, 0x00000093);
                    break;

                case "148. Frame of Sealing (Yuga - Picture Frame Level 2)":
                    Gecko.poke32(address1, 0x00000094);
                    break;

                case "149. Demon King's Frame (Yuga - Picture Frame Level 3)":
                    Gecko.poke32(address1, 0x00000095);
                    break;

                case "150. Unknown (Unk - Unk Level 1)":
                    Gecko.poke32(address1, 0x00000096);
                    break;

                case "151. Unknown (Unk - Unk Level 2)":
                    Gecko.poke32(address1, 0x00000097);
                    break;

                case "152. Unknown (Unk - Unk Level 3)":
                    Gecko.poke32(address1, 0x00000098);
                    break;
            }
        }

        #endregion

        #region Basepower

        public void basepower(uint address1, string mode)
        {
            switch (mode)
            {
                case "Level 1 (80)":
                    Gecko.poke16(address1, 0x0050);
                    break;

                case "Level 2 (150)":
                    Gecko.poke16(address1, 0x0096);
                    break;

                case "Level 3 (280)":
                    Gecko.poke16(address1, 0x0118);
                    break;

                case "Master Sword (300)":
                    Gecko.poke16(address1, 0x012C);
                    break;

                case "999":
                    Gecko.poke16(address1, 0x03E7);
                    break;
            }
        }

        #endregion

        #region Stars

        public void stars(uint address1, string mode)
        {
            switch (mode)
            {
                case "0":
                    Gecko.poke08(address1, 0x00);
                    break;

                case "1":
                    Gecko.poke08(address1, 0x01);
                    break;

                case "2":
                    Gecko.poke08(address1, 0x02);
                    break;

                case "3":
                    Gecko.poke08(address1, 0x03);
                    break;

                case "4":
                    Gecko.poke08(address1, 0x04);
                    break;

                case "5":
                    Gecko.poke08(address1, 0x05);
                    break;
            }
        }

        #endregion

        #region Skills

        public void skills(uint address1, string mode)
        {
            switch (mode)
            {
                case "Nothing":
                    Gecko.poke32(address1, 0xFFFFFFFF);
                    break;

                case "Strong Att.+":
                    Gecko.poke32(address1, 0x00000001);
                    break;

                case "Strength II":
                    Gecko.poke32(address1, 0x00000002);
                    break;

                case "Strength III":
                    Gecko.poke32(address1, 0x00000003);
                    break;

                case "Strength IV":
                    Gecko.poke32(address1, 0x00000004);
                    break;

                case "Strength V":
                    Gecko.poke32(address1, 0x00000005);
                    break;

                case "Strength VI":
                    Gecko.poke32(address1, 0x00000006);
                    break;

                case "Fire+":
                    Gecko.poke32(address1, 0x00000007);
                    break;

                case "Water+":
                    Gecko.poke32(address1, 0x00000008);
                    break;

                case "Lightning+":
                    Gecko.poke32(address1, 0x00000009);
                    break;

                case "Light+":
                    Gecko.poke32(address1, 0x0000000A);
                    break;

                case "Darkness+":
                    Gecko.poke32(address1, 0x0000000B);
                    break;

                case "VS Legend":
                    Gecko.poke32(address1, 0x0000000C);
                    break;

                case "VS Time":
                    Gecko.poke32(address1, 0x0000000D);
                    break;

                case "VS Twilight":
                    Gecko.poke32(address1, 0x0000000E);
                    break;

                case "VS Skyward":
                    Gecko.poke32(address1, 0x0000000F);
                    break;

                case "VS Sorceress":
                    Gecko.poke32(address1, 0x00000010);
                    break;

                case "VS Beast":
                    Gecko.poke32(address1, 0x00000011);
                    break;

                case "VS Dragon":
                    Gecko.poke32(address1, 0x00000012);
                    break;

                case "VS Undead":
                    Gecko.poke32(address1, 0x00000013);
                    break;

                case "VS Soldier":
                    Gecko.poke32(address1, 0x00000014);
                    break;

                case "VS Ganon":
                    Gecko.poke32(address1, 0x00000015);
                    break;

                case "EXP+":
                    Gecko.poke32(address1, 0x00000016);
                    break;

                case "Rupees+":
                    Gecko.poke32(address1, 0x00000017);
                    break;

                case "Materials+":
                    Gecko.poke32(address1, 0x00000018);
                    break;

                case "Slots+":
                    Gecko.poke32(address1, 0x00000019);
                    break;

                case "Stars+":
                    Gecko.poke32(address1, 0x0000001A);
                    break;

                case "Hearts+":
                    Gecko.poke32(address1, 0x0000001B);
                    break;

                case "Health+":
                    Gecko.poke32(address1, 0x0000001C);
                    break;

                case "Special+":
                    Gecko.poke32(address1, 0x0000001D);
                    break;

                case "Bombs+":
                    Gecko.poke32(address1, 0x0000001E);
                    break;

                case "Arrows+":
                    Gecko.poke32(address1, 0x0000001F);
                    break;

                case "Boomerang+":
                    Gecko.poke32(address1, 0x00000020);
                    break;

                case "Hookshot+":
                    Gecko.poke32(address1, 0x00000021);
                    break;

                case "One-Hit Kill":
                    Gecko.poke32(address1, 0x00000022);
                    break;

                case "Sturdy Feet":
                    Gecko.poke32(address1, 0x00000023);
                    break;

                case "Regen":
                    Gecko.poke32(address1, 0x00000024);
                    break;

                case "Defenseless":
                    Gecko.poke32(address1, 0x00000025);
                    break;

                case "No Healing":
                    Gecko.poke32(address1, 0x00000026);
                    break;

                case "Adversity":
                    Gecko.poke32(address1, 0x00000027);
                    break;

                case "Compatriot":
                    Gecko.poke32(address1, 0x00000028);
                    break;

                case "Evil's Bane":
                    Gecko.poke32(address1, 0x00000029);
                    break;

                case "Legendary":
                    Gecko.poke32(address1, 0x0000002A);
                    break;

                case "Special Attack+":
                    Gecko.poke32(address1, 0x0000002B);
                    break;

                case "Finishing Blow+":
                    Gecko.poke32(address1, 0x0000002C);
                    break;

                case "Regular Attack+":
                    Gecko.poke32(address1, 0x0000002D);
                    break;

                case "Heart-strong":
                    Gecko.poke32(address1, 0x0000002E);
                    break;

                case "Focus Spirit+":
                    Gecko.poke32(address1, 0x0000002F);
                    break;

                case "Hasty Attacks":
                    Gecko.poke32(address1, 0x00000030);
                    break;
            }
        }

        #endregion

        #region Write2Slot

        private void button17_Click_2(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF594));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 1 = 356FF590
                // Difference between each slot: +9C
                weapontype(0x356FF590, comboBox3.Text);
                weaponid(0x356FF594, comboBox4.Text);
                basepower(0x356FF598, comboBox5.Text);
                stars(0x356FF59B, comboBox6.Text);
                skills(0x356FF59C, comboBox7.Text);
                skills(0x356FF5A0, comboBox8.Text);
                skills(0x356FF5A4, comboBox9.Text);
                skills(0x356FF5A8, comboBox10.Text);
                skills(0x356FF5AC, comboBox11.Text);
                skills(0x356FF5B0, comboBox12.Text);
                skills(0x356FF5B4, comboBox13.Text);
                skills(0x356FF5B8, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF630));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 2 = 356FF62C
                // Difference between each slot: +9C
                weapontype(0x356FF62C, comboBox3.Text);
                weaponid(0x356FF630, comboBox4.Text);
                basepower(0x356FF634, comboBox5.Text);
                stars(0x356FF637, comboBox6.Text);
                skills(0x356FF638, comboBox7.Text);
                skills(0x356FF63C, comboBox8.Text);
                skills(0x356FF640, comboBox9.Text);
                skills(0x356FF644, comboBox10.Text);
                skills(0x356FF648, comboBox11.Text);
                skills(0x356FF64C, comboBox12.Text);
                skills(0x356FF650, comboBox13.Text);
                skills(0x356FF654, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF6CC));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 3 = 356FF6C8
                // Difference between each slot: +9C
                weapontype(0x356FF6C8, comboBox3.Text);
                weaponid(0x356FF6CC, comboBox4.Text);
                basepower(0x356FF6D0, comboBox5.Text);
                stars(0x356FF6D3, comboBox6.Text);
                skills(0x356FF6D4, comboBox7.Text);
                skills(0x356FF6D8, comboBox8.Text);
                skills(0x356FF6DC, comboBox9.Text);
                skills(0x356FF6E0, comboBox10.Text);
                skills(0x356FF6E4, comboBox11.Text);
                skills(0x356FF6E8, comboBox12.Text);
                skills(0x356FF6EC, comboBox13.Text);
                skills(0x356FF6F0, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF768));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 4 = 356FF764
                // Difference between each slot: +9C
                weapontype(0x356FF764, comboBox3.Text);
                weaponid(0x356FF768, comboBox4.Text);
                basepower(0x356FF76C, comboBox5.Text);
                stars(0x356FF76F, comboBox6.Text);
                skills(0x356FF770, comboBox7.Text);
                skills(0x356FF774, comboBox8.Text);
                skills(0x356FF778, comboBox9.Text);
                skills(0x356FF77C, comboBox10.Text);
                skills(0x356FF780, comboBox11.Text);
                skills(0x356FF784, comboBox12.Text);
                skills(0x356FF788, comboBox13.Text);
                skills(0x356FF78C, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF804));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 5 = 356FF800
                // Difference between each slot: +9C
                weapontype(0x356FF800, comboBox3.Text);
                weaponid(0x356FF804, comboBox4.Text);
                basepower(0x356FF808, comboBox5.Text);
                stars(0x356FF80B, comboBox6.Text);
                skills(0x356FF80C, comboBox7.Text);
                skills(0x356FF810, comboBox8.Text);
                skills(0x356FF814, comboBox9.Text);
                skills(0x356FF818, comboBox10.Text);
                skills(0x356FF81C, comboBox11.Text);
                skills(0x356FF820, comboBox12.Text);
                skills(0x356FF824, comboBox13.Text);
                skills(0x356FF828, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF8A0));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 6 = 356FF89C
                // Difference between each slot: +9C
                weapontype(0x356FF89C, comboBox3.Text);
                weaponid(0x356FF8A0, comboBox4.Text);
                basepower(0x356FF8A4, comboBox5.Text);
                stars(0x356FF8A7, comboBox6.Text);
                skills(0x356FF8A8, comboBox7.Text);
                skills(0x356FF8AC, comboBox8.Text);
                skills(0x356FF8B0, comboBox9.Text);
                skills(0x356FF8B4, comboBox10.Text);
                skills(0x356FF8B8, comboBox11.Text);
                skills(0x356FF8BC, comboBox12.Text);
                skills(0x356FF8C0, comboBox13.Text);
                skills(0x356FF8C4, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF93C));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 7 = 356FF938
                // Difference between each slot: +9C
                weapontype(0x356FF938, comboBox3.Text);
                weaponid(0x356FF93C, comboBox4.Text);
                basepower(0x356FF940, comboBox5.Text);
                stars(0x356FF943, comboBox6.Text);
                skills(0x356FF944, comboBox7.Text);
                skills(0x356FF948, comboBox8.Text);
                skills(0x356FF94C, comboBox9.Text);
                skills(0x356FF950, comboBox10.Text);
                skills(0x356FF954, comboBox11.Text);
                skills(0x356FF958, comboBox12.Text);
                skills(0x356FF95C, comboBox13.Text);
                skills(0x356FF960, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FF9D8));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("This isn't actually Slot 308. This is Slot 313 because Slot 308 crashes the game.\n\nCreating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 8 = 356FFCE0
                // Difference between each slot: +9C
                weapontype(0x356FFCE0, comboBox3.Text);
                weaponid(0x356FFCE4, comboBox4.Text);
                basepower(0x356FFCE8, comboBox5.Text);
                stars(0x356FFCEB, comboBox6.Text);
                skills(0x356FFCEC, comboBox7.Text);
                skills(0x356FFCF0, comboBox8.Text);
                skills(0x356FFCF4, comboBox9.Text);
                skills(0x356FFCF8, comboBox10.Text);
                skills(0x356FFCFC, comboBox11.Text);
                skills(0x356FFD00, comboBox12.Text);
                skills(0x356FFD04, comboBox13.Text);
                skills(0x356FFD08, comboBox14.Text);
                //
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FFA74));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 9 = 356FFA70
                // Difference between each slot: +9C
                weapontype(0x356FFA70, comboBox3.Text);
                weaponid(0x356FFA74, comboBox4.Text);
                basepower(0x356FFA78, comboBox5.Text);
                stars(0x356FFA7B, comboBox6.Text);
                skills(0x356FFA7C, comboBox7.Text);
                skills(0x356FFA80, comboBox8.Text);
                skills(0x356FFA84, comboBox9.Text);
                skills(0x356FFA88, comboBox10.Text);
                skills(0x356FFA8C, comboBox11.Text);
                skills(0x356FFA90, comboBox12.Text);
                skills(0x356FFA94, comboBox13.Text);
                skills(0x356FFA98, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FFB10));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 10 = 356FFB0C
                // Difference between each slot: +9C
                weapontype(0x356FFB0C, comboBox3.Text);
                weaponid(0x356FFB10, comboBox4.Text);
                basepower(0x356FFB14, comboBox5.Text);
                stars(0x356FFB17, comboBox6.Text);
                skills(0x356FFB18, comboBox7.Text);
                skills(0x356FFB1C, comboBox8.Text);
                skills(0x356FFB20, comboBox9.Text);
                skills(0x356FFB24, comboBox10.Text);
                skills(0x356FFB28, comboBox11.Text);
                skills(0x356FFB2C, comboBox12.Text);
                skills(0x356FFB30, comboBox13.Text);
                skills(0x356FFB34, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FFBAC));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 11 = 356FFBA8
                // Difference between each slot: +9C
                weapontype(0x356FFBA8, comboBox3.Text);
                weaponid(0x356FFBAC, comboBox4.Text);
                basepower(0x356FFBB0, comboBox5.Text);
                stars(0x356FFBB3, comboBox6.Text);
                skills(0x356FFBB4, comboBox7.Text);
                skills(0x356FFBB8, comboBox8.Text);
                skills(0x356FFBBC, comboBox9.Text);
                skills(0x356FFBC0, comboBox10.Text);
                skills(0x356FFBC4, comboBox11.Text);
                skills(0x356FFBC8, comboBox12.Text);
                skills(0x356FFBCC, comboBox13.Text);
                skills(0x356FFBD0, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            var weaponidx = Convert.ToString(Gecko.peek(0x356FFC48));
            if (weaponidx.Contains("4294967295"))
            {
                weaponidx = "No Weapon on this Slot";
            }

            DialogResult dialogResult = MessageBox.Show("Creating this weapon might overwrite an existing weapon!\n\nReplacing: " + weaponidx + "\n\nContinue anyway ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Slot 12 = 356FFC44
                // Difference between each slot: +9C
                weapontype(0x356FFC44, comboBox3.Text);
                weaponid(0x356FFC48, comboBox4.Text);
                basepower(0x356FFC4C, comboBox5.Text);
                stars(0x356FFC4F, comboBox6.Text);
                skills(0x356FFC50, comboBox7.Text);
                skills(0x356FFC54, comboBox8.Text);
                skills(0x356FFC58, comboBox9.Text);
                skills(0x356FFC5C, comboBox10.Text);
                skills(0x356FFC60, comboBox11.Text);
                skills(0x356FFC64, comboBox12.Text);
                skills(0x356FFC68, comboBox13.Text);
                skills(0x356FFC6C, comboBox14.Text);
            }
            else if (dialogResult == DialogResult.No)
            {
                //Do nothing
            }
        }

        #endregion

        #region Trash

        private void button18_Click(object sender, EventArgs e)
        {
            if (button18.Text == "Enable Weapon Slot editing")
            {
                button18.Text = "Disable Weapon Slot editing";
                this.Size = new System.Drawing.Size(488, 446);
                tabControl1.Size = new System.Drawing.Size(455, 291);
                button18.BackColor = System.Drawing.Color.GreenYellow;
                button3.Location = new Point(12, 379);
                label1.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                comboBox5.Visible = true;
                comboBox6.Visible = true;
                comboBox7.Visible = true;
                comboBox8.Visible = true;
                comboBox9.Visible = true;
                comboBox10.Visible = true;
                comboBox11.Visible = true;
                comboBox12.Visible = true;
                comboBox13.Visible = true;
                comboBox14.Visible = true;
                groupBox7.Visible = true;
            }

            else if (button18.Text == "Disable Weapon Slot editing")
            {
                button18.Text = "Enable Weapon Slot editing";
                this.Size = new System.Drawing.Size(370, 339);
                tabControl1.Size = new System.Drawing.Size(332, 182);
                button3.Location = new Point(12, 270);
                button18.BackColor = System.Drawing.Color.Transparent;
                label1.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                comboBox5.Visible = false;
                comboBox6.Visible = false;
                comboBox7.Visible = false;
                comboBox8.Visible = false;
                comboBox9.Visible = false;
                comboBox10.Visible = false;
                comboBox11.Visible = false;
                comboBox12.Visible = false;
                comboBox13.Visible = false;
                comboBox14.Visible = false;
                groupBox7.Visible = false;
            }
        }

        #endregion

        #region Menu Strip

        private void gBAtempToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/");
        }

        private void dekiraiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/dekirai.393668/");
        }

        private void pikaArcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/pikaarc.396014/");
        }

        private void steamboy0x7E0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/steamboy0x7e0.404952/");
        }

        private void jaguar9400ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/jaguar9400.366384/");
        }

        private void wolfsnakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/wolf-snake.156732/");
        }

        private void gamePilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/gamepil.399819/");
        }

        private void missingNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/missing-number.16713/");
        }

        private void ingameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void weaponSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void characterSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void trainingDojoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void miscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        #endregion
    }
}
