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

        /// <summary>
        /// Метод обратного распространения
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public override double[] BackwardPass (double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurouns; k++)
                    sum += neurons[k].Weights[j] * neurons[k].Derivative * gr_sum[k]; //через градиентные суммы и производную
                gr_sum[j] = sum;
            }
            //обучение, коррекция синоптических весов
            for (int i = 0; i < numofneurouns; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0) //если порог
                        deltaw = momentum * lastdeltaweights[i, 0] + learnigrate * neurons[i].Derivative * gr_sum[i];
                    else
                        deltaw = momentum * lastdeltaweights[i, n] + learnigrate * neurons[i].Inputs[n - 1] * neurons[i].Derivative * gr_sum[i];
                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] = deltaw; // коррекция весов
                }
            }
            return gr_sum;
        }
    }
}
