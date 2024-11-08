namespace ProvisionPadel.Api.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly byte[] _key;

    public JwtService(IOptions<JwtSettings> jwtSettings,
                                UserManager<User> userManager,
                                RoleManager<Role> roleManager)
    {
        _jwtSettings = jwtSettings.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
        _roleManager = roleManager;
        _userManager = userManager;
    }

    private JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

    public async Task<string> GetJwtString(User user)
    {
        var claims = await GetClaims(user);
        var claimsIdentity = new ClaimsIdentity(claims);

        var token = CreateSecurityToken(claimsIdentity);

        return WriteToken(token);
    }

    private SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity)
    {
        var tokenDescriptor = GetTokenDescriptor(claimsIdentity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    private string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("IdentityId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var roleNames = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        return claims;
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddHours(2),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                                        SecurityAlgorithms.HmacSha256),
        };
    }
}
