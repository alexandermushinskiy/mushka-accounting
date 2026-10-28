using System.ComponentModel.DataAnnotations;

namespace Mushka.Api.ClientModels.Category;

public class CreateCategoryRequestModel
{
    [Required]
    public string Name { get; set; }

    public bool IsSizeRequired { get; set; }

    public bool IsAdditional { get; set; }
}
