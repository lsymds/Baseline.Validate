# 👋 Baseline.Validate

> This repository has been archived. Whilst there were grand plans for it initially, you're probably better off with one of the many existing validation libraries out there or just writing your own classes.

> Documentation remains up but not linked to on the documentation website.

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

## 🗿 Licensing

Baseline.Validate is licensed under the permissive MIT license. More information is available within this repository's
LICENSE file located here: [https://github.com/getbaseline/Baseline.Validate/blob/main/LICENSE](https://github.com/getbaseline/Baseline.Validate/blob/main/LICENSE)
