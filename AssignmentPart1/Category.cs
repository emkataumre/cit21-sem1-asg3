using System;

namespace Category;

public class Category
{

    public int Cid { get; set; }
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"{Cid}: {Name}";
    }
}