using System.ComponentModel.DataAnnotations;

using FluentValidation;

using Services.DTOs.HistoryDTOs;

namespace Services.DtoValidators
{
    public class UpdateHistoryDtoValidator : AbstractValidator<UpdateHistoryDto>
    {
        public UpdateHistoryDtoValidator()
        {
            RuleFor(x => x.MangaExternalId)
                .NotEmpty().WithMessage("Hello form validator. MangaExternalId is required.");

            RuleFor(x => x.LastChapterId)
                .NotEmpty().WithMessage("LastChapterId is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.");

            RuleFor(x => x.LastChapterTitle)
                .NotEmpty().WithMessage("LastChapterTitle is required.");

            RuleFor(x => x.LastChapterNumber)
                .GreaterThan(0).WithMessage("LastChapterNumber must be greater than zero.");
        }
    }
}
