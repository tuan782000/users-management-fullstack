# We use these packages:

-   "Microsoft.AspNetCore.Authentication.JwtBearer"
-   "Microsoft.AspNetCore.Identity.EntityFrameworkCore"
-   "Microsoft.EntityFrameworkCore.SqlServer"
-   "Microsoft.EntityFrameworkCore.Tools"

# Cấu hình cơ sở dữ liệu:

appsettings.json

1. Constants:

-   Lưu trữ các giá trị hằng số (constants) được sử dụng trong toàn bộ dự án. Ví dụ như chuỗi kết nối cơ sở dữ liệu,
    các giá trị cố định hoặc chuỗi xác định vai trò, quyền hạn.

-   Điều này giúp mã nguồn trở nên dễ bảo trì hơn khi cần thay đổi các giá trị cố định.

2. DbContext:

-   Chứa lớp DbContext để quản lý và tương tác với cơ sở dữ liệu thông qua Entity Framework Core.
    Lớp này đại diện cho phiên làm việc với cơ sở dữ liệu, bao gồm các bộ truy vấn và cập nhật.

-   Định nghĩa các DbSet<T> cho các thực thể (entities) trong dự án.

3. Dtos (Data Transfer Objects):

-   Chứa các lớp DTO, được dùng để chuyển dữ liệu giữa các tầng của ứng dụng (ví dụ từ API đến service).
    DTO giúp đảm bảo dữ liệu được truyền một cách an toàn và dễ kiểm soát hơn so với việc dùng trực tiếp các thực thể (entities).

4. Entities:

-   Chứa các lớp thực thể (entities), đại diện cho các bảng trong cơ sở dữ liệu.
    Mỗi thực thể tương ứng với một bảng và chứa các thuộc tính (fields) tương ứng với các cột trong bảng đó.

5. Interfaces:

-   Chứa các interface (giao diện) định nghĩa các hợp đồng (contracts) cho các service hoặc repository.
    Việc sử dụng interface giúp việc quản lý phụ thuộc dễ dàng hơn và hỗ trợ tiêm phụ thuộc (dependency injection).

6. Services:

-   Chứa các lớp service, cung cấp logic nghiệp vụ của ứng dụng. Các service sẽ thực thi các interface được định nghĩa trong thư mục Interfaces.
    Đây là nơi các tác vụ chính như xử lý dữ liệu, tương tác với repository và gọi các phương thức liên quan được thực hiện.

# Create entities

Mình sẽ tạo ApplicationUser.cs

```C#

using System;
using Microsoft.AspNetCore.Identity;

namespace backend_dotnet7.Core.Entities
{
	public class ApplicationUser:IdentityUser
	{
		public ApplicationUser()
		{
		}
	}
}


```

ApplicationUser:IdentityUser Đang kế thừa IdentityUser

IdentityUser:

IdentityUser là lớp có sẵn trong ASP.NET Core Identity, cung cấp các thuộc tính và chức năng cần thiết để quản lý người dùng. Nó bao gồm các thuộc tính mặc định như:

-   UserName: Tên người dùng.
-   Email: Địa chỉ email.
-   PhoneNumber: Số điện thoại.
-   PasswordHash: Mã băm mật khẩu.
-   Và nhiều thuộc tính khác liên quan đến việc quản lý danh tính người dùng.

```C#
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace backend_dotnet7.Core.Entities
{
	public class ApplicationUser:IdentityUser
	{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}


```

get; set;: Cho phép lấy (get) và thiết lập (set) giá trị của thuộc tính FirstName.

[NotMapped]: Như đã giải thích trước, đây là một thuộc tính chỉ định rằng Roles sẽ không được lưu trữ trong cơ sở dữ liệu. Điều này có nghĩa là Entity Framework sẽ bỏ qua thuộc tính này khi tạo bảng trong cơ sở dữ liệu.

public IList<string> Roles { get; set; }:

IList<string> là một danh sách các chuỗi (List of strings), trong đó mỗi chuỗi đại diện cho một vai trò (role) mà người dùng có thể có.

Roles có thể được sử dụng để lưu trữ tạm thời vai trò của người dùng mà không cần phải lưu vào cơ sở dữ liệu.

# Tạo ra 1 file Entity chung

