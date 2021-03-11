using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;

namespace PopOrgChart
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            var AdUsers = new List<ADUser>();
            DirectoryEntry entry = new DirectoryEntry("LDAP://PATH TO Users OU");
            DirectorySearcher DSearch = new DirectorySearcher(entry);
            DSearch.Filter = "(&(objectCategory=person)(!userAccountControl:1.2.840.113556.1.4.803:=2))";
            DSearch.PropertiesToLoad.Add("sn");
            DSearch.PropertiesToLoad.Add("title");
            DSearch.PropertiesToLoad.Add("givenName");
            DSearch.PropertiesToLoad.Add("Manager");
            DSearch.PropertiesToLoad.Add("mail");
            DSearch.PageSize = 2000;
            using (SearchResultCollection src = DSearch.FindAll())
            {
                foreach (SearchResult sr in src)
                {
                    var User = new ADUser
                    {
                        id = (100 + (i++)).ToString(),
                        name = "<b>" + GetProperty(sr, "givenName") + " " + GetProperty(sr, "sn") + "</b><br />" + GetProperty(sr, "title") + "<br />" + "<a href=mailto:" + GetProperty(sr, "mail") + ">Send Email</a>",
                        data = "{}",
                        Manager = ADUser.ConvertManager(GetProperty(sr, "Manager")),
                        FullName = GetProperty(sr, "givenName") + " " + GetProperty(sr, "sn")
                    };
                    AdUsers.Add(User);
                }
            }

            JSONGen JSONstr = new JSONGen();
            var SortedUsers = JSONstr.FlatToHierarchy(AdUsers);
            string json = JSONstr.ConvertoJSON(SortedUsers);
            char[] trimchars = { '[', ']' };
            json = json.TrimStart(trimchars);
            json = json.TrimEnd(trimchars);
            File.WriteAllText(@"<PATH TO WEB SERVER\json.json", json);
        }

        private static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
