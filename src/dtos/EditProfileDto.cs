using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.src.dtos
{
    /// <summary>
    /// Data Transfer Object for editing user profile information.
    /// </summary>
    public class EditProfileDto
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// The user's last name.
        /// </summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "Los apellidos solo pueden contener letras y espacios")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@perlametro\.cl$",
            ErrorMessage = "El correo debe pertenecer al dominio @perlametro.cl")]
        public string Email { get; set; } = string.Empty;
    }
}