Giải quyết các vấn để các thuộc tính hay lặp lại - tạo file chung xong các file khác kế thừa lại

```c#
using System;
namespace backend_dotnet7.Core.Entities
{
	public class BaseEnt<TID>
	{
		public TID Id { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;
	}
}


```

Tôi đang tạo ra 1 entity chung để các entities khác kế thừa lại - TID là 1 id hỗn hợp

lớp BaseEnt<TID> là một lớp cơ sở tổng quát (generic base class),
và việc sử dụng tham số kiểu TID giúp tạo ra một lớp linh hoạt có thể xử lý nhiều kiểu dữ liệu cho thuộc tính Id (bao gồm chuỗi, số nguyên, hoặc các kiểu dữ liệu khác).

Ý nghĩa và lợi ích của <TID>:

1. Tính tổng quát (Generics):

-   TID là một tham số kiểu cho phép bạn định nghĩa kiểu dữ liệu của thuộc tính Id khi kế thừa lớp BaseEnt.

-   Khi một lớp thực thể kế thừa từ BaseEnt, bạn có thể chỉ định kiểu dữ liệu cho Id dựa trên nhu cầu cụ thể (ví dụ: kiểu string cho GUID hoặc kiểu int cho số nguyên).

    2.Tính linh hoạt:

-   Bạn có thể sử dụng BaseEnt cho nhiều lớp khác nhau mà không cần định nghĩa lại thuộc tính Id với từng kiểu dữ liệu.
    Điều này giúp bạn giảm bớt sự trùng lặp mã và quản lý các lớp kế thừa dễ dàng hơn.

    3.Dễ dàng mở rộng:

-   Nếu có nhiều lớp thực thể với Id là các kiểu dữ liệu khác nhau (ví dụ: int, string, hoặc Guid),
    bạn chỉ cần định nghĩa một lớp BaseEnt<TID>, và các lớp thực thể con sẽ kế thừa lớp này với kiểu dữ liệu Id phù hợp.

# Sử dụng Entity chung để tạo thửEntity

```c#
using System;
namespace backend_dotnet7.Core.Entities
{
	public class Log: BaseEntity<long>
    {
		public string? UserName { get; set; }
		public string Description { get; set; }
	}
}


```

BaseEntity<long> ở trên dùng TID là hỗ hợp chuỗi và số đều chấp nhận báo nó là long thì kế thừa lại và đặt là long

?: cho phép null

# Tạo thực thể Message

Cũng kế thừa lại BaseEnity tinh gọn code hơn và bổ sung các thuộc tính của riêng nó

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Entities
{
    public class Message:BaseEntity<long>
    {
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set;}
        public string Text { get; set; }
    }
}

```

# Tạo DBContext

Thiết lặp ApplicationDbContext và đổi tên 1 số bảng mặc định lại thành theo ý của mình

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        #endregion
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Config anything we want
            //1
            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
            });
            //2
            builder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("UserClaims");
            });
            //3
            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogins");
            });
            //4
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserTokens");
            });
            //5
            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Roles");
            });
            //6
            builder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("RoleClaims");
            });
            //7
            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UserRoles");
            });
        }
        #endregion
    }
}
```

IdentityDbContext<ApplicationUser>: Lớp này cho phép bạn sử dụng ASP.NET Core Identity với Entity Framework Core. ApplicationUser là lớp người dùng của bạn, được kế thừa từ IdentityUser.

ApplicationDbContext: Đây là lớp quản lý kết nối với cơ sở dữ liệu và ánh xạ các thực thể (entities) vào các bảng trong cơ sở dữ liệu.

DbContextOptions<ApplicationDbContext>: Đây là các tùy chọn cấu hình cho DbContext, được truyền vào từ bên ngoài (thường là từ Startup.cs khi thiết lập dịch vụ cho Entity Framework Core).

: base(options): Đây là cách gọi Constructor của lớp cơ sở (IdentityDbContext<ApplicationUser>), truyền các tùy chọn cấu hình cho lớp cha.

DbSet<Log>: Đại diện cho bảng Logs trong cơ sở dữ liệu, ánh xạ đến thực thể Log.

DbSet<Message>: Đại diện cho bảng Messages, ánh xạ đến thực thể Message.

