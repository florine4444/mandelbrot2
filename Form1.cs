using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelgetal
{
    public partial class Form1 : Form
    {
        //declareert de variabelen die we nodig gaan hebben
        int t;
        double invoerX, invoerY, max, schaal, afstand, a, b, olda, min, nieuwX, nieuwY, geschaaldeX, geschaaldeY;
        //declareert de standaardkleuren van onze mandelbrot
        Color kleur1 = Color.Black;
        Color kleur2 = Color.White;
        Color kleur3 = Color.Black;
        Color kleur4 = Color.White;
        Color kleur5 = Color.Black;


        //creëert de voorbeeldplaatjes
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //deze variabele neemt de waarde van het geselecteerde item aan
            var itemstring = (string)listBox1.SelectedItem;
            //door middel van if-statements checken we welke waarde de variabele heeft gekregen en worden de voorbeeldplaatjes gegenereerd
            if (itemstring == "vuur")
            {
                textBox1.Text = "-324";
                textBox3.Text = "3080";
                textBox2.Text = "0,0003125";
                textBox4.Text = "100";
                kleur1 = Color.Red;
                kleur2 = Color.Orange;
                kleur3 = Color.DarkOrange;
                kleur4 = Color.SandyBrown;
                kleur5 = Color.OrangeRed;
            }
            if (itemstring == "ster")
            {
                textBox1.Text = "-361768";
                textBox3.Text = "-8530";
                textBox2.Text = "4,8828125E-06";
                textBox4.Text = "100";
                kleur1 = Color.White;
                kleur2 = Color.LightYellow;
                kleur3 = Color.Wheat;
                kleur4 = Color.DarkOrange;
                kleur5 = Color.DarkOrange;
            }
            if (itemstring == "salamander")
            {
                textBox1.Text = "-35006";
                textBox3.Text = "-170942";
                textBox2.Text = "4,8828125E-06";
                textBox4.Text = "100";
                kleur1 = Color.Blue;
                kleur2 = Color.ForestGreen;
                kleur3 = Color.DarkGreen;
                kleur4 = Color.DarkSeaGreen;
                kleur5 = Color.DarkOliveGreen;
            }
            if (itemstring == "basis")
            {
                textBox1.Text = "0";
                textBox3.Text = "0";
                textBox2.Text = "0,01";
                textBox4.Text = "100";
                kleur1 = Color.Black;
                kleur2 = Color.White;
                kleur3 = Color.Black;
                kleur4 = Color.White;
                kleur5 = Color.Black;
            }
            //panel 1 wordt opnieuw getekend zodat het voorbeeldplaatje verschijnt
            panel1.Invalidate();
        }

        //regelt wat er gebeurt als de gebruiker op het plaatje klikt
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //x en y van het aangeklikte punt worden gebruikt om het nieuwe middelpunt te bepalen
            //hierbij wordt rekening gehouden met waar het nulpunt van panel 1 ligt
            textBox1.Text = ((int.Parse(textBox1.Text) + (e.X - panel1.Width / 2)) * 2).ToString();
            textBox3.Text = ((int.Parse(textBox3.Text) + (e.Y - panel1.Height / 2)) * 2).ToString();
            //de schaal wordt twee keer zo klein
            textBox2.Text = (schaal / 2).ToString();
            //panel 1 wordt opnieuw getekend
            panel1.Invalidate();
        }

     
        public Form1()
        {
            InitializeComponent();
            //geeft de textboxen beginwaardes
            textBox1.Text = "0";
            textBox2.Text = "0,01";
            textBox3.Text = "0";
            textBox4.Text = "100";
            //bepaalt de grootte van het form
            this.ClientSize = new Size(505, 560);

        }
        //regelt wat er gebeurt als er op de OK knop wordt gedrukt
        private void button1_Click(object sender, EventArgs e)
        {
            //panel 1 wordt opnieuw getekend
            panel1.Invalidate();
        }

        //berekent de schaal die we nodig hebben voor het tekenen van de mandelbrot
        private void Schaalberekening(double schaal)
        {
            var uitkomst = schaal * (panel1.Width / 2);
            min = 0 - uitkomst;
        }
        //hiermee gaan we de geschaalde x en y coördinaat berekenen die in de functie moeten
        private double GeschaaldeXY(double xy)
        {
            return min + (schaal * xy);
        }

        //regelt het tekenen van de Mandelbrot-figuur in het panel
        private void panel1_Paint(object sender, PaintEventArgs pea)
        {
            //de invoer van de tekstboxen worden omgezet naar variabelen
            invoerX = double.Parse(textBox1.Text);
            invoerY = double.Parse(textBox3.Text);
            schaal = double.Parse(textBox2.Text);
            max = double.Parse(textBox4.Text);
            //methode wordt opgeroepen om de schaal te berekenen
            Schaalberekening(schaal);

            //voor elke pixel op de y-as, voer de while-loop uit zodat de pixel getekend wordt
            for (var huidigY = 0; huidigY <= panel1.Height; huidigY++)
            {
                //voor elke pixel op de x-as, voer de while-loop uit zodat de pixel getekend wordt
                for (var huidigX = 0; huidigX <= panel1.Width; huidigX++)
                {
                    //reset de afstand, teller, a en b naar 0 vóór elke while-loop
                    a = 0;
                    b = 0;
                    afstand = 0;
                    t = 0;
                    //berekent de geschaalde x en y coördinaat
                    nieuwX = huidigX + invoerX;
                    nieuwY = huidigY + invoerY;
                    geschaaldeX = GeschaaldeXY(nieuwX);
                    geschaaldeY = GeschaaldeXY(nieuwY);

                    //hier gaan we de functie toepassen
                    //dit gebeurt alleen zolang de afstand onder 2 is en het aantal herhalingen van de functie onder de max is
                    while (afstand <= 2 && t < max)
                    {
                        //we slaan de oude waarde van 'a' op zodat er niet met de nieuwe waarde wordt gerekend in de formule voor 'b'
                        olda = a;
                        t++;
                        a = a * a - b * b + geschaaldeX;
                        b = 2 * olda * b + geschaaldeY;
                        afstand = Math.Sqrt(a * a + b * b);

                    }

                    //tekent de pixels
                    //voor elk standaardplaatje worden er andere kleuren gebruikt dmv de kleur variabelen
                    if (t % 5 == 0)
                    {
                        pea.Graphics.FillRectangle(new SolidBrush(kleur1), huidigX, huidigY, 1, 1);
                    }
                    else if (t % 4 == 0)
                    {
                        pea.Graphics.FillRectangle(new SolidBrush(kleur2), huidigX, huidigY, 1, 1);
                    }
                    else if (t % 3 == 0)
                    {
                        pea.Graphics.FillRectangle(new SolidBrush(kleur3), huidigX, huidigY, 1, 1);
                    }
                    else if (t % 2 == 0)
                    {
                        pea.Graphics.FillRectangle(new SolidBrush(kleur4), huidigX, huidigY, 1, 1);
                    }
                    else if (t % 1 == 0)
                    {
                        pea.Graphics.FillRectangle(new SolidBrush(kleur5), huidigX, huidigY, 1, 1);
                    }

                }
            }
        }

    }

}
