
# SW.HttpExtensions
[![Build Status](https://dev.azure.com/simplify9/Github%20Pipelines/_apis/build/status/simplify9.HttpExtensions?branchName=master)]
| **Package**       | **Version** |
| :----------------:|:----------------------:|
| ``SimplyWorks.HttpExtensions``|![Nuget](https://img.shields.io/nuget/v/SimplyWorks.HttpExtensions?style=for-the-badge)|

[SW.HttpExtensions](https://www.nuget.org/packages/SimplyWorks.HttpExtensions/) is a lightweight set of extensions made for abstracting HTTP tasks. Supports JavaScript servers. 

- **HttpContent Extensions**: JSON response casting.
- **HttpClientExtensions**: POST JSON object streamlining. 
- **HttpClientFactoryExtensions**: Streamlining authorization and base address initialization. 

# HttpContent Extensions
- Adds a method `ReadAsAsync` to cast JSON result into a type passed as a generic argument.

# HttpClientExtensions
- Adds a `PostAsync` function that serializes an object `payload` into a JSON string.

# HttpClientFactoryExtensions
- Adds Auth Token (*JWT*) initializations and BaseAddress in `HttpClient` creation overload.

## Getting support 👷
If you encounter any bugs, don't hesitate to submit an [issue](https://github.com/simplify9/HttpExtensions/issues). We'll get back to you promptly!