OnModelCreating(ModelBuilder builder): Phương thức này dùng để tùy chỉnh mô hình dữ liệu khi Entity Framework Core tạo cơ sở dữ liệu. Bạn có thể thay đổi cách các bảng được tạo, đổi tên bảng, thêm ràng buộc, v.v.

base.OnModelCreating(builder): Gọi phương thức từ lớp cha để giữ nguyên các thiết lập cơ bản của ASP.NET Identity.

Các đoạn dưới đổi tên bảng mặc định lại thành theo ý mình

# Kết nối đến sql server

Mình sẽ vào Progran.cs để làm các đoạn code kết nối sql server

appsetting.json

"local": "Server=.;Database=UserManagmentFullStackDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"

program.cs

Bổ sung đoạn sau

```c#
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
});
```

Sử dụng cú pháp

Add-migration init

database-update

Hoặc

dotnet ef migrations add init

dotnet ef database update

# Config Enums in Program.cs

```c#
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
```

.AddJsonOptions(options =>
{
options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

cho phép sử dụng enums

cấu hình cho dự án có thể sử dụng được enums

Tạo folder Auth trong Dtos

LoginDto.cs

Dùng cho dịch vụ đăng nhập

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
```

LoginServiceResponse.cs

Phản hồi thông tin người dùng

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class LoginServiceResponseDto
    {
        public string NewToken { get; set; }
        // this would be returned to front-end
        public UserInfoResult UserInfo { get; set; }
    }
}
```

MeDto.cs

Thông tin người dùng

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class MeDto
    {
        public string Token { get; set; }
    }
}
```

RegisterDto.cs

Đăng ký

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
```

UpdateRoleDto.cs

Cập nhật role cho người dùng

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        public RoleType NewRole { get; set; }
    }

    public enum RoleType {
        ADMIN,
        MANAGER,
        USER
    }
}

// 4 role trong dự án này
// ADMIN MANAGER USER và OWNER - Nhưng vì OWNER cao nhất không hiển thị phía client nên chỉ cần 3 cái
```

UserInfoResult.cs

kết quả cuối cùng

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UserInfoResult
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
```

# Constants

StaticUserRoles

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Constants
{
    // This class will be used to avoid typing errors - Lớp này sẽ được sử dụng để avoid typing errors
    public static class StaticUserRoles
    {
        public const string OWNER = "OWNER";
        public const string ADMIN = "ADMIN";
        public const string MANAGER = "MANAGER";
        public const string USER = "USER";

        public const string OwnerAdmin = "OWNER,ADMIN";
        public const string OwnerAdminManager = "OWNER,ADMIN,MANAGER";
        public const string OwnerAdminManagerUser = "OWNER,ADMIN,MANAGER,USER";
    }
}
```

# Add Identity

```c#
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

Cấu hình dịch vụ Identity với các lớp ApplicationUser và IndetityRole.

ApplicationUser: lớp người dùng tùy chỉnh kế thừa IndetityUser và IndetityRole - liên quan cấu hình và token

# Config identity

// Config Identity
builder.Services.Configure<IdentityOptions>(options => {
options.Password.RequiredLength = 8;
options.Password.RequireDigit = false;
options.Password.RequireLowercase = false;
options.Password.RequireUppercase = false;
options.Password.RequireNonAlphanumeric = false;
options.SignIn.RequireConfirmedAccount = false;
options.SignIn.RequireConfirmedEmail = false;
options.SignIn.RequireConfirmedPhoneNumber = false;
});

# Config JWT

```c#
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });





    ....

    app.UseAuthentication();


```

```json
 "JWT": {
        "ValidIssuer": "https://localhost:7237",
        "ValidAudience": "https://localhost:3000",
        "Secret": "SDFASDdsfsdkfoi3249rfmASDFetofoip32094u32476tchSADAFi23o487kdjkjfh"
    }
```

1. Cấu hình Authentication và JwtBearer
   AddAuthentication: Cấu hình hệ thống xác thực cho ứng dụng.
   DefaultScheme: Cấu hình mặc định cho hệ thống xác thực là sử dụng JWT Bearer (JwtBearerDefaults.AuthenticationScheme).
   DefaultAuthenticateScheme: Cấu hình mặc định khi xác thực là dùng JWT.
   DefaultChallengeScheme: Cấu hình mặc định khi gặp lỗi xác thực (thách thức), JWT sẽ được sử dụng để xử lý.
