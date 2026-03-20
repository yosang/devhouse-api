namespace devhouse.Services;

/// <summary>Includes methods that create an instance of ServiceResult with specific boolean values based on the method used</summary>
public class ServiceResult
{
    public bool notFound { get; protected set; } = false;
    public bool invalidCredentials { get; protected set; } = false;
    public bool unauthorized { get; protected set; } = false;
    public bool badRequest { get; protected set; } = false;

    public static ServiceResult InvalidCredentials() => new ServiceResult { invalidCredentials = true };
    public static ServiceResult Unauthorized() => new ServiceResult { unauthorized = true };
    public static ServiceResult Notfound() => new ServiceResult { notFound = true };
    public static ServiceResult Badrequest() => new ServiceResult { badRequest = true };
    public static ServiceResult Success() => new ServiceResult();
}

/// <summary>Derived class that allows the use of DTO to return data</summary>
/// <typeparam name="T">Type safe DTO</typeparam>
public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> WithData(T data) => new ServiceResult<T> { Data = data };
    public new static ServiceResult<T> InvalidCredentials() => new ServiceResult<T> { invalidCredentials = true };
    public new static ServiceResult<T> Unauthorized() => new ServiceResult<T> { unauthorized = true };
    public new static ServiceResult<T> Notfound() => new ServiceResult<T> { notFound = true };
    public new static ServiceResult<T> Badrequest() => new ServiceResult<T> { badRequest = true };

}