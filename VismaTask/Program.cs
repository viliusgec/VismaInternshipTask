using System;
using VismaTask.Controllers;

namespace VismaTask
{
    class Program
    {
        static void Main(string[] args)
        {
            RunProgram();
        }

        static void RunProgram()
        {
            BookController bookController = new BookController();
            Console.WriteLine("Welcome to online book library");
            Console.WriteLine("For all commands type: commands");
            while (true)
            {
                try
                {
                    var command = Console.ReadLine().ToLower();
                    switch (command)
                    {
                        case "commands":
                            GetCommands();
                            break;
                        case "addbook":
                            bookController.AddBook();
                            break;
                        case "takebook":
                            bookController.TakeBook();
                            break;
                        case "returnbook":
                            bookController.ReturnBook();
                            break;
                        case "getbooklist":
                            bookController.GetBookList();
                            break;
                        case "deletebook":
                            bookController.DeleteBook();
                            break;
                        default:
                            Console.WriteLine("Command is invalid. To get all command - type: commands.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                Console.WriteLine("Command finished.");
            }
        }

        static void GetCommands()
        {
            Console.WriteLine("Commands list:");
            Console.WriteLine("Commands - to see all commands.");
            Console.WriteLine("AddBook - to add a new book to the library.");
            Console.WriteLine("TakeBook - to take a book from the library.");
            Console.WriteLine("ReturnBook - to return a book to the library.");
            Console.WriteLine("GetBookList - to get a book list.");
            Console.WriteLine("DeleteBook - to delete a book from the library.");
        }
    }
}
