using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.DTOs
{
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống.")
                .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự.");

            // Validate Đơn Nghỉ Phép hoặc Giải trình công
            When(x => x.Type == TicketType.LeaveRequest || x.Type == TicketType.AttendanceAdjustment, () =>
            {
                RuleFor(x => x.StartDate)
                    .NotNull().WithMessage("Ngày bắt đầu không được để trống.");

                RuleFor(x => x.EndDate)
                    .NotNull().WithMessage("Ngày kết thúc không được để trống.")
                    .GreaterThanOrEqualTo(x => x.StartDate)
                    .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
            });

            // Validate Đơn Thanh Toán / Hoàn Ứng
            When(x => x.Type == TicketType.ExpenseClaim, () =>
            {
                RuleFor(x => x.Amount)
                    .NotNull().WithMessage("Số tiền không được để trống.")
                    .GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0 VNĐ.");
            });
        }
    }
}
