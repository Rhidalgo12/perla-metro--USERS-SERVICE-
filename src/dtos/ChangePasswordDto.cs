using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.src.dtos
{
    public class ChangePasswordDto
    {
    [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 20 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,20}$", 
    ErrorMessage = "La contraseña debe incluir al menos una mayúscula, una minúscula, un número y un carácter especial.")]
    public string CurrentPassword { get; set; } = null!;

    
    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 20 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,20}$", 
    ErrorMessage = "La contraseña debe incluir al menos una mayúscula, una minúscula, un número y un carácter especial.")]
    public string NewPassword { get; set; } = null!;
    }
}