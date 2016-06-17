using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Prototipo_Conversor_ImgBmp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;


namespace UnitTestTrivium
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestVEctorsSet2VectorNum0Stream0to63()
        {
            string result = 
                "FBE0BF265859051B517A2E4E239FC97F" + 
                "563203161907CF2DE7A8790FA1B2E9CD" + 
                "F75292030268B7382B4C1A759AA2599A" + 
                "285549986E74805903801A4CB5A5D4F2";

            byte[] key = new byte[10]; //0s
            byte[] IV = new byte[10]; //0s

            byte[] input = new byte[512]; //0s

            Form1 program = new Form1();

            List<bool> z = program.calculateZ(input.Length, program.initialState(key, IV));

            BitArray ba = program.convertToBitArray(z);
            byte[] byteArray = ba.ToByteArray();
            string hexaAllZ = program.ByteArrayToHexaString(byteArray);

            string hexaZTested = "";

            for (int i = 0; i < 128; i++)
                hexaZTested += hexaAllZ[i];

            Assert.AreEqual(result, hexaZTested);
        }

        [TestMethod]
        public void TestVEctorsSet1VectorNum72Stream0to63()
        {
            string result =
                "5D492E77F8FE62D769C6A142056BE936" +
                "1FA0ADD8A54601DE615EBC04C4F8B2C1" +
                "2A8ED2DC9AB286A0F6C49C7AB319BA6A" +
                "AFAAF0CD42D0A44C7DACBC90791855D8";

            byte[] key = { 128, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] IV = new byte[10]; //0s

            byte[] input = new byte[512]; //0s

            Form1 program = new Form1();

            List<bool> z = program.calculateZ(input.Length, program.initialState(key, IV));

            BitArray ba = program.convertToBitArray(z);
            byte[] byteArray = ba.ToByteArray();
            string hexaAllZ = program.ByteArrayToHexaString(byteArray);

            string hexaZTested = "";

            for (int i = 0; i < 128; i++)
                hexaZTested += hexaAllZ[i];

            Assert.AreEqual(result, hexaZTested);
        }
    }
}
