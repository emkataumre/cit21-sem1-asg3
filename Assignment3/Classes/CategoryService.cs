namespace AssignmentPart1;

public class CategoryService
{
    public List<Category> categories;

    public CategoryService()
    {
        categories = new List<Category>
        {
            new Category { Id = 1, Name = "Beverages" },
            new Category { Id = 2, Name = "Condiments" },
            new Category { Id = 3, Name = "Confections" }
        };
    }
    public List<Category> GetCategories()
    {
        return categories;
    }
    public Category? GetCategory(int cid)
    {
        foreach (var category in categories)
        {
            if (category.Id == cid)
            {
                return category;
            }
        }
        return null;
    }
    public bool CreateCategory(int id, string name)
    {
        if (GetCategory(id) == null)
        {
            categories.Add(new Category { Id = id, Name = name });
            return true;
        }
        return false;
    }
    public bool UpdateCategory(int id, string newName)
    {
        var category = GetCategory(id);
        if (category != null)
        {
            category.Name = newName;
            return true;
        }
        return false;
    }
    public bool DeleteCategory(int id)
    {
        var category = GetCategory(id);
        if (category != null)
        {
            return categories.Remove(category);
        }
        return false;
    }
}