2. Cấu hình JWT Bearer
   AddJwtBearer: Thêm và cấu hình JWT Bearer vào hệ thống xác thực.
   SaveToken = true: Chỉ ra rằng token đã xác thực sẽ được lưu lại trong HttpContext sau khi xác thực.
   RequireHttpsMetadata = false: Không bắt buộc sử dụng HTTPS cho các yêu cầu (thường sử dụng trong môi trường phát triển, khi không cần mã hóa HTTPS).
   TokenValidationParameters: Các thông số để xác thực token.
   ValidateIssuer = true: Bắt buộc kiểm tra Issuer (nguồn phát hành token).
   ValidateAudience = true: Bắt buộc kiểm tra Audience (người nhận token).
   ValidIssuer: Xác định nguồn phát hành token hợp lệ (lấy từ cấu hình ứng dụng: JWT:ValidIssuer).
   ValidAudience: Xác định người nhận hợp lệ của token (lấy từ cấu hình: JWT:ValidAudience).
   IssuerSigningKey: Chìa khóa bí mật (secret key) để mã hóa và giải mã token, được cấu hình từ JWT:Secret.
   Mục đích của đoạn mã:
   Đoạn mã trên dùng để thiết lập hệ thống xác thực cho ứng dụng bằng JWT. Khi một yêu cầu (request) đến, token từ phía client sẽ được xác thực dựa trên các thông số đã cấu hình như Issuer, Audience, và SigningKey. Nếu token hợp lệ, yêu cầu sẽ được chấp nhận và xử lý.

ValidIssuer Issuer là đơn vị phát hành token. Ở đây, giá trị là URL của server phát hành token,

ValidAudience Audience là đối tượng được cấp quyền sử dụng token.

Secret là chìa khóa bí mật (secret key) được sử dụng để mã hóa và giải mã token.

# Tạo Controllers

