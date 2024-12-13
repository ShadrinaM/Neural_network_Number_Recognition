using System;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    //моя конфигурация 15 + 74 + 31 + 10
    //входной слой(15 входных нейронов т.к. поле 3х5) + скрытый слой(74) + скрытый слой(31) + выходной слой(10 выходных нейронов т.к 10 цифр)   
    class Network
    {
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(74, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(31, 74, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 31, NeuronType.Output, nameof(output_layer));

        private double[] fact = new double[10]; // массив фактического выхода из сети
        private double[] e_error_avr; //среднее значение энергии ошибки

        public double[] Fact { get => fact; }
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        public Network() { }

        /// <summary>
        /// Обучение
        /// </summary>
        public void Train(Network net)
        {
            
            net.input_layer = new InputLayer(NeuroworkMode.Train); //инициализация входного слоя с режимом работы
            //ПОДОБРАТЬ КОЛИЧЕСТВО ЭПОХ ЧТОБЫ СЕТЬ ЛУЧШЕ И БЫСТРЕЕ ОБУЧАЛАСБ, 100 МНОГО
            int epoches = 60; // кол-во эпох обучения(кол-во прогонов программы)
            double tmpSumError = 0;// временная переменная суммы ошибок
            double[] errors;//вектор сигнала ошибки
            double[] temp_gsums1;//вектор градиента 1 скрытого слоя
            double[] temp_gsums2;//вектор градиента 2 скрытого слоя

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++) //перебор эпох обучения
            {
                e_error_avr[k] = 0; // в начале каждой эпохи обучения значение средней энергии ошибки эпохи обнуляется
                if (net.input_layer.Trainset == null)
                    throw new InvalidOperationException($"Network Train трейнсетнул");
                net.input_layer.Shuffling_Array_Rows(net.input_layer.Trainset);
                for (int i = 0; i < net.input_layer.Trainset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = net.input_layer.Trainset[i, j + 1];

                    ForwardPass(net, tmpTrain); //прямой проход обучающего образа

                    //вычисление ошибки по итерации
                    tmpSumError = 0;
                    errors = new double[net.fact.Length];// для каждого обучающего примера значение ошибки
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.Trainset[i, 0]) // если номер выбранного нейрона совпадает
                            errors[x] = 1.0 - net.fact[x];
                        else
                            errors[x] = -net.fact[x]; // =0-net.fact[x]
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; //сумматрное значение энергии ошибки

                    //обратный проход и коррекция весов
                    temp_gsums2 = net.output_layer.BackwardPass(errors);
                    temp_gsums1 = net.hidden_layer2.BackwardPass(temp_gsums2);
                    net.hidden_layer1.BackwardPass(temp_gsums1);
                }
                e_error_avr[k] /= net.input_layer.Trainset.GetLength(0);
            }
            net.input_layer = null; //обнуление входного слоя

            // запись скорректированных весов в память
            net.hidden_layer1.WeightInitialize(MemoryMode.SET, nameof(hidden_layer1) + "_memory.csv");
            net.hidden_layer2.WeightInitialize(MemoryMode.SET, nameof(hidden_layer2) + "_memory.csv");
            net.output_layer.WeightInitialize(MemoryMode.SET, nameof(output_layer) + "_memory.csv");
        }

        public void Test(Network net)
        {
            //ДОПИСАТЬ загруж тестовое множество и не меняем веса

            net.input_layer = new InputLayer(NeuroworkMode.Test); //инициализация входного слоя с режимом работы
            int epoches = 60; // кол-во эпох обучения(кол-во прогонов программы) 2-3
            double tmpSumError = 0;// временная переменная суммы ошибок
            double[] errors;//вектор сигнала ошибки
            double[] temp_gsums1;//вектор градиента 1 скрытого слоя
            double[] temp_gsums2;//вектор градиента 2 скрытого слоя

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++) //перебор эпох обучения
            {
                e_error_avr[k] = 0; // в начале каждой эпохи обучения значение средней энергии ошибки эпохи обнуляется
                if (net.input_layer.Testset == null)
                    throw new InvalidOperationException($"Network Test тестсетнул");
                net.input_layer.Shuffling_Array_Rows(net.input_layer.Testset);
                for (int i = 0; i < net.input_layer.Testset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = net.input_layer.Testset[i, j + 1];

                    ForwardPass(net, tmpTrain); //прямой проход обучающего образа

                    //вычисление ошибки по итерации
                    tmpSumError = 0;
                    errors = new double[net.fact.Length];// для каждого обучающего примера значение ошибки
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.Testset[i, 0]) // если номер выбранного нейрона совпадает
                            errors[x] = 1.0 - net.fact[x];
                        else
                            errors[x] = -net.fact[x]; // =0-net.fact[x]
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; //сумматрное значение энергии ошибки
                }
                e_error_avr[k] /= net.input_layer.Testset.GetLength(0);
            }
            net.input_layer = null; //обнуление входного слоя
        }


        //прямой проход сети
        public void ForwardPass(Network net, double[] netInput)
        {
            net.hidden_layer1.Data = netInput;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }
    }
}
