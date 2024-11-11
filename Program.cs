using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Spectre.Console;
using Figgle;

namespace QuizP
{
    internal class Program
    {
        private static int totalQuestions = 0;
        private static int correctAnswers = 0;
        static void Main(string[] args)
        {
            var quizApp = new QuizApp();
            quizApp.LoadQuestions("questions.json"); // Path to JSON file
            var figgleTitle = FiggleFonts.Standard.Render("Quiz app");
            AnsiConsole.MarkupLine($"[bold blue]{figgleTitle}[/]");
            

            AnsiConsole.Markup("[bold yellow]Welcome to the Quiz App![/]");

            foreach (var category in quizApp.Categories)
            {
                AnsiConsole.Markup($"\n\n[bold cyan]{category.Name}[/]");
                DisplayQuestions(category.Questions);
            }

            ShowStatistics();
        }

        private static void DisplayQuestions(List<Question> questions)
        {
            foreach (var question in questions)
            {
                totalQuestions++;
                AnsiConsole.Markup($"\n[bold green]{question.Text}[/]\n");

                // Use a SelectionPrompt to allow navigation through answer options
                var selectedAnswer = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Select your answer:[/]")
                        .PageSize(10) // Allows displaying a limited number of options at once
                        .AddChoices(question.Options) // Add choices from the question options
                );

                // Check if selected answer is correct
                if (selectedAnswer == question.Answer)
                {
                    correctAnswers++;
                    AnsiConsole.Markup("[green]Correct![/]\n");
                }
                else
                {
                    AnsiConsole.Markup($"[red]Wrong![/] The correct answer is [bold]{question.Answer}[/].\n");
                }
            }
        }




        private static void ShowStatistics()
        {
            AnsiConsole.Markup($"\n[bold blue]Quiz Completed![/]\n");
            var statsTable = new Table();
            statsTable.AddColumn("Total Questions");
            statsTable.AddColumn("Correct Answers");
            statsTable.AddColumn("Wrong Answers");

            statsTable.AddRow(
                $"{totalQuestions}",
                $"{correctAnswers}",
                $"{totalQuestions - correctAnswers}"
            );

            AnsiConsole.Write(statsTable);
            AnsiConsole.Markup($"\n[bold yellow]Score: {correctAnswers}/{totalQuestions}[/]");
        }
    }
}
public class QuizApp
{
    public List<Category> Categories { get; set; }

    public void LoadQuestions(string filePath)
    {
        var json = File.ReadAllText(filePath);
        Categories = JsonConvert.DeserializeObject<QuizApp>(json).Categories;
    }
}

public class Category
{
    public string Name { get; set; }
    public List<Question> Questions { get; set; }
}

public class Question
{
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string Answer { get; set; }
}