tạo AuthController

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

    }
}
```

Tạo thử ...

# Interfaces

IAuthService, chứa các phương thức liên quan đến xử lý xác thực (authentication) và quản lý người dùng. Đây là phần định nghĩa mà các lớp khác sẽ triển khai để thực hiện logic cụ thể. Hãy phân tích từng phương thức trong interface này:

Task<GeneralServiceResponseDto> SeedRolesAsync()

Phương thức này thực hiện việc khởi tạo (seed) các vai trò (roles) mặc định cho hệ thống.

Kết quả trả về: Một đối tượng GeneralServiceResponseDto, chứa thông tin phản hồi về việc seed roles (thành công, thất bại, thông điệp).

Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto)

Thực hiện việc đăng ký người dùng mới.

Tham số đầu vào:

    - RegisterDto registerDto: Chứa thông tin đăng ký của người dùng (tên, email, mật khẩu, v.v.).

Kết quả trả về: Một GeneralServiceResponseDto để biết kết quả đăng ký (thành công hay thất bại).

Task<LoginServiceResponseDto> LoginAsync(LoginDto loginDto)

Xử lý đăng nhập người dùng.

Tham số đầu vào:

    - LoginDto loginDto: Chứa thông tin đăng nhập (tên đăng nhập, mật khẩu).

Kết quả trả về: Một đối tượng LoginServiceResponseDto, chứa kết quả đăng nhập (JWT token, thông tin người dùng, v.v.).

Task<GeneralServiceResponseDto> UpdateRolesAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)

Thực hiện cập nhật vai trò (roles) của người dùng.

Tham số đầu vào:

    - ClaimsPrincipal User: Đối tượng người dùng hiện tại (dựa trên JWT token).

    - UpdateRoleDto updateRoleDto: Chứa thông tin về vai trò mới của người dùng.

Kết quả trả về: GeneralServiceResponseDto để biết kết quả cập nhật vai trò.

Task<LoginServiceResponseDto> MeAsync(MeDto meDto) - Trả về thông tin cá nhân của người dùng đã đăng nhập.

Tham số đầu vào:

    - MeDto meDto: Dữ liệu có thể bao gồm thông tin yêu cầu (như ID người dùng).

Kết quả trả về: LoginServiceResponseDto, chứa thông tin chi tiết của người dùng hiện tại.

Task<IEnumerable<UserInfoResult>> GetUsersListAsync()

    - Lấy danh sách tất cả người dùng.

Kết quả trả về: Một danh sách đối tượng UserInfoResult, chứa thông tin người dùng.

Task<UserInfoResult> GetUserDetailsByUserName(string userName)

    - Lấy thông tin chi tiết của một người dùng dựa trên tên người dùng (userName).

Tham số đầu vào:

string userName: Tên người dùng để lấy thông tin.

Kết quả trả về: Một đối tượng UserInfoResult chứa thông tin chi tiết của người dùng.

Task<IEnumerable<string>> GetUsernameListAsync()

    - Lấy danh sách các tên người dùng hiện có trong hệ thống.

Kết quả trả về: Một danh sách các chuỗi (string), đại diện cho các tên người dùng.

Tóm lại giống như metod: Task<Mong_muốn_kết_quả_trả_về> Tên_hàm(Tham_số_truyền_vào)

**thể hiện tính trừu tượng (Abstraction)**

1. Task SaveNewLog(string UserName, string Description)

    - Lưu một log mới vào hệ thống.

Tham số:
UserName: Tên người dùng thực hiện hành động cần được lưu lại.
Description: Mô tả sự kiện hoặc hành động cần lưu.

Kết quả trả về: Không có kết quả trả về rõ ràng (trả về Task, có nghĩa là xử lý bất đồng bộ).

2. Task<IEnumerable<GetLogDto>> GetLogsAsync()

    - Lấy tất cả các log trong hệ thống.

    Kết quả trả về: Một danh sách các đối tượng GetLogDto, chứa thông tin về các sự kiện đã được ghi nhận.

3. Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)

    - Lấy danh sách các log liên quan đến người dùng hiện tại.

    Tham số:
    ClaimsPrincipal User: Thông tin người dùng hiện tại (thường được lấy từ JWT token).

    Kết quả trả về: Một danh sách các log (IEnumerable<GetLogDto>) liên quan đến người dùng này.

một interface tên là IMessageService, dùng để quản lý các chức năng liên quan đến tin nhắn (message) của hệ thống. Dưới đây là mô tả các phương thức của interface theo cấu trúc OOP:

1. Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
   Phương thức này dùng để tạo một tin nhắn mới.

Tham số:
ClaimsPrincipal User: Thông tin người dùng hiện tại (người tạo tin nhắn), được lấy từ token JWT.

CreateMessageDto createMessageDto: Đối tượng chứa thông tin về nội dung tin nhắn cần tạo.
Kết quả trả về: Trả về đối tượng GeneralServiceResponseDto để biết kết quả tạo tin nhắn (thành công hoặc thất bại).

2. Task<IEnumerable<GetMessageDto>> GetMessageAsync()

Lấy danh sách tất cả các tin nhắn.

Kết quả trả về: Một tập hợp (IEnumerable) các đối tượng GetMessageDto, chứa thông tin về các tin nhắn trong hệ thống.

3. Task<IEnumerable<GetMessageDto>> GetMyMessageAsync(ClaimsPrincipal User)

Lấy danh sách các tin nhắn của người dùng hiện tại.

Tham số:

ClaimsPrincipal User: Thông tin người dùng hiện tại (được lấy từ token JWT).
Kết quả trả về: Một tập hợp các đối tượng GetMessageDto, chứa thông tin về tin nhắn của người dùng hiện tại.

# Add and Implement LogService

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class LogService : ILogService
    {
        public Task<IEnumerable<GetLogDto>> GetLogsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
        {
            throw new NotImplementedException();
        }

        public Task SaveNewLog(string UserName, string Description)
        {
            throw new NotImplementedException();
        }
    }
}
```

LogService là một lớp (class) kế thừa (hoặc chính xác hơn là thực hiện) interface ILogService. Lớp này định nghĩa các phương thức cần thiết để làm việc với hệ thống log theo cách riêng của nó. Hiện tại, các phương thức trong LogService vẫn chưa được cài đặt chi tiết và sử dụng throw new NotImplementedException(); để báo rằng chúng chưa được triển khai.

