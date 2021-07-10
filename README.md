# 👋 Baseline.Validate

Minimal, magic free, configuration over convention validation constructs for modern .NET projects.

```csharp
public class UserRequestValidator : SyncValidator<UserRequest>
{
    public override ValidationResult Validate(UserRequest toValidate)
    {
        if (toValidate == null)
        {
            return FailureFor("request", ":parameter was not sent.");
        }

        if (string.IsNullOrWhiteSpace(toValidate.Name))
        {
            return FailureFor(tv => tv.Name, ":parameter is required.");
        }
    }
}

// ...

new UserRequestValidator().Validate(userRequest);
```

## 👥 Contributing

To learn more about contributing to the project, please read our contribution guidelines available at the documentation 
link below.

## 📕 Documentation

Documentation for this project is available on our project documentation site: https://docs.getbaseline.net/validate/.
