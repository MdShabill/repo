//using FluentValidation;
//using WebApiDemo1.DTO.InputDTO;
//using WebApiDemo1.Enums;

//public class ProductInputDtoValidator : AbstractValidator<ProductInputDto>
//{
//    public ProductInputDtoValidator()
//    {
//        RuleFor(x => x.ProductName)
//            .NotEmpty().WithMessage("Product name can not be blank")
//            .Length(3, 15).WithMessage("Product name should be between 3 and 15 characters");

//        RuleFor(x => x.BrandName)
//            .NotEmpty().WithMessage("Brand name can not be blank")
//            .Length(3, 15).WithMessage("Brand name should be between 3 and 15 characters");

//        RuleFor(x => x.Size)
//            .GreaterThan(25).WithMessage("Product size should be above 25");

//        RuleFor(x => x.Color)
//            .IsInEnum().WithMessage("Invalid Color");

//        RuleFor(x => x.Fit)
//            .Must(f => string.IsNullOrEmpty(f) || !f.Contains("Skinny fit"))
//            .WithMessage("Skinny Fit is not an accepted fitting option");


        
//        RuleFor(x => x.Fabric)
//            .Must(f => string.IsNullOrEmpty(f) || f.Contains("Polyester"))
//            .WithMessage("Polyester fabric is not accepted");




//        RuleFor(x => x.Category)
//            .Must(c => string.IsNullOrEmpty(c) || !c.Contains("Summer Wear"))
//            .WithMessage("Summer Wear category is not accepted");


//        RuleFor(x => x.Price)
//            .InclusiveBetween(400, 5000)
//            .WithMessage("Product price should be between 400 and 5000");
//    }
//}
