namespace Mushka.WebApi.ClientModels
{
    public class ValidationResponseModel : ResponseModelBase
    {
        public ValidationRModel Data { get; set; }
    }

    public class ValidationRModel
    {
        public bool IsValid { get; }

        public ValidationRModel(bool isValid)
        {
            IsValid = isValid;
        }
    }
}