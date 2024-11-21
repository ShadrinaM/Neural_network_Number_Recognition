using System;
using System.IO;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    class InputLayer
    {
        //поля
        private double[,] trainset; // = new double[100, 16]; //100 изображений, 15 пикселей + желаемый отклик
        private double[,] testset; //= new double[10, 16];

        //свойства
        public double[,] Trainset { get => trainset; }

        public double[,] Testset { get => testset; }

        public InputLayer(NeuroWorkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory; // директорий, где находится .exe
            string[] tmpStr;  // временный массив элементов в строке
            string[] tmpArrStr; // временный массив строк

            switch (nm)
            {
                case NeuroWorkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.csv"); //
                    trainset = new double[tmpArrStr.Length, 16]; //
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(';');                      
                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i,j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(trainset);
                    break;
                case NeuroWorkMode.Test:
                    //дописать фото
                    break;
            }
        }

        public void Shuffling_Array_Rows (double[,] arr)
        {
            int j;
            Random random = new Random();
            double[] temp = new double[arr.GetLength(1)];

            for ( int n = arr.GetLength(0) -1; n>=1; n--)
            {
                j = random.Next(n + 1);

            }
            // дописать фото
        }
    }
}
