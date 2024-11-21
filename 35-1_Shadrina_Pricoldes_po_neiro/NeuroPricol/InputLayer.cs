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

        public InputLayer(NeuroworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory; // директорий, где находится .exe
            string[] tmpArrStr; // временный массив строк
            string[] tmpStr;  // временный массив элементов в строке            

            switch (nm)
            {
                case NeuroworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.csv"); //считывание из файла обучающей выборки
                    trainset = new double[tmpArrStr.Length, 16]; //определение массива обучающей выборки
                    for (int i = 0; i < tmpArrStr.Length; i++) //цикл перебора строк обучающей выборки
                    {
                        tmpStr = tmpArrStr[i].Split(';'); // разбиение i-ой строки на массив отдельных символов                  
                        for (int j = 0; j < 16; j++) // цикл заполнения i-ой строки обучающей выборки
                        {
                            trainset[i,j] = double.Parse(tmpStr[j]); //строковое значение числа преобразуется в сам ...
                        }
                    }
                    Shuffling_Array_Rows(trainset); // перетасовка обучающей выборки методом Фишера-Йетса
                    break;
                case NeuroworkMode.Test:
                    // ДОПИСАТЬ
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
                for ( int i = 0;i < arr.GetLength(1); i++)
                {
                    temp[i] = arr[n, i];
                }
                for ( int i = 0; i< arr.GetLength(1); i++)
                {
                    arr[n, i] = arr[j, i];
                    arr[j, i] = temp[i];
                }
            }
        }
    }
}
