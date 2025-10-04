using System;

namespace Category;

class Program
{
    static void Main(string[] args)
    {

        var tests = new[]
                   {
                new Request { Method="read",   Path="/api/categories",       Date="1728000000" }, // OK
                new Request { Method="read",   Path="/api/categories/5",     Date="1728000000" }, // OK
                new Request { Method="create", Path="/api/categories/abc",   Date="1728000000" }, // bad path
                new Request { Method="dance",  Path="/api/categories",       Date="1728000000" }, // illegal method
                new Request { Method="read",   Path="/api/cats",             Date="1728000000" }, // illegal path
                new Request { Method="read",   Path="/api/categories",       Date=""           }, // missing date
                new Request { Method="read",   Path="/api/categories",       Date="-10"        }, // illegal date
                new Request { Method="",       Path="/api/categories",       Date="123"        }, // missing method
            };

        int i = 1;
        foreach (var req in tests)
        {
            var res = RequestValidator.ValidateRequest(req);
            Console.WriteLine($"Test {i++}: {res}");
        }
    }
}