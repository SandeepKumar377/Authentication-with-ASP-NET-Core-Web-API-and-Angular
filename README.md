# Authentication with ASP.NET Core Web API and Angular

A complete example of an Angular frontend and ASP.NET Core Web API backend implementing user registration, login, JWT-based authentication, and role-based authorization.

## Overview

This project demonstrates a full-stack authentication and authorization system with:
- **Frontend**: Angular-based user interface with role-based guards
- **Backend**: ASP.NET Core Web API with JWT token authentication
- **Security**: JWT-based authentication, role-based authorization, and secure token handling
- **Features**: User registration, login, protected API routes, and role policies

## Technology Stack

### Backend
- **Framework**: ASP.NET Core Web API
- **Authentication**: JWT (JSON Web Tokens)
- **Authorization**: Role-Based Access Control (RBAC)
- **Database**: (Configure as needed)
- **Language**: C# (57.5% of codebase)

### Frontend
- **Framework**: Angular
- **Language**: TypeScript (30.5% of codebase)
- **Styling**: HTML (11.3%) and CSS (0.7%)
- **Features**: Role Guards, Token Management, Secure API Communication

## Project Structure

```
Authentication-with-ASP-NET-Core-Web-API-and-Angular/
├── Backend/                 # ASP.NET Core Web API
│   ├── Controllers/         # API endpoints
│   ├── Models/              # Data models
│   ├── Services/            # Business logic
│   ├── Middleware/          # Authentication middleware
│   └── appsettings.json     # Configuration
├── Frontend/                # Angular application
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/  # Reusable components
│   │   │   ├── guards/      # Route guards
│   │   │   ├── services/    # API services
│   │   │   ├── models/      # Data models
│   │   │   └── app.module.ts
│   │   └── assets/
│   └── angular.json
└── README.md
```

## Key Features

### Authentication
- **User Registration**: Create new user accounts with validation
- **User Login**: Authenticate users with credentials
- **JWT Tokens**: Secure token-based authentication
- **Token Refresh**: Refresh expired tokens seamlessly

### Authorization
- **Role-Based Access Control**: Different access levels for different user roles
- **Protected Routes**: Angular route guards for client-side protection
- **Protected API Endpoints**: ASP.NET Core policies for server-side protection
- **Role Policies**: Fine-grained authorization on the backend

### Security Features
- **Secure Token Handling**: Safe storage and transmission of authentication tokens
- **Password Security**: Secure password storage and validation
- **CORS**: Cross-Origin Resource Sharing configuration
- **Input Validation**: Both client and server-side validation

## Getting Started

### Prerequisites
- **.NET SDK**: Version 6.0 or higher
- **Node.js**: Version 14.0 or higher
- **npm**: Version 6.0 or higher
- **Angular CLI**: Latest version

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd Backend
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the database connection string in `appsettings.json`

4. Run migrations (if applicable):
   ```bash
   dotnet ef database update
   ```

5. Start the API server:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or configured port)

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd Frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update the API base URL in environment configuration files

4. Start the development server:
   ```bash
   ng serve
   ```

The application will be available at `http://localhost:4200`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Authenticate user and receive JWT token
- `POST /api/auth/refresh` - Refresh expired token
- `POST /api/auth/logout` - Logout user

### Protected Routes
All protected routes require a valid JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Configuration

### Backend Configuration (appsettings.json)
- JWT Secret Key
- Token Expiration Time
- CORS Origins
- Database Connection String
- Logging Levels

### Frontend Configuration
- API Base URL
- Token Storage
- HTTP Interceptors
- Route Guards

## Authentication Flow

1. **Registration**: User creates account with email and password
2. **Login**: User authenticates with credentials
3. **Token Issuance**: Backend generates JWT token
4. **Token Storage**: Angular stores token securely
5. **API Requests**: Token included in request headers
6. **Token Validation**: Backend validates token and user role
7. **Authorization**: Access granted/denied based on user role
8. **Token Refresh**: Automatic token refresh on expiration

## Role-Based Authorization

The system supports different user roles with corresponding permissions:
- **Admin**: Full access to all features
- **User**: Limited access based on policies
- **Guest**: Public access only

## Security Best Practices

- ✅ JWT tokens for stateless authentication
- ✅ Secure token storage on the client
- ✅ HTTPS for all communications
- ✅ Token expiration and refresh mechanism
- ✅ CORS policy enforcement
- ✅ Input validation and sanitization
- ✅ Role-based access control on both client and server
- ✅ Password hashing and security

## Usage Example

### Frontend - Login Component
```typescript
login(email: string, password: string) {
  this.authService.login(email, password).subscribe(
    (response) => {
      // Token stored by service
      this.router.navigate(['/dashboard']);
    },
    (error) => {
      // Handle login error
    }
  );
}
```

### Backend - Protected Controller
```csharp
[Authorize(Roles = "Admin")]
[HttpGet("admin-data")]
public IActionResult GetAdminData()
{
    return Ok(new { message = "Admin data" });
}
```

## Environment Variables

Create appropriate environment files for different configurations:
- `environment.ts` - Development
- `environment.prod.ts` - Production

## Deployment

### Backend
```bash
dotnet publish -c Release
# Deploy the published files to your hosting environment
```

### Frontend
```bash
ng build --prod
# Deploy the dist/ folder to your web server
```

## Testing

### Backend Unit Tests
```bash
dotnet test
```

### Frontend Unit Tests
```bash
ng test
```

### End-to-End Tests
```bash
ng e2e
```

## Troubleshooting

### Common Issues

**CORS Errors**
- Verify CORS configuration in ASP.NET Core
- Ensure frontend URL is added to allowed origins

**Token Expiration**
- Implement token refresh logic in HTTP interceptor
- Redirect to login on token expiration

**401 Unauthorized**
- Verify token is being sent in request headers
- Check token validity and expiration
- Ensure user has required role

**403 Forbidden**
- Verify user has required role for the resource
- Check role-based policies on backend

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or suggestions, please create an issue in the repository.

## Additional Resources

- [ASP.NET Core Security Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [Angular Security Guide](https://angular.io/guide/security)
- [JWT.io - JSON Web Tokens](https://jwt.io/)
- [OWASP Authentication Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html)

---

**Last Updated**: 2026-05-18

For the latest updates and contributions, visit the [GitHub Repository](https://github.com/SandeepKumar377/Authentication-with-ASP-NET-Core-Web-API-and-Angular)
