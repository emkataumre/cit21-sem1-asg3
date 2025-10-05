namespace AssignmentPart1;
public class CategoryService
{
    private static List<Category> _categories;

    public CategoryService()
    {
        _categories = new List<Category>()
        {
            new Category() { Id = 1, Name = "Beverages" },
            new Category() { Id = 2, Name = "Condiments" },
            new Category() { Id = 3, Name = "Confections" },
        };
    }
    
    public List<Category> GetCategories()
    {
        return _categories;
    }

    public Category? GetCategory(int cid)
    {
        var category = _categories.FirstOrDefault(c => c.Id == cid);

        if (category is null) return null;
        
        return _categories[cid - 1];
    }

    public bool UpdateCategory(int id, string newName)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        
        if (category is null) return false;
        
        _categories[id - 1].Name = newName;
        
        return true;
    }
    
    public bool  DeleteCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);

        if (category is null) return false;
        
        _categories.RemoveAt(id - 1);
        return true;
    }

    public bool CreateCategory(int id, string name)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);

        if (category is not null) return false;
        
        _categories.Add(new Category() {Id = id, Name = name});
        return true;
    }
}