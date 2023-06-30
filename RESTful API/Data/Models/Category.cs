using RESTful_API.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace RESTful_API.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category : BaseModel
{
    public string Name { get; set; }
}

