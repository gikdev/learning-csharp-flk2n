using System.ComponentModel.DataAnnotations;

namespace DotNetApi.Dtos;

public class UserToAddDto {
  [Required]
  [StringLength(50, MinimumLength = 2)]
  [Display(Name = "First Name", Description = "User's given name")]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  [StringLength(50)]
  public string LastName { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  [Display(Description = "User's email address")]
  public string Email { get; set; } = string.Empty;

  [Required]
  [RegularExpression("Male|Female|Other")]
  [Display(Description = "Gender of the user")]
  public string Gender { get; set; } = "Other";

  [Display(Description = "Whether the user account is active")]
  public bool Active { get; set; } = true;
}
