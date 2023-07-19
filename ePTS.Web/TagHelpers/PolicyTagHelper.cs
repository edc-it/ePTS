using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Claims;

namespace ePTS.Web.TagHelpers
{
    /// <summary>
    /// TagHelper for checking authorization policies and removes tag if not authorized
    /// </summary>
    [HtmlTargetElement(Attributes = "policy")]
    public class PolicyTagHelper : TagHelper
    {
        private readonly IAuthorizationService _authService;
        private readonly ClaimsPrincipal _principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyTagHelper"/> class.
        /// </summary>
        /// <param name="authService">The <see cref="IAuthorizationService"/> to use.</param>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> to use.</param>
        public PolicyTagHelper(IAuthorizationService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _principal = httpContextAccessor.HttpContext!.User;
        }

        /// <summary>
        /// The policy to check for.
        /// </summary>
        [HtmlAttributeName("policy")]
        public string? Policy { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!(await _authService.AuthorizeAsync(_principal, Policy!)).Succeeded)
                output.SuppressOutput();
        }
    }
}
