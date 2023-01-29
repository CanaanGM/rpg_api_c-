using Microsoft.EntityFrameworkCore;

public class AuthRepo : IAuthRepo
{
    private DataContext _context;

    public AuthRepo(DataContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users.FirstOrDefaultAsync(x=> x.Username.ToLower() == username.ToLower());

        if (user is null){
            response.Sucess = false;
            response.Message = "user or password are wrong . . .";
            return response;
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Sucess = false;
            response.Message = "user or password are wrong . . .";
            return response;
        }

        response.Data = user.Id.ToString();
        return response;

    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        var response = new ServiceResponse<int>();
        if (await UserExists(user.Username) ) {
            response.Sucess = false;
            response.Message = "user already exists";
            return response;

        }
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        response.Data = user.Id;
        return response;
    }

    public async Task<bool> UserExists(string username)
    {
        if(await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            return true;
        return false;
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}