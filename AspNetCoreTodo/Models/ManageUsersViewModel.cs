using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Models {
    public class ManageUsersViewModel {
        public IdentityUser[] Administrators { get; set; }
        public IdentityUser[] AllUsers { get; set; }

    }
}