namespace AssignmentPart1;
public class CategoryService
{
    static List<Category> _categories = new List<Category>()
    {
        new Category() { CId = 1, Name = "Beverages" },
        new Category() { CId = 2, Name = "Condiments" },
        new Category() { CId = 3, Name = "Confections" },
    };
    
    public static void GetCategories()
    {
        foreach (var category in _categories)
        {
            Console.Write($"id: {category.CId}, ");
            Console.Write($"Name: {category.Name}");
            Console.WriteLine();
        }
    }

    public static void GetCategory(int cid)
    {
        int indexOfCategory = cid - 1;
        Console.WriteLine($"id: {_categories[indexOfCategory].CId}, name: {_categories[indexOfCategory].Name}");
    }

    public static void UpdateCategory(int id, string newName)
    {
        _categories[id - 1].Name = newName;
    }
    
    public static void DeleteCategory(int id)
    {
        _categories.RemoveAt(id - 1);
    }

    public static void CreateCategory(string name)
    {
        _categories.Add(new Category() {CId = _categories[^1].CId + 1, Name = name});
    }
}