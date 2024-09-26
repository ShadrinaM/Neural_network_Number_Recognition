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

        //конструктор
        public Neuron(double[] weights, NeuroType type)
        {
            _type = type;
            _weights = weights;
        }

        public void Activator(double[] i, double[] w) // нелинейные пеобразования
        {
            double sum = w[0];//аффинное преобразование чеез смещение (нулевой вес, порог)
            for (int m = 0; m < i.Length; m++)
                sum += i[m] * w[m + 1]; //линейные преобразования
            switch (_type)
            {
                case NeuroType.Hidden:
                    _output = GeepTg(sum);
                    _derivative = GeepTg_Derivativator(sum);
                    break;
                case NeuroType.Output:
                    _output = Exp(sum);
                    break;
            }
        }

        //Дописать GeepTg и GeepTg_Derivativator, подготовить обучающие данные для нейронки 10 обучающих, 1 тест
    }
}
