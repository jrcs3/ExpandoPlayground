using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExpandoPlayground;

[Table("Contacts")] // Specifies the table name if different from the class name
public class ContactDTO
{
    [Key] // Specifies this property as the primary key
    public int Id { get; set; }

    [Required] // Specifies that this property is required
    [StringLength(50)] // Specifies the maximum string length
    public string FirstName { get; set; }

    [StringLength(50)]
    public string MiddleName { get; set; } // Optional

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(100)]
    public string JobTitle { get; set; }

    [Required]
    public DateTime HiredDate { get; set; }

    public DateTime? FiredDate { get; set; } // Nullable DateTime for FiredDate

    [StringLength(100)]
    public string Position { get; set; }

    [StringLength(20)]
    public string HomePhone { get; set; }

    [StringLength(20)]
    public string WorkPhone { get; set; }

    [StringLength(20)]
    public string TollFreePhone { get; set; }

    [StringLength(20)]
    public string MobilePhone { get; set; }

    // Home Address Parts
    [StringLength(100)]
    public string HomeAddressLine1 { get; set; }

    [StringLength(100)]
    public string HomeAddressLine2 { get; set; }

    [StringLength(50)]
    public string HomeCity { get; set; }

    [StringLength(50)]
    public string HomeState { get; set; }

    [StringLength(10)]
    public string HomePostalCode { get; set; }

    [StringLength(50)]
    public string HomeCountry { get; set; }

    // Work Address Parts
    [StringLength(100)]
    public string WorkAddressLine1 { get; set; }

    [StringLength(100)]
    public string WorkAddressLine2 { get; set; }

    [StringLength(50)]
    public string WorkCity { get; set; }

    [StringLength(50)]
    public string WorkState { get; set; }

    [StringLength(10)]
    public string WorkPostalCode { get; set; }

    [StringLength(50)]
    public string WorkCountry { get; set; }

    // New fields
    [Required]
    public DateTime Birthdate { get; set; } // Adding Birthdate

    [Required]
    [StringLength(10)]
    public string Sex { get; set; } // Adding Sex

}