Kế thừa interface (ILogService):
Khi lớp LogService triển khai (implement) interface ILogService, nó phải cung cấp các triển khai cụ thể cho tất cả các phương thức đã định nghĩa trong interface.
Interface ILogService định nghĩa ba phương thức: SaveNewLog, GetLogsAsync, và GetMyLogsAsync. Vì vậy, lớp LogService cần cung cấp cách thực hiện cho các phương thức này.
Các phương thức trong LogService:
Task<IEnumerable<GetLogDto>> GetLogsAsync():

Lớp LogService cần triển khai logic để lấy tất cả các log từ hệ thống.
Hiện tại, phương thức này chưa được thực hiện (vẫn đang sử dụng throw new NotImplementedException();).
Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User):

Phương thức này sẽ trả về các log chỉ liên quan đến người dùng hiện tại (được xác định thông qua đối tượng ClaimsPrincipal).
Cũng như phương thức trước, chưa có logic thực thi cho phương thức này.
Task SaveNewLog(string UserName, string Description):

Phương thức này dùng để lưu một log mới với tên người dùng và mô tả sự kiện.
Chưa có cài đặt chi tiết cho phương thức này.

```c#
        // Lưu Log lại
        public async Task SaveNewLog(string UserName, string Description)
        {
            var newLog = new Log(){
                UserName = UserName,
                Description = Description
            };

            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }

        // Lấy ra danh sách logs để theo ngày, mô tả, name sắp xếp kế quả theo thứ tự giảm dần của ngày - kết quả trả về là danh sách
        public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
        {
            var logs = await _context.Logs.Select(q => new GetLogDto {
                CreatedAt = q.CreatedAt,
                Description = q.Description,
                UserName = q.UserName
            }).OrderByDescending(q => q.CreatedAt).ToListAsync();
             return logs;
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
        {
            var logs = await _context.Logs
            .Where(q =>  q.UserName == User.Identity.Name)
            .Select(q => new GetLogDto {
                CreatedAt = q.CreatedAt,
                Description = q.Description,
                UserName = q.UserName
            }).OrderByDescending(q => q.CreatedAt).ToListAsync();
             return logs;
        }
```

Đang sử dụng cú pháp linq

# Message

// Dependency Injection này gọi là DI quản lý và cung cấp các đối tượng
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAuthService, AuthService>();

Interface (Định nghĩa các hàm 1 lớp service sẽ thực thi) khai báo hàm - Service (nơi thực thi cụ thể tính năng đó) nơi viết code cụ thể

ví dụ

ILogService khai báo có bao nhiêu tính năng

LogService sẽ ánh xạ và viết ra đầy đủ các bước mà tính năng đó sẽ hoạt động

```c#

        #region GetMessagesAsync
        public async Task<IEnumerable<GetMessageDto>> GetMessageAsync()
        {
            var messages = await _context.Messages
            .Select(q => new GetMessageDto()
            {
                Id = q.Id,
                SenderUserName = q.SenderUserName,
                ReceiverUserName = q.ReceiverUserName,
                Text = q.Text,
                CreatedAt = q.CreatedAt
            })
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();

        return messages;
        }
        #endregion

        #region GetMyMessagesAsync
        public async Task<IEnumerable<GetMessageDto>> GetMyMessageAsync(ClaimsPrincipal User)
        {
            var loggedInUser = User.Identity.Name;

            var messages = await _context.Messages
                .Where(q => q.SenderUserName == loggedInUser || q.ReceiverUserName == loggedInUser)
            .Select(q => new GetMessageDto()
            {
                Id = q.Id,
                SenderUserName = q.SenderUserName,
                ReceiverUserName = q.ReceiverUserName,
                Text = q.Text,
                CreatedAt = q.CreatedAt
            })
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();

        return messages;
        }
         #endregion
```

