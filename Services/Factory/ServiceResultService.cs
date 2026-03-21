namespace devhouse.Services;

/// <summary>Includes static methods that allow us to transfer conditational logic to the controller</summary>
public class ServiceResult
{
    /// <summary>Protected property that represents a boolean value. This is set by ServiceResult static methods</summary>
    public bool notFound { get; protected set; } = false;

    /// <summary>Protected property that represents a boolean value. This is set by ServiceResult static methods</summary>
    public bool invalidCredentials { get; protected set; } = false;

    /// <summary>Protected property that represents a boolean value. This is set by ServiceResult static methods</summary>
    public bool unauthorized { get; protected set; } = false;

    /// <summary>Protected property that represents a boolean value. This is set by ServiceResult static methods</summary>
    public bool badRequest { get; protected set; } = false;

    public static ServiceResult InvalidCredentials() => new ServiceResult { invalidCredentials = true };

    /// <summary>Represents an unauthorized operation</summary>
    public static ServiceResult Unauthorized() => new ServiceResult { unauthorized = true };

    /// <summary>Represents a search with no match</summary>
    public static ServiceResult Notfound() => new ServiceResult { notFound = true };

    /// <summary>Represents bad input</summary>
    public static ServiceResult Badrequest() => new ServiceResult { badRequest = true };

    /// <summary>Represents a successful operation</summary>
    public static ServiceResult Success() => new ServiceResult();
}

/// <summary>Includes static methods that allow us to transfer conditational logic to the controller. This derived class allows us to transfer typed data</summary>
/// <typeparam name="T">Type safe DTO</typeparam>
public class ServiceResult<T> : ServiceResult
{

    /// <summary>Private property that represents a type. This is set by ServiceResult static methods</summary>
    public T? Data { get; private set; }

    /// <summary>Represents an entity/DTO</summary>
    public static ServiceResult<T> WithData(T data) => new ServiceResult<T> { Data = data };

    /// <summary>Represents lack of valid credentials</summary>
    public new static ServiceResult<T> InvalidCredentials() => new ServiceResult<T> { invalidCredentials = true };

    /// <summary>Represents an unauthorized operation</summary>
    public new static ServiceResult<T> Unauthorized() => new ServiceResult<T> { unauthorized = true };

    /// <summary>Represents a search with no match</summary>
    public new static ServiceResult<T> Notfound() => new ServiceResult<T> { notFound = true };

    /// <summary>Represents bad input</summary>
    public new static ServiceResult<T> Badrequest() => new ServiceResult<T> { badRequest = true };

}