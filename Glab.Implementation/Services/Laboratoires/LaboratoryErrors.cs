using GLAB.Domains.Shared;

namespace Glab.Implementation.Services.Laboratoires
{
    public class LaboratoryErrorsService
    {
        public static ErrorCode LaboratoryIdEmpty { get; } =
           new ErrorCode("LaboratoryErrors.LaboratoryIdEmpty", "The laboratory's id is Empty");


    }
}