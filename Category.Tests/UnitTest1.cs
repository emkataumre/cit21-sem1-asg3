using System;
using Xunit;


namespace Category;

public class PartITests
{
    //////////////////////////////////////////////////////////
    /// 
    /// Testing UrlParser class
    /// 
    ////////////////////////////////////////////////////////// 

    [Fact]
    public void UrlParser_ValidUrlWithoutId_ShouldParseCorrectly()
    {
        var urlParser = new UrlParser();
        var url = "/api/categories";

        var result = urlParser.ParseUrl(url);

        Assert.True(result);
        Assert.False(urlParser.HasId);
        Assert.Equal("/api/categories", urlParser.Path);
    }

    [Fact]
    public void UrlParser_ValidUrlWithId_ShouldParseCorrectly()
    {
        var urlParser = new UrlParser();
        var url = "/api/categories/5";

        var result = urlParser.ParseUrl(url);

        Assert.True(result);
        Assert.True(urlParser.HasId);
        Assert.Equal(5, urlParser.Id);
        Assert.Equal("/api/categories", urlParser.Path);
    }

    //////////////////////////////////////////////////////////
    /// 
    /// Testing RequestValidator class
    /// 
    //////////////////////////////////////////////////////////

    [Fact]
    public void RequestValidator_NoMethod_ShouldReturnMissingMethod()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Path = "/api/categories",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("missing method", result.Body);
    }

    [Fact]
    public void RequestValidator_InvalidMethod_ShouldReturnIllegalMethod()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "fetch",
            Path = "/api/categories/1",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("illegal method", result.Body);
    }

    [Fact]
    public void RequestValidator_NoPath_ShouldReturnMissingPath()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("missing path", result.Body);
    }

    [Fact]
    public void RequestValidator_NoDate_ShouldReturnMissingDate()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/categories",
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("missing date", result.Body);
    }

    [Fact]
    public void RequestValidator_InvalidDate_ShouldReturnIllegalDate()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/categories",
            Date = DateTime.Now.ToString()
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("illegal date", result.Body);
    }

    [Theory]
    [InlineData("create")]
    [InlineData("update")]
    [InlineData("echo")]
    public void RequestValidator_NoBody_ShouldReturnMissingBody(string method)
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/categories",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };

        var result = requestValidator.ValidateRequest(request);


        Assert.Equal("4", result.Status);
        Assert.Contains("missing body", result.Body, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("create", "{\"id\":1,\"name\":\"xxx\"}")]
    [InlineData("update", "{\"id\":1,\"name\":\"xxx\"}")]
    public void RequestValidator_JsonBody_ShouldReturnOk(string method, string body)
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/categories",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Body = body
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Equal("1", result.Status);
        Assert.Equal("Ok", result.Body);
    }

    [Theory]
    [InlineData("create", "xxx")]
    [InlineData("update", "xxx")]
    public void RequestValidator_NoJsonBody_ShouldReturnIllegalBody(string method, string body)
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/categoies",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Body = body
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Contains("4", result.Status);
        Assert.Contains("illegal body", result.Body);

    }

    [Fact]
    public void RequestValidator_ValidGetRequest_ShouldReturnTrue()
    {
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/categories/1",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };

        var result = requestValidator.ValidateRequest(request);

        Assert.Equal("1", result.Status);
        Assert.Equal("Ok", result.Body);
    }

    //////////////////////////////////////////////////////////
    /// 
    /// Testing CategoryService class
    /// 
    //////////////////////////////////////////////////////////

    [Fact]
    public void CategoryService_GetAllCategories_ShouldReturnAllCategories()
    {
        CategoryService categoryService = new CategoryService();

        var categories = categoryService.GetCategories();

        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
    }

    [Fact]
    public void CategoryService_GetCategoryById_ShouldReturnCorrectCategory()
    {
        var categoryService = new CategoryService();

        var category = categoryService.GetCategory(2);

        Assert.NotNull(category);
        Assert.Equal(2, category.Id);
        Assert.Equal("Condiments", category.Name);
    }

    [Fact]
    public void CategoryService_GetCategoryById_NonExistent()
    {
        var categoryService = new CategoryService();

        var category = categoryService.GetCategory(-1);

        Assert.Null(category);
    }

    [Fact]
    public void CategoryService_UpdateCategory_ShouldUpdateSuccessfully()
    {
        var categoryService = new CategoryService();

        var result = categoryService.UpdateCategory(1, "UpdatedName");
        var updatedCategory = categoryService.GetCategory(1);

        Assert.True(result);
        Assert.NotNull(updatedCategory);
        Assert.Equal("UpdatedName", updatedCategory.Name);
    }

    [Fact]
    public void CategoryService_UpdateCategory_NonExistent()
    {
        var categoryService = new CategoryService();

        var result = categoryService.UpdateCategory(-1, "UpdatedName");

        Assert.False(result);
    }

    [Fact]
    public void CategoryService_DeleteCategory_ShouldDeleteSuccessfully()
    {
        var categoryService = new CategoryService();

        var result = categoryService.DeleteCategory(3);
        var deletedCategory = categoryService.GetCategory(3);

        Assert.True(result);
        Assert.Null(deletedCategory);
    }

    [Fact]
    public void CategoryService_DeleteCategory_NonExistent()
    {
        var categoryService = new CategoryService();

        var result = categoryService.DeleteCategory(-1);

        Assert.False(result);
    }

    [Fact]
    public void CategoryService_CreateCategory_ShouldCreateSuccessfully()
    {
        var categoryService = new CategoryService();

        var result = categoryService.CreateCategory(4, "NewCategory");
        var newCategory = categoryService.GetCategory(4);

        Assert.True(result);
        Assert.NotNull(newCategory);
        Assert.Equal(4, newCategory.Id);
        Assert.Equal("NewCategory", newCategory.Name);
    }

    [Fact]
    public void CategoryService_CreateCategory_DuplicateId()
    {
        var categoryService = new CategoryService();

        var result = categoryService.CreateCategory(1, "DuplicateCategory");

        Assert.False(result);
    }
}

