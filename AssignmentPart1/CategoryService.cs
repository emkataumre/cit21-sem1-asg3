using System;

namespace Category;

class CategoryService
{
    public static List<Category> categories = new List<Category>
    {
        new Category {Cid = 1, Name = "Beverages"},
        new Category {Cid = 2, Name = "Condiments"},
        new Category {Cid = 3, Name = "Confections"},

    };
    public static List<Category> GetCategories()
    {
        foreach (Category c in categories)
        {
            Console.WriteLine(c);
        }
        return categories;
    }

    public static bool CreateCategory(string name)
    {
        var category = new Category { Cid = categories.Count + 1, Name = name };
        categories.Add(category);

        return true;
    }

    public static Category? GetCategory(int cid)
    {
        Category category = categories[cid];
        return category;
    }

    public static bool UpdateCategory(int id, string newName)
    {
        Category category = GetCategory(id);
        if (category is null) return false;
        if (category.Name == newName) return false;
        category.Name = newName;
        return true;
    }

    public static bool DeleteCategory(int id)
    {
        Category category = GetCategory(id);
        if (category is null) return false;

        categories.Remove(category);
        return true;
    }
}