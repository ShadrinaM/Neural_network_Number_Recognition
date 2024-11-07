using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    abstract class Layer //
    {
        // поля
        protected string name_Layer; // наименование слояя
        string pathDirWeights; // путь к каталогу, где находится файйл синаптических весов
        string pathFileWeights; // путь к файлу синаптических весов для нейронов
        protected int numofneurouns; // число нейронов текщего слоя
        protected int numofprevneurons; // число нейронов предыдущего слоя
        protected const double learnigrate = 0.005d;// скорость обучения
        protected const double momentum = 0.05d; // момент инерции
        protected double[,] lastdeltaweights; // веса предыдущей итерации
        protected Neuron[] neurons; // массив нейронов

        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data
        {
            set
            {
                for (int i =0; i<numofneurouns; i++)
                {
                    Neurons[i].Activator(value);
                }
            }
        }
        protected Layer(int non, int nopn, NeuroType nt, string nm_Layer)
        {
            numofneurouns = non;
            numofprevneurons = nopn;
            Neurons =new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights;

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            lastdeltaweights = new double[non, nopn + 1];

            for (int i=0; i<non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j=0; j<nopn+1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt);
            }
        }
        public double[,] WeightInitialize (MemoryMode mm, string path)
        {
            int i, j;
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurouns, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights=File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (i=0; i < numofneurouns; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (j=0;j< numofprevneurons+1; j++)
                        {
                            weights[i,j] = double.Parse(memory_elemnt[j].Replace(',','.'), 
                                System.Globalization.CultureInfo.InvariantCulture); 
                        }
                    }
                    break;
                case MemoryMode.INIT:
                    //дописать
                    Random random = new Random();
                    //сгенериенные веса нужно сохранить в файлик
                    break;
                case MemoryMode.SET:
                    //дописать
                    break;

            }
            // ПРАВИЛА УСТАНОВКИ ВЕСОВ
            // средние знаечния всех синопт весов и порогов есть случайные числа
            
            // мат ошидание(сред значение) для каждого нейррона должно быть = 0
            //для этого нужно найти среднеарифм весов и первого порога и вычесть это значение из каждого веса
            
            // среднеквадратическое отклонение(сигма) должно быть равно 1
            //для этого найти сигму полученной выборки и каждый элемент поделить на сигму
            return weights;
        }


        abstract public void Recognize(Network net, Layer nextLayer);
        abstract public double[] BackwardPass(double[] stuff);

    }
}
