namespace AssignmentPart1;
public class CategoryService
{
    static List<Category> _categories = new List<Category>()
    {
        new Category() { Id = 1, Name = "Beverages" },
        new Category() { Id = 2, Name = "Condiments" },
        new Category() { Id = 3, Name = "Confections" },
    };
    
    public List<Category> GetCategories()
    {
        return _categories;
    }

    public Category? GetCategory(int cid)
    {
        if (cid < 1 || cid > _categories[^1].Id)
        {
            return null;
        }
        
        return _categories[cid - 1];
    }

    public bool UpdateCategory(int id, string newName)
    {
        if (id < 1 || id > _categories[^1].Id)
        {
            return false;
        }
        
        _categories[id - 1].Name = newName;
        return true;
    }
    
    public bool  DeleteCategory(int id)
    {
        if (id < 1 || id > _categories[^1].Id)
        {
            return false;
        }
        
        _categories.RemoveAt(id - 1);
        return true;
    }

    public bool CreateCategory(int id, string name)
    {
        foreach (var category in _categories)
        {
            if (id == category.Id)
            {
                return false;
            }
        }
        
        _categories.Add(new Category() {Id = id, Name = name});
        return true;
    }
}