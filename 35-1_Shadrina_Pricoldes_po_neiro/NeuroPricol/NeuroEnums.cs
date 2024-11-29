namespace _35_1_Shadrina_Pricoldes_po_neiro.NeuroPricol
{
    enum NeuronType //тип нейрона
    {
        Hidden,  // нейон скрытого слоя
        Output //нейрон выходного слоя
    }

    enum NeuroworkMode //режим работы сети
    {
        Train,  // режим обучения
        Test, //режим тестирования
        Recogn //режим распознавания
    }

    enum MemoryMode //режим работы памяти
    {
        GET,  // считывание
        SET, // сохранение
        INIT // инициализация
    }
}
