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
            int epoches = 10; // кол-во эпох обучения(кол-во прогонов программы)
            double tmpSumError;// временная переменная суммы ошибок
            double[] errors;//вектор сигнала ошибки
            double[] temp_gsums1;//вектор градиента 1 скрытого слоя
            double[] temp_gsums2;//вектор градиента 2 скрытого слоя

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++) //перебор эпох обучения
            {
                e_error_avr[k] = 0; // в начале каждой эпохи обучения значение средней энергии ошибки эпохи обнуляется
                net.input_layer.Shuffling_Array_Rows(net.input_layer.Trainset);
                for (int i = 0; i < net.input_layer.Trainset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = net.input_layer.Trainset[i, j + 1];
                    //прямой проход
                    //ДОПИСАТЬ
                }
            }
        }

        //прямой проход сети (ЭТОГО НЕ БЫЛО ЕЩЁ)
        public void ForwardPass(Network net, double[] netInput)
        {
            net.hidden_layer1.Data = netInput;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }
    }
}
