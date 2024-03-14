//y\ =\ 2\sin\left(2x+4\right)+2

using Lab4;

//NeuroDelta neuro = new NeuroDelta();
//neuro.Education(100000);
//Console.ReadLine();

NeuroRandomSearch neuroRandomSearch = new NeuroRandomSearch();
neuroRandomSearch.Education();
while (true)
{
    string input = Console.ReadLine();
    if (input == "w")
    {
        string output = "";
        output += $"Показатель: {ObjectInfo.massExponent},\tВес: {neuroRandomSearch.weights[0]}\n";
        output += $"Показатель: {ObjectInfo.momentumExponent},\tВес: {neuroRandomSearch.weights[1]}\n";
        output += $"Показатель: {ObjectInfo.angleExponent},\tВес: {neuroRandomSearch.weights[2]}\n";
        output += $"Показатель: {ObjectInfo.gravityAccelerationExponent},\tВес: {neuroRandomSearch.weights[3]}\n";
        Console.WriteLine(output);
    }
    else if (input == "e")
    {
        break;
    }
    else
    {
        ObjectInfo objectInfo = ObjectInfo.RandomObject
        (
            (1, 5),
            (5, 25),
            (0, double.Pi / 2),
            (9.7, 9.9)
        );
        string output = "";

        double real = objectInfo.path;
        double predict = neuroRandomSearch.Predict(objectInfo, null);
        double difference = neuroRandomSearch.ObjectiveFunction(objectInfo, null);
        double procentage = (real - Math.Abs(real - predict)) / real * 100;

        output += $"Реально: {real}\n";
        output += $"Прогноз: {predict}\n";
        output += $"Разница: {difference}\n";
        output += $"Процент: {procentage}\n";

        Console.WriteLine(output);
    }
    
}