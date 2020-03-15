using System.Collections.Generic;
using System.Linq;
using eTaskAdvisor.WebApi.Data;
using eTaskAdvisor.WebApi.Models;

namespace eTaskAdvisor.WebApi.Helpers
{
  public static class ExtensionMethods
    {
        public static IEnumerable<Client> ForPublics(this IEnumerable<Client> users) {
            return users.Select(x => x.ForPublic());
        }

        public static Client ForPublic(this Client user)
        {
            user.Password = null;
            user.ClientId = "";
            return user;
        }

        public static Client WithPassword(this Client user)
        {
            user.ClientId = "";
            return user;
        }    
    }
}