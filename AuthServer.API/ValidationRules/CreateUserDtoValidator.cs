using AuthServer.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.API.ValidationRules
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email aşanı zorunludur.").EmailAddress().WithMessage("Email hatalı.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola alanı zorunludur.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı alanı zorunludur");
        }
    }
}
