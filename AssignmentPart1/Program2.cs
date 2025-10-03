namespace Assignment1;
class Program
{
    static void Main()
    {
        Category c1 = new Category { cid = 1, name = "One" };
        Console.WriteLine($"{c1.name} {c1.cid}");
    }
}
