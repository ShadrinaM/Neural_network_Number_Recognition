using System;

namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    class HiddenLayer : Layer
    {
        public HiddenLayer( int non, int nopn, NeuronType nt, string type) : base (non, nopn, nt, type) { }
        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[numofneurouns];
            for (int i = 0; i < numofneurouns; i++)
                hidden_out[i] = neurons[i].Output;
            nextLayer.Data = hidden_out; // передача выходного сигнала этого слоя на вход следующего слоя
        }

        //обратный проход 
        public override double[] BackwardPass (double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];

            for (int j = 0; j < numofprevneurons; j++) // цикл вычисления градиентной суммы
            {
                double sum = 0;
                for (int k = 0; k < numofneurouns; k++)
                {
                    if (neurons[k] == null)
                        throw new InvalidOperationException($"Neuron {k} не инициализирован.");
                    if (j >= neurons[k].Weights.Length)
                        throw new IndexOutOfRangeException($"Индекс {j} выходит за пределы Weights для нейрона {k}.");
                    if (k >= gr_sums.Length)
                        throw new IndexOutOfRangeException($"Индекс {k} выходит за пределы gr_sums.");

                    sum += neurons[k].Weights[j] * neurons[k].Derivative * gr_sums[k];
                }
                gr_sum[j] = sum;
            }
            //обучение, коррекция синоптических весов
            for (int i = 0; i < numofneurouns; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0) //если порог
                        deltaw = momentum * lastdeltaweights[i, 0] + learnigrate * neurons[i].Derivative * gr_sums[i];
                    else
                        deltaw = momentum * lastdeltaweights[i, n] + 
                            learnigrate * neurons[i].Inputs[n - 1] * neurons[i].Derivative * gr_sums[i];
                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw; // коррекция весов
                }
            }
            return gr_sum;
        }
    }
}
