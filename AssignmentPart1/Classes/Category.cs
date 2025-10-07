using System;

namespace Category;

public class Category
{

    public int Id { get; set; }
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Name}";
    }
}