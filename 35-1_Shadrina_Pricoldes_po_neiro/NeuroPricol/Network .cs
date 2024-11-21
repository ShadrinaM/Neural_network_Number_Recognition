using System;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    //моя конфигурация 15 + 74 + 31 + 10
    //входной слой(15 входных нейронов т.к. поле 3х5) + скрытый слой(74) + скрытый слой(31) + выходной слой(10 выходных нейронов т.к 10 цифр)   
    class Network
    {
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(74, 15, NeuroType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(31, 74, NeuroType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 31, NeuroType.Output, nameof(output_layer));

        private double[] fact = new double[10]; // массив фактического выхода из сети
        private double e_error_avr; //среднее значение энергии ошибки

        public double[] Fact { get => fact; }
        public double E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        public Network() { }

        public void Train(Network net)
        {
            
            net.input_layer = new InputLayer(NeuroWorkMode.Train); //инициализация входного слоя с режимом работы
            int epoches = 70; // кол-во эпох обучения(кол-во прогонов программы)
            double tmpSumError;// временная переменная суммы ошибок
            double[] errors;//вектор сигнала ошибки
            double[] temp_gsums1;//вектор градиента 1 скрытого слоя
            double[] temp_gsums2;//вектор градиента 2 скрытого слоя

            //дописать фото
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
