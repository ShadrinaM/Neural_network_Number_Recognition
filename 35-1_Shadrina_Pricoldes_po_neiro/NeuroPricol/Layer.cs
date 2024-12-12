using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    abstract class Layer
    {
        
        protected string name_Layer; // наименование слоя // protected - модификатор доступа, доступен в самом классе и наследниках
        string pathDirWeights; // путь к каталогу, где находится файйл синаптических весов
        string pathFileWeights; // путь к файлу синаптических весов для нейронов
        protected int numofneurouns; // число нейронов текщего слоя
        protected int numofprevneurons; // число нейронов предыдущего слоя
        protected const double learnigrate = 0.100d;// скорость обучения
        protected const double momentum = 0.01d; // момент инерции //ЧТО СДЕЛАТЬ: =0 =выключается оптимизация по моменту, нужно ПОДОБРАТЬ learnigrate ЧТО БЫ НЕ ТУПИЛО И НЕ ВЗРЫВАЛОСЬ
        //ПОТОМ ЕСЛИ ГРАФИК СКАЧКАМИ МОЖНО ПРИБАВИТЬ МОМЕНТ
        protected double[,] lastdeltaweights; // веса предыдущей итерации
        protected Neuron[] neurons; // массив нейронов текущего слоя

        /// <summary>
        /// Гетер и сеттер для _neurons
        /// </summary>
        public Neuron[] Neurons { get => neurons; set => neurons = value; }

        /// <summary>
        /// Сеттер устнавливающий входные данные на нейроны слоя и их активация
        /// </summary>
        public double[] Data 
        {
            set // определяет сеттер
            {
                for (int i =0; i<numofneurouns; i++) // Цикл по всем нейронам слоя
                {
                    Neurons[i].Activator(value); // Передача данных текущему нейрону для активации
                }
            }
        }

        /// <summary>
        /// Конструктор для создания слоя нейронов.
        /// </summary>
        /// <param name="non">Количество нейронов в текущем слое.</param>
        /// <param name="nopn">Количество нейронов в предыдущем слое.</param>
        /// <param name="nt">Тип нейронов в текущем слое (скрытый/выходной).</param>
        /// <param name="nm_Layer">Название слоя, которое будет использоваться для сохранения и загрузки данных.</param>
        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurouns = non; // количество нейронов текущего слоя
            numofprevneurons = nopn; // количество нейронов предыдущего слоя
            name_Layer = nm_Layer; // наименование слоя
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            // Указывает каталог, где будут храниться файлы с весами (в данном случае, папка memory в текущей директории)
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";
            // Полный путь к файлу, который содержит веса для текущего слоя.

            
            double[,] Weights; // временный массив синаптических весов текущего слоя

            if (File.Exists(pathFileWeights)) // проверка существования файла весов
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights); //загрузка весов из файла
            else
            {
                Directory.CreateDirectory(pathDirWeights); // Создает директорию, если ее нет
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights); // Инициализация весов
            }

            //Массив для хранения изменений весов на предыдущей итерации
            lastdeltaweights = new double[non, nopn + 1];


            Neurons = new Neuron[non];  // инициализация массива нейронов
            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1]; //массив весов для каждого нейрона
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j]; // копирует веса для данного нейрона из массива весов
                }
                Neurons[i] = new Neuron(tmp_weights, nt); // инициализаций нейрона в слое с таким весом и типом
            }
        }

        /// <summary>
        /// Инициализирует или загружает синаптические веса для текущего слоя нейронной сети.
        /// В зависимости от значения параметра <paramref name="mm"/>, метод может:
        /// 1. Загрузить существующие веса из файла, если они уже были сохранены (MemoryMode.GET).
        /// 2. Инициализировать случайными значениями веса для нового слоя (MemoryMode.INIT).
        /// 3. Сохранить текущие веса в файл (MemoryMode.SET).
        /// </summary>
        /// <param name="mm">Режим работы метода: 
        /// <list type="bullet">
        /// <item><description>MemoryMode.GET - загрузить веса из файла.</description></item>
        /// <item><description>MemoryMode.INIT - инициализировать веса случайными значениями.</description></item>
        /// <item><description>MemoryMode.SET - сохранить текущие веса в файл.</description></item>
        /// </list>
        /// </param>
        /// <param name="path">Путь к файлу, где хранятся или будут сохранены синаптические веса.</param>
        /// <returns> Двумерный массив <c>double[,]</c>, содержащий синаптические веса, где каждая строка соответствует нейрону в текущем слое, а каждый столбец — весу, связанному с нейроном из предыдущего слоя.</returns>
        public double[,] WeightInitialize (MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' }; // разделители слов  - ; и "пробел"
            string tmpStr; // временная строка для чтения
            string[] tmpStrWeights; // временный массив строк
            double[,] weights = new double[numofneurouns, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET: //загружает веса из файла
                    tmpStrWeights=File.ReadAllLines(path); // считывает из файла массив строк весов
                    string[] memory_elemnt;
                    // разбивает каждую строчку и заполняет двумерный массив весов
                    for (int i = 0; i < numofneurouns; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (int j = 0;j< numofprevneurons+1; j++)
                        {
                            weights[i,j] = double.Parse(memory_elemnt[j].Replace(',','.'), 
                                System.Globalization.CultureInfo.InvariantCulture); 
                        }
                    }
                    break;
                case MemoryMode.INIT: //иницализирует веса случайными значениями
                    Random random = new Random();  // Генератор случайных чисел

                    // Инициализация исходящих значений синаптических весов нейронов
                    double tmpRatio;  // Соотношение для масштабирования весов
                    double tmpShift;  // Смещение для корректировки среднего значения весов
                    double[] tmpArr = new double[numofprevneurons + 1]; // Массив для хранения весов одного нейрона
                    tmpStrWeights = new string[numofneurouns];  // Массив строк для сохранения весов

                    for (int i = 0; i < numofneurouns; i++)
                    {
                        // Инициализация временного массива весов для нейрона
                        for (int j = 0; j < numofprevneurons + 1; j++)  // +1 для порога (bias)
                        {
                            tmpArr[j] = 0.02 * random.NextDouble() - 0.01; // Генерация случайного числа от -0.01 до 0.01
                        }

                        // Рассчитываем среднее значение (для корректировки веса, чтобы оно было 0)
                        tmpShift = Calc_Average(tmpArr);

                        // Рассчитываем дисперсию (для корректировки стандартного отклонения)
                        tmpRatio = 1.0d / Math.Sqrt(Calc_Dispers(tmpArr) * numofprevneurons); // Нормализация по дисперсии

                        // Применяем нормализацию: для каждого веса вычитаем среднее и делим на стандартное отклонение
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = (tmpArr[j] - tmpShift) * tmpRatio;  // Корректируем каждый вес
                        }
                    }

                    // Генерация строк для записи в файл (если нужно сохранять)
                    for (int i = 0; i < numofneurouns; i++)
                    {
                        // Преобразуем каждый элемент строки в строку и соединяем их через ";"
                        tmpStrWeights[i] = string.Join(";", Enumerable.Range(0, numofprevneurons + 1)
                                                                      .Select(j => weights[i, j].ToString(CultureInfo.InvariantCulture)));
                    }

                    // Сохранение сгенерированных весов в файл
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                case MemoryMode.SET: // Сохраняет веса в файл
                                     // Проверяем, существует ли директория, если нет, создаем её
                    if (!Directory.Exists(pathDirWeights))
                    {
                        Directory.CreateDirectory(pathDirWeights);
                    }

                    // Открываем файл для записи
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        // Проходим по всем нейронам
                        for (int i = 0; i < numofneurouns; i++)
                        {
                            // Преобразуем веса текущего нейрона в строку, разделённую ";"
                            var row = Enumerable.Range(0, numofprevneurons + 1)
                                                .Select(j => weights[i, j].ToString(CultureInfo.InvariantCulture))
                                                .ToArray();
                            // Записываем строку в файл
                            writer.WriteLine(string.Join(";", row));
                        }
                    }
                    break;
            }
            return weights;
        }
              

        /// <summary>
        /// Метод расчёта среднего значения для массива весов одного нейрона
        /// </summary>
        protected double Calc_Average(double[] arr)
        {
            double _arr = 0d;
            for (int i = 0; i < arr.Length; i++)
                _arr += arr[i];
            return _arr / arr.Length;  // Среднее значение
        }

        /// <summary>
        /// Метод расчёта дисперсии для массива весов одного нейрона
        /// </summary>
        protected double Calc_Dispers(double[] arr)
        {
            double avg = Calc_Average(arr); // Среднее значение
            double disp = 0d;

            // Вычисление дисперсии
            for (int i = 0; i < arr.Length; i++)
            {
                disp += Math.Pow(arr[i] - avg, 2);
            }

            disp /= (arr.Length - 1); // Дисперсия
            return disp;
        }

        /// <summary>
        /// Метод для прямых проходов
        /// </summary>
        /// <param name="net"></param>
        /// <param name="nextLayer"></param>
        abstract public void Recognize(Network net, Layer nextLayer);
        /// <summary>
        /// Метод для обратных проходов
        /// </summary>
        /// <param name="stuff"></param>
        /// <returns></returns>
        abstract public double[] BackwardPass(double[] stuff);

    }
}

// дописать этот код на 21.11.24 при условиях что:
// ПРАВИЛА УСТАНОВКИ ВЕСОВ
// средние знаечния всех синопт весов и порогов есть случайные числа

// мат ошидание(сред значение) для каждого нейррона должно быть = 0
//для этого нужно найти среднеарифм весов и первого порога и вычесть это значение из каждого веса

// среднеквадратическое отклонение(сигма) должно быть равно 1
//для этого найти сигму полученной выборки и каждый элемент поделить на сигму