using Microsoft.AspNetCore.Identity;

namespace BookTest.Services.Identity
{
    public class VietnameseIdentityDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
            => new() { Code = nameof(DefaultError), Description = "Đã xảy ra lỗi không xác định." };

        public override IdentityError ConcurrencyFailure()
            => new() { Code = nameof(ConcurrencyFailure), Description = "Dữ liệu đã bị thay đổi bởi người khác. Vui lòng tải lại và thử lại." };

        public override IdentityError PasswordMismatch()
            => new() { Code = nameof(PasswordMismatch), Description = "Mật khẩu không chính xác." };

        public override IdentityError InvalidToken()
            => new() { Code = nameof(InvalidToken), Description = "Token không hợp lệ." };

        public override IdentityError LoginAlreadyAssociated()
            => new() { Code = nameof(LoginAlreadyAssociated), Description = "Tài khoản đã được liên kết với người dùng khác." };

        public override IdentityError InvalidUserName(string userName)
            => new() { Code = nameof(InvalidUserName), Description = $"Tên người dùng '{userName}' không hợp lệ, chỉ được dùng chữ cái hoặc số." };

        public override IdentityError InvalidEmail(string email)
            => new() { Code = nameof(InvalidEmail), Description = $"Email '{email}' không hợp lệ." };

        public override IdentityError DuplicateUserName(string userName)
            => new() { Code = nameof(DuplicateUserName), Description = $"Tên người dùng '{userName}' đã tồn tại." };

        public override IdentityError DuplicateEmail(string email)
            => new() { Code = nameof(DuplicateEmail), Description = $"Email '{email}' đã được sử dụng." };

        public override IdentityError InvalidRoleName(string role)
            => new() { Code = nameof(InvalidRoleName), Description = $"Tên vai trò '{role}' không hợp lệ." };

        public override IdentityError DuplicateRoleName(string role)
            => new() { Code = nameof(DuplicateRoleName), Description = $"Tên vai trò '{role}' đã tồn tại." };

        public override IdentityError UserAlreadyHasPassword()
            => new() { Code = nameof(UserAlreadyHasPassword), Description = "Người dùng đã có mật khẩu." };

        public override IdentityError UserLockoutNotEnabled()
            => new() { Code = nameof(UserLockoutNotEnabled), Description = "Khóa tài khoản không được bật cho người dùng này." };

        public override IdentityError UserAlreadyInRole(string role)
            => new() { Code = nameof(UserAlreadyInRole), Description = $"Người dùng đã thuộc vai trò '{role}'." };

        public override IdentityError UserNotInRole(string role)
            => new() { Code = nameof(UserNotInRole), Description = $"Người dùng không thuộc vai trò '{role}'." };

        public override IdentityError PasswordTooShort(int length)
            => new() { Code = nameof(PasswordTooShort), Description = $"Mật khẩu phải có ít nhất {length} ký tự." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Mật khẩu phải chứa ít nhất một ký tự đặc biệt." };

        public override IdentityError PasswordRequiresDigit()
            => new() { Code = nameof(PasswordRequiresDigit), Description = "Mật khẩu phải chứa ít nhất một chữ số (0-9)." };

        public override IdentityError PasswordRequiresLower()
            => new() { Code = nameof(PasswordRequiresLower), Description = "Mật khẩu phải chứa ít nhất một chữ cái thường (a-z)." };

        public override IdentityError PasswordRequiresUpper()
            => new() { Code = nameof(PasswordRequiresUpper), Description = "Mật khẩu phải chứa ít nhất một chữ cái in hoa (A-Z)." };
    }
}
