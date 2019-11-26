using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conceptos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Seleccione archivo Conceptos del TIEMPO",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //textBox1.Text = openFileDialog1.FileName;
                try
                {
                    textBox1.Text = File.ReadAllText(openFileDialog1.FileName, Encoding.ASCII);
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puedo abrir el archivo, verifique que exista y no este en uso.");
                    throw;
                }
                if (openFileDialog1.FileName.Contains("_subirAfip"))
                {
                    MessageBox.Show("ERROR: El archivo seleccionado aparentemente es el resultado de una conversión.");
                }
                else
                {
                    
                    try
                    {
                        string encabezado;
                        encabezado = "Código AFIP;Descripción;Código contribuyente;Descripción;Marca repetible;Aportes SIPA;Contribuciones SIPA;Aportes INSSJyP;Contribuciones INSSJyP;Aportes obra social;Contribuciones obra social;Aportes FSR;Contribuciones FSR;Aportes RENATEA;Contribuciones RENATEA;Contribuciones AAFF;Contribuciones FNE;Contribuciones LRT;Aportes diferenciales;Aportes especiales" + "\r\n";
                        string convertido;
                        string convertido2 = "";
                        convertido = encabezado;

                        string lines;
                        lines = textBox1.Text;

                        string[] array = lines.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                        foreach (String item in array)
                        {
                            convertido = convertido + item.Substring(0, 6);     // codigo afip
                            convertido = convertido + ";";
                            convertido = convertido + item.Substring(16, 20).Trim();   // texto concepto afip
                            convertido = convertido + ";";
                            convertido = convertido + item.Substring(0, 9);     // codigo tiempo
                            convertido = convertido + ";";
                            convertido = convertido + item.Substring(16, 20).Trim();   // texto concepto tiempo
                            convertido = convertido + ";";
                            convertido = convertido + item.Substring(166, 20).Trim().Replace(" ", "");   // nomeclarura

                            convertido = convertido + "\r\n";
                            //######################################################
                            convertido2 = convertido2 + item.Substring(0, 6);     // codigo afip
                            convertido2 = convertido2 + (item.Substring(0, 9) + string.Concat(Enumerable.Repeat(" ", 9))).Substring(0, 10);     // codigo tiempo
                            convertido2 = convertido2 + (item.Substring(16, 20).Trim() + string.Concat(Enumerable.Repeat(" ", 150))).Substring(0, 150);    // texto concepto tiempo
                            convertido2 = convertido2 + item.Substring(166, 20).Trim() + string.Concat(Enumerable.Repeat(" ", 9));   // nomeclarura

                            convertido2 = convertido2 + "\r\n";

                            Console.WriteLine(item);

                        }
                        textBox2.Text = convertido;
                        textBox3.Text = convertido2;
                        try
                        {
                            File.WriteAllText(openFileDialog1.FileName.ToUpper().Replace(".TXT", "_subirAfip.txt"), textBox3.Text);
                            MessageBox.Show("Convesión Exitosa, busque el nuevo archivo en la misma ubicacion que el archivo de origen.");

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("ATENCION: Conversión exitosa, no se pudo generar nuevo archivo txt.");
                            throw;
                        }



                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error en la conversión, verifique que el formato es el correcto.");
                        throw;
                    }

                }

                
            }
        }
    }
}
