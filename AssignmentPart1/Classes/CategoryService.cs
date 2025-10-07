using System;

namespace Category;

public class CategoryService
{
    public static List<Category> categories = new List<Category>
    {
        new Category {Id = 1, Name = "Beverages"},
        new Category {Id = 2, Name = "Condiments"},
        new Category {Id = 3, Name = "Confections"},

    };
    public List<Category> GetCategories()
    {
        foreach (Category c in categories)
        {
            Console.WriteLine(c);
        }
        return categories;
    }

    public bool CreateCategory(int id, string name)
    {
        var testCategory = GetCategory(id);
        if (testCategory is not null)
        {
            return false;
        }
        //var category = new Category { Cid = categories.Count + 1, Name = name };
        var category = new Category { Id = id, Name = name };

        categories.Add(category);

        return true;
    }

    public Category? GetCategory(int cid)
    {
        return categories.Find(c => c.Id == cid);
    }

    public bool UpdateCategory(int id, string newName)
    {
        Category category = GetCategory(id);
        if (category is null) return false;
        if (string.IsNullOrWhiteSpace(newName)) return false;
        if (category.Name == newName) return false;
        category.Name = newName;
        return true;
    }

    public bool DeleteCategory(int id)
    {
        Category category = GetCategory(id);
        if (category is null) return false;

        categories.Remove(category);
        return true;
    }
}