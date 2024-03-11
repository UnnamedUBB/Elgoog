using System.ComponentModel.DataAnnotations;

namespace Elgoog.DAL.Models;

public class ItemModel : BaseModel
{
    [MaxLength(255)] public string Name { get; set; }

    public float Price { get; set; }

    [MaxLength(255)] public string Image { get; set; }

    [MaxLength(255)] public string Reference { get; set; }
}