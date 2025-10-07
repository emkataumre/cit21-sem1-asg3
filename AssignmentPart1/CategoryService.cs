namespace AssignmentPart1;
public class CategoryService
{
    private static List<Category> _categories;

    public CategoryService()
    {
        _categories = new List<Category>()
        {
            new Category() { Cid = 1, Name = "Beverages" },
            new Category() { Cid = 2, Name = "Condiments" },
            new Category() { Cid = 3, Name = "Confections" },
        };
    }
    
    public List<Category> GetCategories()
    {
        return _categories;
    }

    public Category? GetCategory(int cid)
    {
        var category = _categories.FirstOrDefault(c => c.Cid == cid);

        if (category is null) return null;
        
        return _categories[cid - 1];
    }

    public bool UpdateCategory(int id, string newName)
    {
        var category = _categories.FirstOrDefault(c => c.Cid == id);
        
        if (category is null) return false;
        
        _categories[id - 1].Name = newName;
        
        return true;
    }
    
    public bool DeleteCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Cid == id);

        if (category is null) return false;
        
        _categories.Remove(category);
        return true;
    }

    public bool CreateCategory(int? id, string name)
    {
        var category = _categories.FirstOrDefault(c => c.Cid == id);

        if (category is not null) return false;
        
        _categories.Add(new Category() {Cid = id ?? _categories[^1].Cid + 1, Name = name});
        return true;
    }
}