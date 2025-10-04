namespace AssignmentPart1;

public class MenuLoop
{
    public static void Start()
    {
        string userInput = string.Empty;
        PrintMenuMessage();

        while (true)
        {
            userInput = Console.ReadLine();
            if (userInput == "1")
            {
                CategoryService.GetCategories();
            }
            else if (userInput == "2")
            {
                Console.Write("Show category with ID: ");
                Int32.TryParse(Console.ReadLine(), out int id);
                CategoryService.GetCategory(id);
            }
            else if (userInput == "3")
            {
                Console.Write("Update category with ID: ");
                Int32.TryParse(Console.ReadLine(), out int id);
                Console.Write("New category name: ");
                string newName = Console.ReadLine();
                CategoryService.UpdateCategory(id, newName);
            }
            else if (userInput == "4")
            {
                Console.Write("Delete category with ID: ");
                Int32.TryParse(Console.ReadLine(), out int id);
                CategoryService.DeleteCategory(id);
            }
            else if (userInput == "5")
            {
                Console.Write("New category name: ");
                string name = Console.ReadLine();
                CategoryService.CreateCategory(name);
            }
            else if (userInput == "6")
            {
                Console.Write("Url to parse: ");
                string url = Console.ReadLine();
                UrlParser parser = new UrlParser();

                bool isValid = parser.ParseUrl(url);

                Console.WriteLine(isValid);
                Console.WriteLine($"Path = {parser.Path}");
                Console.WriteLine($"HasId = {parser.HasId}");
                Console.WriteLine($"Id = {parser.Id}");
            }
            else if (userInput == "7")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.Write("Incorrect number. Choose again from 1-6: ");
            }

            Console.Write("[1] Back to menu or [2] Exit: ");
            userInput = Console.ReadLine();
            if (userInput == "1")
            {
                PrintMenuMessage();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    private static void PrintMenuMessage()
    {
        Console.WriteLine("Welcome to the categories CRUD!");
        Console.WriteLine("Type in a number and press enter to perform an operation.");
        Console.WriteLine("[1]: List categories");
        Console.WriteLine("[2]: Show category by ID");
        Console.WriteLine("[3]: Update category");
        Console.WriteLine("[4]: Delete category");
        Console.WriteLine("[5]: Create category");
        Console.WriteLine("[6]: Parse url");
        Console.WriteLine("[7]: Exit menu");
        Console.Write("Operation number: ");
    }
}
