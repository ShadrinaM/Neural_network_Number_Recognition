using System;
using static System.Math;


namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    internal class Neuron
    {
        // поля с маленькой буквы, свойства с большой, поля приват, свойства паблик

        private NeuroType _type; // тип нейрона
        private double[] _weights; // на нулевом порог, дальше синоптические веса
        private double[] _inputs; // входные данные, на 1 элемент меньше чем в весах
        private double _output; // выход
        private double _derivative; // производная функции активации
        private double a = 0.01; //параметры для функции активации
        // функция активации геперболический тангенс

        public double[] Weights { get => _weights; set => _weights = value; }
        public double[] Inputs 
        {
            get { return _inputs; }
            set { _inputs = value; }
        }

        public double Output { get => _output; }
        public double Derivative { get => _derivative; }

        // Конструктор
        public Neuron(double[] weights, NeuroType type)
        {
            _type = type;
            _weights = weights;
        }

        // Метод активации нейрона (нелинейное преобразование водного слоя)
        public void Activator(double[] i) 
        {
            double[] inputs = i; // передача вектора входного сигнала в массив входных данных
            double sum = _weights[0]; //аффинное преобразование чеез смещение (нулевой вес, порог)
            for (int j = 0; j < inputs.Length; j++)
                sum += inputs[j] * _weights[j + 1]; //линейные преобразования
            switch (_type)
            {
                case NeuroType.Hidden: // для нейронов скрытого слоя
                    _output = GeepTg(sum);
                    _derivative = GeepTg_Derivativator(sum);
                    break;
                case NeuroType.Output:
                    _output = Exp(sum);
                    break;
            }
        }

        public double GeepTg(double sum)
        {
            return (Math.Exp(sum) - Math.Exp(-sum)) / (Math.Exp(sum) + Math.Exp(-sum));
        }

        public double GeepTg_Derivativator(double sum)
        {
            double tg = (Math.Exp(sum) - Math.Exp(-sum)) / (Math.Exp(sum) + Math.Exp(-sum));
            return 1 - tg * tg;
        }
        //1 кнопочки
        //2 Дописать GeepTg и GeepTg_Derivativator, подготовить обучающие данные для нейронки 10 обучающих, 1 тест
        //3 создать классы слоёв
    }
}
