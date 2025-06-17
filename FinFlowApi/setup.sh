read -p "Do you want to create a new project from scratch? (y/n): " choice

if [[ "$choice" == "y" ]]; then
    read -p "Enter your project name: " project_name
    echo "Creating project with name: $project_name"
    dotnet new web -n "$project_name"
    cd "$project_name" || exit
else
    read -p "Enter path to existing project: " project_path
    if [ -d "$project_path" ]; then
        cd "$project_path" || exit
    else
        echo "Directory not exist"
        exit 1
    fi
fi

if ls -la | grep -qE '\.csproj$|Program\.cs'; then
    mkdir -p Controllers DTOs Models Repositories Services Validators
    touch .env

    read -p "Enter a base name (e.g. User, Transaction): " naming
    echo "Generating files for: $naming"

    # Controller
    cat <<EOF > Controllers/"${naming}Controller.cs"
using Microsoft.AspNetCore.Mvc;
using ${project_name}.Services;
using ${project_name}.DTOs;
using ${project_name}.Validators;

namespace ${project_name}.Controllers;

[ApiController]
[Route("[controller]")]
public class ${naming}Controller : ControllerBase
{
    
}
EOF

    # Model
    cat <<EOF > Models/"${naming}.cs"
namespace ${project_name}.Models;

public class ${naming}
{
    
}
EOF

    # DTO
    cat <<EOF > DTOs/"${naming}Dto.cs"
namespace ${project_name}.DTOs;

public class ${naming}Dto
{
    
}
EOF

    # Service Interface
    cat <<EOF > Services/"I${naming}Service.cs"
using ${project_name}.DTOs;

namespace ${project_name}.Services;

public interface I${naming}Service
{
    
}
EOF

    # Service Implementation
    cat <<EOF > Services/"${naming}Service.cs"
using ${project_name}.DTOs;
using ${project_name}.Repositories;

namespace ${project_name}.Services;

public class ${naming}Service : I${naming}Service
{
    
}
EOF

    # Repository Interface
    cat <<EOF > Repositories/"I${naming}Repository.cs"
using ${project_name}.DTOs;

namespace ${project_name}.Repositories;

public interface I${naming}Repository
{
    
}
EOF

    # Repository Implementation
    cat <<EOF > Repositories/"${naming}Repository.cs"
using ${project_name}.DTOs;

namespace ${project_name}.Repositories;

public class ${naming}Repository : I${naming}Repository
{
    
}
EOF

    # Validator
    cat <<EOF > Validators/"${naming}DtoValidator.cs"
using FluentValidation;
using ${project_name}.DTOs;

namespace ${project_name}.Validators;

public class ${naming}DtoValidator : AbstractValidator<${naming}Dto>
{
    public ${naming}DtoValidator()
    {
        
    }
}
EOF

    echo "Project structure and files created!"

    # Packages
    dotnet add package Oracle.ManagedDataAccess.Core
    dotnet add package DotNetEnv
    dotnet add package FluentValidation
    dotnet add package FluentValidation.AspNetCore
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    dotnet add package Microsoft.AspNetCore.Identity
    dotnet add package Swashbuckle.AspNetCore

    echo "Packages installed!"

    # .gitignore
    echo -e ".env\nbin/\nobj/\nrequestResponse.txt" > .gitignore
    touch requestResponse.txt

else
    echo "Not a valid .NET project (missing .csproj or Program.cs)"
fi
