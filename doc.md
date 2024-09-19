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
        public string ReplierUserName { get; set;}
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