1. GetMessagesAsync
   Mục đích: Hàm này được sử dụng để lấy tất cả các tin nhắn từ cơ sở dữ liệu, sắp xếp theo thời gian tin nhắn được tạo (CreatedAt) theo thứ tự giảm dần.
   Cách hoạt động:
   Dùng \_context.Messages để truy vấn dữ liệu từ bảng Messages.
   Sử dụng Select để chuyển đổi các tin nhắn từ cơ sở dữ liệu thành đối tượng GetMessageDto.
   Sắp xếp các tin nhắn theo CreatedAt giảm dần (OrderByDescending).
   Cuối cùng, gọi ToListAsync() để lấy danh sách tin nhắn dưới dạng danh sách bất đồng bộ (async list).

2. GetMyMessageAsync
   Mục đích: Hàm này được sử dụng để lấy các tin nhắn mà người dùng hiện tại đã gửi hoặc nhận. Người dùng hiện tại được lấy từ đối tượng ClaimsPrincipal (đối tượng chứa thông tin về người dùng đăng nhập).
   Cách hoạt động:
   Dùng User.Identity.Name để lấy tên người dùng hiện tại.
   Sử dụng \_context.Messages để truy vấn tin nhắn trong cơ sở dữ liệu.
   Dùng Where để lọc các tin nhắn có SenderUserName hoặc ReceiverUserName là tên người dùng hiện tại.
   Sử dụng Select để chuyển đổi các tin nhắn thành đối tượng GetMessageDto.
   Sắp xếp các tin nhắn theo CreatedAt theo thứ tự giảm dần.
   Cuối cùng, trả về danh sách các tin nhắn tương ứng với người dùng hiện tại.
   Tóm lại:
   GetMessagesAsync lấy tất cả các tin nhắn.
   GetMyMessageAsync lấy các tin nhắn liên quan đến người dùng đăng nhập (đã gửi hoặc nhận).

    Từ khóa new trong C# được sử dụng để tạo ra một thể hiện (instance) mới của một lớp hoặc kiểu dữ liệu. Nó khởi tạo đối tượng và gọi hàm dựng (constructor) của lớp đó.

    new GetMessageDto() có nghĩa là bạn đang tạo một đối tượng mới thuộc kiểu GetMessageDto. Sau khi tạo, bạn gán các giá trị từ đối tượng q (đại diện cho một bản ghi từ bảng Messages trong cơ sở dữ liệu) vào các thuộc tính tương ứng của đối tượng GetMessageDto này (như Id, SenderUserName, ReceiverUserName, Text, và CreatedAt).

    Tóm lại, new trong ngữ cảnh này dùng để tạo một thể hiện mới của lớp GetMessageDto để chứa dữ liệu tin nhắn.

# Auth Service

Tạo token

```c#
#region GenerateJWTTokenAsync
        private async Task<string> GenerateJWTTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
        };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: signingCredentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }
        #endregion
```

GenerateJWTTokenAsync dùng để tạo ra một JSON Web Token (JWT) cho người dùng (ApplicationUser). JWT là một chuẩn mở để truyền tải thông tin giữa các bên dưới dạng JSON, bảo mật thông qua chữ ký (signature).

```c#
var authClaims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim("FirstName", user.FirstName),
    new Claim("LastName", user.LastName),
};
```

Claim: Là những thông tin về người dùng được đưa vào token. Các claim này có thể là tên, ID, vai trò, hoặc các thông tin tùy chỉnh khác.
Danh sách authClaims này chứa các thông tin như:
Tên đăng nhập (ClaimTypes.Name)
ID người dùng (ClaimTypes.NameIdentifier)
Tên và họ của người dùng ("FirstName", "LastName").

Thêm vai trò (Roles) vào Claims

```c#
var userRoles = await _userManager.GetRolesAsync(user);

foreach (var userRole in userRoles)
{
    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
}

```

GetRolesAsync(user): Lấy danh sách các vai trò mà người dùng có.

Sau đó, thêm mỗi vai trò (role) của người dùng vào danh sách authClaims với ClaimTypes.Role.

Tạo khóa bảo mật (Secret Key) và Signing Credentials

```c#
var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

```

authSecret: Tạo khóa bảo mật (symmetric key) từ chuỗi bí mật được lưu trong cấu hình (\_configuration["JWT:Secret"]).

Chuỗi này được dùng để mã hóa và xác thực token, đảm bảo chỉ những ai có khóa bí mật mới có thể tạo hoặc xác thực JWT.

