using infrastructure.Services.Email;
using Microsoft.Extensions.Options;

namespace Presentation.OptionsSetup;

public class EtherealEmailOptionsSetup : IConfigureOptions<EtherealEmailOptions>
{
    public const string SectionName = "Ethereal";
    private readonly IConfiguration _configuration;

    public EtherealEmailOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EtherealEmailOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}