namespace AssignmentPart1;

class Server
{
    static void Main()
    {
        CategoryService service = new CategoryService();
        var categories = service.GetCategories();
        Console.WriteLine(categories[0].Name);
        Console.WriteLine(string.Join(Environment.NewLine, categories.Select(c => c.Name)));
        Console.WriteLine(service.CreateCategory(4, "Test"));
        
        var c1 = service.GetCategory(4);
        Console.WriteLine(c1?.Name);
        Console.WriteLine(service.UpdateCategory(4, "Updated Test"));
        
        var c2 = service.GetCategory(4);
        Console.WriteLine(c2?.Name);
        Console.WriteLine(service.DeleteCategory(4));
        
        var c3 = service.GetCategory(4);
        Console.WriteLine(c3 == null);

        Console.WriteLine("-----------------");

        // Test requests
        RequestValidator validator = new RequestValidator();

        var req1 = new Request { Method = "create", Path = "api/categories", Date = "2633036800", Body = "{\"cid\":5,\"name\":\"Dairy Products\"}" };
        var res1 = validator.ValidateRequest(req1);
        Console.WriteLine(res1.Status);

        var req2 = new Request { Method = "test", Path = "", Date = "pippi", Body = "" };
        var res2 = validator.ValidateRequest(req2);
        Console.WriteLine(res2.Status);

        var req3 = new Request { Method = "read", Path = "api/categories/1", Date = "-1" };
        var res3 = validator.ValidateRequest(req3);
        Console.WriteLine(res3.Status);

        var req4 = new Request { Method = "update", Path = "api/categories/1", Date = "2633036800" };
        var res4 = validator.ValidateRequest(req4);
        Console.WriteLine(res4.Status);

        var req5 = new Request { Method = "echo", Path = "/echo", Date = "2633036800", Body = "test" };
        var res5 = validator.ValidateRequest(req5);
        Console.WriteLine(res5.Status);

        // Test urlparser
        UrlParser parser1 = new UrlParser();
        parser1.ParseUrl("api/categories/1");
        Console.WriteLine($"{parser1.HasId}, {parser1.Id}, {parser1.Path}");

        UrlParser parser2 = new UrlParser();
        parser2.ParseUrl("api/categories");
        Console.WriteLine($"{parser2.HasId}, {parser2.Id}, {parser2.Path}");
    }
}