signingCredentials: Chữ ký số sử dụng thuật toán HmacSha256 cùng với khóa bảo mật authSecret. Điều này giúp đảm bảo rằng JWT không thể bị giả mạo.

Tạo đối tượng JWT

```c#
var tokenObject = new JwtSecurityToken(
    issuer: _configuration["JWT:ValidIssuer"],
    audience: _configuration["JWT:ValidAudience"],
    notBefore: DateTime.Now,
    expires: DateTime.Now.AddHours(3),
    claims: authClaims,
    signingCredentials: signingCredentials
);

```

JwtSecurityToken: Là đối tượng đại diện cho một JWT.

Các tham số quan trọng:
issuer: Định danh của bên phát hành token (thường là server hoặc ứng dụng của bạn).
audience: Định danh của bên nhận token (có thể là client hoặc service khác).
notBefore: Token sẽ không hợp lệ trước thời gian này (thời gian hiện tại).
expires: Token sẽ hết hạn sau 3 giờ kể từ khi được phát hành.
claims: Danh sách các thông tin (claims) đã tạo trước đó.
signingCredentials: Chữ ký số dùng để ký token nhằm bảo mật.

Chuyển JWT thành chuỗi trả về

```c#
string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
return token;

```

JwtSecurityTokenHandler.WriteToken: Chuyển đối tượng JwtSecurityToken thành một chuỗi JWT.

Cuối cùng, chuỗi JWT này được trả về và có thể được sử dụng cho người dùng để thực hiện các yêu cầu bảo mật (authentication) tới hệ thống.

Tóm lại:
Phương thức GenerateJWTTokenAsync tạo một JWT cho người dùng dựa trên:

Các thông tin cơ bản của người dùng (tên đăng nhập, ID, tên, họ).

Các vai trò của người dùng.

Token này sẽ có thời hạn sử dụng là 3 giờ và được bảo mật bằng khóa bí mật và thuật toán HmacSha256.

**Xem thêm comment bên trong code**

# Cấu hình thử controller của testController

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    // Phương Thức (GET/POST/PUT/DELETE) api/(TestsController -> bỏ Controller -> Tests)/(Route)
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        [HttpGet]
        [Route("get-public")]
        public IActionResult GetPublicData() {
            return Ok("Public Data");
        }

        [HttpGet]
        [Route("get-user-role")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public IActionResult GetUserData() {
            return Ok("User role data");
        }

        [HttpGet]
        [Route("get-manager-role")]
        [Authorize(Roles = StaticUserRoles.MANAGER)]
        public IActionResult GetManagerData() {
            return Ok("Manager role data");
        }

        [HttpGet]
        [Route("get-admin-role")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public IActionResult GetAdminData() {
            return Ok("Admin role data");
        }

        [HttpGet]
        [Route("get-owner-role")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public IActionResult GetOwnerData() {
            return Ok("Owner role data");
        }
    }
}
```

http://localhost:5112/api/Tests/get-public

Tên_miền/api/Tên_controller/route

TestsController : ControllerBase

-   TestsController kế thừa ControllerBase

HttpGet: Định danh phương thức

Route("get-public"): Cái này route

Nếu tính năng có yêu cầu quyền (role) thì dùng Authorize (StaticUserRoles tham chiếu vào để lấy ra role)

-   Nếu bạn không có role thì chặn 401 Undocumented Error: Unauthorized

# Viết controller cho LogsController

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        // sử dụng để lưu trữ tham chiếu đến service
        // chỉ thực hiện hành động liên quan đến ghi hoặc đọc log.
        private readonly ILogService _logService;

        // constructor của class LogsController
        public LogsController(ILogService logService)
        {
            _logService = logService;
            // để truy cập các phương thức của service mà không cần truyền lại trong các phương thức đó.
        }

        // OwnerAdmin sự kết hợp OWNER và ADMIN ở trong core constants
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetLogs()
        {
            var logs = await _logService.GetLogsAsync();
            return Ok(logs);
        }

        [HttpGet]
        [Route("mine")]
        [Authorize] // Bất kỳ ai đã đăng nhập vào không cần xét quyền thì đều dùng được
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetMyLogs()
        {
            var logs = await _logService.GetMyLogsAsync(User);
            return Ok(logs);
        }
    }
}
```

# Implement Message Controller
