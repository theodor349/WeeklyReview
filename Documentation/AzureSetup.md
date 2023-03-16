# Azure Active Directory
# App Registrations
## Functions
| Setting | Value | Comment |
|---------|-------|---------|
| Redirect URL | `https://{funcName}.azurewebsites.net/.auth/login/aad/callback` | Automatically set by Visual Studio |
| Scopes | `api://{clientId}/user_impersonation` | Automatically set by Visual Studio |

## Blazor Client
| Setting | Value | Comment |
|---------|-------|---------|
| Redirect URL | `https://localhost:5001/authentication/login-callback` | This will change when put into production |
| API Permissions | `{functionName}/user_impersonation` | `My APIs/{functionName}/Delegated/user_impersonation`  |

# Azure Functions
| Setting | Value | Comment |
|---------|-------|---------|
| AuthorizationLevel | `Function` | This makes sure that you can call the function if you also provide a kay in the url e.g. `https://{functionName}.azurewebsites.net/api/GetUserId?code={key}` |
| Dependency | `ClaimsPrincipal` | This enables the function to read information about the user e.g. oid |
| Authentication | Microsoft | This makes sure that all request must be sent with a valid `JWT` so that we can access user information |
| CORS | `https://localhost:7037` | This enables the client to call the function |
