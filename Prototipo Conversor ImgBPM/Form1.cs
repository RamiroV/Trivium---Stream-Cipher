using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo_Conversor_ImgBmp
{
    public partial class Form1 : Form
    {
        byte[] key = new byte[10];
        byte[] IV = new byte[10];
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCargarImagen_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap) Image.FromFile(txtFileRute.Text);

            pictureBox1.Image = image;
            
            byte[] imgArray = image.ToByteArray(ImageFormat.Bmp);

            //claves harcodeadas
            byte[] k = { 128, 128, 128, 128, 128, 128, 128, 128, 128, 128 };
            byte[] Iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var bitList = convertToBitList(new BitArray(imgArray));

            bitList = Xor(bitList, calculateZ(bitList.Count, initialState(k, Iv)));

            //Si se muestra esto, es el mensaje cifrado, se ve ruido...
            pictureBox2.Image = convertToBitArray(bitList).ToByteArray().ToImage();

            //Luego de generar el keystream, lo descifra, haciendo nuevamente el XOR...
            bitList = Xor(bitList, calculateZ(bitList.Count, initialState(k, Iv)));

            //debería mostrar el mensaje ya descifrado, igual al anterior...
            pictureBox3.Image = convertToBitArray(bitList).ToByteArray().ToImage();

            //Este es para guardar la imagen, pero no está andando... no se porqué
            //pictureBox3.Image.Save(@"C:\Users\ramiro\Pictures\Imágenes para Probar Criptografía\res.bmp", ImageFormat.Bmp);
        }

        public List<bool> initialState(byte[] key, byte[] IV)
        {
            var keyBits = ByteToBitArrayReverse(key);
            var IVBits = ByteToBitArrayReverse(IV);

            var s = new List<bool>();

            bool t1, t2, t3;

            for(int i = 0; i < 80; i++)
                s.Add(keyBits[i]);
            for (int i = 80; i < 93; i++)
                s.Add(false);
            for(int i = 93; i < 173; i++)
                s.Add(IVBits[i - 93]);
            for (int i = 173; i < 176; i++)
                s.Add(false);
            for (int i = 176; i < 285; i++)
                s.Add(false);
            for (int i = 285; i < 288; i++)
                s.Add(true);

            for (int i = 0; i < (4 * 288); i++)
            {
                t1 = s[65] ^ (s[90] && s[91]) ^ s[92] ^ s[170];
                t2 = s[161] ^ (s[174] && s[175]) ^ s[176] ^ s[263];
                t3 = s[242] ^ (s[285] && s[286]) ^ s[287] ^ s[68];

                s = sAsignation(s, t1, t2, t3);
            }

            return s;
        }

        //Este método es el que tarda en ejecutarse más, haciendo todo más lento...
        public List<bool> sAsignation(List<bool> s, bool t1, bool t2, bool t3)
        {
            s.Insert(0, t3);

            s.RemoveAt(93);

            s.Insert(93, t1);

            s.RemoveAt(177);

            s.Insert(177, t2);

            s.RemoveAt(s.Count - 1);

            return s;
        }

        public List<bool> calculateZ(int lengthOfBitArray, List<bool> s)
        {
            var z = new List<bool>();

            bool t1, t2, t3;

            for (int i = 0; i < lengthOfBitArray; i++)
            {
                t1 = s[65] ^ s[92];
                t2 = s[161] ^ s[176];
                t3 = s[242] ^ s[287];

                z.Add(t1 ^ t2 ^ t3);

                t1 = t1 ^ (s[90] && s[91]) ^ s[170];
                t2 = t2 ^ (s[174] && s[175]) ^ s[263];
                t3 = t3 ^ (s[285] && s[286]) ^ s[68];

                s = sAsignation(s, t1, t2, t3);
            }

            return z;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var myForm = new Form1();
            pictureBox1.Location = new Point(myForm.Bounds.Width / 6, myForm.Bounds.Height / 6);
            pictureBox2.Location = new Point((myForm.Bounds.Width / 6) + (myForm.Bounds.Width / 2), myForm.Bounds.Height / 6);
            pictureBox3.Location = new Point(myForm.Bounds.Width / 3, (myForm.Bounds.Height / 6) + (myForm.Bounds.Height / 2));
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileRute.Text = openFileDialog1.FileName;
                btnCifrarYDescifrar.Enabled = true;
            }
        }

        //Antes usaba las extensiones de BitArray y ByteArray para hacerlo mejor,
        //pero al agregar las listas, los métodos los dejé acá :(
        public List<bool> convertToBitList(BitArray bits)
        {
            var bitList = new List<bool>();

            foreach (bool bit in bits)
                bitList.Add(bit);

            return bitList;
        }

        public BitArray convertToBitArray(List<bool> bitList)
        {
            var bits = new BitArray(bitList.Count);

            for (int i = 0; i < bitList.Count; i++)
                bits[i] = bitList[i];

            return bits;
        }

        public string ByteArrayToHexaString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public BitArray ByteToBitArrayReverse(byte[] ba)
        {
            BitArray bitArray = new BitArray(ba);

            for (int b = 0; b < ba.Length; b += 8)
            {
                for (int i = 0; i < 4; i++)
                {
                    var aux = bitArray[i + b];
                    bitArray[i + b] = bitArray[7 - i + b];
                    bitArray[7 - i + b] = aux;
                }
            }

            return bitArray;
        }

        public List<bool> Xor(List<bool> aBitList, List<bool> anotherBitList)
        {
            var resultBitList = new List<bool>();
            //Le puse esto para que no me calcule la cabecera del .bmp
            //Sería mejor hacerlo desde afuera, no pasándole esos bits y así dejar el XOR genérico
            var firstBitOfDataInBMPFormat = (54 * 8);

            for (int i = 0; i < firstBitOfDataInBMPFormat; i++)
                resultBitList.Add(aBitList[i]);
            for (int i = firstBitOfDataInBMPFormat; i < aBitList.Count; i++)
                resultBitList.Add(aBitList[i] ^ anotherBitList[i]);

            return resultBitList;
        }

    }
}
