using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elgoog.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elgoog.DAL.Models;

[Table("products")]
public class ProductModel : BaseModel, IModified
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Image { get; set; }

    public string Reference { get; set; }
    
    public DateTime DateModifiedUtc { get; set; }
}