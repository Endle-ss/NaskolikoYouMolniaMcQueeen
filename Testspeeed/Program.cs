using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
class TypingTest
{
    static string[] sourceText = {
        "карл у клары украл кораллы, клара у карла украла кларнет",
        "кукушка кукушонку купила капюшон в капюшоне кукушонок смешон",
        "шла саша по шоссе и сосала сушку",
        "шапкой саша шишки сшиб",
        "хыхахаха я сума схожу помогите"
    };
    static Random random = new Random();
    static Stopwatch stopwatch = new Stopwatch();
    static int wordCount = 0;
    static bool testInProgress = true;
    static List<TestResult> results = new List<TestResult>();
    static void Main()
    {
        Console.WriteLine("Это тест на молнию маквина");
        Console.Write("Введите ваше имя: ");
        string username = Console.ReadLine();
        Console.WriteLine("Введите 'redi' чтобы начать. Ну или не введите, чтобы выйти.");
        string readiness = Console.ReadLine();
        if (readiness.ToLower() == "redi")
        {    
        Console.Clear();
            Thread timerThread = new Thread(Timer);
            timerThread.Start();
            RunTypingTest();
            SaveResultsToJson(username, results);
        }
        else
        {
            Console.WriteLine("Чтобы выйти нажмите что-нибудь");
            Console.ReadKey();
        }
    }
    static void RunTypingTest()
    {
        stopwatch.Start();
        int matchCount = 0;
        int allCount = 0;
        while (wordCount < sourceText.Length && testInProgress)
        {
            string currentText = sourceText[wordCount];
            Console.WriteLine($"Вот текст '{currentText}': ");
            Console.WriteLine("Введите текст:");
            Console.InputEncoding = System.Text.Encoding.Unicode;
            string userInput = Console.ReadLine();
            for (int i = 0; i < currentText.Length; i++)
            {
                if (i < userInput.Length && currentText[i] == userInput[i])
                {
                    matchCount++;
                    allCount++;
                }
            }
            Console.WriteLine("Количество совпадений: " + matchCount);
            matchCount = 0;
            wordCount++;
        }
        stopwatch.Stop();
        Console.WriteLine($"Время, затраченное на тест: {stopwatch.Elapsed.TotalSeconds} секунд");
        Console.WriteLine("Итоговое количество совпадений: " + allCount);
    }
    static void Timer()
    {
        int testDurationSeconds = 60;
        for (int i = 0; i < testDurationSeconds; i++)
        {
            Thread.Sleep(1000);
        }
        testInProgress = false;
    }
    static void SaveResultsToJson(string username, List<TestResult> results)
    {
        TestResult currentUserResult = new TestResult
        {
            Username = username,
            TotalTexts = wordCount,
            TestDurationSeconds = stopwatch.Elapsed.TotalSeconds,
            SymbolsPerSecond = wordCount / stopwatch.Elapsed.TotalSeconds
        };
        results.Add(currentUserResult);
        string jsonData = JsonConvert.SerializeObject(results);
        File.WriteAllText("leaderboard.json", jsonData);

        Console.WriteLine("Результаты сохранены в leaderboard.json:");
        Console.WriteLine(jsonData);
    }
}
class TestResult
{
    public string Username { get; set; }
    public int TotalTexts { get; set; }
    public double TestDurationSeconds { get; set; }
    public double SymbolsPerSecond { get; set; }
}