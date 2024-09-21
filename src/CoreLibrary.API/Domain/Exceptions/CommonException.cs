using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CoreLibrary.API.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class CommonException : Exception
{
    public CommonExceptionModel ExceptionModel { get; } = new CommonExceptionModel();

    //The Message Property below should contain general message with a gist of errors occured where it will be useful in case
    //the exception is unhandled by Api filters it will be caught under global exception and will display general message that was provided here.
    public CommonException(CommonExceptionModel exceptionModel) : base(exceptionModel.Detail)
    {
        ExceptionModel = exceptionModel;
    }

    public CommonException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected CommonException(SerializationInfo info, StreamingContext context)
            : base(info, context)
    {
        ExceptionModel = (CommonExceptionModel)
            (info.GetValue("CommonExceptionModel", typeof(CommonExceptionModel)) ?? new CommonExceptionModel());
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("ExceptionModel", ExceptionModel);
        base.GetObjectData(info, context);
    }
}

[ExcludeFromCodeCoverage]
public class CommonExceptionModel
{
    public int HttpStatusCode { get; set; } = 500;
    public string Title { get; } = "Internal Server Error";
    public string Detail { get; } = "Internal Server Error";

    public CommonExceptionModel(int httpStatusCode, string title, string detail)
    {
        HttpStatusCode = httpStatusCode;
        Title = title;
        Detail = detail;
    }

    public CommonExceptionModel(string detail)
    {
        Detail = detail;
    }

    public CommonExceptionModel()
    {
    }
}
