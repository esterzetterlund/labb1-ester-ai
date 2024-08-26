using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using System;

namespace labb1esterai
{
    internal class Program
    {
        // Text Analytics konfiguration
        private static readonly string textAnalyticsEndpoint = "https://labb1textana.cognitiveservices.azure.com/";
        private static readonly string textAnalyticsKey = "5ac1df62e6e04c488e133e85eac968bd";

        // QnA konfiguration
        private static readonly string qnaEndpoint = "https://labb1esteraiai.cognitiveservices.azure.com/";
        private static readonly string qnaKey = "5342e8ae5a8645bbab41d666e1225772";
        private static readonly string projectName = "labb1qna";
        private static readonly string deploymentName = "FAQCat";

        static void Main(string[] args)
        {
            // Skapa klienter för Text Analytics och QnA
            var textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyticsEndpoint), new AzureKeyCredential(textAnalyticsKey));
            var qnaClient = new QuestionAnsweringClient(new Uri(qnaEndpoint), new AzureKeyCredential(qnaKey));
            var project = new QuestionAnsweringProject(projectName, deploymentName);

            // Visa meny och hantera användarens val
            while (true)
            {
                Console.Clear(); // Rensa konsolen
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Ask QnA a question about cats");
                Console.WriteLine("2. Detecting language in a text");
                Console.WriteLine("3. Exit");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AskQuestion(qnaClient, project);
                        break;
                    case "2":
                        DetectLanguageAndDisplay(textAnalyticsClient);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select again.");
                        break;
                }
            }
        }

        // Metod för att ställa en fråga till QnA
        static void AskQuestion(QuestionAnsweringClient client, QuestionAnsweringProject project)
        {
            Console.Clear(); // Rensa konsolen
            Console.WriteLine("Enter your question about cats:");
            string question = Console.ReadLine();

            try
            {
                var response = client.GetAnswers(question, project);
                Console.WriteLine($"Answer: {(response.Value.Answers.Count > 0 ? response.Value.Answers[0].Answer : "No answer was found.")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on request: {ex.Message}");
            }

            // Vänta på att användaren trycker på Enter innan konsolen rensas
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        // Metod för att detektera språk i en text
        static void DetectLanguageAndDisplay(TextAnalyticsClient client)
        {
            Console.Clear(); // Rensa konsolen
            Console.WriteLine("Enter text for language detection:");
            string text = Console.ReadLine();

            try
            {
                var response = client.DetectLanguage(text);
                var detectedLanguage = response.Value;
                Console.WriteLine($"Language detected: {detectedLanguage.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Language Detection Error: {ex.Message}");
            }

            // Vänta på att användaren trycker på Enter innan konsolen rensas
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
