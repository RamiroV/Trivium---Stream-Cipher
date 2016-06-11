﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Prototipo_Conversor_ImgBmp;
using System.Drawing;
using System.Drawing.Imaging;


namespace UnitTestTrivium
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestVEctorsSet2VectorN0FirstStream()
        {
            byte[] result = { 15, 11, 14, 0, 11, 15, 2, 6, 5, 8, 5, 9, 0, 5, 1, 11, 5, 1, 7, 10, 2, 14, 4, 14, 2, 3, 9, 
                                15, 12, 9, 7, 15, 5, 6, 3, 2, 0, 3, 1, 6, 1, 9, 0, 7, 12, 15, 2, 13, 14, 7, 10, 8, 7, 9, 
                                0, 15, 10, 1, 11, 2, 14, 9, 12, 13, 15, 7, 5, 2, 9, 2, 0, 3, 0, 2, 6, 8, 11, 7, 3, 8, 2, 
                                11, 4, 12, 1, 10, 7, 5, 9, 10, 10, 2, 5, 9, 9, 10, 2, 8, 5, 5, 4, 9, 9, 8, 6, 14, 7, 4, 
                                8, 0, 5, 9, 0, 3, 8, 0, 1, 10, 4, 12, 11, 5, 10, 5, 13, 4, 15, 2 };

            byte[] key = new byte[10];
            byte[] IV = new byte[10];

            Form1 main = new Form1();
            var firstByteOfDataInBMPFormat = 54;

            byte[] originalStream = new byte[512];
            var bitList = main.convertToBitList(new BitArray(originalStream));
            bitList = main.Xor(bitList, main.calculateZ(bitList.Count, main.initialState(key, IV)));

            for (int i = firstByteOfDataInBMPFormat; i < result.Length; i++)
                Assert.AreEqual(result[i], main.convertToBitArray(bitList).ToByteArray()[i]);
        }
        //[TestMethod]
        public void TestVEctorsSet1VectorN0SFirstStream()
        {
            byte[] result = { 1, 11, 14, 9, 5, 0, 9, 1, 11, 8, 14, 10, 8, 5, 7, 11, 0, 6, 2, 10, 13, 5, 2, 11, 10, 13,
                                 15, 4, 7, 7, 8, 4, 10, 12, 6, 13, 9, 11, 2, 14, 3, 15, 8, 5, 10, 9, 13, 7, 9, 9, 9, 5,
                                 0, 4, 3, 3, 0, 2, 15, 0, 15, 13, 15, 8, 11, 7, 6, 14, 5, 11, 12, 8, 11, 7, 11, 4, 15,
                                 0, 10, 10, 4, 6, 12, 13, 2, 0, 13, 13, 10, 0, 4, 15, 13, 13, 1, 9, 7, 11, 12, 5, 14, 1,
                                 6, 3, 5, 4, 9, 6, 8, 2, 8, 15, 2, 13, 11, 15, 11, 2, 3, 15, 6, 11, 13, 5, 13, 0 };

            byte[] key = new byte[10];
            byte[] IV = new byte[10];

            Form1 main = new Form1();
            var firstByteOfDataInBMPFormat = 54;

            byte[] originalStream = new byte[512];
            var bitList = main.convertToBitList(new BitArray(originalStream));
            bitList = main.Xor(bitList, main.calculateZ(bitList.Count, main.initialState(key, IV)));

            for (int i = firstByteOfDataInBMPFormat; i < result.Length; i++)
                Assert.AreEqual(result[i], main.convertToBitArray(bitList).ToByteArray()[i]);
        }
    }
}
